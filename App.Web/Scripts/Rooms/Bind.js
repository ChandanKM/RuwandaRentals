var urlPath = window.location.pathname;
var PID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

var PropID = "RoomPage";

if (PID != "RoomPage") {
     PropID = PID;
}

else {
    PropID = $.localStorage("RoomPropertyId");
}
$(function () {

    RoomListVM.getRoom();
    ko.applyBindings(RoomListVM, document.getElementById('RoomsDT'));
    //$('.RoomsDTb').dataTable({ responsive: true, 'iDisplayLength': 15, });
    //$('#DataTables_Table_0_length').css('display', 'none');
    ////var x = document.getElementsByClassName("odd");
    //$('.odd').remove();
    //$('#DataTables_Table_0_info').css('display', 'none');
    //element.parentNode.removeChild(x);
});
function CreateRooms() {
    
       

    window.location.href = '/Vendor/RoomCreate/' + PropID + '';
    }

//View Model
var RoomListVM = {

    Rooms: ko.observableArray([]),
    getRoom: function () {
       

        var self = this;
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/rooms/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Prop_Id: PropID},

            success: function (data) {
             
                
                if (data.length == 0) {
                 
                    $('#liRoomCount').append("Total Rooms : " + data.length);
                    $('#nodata').hide()
                    $('#lblnodata').css('display', 'block')
                    $('#lblnodata').append('<p>No rooms available</p>')
                    AppCommonScript.HideWaitBlock();
                }
                else {
                    AppCommonScript.HideWaitBlock();
                    $('#PropertyNameLI').append(data[0].Prop_Name);
                    $('#liRoomCount').append("Total Rooms : " + data.length);
                    for (var i = 0; i < data.length; i++) {
                        self.Rooms.push(new RoomClass(data[i]));
                    } 

                   
                }
            },
            error: function (err) {
                AppCommonScript.HideWaitBlock();
                //  alert(err.status + " : " + err.statusText);

            }

        });

    },
};
self.editroom = function (room) {

    $.cookie("ImageRoom", 'null')
    $.localStorage("RoomPropertyIdEdit", PropID);
    $.localStorage("Tabs", "FF");
    window.location.href = '/Vendor/RoomEdit/' + room.Room_Id;


};
self.Createroom = function (room) {

    
    window.location.href = '/Vendor/RoomCreate/' + PropID;


};
self.editroomPolicy = function (room) {

    $.cookie("ImageRoom", 'null')
    $.localStorage("RoomPropertyIdEdit", PropID);
    $.localStorage("Tabs", "PP");
    window.location.href = '/Vendor/RoomEdit/' + room.Room_Id;


};
self.deleterooms = function () {

    if (confirm('Are you sure you want to delete this?')) {
        var room = new InitializePropertyDelete(this);
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            url: '/api/rooms/DeteteProperty',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(room),

            contentType: 'application/json',
            success: function (result) {
                window.location.href = "/property/Bind";

                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(result);
            },
            error: function (err) {
                if (err.responseText == "Creation Failed") {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify("Creation Failed");
                }
                else {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);

                }
            },
            complete: function () {
            }
        });
    }
};


var RoomSuspend = {

    ActiveSuspend: function (data) {
        $.ajax({
            type: "POST",
            url: '/api/rooms/Suspend',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            success: function (response) {
                location.reload();
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);
            }
        });
    }
};


self.Suspendroom = function (room) {
    if (confirm('Are you sure to Suspend this room ?')) {
        RoomSuspend.ActiveSuspend(room)
    }
    // window.location.href = 'RoomType/Suspend/' + room.Room_Id;
};




//Model
function RoomClass(data) {
    
    var room = this;
    room.Room_Id = data.Room_Id;
    room.Room_Name = ko.observable(data.Room_Name);
    room.Room_Descr = ko.observable(data.Room_Descr);
    room.Room_Agreed_Availability = ko.observable(data.Room_Agreed_Availability);
    room.Room_Standard_rate = ko.observable(data.Room_Standard_rate);
    room.Image_dir = ko.observable(data.Image_dir);

}


//function InitializePropertyDelete(data) {

//    var property = new function () { };

//    $.cookie("V_Id", "4");
//    property.PropId = data.Id;
//    property.Prop_Approved_By = $.cookie("V_Id");
//    return property;
//}

