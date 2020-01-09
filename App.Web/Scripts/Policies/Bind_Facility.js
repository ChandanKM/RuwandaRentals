var urlPath = window.location.pathname;
var room_id = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

$(function () {
    Facilities: ko.observableArray([]),
    FacilityListVM.getFacility();
    ko.applyBindings(FacilityListVM, document.getElementById('tab_1_2'));


});

var FacilityListVM = {


    Room_Id: room_id,
    Facility_Id: ko.observable(""),
    Facility_Name: ko.observable(""),
    Facility_Type: ko.observable(""),
    Facility_descr: ko.observable(""),
    Facility_Image_dir: ko.observable(""),
    Active_flag: ko.observable(""),
    Id: ko.observable(""),
    IsHeader: ko.observable(""),
    FTypecount: ko.observable(""),

    Facilities: ko.observableArray([]),

    getFacility: function () {
        
        var self = this;

        $.ajax({
            type: "GET",
            url: '/api/rooms/BindFacility',
            contentType: "application/json; charset=utf-8",
            data: { Room_id: room_id },
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {

                    FacilityListVM.Facilities.push(new FacilityClass(data[i])); //Put the response in ObservableArray
                    $('.1').hide();
                    $('.5').addClass('break');

                }

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }
        });
    },

};


var AddFacility = {

    Active: function (data) {
        
        $.ajax({
            type: "POST",
            url: '/api/rooms/ActivateFacility',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            success: function (response) {

                // ko.applyBindings(FacilityListVM.getFacility(), document.getElementById('tab2'));


            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);
            }
        });
        var obj = new FacilityClass(data)
        obj.Active_flag('false');

    },

};


self.ActivateFacility = function (facility) {

    AddFacility.Active(facility)

};

function InitializeFacility() {

    var facility = new function () { };

    facility.Room_Id = facilityViewModel.Room_Id();
    facility.Facility_Id = facilityViewModel.Facility_Id();
    facility.Facility_Name = facilityViewModel.Facility_Name();
    facility.Facility_Type = facilityViewModel.Facility_Type();
    facility.Facility_descr = facilityViewModel.Facility_descr();
    facility.Facility_Image_dir = facilityViewModel.Facility_Image_dir();
    facility.Active_flag = facilityViewModel.Active_flag();
    facility.Id = facilityViewModel.Id();
    facility.IsHeader = facilityViewModel.IsHeader();
    facility.FTypecount = facilityViewModel.FTypecount();
    return rooms;
}



var obj = new FacilityClass
//Model
function FacilityClass(data) {
    data = data || {};
    var Facility = this;

    Facility.Facility_Id = data.Facility_Id;
    Facility.Room_Id = room_id;
    Facility.Facility_Name = ko.observable(data.Facility_Name);
    Facility.Facility_Type = ko.observable(data.Facility_Type);
    Facility.Facility_descr = data.Facility_descr;
    Facility.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
    Facility.Active_flag = ko.observable(data.Active_flag);
    Facility.spamFlavors = ko.observable(data.Active_flag);
    Facility.Id = ko.observable(data.Id);
    Facility.IsHeader = ko.observable(data.IsHeader);
    Facility.FTypecount = ko.observable(data.FTypecount);
}




