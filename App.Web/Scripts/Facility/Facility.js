var windowURL = window.URL || window.webkitURL;  // Custom filehandler function

var facilityimage = null;
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
            alert('Your image is too big, it must be less than 600*300');
        }
        else {
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: "../Facility/ImageUpload",
                type: "POST",
                data: {
                    file: uploadModel.images._latestValue[0].firstBytes()
                },
                traditional: true,
                success: function (response) {
                    facilityimage = response;
                    self.CancelUpload();
                    $('#imgFacility').attr('src', response);
                    AppCommonScript.HideWaitBlock();
                },
                error: function (err) {
                    Failed(JSON.parse(err.responseText));
                    AppCommonScript.HideWaitBlock();
                }
            });
        }
    }

    self.CancelUpload = function () {
        self.imageFile('');
        self.imageObjectURL('');
        self.imageBinary('');
        $('#imgAdd').attr('src', '');
    }
}

$(document).ready(function () {

    InitializefacilityViewModel();
});

function FacilityViewModel() {

    
    this.Facility_Name = ko.observable("");
    this.Facility_Type = ko.observable("");
    this.Facility_descr = ko.observable("");
    this.Facility_Image_dir = ko.observable("/img/Design/booking icon.png");

    this.Facility_Active_flag = ko.observable("");

    this.CreateFacility = function () {
        CreateFacility();
    };
}




function InitializefacilityViewModel() {


    facilityViewModel = new FacilityViewModel();
    ko.applyBindings(facilityViewModel, document.getElementById('divFacilityCreate'));
}

function CreateFacility() {
    // AppCommonScript.ShowWaitBlock();
    var facility = new InitializeFacility();
    facility.Facility_Image_dir = facilityimage;
    $.ajax({
        type: "POST",
        url: "/api/facility/create",
        data: $.parseJSON(ko.toJSON(facility)),
        dataType: "json",
        success: function (response) {
            alert('Facility added!')
            window.location.href = "/SuperAdmin/Facilitiess";
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

function InitializeFacility() {

    var facility = new function () { };

    facility.Facility_Name = facilityViewModel.Facility_Name();
    facility.Facility_Type = facilityViewModel.Facility_Type();
    facility.Facility_descr = facilityViewModel.Facility_descr();
    facility.Facility_Image_dir = facilityViewModel.Facility_Image_dir();
    facility.Facility_Active_flag = facilityViewModel.Facility_Active_flag == true ? 'True' : 'Flase';

    return facility;
}
