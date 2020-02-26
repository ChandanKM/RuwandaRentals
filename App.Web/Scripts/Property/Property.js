

$(function () {




    var urlPath = window.location.pathname;
    var RedirectAfterImage = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
    if (RedirectAfterImage == "RedirectView") {

        $('#Tab1').removeClass('active');
        $('#tab_1_1').removeClass('active');

        $('#Tab3').addClass('active');
        $('#tab_1_3').addClass('active');
        $("#Tab2").css("pointer-events", "none");

        $("#Tab1").css("pointer-events", "none");
        InitializepropertyViewModel();
    }

    else {
        InitializepropertyViewModel();
        $("#Tab2").css("pointer-events", "none");
        $("#Tab3").css("pointer-events", "none");
        $("#Tab4").css("pointer-events", "none");
        $("#Tab5").css("pointer-events", "none");
    }
    $("#txtPincode").autocomplete({

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
                        label: item.Pincode,
                        val: item.Id,
                        city: item.City,
                        state: item.State,
                        pine: item.Pincode,
                    }
                }))
            }
        },
        select: function (e, i) {
            $("#txtLocation").val(i.item.Locvalue);
            $("#hdnLocationId").val(i.item.val);
            $("#txtState").val(i.item.state);
            $("#txtPincode").val(i.item.pine);
            $("#txtCities").val(i.item.city);
            $("#txtsearch").val(i.item.Locvalue + ',' + i.item.city);
            $('#btnGMap').trigger('click');
        },
        minLength: 2
    });

    $("#txtLocation1").autocomplete({

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
            $("#txtLocation1").val(i.item.label);
            $("#hdnLocationId1").val(i.item.val);
            $("#txtState1").val(i.item.state);
            $("#txtPincode1").val(i.item.pine);
            $("#txtCities1").val(i.item.city);
            //$("#txtsearch1").val(i.item.Locvalue + ',' + i.item.city);
        },
        minLength: 2
    });
});
function BankCityType() {
    // 
    var RType = $("#txtDemoBank").val();
    $("#txtLocationBank").val(RType);
}
function PropCityType() {
    //  
    var RType = $("#txtDemo").val();
    $("#txtLocation").val(RType);
}
function phonenumber(inputtxt) {
    var phoneno = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (inputtxt.value.match(phoneno)) {
        return true;
    }
    else {
        alert("Not a valid Phone Number");
        return false;
    }
}

//var abc = '';
function InitializepropertyViewModel() {
    debugger;
    propertyViewModel = new PropertyViewModel();
    $.ajax({
        type: "GET",
        url: '/Vendor/GetNoOfProperties',

        contentType: "application/json; charset=utf-8",
        // dataType: "json",
        async: false,
        success: function (data)
        {
            debugger;
            if (data == 2) {
                alert('You have exceeded you property creation limit')
                //window.location.href = '/Vendor/PropertyPage'
            }


        },
        error: function (err) {
        }
    });
    $.ajax({
        type: "GET",
        url: '/api/vendors/Bind',
        data: { ID: $.localStorage("VendId") },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            //  alert(data[0].Vndr_Cinno)

            //  abc = 'dfd'// data[0].Vndr_Cinno
            //  alert(abc)
            if (data.length > 0) 
            //propertyViewModel.Prop_Cin_No = data[0].Vndr_Cinno
            // alert(data[0].Vndr_Cinno)
            //  PCIN = data[0].Vndr_Cinno
            //  $.localStorage("CINNUMBER", PCIN)
            // alert(abc)

        },
        error: function (err) {
        }
    });
    //propertyViewModel.GetCINNumber();
    // alert(abc)
    ko.applyBindings(propertyViewModel, document.getElementById("divPropertyKO"));
}



function ImageModel(data) {
    var self = this;
    data = data || {};
    self.Image_Id = ko.observable(data.Image_Id || '');
    self.Image_dir = ko.observable(data.Image_dir || '');
    self.Image_Name = ko.observable(data.Image_Name || '');
    self.Image_Remarks = ko.observable(data.Image_Remarks);
    self.Active_flag = ko.observable(data.Active_flag.trim() == 'true' ? true : false)
    self.Status = ko.observable(data.Active_flag.trim() == 'true' ? 'Active' : 'Suspended')
    self.Select = function (eRow) {
        UpdateImageFlag(eRow);
        eRow.Status(eRow.Active_flag() == true ? 'Suspended' : 'Active');
    }
}

function UpdateImageFlag(data) {
    $.ajax({
        type: "Get",
        url: "/api/Property/UpdateImageStatus",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Id: data.Image_Id(), flag: data.Active_flag() },
        success: function (response) {
            Successed(response)
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}
function PropertyViewModel() {
    debugger;
    this.Facilities = ko.observableArray([]);
    this.Policies = ko.observableArray([]);
    this.Images = ko.observableArray([]);
    this.ImagesList = ko.observableArray();
    this.EventImages = ko.observableArray([]);
    this.Prop_Name = ko.observable("");
    this.Prop_Cin_No = ko.observable(abc);
    this.Prop_Addr1 = ko.observable("");
    this.Prop_Addr2 = ko.observable("");
    this.Prop_Type = ko.observable("");
    this.Pricing_Type = ko.observable("");

    this.SelectedCity = ko.observable("");

    this.StarRatingLookup = ko.observableArray(['1', '2', '3', '4', '5']);
    this.SelectedStarRating = ko.observable("");

    this.FacilityTypeLookup = GetFacilityTypeLookup();
    this.SelectedFacilityType = ko.observable("");

    this.FacilityNameLookup = GetFacilityNameLookup();
    this.SelectedFacilityName = ko.observable("");
    this.Prop_GPS_Pos = ko.observable("");
    this.TripAdv = ko.observable("");
    this.State_Name = ko.observable("");
    this.Location_Name = ko.observable("");
    this.Pin_Code = ko.observable("");
    this.City_Name = ko.observable("");

    this.Prop_Booking_MailId = ko.observable("");
    this.Prop_Booking_Mob = ko.observable("");
    this.Prop_Pricing_MailId = ko.observable("");
    this.Prop_Pricing_Mob = ko.observable("");
    this.Prop_Inventory_MailId = ko.observable("");
    this.Prop_Inventory_Mob = ko.observable("");
    this.Bank_Name = ko.observable("");
    this.Bank_descr = ko.observable("");
    this.Bank_Branch_Name = ko.observable("");
    this.Bank_Branch_Code = ko.observable("");
    this.Bank_IFC_code = ko.observable("");
    this.Bank_Accnt_No = ko.observable("");
    this.Bank_Accnt_Name = ko.observable("");
    this.Event_Name = ko.observable("");
    this.Event_descr = ko.observable("");
    this.Event_Start = ko.observable("");
    this.Event_End = ko.observable("");
    this.City_Area = ko.observable("");
    this.Prop_Overview = ko.observable("");
    //  this.Image_dir = ko.observable($.cookie("PropertyImageDefault1") == 'null' || $.cookie("PropertyImageDefault1") == '' ? '/img/Vendor/Avtar.jpg' : $.cookie("PropertyImageDefault1"));
    this.Image_dir = ko.observable('/img/Propertyavtar.png');
    this.Image_Name = ko.observable("");
    this.Image_Id = ko.observable("");
    this.PropId = ko.observable("");
    this.Policy_Id = ko.observable("");
    this.Policy_Name = ko.observable("");
    this.Policy_descr = ko.observable("");
    this.Vend_Id = ko.observable("");
    this.Prop_Id = ko.observable("");
    this.Room_Checkins = ko.observable("12:00pm");
    this.Room_Checkouts = ko.observable("12:00pm");
    this.CreateProperty = function (tab)
    {
        debugger;
        CreateProperty();
    };

    this.CreateBank = function () {
        CreateBank();
    };



    //this.getPolicy = function () {
    //    getPolicy();
    //};
    // $.cookie('pId1', "126");
    this.GetImages = function () {
        $.ajax({
            type: "Get",
            url: "/api/Property/BindImage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: $.cookie('pId1') },
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    propertyViewModel.ImagesList.push(new ImageModel(data[i]));
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(propertyViewModel, document.getElementById("imageGallery"));
        });
    }
    this.ResetFormProperty = function () {
        this.Prop_Name("");

        this.Prop_Cin_No("");
        this.Prop_Addr1("");
        this.Prop_Addr2("");
        this.Prop_GPS_Pos("");
        this.Prop_Booking_MailId("");
        this.Prop_Booking_Mob("");
        this.Prop_Pricing_MailId("");
        this.Prop_Pricing_Mob("");
        this.Prop_Inventory_MailId("");
        this.Prop_Inventory_Mob("");

    }
    this.ResetFormBank = function () {
        this.Prop_Name("");

        this.Bank_Name("");
        this.Bank_descr("");
        this.Bank_Branch_Name("");
        this.Bank_Branch_Code("");
        this.Bank_IFC_code("");
        this.Bank_Accnt_No("");
        this.Bank_Accnt_Name("");

    }
    this.Cancel = function () {

        window.location.href = "/Vendor/PropertyPage"
    }

    //this.GetCINNumber = function () {
    //    $.ajax({
    //        type: "GET",
    //        url: '/api/vendors/Bind',
    //        data: { ID: Id },
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (data) {
    //            
    //        },
    //        error: function (err) {
    //        }
    //    });
    //    this.Prop_Cin_No('a');
    //}
}
function TripAdviser() {


    $.ajax({
        url: 'http://api.tripadvisor.com/api/partner/2.0/map/' + $('#lat').val() + ',' + $('#lon').val() + '?key=6e6c5fb839154bf3873229158cac5af7&q=' + $('#New_Prop_Name').val() + '',
        type: 'GET',
        dataType: 'json',
        success: function (data) {


            $('#lat').val(data.data[0].latitude)
            $('#lon').val(data.data[0].longitude)
            propertyViewModel.Prop_Name(data.data[0].name);
            propertyViewModel.TripAdv(data.data[0].api_detail_url)
            propertyViewModel.City_Name(data.data[0].address_obj.city);
            propertyViewModel.State_Name(data.data[0].address_obj.state);
            propertyViewModel.Location_Name(data.data[0].address_obj.street2);
            propertyViewModel.Prop_Addr1(data.data[0].address_obj.street1);

            propertyViewModel.Pin_Code(data.data[0].address_obj.postalcode.replace(/\s/g, ''));


            // WriteResponse(data);
        },
        error: function (x, y, z) {
            //  alert(x + '\n' + y + '\n' + z);
        }
    });
}
function CreateProperty() {
    debugger;
    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response)
        {
            debugger;
            $.localStorage("VendId", response)
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
   
  


    if (!$("input[name='rating']:checked").val()) {
        star = "not valid"
        //   alert('Please check hotel rating!'); return false;
    }
    else {
        var star = $("input[name='rating']:checked").val();
    }
    if (!$("input[name='Channel']:checked").val()) {
        //  alert('Nothing is checked!'); return false;
    }
    else {
        var Prop_Type_Value = $("input[name='Channel']:checked").val();

    }
    if (!$("input[name='Rate']:checked").val()) {
        //  alert('Nothing is checked!'); return false;
    }
    else {
        var Pricing_Type_Value = $("input[name='Rate']:checked").val();
        //alert(Pricing_Type_Value)
    }



    AppCommonScript.ShowWaitBlock();
    var property = new InitializeProperty();

    property.Location_Name = $('#txtLocation').val();
    property.State_Name = $('#txtState').val();
    property.City_Name = $('#txtCities').val();
    property.Pin_Code = $('#txtPincode').val();
    //comment for Image don't remove it
    property.Prop_GPS_Pos = '(' + $('#lat').val() + ',' + $('#lon').val() + ')';
    property.City_Id = $('#hdnLocationId').val();
    property.TripAdv = $('#txttripadv').val();
    property.Prop_Star_Rating = star;
    property.Prop_Type = Prop_Type_Value;
    property.Pricing_Type = Pricing_Type_Value;
    property.Vndr_Id = $.localStorage("VendId")
    $.ajax({
        type: "POST",
        url: "/api/property/create",
        data: $.parseJSON(ko.toJSON(property)),
        dataType: "json",
        success: function (response) {

            //   $.cookie("PropertyImageDefault1", 'null');
            $.localStorage('PropId', response);
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Property created Successfully" }, ErrorType: "Success" };
            Successed(result);
            //$.cookie("ImageProperty", "CreatedImage");
            $('#Tab1').removeClass('active');
            $('#tab_1_1').removeClass('active');
            $('#Tab2').addClass('active');
            $('#tab_1_2').addClass('active');
            $("#Tab1").css("pointer-events", "none");
            $("#Tab2").css("display", "block");
            window.scrollTo(0, 0);

            getFacility()

        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });

    //assign a Prperty Id for Image Upload
    $('#PropertyId').val($.localStorage('PropId'))
}

function CreateBank() {
    debugger;
    AppCommonScript.ShowWaitBlock();
    var bank = new InitializeBank();
    bank.City_Area = $('#txtLocationBank').val();
    bank.City_Id = $('#hdnLocationId1').val();
    $.ajax({
        type: "POST",
        url: "/api/property/createBank",
        data: $.parseJSON(ko.toJSON(bank)),
        dataType: "json",
        success: function (response) {
            //    

            $.cookie('pId1', response);
            Prop_Id = $.cookie('pId1');
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Bank detailssuccessfully created " }, ErrorType: "Success" };
            Successed(result);

            $('#Tab3').addClass('active');
            $('#tab_1_3').addClass('active');

            $("#Tab1").css("pointer-events", "none");
            $("#Tab2").css("pointer-events", "none");

            $("#Tab3").css("display", "block");
            $('#Tab2').removeClass('active');
            $('#tab_1_2').removeClass('active');
            window.scrollTo(0, 0);
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}


function CreateImage() {
    AppCommonScript.ShowWaitBlock();
    var image = new InitializeImages();

    $.ajax({
        type: "POST",
        url: "../Property/UploadMultiple",
        data: $.parseJSON(ko.toJSON(image)),
        dataType: "json",
        success: function (response) {


            var result = { Status: true, ReturnMessage: { ReturnMessage: "Successfully " }, ErrorType: "Success" };
            Successed(result);
            getFacility();
            $('#Tab4').removeClass('active');
            $('#tab_1_4').removeClass('active');

            $('#Tab5').addClass('active');
            $('#tab_1_5').addClass('active');


            //propertyViewModel.Facilities.push("121","Sas","ASasas");
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}
function getPolicy() {
    var self = this;

    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response) {
            $.localStorage("VendId", response)

        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
    var VendId = $.localStorage("VendId")
    var Prop_Id = $.localStorage('PropId')
    $.ajax({
        type: "GET",
        url: '/api/property/BindPolicy',
        data: { PropId: Prop_Id, VendId: VendId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //   alert(Prop_Id);
            propertyViewModel.Policies.removeAll();

            for (var i = 0; i < data.length; i++) {
                //find out in facilities whether data existi..
                propertyViewModel.Policies.push(new PolicyClass(data[i]));
            }//Put the response in ObservableArray
            ko.applyBindings(propertyViewModel.Policies, document.getElementById("PolicyDT"));

            $(".PolicyDT").DataTable({ responsive: true });

        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);

        }

    });

    this.deletePolicy = function () {
        if (confirm('Are you sure you want to delete this?')) {

            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/property/DeletePolicy',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(this),

                contentType: 'application/json',
                success: function (result) {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(result);
                    getPolicy();
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
    }
    this.EditPolicy = function (data) {
        propertyViewModel.Policy_Id(data.Policy_Id)
        propertyViewModel.Policy_Name(data.Policy_Name);

        propertyViewModel.Policy_descr(data.Policy_descr);

    }

}

ResetFormPolicy = function () {

    propertyViewModel.Policy_Name("");
    propertyViewModel.Policy_Id("");
    propertyViewModel.Policy_descr("");


}
function CreatePolicy() {
    AppCommonScript.ShowWaitBlock();
    //  
    var Policy = new InitializePolicy();
    $.ajax({
        type: "POST",
        url: "/api/property/createPolicy",
        data: $.parseJSON(ko.toJSON(Policy)),
        dataType: "json",
        success: function (response) {

            var result = { Status: true, ReturnMessage: { ReturnMessage: "Policy created Successfully " }, ErrorType: "Success" };
            Successed(result);
            //    window.location.reload()

            $('#myModalPolicy').click('hidden.bs.modal', function () {
                //window.alert('hidden event fired!');
            });
            ResetFormPolicy();
            //getFacility();
            window.location.href = 'PropertyEdit/' + ($.localStorage('PropId'));
            // getPolicy();

            $('#Tab1').removeClass('active');
            $('#tab_1_1').removeClass('active');
            $('#Tab2').removeClass('active');
            $('#tab_1_2').removeClass('active');


            $("#Tab2").css("pointer-events", "none");

            $("#Tab1").css("pointer-events", "none");
            $("#Tab3").css("pointer-events", "auto");
            $("#Tab4").css("pointer-events", "auto");
            $("#Tab5").css("pointer-events", "auto");
            AppCommonScript.HideWaitBlock();
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}

function getEvent() {

    var pId = $.cookie("pId1");

    Events = ko.observableArray([]);
    var self = this;
    $.ajax({
        type: "GET",

        url: '/api/property/BindEvent',
        data: { ID: pId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (data) {
            self.Events(data);
        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);

        }
    });

    this.deleteEvent = function () {
        if (confirm('Are you sure you want to delete this?')) {

            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/property/DeteteEvent',
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
    }
}

//Load country

function GetFacilityTypeLookup() {
    var facilitytypes = ko.observableArray([{ Facility_Type: 'Select Facility Type' }]);

    $.getJSON("/api/property/allfacilitytype", function (result2) {
        ko.utils.arrayMap(result2, function (item2) {
            facilitytypes.push({ Facility_Type: item2.Facility_Type })
        });
    });
    return facilitytypes;
}

function GetFacilityNameLookup() {
    var facilitynames = ko.observableArray([{ Facility_Name: 'Select Facility Name' }]);

    $.getJSON("/api/property/allfacilityname", function (result2) {
        ko.utils.arrayMap(result2, function (item2) {
            facilitynames.push({ Facility_Name: item2.Facility_Name })
        });
    });
    return facilitynames;
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}
this.Facilities = ko.observableArray([]);

this.getFacility = function () {
    var self = this;
    // alert($.localStorage('PropId'))
    $.ajax({
        type: "GET",
        url: '/api/property/BindFacility',
        contentType: "application/json; charset=utf-8",
        data: { prop_id: $.localStorage('PropId') },
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                propertyViewModel.Facilities.push(new FacilityClass(data[i])); //Put the response in ObservableArray
                $('.1').hide();
                $('.5').addClass('break');

            }

        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);

        }
    }).done(function () {
        ko.applyBindings(propertyViewModel, document.getElementById("tab4"));
    });
}


function InitializeProperty() {

    var property = new function () { };
    property.Prop_Name = propertyViewModel.Prop_Name();
    property.Prop_Cin_No = propertyViewModel.Prop_Cin_No;
    property.Prop_Addr1 = propertyViewModel.Prop_Addr1();
    property.Prop_Addr2 = propertyViewModel.Prop_Addr2();
    property.City_Id = propertyViewModel.SelectedCity();
    property.Prop_Star_Rating = propertyViewModel.SelectedStarRating();
    property.Prop_GPS_Pos = propertyViewModel.Prop_GPS_Pos();
    property.Prop_Booking_MailId = propertyViewModel.Prop_Booking_MailId();
    property.Prop_Booking_Mob = propertyViewModel.Prop_Booking_Mob();
    property.Prop_Pricing_MailId = propertyViewModel.Prop_Pricing_MailId();
    property.Prop_Pricing_Mob = propertyViewModel.Prop_Pricing_Mob();
    property.Prop_Inventory_MailId = propertyViewModel.Prop_Inventory_MailId();
    property.Prop_Inventory_Mob = propertyViewModel.Prop_Inventory_Mob();
    property.Vndr_Id = $.cookie("VendId");
    property.Image_dir = propertyViewModel.Image_dir();
    property.Pricing_Type = propertyViewModel.Pricing_Type();
    property.Prop_Type = propertyViewModel.Prop_Type();
    property.City_Area = propertyViewModel.City_Area();
    property.Prop_Overview = propertyViewModel.Prop_Overview();
    property.Room_Checkins = propertyViewModel.Room_Checkins();
    property.Room_Checkouts = propertyViewModel.Room_Checkouts();
    property.TripAdv = propertyViewModel.TripAdv();
    property.Location_Name = propertyViewModel.Location_Name();
    property.State_Name = propertyViewModel.State_Name();
    property.Pin_Code = propertyViewModel.Pin_Code();
    property.City_Name = propertyViewModel.City_Name();

    return property;
}

function InitializeBank() {

    var bank = new function () { };
    bank.Prop_Id = $.localStorage('PropId');
    bank.Bank_Name = propertyViewModel.Bank_Name();
    bank.Bank_descr = propertyViewModel.Bank_descr();
    bank.Bank_Branch_Name = propertyViewModel.Bank_Branch_Name();
    bank.Bank_Branch_Code = propertyViewModel.Bank_Branch_Code();
    bank.Bank_IFC_code = propertyViewModel.Bank_IFC_code();
    bank.City_Id = propertyViewModel.SelectedCity();
    bank.Bank_Accnt_No = propertyViewModel.Bank_Accnt_No();
    bank.Bank_Accnt_Name = propertyViewModel.Bank_Accnt_Name();
    bank.Vndr_Id = $.cookie("VendId");
    bank.City_Area = propertyViewModel.City_Area();
    return bank;
}

function InitializeFacility() {
    var facility = new function () { };
    facility.Prop_Id = Prop_Id;
    facility.Facility_Type = propertyViewModel.SelectedFacilityType();
    facility.Facility_Name = propertyViewModel.SelectedFacilityName();

    return facility
}

function InitializeImages() {
    var image = new function () { };
    //   
    image.Prop_Id = $.localStorage("PropId")
    image.Image_Name = propertyViewModel.Image_Name();
    image.Image_dir = propertyViewModel.Image_dir();

    return image
}

function ImageClass(data) {
    var image = this;
    image.Image_Id = data["Image_Id"];
    image.Image_dir = data["Image_dir"];
    image.Image_descr = data["Image_descr"];
    image.Image_Name = data["Image_Name"];


}
function PolicyClass(data) {
    var policy = this;
    policy.Policy_Id = data["Policy_Id"];
    policy.Policy_Name = data["Policy_Name"];
    policy.Policy_descr = data["Policy_descr"];



}
function InitializePolicy() {
    var policy = new function () { };
    var Vid = $.cookie("VendId");
    policy.Vndr_Id = Vid
    policy.Policy_Name = propertyViewModel.Policy_Name();
    policy.Prop_Id = $.localStorage("PropId")
    policy.Policy_Id = propertyViewModel.Policy_Id();
    policy.Policy_descr = propertyViewModel.Policy_descr();


    return policy
}



//vinod
function FacilityClass(data) {
    var Facility = this;


    Facility.Facility_Id = data["Facility_Id"];
    Facility.Prop_Id = ko.observable(data.Prop_Id);
    Facility.Facility_Name = ko.observable(data.Facility_Name);
    Facility.Facility_Type = ko.observable(data.Facility_Type);
    Facility.Facility_descr = data.Facility_descr;
    Facility.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
    Facility.Active_flag = ko.observable(data.Active_flag);
    Facility.Id = ko.observable(data.Id);
    Facility.IsHeader = ko.observable(data.IsHeader);
    Facility.FTypecount = ko.observable(data.FTypecount);
}

var AddFacility = {

    Active: function (data) {
        $.ajax({
            type: "POST",
            url: '/api/property/ActivateFacility',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            success: function (response) {

            },
            error: function (err) {
                // alert(err.status + " : " + err.statusText);
            }
        });
        var obj = new FacilityClass(data)
        obj.Active_flag('false');

    },

};


self.ActivateFacility = function (facility) {

    AddFacility.Active(facility)


};


function ValidateSpecialChar(e) {
    var k;
    document.all ? k = e.keyCode : k = e.which;
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
}
$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: "/Vendor/GetPagePermission",
        dataType: "json",

        success: function (response) {
            if (parseInt(response) == 0) {

                alert('Permission for create property is not enable For You. Please contact to Super Admin !!')
                window.location.href = '/Vendor/PropertyPage'

                //var result = { Status: true, ReturnMessage: { ReturnMessage: 'Permission for create property is not enable For You. Please contact to Super Admin !!' }, ErrorType: "Error" };
                //Failed(result);
                //return;
            }
        }

    });
});
