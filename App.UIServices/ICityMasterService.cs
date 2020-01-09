using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
   public interface  ICityMasterService
    {
      
       TransactionStatus AddCityMaster(CityMasterBo cityMaster);
       DataSet Bind();
    }

}
