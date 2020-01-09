using App.BusinessObject;
using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UIServices.InterfaceServices
{
    public interface IPropertyService
    {
        //TransactionStatus EditUser(UserBo1 userBo);
        List<object> Bind(int VendId);
        //List<object> Edit(int Id);
        //TransactionStatus DeleteUser(UserBo1 userBo);

        List<object> GetCity();

     
      

        string CreateProperty(PropertyBo propertyBo);

        List<object> BindFacility(string Id);

        List<object> GetFacilityType();

        List<object> GetFacilityName();

        TransactionStatus CreateFacility(PropertyBo propertyBo);

        TransactionStatus DeleteFacility(PropertyBo propertyBo);

        TransactionStatus DeleteProperty(PropertyBo propertyBo);

        List<object> Edit(string Id);

        List<object> BindPolicy(int PropId, int VendId);
        List<object> BindRoomPolicy(int PropId, int VendId,int RoomId);

        TransactionStatus DeteteEvent(PropertyBo propertyBo);
        TransactionStatus DeteteImage(PropertyBo propertyBo);
        TransactionStatus Edit(PropertyBo userBo);

        string CreateBankDetails(PropertyBo propertyBo);

        TransactionStatus EditBank(PropertyBo bankBo);

        List<object> EditBankDetails(string Id);

       
        
        List<object> BindImage(string Id);
        TransactionStatus UpdateImageFlag(int Id, string flag);
        TransactionStatus SetDefaultImage(int PropId, int ImageId);

        TransactionStatus createPolicy(PropertyBo propertyBo);
        TransactionStatus createRoomPolicy(PropertyBo propertyBo);
        TransactionStatus EditPolicy(PropertyBo propertyBo);
        TransactionStatus DetetePolicy(PropertyBo propertyBo);


        List<object> BindFacilityimage(int prop_id);
        TransactionStatus ActivateFacility(RoomFacilityEditBo roomfacility);

        List<string> GetAutoCompleteLocation(string terms);
        List<object> GetAutoCompleteLocationWithId(string terms);
        List<object> PropertyAutoCompleteSearch(string terms);
        
    }
}
