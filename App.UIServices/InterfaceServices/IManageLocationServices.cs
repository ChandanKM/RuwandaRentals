using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices.InterfaceServices
{
    public interface IManageLocationServices
    {
        DataSet AddLocation(ManageLocationBo locationBo);
        TransactionStatus UpdateLocation(ManageLocationBo locationBo);
        DataSet GetAllLocation();
        TransactionStatus Delete(int Id);
    }
}
