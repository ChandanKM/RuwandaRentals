var urlPath = window.location.pathname;

function NewProperty() {

    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response) {
            if (response == 0) {
                window.location.href = '../Vendor/Create';

            }
            else {
              
                $.localStorage("VendId", response)
                window.location.href = '/Vendor/ProprtyCreate';
            }
        },
        error: function (jqxhr) {

            alert(JSON.parse(jqxhr.responseText));
        }
    });
}

//View Model

var PropertyListVM = function () {
    Prop = this;
    

    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response) {


            $.localStorage("VendId", response)

        },
        error: function (jqxhr) {

            alert(JSON.parse(jqxhr.responseText));
        }
    });
    var vId = $.localStorage("VendId")
    Prop.getProperty = function () {
        
        var self = this;
        self.Properties = ko.observableArray([]),
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/property/Bind',
            contentType: "application/json; charset=utf-8",
            data: { VendId: vId },
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.Properties.push(new PropertyClass(data[i])); //Put the response in ObservableArray
                }
                ko.applyBindings(AllProperties, document.getElementById("PropertiesDT"));

                $(".PropertiesDT").DataTable({

                    "order": [[5, "desc"]],
                    responsive: true,
                    'iDisplayLength': 15,




                });
                $('#DataTables_Table_0_length').css('display', 'none')
                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);
                AppCommonScript.HideWaitBlock();
            }

        });
    }
    self.editProperty = function (Property) {
        $.cookie("PropertyImageDefault1", 'null');
        $.cookie("Image1", 'null')
        window.location.href = '/Vendor/PropertyEdit/' + Property.Prop_Id;
    };
    self.RoomDT = function (Property) {

        $.localStorage("RoomPropertyId", Property.Prop_Id);
        window.location.href = '/Vendor/RoomPage/' + Property.Prop_Id;
    };
    self.RateDT = function (Property) {
        
        $.localStorage("Property_Id", Property.Prop_Id);

        window.location.href = '/Vendor/RoomRateCalender/' + Property.Prop_Id;

    };
    self.Param = function (Property) {
       
        window.location.href = '/Vendor/Parameters/' + Property.Prop_Id;

    };
    self.PropManager = function (Property) {

        window.location.href = '/Vendor/UserProfile/4-' + Property.Prop_Id;

    };
    self.deleteProperty = function (Property) {

        if (confirm('Are you sure you want to delete this?')) {

            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/property/DeteteProperty',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(Property),

                contentType: 'application/json',
                success: function (result) {
                    window.location.href = "/Vendor/PropertyPage";

                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify("Record Deleted");
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
}




//Model
function PropertyClass(data) {
    var prop = this;

    prop.Prop_Id = data["Prop_Id"];
    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.City_Name = ko.observable(data.City_Name);
    prop.Location = ko.observable(data.Location);
    prop.Prop_Booking_MailId = ko.observable(data.Prop_Booking_MailId);
    prop.Prop_Booking_Mob = ko.observable(data.Prop_Booking_Mob);
    prop.PropertyCount = ko.observable(data.PropertyCount);
    $.cookie("V_Id", "4");
    prop.Prop_Approved_By = $.cookie("V_Id");
}

function InitializePropertyDelete(data) {

    var property = new function () { };

    $.cookie("V_Id", "4");

    property.PropId = data.Id;
    property.Prop_Approved_By = $.cookie("V_Id");
    return property;
}
$(document).ready(function () {

    AllProperties = new PropertyListVM();

    AllProperties.getProperty();
    $.ajax({
        type: "GET",
        url: '/Vendor/GetNoOfProperties',

        contentType: "application/json; charset=utf-8",
        // dataType: "json",
        async: false,
        success: function (data) {
            if (data == 2) {
               
                $('#NewProp').hide();
            }


        },
        error: function (err) {
        }
    });

});
