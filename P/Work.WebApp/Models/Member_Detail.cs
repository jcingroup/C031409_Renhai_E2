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
    
    using Newtonsoft.Json;
    public partial class Member_Detail 
    {
        public Member_Detail()
        {
            this.Orders_Detail = new HashSet<Orders_Detail>();
            this.Reject_Detail = new HashSet<Reject_Detail>();
            this.Fortune_Light = new HashSet<Fortune_Light>();
            this.Wish_Light = new HashSet<Wish_Light>();
            this.AssemblyBatchChglog = new HashSet<AssemblyBatchChglog>();
        }
    
        public int member_detail_id { get; set; }
        public int member_id { get; set; }
        public bool is_holder { get; set; }
        public string member_name { get; set; }
        public string 電話區碼 { get; set; }
        public string tel { get; set; }
        public string zip { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string lbirthday { get; set; }
        public string born_time { get; set; }
        public string EMAIL { get; set; }
        public string Memo { get; set; }
        public string born_sign { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public Nullable<System.DateTime> i_InsertDateTime { get; set; }
        public bool isOnLeapMonth { get; set; }
        public int member_detail_category_id { get; set; }
        public string sno { get; set; }
        public string insure_birthday { get; set; }
        public bool is_delete { get; set; }
    
    	[JsonIgnore]
        public virtual Member Member { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Orders_Detail> Orders_Detail { get; set; }
    	[JsonIgnore]
        public virtual Member_Detail_Category Member_Detail_Category { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Reject_Detail> Reject_Detail { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Fortune_Light> Fortune_Light { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Wish_Light> Wish_Light { get; set; }
    	[JsonIgnore]
        public virtual ICollection<AssemblyBatchChglog> AssemblyBatchChglog { get; set; }
    }
}
