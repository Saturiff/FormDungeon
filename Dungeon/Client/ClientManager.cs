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
            svMsgStatus = ClientStatus.None;
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

                SendToServer(ClientMessageType.Verification, playerName);

                svMsgStatus = ClientStatus.Waiting;
                while (svMsgStatus == ClientStatus.Waiting) ;

                if (svMsgStatus == ClientStatus.Success)
                {
                    status = OnlineStatus.Online;
                    SendToServer(ClientMessageType.Online, playerName);
                }

                svMsgStatus = ClientStatus.None;
            }
            catch { }
        }

        /// <summary>
        /// 玩家移動後對伺服器傳送玩家的新位置
        /// </summary>
        public static void UpdatePlayerLocation()
        {
            SendToServer(ClientMessageType.Action, string.Format("{0}|{1}|{2}",
                playerName, UI.player.Location.X, UI.player.Location.Y));
        }

        /// <summary>
        /// 玩家對所有在線玩家發送訊息
        /// </summary>
        /// <param name="msg">玩家發送的訊息</param>
        public static void SendMessage(string msg)
            => SendToServer(ClientMessageType.TextMessage, playerName + " : " + msg);

        /// <summary>
        /// 由伺服器回傳的資料進行介面更新，會不斷執行
        /// </summary>
        public static void UpdateUI()
        {
            RequestPlayersData();

            players[playerName] = GetPlayerCharacter();
            playerUpdateStatus[playerName] = true;

            UI.tb_CharacterStatus.Text = players[playerName].status;
            if(UI.focusEnemyName != "" && UI.focusEnemyName != null)
                UI.tb_EnemyStatus.Text = players[UI.focusEnemyName].status;
        }

        /// <summary>
        /// 向伺服器請求玩家狀態
        /// </summary>
        private static void RequestPlayersData()
            => SendToServer(ClientMessageType.SyncPlayerData, playerName);

        /// <summary>
        /// 查詢玩家
        /// </summary>
        /// <returns>Character物件</returns>
        public static Player GetPlayerCharacter() => players[playerName];

        /// <summary>
        /// 查詢指定名稱玩家
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <returns>Character物件</returns>
        public static Player GetPlayerCharacter(string name) => players[name];

        /// <summary>
        /// 傳送登出請求與玩家名稱
        /// </summary>
        public static void Logout()
        {
            try
            {
                SendToServer(ClientMessageType.Offline, playerName);
            }
            catch { }

            foreach (Player c in players.Values)
            {
                UI.DestroyFromViewport(c);
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
        private static void SendToServer(ClientMessageType type, string inMsg)
        {
            string msg = EnumEx<ClientMessageType>.GetOrderByEnum(type).ToString() + ">" + inMsg;

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
            string[] datas;
            int cmdOrder;
            ClientMessageType cmd;

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
                datas = rawData.Split('>');
                cmdOrder = Convert.ToInt32(datas[0]);
                cmd = EnumEx<ClientMessageType>.GetEnumByOrder(cmdOrder);
                
                switch (cmd)
                {
                    case ClientMessageType.Offline:
                        ForceOffline();
                        break;

                    case ClientMessageType.Verification:
                        ContinueVerification(datas[1]);
                        break;

                    case ClientMessageType.Online:
                        LoadPlayerCharacterStatus(datas[1]);
                        break;

                    case ClientMessageType.TextMessage:
                        ReceiveTextMessage(datas[1]);
                        break;

                    case ClientMessageType.SyncPlayerData:
                        SyncAllPlayersData(datas[1]);
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
            UI.AddLog("Server offline.");
        }

        /// <summary>
        /// 伺服器端驗證角色名稱
        /// </summary>
        private static void ContinueVerification(string result)
        {
            int resultIdx = Convert.ToInt32(result);
            svMsgStatus = EnumEx<ClientStatus>.GetEnumByOrder(resultIdx);
        }

        /// <summary>
        /// 玩家上線接收自己的角色資料
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void LoadPlayerCharacterStatus(string dataPack)
        {
            Player c = new Player(dataPack);
            players.Add(playerName, c);
            isWaitingPlayerData = false;
        }

        /// <summary>
        /// 由伺服器傳來的資料做玩家資料的分割整理
        /// 格式: other_player_count,name|{datapack},name|{datapack}, ...
        /// </summary>
        /// <param name="dataPacks">伺服器傳來的原始資料</param>
        /// <returns></returns>
        private static IEnumerable<string> ExtractDataPack(string dataPacks)
        {
            string[] datas = dataPacks.Split(',');

            int otherPlayerNum = Convert.ToInt32(datas[0]);

            if (otherPlayerNum < 1)
                yield break;

            for (int i = 0; i < otherPlayerNum; i++)
                yield return datas[i + 1];
        }

        /// <summary>
        /// 接收文字訊息
        /// </summary>
        /// <param name="rawData">伺服器傳來的原始資料</param>
        private static void ReceiveTextMessage(string textMessage) => UI.AddTextMessage(textMessage);

        /// <summary>
        /// 同步所有其他玩家狀態資料，並在有玩家登出時移除該玩家
        /// </summary>
        /// <param name="playerDatas">伺服器傳來的原始資料</param>
        private static void SyncAllPlayersData(string playerDatas)
        {
            // 初始化判斷離線的參數
            foreach (string name in playerUpdateStatus.Keys.ToList())
                if (name != playerName)
                    playerUpdateStatus[name] = false;

            // 更新/新增在線玩家資料
            foreach (string dataPack in ExtractDataPack(playerDatas))
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

                    UI.SpawnInViewport(players[c.name]);

                    playerUpdateStatus.Add(c.name, true);
                }
            }

            // 清除離線玩家角色
            foreach (string name in playerUpdateStatus.Keys.ToList())
                if (!playerUpdateStatus[name])
                {
                    UI.DestroyFromViewport(players[name]);

                    playerUpdateStatus.Remove(name);

                    players.Remove(name);

                    if (UI.focusEnemyName == name)
                    {
                        UI.focusEnemyName = "";
                        UI.tb_EnemyStatus.Text = "";
                    }
                }
        }
        #endregion

        public static bool isOnline => status == OnlineStatus.Online;

        private static string playerName { get; set; }
        private static string ip { get; set; }
        private const int port = 8800;
        private const int dataSize = 0x3ff;
        private static ClientStatus svMsgStatus = ClientStatus.None;
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
