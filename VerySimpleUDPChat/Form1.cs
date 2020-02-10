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
		const string UserLeftGroup = "User left group";
		const string UserDisconnected = "User disconnected";
		const string RemoveChat = "Chat disconnected";
		const char Separator = '|';
		const string CommonChat = "CommonChat";
		int userPort = -1;
		const string Host = "235.5.5.1";
		private Dictionary<String, UdpClient> listenedPort = new Dictionary<String, UdpClient>();
		private HashSet<String> groupNames = new HashSet<string>();
		private HashSet<String> allGroups = new HashSet<string>();
		private HashSet<String> allUsers = new HashSet<string>();
		private List<ValueTuple<String, String>> allMessages = new List<ValueTuple<String, String>>();
		private int lastSelectedIndex = 0;

		public Form1()
		{
			InitializeComponent();
		}

		private void sendButton_Click(object sender, EventArgs e)
		{
			if (messageTextBox.Text.Length == 0 || chatListView.FocusedItem == null)
				return;
			string message = $"{Separator}from {userName}{Separator}: {messageTextBox.Text}";
			string chatName = chatListView.FocusedItem.Text;
			if (Send(MessageQuery, chatName + Separator + (allGroups.Contains(chatName) ? chatName : userName) + Separator + message))
			{
				chatTextBox.Text = messageTextBox.Text + "\r\n" + chatTextBox.Text;
				allMessages.Add((chatName, messageTextBox.Text));
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
			if (userName.Length == 0 || userName.Contains(Separator) || allUsers.Contains(userName))
			{
				MessageBox.Show("Bad name");
				return;
			}
			chatListView.Items.Add(CommonChat);
			inputTextBox.ReadOnly = true;
			inputButton.Enabled = false;

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

		private void ReceiveMessages(Func<bool> ProcessMessage)
		{
			try
			{
				while (ProcessMessage()) ;
			}
			catch (ObjectDisposedException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("WSACancelBlockingCall"))
					MessageBox.Show(ex.Message);
			}
		}

		private bool ProcessGroupMessage(string groupName)
		{
			IPEndPoint remoteIp = null;
			byte[] data = listenedPort[groupName].Receive(ref remoteIp);
			if (!listenedPort.ContainsKey(groupName))
				return false;
			string message = Encoding.Unicode.GetString(data);
			string header = message.Split(new char[] { Separator }, 2)[0];
			string body = message.Split(new char[] { Separator }, 2)[1];
			string sender = body.Split(new char[] { Separator }, 2)[0];
			string text = body.Split(new char[] { Separator }, 2)[1];
			if (text.Contains(Separator) && text.Split(new char[] { Separator }, 3)[1].Split(' ')[1] == userName)
				return true;
			Invoke(new MethodInvoker(() =>
			{
				if ((lastSelectedIndex == 0 || chatListView.Items[lastSelectedIndex].Text == sender))
					chatTextBox.Text = text + "\r\n" + chatTextBox.Text;
				allMessages.Add((sender, text));
			}));
			return true;
		}


		private bool ProcessUserMessage()
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
					listenedPort.Add(groupName, newClient);
					groupNames.Add(groupName);
					Task receiveTask = new Task(() => ReceiveMessages(() => ProcessGroupMessage(groupName)));

					Invoke(new MethodInvoker(() =>
					{
						for (int i = 0; i < chatListView.Items.Count; ++i)
							if (chatListView.Items[i].Text == groupName)
							{
								chatListView.Items[i].ForeColor = Color.Green;
							}
						UpdateChat();
					}));
					receiveTask.Start();
					allMessages.Add((groupName, $"User {userName} joined the group {groupName}"));
					Invoke(new MethodInvoker(() =>
					{
						chatTextBox.Text = $"User {userName} joined the group {groupName}" + "\r\n" + chatTextBox.Text;
					}));
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
				case MessageQuery:
					string sender = body.Split(new char[] { Separator }, 2)[0];
					string text = body.Split(new char[] { Separator }, 2)[1];
					Invoke(new MethodInvoker(() =>
					{
						if (lastSelectedIndex == 0 || chatListView.Items[lastSelectedIndex].Text == sender)
							chatTextBox.Text = text + "\r\n" + chatTextBox.Text;
						allMessages.Add((sender, text));
					}));
					break;
			}
			return true;

		}
		private bool ProcessServerMessage()
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
						return true;
					userPort = int.Parse(body);
					userClient = new UdpClient(userPort);
					userClient.JoinMulticastGroup(IPAddress.Parse(Host), 50);
					Task receiveTask = new Task(() => ReceiveMessages(ProcessUserMessage));
					receiveTask.Start();
					Send(UserConnected, userName + Separator + userPort);
					Invoke(new MethodInvoker(() => disconnectButton.Enabled = true));
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
				case RemoveChat:
					chatName = body.Split(new char[] { Separator }, 2)[0];
					type = body.Split(new char[] { Separator }, 2)[1];
					if (type == "group")
					{
						groupNames.Remove(chatName);
						allGroups.Remove(chatName);
					}
					else
						allUsers.Remove(chatName);
					Invoke(new MethodInvoker(() =>
					{
						int pos = -1;
						for (int i = 0; i < chatListView.Items.Count; ++i)
							if (chatListView.Items[i].Text == chatName)
							{
								pos = i;
								break;
							}
						if (pos != -1)
							chatListView.Items.RemoveAt(pos);
					}));
					break;
				default:
					break;
			}
			return true;
		}

		~Form1()
		{
			if (serverClient.Client.Connected)
				serverClient.Close();
			if (userClient.Client.Connected)
				userClient.Close();
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (chatListView.FocusedItem.Bounds.Contains(e.Location))
				{
					if (allGroups.Contains(chatListView.FocusedItem.Text))
					{
						if (groupNames.Contains(chatListView.FocusedItem.Text))
							contextMenuStrip2.Show(Cursor.Position);
						else
							contextMenuStrip1.Show(Cursor.Position);
					}
				}
			} 
			else if (e.Button == MouseButtons.Left && lastSelectedIndex != chatListView.FocusedItem.Index)
			{
				lastSelectedIndex = chatListView.FocusedItem.Index;
				UpdateChat();
			}
		}

		private void joinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Send(AddUserToGroupQuery, userName + Separator + chatListView.FocusedItem.Text);
		}
		private void leaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			groupNames.Remove(chatListView.FocusedItem.Text);
			listenedPort[chatListView.FocusedItem.Text].Close();
			listenedPort.Remove(chatListView.FocusedItem.Text);
			Invoke(new MethodInvoker(() =>
			{
				chatListView.FocusedItem.ForeColor = Color.Red;
				UpdateChat();
			}));
			Send(UserLeftGroup, userName + Separator + chatListView.FocusedItem.Text);
		}

		private void createGroupButton_Click(object sender, EventArgs e)
		{
			string groupName = groupNameTextBox.Text;
			if (groupName.Length == 0 || groupName.Contains(Separator) || allGroups.Contains(groupName))
			{
				MessageBox.Show("Bad name");
				return;
			}

			Send(GroupPortQuery, groupName + Separator + userName);
		}

		private void UpdateChats(string chatName, string type)
		{
			chatListView.Items.Add(chatName);
			chatListView.Items[chatListView.Items.Count - 1].ForeColor = type == "user" ? Color.Blue : Color.Red;
			allUsers.Add(chatName);
			if (type == "group")
				allGroups.Add(chatName);
		}

		private void UpdateChat()
		{
			if (chatListView.FocusedItem == null)
				return;
			sendButton.Enabled = lastSelectedIndex != 0 && (!allGroups.Contains(chatListView.FocusedItem.Text) || groupNames.Contains(chatListView.FocusedItem.Text));
			if (!allGroups.Contains(chatListView.FocusedItem.Text) || groupNames.Contains(chatListView.FocusedItem.Text))
				chatTextBox.Text = string.Join("\r\n", allMessages.FindAll(tuple => chatListView.FocusedItem.Text == CommonChat || tuple.Item1 == userName || tuple.Item1 == chatListView.FocusedItem.Text).Select(tuple => tuple.Item2).Reverse());
			else
				chatTextBox.Clear();
		}

		private void disconnectButton_Click(object sender, EventArgs e)
		{
			foreach(var group in groupNames)
			{
				listenedPort[group].Close();
				listenedPort.Remove(group);
				Send(UserLeftGroup, userName + Separator + group);
			}
			groupNames.Clear();
			allGroups.Clear();
			allUsers.Clear();
			allUsers.Clear();
			lastSelectedIndex = 0;
			userPort = -1;
			userClient.Close();

			disconnectButton.Enabled = false;
			inputButton.Enabled = true;
			inputTextBox.ReadOnly = false;

			label3.Visible = false;
			groupNameTextBox.Visible = false;
			groupNameTextBox.Enabled = false;
			createGroupButton.Visible = false;
			createGroupButton.Enabled = false;
			Invoke(new MethodInvoker(() =>
			{
				chatListView.Clear();
				chatTextBox.Clear();
			}));
			Send(UserDisconnected, userName);
			serverClient.Close();

		}
	}
}
