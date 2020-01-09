var urlPath = window.location.pathname;
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
            url: '/api/pincode/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
              
                for (var i = 0; i <=data.length;i++)
                    self.Pincodes.push(new PincodeClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
};
self.editPincode = function (pincode) {
    window.location.href = 'Edit/' + pincode.PincodeId;
};

//Model
function PincodeClass(data) {
    var pincode = this;
    pincode.PincodeId = data["PincodeId"];
    pincode.Pincode = data["Pincode"];
}


