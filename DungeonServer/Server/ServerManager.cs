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
    /// <summary>
    /// 對客戶端進行接收、傳送資料，根據遊戲性做資料處理
    /// </summary>
    public class ServerManager
    {
        #region 伺服器：啟動、監聽、終止
        /// <summary>
        /// 啟用伺服器，開始接收客戶端資料，初始化地圖物件隨機生成器
        /// </summary>
        public void StartServer()
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

        /// <summary>
        /// 隨機生成地圖帶有Item類的Pickable類，3秒執行一次
        /// </summary>
        /// <param name="sender">Timer物件</param>
        /// <param name="e">EventArgs參數</param>
        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            if (spawnedPickables.Count < 5 && players.Count != 0)
            {
                Pickable p;
                bool isSpawnSuccess;

                do
                {
                    isSpawnSuccess = true;

                    (int x, int y) spawnLoc = map.GetRandomFitPointInPlayGround(Pickable.size.w, Pickable.size.h);

                    p = new Pickable(Pickable.GetRandomItemNum(), spawnLoc);

                    foreach (Pickable _p in spawnedPickables)
                        if (p.Rect.IsOverlapped(_p.Rect))
                        {
                            isSpawnSuccess = false;
                            break;
                        }
                }
                while (!isSpawnSuccess);

                spawnedPickables.Add(p);

                SendToAll(ServerMessageType.SpawnItem, p.ToString());
            }
        }

        /// <summary>
        /// 處理Socket類連線
        /// </summary>
        private void ServerLoop()
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

        /// <summary>
        /// 終止伺服器，告知所有玩家伺服器下線消息，釋放資源
        /// </summary>
        public void StopServer()
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

                GC.Collect();
            }
        }
        #endregion

        #region 傳送資料
        /// <summary>
        /// 重生後傳遞給玩家重生資訊
        /// </summary>
        /// <param name="name">玩家名稱</param>
        public void Respawn(string name)
            => SendToPlayer(ServerMessageType.Respawn, name, players[name].RespawnDataPack);
        #endregion

        #region 監聽客戶端訊息迴圈
        /// <summary>
        /// 監聽與處理客戶端傳來的所有資料
        /// </summary>
        private void Listen()
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
                    ServerMessageType cmd = EnumEx.GetEnumByOrder<ServerMessageType>(cmdOrder);

                    switch (cmd)
                    {
                        case ServerMessageType.Offline:
                            PlayerOffline(datas[1], th);
                            break;

                        case ServerMessageType.Verification:
                            string res = EnumEx.GetOrderByEnum(players.ContainsKey(datas[1]) ? ServerMessageStatus.Fail
                                                                                             : ServerMessageStatus.Success).ToString();
                            SendToSocket(ServerMessageType.Verification, sk, res);
                            break;

                        case ServerMessageType.Online:
                            PlayerOnline(name: datas[1], sk);
                            break;

                        case ServerMessageType.TextMessage:
                            SendTextToAll(message: datas[1]);
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

                        case ServerMessageType.FireSingle:
                            FireSingle(datas[1]);
                            break;

                        case ServerMessageType.Hit:
                            string[] hitDatas = datas[1].Split('|');
                            OnHit(hitDatas[0], hitDatas[1]);
                            break;

                        case ServerMessageType.ClearItem:
                            ClearPlayerItem(datas[1]);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    foreach (string name in socketHT.Keys)
                    {
                        if (socketHT[name] == sk)
                        {
                            PlayerOffline(name, th);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 由玩家池移除，保存紀錄，中斷連線
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <param name="th">玩家線程類</param>
        private void PlayerOffline(string name, Thread th)
        {
            players[name].Save();
            players.Remove(name);

            socketHT.Remove(name);

            UI.RemoveFromPlayerList(name);
            UI.AddLog(name + " offline.");

            th.Abort();
        }

        /// <summary>
        /// 新增至玩家池，新增或讀取紀錄檔
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <param name="sk"></param>
        private void PlayerOnline(string name, Socket sk)
        {
            players.Add(name, new Character(name));
            players[name].Read();

            if (players[name].health <= 0)
                players[name].Respawn(map);
            
            socketHT.Add(name, sk);

            SendToPlayer(ServerMessageType.Online, name, string.Format("{0: Player name}|{1: Data Pack With Item},{2: Floor Datas}",
                                                            name, players[name].DataPackWithItem, FloorItemDatas));

            UI.AddToPlayerList(name);
            UI.AddLog(name + " online.");
        }

        /// <summary>
        /// 對所有玩家發送文字訊息
        /// </summary>
        /// <param name="message">文字訊息</param>
        private void SendTextToAll(string message)
        {
            SendToAll(ServerMessageType.TextMessage, message);

            UI.AddLog(message);
        }

        /// <summary>
        /// 更新玩家位置
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <param name="x">在Viewport中的座標X</param>
        /// <param name="y">在Viewport中的座標Y</param>
        private void UpdatePlayerLocation(string name, int x, int y)
            => players[name].UpdateLocation(x, y);

        /// <summary>
        /// 同步所有玩家資料，傳給所有玩家除了自己以外的資料
        /// <para>格式 = 同步代碼,其他玩家數,玩家1名稱|玩家素質(由管線符號'|'分隔),玩家2名稱| ...</para>
        /// </summary>
        /// <param name="name">排除的玩家名稱</param>
        private void SyncAllPlayersData(string name)
        {
            string syncStr = (players.Count - 1).ToString();
            foreach (var key in players.Keys)
            {
                if (key != name)
                    syncStr += "," + key + "|" + players[key].DataPackWithItem;
            }

            SendToPlayer(ServerMessageType.SyncPlayerData, name, syncStr);
        }

        /// <summary>
        /// 玩家撿起物品
        /// <para>格式: 玩家名稱, 物品編號 | X | Y </para>
        /// </summary>
        /// <param name="ppiInfo"></param>
        private void PlayerPickItem(string ppiInfo)
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

                            players[infos[0]].item = p.ItemNum;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 某玩家開火之訊號，告知所有玩家
        /// </summary>
        /// <param name="fireInfo"></param>
        private void FireSingle(string fireInfo)
            => SendToAll(ServerMessageType.FireSingle, fireInfo);

        /// <summary>
        /// 受傷訊號，經擊殺判定後傳給該玩家更新後的素質
        /// <para>若血量不足會進行重生</para>
        /// </summary>
        /// <param name="name">玩家名稱</param>
        /// <param name="damage">造成傷害</param>
        private void OnHit(string name, string damage)
        {
            Character ch = players[name];
            if (ch.health > 0)
                ch.health -= Convert.ToInt32(damage);

            if (!ch.isRespawning)
            {
                if (ch.health <= 0)
                {
                    UI.AddLog(name + " killed.");
                    ch.Respawn(map);
                }
            }
            SendToPlayer(ServerMessageType.Hit, name, players[name].health.ToString());
        }

        /// <summary>
        /// 清空玩家物品
        /// </summary>
        /// <param name="name">玩家名稱</param>
        private void ClearPlayerItem(string name)
            => players[name].item = "0";
        #endregion

        #region 傳遞位元組資料
        /// <summary>
        /// 傳送給目標玩家(玩家名稱)
        /// </summary>
        /// <param name="type">伺服器訊息種類</param>
        /// <param name="playerName">玩家名稱</param>
        /// <param name="inMsg">已封裝的訊息</param>
        private void SendToPlayer(ServerMessageType type, string playerName, string inMsg)
        {
            string code = EnumEx.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            SendToClient((Socket)socketHT[playerName], byteDatas);
        }

        /// <summary>
        /// 傳送給目標玩家(玩家插槽)
        /// </summary>
        /// <param name="type">伺服器訊息種類</param>
        /// <param name="sk">目標玩家的插槽</param>
        /// <param name="inMsg">已封裝的訊息</param>
        private void SendToSocket(ServerMessageType type, Socket sk, string inMsg)
        {
            string code = EnumEx.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            SendToClient(sk, byteDatas);
        }

        /// <summary>
        /// 傳送給所有玩家
        /// </summary>
        /// <param name="type">伺服器訊息種類</param>
        /// <param name="inMsg">已封裝的訊息</param>
        private void SendToAll(ServerMessageType type, string inMsg = "")
        {
            string code = EnumEx.GetOrderByEnum(type).ToString();
            byte[] byteDatas = Encoding.Default.GetBytes(code + ">" + inMsg);
            foreach (Socket sk in socketHT.Values)
                SendToClient(sk, byteDatas);
        }

        /// <summary>
        /// 傳送給目標插槽，不應直接使用此方法進行傳遞
        /// </summary>
        /// <param name="sk">目標插槽</param>
        /// <param name="byteDatas">轉換過的位元組資料</param>
        private void SendToClient(Socket sk, byte[] byteDatas)
            => sk.Send(byteDatas, 0, byteDatas.Length, SocketFlags.None);
        #endregion

        private ServerStatus status = ServerStatus.Offline;
        private const int dataSize = 0x3ff;
        private const int maxPlayers = 5;
        private string ip;
        private string port;
        private TcpListener svListener;
        private Socket svSocket;
        private Thread serverThread;
        private Thread clientThread;
        private Timer spawnTimer;
        private Map map;

        // 所有連線清單，[玩家名稱 : 連線物件]
        private Hashtable socketHT = new Hashtable();
        // 所有玩家清單，[玩家名稱 : 角色物件]
        private Dictionary<string, Character> players = new Dictionary<string, Character>();
        private List<Pickable> spawnedPickables = new List<Pickable>();
        private string FloorItemDatas
        {
            get
            {
                List<string> datas = new List<string>();

                foreach (Pickable p in spawnedPickables)
                    datas.Add(p.ToString());

                return string.Join("|", datas);
            }
        }

        public bool IsOnline => status == ServerStatus.Online;
    }
}
