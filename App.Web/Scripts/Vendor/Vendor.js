$(document).ready(function () {
    
    $('#imgVendor').attr("src", "/img/Vendor/Avtar.jpg");
    InitializevendorViewModel();
    $("#txtLocation").autocomplete({

        source: function (request, response) {
            $.ajax({
                type: "GET",
                url: "/api/Property/GetAutoCompleteSearch",
                data: { terms: request.term },
                dataType: "json",
                cacheResults: true,
                contentType: "application/json; charset=utf-8",
                success: OnSuccess,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    // alert(textStatus);
                }
            });
            function OnSuccess(r) {
                response($.map(r, function (item) {
                    return {
                        Locvalue: item.Location,
                        label: item.Location,
                        val: item.Id,
                        city: item.City,
                        state: item.State,
                        pine: item.Pincode,
                    }
                }))
            }
        },
        select: function (e, i) {
            $("#txtLocation").val(i.item.label);
            $("#hdnLocationId").val(i.item.val);
            $("#txtState").val(i.item.state);
            $("#txtPincode").val(i.item.pine);
            $("#txtCities").val(i.item.city);
            $("#txtsearch").val(i.item.Locvalue + ',' + i.item.city);
         
        },
        minLength: 2
    });
});

function VendorViewModel() {
    var self = this;
    self.Vndr_Lanline_Nos = ko.observable("");
    self.Vndr_Mobile_Nos = ko.observable("");
    self.Vndr_Name = ko.observable("");
    self.Image_dir = ko.observable("/img/Vendor/Avtar.jpg");
    self.Vndr_Cinno = ko.observable("");
    self.Vndr_Addr1 = ko.observable("");

    self.Cityvalue = ko.observable("");
    self.locationvalue = ko.observable("");
    self.pincodevalue = ko.observable("");

    self.Vndr_Gps_Pos = ko.observable("");
    self.Vndr_Overview = ko.observable("");

    self.Vndr_Contact_person = ko.observable("");
    self.Vndr_Contact_Email = ko.observable("");
    self.Vndr_Contact_Nos = ko.observable("");
    self.Vndr_Contact_Mobile = ko.observable("");
    self.Vndr_Contact_Designation = ko.observable("");
    self.Vndr_Alternate_person = ko.observable("");
    self.Vndr_Alternate_Email = ko.observable("");
    self.Vndr_Alternate_Nos = ko.observable("");
    self.Vndr_Alternate_Mobile = ko.observable("");
    self.Vndr_Alternate_Designation = ko.observable("");
    //self.CityLookup = GetCityLookup();
    self.SelectedCity = ko.observable("");
    self.City_Area = ko.observable("");
    self.UserProfile_Id = ko.observable("");


    self.CreateVendor = function () {
        CreateVendor();
    };
}




//Load country
//function GetCityLookup() {
//    var cities = ko.observableArray([{ City_Id: 0, CityName: 'Select City' }]);

//    $.getJSON("/api/property/allcities", function (result) {
//        ko.utils.arrayMap(result, function (item) {
//            cities.push({ City_Id: item.City_Id, CityName: item.CityName });
//        });
//    });
//    return cities;
//}







function InitializevendorViewModel() {
    if ($.cookie("ProfileImage") == "Default") {

    }
    else {
        $('#imgVendor').attr("src", $.cookie("ProfileImage"));
    }
    vendorViewModel = new VendorViewModel();
    ko.applyBindings(vendorViewModel,document.getElementById("divVendorCreateProfile"));
}

function CreateVendor() {
    var vendor = new InitializeVendor();
    vendor.Image_dir = '/img/Vendor/Avtar.jpg';
    vendor.City_Area = $("#txtLocation").val();
    vendor.City_Id = $('#hdnLocationId').val();
    vendor.UserProfile_Id = $.cookie("userprofileId");
  //  AppCommonScript.ShowWaitBlock();
    $.ajax({
        type: "POST",
        url: "/api/vendors/create",
        data: $.parseJSON(ko.toJSON(vendor)),
        dataType: "json",
        success: function (response) {
            $.cookie("ProfileImage", 'null');
            $.cookie("EditProfileImage", 'null');
            Successed(window.location.href = '/vendor/Bind/');

        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}


function ClearVendor() {
    AppCommonScript.ShowWaitBlock();
    var vendor = new InitializeVendor();
    $.ajax({
        dataType: "json",
        url: "/api/vendors/create",
        data: $(document).ready(function () { $('input[type=text]').each(function () { $(this).val(''); }); }),
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




function InitializeVendor() {
    //  
    var vendor = new function () { };


    vendor.Vndr_Name = vendorViewModel.Vndr_Name();
    vendor.Image_dir = vendorViewModel.Image_dir();

    vendor.Vndr_Cinno = vendorViewModel.Vndr_Cinno();
    vendor.Vndr_Addr1 = vendorViewModel.Vndr_Addr1();
    vendor.Vndr_Gps_Pos = vendorViewModel.Vndr_Gps_Pos();
    vendor.Vndr_Overview = vendorViewModel.Vndr_Overview();

    vendor.Vndr_Lanline_Nos = vendorViewModel.Vndr_Lanline_Nos();
    vendor.Vndr_Mobile_Nos = vendorViewModel.Vndr_Mobile_Nos();
    vendor.Vndr_Contact_person = vendorViewModel.Vndr_Contact_person();
    vendor.Vndr_Contact_Email = vendorViewModel.Vndr_Contact_Email();
    vendor.Vndr_Contact_Nos = vendorViewModel.Vndr_Contact_Nos();
    vendor.Vndr_Alternate_person = vendorViewModel.Vndr_Alternate_person();
    vendor.Vndr_Alternate_Email = vendorViewModel.Vndr_Alternate_Email();
    vendor.Vndr_Alternate_Nos = vendorViewModel.Vndr_Alternate_Nos();
    vendor.Vndr_Alternate_Mobile = vendorViewModel.Vndr_Alternate_Mobile();
    vendor.Vndr_Alternate_Designation = vendorViewModel.Vndr_Alternate_Designation();
    vendor.Vndr_Contact_Mobile = vendorViewModel.Vndr_Alternate_Mobile();
    vendor.Vndr_Contact_Designation = vendorViewModel.Vndr_Alternate_Designation();
    vendor.City_Id = vendorViewModel.SelectedCity();
    vendor.City_Area = vendorViewModel.City_Area();
    vendor.UserProfile_Id = vendorViewModel.UserProfile_Id();

    return vendor;
}
