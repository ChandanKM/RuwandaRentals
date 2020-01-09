

var VendId;
var VendorListVM = {

    Vendors: ko.observableArray([]),

    GetVendor: function () {
        $.ajax({
            type: "POST",
            url: "/Vendor/GetLoginVendorId",
            dataType: "json",
            async:false,
            success: function (response) {
                
                $.localStorage("VendId", response);
                VendorListVM.OnSuccess(response);
               
            },
            error: function (jqxhr) {
                Failed(JSON.parse(jqxhr.responseText));
            }
        });
    },
    OnSuccess: function (Id) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/vendors/Bind',
            data: { ID: Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
               
             
                if (data.length == 0) {
                    window.location.href = '/Vendor/Create'
                }
                else {
                    for (var i = 0; i < data.length; i++)
                        VendorListVM.Vendors.push(new VendorClass(data[i]));
                }
            },
            error: function (err) {
            }
        });
     
        AppCommonScript.HideWaitBlock();
    }
};
self.editProperty = function (Vendors) {
    $.cookie("ImagePropertyd", 'null')
    window.location.href = '/vendor/Edit/' + Vendors.Vndr_Id;

};

self.deleteProperty = function () {

    if (confirm('Are you sure you want to delete this?')) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            url: '/api/vendors/Edit',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(this),
            contentType: 'application/json',
            success: function (result) {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(result);
            },
            error: function (err) {
                if (err.responseText == "Creation Failed") {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify("Creation Failed");
                }
                else {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);

                }
            },
            complete: function () {
            }
        });
    }
};

VendorListVM.GetVendor()
ko.applyBindings(VendorListVM, document.getElementById("divVendorProfile"))

//Model

function VendorClass(data) {
    var vendor = this;
    vendor.Vndr_Id = data["Vndr_Id"];
    vendor.Vndr_Name = ko.observable(data["Vndr_Name"]);
    vendor.Vndr_Cinno = ko.observable(data.Vndr_Cinno);
    vendor.Vndr_Addr1 = ko.observable(data.Vndr_Addr1);
    vendor.City_Id = data["City_Id"];
    vendor.City_Area = data["City_Area"];
    vendor.Pincode = data["Pincode"];
    vendor.State_Name = data["State_Name"];
    vendor.City_Name = data["City_Name"];
    vendor.Vndr_Prop_Count = ko.observable(data.Vndr_Prop_Count);
    vendor.Vndr_Gps_Pos = ko.observable(data.Vndr_Gps_Pos);
    vendor.Vndr_Overview = ko.observable(data.Vndr_Overview);
    vendor.Vndr_Mobile_Nos = ko.observable(data.Vndr_Mobile_Nos);
    vendor.Vndr_Lanline_Nos = ko.observable(data.Vndr_Lanline_Nos);
    vendor.Vndr_Contact_person = ko.observable(data.Vndr_Contact_person);
    vendor.Vndr_Contact_Email = ko.observable(data.Vndr_Contact_Email);
    vendor.Vndr_Contact_Nos = ko.observable(data.Vndr_Contact_Nos);
    vendor.Vndr_Alternate_person = ko.observable(data.Vndr_Alternate_person);
    vendor.Vndr_Alternate_Email = ko.observable(data.Vndr_Alternate_Email);
    vendor.Vndr_Alternate_Nos = ko.observable(data.Vndr_Alternate_Nos);
    vendor.Vndr_Contact_Mobile = data["Vndr_Contact_Mobile"];
    vendor.Vndr_Contact_Designation = data["Vndr_Contact_Designation"];
    vendor.Vndr_Alternate_Mobile = data["Vndr_Alternate_Mobile"];
    vendor.Vndr_Alternate_Designation = data["Vndr_Alternate_Designation"];
   
}
