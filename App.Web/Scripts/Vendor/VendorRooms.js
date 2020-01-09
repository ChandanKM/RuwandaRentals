
var urlPath = window.location.pathname;
var PropID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
$(document).ready(function () {

    AllRooms = new RoomyListVM();

    AllRooms.getRooms();

});
//View Model
var RoomyListVM = function () {
    Prop = this;
    Prop.getRooms = function () {
        var self = this;
        self.Rooms = ko.observableArray([]),
        $.ajax({
            type: "GET",
            url: '/api/vendors/GetRooms',
            data: { ID: PropID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {
                    self.Rooms.push(new RoomClass(data[i])); //Put the response in ObservableArray
                }
                ko.applyBindings(AllRooms, document.getElementById("PropertiesDT"));

            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    }
};






//Model
function RoomClass(data) {
    
    var room = this;
    //  room.Room_Id = data["Room_Id"];
    room.Room_Name = ko.observable(data.Room_Name);


}


//function InitializePropertyDelete(data) {

//    var property = new function () { };

//    $.cookie("V_Id", "4");
//    property.PropId = data.Id;
//    property.Prop_Approved_By = $.cookie("V_Id");
//    return property;
//}
