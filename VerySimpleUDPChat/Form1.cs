using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VerySimpleUDPChat
{
	public partial class Form1 : Form
	{
		private string userName;
		private UdpClient serverClient;
		private UdpClient userClient;
		const int LocalPort = 8001;
		const int RemotePort = 8002;
		const string UserPortQuery = "Get free port for user";
		const string GroupPortQuery = "Get free port for group";
		const string AddedNewChat = "New chat connected";
		const string UserConnected = "User connected";
		const string GetAllChatsQuery = "Get all chat's list";
		const string MessageQuery = "Send message to certain chat";
		const string AddUserToGroupQuery = "Add user to group";
		const char Separator = '|';
		int userPort = -1;
		const string Host = "235.5.5.1";
		private Dictionary<int, UdpClient> listenedPort = new Dictionary<int, UdpClient>();
		private HashSet<String> groupNames = new HashSet<string>();
		private HashSet<String> allGroupes = new HashSet<string>();

		public Form1()
		{
			InitializeComponent();

		}

		private void sendButton_Click(object sender, EventArgs e)
		{
			if (messageTextBox.Text.Length == 0 || chatComboBox.SelectedIndex < 0)
				return;
			string message = $"(from {userName}): {messageTextBox.Text}";
			if(Send(MessageQuery, chatComboBox.SelectedItem.ToString() + Separator + message))
			{
				chatTextBox.Text = messageTextBox.Text + "\r\n" + chatTextBox.Text;
				messageTextBox.Clear();
			}
		}
		public bool Send(string header, string body)
		{
			try
			{
				Byte[] buffer = Encoding.Unicode.GetBytes(header + Separator + body);
				serverClient.Send(buffer, buffer.Length, Host, RemotePort);
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}
		private void inputButton_Click(object sender, EventArgs e)
		{
			userName = inputTextBox.Text;
			if (userName.Length == 0 || userName.Contains(Separator) || chatComboBox.Items.Contains(userName))
			{
				MessageBox.Show("Bad name");
				return;
			}
			inputTextBox.ReadOnly = true;
			inputButton.Enabled = false;
			sendButton.Enabled = true;

			label3.Visible = true;
			groupNameTextBox.Enabled = true;
			groupNameTextBox.Visible = true;
			createGroupButton.Enabled = true;
			createGroupButton.Visible = true;

			try
			{
				serverClient = new UdpClient();
				IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, LocalPort);
				serverClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				serverClient.ExclusiveAddressUse = false;
				serverClient.Client.Bind(localPoint);
				serverClient.JoinMulticastGroup(IPAddress.Parse(Host), 50);
				Task receiveTask = new Task(() => ReceiveMessages(ProcessServerMessage));
				receiveTask.Start();
				Send(UserPortQuery, userName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ReceiveMessages(Action ProcessMessage)
		{
			try
			{
				while (true)
					ProcessMessage();
			}
			catch (ObjectDisposedException)
			{
				throw;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ProcessGroupMessage(int port)
		{
			IPEndPoint remoteIp = null;
			byte[] data = listenedPort[port].Receive(ref remoteIp);
			string message = Encoding.Unicode.GetString(data);
			string header = message.Split(new char[] { Separator }, 2)[0];
			string body = message.Split(new char[] { Separator }, 2)[1];
			Invoke(new MethodInvoker(() =>
			{
				chatTextBox.Text = body + "\r\n" + chatTextBox.Text;
			}));
		}


		private void ProcessUserMessage()
		{
			IPEndPoint remoteIp = null;
			byte[] data = userClient.Receive(ref remoteIp);
			string message = Encoding.Unicode.GetString(data);
			string header = message.Split(new char[] { Separator }, 2)[0];
			string body = message.Split(new char[] { Separator }, 2)[1];
			switch (header)
			{
				case AddUserToGroupQuery:
					int port = int.Parse(body.Split(new char[] { Separator }, 2)[0]);
					string groupName = body.Split(new char[] { Separator }, 2)[1];
					UdpClient newClient = new UdpClient();
					IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, port);
					newClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					newClient.ExclusiveAddressUse = false;
					newClient.Client.Bind(localPoint);
					newClient.JoinMulticastGroup(IPAddress.Parse(Host), 50);
					listenedPort.Add(port, newClient);
					groupNames.Add(groupName);
					Task receiveTask = new Task(() => ReceiveMessages(() => ProcessGroupMessage(port)));

					Invoke(new MethodInvoker(() =>
					{
						for (int i = 0; i < chatListView.Items.Count; ++i)
							if (chatListView.Items[i].Text == groupName)
							{
								chatListView.Items[i].ForeColor = Color.Green;
							}
					}));
					chatComboBox.Items.Add(groupName);
					receiveTask.Start();
					break;
				case GetAllChatsQuery:
					string[] chats = body.Split(Separator);
					if (!chats[0].Equals(""))	
						Invoke(new MethodInvoker(() =>
						{
							for(int i = 0; i < chats.Count(); i += 2)
							{
								UpdateChats(chats[i], chats[i + 1]);
							}
						}));
					break;
				default:
					Invoke(new MethodInvoker(() =>
					{
						chatTextBox.Text = body + "\r\n" + chatTextBox.Text;
					}));
					break;
			}
		}
		private void ProcessServerMessage()
		{
			IPEndPoint remoteIp = null;
			byte[] data = serverClient.Receive(ref remoteIp);
			string message = Encoding.Unicode.GetString(data);
			string header = message.Split(new char[] { Separator }, 2)[0];
			string body = message.Split(new char[] { Separator }, 2)[1];
			switch (header)
			{
				case UserPortQuery:
					if (userPort != -1)
						return;
					userPort = int.Parse(body);
					userClient = new UdpClient(userPort);
					userClient.JoinMulticastGroup(IPAddress.Parse(Host), 50);
					Task receiveTask = new Task(() => ReceiveMessages(ProcessUserMessage));
					receiveTask.Start();
					Send(UserConnected, userName + Separator + userPort);
					break;
				case AddedNewChat:
					string chatName = body.Split(new char[] { Separator }, 2)[0];
					string type = body.Split(new char[] { Separator }, 2)[1];
					if (chatName != userName)
						Invoke(new MethodInvoker(() =>
						{
							UpdateChats(chatName, type);
						}));
					break;
				default:
					break;
			}
		}

		~Form1()
		{
			serverClient.Close();
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (chatListView.FocusedItem.Bounds.Contains(e.Location))
				{
					if (allGroupes.Contains(chatListView.FocusedItem.Text))
					{
						if (groupNames.Contains(chatListView.FocusedItem.Text))
							contextMenuStrip2.Show(Cursor.Position);
						else
							contextMenuStrip1.Show(Cursor.Position);
					}
				}
			}
		}

		private void joinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Send(AddUserToGroupQuery, userName + Separator + chatListView.FocusedItem.Text);
		}
		private void leaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(chatListView.FocusedItem.Text);
		}

		private void createGroupButton_Click(object sender, EventArgs e)
		{
			string groupName = groupNameTextBox.Text;
			if (groupName.Length == 0 || groupName.Contains(Separator) || allGroupes.Contains(groupName))
			{
				MessageBox.Show("Bad name");
				return;
			}

			Send(GroupPortQuery, groupName + Separator + userName);
			groupNameTextBox.Clear();
		}

		private void UpdateChats(string chatName, string type)
		{
			chatListView.Items.Add(chatName);
			chatListView.Items[chatListView.Items.Count - 1].ForeColor = type == "user" ? Color.Blue : Color.Red;
			if (type == "group")
				allGroupes.Add(chatName);
			else
				chatComboBox.Items.Add(chatName);
		}
	}
}
