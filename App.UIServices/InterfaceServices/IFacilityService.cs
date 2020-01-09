using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IFacilityService
    {
        TransactionStatus CreateFacility(FacilityBo facilityBo);
        //TransactionStatus EditFacility(FacilityEditBo facilityeditBo);
        //List<object> Edit(int Id);

        List<object> BindFacility();
        TransactionStatus DeleteFacility(FacilityBo facilityBo);
        List<object> Edit(string Id);
        TransactionStatus EditFacility(FacilityBo facilityBo);
    }
}
