var urlPath = window.location.pathname;


$(document).ready(function () {

    AllFacility = new FacilityListVM();

    AllFacility.getFacility();

});
//View Model
var FacilityListVM = function () {
    Prop = this;
    Prop.getFacility = function () {
        
        var self = this;
        self.Facility = ko.observableArray([]),

        $.ajax({
            type: "GET",
            url: '/api/facility/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                
                self.Facility(data);
                ko.applyBindings(AllFacility, document.getElementById("FacilityDT"));

                $(".FacilityDT").DataTable({ responsive: true, 'iDisplayLength': 15 });
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
                
            }
        });
    },



self.editFacility = function (facility) {
    
    window.location.href = 'EditFacility/' + facility.Facility_Id;
};
 

    self.deleteFacility = function (facility) {

        if (confirm('Are you sure you want to delete this?')) {
            //var property = new InitializePropertyDelete(this);
            //alert(property.PropId)
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/facility/DeteteFacilty',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(facility),

                contentType: 'application/json',
                success: function (result) {
                    window.location.href = "/SuperAdmin/Facilitiess";

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
    };
}


//Model
function Create() {
    window.location.href = 'CreateFacility';

}
function Facility(data) {
    //Model
    

    this.Facility_Id = ko.observable(data.Facility_Id);
    this.Facility_Name = ko.observable(data.Facility_Name);
    this.Facility_Type = ko.observable(data.Facility_Type);
    this.Facility_descr = ko.observable(data.Facility_descr);
    this.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
    this.Facility_Active_flag = ko.observable(data.Facility_Active_flag === 'True' ? 'true' : false);

}
