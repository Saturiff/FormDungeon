namespace DungeonServer.Server
{
    /// <summary>
    /// 等待伺服器回傳值之狀態
    /// </summary>
    public enum ServerMessageStatus
    {
        None,
        Waiting,
        Success,
        Fail
    }
}
