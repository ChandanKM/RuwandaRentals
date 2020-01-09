using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BusinessObject;
using App.Common;

namespace App.UIServices
{
   public interface ILocationService
    {
        TransactionStatus CreateLocation(LocationBo locationBo);
        TransactionStatus EditLocation(EditLocationBo editlocationBo);
        List<object> BindLocation();
        List<object> Edit(string id);
    }
}
