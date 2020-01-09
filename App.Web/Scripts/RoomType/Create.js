$(document).ready(function () {
    InitializeRoomtypeViewModel();
});

function RoomTypeViewModel() {

    this.Room_TypeId = ko.observable();
    this.Room_Name = ko.observable();
    this.Room_Descr = ko.observable();
    this.Room_Active_flag = ko.observable();

}


function InitializeRoomtypeViewModel() {

    roomtypeViewModel = new RoomTypeViewModel();
    ko.applyBindings(roomtypeViewModel, document.getElementById("divCreateRoomType"));
}

function CreateRoomType() {
    // AppCommonScript.ShowWaitBlock();
    var roomtype = new InitializeRoomType();
    
    $.ajax({
        type: "POST",
        url: "/api/RoomType/create",
        data: $.parseJSON(ko.toJSON(roomtype)),
        dataType: "json",
        success: function (response) {
            //  Successed(response);
            BlockHideShow();
            //  pushDataToGrid();
            //BindRoomTypesGrid();
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}


function UpdateRoomType(roomtype) {


    $.ajax({
        type: "POST",
        url: "/api/RoomType/update",
        data: $.parseJSON(ko.toJSON(roomtype)),
        // data: {roomtype_Id:Room_TypeId,room_name:Room_Name,room_descr:Room_Descr},
        dataType: "json",
        success: function (response) {
            //  Successed(response);
            location.reload();
            //  pushDataToGrid();
            //BindRoomTypesGrid();
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}

function BlockHideShow() {
    //$('#btnSave').hide();
    $('#divCreateRoomType').hide();
    $('#btnCreate').show();
    $('#Room_Name').val('');
    $('#Room_Descr').val('');
    $('#btnUpdate').hide();
    location.reload();
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function InitializeRoomType() {
    var roomtype = new function () { };
    roomtype.Room_TypeId = roomtypeViewModel.Room_TypeId();
    roomtype.Room_Name = roomtypeViewModel.Room_Name();
    roomtype.Room_Descr = roomtypeViewModel.Room_Descr();
    roomtype.Room_Active_flag = roomtypeViewModel.Room_Active_flag();

    return roomtype;
}


function InitializeEditModel() {
    var self = this;
    self.Room_TypeId = ko.observable();
    self.Room_Name = ko.observable();
    self.Room_Descr = ko.observable();
    self.Room_Active_flag = ko.observable();

}






