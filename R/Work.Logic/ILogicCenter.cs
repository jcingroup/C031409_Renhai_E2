using System;
namespace ProcCore.Business
{
    interface ILogicCenter
    {
        int GetNewId(CodeTable tab);
        string getOrderSN();
        bool existHolder(int member_id);
    }
}
