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
    public partial class AssemblyBatch 
    {
        public AssemblyBatch()
        {
            this.訂單明細檔 = new HashSet<Orders_Detail>();
            this.點燈位置資料表 = new HashSet<Light_Site>();
            this.NewBatch = new HashSet<AssemblyBatchChglog>();
            this.OldBatch = new HashSet<AssemblyBatchChglog>();
        }
    
        public int batch_sn { get; set; }
        public string batch_title { get; set; }
        public string time_sn { get; set; }
        public string batch_timeperiod { get; set; }
        public System.DateTime batch_date { get; set; }
        public string lunar_y { get; set; }
        public string lunar_m { get; set; }
        public string lunar_d { get; set; }
        public int batch_qty { get; set; }
    
    	[JsonIgnore]
        public virtual ICollection<Orders_Detail> 訂單明細檔 { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Light_Site> 點燈位置資料表 { get; set; }
    	[JsonIgnore]
        public virtual ICollection<AssemblyBatchChglog> NewBatch { get; set; }
    	[JsonIgnore]
        public virtual ICollection<AssemblyBatchChglog> OldBatch { get; set; }
    }
}
