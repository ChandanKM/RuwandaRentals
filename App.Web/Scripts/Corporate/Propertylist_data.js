$(document).ready(function () {
    
    InitializeuserViewModel();
});

function PropertyViewModel() {
    
  
    this.CityLookup = GetCityLookup();
    this.LocationLookup = GetLocationLookup();
    this.pincode_Id = ko.observable("");
    this.SelectedCity = ko.observable();
    this.SelectedLocation = ko.observable();
    this.Room_Checkin = ko.observable("");
    this.Room_Checkout = ko.observable("");
    this.No_Of_Rooms = ko.observable("");
    this.CreateUser = function () {
        CreateUser();
    };
}

//Load country



function InitializeuserViewModel() {

    propViewModel = new PropertyViewModel();
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

function GetLocationLookup() {
    var locations = ko.observableArray([{ LocationId: 0, LocationName: 'Select Location' }]);

    $.getJSON("/api/consumer/alllocations", function (result1) {
        ko.utils.arrayMap(result1, function (items) {
            locations.push({ LocationId: items.LocationId, LocationName: items.LocationName })
        });
    });
    return locations;
}
function CreateUser() {
   //AppCommonScript.ShowWaitBlock();
    var user = new InitializeUser();

   
    $.ajax({
        type: "Post",
        url: "/api/consumer/PropertyListing",
        data: { CityId: $.cookie("CityId1"), LocationId: $.cookie("LocationId"), Room_Checkin: $.cookie("Room_Checkin"), Room_Checkout: $.cookie("Room_Checkout"), No_Of_Rooms: $.cookie("No_Of_Rooms") },
        dataType: "json",
        success: function (response) {
            

            Successed(response);
        
         
        },
        error: function (jqxhr) {
           
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
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
    user.LocationId = propViewModel.SelectedLocation();
    user.Room_Checkin = propViewModel.Room_Checkin();
    user.Room_Checkout = propViewModel.Room_Checkout();
    user.No_Of_Rooms = propViewModel.No_Of_Rooms();
    return user;
}
