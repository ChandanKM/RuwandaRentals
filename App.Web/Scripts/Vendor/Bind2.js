
var urlPath = window.location.pathname;
var vendorID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
$(function () {

    ko.applyBindings(VendorListVM);
    VendorListVM.getVendor();

});

//View Model
var VendorListVM = {


    Vendors: ko.observableArray([]),
    getVendor: function () {
        //
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/vendors/Bind',
            data: { Vndr_Id: vendorID },
            // data: { Vndr_Id: array[3] },
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                for (var i = 0; i < data.length; i++)
                    self.Vendors.push(new VendorClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    SaveVendor: function () {
        $.ajax({
            url: '/api/vendors/Edit',
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

self.editEdit = function (vendor) {
    
    window.location.href = 'api/vendors/Bind/' + vendor.vendorID;
};

//Model
function VendorClass(data) {
    
    var vendor = this;
    vendor.Vndr_Id = data["Vndr_Id"];
    vendor.Vendor_Name = data["Vendor_Name"];
    vendor.Vendor_Cinno = data["Vendor_Cinno"];
    vendor.Vendor_Address1 = data["Vendor_Address1"];
    //vendor.Vendor_Address2 = data["Vendor_Address2"];
    //vendor.Cityvalue = data["Cityvalue"];
    //vendor.locationvalue = data["locationvalue"];
    //vendor.pincodevalue = data["pincodevalue"];
    vendor.Vndr_Prop_Count = data["Vndr_Prop_Count"];
    vendor.GPS_Position = data["GPS_Position"];
    vendor.Overview = data["Overview"];
    vendor.Vendor_Cont_Name = data["Vendor_Cont_Name"];
    vendor.Vendor_Cont_Email = data["Vendor_Cont_Email"];
    vendor.Vendor_Cont_Phone = data["Vendor_Cont_Phone"];
    vendor.Vendor_Alt_Name = data["Vendor_Alt_Name"];
    vendor.Vendor_Alt_Email = data["Vendor_Alt_Email"];
    vendor.Vendor_Alt_Phone = data["Vendor_Alt_Phone"];

    //vendor.City_Id = data["SelectedCity"];
    //vendor.Location_Id = data["SelectedLocation"];
    //vendor.pincode_Id = data["SelectedPincode"];

}

