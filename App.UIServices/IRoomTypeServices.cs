using System;
using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IRoomTypeServices
    {
        DataSet AddRoomType(RoomTypeBo roomBo);
        TransactionStatus EditRoomType(RoomTypeBo roomBo);
        DataSet GetRoomTypeById(int room_Id);
        TransactionStatus SuspendRoomType(int room_Id);
        TransactionStatus ActiveRoomType(int room_Id);
        List<Object> Bind();

        roomresponse GetRoomMap(Roomrequest Roomrequest);

        TransactionStatus RoomInventry(request request);
        bool SetDataToEMCLog(string requestFrom, string requestTo, string requestBody, string status);

       
    }
}
