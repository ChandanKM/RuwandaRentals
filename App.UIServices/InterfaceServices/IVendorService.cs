using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;



namespace App.UIServices
{
    public interface IVendorService
    {
        TransactionStatus CreateVendor(VendorBo vendorBo);
        TransactionStatus EditVendor(VendorEditBo vendorEditBo);
        List<object> Bind(int id);
        List<object> Edit(int Id);
        TransactionStatus DeleteVendor(VendorEditBo vendorEditBo);
        List<object> BindPropertyByVendorId(int id);

        void ExecuteAddRoomTimer();
        DataSet GetRoomsRate(int ID);
        TransactionStatus UpdateRoomRates(int Inv_Id, int Price);
        TransactionStatus UpdateAvailableRoom(int Inv_Id, int Available);
        TransactionStatus updaterackRates(int Inv_Id, int Vndr_Amnt);
    }
}
