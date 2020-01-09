$(document).ready(function () {
    
    InitializeViewModel();
});

function LocationViewModel() {
    
    
    this.Location_desc = ko.observable("");
    
    this.CreateLocation = function () {
        CreateLocation();
    };
}

function InitializeViewModel() {

    locationViewModel = new LocationViewModel();
    ko.applyBindings(locationViewModel);
}

function CreateLocation() {
    AppCommonScript.ShowWaitBlock();
    var location = new Initialize();
   
    $.ajax({
        type: "POST",
        url: "/api/location/create",
        data: $.parseJSON(ko.toJSON(location)),
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
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Initialize() {
    
    var location = new function () { };
  
    location.Location_desc = locationViewModel.Location_desc();
    
    return location;
}
