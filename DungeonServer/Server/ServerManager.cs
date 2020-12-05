﻿using DungeonServer.Server;
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

            serverThread = new Thread(ServerLoop);
            serverThread.IsBackground = true;
            serverThread.Start();
            status = ServerStatus.Online;

            spawnTimer = new Timer();
            spawnTimer.Interval = 5000;
            spawnTimer.Tick += SpawnTimer_Tick;
            spawnTimer.Start();
        }

        private static void SpawnTimer_Tick(object sender, EventArgs e)
        {
            (int x, int y) spawnLoc = Rand.GetRandPointInRect(playGround);
            //SendAll(code + ">" + spawnLoc.x + "|" + spawnLoc.y);
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
                            SendToPlayer(ServerMessageType.Online, playerName: datas[1], datas[1] + "|" + players[datas[1]].dataPack);
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
                    syncStr += "," + key + "|" + players[key].dataPack;
            }

            SendToPlayer(ServerMessageType.SyncPlayerData, name, syncStr);
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
        private static Rect playGround = new Rect()
        {
            x0y0 = (0, 0),
            x0y1 = (0, 440),
            x1y0 = (800, 0),
            x1y1 = (800, 440)
        };

        // 所有連線清單，[玩家名稱 : 連線物件]
        private static Hashtable socketHT = new Hashtable();
        // 所有玩家清單，[玩家名稱 : 角色物件]
        private static Dictionary<string, Character> players = new Dictionary<string, Character>();
    }
}
