using App.BusinessObject;
using App.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UIServices
{
    public interface ICCAvenueServices
    {
        DataSet AddCCAvenue(CCAvenueBo ccBo);
        TransactionStatus EditCCAvenue(CCAvenueBo ccBo);
        DataSet GetCCAvenueById(int ccavenue_Id);
        List<Object> Bind();
    }
}
