//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Work.YearLight
{
    using System;
    using System.Collections.Generic;
    
    public partial class 點燈位置資料表
    {
        public int 序號 { get; set; }
        public string 位置名稱 { get; set; }
        public int 年度 { get; set; }
        public string 空位 { get; set; }
        public string 產品編號 { get; set; }
        public Nullable<int> 價格 { get; set; }
        public Nullable<bool> C_Hiden { get; set; }
        public Nullable<int> C_InsertUserID { get; set; }
        public Nullable<System.DateTime> C_InsertDateTime { get; set; }
        public Nullable<int> C_UpdateUserID { get; set; }
        public Nullable<System.DateTime> C_UpdateDateTime { get; set; }
        public Nullable<int> C_LockUserID { get; set; }
        public Nullable<System.DateTime> C_LockDateTime { get; set; }
        public Nullable<bool> C_LockState { get; set; }
        public bool IsReject { get; set; }
    }
}
