var urlPath = window.location.pathname;

$(function () {
   
    ko.applyBindings(UserListVM);
    UserListVM.getUsers();
});
//View Model
var UserListVM = {
 
    Properties: ko.observableArray([]),
   
    getUsers: function () {
        var self = this;
        $.ajax({
            type: "Post",
            url: "/api/consumer/Room_Listing",
            data: { PropId: $.cookie("PropId1"), Room_Checkin: $.cookie("Room_Checkindetails"), Room_Checkout: $.cookie("Room_Checkoutdetails"), No_Of_Rooms: $.cookie("No_Of_Roomsdetails") },
            dataType: "json",
            success: function (response) {
                
                var prop = [];
                var prop1 = [];
                var prop2 = [];
                for (i = 0; i < response[0].Table.length; i++)
                    {
                    prop.push(response[0].Table[i]);
               
                }
              
               
            
               
                self.Properties(prop);
             
              
            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });

    },
 
  
  
};

//Model
function Properties(response) {
    
    this.Image_dir = ko.observable(response.Image_dir);
  
}

//Model

 