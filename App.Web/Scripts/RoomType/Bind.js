$(function () {

    ko.applyBindings(RoomListVM, document.getElementById("divRoomTypeGrid"));
    RoomListVM.getRoomTypes();
});


var RoomListVM = {

    RoomTypes: ko.observableArray([]),
    getRoomTypes: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/RoomType/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
               // for (var i = 0; i < data.lenght; i++) {
                   self.RoomTypes(data);
              //  }
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};


var RoomTypeSuspend = {

    ActiveSuspend: function (data) {
        $.ajax({
            type: "POST",
            url: '/api/RoomType/Suspend',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            success: function (response) {
                location.reload();
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};


self.Suspend = function (room) {
    if (confirm('Are you sure ?')) {
        RoomTypeSuspend.ActiveSuspend(room)
    }
    // window.location.href = 'RoomType/Suspend/' + room.Room_Id;
};





var RoomTypeEditModel = {

    getRoomTypes1: function (roomtype_Id) {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/RoomType/GetRoomById',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { roomtype_Id: roomtype_Id },
            success: function (data) {

                self.Room_TypeId = (data.Table[0].Room_TypeId);
                self.Room_Name = (data.Table[0].Room_Name);
                self.Room_Descr = (data.Table[0].Room_Descr);
                self.Room_Active_flag = (data.Table[0].Room_Active_flag);
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    }
};




self.Edit = function (room) {
    
    ko.cleanNode(document.getElementById('divCreateRoomType'))
    RoomTypeEditModel.getRoomTypes1(room.Room_TypeId);
    ko.applyBindings(RoomTypeEditModel, document.getElementById('divCreateRoomType'));

    $('#divCreateRoomType').show();
    $('#btnCreate').hide();
    $('#btnSave').hide();
    $('#btnUpdate').show();
};



