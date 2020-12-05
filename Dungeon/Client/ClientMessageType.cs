namespace DungeonGame.Client
{
    /// <summary>
    /// 伺服器特殊指令碼，客戶端需與伺服器端相同
    /// </summary>
    public enum ClientMessageType
    {
        Online,
        Verification,
        Offline,
        TextMessage,
        Action,
        SyncPlayerData
    }
}
