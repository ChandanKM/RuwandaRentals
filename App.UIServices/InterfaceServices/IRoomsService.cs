using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices.InterfaceServices
{
    public interface IRoomsService
    {
        string CreateRooms(RoomsBo roomsBo);
        TransactionStatus EditRooms(RoomsEditBo roomsEditBo);
        List<object> Bind(int Prop_Id);
        List<object> Edit(int Id);
        TransactionStatus DeleteRooms(RoomsEditBo roomsEditBo);
        TransactionStatus SuspendRoom(int room_Id);
        List<object> GetRoomType();


        TransactionStatus Createpolicies(PoliciesBo policiesBo);
        TransactionStatus Editpolicies(PoliciesEditBo policiesEditBo);
        List<object> Bindpolicies(int Prop_Id, int Vendor_Id);
        List<object> Editpoliciesbyid(int Id);        
        TransactionStatus Suspendpolicy(int Id);


        List<object> BindFacility(int Room_id);
        TransactionStatus ActivateFacility(RoomFacilityEditBo roomfacility);
        List<string> GetAutoCompleteRoom(string terms);


        TransactionStatus UpdateRackPrice(int Inv_Id, int race_price);
    }
}
