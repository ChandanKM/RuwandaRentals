var urlPath = window.location.pathname;
var array = urlPath.split('/');
$(function () {
  
    ko.applyBindings(PincodeListVM);
    PincodeListVM.getPincode();
    
});
//View Model
var PincodeListVM = {
    
  
    Pincodes: ko.observableArray([]),
    getPincode: function () {
       
        var self = this;
        
        $.ajax({
            type: "GET",
            url: '/api/pincode/Edit',
            data: { Id: array[3] },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
          
            success: function (data) {
              
                for (var i = 0; i < data.length; i++)
                    self.Pincodes.push(new PincodeClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    
    SavePincode: function () {
        $.ajax({
            url: '/api/pincode/EditPincode',
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

self.editPincode = function (pincode) {
  
    window.location.href = '/api/pincode/Edit/' + pincode.Pincode_Id;
};

//Model
function PincodeClass(data) {
  
    var pincode = this;
    pincode.PincodeId = data["PincodeId"];
    pincode.Pincode = data["Pincode"];

}

