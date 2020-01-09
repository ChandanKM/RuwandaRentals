var urlPath = window.location.pathname;
$(function () {
   
    ko.applyBindings(LocationListVM);
    LocationListVM.getLocation();
});
//View Model
var LocationListVM = {
 
    Locations: ko.observableArray([]),
    getLocation: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/location/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
              
                for (var i = 0; i <=data.length;i++)
                    self.Locations.push(new LocationClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
};
self.editLocation = function (location) {
    
    window.location.href = 'Edit/' + location.Location_Id;
};

//Model
function LocationClass(data) {
    
    var location = this;
    location.Location_Id = data["Location_Id"];
    location.Location_desc = data["Location_desc"];
}