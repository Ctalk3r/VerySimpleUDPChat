using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
	class Program
	{
		public static void Main()
		{
			Server server = new Server();
			new Thread(server.Start).Start();
		}
	}

	class Server
	{
		private UdpClient client;
		const int LocalPort = 8002;
		const int RemotePort = 8001;
		int lastFreePort = LocalPort + 1;
		const string UserPortQuery = "Get free port for user";
		const string GroupPortQuery = "Get free port for group";
		const string AddedNewChat = "New chat connected";
		const string UserConnected = "User connected";
		const string GetAllChatsQuery = "Get all chat's list";
		const string MessageQuery = "Send message to certain chat";
		const string AddUserToGroupQuery = "Add user to group";
		const string Host = "235.5.5.1";
		const char Separator = '|';
		Dictionary<string, int> namePortsMapping;
		private HashSet<String> allGroupes;

		public Server()
		{
			client = new UdpClient();
			namePortsMapping = new Dictionary<string, int>();
			allGroupes = new HashSet<string>();
		}
		public void Send(string header, string body, int port = RemotePort, string to = "all")
		{
			Console.WriteLine($"Server sends to {to}: {header} - {body}");
			Byte[] buffer = Encoding.Unicode.GetBytes(header + Separator + body);
			client.Send(buffer, buffer.Length, Host, port);
		}

		public void Start()
		{
			client = new UdpClient();
			IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, LocalPort);
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			client.ExclusiveAddressUse = false;
			client.Client.Bind(localPoint);
			client.JoinMulticastGroup(IPAddress.Parse(Host), 50);
			Console.WriteLine($"Server started");
			while (true)
			{
				Byte[] data = client.Receive(ref localPoint);
				string query = Encoding.Unicode.GetString(data);
				string header = query.Split(Separator, 2)[0];
				string body = query.Split(Separator, 2)[1];
				switch (header)
				{
					case UserPortQuery:
						Console.WriteLine($"User {body} is connecting to port {lastFreePort++}");
						Send(UserPortQuery, (lastFreePort - 1).ToString());
						break;
					case GroupPortQuery:
						string groupName = body.Split(Separator, 2)[0];
						string owner = body.Split(Separator, 2)[1];
						namePortsMapping.Add(groupName, lastFreePort++);
						Console.WriteLine($"Group {groupName} is connected to port {lastFreePort - 1}");
						Send(AddedNewChat, groupName + Separator + "group");
						Send(AddUserToGroupQuery, (lastFreePort - 1).ToString() + Separator + groupName, namePortsMapping[owner], owner);
						allGroupes.Add(groupName);
						break;
					case AddUserToGroupQuery:
						groupName = body.Split(Separator, 2)[1];
						string memberName = body.Split(Separator, 2)[0];
						Send(MessageQuery, $"{memberName} joined the group {groupName}", namePortsMapping[groupName], groupName);
						Send(AddUserToGroupQuery, namePortsMapping[groupName].ToString() + Separator + groupName, namePortsMapping[memberName], memberName);
						break;
					case MessageQuery:
						string text = body.Split(Separator, 2)[1];
						string receiver = body.Split(Separator, 2)[0];
						Send(MessageQuery, text, namePortsMapping[receiver], receiver);
						break;
					case UserConnected:
						string userName = body.Split(Separator, 2)[0];
						int port = int.Parse(body.Split(Separator, 2)[1]);
						Console.WriteLine($"User {userName} is connected to port {port}");
						List<string> response = new List<String>();
						foreach (var name in namePortsMapping.Keys)
						{
							response.Add(name);
							response.Add(allGroupes.Contains(name) ? "group" : "user");
						}
						Send(GetAllChatsQuery, string.Join(Separator, response), port, userName);
						namePortsMapping.Add(userName, port);
						Send(AddedNewChat, userName + Separator + "user");
						break;
					default:
						Console.WriteLine($"Server gets: {query}");
						break;
				}
			}
		}

		~Server()
		{
			client.Close();
		}
	}
}
