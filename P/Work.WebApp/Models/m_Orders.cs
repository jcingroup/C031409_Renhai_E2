//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Work.WebApp.Models
{
    using System;
    using System.Collections.Generic;
    public partial class m_Orders  {
    public int orders_id { get; set; }
    public string orders_sn { get; set; }
    public int member_detail_id { get; set; }
    public string member_name { get; set; }
    public string tel { get; set; }
    public string zip { get; set; }
    public string address { get; set; }
    public string mobile { get; set; }
    public string gender { get; set; }
    public string 申請人生日 { get; set; }
    public string email { get; set; }
    public int total { get; set; }
    public string 付款方式 { get; set; }
    public Nullable<System.DateTime> transation_date { get; set; }
    public Nullable<System.DateTime> 付款時間 { get; set; }
    public int orders_state { get; set; }
    public string 查詢序號 { get; set; }
    public string 付款方式名稱 { get; set; }
    public string orders_state_name { get; set; }
    public string 銀行帳號 { get; set; }
    public System.DateTime 新增時間 { get; set; }
    public int InsertUserId { get; set; }
    public int member_id { get; set; }
    public Nullable<bool> C_Hiden { get; set; }
    public Nullable<int> C_InsertUserID { get; set; }
    public Nullable<System.DateTime> C_InsertDateTime { get; set; }
    public Nullable<int> C_UpdateUserID { get; set; }
    public Nullable<System.DateTime> C_UpdateDateTime { get; set; }
    public int orders_type { get; set; }
    public int y { get; set; }
    }
}
