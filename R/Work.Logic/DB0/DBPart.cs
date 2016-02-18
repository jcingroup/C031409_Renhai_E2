using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace ProcCore.Business.DB0
{
    public partial class Renhai_LightSiteEntities : DbContext
    {
        public Renhai_LightSiteEntities(string connectionstring)
            : base(connectionstring)
        {
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Log.Write(ex.Message, ex.StackTrace);
                foreach (var err_Items in ex.EntityValidationErrors)
                {
                    foreach (var err_Item in err_Items.ValidationErrors)
                    {
                        Log.Write("欄位驗證錯誤", err_Item.PropertyName, err_Item.ErrorMessage);
                    }
                }

                throw;
            }
            catch (DbUpdateException ex)
            {
                Log.Write("DbUpdateException", ex.InnerException.Message);
                throw;
            }
            catch (EntityException ex)
            {
                Log.Write("EntityException", ex.Message);
                throw;
            }
            catch (UpdateException ex)
            {
                Log.Write("UpdateException", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                 Log.Write("Exception", ex.Message);
                throw;
            }
        }

    }

    #region Model Expand

    public partial class Product
    {
        public string product_sn_o { get; set; }
    }

    public partial class Member : BaseEntityTable
    {
        public Member_Detail[] getMemberDetail { get; set; }    
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
    public class q_Member_Detail : QueryBase
    {
        public string name { set; get; }
    }
    public class q_Member : QueryBase {
        public string key { set; get; }
    }


    public class q_Orders_Detail : QueryBase
    {
        public string orders_sn { set; get; }
    }
    public class q_Orders : QueryBase
    {
        public string orders_sn { set; get; }
    }


    #endregion

    #region c_Model_Define
    public class c_Member
    {
        public q_Member q { get; set; }
        public Member m { get; set; }
        public q_Member_Detail qd { get; set; }
        public Member_Detail md { get; set; }
    }
    public class c_Orders
    {
        public q_Orders q { get; set; }
        public Orders m { get; set; }
        public q_Orders_Detail qd { get; set; }
        public Orders_Detail md { get; set; }
    }


    public class c_AspNetRoles
    {
        public q_AspNetRoles q { get; set; }
        public AspNetRoles m { get; set; }
    }
    public partial class c_AspNetUsers
    {
        public q_AspNetUsers q { get; set; }
        public AspNetUsers m { get; set; }
    }
    #endregion
}
