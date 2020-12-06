using DungeonServer.Server;
using DungeonUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

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

            map = new Map();

            serverThread = new Thread(ServerLoop);
            serverThread.IsBackground = true;
            serverThread.Start();
            status = ServerStatus.Online;

            spawnTimer = new Timer();
            spawnTimer.Interval = 3000;
            spawnTimer.Tick += SpawnTimer_Tick;
            spawnTimer.Start();
        }

        private static void SpawnTimer_Tick(object sender, EventArgs e)
        {
            if (spawnedPickables.Count < 5 && players.Count != 0)
            {
                (int x, int y) spawnLoc = Map.GetRandomPointInPlayGround();
                Pickable p = new Pickable(Pickable.GetRandomItemNum(), spawnLoc);
                
                if (map.IsWalkable(p.rect))
                {
                    spawnedPickables.Add(p);

                    SendToAll(ServerMessageType.SpawnItem, p.ToString());
                }
            }
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
            try
            {
                SendToAll(ServerMessageType.Offline);

                spawnTimer.Stop();

                svListener.Stop();
                serverThread.Abort();
                if (clientThread != null) clientThread.Abort();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                status = ServerStatus.Offline;
                spawnedPickables.Clear();
            }
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
                    byte[] byteDatas = new byte[dataSize];
                    int inLen = sk.Receive(byteDatas);
                    string rawData = Encoding.Default.GetString(byteDatas, index: 0, inLen);
                    string[] datas = rawData.Split('>');
                    int cmdOrder = Convert.ToInt32(datas[0]);
                    ServerMessageType cmd = EnumEx<ServerMessageType>.GetEnumByOrder(cmdOrder);

                    switch (cmd)
                    {
                        case ServerMessageType.Offline:
                            PlayerOffline(datas[1], th);
                            break;

                        case ServerMessageType.Verification:
                            string res = EnumEx<ServerMessageStatus>.GetOrderByEnum(players.ContainsKey(datas[1]) ? ServerMessageStatus.Fail
                                                                                                                  : ServerMessageStatus.Success).ToString();
                            SendToSocket(ServerMessageType.Verification, sk, res);
                            break;

                        case ServerMessageType.Online:
                            PlayerOnline(playerName: datas[1], sk);

                            SendToPlayer(ServerMessageType.Online, playerName: datas[1], string.Format("{0: Player name}|{1: Data Pack With Item},{2: Floor Datas}",
                                                                                                       datas[1], players[datas[1]].dataPackWithItem, floorItemDatas));
                            break;

                        case ServerMessageType.TextMessage:
                            SendTextToAll(ServerMessageType.TextMessage, message: datas[1]);
                            break;

                        case ServerMessageType.Action:
                            string[] actionDatas = datas[1].Split('|');
                            UpdatePlayerLocation(name: actionDatas[0],
                                                 x: Convert.ToInt32(actionDatas[1]),
                                                 y: Convert.ToInt32(actionDatas[2]));
                            break;

                        case ServerMessageType.SyncPlayerData:
                            SyncAllPlayersData(name: datas[1]);
                            break;

                        case ServerMessageType.PickItem:
                            PlayerPickItem(ppiInfo: datas[1]);
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
        private static void PlayerOnline(string playerName, Socket sk)
        {
            players.Add(playerName, new Character(playerName));
            players[playerName].Read();

            socketHT.Add(playerName, sk);

            UI.AddToPlayerList(playerName);
            UI.AddLog(playerName + " online.");
        }

        // 對所有玩家發送訊息
        private static void SendTextToAll(ServerMessageType type, string message)
        {
            SendToAll(type, message);

            UI.AddLog(message);
        }

        // 更新玩家位置
        private static void UpdatePlayerLocation(string name, int x, int y)
            => players[name].UpdateLocation(x, y);

        // 同步所有玩家資料，傳給所有玩家除了自己以外的資料
        // 格式 = 同步代碼,其他玩家數,玩家1名稱|玩家素質(由管線符號'|'分隔),玩家2名稱| ...
        private static void SyncAllPlayersData(string name)
        {
            string syncStr = (players.Count - 1).ToString();
            foreach (var key in players.Keys)
            {
                if (key != name)
                    syncStr += "," + key + "|" + players[key].dataPackWithItem;
            }

            SendToPlayer(ServerMessageType.SyncPlayerData, name, syncStr);
        }

        // 玩家撿起物品
        private static void PlayerPickItem(string ppiInfo)
        {
            lock (spawnedPickables)
            {
                string[] infos = ppiInfo.Split(',');

                Pickable p = new Pickable(infos[1]);
                for (int i = 0; i < spawnedPickables.Count; i++)
                    if (spawnedPickables[i] == p)
                    {
                        if (spawnedPickables.Remove(spawnedPickables[i]))
                        {
                            foreach (string name in players.Keys)
                                SendToPlayer(ServerMessageType.PickItem, playerName: name, ((name != infos[0]) ? "-" : "") + p.ToString());

                            players[infos[0]].item = p.itemNum;
                        }
                        break;
                    }
            }
        }
        #endregion

        #region 傳遞位元組資料
        private static void SendToPlayer(ServerMessageType type, string playerName, string inMsg)
        {
            string code = EnumEx<ServerMessageType>.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            SendToClient((Socket)socketHT[playerName], byteDatas);
        }

        private static void SendToSocket(ServerMessageType type, Socket sk, string inMsg)
        {
            string code = EnumEx<ServerMessageType>.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            SendToClient(sk, byteDatas);
        }

        private static void SendToAll(ServerMessageType type, string inMsg = "")
        {
            string code = EnumEx<ServerMessageType>.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            foreach (Socket sk in socketHT.Values)
                SendToClient(sk, byteDatas);
        }

        private static void SendToClient(Socket sk, byte[] byteDatas) => sk.Send(byteDatas, 0, byteDatas.Length, SocketFlags.None);
        #endregion

        private static ServerStatus status = ServerStatus.Offline;
        public static bool isOnline => status == ServerStatus.Online;

        private const int dataSize = 0x3ff;
        private const int maxPlayers = 5;
        private static string ip { get; set; }
        private static string port { get; set; }
        private static TcpListener svListener { get; set; }
        private static Socket svSocket { get; set; }
        private static Thread serverThread { get; set; }
        private static Thread clientThread { get; set; }
        private static Timer spawnTimer { get; set; }
        private static Map map;

        // 所有連線清單，[玩家名稱 : 連線物件]
        private static Hashtable socketHT = new Hashtable();
        // 所有玩家清單，[玩家名稱 : 角色物件]
        private static Dictionary<string, Character> players = new Dictionary<string, Character>();
        private static List<Pickable> spawnedPickables = new List<Pickable>();
        private static string floorItemDatas
        {
            get
            {
                List<string> datas = new List<string>();

                foreach (Pickable p in spawnedPickables)
                    datas.Add(p.ToString());

                return string.Join("|", datas);
            }
        }
    }
}
