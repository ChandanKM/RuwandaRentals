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
    Properties3: ko.observableArray([]),
    Properties5: ko.observableArray([]),
    Properties6: ko.observableArray([]),
    getUsers: function () {
    
        var self = this;
        $.ajax({
            type: "Post",
            url: "/api/consumer/PropertyListingDetails",
            data: { PropId: $.cookie("PropId1"), Room_Checkin: $.cookie("Room_Checkindetails"), Room_Checkout: $.cookie("Room_Checkoutdetails"), No_Of_Rooms: $.cookie("No_Of_Roomsdetails") },
            dataType: "json",
            success: function (response) {
               
                var prop = [];
                var prop1 = [];
                var prop2 = [];
                var prop3 = [];
               var prop5 = [];
               // var prop6 = [];
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
                for (i = 0; i < response[0].Table3.length; i++) {
                    prop3.push(response[0].Table3[i]);

                }
                for (i = 0; i < response[0].Table4.length; i++) {
                    prop5.push(response[0].Table4[i]);

                }
                //for (i = 0; i < response[0].Table6.length; i++) {
                //    prop6.push(response[0].Table6[i]);

                //}
                self.Properties(prop);
                self.Properties1(prop1);
                self.Properties2(prop2);
                self.Properties3(prop2);
             self.Properties5(prop5);
                //self.Properties6(prop6);
            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });

    },
 
  
  
};
self.ViewRoomDetails = function (Properties5) {
    
 //   alert(Properties.Prop_Id);
   
    window.location.href = '/Consumer/PropertyRoomDetails/'
};
//Model
function Properties(response) {
    
    this.Image_dir = ko.observable(response.Image_dir);
  
}

//Model

 