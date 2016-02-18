namespace ProcCore.DatabaseCore
{
    /// <summary>
    /// 系統欄位更新所要使用的欄位
    /// </summary>
    public enum UpdateFieldsInfoType
    {
        None, Insert, Update, Both,Lock,UnLock
    }
    public enum SQLUpdateType
    { Insert, Update, Delete }
    public enum WhereCompareType
    {
        Like, LikeRight, LikeLeft,
        Equel,
        Than,
        Less,
        ThanEquel,
        LessEquel,
        UnEquel,
        Between,
        NotBetween,
        In
    }
    public enum SQLValueType
    {
        Int, String, DateTime, Boolean
    }
    public enum OrderByType
    {
        ASC, DESC
    }
    public enum ConnectionType
    {
        OleDb, SqlClient, MySqlClient
    }
}
