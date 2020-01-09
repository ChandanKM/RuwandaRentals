using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface ICityService
    {
        TransactionStatus CreateCity(CityBo cityBo);
        TransactionStatus EditCity(CityBo1 cityBo);
        List<object> Bind();
        List<object> Edit(int Id); 
    }
}
