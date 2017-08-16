using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Linq;
using Work.WebApp.Models;
using ProcCore.Business.Base;
using System.Collections.Generic;

namespace Work.WebApp.Models
{
    #region Model Expand

    public partial class Orders
    {
        public IList<Orders_Detail> getOrders_Detail { get; set; }
    }
    public partial class Reject
    {
        public IList<Reject_Detail> getReject_Detail { get; set; }
    }
    public partial class Member
    {
        public IList<Member_Detail> getMember_Detail { get; set; }
    }
    public partial class Member_Detail
    {
        public int fortune_value { get; set; }
        public string join_date { get; set; }//契子入會日期
    }
    public partial class m_Member
    {
        public int member_detail_count { get; set; }
        public int year { get; set; }//判斷今年度該戶有沒有購買資料
        public int member_detail_id { get; set; }
    }
    public partial class m_Member_Detail
    {
        public string householder { get; set; }
    }
    public partial class m_Orders
    {
        public string InsertUserName { get; set; }
    }
    public class RejectReport
    {
        public string orders_sn { get; set; }
        public int total { get; set; }
        public string light_name { get; set; }
        public int price { get; set; }
        public string member_name { get; set; }
        public DateTime reject_date { get; set; }
        public int user_id { get; set; }
        public string InsertUserName { get; set; }

    }
    public partial class m_TempleAccount
    {
        public string member_name { get; set; }
        public string birthday { get; set; }
        public string sno { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string zip { get; set; }
        public string addr { get; set; }
        public DateTime? member_insertDateTime { get; set; }
    }
    public partial class m_Orders_Detail
    {
        public string tel { get; set; }
        public string mobile { get; set; }
        public string sno { get; set; }
        public string insure_birthday { get; set; }
        public DateTime? member_insertDateTime { get; set; }
        public string l_insertDateTime { get; set; }//國曆繳款日期
    }
    public partial class m_AssemblyBatch
    {
        public int count { get; set; }
        public int count_1401 { get; set; }//祖先甲乙
        public int count_1402 { get; set; }//祖先乙
        public int count_1403 { get; set; }//冤親債主
        public int count_1404 { get; set; }//嬰靈
        public int index { get; set; }
    }
    /// <summary>
    /// 過濾地址不重複
    /// </summary>
    public class AddressCompare : IEqualityComparer<m_Member>
    {
        #region IEqualityComparer<m_Member> m_Member

        public bool Equals(m_Member x, m_Member y)
        {
            return (x.zip == y.zip && x.address == y.address);
        }

        public int GetHashCode(m_Member obj)
        {
            return obj.ToString().GetHashCode();
        }
        #endregion
    }
    #endregion
    #region q_Model_Define
    public class q_AspNetRoles : QueryBase
    {
        public string Name { set; get; }

    }
    public class q_AspNetUsers : QueryBase
    {
        public string UserName { set; get; }

    }
    public class q_Member : QueryBase
    {
        public string householder { get; set; }

        public string member_tel { get; set; }
    }
    public class q_Member_Detail : QueryBase
    {
        public int? member_id { get; set; }
        public int? year { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string zipcode { get; set; }
        public int? type { get; set; }

    }
    public class q_Order_Detail : QueryBase
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public string fortune_psn { get; set; }

    }
    public class q_Reject : QueryBase
    {
        public string order_sn { get; set; }
        public string member_name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class q_Excel_統計表 : QueryBase
    {
        /// <summary>
        /// 列印日期：起 
        /// </summary>
        public DateTime? Date1 { get; set; }
        /// <summary>
        /// 起
        /// </summary>
        public int Time1 { get; set; }
        /// <summary>
        /// 列印日期：迄
        /// </summary>
        public DateTime? Date2 { get; set; }
        /// <summary>
        /// 迄
        /// </summary>
        public int Time2 { get; set; }
        /// <summary>
        /// 經手人員
        /// </summary>
        public int People { get; set; }

    }
    public class q_TempleMember : QueryBase
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string member_name { get; set; }
        public string tel { get; set; }
        public string pageclass { get; set; }
        public string is_close { get; set; }
        public string mobile { get; set; }
    }
    public class q_TempleAccount : QueryBase
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string product_sn { get; set; }
        public int? account_sn { get; set; }
        public int InsertUserId { get; set; }
        public string member_name { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public int? temple_member_id { get; set; }
    }
    public class q_AssemblyBatch : QueryBase
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? year { get; set; }
        public string keyword { get; set; }
    }
    public class q_BatchList : QueryBase
    {
        public int? year { get; set; }
        public int? assembly_batch_sn { get; set; }
        public string product_sn { get; set; }
    }
    public class q_WishList : QueryBase
    {
        public int? year { get; set; }
    }
    #endregion
}
