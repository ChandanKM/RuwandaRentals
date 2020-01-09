var urlPath = window.location.pathname;
var array = urlPath.split('/');
$(function () {
  
    ko.applyBindings(LocationListVM);
    LocationListVM.getlocation();
    
});
//View Model
var LocationListVM = {
    
  
    Locations: ko.observableArray([]),
    getlocation: function () {
        var self = this;
        
        $.ajax({
            type: "GET",
            url: '/api/Location/Edit',
            data: { Location_Id: array[3] },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            
            success: function (data) {
                for (var i = 0; i < data.length; i++)
                    self.Locations.push(new LocationClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    
    SaveLocation: function () {
        $.ajax({
            url: '/api/location/EditLocation',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(this),
           
            contentType: 'application/json',
            success: function (result) {
              
            },
            error: function (err) {
                if (err.responseText == "Creation Failed")
                { }
                else {
                    alert("Status:" + err.responseText);
                   
                }
            },
            complete: function () {
                alert('User Updated');
            }
        });
    },

};

self.editLocation = function (location) {
  
    window.location.href = '/api/location/Edit/' + location.Location_Id;
};

//Model
function LocationClass(data) {
  
    var location = this;
    location.Location_Id = data["Location_Id"];
    location.Location_desc = data["Location_desc"];

}

