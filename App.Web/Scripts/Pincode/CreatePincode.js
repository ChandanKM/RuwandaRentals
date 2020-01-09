$(document).ready(function () {
    
    InitializeViewModel();
});

function PincodeViewModel() {
    
  
    this.Pincode = ko.observable("");
    
    this.CreatePincode = function () {
        CreatePincode();
    };
}

function InitializeViewModel() {

    pincodeViewModel = new PincodeViewModel();
    ko.applyBindings(pincodeViewModel);
}

function CreatePincode() {
    AppCommonScript.ShowWaitBlock();
    var pincode = new Initialize();
   
    $.ajax({
        type: "POST",
        url: "/api/pincode/create",
        data: $.parseJSON(ko.toJSON(pincode)),
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
    
    var pincode = new function () { };
  
    pincode.Pincode = pincodeViewModel.Pincode();
    
    return pincode;
}
