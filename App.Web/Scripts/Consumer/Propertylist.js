$(document).ready(function () {
    
    InitializeuserViewModel();
});

function PropertyViewModel1() {
    
  
    this.CityLookup = GetCityLookup();
  //  this.LocationLookup = GetLocationLookup();
   // this.pincode_Id = ko.observable("");
 this.SelectedCity = ko.observable();
    this.Room_Checkin = ko.observable();
    this.Room_Checkout = ko.observable();
    this.No_Of_Rooms = ko.observable();
    this.CreateUser = function () {
        CreateUser();
    };
}

//Load country



function InitializeuserViewModel() {

    propViewModel = new PropertyViewModel1();
    ko.applyBindings(propViewModel);
}
function GetCityLookup() {
    var cities = ko.observableArray([{ CityId: 0, CityName: 'Select City' }]);

    $.getJSON("/api/consumer/allcities", function (result) {
        ko.utils.arrayMap(result, function (item) {
            cities.push({ CityId: item.CityId, CityName: item.CityName });
        });
    });
    return cities;
}

function CreateUser() {
   AppCommonScript.ShowWaitBlock();
   var user = new InitializeUser();
   
   alert(user.CityId)
   $.cookie("CityId1", user.CityId);
   $.cookie("Room_Checkin", user.Room_Checkin);
   $.cookie("Room_Checkout", user.Room_Checkout);
   $.cookie("No_Of_Rooms", user.No_Of_Rooms);
   window.location.href = "/Consumer/Listing";
    //$.ajax({
    //    type: "POST",
    //    url: "/api/consumer/PropertyListing",
    //    data: $.parseJSON(ko.toJSON(user)),
    //    dataType: "json",
    //    success: function (response) {
    //        

    //        Successed(response);
        
         
    //    },
    //    error: function (jqxhr) {
           
    //        Failed(JSON.parse(jqxhr.responseText));
    //    }
    //});
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
    //window.location.href = 'User/Bind/';
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function InitializeUser() {
    
    var user = new function () { };
  
    user.CityId = propViewModel.SelectedCity();
    user.Room_Checkin = propViewModel.Room_Checkin();
    user.Room_Checkout = propViewModel.Room_Checkout();
    user.No_Of_Rooms = propViewModel.No_Of_Rooms();
    
    return user;
}
