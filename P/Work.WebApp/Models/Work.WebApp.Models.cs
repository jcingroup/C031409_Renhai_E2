﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Work.WebApp.Models
{
    public class cartMaster
    {
        public int member_id { get; set; }
        public int member_detail_id { get; set; }
        public string member_name { get; set; }
        public string gender { get; set; }
        public string zip { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string tel { get; set; }
        public int total { get; set; }
        public int race { get; set; }
        public int gold { get; set; }
        public bool is_light_serial { get; set; }
        public IList<cartDetail> Item { get; set; }

        //修改時會用到
        public string orders_sn { get; set; }
        public DateTime? transation_date { get; set; }
        public int? y { get; set; }
    }
    public class cartDetail
    {
        public int price { get; set; }
        public string product_sn { get; set; }
        /// <summary>
        /// 可選擇燈位需用到 主副斗用
        /// </summary>
        public int light_site_id { get; set; }
        public string light_name { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string product_name { get; set; }
        /// <summary>
        /// 這裡指的是會員資料表中的序號
        /// </summary>
        public int member_detail_id { get; set; }
        public string member_name { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string born_time { get; set; }
        public string born_sign { get; set; }
        public string l_birthday { get; set; }
        public string age { get; set; }
        /// <summary>
        /// 析福事項
        /// </summary>
        public string memo { get; set; }
        public int race { get; set; }
        public int gold { get; set; }
        public int manjushri { get; set; }
        public int SY { get; set; }
        public int SM { get; set; }
        public int SD { get; set; }
        public int LY { get; set; }
        public int LM { get; set; }
        public int LD { get; set; }
        public bool isOnLeapMonth { get; set; }
        public bool isOnOrder { get; set; }
        public int detail_sort { get; set; }

        /// <summary>
        /// 超渡法會用
        /// </summary>
        public string departed_address { get; set; }
        public string departed_name { get; set; }
        public string departed_qty { get; set; }
        public int? assembly_batch_sn { get; set; }
        public int? y { get; set; }

        //祈福許願燈用
        public List<WishText> wishs { get; set; }
        public string wish_memo { get; set; }
    }
    public class WishText
    {
        public int wish_id { get; set; }
        public bool can_text { get; set; }
        public string wish_text { get; set; }
    }
    public class LuniInfo
    {
        public int SY { get; set; } //西元
        public int LY { get; set; } //民國
        public int M { get; set; }
        public int D { get; set; }
        public bool IsLeap { get; set; }
    }
    /// <summary>
    /// 超渡法會
    /// </summary>
    public class BatchList : m_Orders_Detail
    {
        public string batch_title { get; set; }
        public System.DateTime batch_date { get; set; }
        public string batch_timeperiod { get; set; }
        public string lunar_y { get; set; }
        public string lunar_m { get; set; }
        public string lunar_d { get; set; }
    }
}