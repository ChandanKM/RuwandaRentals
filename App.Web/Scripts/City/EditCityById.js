/// <reference path="EditUserById.js" />
/// <reference path="EditUserById.js" />
/// <reference path="EditUserById.js" />

//var urlPath = window.location.pathname;
//var array = urlPath.split('/');


var urlPath = window.location.pathname;
var City_Id = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

$(function () {
  
    ko.applyBindings(CityListVM);
    CityListVM.getCities();
    
});
//View Model
var CityListVM = {
    
  
    City: ko.observableArray([]),
    getCities: function () {
        
       
        var self = this;
        
        $.ajax({
            type: "GET",
            url: '/api/city/Edit',
            data: { ID: City_Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
          
            success: function (data) {
              
                for (var i = 0; i < data.length; i++)
                    self.City.push(new CityClass(data[i]));
                    //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    SaveCity: function () {
        
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            url: '/api/city/Edit',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(this),
           
            contentType: 'application/json',
            success: function (result) {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(result);              
            },
            error: function (err) {
                if (err.responseText == "Creation Failed")
                {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);
                }
                else {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);
                   
                }
            },
            complete: function () {
               
            }
        });
    },

};

self.editCity = function (city) {
  
    window.location.href = '/api/city/Edit/' + city.City_Id;
};

//Model
function CityClass(data) {
  
    var city = this;
    
    city.City_Id = data["City_Id"];
    city.City_Name = data["City_Name"];
   
}

