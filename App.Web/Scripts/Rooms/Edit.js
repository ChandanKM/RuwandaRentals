
var windowURL = window.URL || window.webkitURL;  // Custom filehandler function
if ($.localStorage("Tabs") == "PP")
{
    $('#tab_1_2').removeClass('active')
    $('#tab_1_3').addClass('active')
    $('#Tab2').removeClass('active')
    $('#Tab3').addClass('active')
}

var urlPath = window.location.pathname;
var RoomID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
//var PropID = $.cookie("Property_Id");

$("#txtDemo").autocomplete({

    source: function (request, response) {

        $.ajax({
            type: "GET",
            url: "/api/Rooms/GetAutoCompleteSearch",
            data: { terms: request.term },
            dataType: "json",
            cacheResults: true,
            contentType: "application/json; charset=utf-8",
            success: OnSuccess,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
        function OnSuccess(r) {
            response($.map(r, function (item) {
                
                return {
                    value: item,
                    label: item,
                    val: item
                }
            }))
        }
    },
    select: function (e, i) {

        $("#txtRType").val(i.item.value);
        // $("#txtLocationBank").val(i.item.val);
    },
    minLength: 2
});

function RoomType() {
    
    var RType = $("#txtDemo").val();
    $("#txtRType").val(RType);
}
var roomVM = new RoomListVM();
var PropID = $.localStorage("RoomPropertyIdEdit")
roomVM.GetRoom();

var roomimage = null;
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
                url: "/Rooms/ImageUpload",
                type: "POST",
                data: {
                    file: uploadModel.images._latestValue[0].firstBytes()
                },
                traditional: true,
                success: function (response) {
                    roomimage = response;
                    self.CancelUpload();
                    $('#imgroom').attr('src', response);
                    AppCommonScript.HideWaitBlock();
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
    }
}



//View Model
function RoomListVM() {

    var self = this;
    self.Rooms = ko.observableArray([]);

    self.GetRoom = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/rooms/GetAmountType',
            contentType: "application/json; charset=utf-8",
            data: { Prop_Id: PropID },
            dataType: "json",
            success: function (data) {

                OnSuccessGetRoom();
            },
            error: function (err) {

            }
        });
    }

    OnSuccessGetRoom = function () {
        $.ajax({
            type: "GET",
            url: '/api/rooms/Edit',
            data: { Id: RoomID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.Rooms.push(new RoomClass(data[i])); //Put the response in ObservableArray
                }
                $('#RoomName').append(data[0].Room_Name);

                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
            }
        }).done(function () {

            ko.applyBindings(roomVM, document.getElementById('tab_1_1'));
        });
    };

};





function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function GetroomtypeLookup() {

    var roomtypes = ko.observableArray([{ Type_Id: 0, Room_Name: 'Select Room Type' }]);
    $.getJSON("/api/rooms/allroomtypes", function (result) {
        ko.utils.arrayMap(result, function (item) {
            roomtypes.push({ Type_Id: item.Type_Id, Room_Name: item.Room_Name });
        });
    });
    return roomtypes;
}


//Model
function RoomClass(data) {
    var rooms = this;
    
    rooms.Room_Id = ko.observable(data["Room_Id"]);
    rooms.Room_Name = ko.observable(data["Room_Name"]);
    rooms.Room_Overview = ko.observable(data["Room_Overview"]);
    rooms.Room_Adult_occup = ko.observable(data["Room_Adult_occup"]);
    rooms.Room_Child_occup = ko.observable(data["Room_Child_occup"]);
    rooms.Room_Extra_Adul = ko.observable(data["Room_Extra_Adul"]);
    rooms.Room_Standard_rate = ko.observable(data["Room_Standard_rate"]);
    rooms.Room_Agreed_Availability = ko.observable(data["Room_Agreed_Availability"]);
    rooms.Room_Lmk_Rate = ko.observable(data["Room_Lmk_Rate"]);
    rooms.Room_camflg = ko.observable(data["Room_camflg"]);
    rooms.Room_Checkin = ko.observable(data["Room_Checkin"]);
    rooms.Room_Checkout = ko.observable(data["Room_Checkout"]);
    rooms.Room_Grace_time = ko.observable(data["Room_Grace_time"]);
    rooms.Room_Max_Thrshold_Disc = ko.observable(data["Room_Max_Thrshold_Disc"]);
    rooms.Tax_Id = ko.observable(data["Tax_Id"]);
    //  rooms.RoomType = GetroomtypeLookup();
    // rooms.Selectedroomtype = ));
    //   rooms.Type_Id = ko.observable(data["Type_Id"]);
    //rooms.Type_Id = data["Type_Id"];
    rooms.Image_dir = ko.observable(data.Image_dir || "/img/Room-image.png");

    rooms.Image_Id = ko.observable(data["Image_Id"]);
    rooms.Prop_Id = ko.observable(data["Prop_Id"]);

    rooms.Editroomsdetails = function (data) {
        
        if (roomimage != null)
            data.Image_dir = roomimage;
        data.Room_Name = $('#txtRType').val();
        if ($('#GrossRate').css('display') == 'block') {

            if (data.Room_Standard_rate() == '' || data.Room_Standard_rate() == '0') {
                alert('Please enter Pricing Amount')

            }
            else {

                AppCommonScript.ShowWaitBlock();
                $.ajax({
                    url: '/api/rooms/EditRoomdetail',
                    type: 'post',
                    dataType: 'json',
                    data: ko.toJSON(data),
                    contentType: 'application/json',
                    success: function (result) {
                        

                        
                        AppCommonScript.HideWaitBlock();
                        AppCommonScript.showNotify(result);
                    },
                    error: function (jqxhr) {
                        alert('s')
                        Failed(JSON.parse(jqxhr.responseText));
                    }
                });
            }
        }
        else {
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/rooms/EditRoomdetail',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(data),
                contentType: 'application/json',
                success: function (result) {

                   AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(result);
                },
                error: function (jqxhr) {
                    alert('Room Type already exists')
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(jqxhr.responseText);
                   Failed(JSON.parse(jqxhr.responseText));
                }
            });

        }

    };
}
