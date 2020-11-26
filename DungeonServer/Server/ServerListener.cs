using DungeonServer.Server;
using DungeonUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DungeonServer
{
    public static class ServerListener
    {
        #region 雜項
        public static string GetMyIP()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            foreach (IPAddress ip in ips)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return "";
        }
        #endregion

        #region 伺服器：啟動、監聽、終止
        public static void StartServer()
        {
            ip = UI.tb_ServerIP.Text;
            port = UI.tb_ServerPort.Text;

            serverThread = new Thread(ServerLoop);
            serverThread.IsBackground = true;
            serverThread.Start();
            status = ServerStatus.Online;
        }

        private static void ServerLoop()
        {
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            svListener = new TcpListener(ipEP);
            svListener.Start(maxPlayers);

            while (true)
            {
                svSocket = svListener.AcceptSocket();
                clientThread = new Thread(Listen);
                clientThread.IsBackground = true;
                clientThread.Start();
            }
        }

        public static void StopServer()
        {
            string code = EnumEx<ServerMessageType>.GetOrderByEnum(ServerMessageType.Offline).ToString();
            SendAll(code);

            svListener.Stop();
            serverThread.Abort();
            if (clientThread != null) clientThread.Abort();

            status = ServerStatus.Offline;
        }
        #endregion

        #region 監聽客戶端訊息迴圈
        private static void Listen()
        {
            Socket sk = svSocket;
            Thread th = clientThread;

            while (true)
            {
                try
                {
                    byte[] data = new byte[1023];
                    int inLen = sk.Receive(data);
                    string msg = Encoding.Default.GetString(data, 0, inLen);
                    int cmdOrder = Convert.ToInt32(msg[0].ToString());
                    ServerMessageType cmd = EnumEx<ServerMessageType>.GetEnumByOrder(cmdOrder);
                    string str = msg.Substring(1);

                    switch (cmd)
                    {
                        case ServerMessageType.Offline:
                            PlayerOffline(str, th);
                            break;

                        case ServerMessageType.Verification:
                            string res = EnumEx<ServerMessageStatus>.GetOrderByEnum(players.ContainsKey(str) ? ServerMessageStatus.Fail
                                                                                                                     : ServerMessageStatus.Success).ToString();
                            SendTo(sk, cmdOrder.ToString() + res);
                            break;

                        case ServerMessageType.Online:
                            PlayerOnline(str, sk);
                            SendTo(str, cmdOrder.ToString() + players[str].dataPack);
                            break;

                        case ServerMessageType.Message:
                            SendTextToAll(cmdOrder, str);
                            break;

                        case ServerMessageType.Action:
                            // 更新玩家位置
                            break;

                        case ServerMessageType.Sync:
                            SyncPlayerData(str);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        // 由玩家池移除，保存紀錄，中斷連線
        private static void PlayerOffline(string name, Thread th)
        {
            players[name].Save();
            players.Remove(name);

            socketHT.Remove(name);

            UI.RemoveFromPlayerList(name);
            UI.AddLog(name + " offline.");

            th.Abort();
        }

        // 新增至玩家池，(新增)讀取紀錄
        private static void PlayerOnline(string name, Socket sk)
        {
            players.Add(name, new Character(name));
            players[name].Read();

            socketHT.Add(name, sk);

            UI.AddToPlayerList(name);
            UI.AddLog(name + " online.");
        }

        // 對所有玩家發送訊息
        private static void SendTextToAll(int cmdOrder, string message)
        {
            SendAll(cmdOrder.ToString() + message);

            UI.AddLog(message);
        }

        // 同步所有玩家資料，傳給所有玩家除了自己以外的資料
        // 格式 = 同步代碼,伺服器時間,玩家1名稱|玩家素質(由管線符號'|'分隔),玩家2名稱| ...
        private static void SyncPlayerData(string name)
        {
            string cmd = EnumEx<ServerMessageType>.GetOrderByEnum(ServerMessageType.Sync).ToString();
            string syncStr = cmd;
            foreach (var key in players.Keys)
            {
                if (key != name)
                    syncStr += "," + key + "|" + players[key].dataPack;
            }

            SendTo(name, syncStr);
        }
        #endregion

        #region 傳遞位元組資料
        private static void SendTo(string name, string str)
        {
            byte[] data = Encoding.Default.GetBytes(str);
            Socket sk = (Socket)socketHT[name];
            sk.Send(data, 0, data.Length, SocketFlags.None);
        }

        private static void SendTo(Socket sk, string str)
        {
            byte[] data = Encoding.Default.GetBytes(str);
            sk.Send(data, 0, data.Length, SocketFlags.None);
        }

        private static void SendAll(string str)
        {
            byte[] data = Encoding.Default.GetBytes(str);
            foreach (Socket s in socketHT.Values)
                s.Send(data, 0, data.Length, SocketFlags.None);
        }
        #endregion

        private static ServerStatus status = ServerStatus.Offline;
        public static bool isOnline => status == ServerStatus.Online;

        private const int maxPlayers = 5;
        private static string ip { get; set; }
        private static string port { get; set; }
        private static TcpListener svListener { get; set; }
        private static Socket svSocket { get; set; }
        private static Thread serverThread { get; set; }
        private static Thread clientThread { get; set; }

        // 所有連線清單，[玩家名稱 : 連線物件]
        private static Hashtable socketHT = new Hashtable();
        // 所有玩家清單，[玩家名稱 : 角色物件]
        private static Dictionary<string, Character> players = new Dictionary<string, Character>();
    }
}
