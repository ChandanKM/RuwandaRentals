$(document).ready(function () {

    InitializevendorViewModel();

});

function VendorViewModel() {

    

    this.PropLookup = GetPropLookup();
    this.SelectedProp = ko.observable("");
    this.ViewRooms = function () {
        ViewRooms();
    };
};



//Load PRoperties
function GetPropLookup() {
    var Props = ko.observableArray([{ PropId: 0, PropName: 'Select Property' }]);

    $.ajax({
        type: "GET",
        url: "/api/vendors/allproperty",
        data: { VendorId: $.localStorage("VendId") },
        dataType: "json",
        success: function (result) {
            ko.utils.arrayMap(result, function (item) {
                Props.push({ PropId: item.PropId, PropName: item.PropName });
            });
        }
    });


    return Props;

}






function InitializevendorViewModel() {


    vendorViewModel = new VendorViewModel();
    ko.applyBindings(vendorViewModel, document.getElementById("modelSelectProperty"));
}

function ViewRooms() {
    if (vendorViewModel.SelectedProp() == 0) {
        var result = { Status: true, ReturnMessage: { ReturnMessage: "Please select a Property!!" }, ErrorType: "Success" };
        Failed(result);
    }
    else {
     
        window.location.href = '/Vendor/UserProfile/5-' + vendorViewModel.SelectedProp()+'';
    }
}


function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}




