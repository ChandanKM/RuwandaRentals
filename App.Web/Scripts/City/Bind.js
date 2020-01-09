var urlPath = window.location.pathname;
$(function () {
   
    ko.applyBindings(CityListVM);
    CityListVM.getCities();
});
//View Model
var CityListVM = {
 
    Cities: ko.observableArray([]),
    getCities: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/city/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i <= data.length; i++)
                    self.Cities(data);
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },

   
};
self.editCity = function (city) {
    
    window.location.href = 'Edit/' + city.City_Id;
};


function CityClass(data) {
    var city = this;

    city.City_Id = data["City_Id"];
    city.City_Name = data["City_Name"];

    //
    //this.City_Id = ko.observable(data.City_Id);
    //this.City_Name = ko.observable(data.City_Name);
}
  
