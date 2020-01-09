var urlPath = window.location.pathname;

$(function () {
    Facilities: ko.observableArray([]),
    FacilityListVM.getFacility();
    ko.applyBindings(FacilityListVM, document.getElementById('tab2'));


});

var FacilityListVM = {

    Facilities: ko.observableArray([]),

    getFacility: function () {
        
        var self = this;

        $.ajax({
            type: "GET",
            url: '/api/rooms/BindFacility',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {

                    FacilityListVM.Facilities.push(new FacilityClass(data[i])); //Put the response in ObservableArray
                    $('.1').hide();
                    $('.5').addClass('break');

                }

            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    },



};

$(document).ready(function () {

    InitializeFacilityViewModel();
});

function FacilityViewModel() {

    
    this.Room_Id = ko.observable("");
    this.Facility_Id = ko.observable("");
    this.Facility_Name = ko.observable("");
    this.Facility_Type = ko.observable("");
    this.Facility_descr = ko.observable("");
    this.Facility_Image_dir = ko.observable("");
    this.Active_flag = ko.observable("");
    this.spamFlavors = ko.observable("");
    this.Id = ko.observable("");
    this.IsHeader = ko.observable("");
    this.FTypecount = ko.observable("");

    this.CreateFacility = function () {
        CreateFacility();
    };
}



function InitializeFacilityViewModel() {


    facilityViewModel = new FacilityViewModel();
    ko.applyBindings(facilityViewModel);


}

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
                alert(err.status + " : " + err.statusText);
            }
        });
        var obj = new FacilityClass(data)
        obj.spamFlavors('false');

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
    facility.spamFlavors = facilityViewModel.Active_flag();
    facility.Id = facilityViewModel.Id();
    facility.IsHeader = facilityViewModel.IsHeader();
    facility.FTypecount = facilityViewModel.FTypecount();
    return rooms;
}



var obj = new FacilityClass
//Model
function FacilityClass(data) {
    var Facility = this;


    Facility.Facility_Id = data.Facility_Id;
    Facility.Room_Id = ko.observable(data.Room_Id);
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




