$(document).ready(function () {

    InitializevendorViewModel();
    $("#txtProp").autocomplete({

        source: function (request, response) {
            $('#txtProp').addClass('loadinggif');
            $.ajax({
                type: "GET",
                url: "/api/Property/PropertyAutoCompleteSearch",
                data: { terms: request.term },
                dataType: "json",
                cacheResults: true,
                contentType: "application/json; charset=utf-8",
                success: OnSuccess,
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });
            function OnSuccess(r) {
                response($.map(r, function (item) {
                    
                    return {
                        value: item.Prop_Name,
                        label: item.Prop_Name,
                        val: item.Prop_Id
                    }
                }))
            }
            $('#txtProp').removeClass('loadinggif');
        },
        select: function (e, i) {



            $("#txtRType").val(i.item.value);
            $("#txtPropIds").val(i.item.val);
            //  alert($("#txtPropIds").val())
            // $("#txtLocationBank").val(i.item.val);
        },
        minLength: 2
    });
});

function VendorViewModel() {

    

  //  this.PropLookup = GetPropLookup();
    this.SelectedProp = ko.observable("");
    this.ViewRooms = function () {
        ViewRooms();
    };
};



//Load PRoperties
function GetPropLookup() {
    var Props = ko.observableArray([{ PropId: 0, PropName: 'Select Property' }]);
    AppCommonScript.ShowWaitBlock();
    $.ajax({
        type: "GET",
        url: "/api/Booking/allproperty",
       
        dataType: "json",
        success: function (result) {
            ko.utils.arrayMap(result, function (item) {
                Props.push({ PropId: item.PropId, PropName: item.PropName });
            });
            AppCommonScript.HideWaitBlock();
        }
    });


    return Props;

}






function InitializevendorViewModel() {


    vendorViewModel = new VendorViewModel();
    ko.applyBindings(vendorViewModel, document.getElementById("modelSelectProperty"));
}

function ViewRooms() {
    if ($("#txtPropIds").val() == "") {
        var result = { Status: true, ReturnMessage: { ReturnMessage: "Please select a Property!!" }, ErrorType: "Success" };
        Failed(result);
    }
    else {
        $.localStorage("Property_Id", vendorViewModel.SelectedProp());
        window.location.href = '/SuperAdmin/RoomRateCal/' + $("#txtPropIds").val() + '';
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




