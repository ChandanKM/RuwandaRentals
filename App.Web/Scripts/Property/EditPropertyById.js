var windowURL = window.URL || window.webkitURL;  // Custom filehandler function

var urlPath = window.location.pathname;
var propID = urlPath.substring(urlPath.lastIndexOf("/") +1, urlPath.length);


$.cookie('pId', propID);
//$.cookie("V_Id", "4");
var Vndr_Id = $.cookie("VendId");


$(document).ready(function () {

    $('#PropertyId').val($.cookie('pId'))
    InitializepropertyViewModel();


});
function TripAdviser() {
    
  
    $.ajax({
        url: 'http://api.tripadvisor.com/api/partner/2.0/map/' + $('#lat').val() + ',' + $('#lon').val() + '?key=6e6c5fb839154bf3873229158cac5af7&q=' + $('#New_Prop_Name').val() + '',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            
            $('#txtCities').val(data.data[0].address_obj.city);
            $('#txtState').val(data.data[0].address_obj.state);
            $('#txtLocation').val(data.data[0].address_obj.street2);
            $('#Prop_Addr1').val(data.data[0].address_obj.street1);
            $('#New_Prop_Name').val(data.data[0].name);
          

            
            $('#lat').val(data.data[0].latitude)
            $('#lon').val(data.data[0].longitude)
            
           
          
            $('#txttripadv').val(data.data[0].api_detail_url);
            $('#txtPincode').val(data.data[0].address_obj.postalcode.replace(/\s/g, ''));
            // WriteResponse(data);
        },
        error: function (x, y, z) {
            //  alert(x + '\n' + y + '\n' + z);
        }
    });
}
function BankCityType() {

    var RType = $("#txtDemoBank").val();
    $("#txtLocationBank").val(RType);
}
function PropCityType() {

    var RType = $("#txtDemo").val();
    $("#txtLocation").val(RType);
}

// Custom filehandler function

ko.bindingHandlers.files = {
    init: function (element, valueAccessor) {
        var value = ko.unwrap(valueAccessor());
        $(element).change(function () {
            var file = this.files[0];
            if (ko.isObservable(valueAccessor())) {
                valueAccessor()(file);
            }
        });
    },

    update: function (element, valueAccessor, allBindingsAccessor) {

        var file = ko.utils.unwrapObservable(valueAccessor());
        var bindings = allBindingsAccessor();

        if (bindings.fileObjectURL && ko.isObservable(bindings.fileObjectURL)) {
            var oldUrl = bindings.fileObjectURL();
            if (oldUrl) {
                windowURL.revokeObjectURL(oldUrl);
            }
            bindings.fileObjectURL(file && windowURL.createObjectURL(file));
        }

        if (bindings.fileBinaryData && ko.isObservable(bindings.fileBinaryData)) {
            if (!file) {
                bindings.fileBinaryData(null);
            } else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    bindings.fileBinaryData(e.target.result);
                };
                reader.readAsArrayBuffer(file);
            }
        }
    }
};

function InitializepropertyViewModel() {
    //if ($.cookie("PropertyImage") == "CreatedImage") {
    //    $('#imgVendor').attr("src", "/img/Vendor/Avtar.jpg")
    //}
    //else if ($.cookie("PropertyImage") == "") {
    //    property.Image_dir = "/img/Vendor/Avtar.jpg"
    //}
    //else {
    //    $('#imgVendor').attr("src", $.cookie("PropertyImage"));
    //}

    propertyViewModel = new PropertyViewModel();
    propertyViewModel.getProperty();
    propertyViewModel.getBankDetails();
    propertyViewModel.getPolicy();
    propertyViewModel.getFacility();
    propertyViewModel.GetImages();
    //propertyViewModel.getEvent();
    // propertyViewModel.getImage();
    // ko.applyBindings(propertyViewModel, document.getElementById("divPropertyKO"));

    // viewModel.chosenCountries.push('France');

}
function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}
function PropertyViewModel() {
    this.ImagesList = ko.observableArray();
    this.Facilities = ko.observableArray([]);
    this.Images = ko.observableArray([]);
    this.Policies = ko.observableArray([]);

    this.getBankDetails = function () {
        getBankDetails();
    };

    this.getProperty = function () {
        getProperty();
    };
    this.getPolicy = function () {
        getPolicy();
    };

 //   this.CityLookup = GetCityLookup();

    this.City_Id = ko.observable("");

    this.Prop_Star_Rating = ko.observable("");

    //this.Image_dir = ko.observable();
    this.Image_Id = ko.observable("");
    this.Prop_Id = ko.observable("");
    this.Policy_Id = ko.observable("");
    this.Policy_Name = ko.observable("");
    this.Policy_descr = ko.observable("");
    this.Policy_descrEdit = ko.observable("");
    this.Vend_Id = ko.observable("");
    //this.Prop_Id = ko.observable("");
    this.CreateProperty = function () {
        CreateProperty();
    };

    this.CreateBank = function () {
        CreateBank();
    };

    this.GetImages = function () {
        $.ajax({
            type: "Get",
            url: "/api/Property/BindImage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: $.cookie('pId') },
            success: function (data) {
                propertyViewModel.ImagesList.removeAll();
                for (var i = 0; i < data.length; i++) {
                    propertyViewModel.ImagesList.push(new ImageModel(data[i]));
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(propertyViewModel, document.getElementById("imageGallery"));
            
            $('#map_Updates').trigger('click');
        });
    }

    this.Facilities = ko.observableArray([]);

    this.getFacility = function () {

        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/property/BindFacility',
            contentType: "application/json; charset=utf-8",
            data: { prop_id: propID },
            dataType: "json",
            success: function (data) {
                propertyViewModel.Facilities.removeAll();
                ko.cleanNode(propertyViewModel.Facilities, document.getElementById("tab_1_4"));
                for (var i = 0; i < data.length; i++) {
                    propertyViewModel.Facilities.push(new FacilityClass(data[i])); //Put the response in ObservableArray


                }

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }
        }).done(function () {
            ko.applyBindings(propertyViewModel, document.getElementById("tab4"));
            $('.1').hide();
            $('.5').remove();
        });
    }



}
this.Cancel = function () {
    
    window.history.back();
    //window.location.href = "/Property/Bind"
}

// knockout ImageBinding 
var uploadModel = new UploadImageModel();
ko.applyBindings(uploadModel, document.getElementById("modelSelectImage"));

function UploadImageModel() {
    var self = this;
    self.imageFile = ko.observable();
    self.imageObjectURL = ko.observable();
    self.imageBinary = ko.observable();
    self.slotModel = function () {
        var that = {};
        that.firstBytes = ko.computed(function () {
            if (self.imageBinary()) {
                var buf = new Uint8Array(self.imageBinary());
                var bytes = [];
                for (var i = 0; i < buf.length ; ++i) {
                    bytes.push(buf[i]);
                }
                return bytes;
            } else {
                return '';
            }
        }, that);

        return that;
    };
    self.images = ko.observableArray([self.slotModel()]);

    self.SubmitImage = function () {


        var imgHeight = document.getElementById('imgAdd').naturalHeight;
        var imgWidth = document.getElementById('imgAdd').naturalWidth;

        if (imgWidth > 600 & imgHeight > 300) {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Your image is too big, it must be less than 600*300" }, ErrorType: "success" };
            Failed(result);

            self.CancelUpload();
        }
        else {
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: "/Vendor/PropertyImageUpload",
                type: "POST",
                data: {
                    file: uploadModel.images._latestValue[0].firstBytes(),
                    PropertyId: propID
                },
                traditional: true,
                success: function (response) {
                    self.CancelUpload();
                    AppCommonScript.HideWaitBlock();
                    propertyViewModel.GetImages();

                },
                error: function (err) {
                    Failed(JSON.parse(err.responseText));
                }
            });
        }
    }

    self.CancelUpload = function () {
        self.imageFile('');
        self.imageObjectURL('');
        self.imageBinary('');
        $('#imgAdd').attr('src', '');
        $('#imgUploadNew').val('')
    }
}




function getProperty() {
    Properties = ko.observableArray([]);
    AppCommonScript.ShowWaitBlock();
    var self = this;
    $.ajax({
        type: "GET",
        url: '/api/property/Edit',
        data: { Id: propID },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                self.Properties.push(new UserClass(data[i])); //Put the response in ObservableArray

            }
             
            $('input[name=rating][value=' + data[0].Prop_Star_Rating + ']').prop('checked', true)
            //$('input[name=Channel][value=' + data[0].Prop_Type + ']').prop('checked', true)
            $('input[name=Rate][value=' + data[0].Pricing_Type + ']').prop('checked', true)


        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);

        }
    }).done(function () {
        ko.applyBindings(propertyViewModel, document.getElementById("tab_1_1"));
        $('#map_Updates').trigger('click');
        AppCommonScript.HideWaitBlock();
    });

    this.CreateProperty = function (data) {
       
        AppCommonScript.ShowWaitBlock();

       

        // data.Image_dir = $.cookie("ImagePropertyd");
        if (!$("input[name='rating']:checked").val()) {
         alert('Rating is checked!'); return false;
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

        }
        data.Prop_Type = Prop_Type_Value
       
        data.Pricing_Type = Pricing_Type_Value
        
        data.Prop_Star_Rating = star;
        data.Prop_GPS_Pos = $('#Prop_GPS_Pos').val();
        data.City_Area = $('#txtLocation').val();
        data.Prop_GPS_Pos = '(' + $('#lat').val() + ',' + $('#lon').val() + ')';
        data.City_Id = $('#hdnLocationId').val();
        //   alert(data.Prop_Star_Rating)
    data.City_Name=$('#txtCities').val();
    data.State_Name=  $('#txtState').val();
    data.Location_Name = $('#txtLocation').val();
    data.Prop_Addr1 = $('#Prop_Addr1').val();
    data.Prop_Name = $('#New_Prop_Name').val();
    data.Pin_Code = $('#txtPincode').val();
    data.TripAdv = $('#txttripadv').val();
    

        $.ajax({
            url: '/api/property/Edit',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(data),

            contentType: 'application/json',
            success: function (result) {
                $.cookie("ImagePropertyd", 'null');
                var result = { Status: true, ReturnMessage: { ReturnMessage: "Property updated Successfully" }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                if (err.responseText == "Creation Failed") {
                    Failed(JSON.parse(err.responseText));
                }
                else {
                    Failed(JSON.parse(err.responseText));

                }
            },
            complete: function () {

            }
        });
    };
}

function getBankDetails() {
    var self = this;
    self.BankDetails = ko.observableArray([]);
    $.ajax({
        type: "GET",
        url: '/api/property/EditBankDetails',
        data: { ID: propID },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.length <= 0)
                self.BankDetails.push(new BankDetailsClass(''));
            for (var i = 0; i < data.length; i++)
                self.BankDetails.push(new BankDetailsClass(data[i])); //Put the response in ObservableArray
        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);

        }
    }).done(function () {
        ko.applyBindings(propertyViewModel, document.getElementById("tab_1_2"));
    });

    this.CreateBank = function (data) {
        
        if ($('#txtLocation1').val() == '') {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Please Enter Location" }, ErrorType: "error" };
            Failed(result)
            return false;
        }
        AppCommonScript.ShowWaitBlock();
        data.City_Area = $('#txtLocationBank').val();
        data.City_Id = $('#hdnLocationId1').val();
        $.ajax({
            url: '/api/property/EditBank',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(data),

            contentType: 'application/json',
            success: function (result) {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(result);
            },
            error: function (err) {
                if (err.responseText == "Creation Failed") {
                    Failed(JSON.parse(err.responseText));
                }
                else {
                    Failed(JSON.parse(err.responseText));

                }
            },
            complete: function () {

            }
        });
    };
};

function getPolicy() {

    var self = this;

    var VendId = $.cookie("VendId");

    $.ajax({
        type: "GET",
        url: '/api/property/BindPolicy',
        data: { PropId: propID, VendId: 0 },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //   alert(Prop_Id);
            propertyViewModel.Policies.removeAll();
            ko.cleanNode(propertyViewModel.Policies, document.getElementById("tab_1_3"));
            for (var i = 0; i < data.length; i++) {
                //find out in facilities whether data existi..
                propertyViewModel.Policies.push(new PolicyClass(data[i]));
            }//Put the response in ObservableArray
            //  ko.applyBindings(propertyViewModel.Policies, document.getElementById("PolicyDT"));

            $(".PolicyDT").DataTable({ responsive: true });

            //$(".cke_editable cke_editable_themed cke_contents_ltr cke_show_borders").append('<p>sdsdsd</p>');


        },
        error: function (err) {
            //  alert(err.status + " : " + err.statusText);

        }

    }).done(function () {
        ko.applyBindings(propertyViewModel, document.getElementById("tab_1_3"));
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
    this.ResetFormPolicy = function () {

        propertyViewModel.Policy_Name("");
        propertyViewModel.Policy_Id("");
        propertyViewModel.Policy_descr("");


    }
}

function CreatePolicy() {

    AppCommonScript.ShowWaitBlock();
    var Policy = new InitializePolicy();
    //  Policy.Policy_descr = Policy.Policy_descrEdit;
    $.ajax({
        type: "POST",
        url: "/api/property/createPolicy",
        data: $.parseJSON(ko.toJSON(Policy)),
        dataType: "json",
        success: function (response) {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Policy created successfully " }, ErrorType: "Success" };
            Successed(result);
            //    window.location.reload()
            getPolicy()
            $('#myModalPolicy').click('hidden.bs.modal', function () {
                //window.alert('hidden event fired!');
            });
            ResetFormPolicy();
            //getFacility();
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}
function PolicyClass(data) {
    var policy = this;

    policy.Policy_Id = data["Policy_Id"];
    policy.Policy_Name = data["Policy_Name"];
    var PD = data["Policy_descr"];
    policy.Policy_descr = data["Policy_descr"];

    policy.Policy_descrEdit = PD.replace(/\n/g, '<br/>');


}
function InitializePolicy() {
    var policy = new function () { };

    policy.Vndr_Id = Vndr_Id
    //   alert(Vndr_Id)
    policy.Policy_Name = propertyViewModel.Policy_Name();
    policy.Prop_Id = propID
    policy.Policy_Id = propertyViewModel.Policy_Id();
    policy.Policy_descr = propertyViewModel.Policy_descr();
    policy.Policy_descrEdit = propertyViewModel.Policy_descrEdit();

    return policy
}





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


function InitializeFacility() {
    var facility = new function () { };

    facility.Prop_Id = propID;
    facility.Facility_Type = propertyViewModel.SelectedFacilityType();
    facility.Facility_Name = propertyViewModel.SelectedFacilityName();

    return facility
}
//Model
function UserClass(data) {
    
    var user = this;

    user.Prop_Id = data["Prop_Id"];
    user.Prop_Name = ko.observable(data["Prop_Name"]);
    user.Prop_Cin_No = ko.observable(data["Prop_Cin_No"]);
    user.Prop_Addr1 = data["Prop_Addr1"];
    user.Prop_Addr2 = data["Prop_Addr2"];

    user.Prop_Overview = data["Prop_Overview"];

    user.Prop_Star_Rating = data["Prop_Star_Rating"];
    user.Prop_GPS_Pos = data["Prop_GPS_Pos"];
    user.Prop_Booking_MailId = data["Prop_Booking_MailId"];
    user.Prop_Booking_Mob = data["Prop_Booking_Mob"];
    user.Prop_Pricing_MailId = data["Prop_Pricing_MailId"];
    user.Prop_Pricing_Mob = data["Prop_Pricing_Mob"];
    user.Prop_Inventory_MailId = data["Prop_Inventory_MailId"];
    user.Prop_Inventory_Mob = data["Prop_Inventory_Mob"];

    user.City_Id = data["City_Id"];
    //alert(user.City_Id)
    user.TripAdv = data["TripId"];
    user.Location_Name = data["City_Area"];
    user.Pin_Code = data["Pincode"];
    user.State_Name = data["State_Name"];
    user.City_Name = data["City_Name"];
    user.StarRatingLookup = ko.observableArray(['1', '2', '3', '4', '5']);
    user.Room_Checkins = data["Room_Checkin"];
    user.Room_Checkouts = data["Room_Checkout"];
    $('#PropertyNameLI').append(data.Prop_Name);

   // user.CityLookup = GetCityLookup();
    //alert(user.Prop_Star_Rating)
    $('input[name=rating][value=' + user.Prop_Star_Rating + ']').prop('checked', true)
    var latlong = data["Prop_GPS_Pos"];
   
    var myString = latlong;
    var arr = myString.split(',');
    arr[0] = arr[0].replace('(', '');
    arr[1] = arr[1].replace(')', '');
 
 $('#lat').val(arr[0]);
 $('#lon').val(arr[1]);
}

function BankDetailsClass(data) {
    var bank = this;
    bank.Prop_Id = propID;
    bank.Bank_Id = data["Bank_Id"] || '';
    bank.Bank_Name = data["Bank_Name"] || '';
    bank.Bank_descr = data["Bank_descr"];
    bank.Bank_Branch_Name = data["Bank_Branch_Name"] || '';
    bank.Bank_Branch_Code = data["Bank_Branch_Code"] || '';
    bank.Bank_IFC_code = data["Bank_IFC_code"] || '';
    bank.Bank_Accnt_No = data["Bank_Accnt_No"] || '';
    bank.Bank_Accnt_Name = data["Bank_Accnt_Name"] || '';
    bank.Vndr_Id = Vndr_Id;
 //   bank.CityLookup = GetCityLookup();
    bank.City_Id = data["City_Id"];
    bank.City_Area = data["City_Area"];
    bank.Pincode = data["Pincode"];
    bank.State_Name = data["State_Name"];
    bank.City_Name = data["City_Name"];

}

//function FacilityClass(data) {
//    var facility = this;
//    facility.Facility_Id = data["Facility_Id"];
//    facility.Facility_Type = data["Facility_Type"];
//    facility.Facility_Image_dir = data["Facility_Image_dir"];
//    facility.Facility_Name = data["Facility_Name"];

//    facility.FacilityTypeLookup = GetFacilityTypeLookup();
//    facility.SelectedFacilityType = ko.observable("");

//    facility.FacilityNameLookup = GetFacilityNameLookup();
//    facility.SelectedFacilityName = ko.observable("");
//}

function ImageModel(data) {
    var self = this;
    data = data || {};
    self.Image_Id = ko.observable(data.Image_Id || '');
    self.Image_dir = ko.observable(data.Image_dir || '');
    self.Image_Name = ko.observable(data.Image_Name || '');
    self.Image_Remarks = ko.observable(data.Image_Remarks);
    self.Active_flag = ko.observable(data.Active_flag.trim() == 'true' ? true : false);
    self.Status = ko.observable(data.Active_flag.trim() == 'true' ? 'Active' : 'Suspended');
    self.Set_Flag = ko.observable(false);
    if (data.Default_flag != null)
        self.Set_Flag(data.Default_flag.trim() == 'true' ? true : false);
    self.Select = function (eRow) {
        UpdateImageFlag(eRow);
        eRow.Status(eRow.Active_flag() == true ? 'Suspended' : 'Active');
    }

    self.SetDefault = function (eRow) {
        SetDefaultImage(eRow)
        eRow.Set_Flag(true);

    }
}

function UpdateImageFlag(data) {
    AppCommonScript.ShowWaitBlock();
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

function SetDefaultImage(data) {
    AppCommonScript.ShowWaitBlock();
    $.ajax({
        type: "Get",
        url: "/api/Property/SetDefaultImage",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { PropId: propID, ImageId: data.Image_Id() },
        success: function (response) {
            Successed(response)

            propertyViewModel.GetImages();
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}

//vinod
function FacilityClass(data) {
    var Facility = this;
    Facility.Facility_Id = data["Facility_Id"];
    Facility.Prop_Id = propID;
    Facility.Facility_Name = ko.observable(data.Facility_Name);
    Facility.Facility_Type = ko.observable(data.Facility_Type);
    Facility.Facility_descr = data.Facility_descr;
    Facility.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
    Facility.Active_flag = ko.observable(data.Active_flag);
    Facility.Id = ko.observable(data.Id);
    Facility.IsHeader = ko.observable(data.IsHeader);
    Facility.FTypecount = ko.observable(data.FTypecount);
    // Facility.Status = ko.observable(data.Active_flag.trim() == 'true' ? 'Active' : 'Suspended')

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
                //   alert(err.status + " : " + err.statusText);
            }
        });
        var obj = new FacilityClass(data)
        obj.Active_flag('false');
        //  obj.Status(obj.Active_flag() == true ? 'Suspended' : 'Active');
    },

};


self.ActivateFacility = function (facility) {

    AddFacility.Active(facility)


};


