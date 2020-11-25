namespace DungeonServer
{
    /// <summary>
    /// 伺服器特殊指令碼，客戶端需與伺服器端相同
    /// </summary>
    public enum ServerMessageType
    {
        Online,
        Verification,
        Offline,
        Message,
        Action,
        Sync
    }
}
