using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BusinessObject;
using App.Common;

namespace App.UIServices
{
    public interface IPincodeService
    {
        TransactionStatus CreatePincode(PincodeBo pincodeBo);
        TransactionStatus EditPincode(EditPincodeBo editpincodeBo);
        List<object> BindPincode();
        List<object> Edit(string id);
    }
}
