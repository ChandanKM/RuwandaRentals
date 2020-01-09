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
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            url: "/Facility/ImageUpload",
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

    self.CancelUpload = function () {
        self.imageFile('');
        self.imageObjectURL('');
        self.imageBinary('');
        $('#imgAdd').attr('src', '');
    }
}


var urlPath = window.location.pathname;
var UserID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

$('#FacilityId').val(UserID);

var facilityViewModel = new FacilityViewModel();
facilityViewModel.GetFacility();
ko.applyBindings(facilityViewModel, document.getElementById('divFacilityEdit'));



function FacilityViewModel() {
    var self = this;
    self.List = ko.observableArray();
    self.GetFacility = function () {
        $.ajax({
            type: "GET",
            url: '/api/facility/BindFacility',
            data: { ID: UserID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.List.push(new FacilityClass(data[i])); //Put the response in ObservableArray
                }
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};




function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}
//Model
function FacilityClass(data) {
    var self = this;
    self.Facility_Id = data["Facility_Id"];
    self.Facility_Type = data["Facility_Type"];
    self.Facility_Image_dir = data["Facility_Image_dir"];
    self.Facility_Name = data["Facility_Name"];
    self.Facility_descr = data["Facility_descr"];


    self.Edit = function (data) {
        if (confirm('Are you sure you want to Edit this?')) {
            if (facilityimage != null)
                data.Facility_Image_dir = facilityimage;
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/facility/EditFacility',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: 'application/json',
                success: function (result) {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(result);
                    //window.location.herf = "facility/Bind";
                    window.location.href = '/SuperAdmin/Facilities';
                    return false;
                },
                error: function (err) {
                    
                    if (err.responseText == "Creation Failed") {
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


