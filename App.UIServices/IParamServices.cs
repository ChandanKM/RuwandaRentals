using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IParamServices
    {
        DataSet AddParam(ParamBo paramBo);
        TransactionStatus UpdateParam(ParamBo paramBo);
        TransactionStatus SuspendParam(int param_Id);
        DataSet GetParam(int propId, int authId);

        TransactionStatus UpdateParamHiddenGems(int Id, string value);
          TransactionStatus UpdateParamValue(int Id, string value);
        TransactionStatus UpdateParamType(int Id, string type);
        TransactionStatus UpdateParamPermission(int Id, string flag);
     
    }
}
