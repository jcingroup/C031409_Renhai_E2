using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotWeb.WebApp.Models
{
    public class c_ShoppingMaster
    {
        public string orders_id { get; set; }
        public string member_name { get; set; }
        public string address { get; set; }
        public string zip { get; set; }
        public c_ShoppingDetail[] detail { get; set; }
    }
    public class c_ShoppingDetail
    {
        public string product_sn { get; set; }
        public string product_name { get; set; }
        public int member_detail_id { get; set; }
        public int member_name { get; set; }
        public int price { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string l_birthday { get; set; }
        public string born_time { get; set; }
        public string born_sign { get; set; }



    }
}