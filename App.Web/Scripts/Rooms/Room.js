

function getValueUsingParentTag() {
    var chkArray = [];

    /* look for all checkboes that have a parent id called 'checkboxlist' attached to it and check if it was checked */
    $("#GrossRate input:checked").each(function () {
        chkArray.push($(this).val());
    });

    /* we join the array separated by the comma */
    var selected;
    selected = chkArray.join(',') + "";

    /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
    if (selected.length > 1) {
     //   alert("You have selected " + selected);
    } else {
       // alert("Please at least one of the checkbox");
    }
}
var windowURL = window.URL || window.webkitURL;  // Custom filehandler function
var urlPath = window.location.pathname;
var PID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

var AllMeal = document.getElementById("AllMeal");
var BrkFst = document.getElementById("BrkFst");
var Lunch = document.getElementById("Lunch");
var Dinner = document.getElementById("Dinner");
var ap = document.getElementById("AirportPickup");
var ad = document.getElementById("AirportDrop");
var wifi = document.getElementById("wifi");


AllMeal.onchange = function () {
    // $('#Brkfst').checked = 'false';
    $('#BrkFst').attr('checked', false);
    $('#Lunch').attr('checked', false);
    $('#Dinner').attr('checked', false);

};
BrkFst.onchange = function () {
    // $('#Brkfst').checked = 'false';
    $('#AllMeal').attr('checked', false);

};
Lunch.onchange = function () {
    // $('#Brkfst').checked = 'false';
    $('#AllMeal').attr('checked', false);

};
Dinner.onchange = function () {
    // $('#Brkfst').checked = 'false';
    $('#AllMeal').attr('checked', false);

};

$("#txtRType").autocomplete({

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


InitializeRoomViewModel();




function RoomType() {
    
    var RType = $("#txtRType").val();
    $("#txtRType").val(RType);
}
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
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Your image is too big, it must be less than 600*300" }, ErrorType: "success" };
            Failed(result);

            self.CancelUpload();
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
        $('#imgUploadNew').val('')
    }
}


function RoomViewModel() {

    this.Room_Name = ko.observable("");
    this.Image_dir = ko.observable(roomimage != null ? roomimage : "/img/Room-image.png");

    this.Room_Overview = ko.observable("");
    this.Room_Adult_occup = ko.observable("");
    this.Room_Child_occup = ko.observable("");
    this.Room_Extra_Adul = ko.observable("");
    this.Room_Standard_rate = ko.observable("");
    this.Room_Agreed_Availability = ko.observable("");
    this.Room_Lmk_Rate = ko.observable("");
    this.Room_camflg = ko.observable("");
    this.Room_Checkin = ko.observable("");
    this.Room_Checkout = ko.observable("");
    this.Room_Grace_time = ko.observable("");
    this.Room_Max_Thrshold_Disc = ko.observable("");
    this.Tax_Id = ko.observable("");
    this.Prop_Id = ko.observable("");
    this.RTypeName = ko.observable("");
    //   this.roomtypeLookup = GetroomtypeLookup();
    //  this.Selectedroomtype = ko.observable();


    //this.Room_Active_flag = ko.observable("");

    this.CreateRoom = function () {
        CreateRoom();



    };

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

function InitializeRoomViewModel() {


    roomViewModel = new RoomViewModel();
    //ko.applyBindings(roomViewModel);
    ko.applyBindings(roomViewModel, document.getElementById("tab_1_1"));
    $("#Tab2").css("display", "none");
    $("#Tab3").css("display", "none");

    var PropID = PID;
    $.ajax({
        type: "GET",
        url: '/rooms/GetAmountType',
        contentType: "application/json; charset=utf-8",
        data: { Prop_Id: PropID },
        dataType: "json",
        success: function (data) {

        },
        error: function (err) {
            //alert(err.status + " : " + err.statustext);

        }
    });

}

function CreateRoom() {
    // AppCommonScript.ShowWaitBlock();
    

    var room = new InitializeRoom();
    var Inclusion = '';
    var others = '';
    if (!$("input[id='AllMeal']:checked").val()) {

        Inclusion = '';
        if (!$("input[id='BrkFst']:checked").val()) {


            Inclusion = '';
        }
        else {
            Inclusion = Inclusion + 'BreakFast,'

        }
        if (!$("input[id='Lunch']:checked").val()) {


            // Inclusion = '';
        }
        else {
            Inclusion = Inclusion + 'Lunch,'

        }
        if (!$("input[id='Dinner']:checked").val()) {


            //  Inclusion = '';
        }
        else {
            Inclusion = Inclusion + 'Dinner,'

        }
        var lastChar = Inclusion.slice(-1);
        if (lastChar == ',') {
            Inclusion = Inclusion.slice(0, -1);
        }

    }
    else {
        Inclusion = 'All Meals'

    }
    var chkArray = [];

    /* look for all checkboes that have a parent id called 'checkboxlist' attached to it and check if it was checked */
    $("#GrossRate input:checked").each(function () {
        chkArray.push($(this).val());
    });

    /* we join the array separated by the comma */
    var selected;
    selected = chkArray.join(',') + "";

    /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
    if (selected.length > 1) {
        //   alert("You have selected " + selected);
    } else {
        // alert("Please at least one of the checkbox");
    }
    

    Inclusion = Inclusion + ' inclusive';
    
    AppCommonScript.ShowWaitBlock();
    room.Room_Max_Thrshold_Disc = Inclusion;
    room.Room_Name = $('#txtRType').val();
    room.Room_Checkin = '12:00:00 PM';
    room.Room_Checkout = '12:00:00 PM';
    room.Room_Grace_time = '12:00:00 PM';
    room.Image_dir = roomimage != null ? roomimage : '/img/Room-image.png';
    // alert($.localStorage("RoomPropertyId"))
    room.Prop_Id = PID;
    room.Room_Extra_Adul = selected;
    room.Room_Standard_rate = $('#Room_Standard_rate').val();
    $.ajax({
        type: "POST",
        url: "/api/rooms/create",
        data: $.parseJSON(ko.toJSON(room)),
        dataType: "json",

        success: function (response) {
            

            
            var res = response;

            if (res == '0') {
                var result = { Status: true, ReturnMessage: { ReturnMessage: "Room Type already exists!!" }, ErrorType: "Success" };
                Failed(result);
                AppCommonScript.HideWaitBlock();
            }
            else {
                alert('Room Created successfully!');
                AppCommonScript.HideWaitBlock();
                $.cookie("RoomsID", response)

                //   alert($.cookie("RoomsID"))
                $.cookie("ImageRoom", 'null')
                $.localStorage("Tabs", "FF");
                window.location.href = '/Vendor/RoomEdit/' + response;
            }

        },
        error: function (jqxhr) {


            Failed(JSON.parse(jqxhr.responseText));
        }

    });

}



//}




getFacility = function () {
    Facilities = ko.observableArray([]);
    
    var self = this;

    $.ajax({
        type: "GET",
        url: '/api/rooms/BindFacility',
        contentType: "application/json; charset=utf-8",
        data: { Room_id: $.cookie("RoomsID") },
        dataType: "json",
        success: function (data) {
            
            //Facilities.removeAll();
            for (var i = 0; i < data.length; i++) {
                Facilities.push(new FacilityClass(data[i])); //Put the response in ObservableArray
                $('.1').hide();
                $('.5').addClass('break');
            }

            ko.applyBindings(Facilities, document.getElementById('tab2'));

        },
        error: function (err) {
            //alert(err.status + " : " + err.statusText);

        }
    });
},



function Successed(response) {

    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);

}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function InitializeRoom() {

    var rooms = new function () { };



    //  rooms.Room_Name = roomViewModel.Room_Name();
    rooms.Image_dir = roomViewModel.Image_dir();
    rooms.Room_Overview = roomViewModel.Room_Overview();
    rooms.Room_Adult_occup = roomViewModel.Room_Adult_occup();
    rooms.Room_Child_occup = roomViewModel.Room_Child_occup();
    rooms.Room_Extra_Adul = roomViewModel.Room_Extra_Adul();
    rooms.Room_Standard_rate = roomViewModel.Room_Standard_rate();
    rooms.Room_Agreed_Availability = roomViewModel.Room_Agreed_Availability();
    rooms.Room_Lmk_Rate = roomViewModel.Room_Lmk_Rate();
    rooms.Room_camflg = roomViewModel.Room_camflg();
    rooms.Room_Checkin = roomViewModel.Room_Checkin();
    rooms.Room_Checkout = roomViewModel.Room_Checkout();
    rooms.Room_Grace_time = roomViewModel.Room_Grace_time();
    rooms.Room_Max_Thrshold_Disc = roomViewModel.Room_Max_Thrshold_Disc();
    rooms.Tax_Id = roomViewModel.Tax_Id();
    //  rooms.Type_Id = roomViewModel.Selectedroomtype();
    rooms.Prop_Id = roomViewModel.Prop_Id();
    rooms.Room_Name = roomViewModel.Room_Name();
    return rooms;
}


//var AddFacility = {

//    Active: function (data) {
//        
//        $.ajax({
//            type: "POST",
//            url: '/api/rooms/ActivateFacility',
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            data: ko.toJSON(data),
//            success: function (response) {

//                // ko.applyBindings(FacilityListVM.getFacility(), document.getElementById('tab2'));


//            },
//            error: function (err) {
//                //  alert(err.status + " : " + err.statusText);
//            }
//        });
//        var obj = new FacilityClass(data)
//        obj.Active_flag('false');

//    },

//};


//self.ActivateFacility = function (facility) {

//    AddFacility.Active(facility)


//};



//var obj = new FacilityClass
////Model
//function FacilityClass(data) {
//    var Facility = this;


//    Facility.Facility_Id = data.Facility_Id;
//    Facility.Room_Id = $.cookie("RoomsID");
//    Facility.Facility_Name = ko.observable(data.Facility_Name);
//    Facility.Facility_Type = ko.observable(data.Facility_Type);
//    Facility.Facility_descr = data.Facility_descr;
//    Facility.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
//    Facility.Active_flag = ko.observable(data.Active_flag);
//    Facility.spamFlavors = ko.observable(data.Active_flag);
//    Facility.Id = ko.observable(data.Id);
//    Facility.IsHeader = ko.observable(data.IsHeader);
//    Facility.FTypecount = ko.observable(data.FTypecount);
//}


