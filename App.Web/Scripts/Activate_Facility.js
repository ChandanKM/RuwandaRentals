$(document).ready(function () {

    InitializeFacilityViewModel();
});

function FacilityViewModel() {

    
    this.Room_Id = ko.observable("");
    this.Facility_Id = ko.observable("");

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


                //ko.applyBindings(PoliciesListVM.getPolicy(), document.getElementById('tab3'));
                ko.applyBindings(facilityViewModel, document.getElementById("tab_1_2"));
                window.location.reload();
              
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });

    }
};


self.ActivateFacility = function (facility) {
    
        AddFacility.Active(facility)

    
};

function InitializeFacility() {
    
    var facility = new function () { };

    facility.Room_Id = facilityViewModel.Room_Id();
    facility.Facility_Id = facilityViewModel.Facility_Id();

    return rooms;
}