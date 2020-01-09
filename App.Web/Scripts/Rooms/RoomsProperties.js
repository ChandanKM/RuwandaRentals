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
        type: "Get",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response) {
            $.localStorage("VendId", response)


        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
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
        //$.cookie("Property_Id", vendorViewModel.SelectedProp());
        $.localStorage("RoomPropertyId", vendorViewModel.SelectedProp());

        window.location.href = '/Vendor/RoomPage/' + vendorViewModel.SelectedProp()+'';
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




