$(document).ready(function () {

    InitializeRoomViewModel();
});
var PropID = $.cookie("Property_Id");
var VendorId = $.cookie("VendId")
function RoomViewModel() {

    
    this.Policy_Descr = ko.observable("");
    this.Policy_Name = ko.observable("");
   
    this.CreatePolicy = function () {
        CreatePolicy();
    };
}

function GetroomtypeLookup() {
    
    var roomtypes = ko.observableArray([{ Type_Id: 0, Room_Descr: 'Select Room Type' }]);

    $.getJSON("/api/rooms/allroomtypes", function (result) {
        ko.utils.arrayMap(result, function (item) {
            roomtypes.push({ Type_Id: item.Type_Id, Room_Descr: item.Room_Descr });
        });
    });
    return roomtypes;
}

function InitializeRoomViewModel() {
    
    roomViewModel = new RoomViewModel();   
    ko.applyBindings(roomViewModel, document.getElementById("myModal1"));

}

function CreatePolicy() {
    AppCommonScript.ShowWaitBlock();
    var policy = new InitializePolicy();
    $.ajax({
        type: "POST",
        url: "/api/rooms/create",
        data: $.parseJSON(ko.toJSON(room)),
        dataType: "json",
        success: function (response) {

            var result = { Status: true, ReturnMessage: { ReturnMessage: "Successfully " + response }, ErrorType: "Success" };
            Successed(result);

        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function InitializePolicy() {
    
    var policy = new function () { };
    policy.Policy_Descr = roomViewModel.Policy_Descr();
    policy.Policy_Name = roomViewModel.Policy_Name();
   
   
    return policy;
}
