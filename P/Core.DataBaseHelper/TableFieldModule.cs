using System;
using System.Collections.Generic;
using System.Data;

using System.Linq.Expressions;
using System.Reflection;

using ProcCore.NetExtension;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.DataBaseConnection;

namespace ProcCore.DatabaseCore.TableFieldModule
{
    public abstract class TableMap<TabObjSource>: IDisposable
    {
        public String N { get; set; }
        public TabObjSource GetTabObj { get; set; }

        /// <summary>
        /// 收集Table的Key Value對應表，主要提供給Grid的代碼類欄位做轉換，可減沙在SQL做Table Join 。
        /// </summary>
        /// <param name="idTabFields">Id欄位</param>
        /// <param name="nameTabFields">Text欄位</param>
        /// <param name="conn">Connection連線</param>
        /// <returns> Dictionary int String </returns>
        public Dictionary<int, String> CollectIdNameFields(
            Expression<Func<TabObjSource, FieldModule>> idTabFields,
            Expression<Func<TabObjSource, FieldModule>> nameTabFields,
            CommConnection conn
            )
        {
            Func<TabObjSource, FieldModule> id = idTabFields.Compile();
            Func<TabObjSource, FieldModule> name = nameTabFields.Compile();

            FieldModule fieldId = id.Invoke(this.GetTabObj);
            FieldModule fieldName = name.Invoke(this.GetTabObj);

            String sql = String.Format("Select {0},{1} From {2}", fieldId.N, fieldName.N, this.N);
            DataTable dt = conn.ExecuteData(sql);

            Dictionary<int, String> data = new Dictionary<int, String>();

            foreach (DataRow dr in dt.Rows)
            {
                data.Add(dr[fieldId.N].CInt(), dr[fieldName.N].ToString());
            }
            return data;
        }

        /// <summary>
        /// 取得Table的FieldModule的陣列集合
        /// </summary>
        /// <returns></returns>
        public FieldModule[] GetFieldModules()
        {
            var FInfo = this.GetType().GetFields();
            List<FieldModule> ls_FieldModules = new List<FieldModule>();
            foreach (FieldInfo f in FInfo)
            {
                Object GetObj = f.GetValue(this);

                if (GetObj.GetType() == typeof(FieldModule))
                {
                    ls_FieldModules.Add((FieldModule)GetObj);
                }
            }
            return ls_FieldModules.ToArray();
        }
        public Dictionary<String,FieldModule> KeyFieldModules { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
    public class FieldModule
    {
        /// <summary>
        /// 資料庫欄位實際對應名稱
        /// </summary>
        public String N { get; set; }
        /// <summary>
        /// 資料庫欄位大略型態 Int Boolean DateTime String
        /// </summary>
        public SQLValueType T { get; set; }

        /// <summary>
        /// 可代入值 在primary key才比較會用到，其他請用標準module代值 
        /// </summary>
        public Object V { get; set; }

        public FieldsRules rule { get; set; }
    }

    //以下尚未進行完成 先做訂義
    public enum CheckType {
        none, email, digits,url,date,customer  
    }
    public class FieldsRules{
        public Boolean required { get; set; }
        public Boolean rangecheck { get; set; }
        public CheckType checktype { get; set; }
        public String requiredErrMessage { get; set; }
        public String checktypeErrMessage { get; set; }

        public int? min { get; set; }
        public int? max { get; set; }

        public DateTime? minDate { get; set; }
        public DateTime? maxDate { get; set; }
    }
}
