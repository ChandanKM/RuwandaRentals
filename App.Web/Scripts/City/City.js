$(document).ready(function () {
    
    InitializecityViewModel();
});

function CityViewModel() {
    
  
    this.City_Name = ko.observable("");
    
    this.CreateCity = function () {
        CreateCity();
    };
}

//Load country



function InitializecityViewModel() {
    
    cityViewModel = new CityViewModel();
    ko.applyBindings(cityViewModel);
    
}

function CreateCity() {
    AppCommonScript.ShowWaitBlock();
    var city = new InitializeCity();
   
    $.ajax({
        type: "POST",
        url: "/api/city/create",
        data: $.parseJSON(ko.toJSON(city)),
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

function InitializeCity() {
    
    var city = new function () { };

    city.City_Name = cityViewModel.City_Name();
    return city;
}
