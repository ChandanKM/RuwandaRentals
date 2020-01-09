var urlPath = window.location.pathname;

$(function () {
   
    ko.applyBindings(UserListVM);
    UserListVM.getUsers();
});
//View Model
var UserListVM = {
 
    Properties: ko.observableArray([]),
    Properties1: ko.observableArray([]),
    Properties2: ko.observableArray([]),
    getUsers: function () {
       
        var self = this;
        $.ajax({
            type: "Post",
            url: "/api/consumer/PropertyListing",
            data: { CityMasterId: $.cookie("CityId1"), Room_Checkin: $.cookie("Room_Checkin"), Room_Checkout: $.cookie("Room_Checkout"), No_Of_Rooms: $.cookie("No_Of_Rooms") },
            dataType: "json",
            success: function (response) {
                
                var prop = [];
                var prop1 = [];
                var prop2 = [];
                if (response[0].Table2.length != 0)
                    {
                for (i = 0; i < response[0].Table.length; i++)
                    {
                    prop.push(response[0].Table[i]);
               
                }
                for (i = 0; i < response[0].Table1.length; i++) {
                    prop1.push(response[0].Table1[i]);

                }
                for (i = 0; i < response[0].Table2.length; i++) {
                    prop2.push(response[0].Table2[i]);

                }
                self.Properties(prop);
                self.Properties1(prop1);
                self.Properties2(prop2);
                }
            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });

    },
 
  
  
};
self.ViewDetails = function (Properties) {
    
    $.cookie("PropId1", Properties.PropId)
 
    $.cookie("Room_Checkindetails", $.cookie("Room_Checkin"))
    $.cookie("Room_Checkoutdetails", $.cookie("Room_Checkout"))
    $.cookie("No_Of_Roomsdetails", $.cookie("No_Of_Rooms"))
    alert(  $.cookie("No_Of_Roomsdetails"))
    window.location.href = '/Consumer/PropertyDetails/'
};
//Model
function Properties(response) {
    
    this.Image_dir = ko.observable(response.Image_dir);
  
}

//Model

 