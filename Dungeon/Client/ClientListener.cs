﻿using DungeonGame.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DungeonGame
{
    public static class TCPClient
    {
        public static void SetServerIP(string inIP = "127.0.0.1") => ip = inIP;

        /// <summary>
        /// 查詢玩家
        /// </summary>
        /// <returns>Character物件</returns>
        public static Character GetPlayerInfo() => players[playerName];

        /// <summary>
        /// 查詢指定名稱玩家
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <returns>Character物件</returns>
        public static Character GetPlayerInfo(string name) => players[name];

        /// <summary>
        /// 登入伺服器
        /// <para>1. 初始化TCP監聽</para>
        /// <para>2. 傳送名稱驗證請求與玩家名稱至伺服器</para>
        /// <para>3. 等待回傳結果</para>
        /// <para>4. 登入成功則傳送登入請求與玩家名稱至伺服器</para>
        /// </summary>
        /// <param name="name"></param>
        public static void Login(string name)
        {
            svMsgStatus = ServerMessageStatus.None;
            isWaitingPlayerData = true;
            playerName = name;

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEndPoint);
                tcpThread = new Thread(Listen);
                tcpThread.IsBackground = true;
                tcpThread.Start();

                SendToServer(ServerMessageType.Verification, playerName);

                svMsgStatus = ServerMessageStatus.Waiting;
                while (svMsgStatus == ServerMessageStatus.Waiting) ;

                if (svMsgStatus == ServerMessageStatus.Success)
                {
                    status = OnlineStatus.online;
                    SendToServer(ServerMessageType.Online, playerName);
                }

                svMsgStatus = ServerMessageStatus.None;
            }
            catch { }
        }

        /// <summary>
        /// 對伺服器傳送資料
        /// </summary>
        /// <param name="type">伺服器請求的種類指令碼</param>
        /// <param name="inMsg">欲傳送之資料</param>
        private static void SendToServer(ServerMessageType type, string inMsg)
        {
            string msg = EnumExtensions<ServerMessageType>.GetOrderByEnum(type).ToString() + inMsg;

            byte[] data = Encoding.Default.GetBytes(msg);
            socket.Send(data, 0, data.Length, SocketFlags.None);
        }

        /// <summary>
        /// 玩家對所有在線玩家發送訊息
        /// </summary>
        /// <param name="msg">玩家發送的訊息</param>
        public static void SendMessage(string msg)
            => SendToServer(ServerMessageType.Message, playerName + " : " + msg);

        /// <summary>
        /// 監聽伺服器資料，會不斷執行
        /// </summary>
        private static void Listen()
        {
            EndPoint svEndpoint = socket.RemoteEndPoint;
            byte[] data = new byte[1023];
            int inLen = 0;
            string msg;
            int cmdOrder;
            ServerMessageType cmd;

            while (true)
            {
                try
                {
                    inLen = socket.ReceiveFrom(data, ref svEndpoint);
                }
                catch
                {
                    socket.Close();
                    tcpThread.Abort();
                }

                msg = Encoding.Default.GetString(data, 0, inLen);
                cmdOrder = Convert.ToInt32(msg[0].ToString());
                cmd = EnumExtensions<ServerMessageType>.GetEnumByOrder(cmdOrder);

                switch (cmd)
                {
                    case ServerMessageType.Offline:
                        ForceOffline();
                        break;

                    case ServerMessageType.Verification:
                        int res = Convert.ToInt32(msg.Substring(1));
                        svMsgStatus = EnumExtensions<ServerMessageStatus>.GetEnumByOrder(res);
                        break;

                    case ServerMessageType.Online:
                        LoadCharacterStatus(msg);
                        break;

                    case ServerMessageType.Message:
                        ReceiveTextMessage(msg);
                        break;

                    case ServerMessageType.Sync:
                        // 同步所有玩家資料
                        // Console.WriteLine(msg);

                        // 若不在字典中，新增key
                        // 更新玩家物件
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 伺服器離線，玩家自動下線
        /// </summary>
        private static void ForceOffline()
        {
            UI.Destroy();
            UI.Log("Server offline.");
        }

        /// <summary>
        /// 玩家上線接收自己的角色資料
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void LoadCharacterStatus(string rawData)
        {
            string[] datas = rawData.Substring(1).Split('|');
            Character c = new Character
            {
                name = playerName,
                health = Convert.ToUInt32(datas[0]),
                atk = Convert.ToInt32(datas[1]),
                def = Convert.ToInt32(datas[2]),
                coin = Convert.ToUInt32(datas[3])
            };
            players.Add(playerName, c);
            isWaitingPlayerData = false;
        }

        /// <summary>
        /// 接收訊息
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void ReceiveTextMessage(string rawData)
        {
            UI.Message(rawData.Substring(1));
        }

        /// <summary>
        /// 向伺服器請求其他玩家資料
        /// </summary>
        private static void RequestSyncPlayersData()
        {
            SendToServer(ServerMessageType.Sync, playerName);
        }

        /// <summary>
        /// 由伺服器回傳的資料進行介面更新，會不斷執行
        /// </summary>
        public static void UpdateUI()
        {
            RequestSyncPlayersData();

            players[playerName] = GetPlayerInfo();

            UI.tb_CharacterStatus.Text = players[playerName].status;
        }

        /// <summary>
        /// 傳送登出請求與玩家名稱
        /// </summary>
        public static void Logout()
        {
            try
            {
                SendToServer(ServerMessageType.Offline, playerName);
            }
            catch { }

            players.Remove(playerName);

            status = OnlineStatus.offline;
            socket.Close();
        }

        public static bool isOnline
        {
            get 
            {
                return status == OnlineStatus.online;
            }
        }

        private static ServerMessageStatus svMsgStatus = ServerMessageStatus.None;
        private static OnlineStatus status = OnlineStatus.offline;
        private static string playerName;
        private static string ip = "";
        private static int port = 8800;
        private static Socket socket;
        private static Thread tcpThread;
        public static bool isWaitingPlayerData = true;
        // 其他玩家清單，[玩家名稱 : 角色物件]
        private static Dictionary<string, Character> players = new Dictionary<string, Character>();
    }
}