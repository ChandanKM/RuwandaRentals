
var VendId = $.localStorage("VendId")

$(function () {
 
    VendorListVM.getVendor();
    ko.applyBindings(VendorListVM, document.getElementById("divVendorEditProfile"));

});
//View Model
var VendorListVM = {

    Vendors: ko.observableArray([]),
    getVendor: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/vendors/Edit',
            data: { ID: VendId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.Vendors.push(new VendorClass(data[i])); //Put the response in ObservableArray
                }
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        }).done(function () {
          

        });

    },
};





function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}
//Model
function VendorClass(data) {

    var vendor = this;
    vendor.Vndr_Id = data["Vndr_Id"];
    vendor.Vndr_Name = data["Vndr_Name"];
    vendor.Vndr_Cinno = data["Vndr_Cinno"];
    vendor.Vndr_Addr1 = data["Vndr_Addr1"];

    vendor.Vndr_Gps_Pos = data["Vndr_Gps_Pos"];
    vendor.Vndr_Overview = data["Vndr_Overview"];
    vendor.Vndr_Contact_person = data["Vndr_Contact_person"];
    vendor.Vndr_Contact_Email = data["Vndr_Contact_Email"];
    vendor.Vndr_Contact_Nos = data["Vndr_Contact_Nos"];
    vendor.Vndr_Contact_Mobile = data["Vndr_Contact_Mobile"];
    vendor.Vndr_Contact_Designation = data["Vndr_Contact_Designation"];
    vendor.Vndr_Alternate_person = data["Vndr_Alternate_person"];
    vendor.Vndr_Alternate_Nos = data["Vndr_Alternate_Nos"];
    vendor.Vndr_Alternate_Mobile = data["Vndr_Alternate_Mobile"];
    vendor.Vndr_Alternate_Designation = data["Vndr_Alternate_Designation"];
    vendor.Vndr_Alternate_Email = data["Vndr_Alternate_Email"];

    vendor.City_Id = data["City_Id"];
    vendor.City_Area = data["City_Area"];
    vendor.Pincode = data["Pincode"];
    vendor.State_Name = data["State_Name"];
    vendor.City_Name = data["City_Name"];

    vendor.Image_dir = ko.observable();

    vendor.Image_dir = "/img/Vendor/Avtar.jpg";

  //  vendor.CityLookup = GetCityLookup();
   
    vendor.City_Id = data["City_Id"];

    vendor.Edit = function (data) {
        data.City_Area = $('#txtLocation').val();
        data.City_Id = $('#hdnLocationId').val();
        if (confirm('Are you sure you want to Update this?')) {

            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/vendors/EditVendor',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(data),

                contentType: 'application/json',
                success: function (result) {
                    alert('Profile updated Successfully')
                    var results = { Status: true, ReturnMessage: { ReturnMessage: "Profile updated Successfully" }, ErrorType: "Success" };

                    AppCommonScript.HideWaitBlock();

                    window.location.href = '/vendor/Bind'
                },
                error: function (err) {

                    if (err.responseText == "Editing Failed") {
                        Failed(JSON.parse(err.responseText));
                    }
                    else {
                        Failed(JSON.parse(err.responseText));
                    }
                },

            });
        }
    };
}
function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}
function GetCityLookup() {
    var cities = ko.observableArray([{ City_Id: 0, CityName: 'Select City' }]);

    $.getJSON("/api/property/allcities", function (result) {
        ko.utils.arrayMap(result, function (item) {
            cities.push({ City_Id: item.City_Id, CityName: item.CityName });
        });
    });
    return cities;
}