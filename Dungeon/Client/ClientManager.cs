using DungeonGame.Client;
using DungeonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DungeonGame
{
    /// <summary>
    /// 對遊戲伺服器進行傳送資料與接收資料
    /// </summary>
    public static class ClientManager
    {
        #region 初始化
        public static void SetServerIP(string inIP = "127.0.0.1") => ip = inIP;
        #endregion

        #region 傳送資料
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
                    status = OnlineStatus.Online;
                    SendToServer(ServerMessageType.Online, playerName);
                }

                svMsgStatus = ServerMessageStatus.None;
            }
            catch { }
        }

        /// <summary>
        /// 玩家移動後對伺服器傳送玩家的新位置
        /// </summary>
        public static void UpdatePlayerLocation()
        {
            SendToServer(ServerMessageType.Action, string.Format("{0}|{1}|{2}",
                playerName, UI.player.Location.X, UI.player.Location.Y));
        }

        /// <summary>
        /// 玩家對目標造成傷害
        /// </summary>
        /// <param name="name">目標名稱</param>
        /// <param name="damage">傷害值</param>
        public static void Hit(string name, int damage)
        {
            SendToServer(ServerMessageType.Hit, name + "|" + damage.ToString());
        }

        /// <summary>
        /// 玩家對所有在線玩家發送訊息
        /// </summary>
        /// <param name="msg">玩家發送的訊息</param>
        public static void SendMessage(string msg)
            => SendToServer(ServerMessageType.TextMessage, playerName + " : " + msg);

        /// <summary>
        /// 由伺服器回傳的資料進行介面更新，會不斷執行
        /// </summary>
        public static void UpdateUI()
        {
            RequestSyncPlayersData();

            players[playerName] = GetPlayerInfo();
            playerUpdateStatus[playerName] = true;

            UI.tb_CharacterStatus.Text = players[playerName].status;
        }

        /// <summary>
        /// 向伺服器請求其他玩家狀態資料
        /// </summary>
        private static void RequestSyncPlayersData()
            => SendToServer(ServerMessageType.SyncPlayerData, playerName);

        /// <summary>
        /// 向伺服器請求特定玩家物品欄資料
        /// </summary>
        /// <param name="targetName"></param>
        public static void RequestCharacterItem()
            => SendToServer(ServerMessageType.RequestCharacterItem, playerName);

        public static void RequestPickItem(string targetName, int slotIdx)
            => SendToServer(ServerMessageType.RequestPickItem, playerName + "|" + targetName + "|" + slotIdx.ToString());

        public static void RequestDropItem(int slotIdx)
            => SendToServer(ServerMessageType.RequestDropItem, playerName + "|" + slotIdx);

        /// <summary>
        /// 查詢玩家
        /// </summary>
        /// <returns>Character物件</returns>
        public static Player GetPlayerInfo() => players[playerName];

        /// <summary>
        /// 查詢指定名稱玩家
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <returns>Character物件</returns>
        public static Player GetPlayerInfo(string name) => players[name];

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

            foreach (Player c in players.Values)
            {
                UI.DestroyCharacter(c);
            }

            players.Clear();
            playerUpdateStatus.Clear();

            status = OnlineStatus.Offline;
            socket.Close();
        }

        /// <summary>
        /// 對伺服器傳送資料
        /// </summary>
        /// <param name="type">伺服器請求的種類指令碼</param>
        /// <param name="inMsg">欲傳送之資料</param>
        private static void SendToServer(ServerMessageType type, string inMsg)
        {
            string msg = EnumEx<ServerMessageType>.GetOrderByEnum(type).ToString() + inMsg;

            byte[] data = Encoding.Default.GetBytes(msg);
            socket.Send(data, 0, data.Length, SocketFlags.None);
        }
        #endregion

        #region 處理接收的資料
        /// <summary>
        /// 監聽伺服器資料，會不斷執行
        /// </summary>
        private static void Listen()
        {
            EndPoint svEndpoint = socket.RemoteEndPoint;
            byte[] byteDatas = new byte[dataSize];
            int inLen = 0;
            string rawData;
            int cmdOrder;
            ServerMessageType cmd;

            while (true)
            {
                try
                {
                    inLen = socket.ReceiveFrom(byteDatas, ref svEndpoint);
                }
                catch
                {
                    socket.Close();
                    tcpThread.Abort();
                }

                rawData = Encoding.Default.GetString(byteDatas, 0, inLen);
                cmdOrder = Convert.ToInt32(rawData[0].ToString());
                cmd = EnumEx<ServerMessageType>.GetEnumByOrder(cmdOrder);

                switch (cmd)
                {
                    case ServerMessageType.Offline:
                        ForceOffline();
                        break;

                    case ServerMessageType.Verification:
                        ContinueVerification(rawData);
                        break;

                    case ServerMessageType.Online:
                        LoadPlayerCharacterStatus(rawData);
                        break;

                    case ServerMessageType.TextMessage:
                        ReceiveTextMessage(rawData);
                        break;

                    case ServerMessageType.SyncPlayerData:
                        SyncAllPlayersData(rawData);
                        break;

                    case ServerMessageType.RequestCharacterItem:
                        SyncPlayerItem(rawData);
                        break;

                    default:
                        Console.WriteLine("bad data: " + cmdOrder);
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
        /// 伺服器端驗證角色名稱
        /// </summary>
        private static void ContinueVerification(string rawData)
        {
            int resultIdx = Convert.ToInt32(rawData.Substring(1));
            svMsgStatus = EnumEx<ServerMessageStatus>.GetEnumByOrder(resultIdx);
        }

        /// <summary>
        /// 玩家上線接收自己的角色資料
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void LoadPlayerCharacterStatus(string rawData)
        {
            string dataPack = rawData.Substring(1);
            Player c = new Player(dataPack);
            players.Add(playerName, c);
            isWaitingPlayerData = false;
        }

        /// <summary>
        /// 由伺服器傳來的資料做玩家資料的分割整理
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        /// <returns></returns>
        private static IEnumerable<string> ExtractDataPack(string rawData)
        {
            string[] datas = rawData.Split(',');

            int otherPlayerNum = Convert.ToInt32(datas[1]);

            if (otherPlayerNum < 1)
                yield break;

            for (int i = 0; i < otherPlayerNum; i++)
                yield return datas[i + 2];
        }

        /// <summary>
        /// 接收文字訊息
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void ReceiveTextMessage(string rawData) => UI.Message(rawData.Substring(1));

        /// <summary>
        /// 同步所有其他玩家狀態資料，並在有玩家登出時移除該玩家
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void SyncAllPlayersData(string rawData)
        {
            // 初始化判斷離線的參數
            foreach (string name in playerUpdateStatus.Keys.ToList())
                if (name != playerName)
                    playerUpdateStatus[name] = false;

            // 更新/新增在線玩家資料
            foreach (string dataPack in ExtractDataPack(rawData))
            {
                string name = dataPack.Split('|')[0];

                if (players.ContainsKey(name))
                {
                    players[name].UpdateByDataPack(dataPack);

                    playerUpdateStatus[name] = true;
                }
                else
                {
                    Player c = new Player(dataPack);

                    players.Add(c.name, c);

                    UI.SpawnCharacter(players[c.name]);

                    playerUpdateStatus.Add(c.name, true);
                }
            }

            // 清除離線玩家角色
            foreach (string name in playerUpdateStatus.Keys.ToList())
                if (!playerUpdateStatus[name])
                {
                    UI.DestroyCharacter(players[name]);

                    playerUpdateStatus.Remove(name);

                    players.Remove(name);
                }
        }

        /// <summary>
        /// 更新特定玩家物品欄資料
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void SyncPlayerItem(string rawData)
        {
            string itemPack = rawData.Substring(1);
            
            UI.inventory.Update(itemPack);
        }
        #endregion

        public static bool isOnline => status == OnlineStatus.Online;

        private static string playerName { get; set; }
        private static string ip { get; set; }
        private const int port = 8800;
        private const int dataSize = 0x3ff;
        private static ServerMessageStatus svMsgStatus = ServerMessageStatus.None;
        private static OnlineStatus status = OnlineStatus.Offline;
        private static Socket socket { get; set; }
        private static Thread tcpThread { get; set; }

        // 是否在等待伺服器回傳玩家狀態資料
        public static bool isWaitingPlayerData = true;
        // 線上玩家清單，[玩家名稱 : 角色物件]
        private static Dictionary<string, Player> players = new Dictionary<string, Player>();
        // 玩家更新狀態，若同步資料後該玩家沒更新過，則會移除該玩家，[玩家名稱 : 是否更新過]
        private static Dictionary<string, bool> playerUpdateStatus = new Dictionary<string, bool>();
    }
}
