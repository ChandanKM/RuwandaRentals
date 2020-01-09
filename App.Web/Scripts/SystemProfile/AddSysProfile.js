$(document).ready(function () {
    var objSysProfileVM = new SystemProfileViewModel();
    ko.applyBindings(objSysProfileVM);
});
var urlPath = window.location.pathname;
var UserId = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

function SystemProfileViewModel() {
    var self = this;
    //  self.AllCities = GetAllCity();


    self.Company_Title = ko.observable('');
    self.Owned_By = ko.observable('');
    self.CIN_Number = ko.observable('');
    self.Adr1 = ko.observable('');
    self.Adr2 = ko.observable('');
    self.Location = ko.observable('');
    self.City = ko.observable('');
    self.Tin_id = ko.observable('');
    self.Phone = ko.observable('');
    self.Mobile = ko.observable('');
    self.Email = ko.observable('');
    self.Sms_Url = ko.observable('');
    self.User_Id = ko.observable(UserId);

//    self.UserProfile_Id = $.cookie("userprofileId");
    self.SaveProfile = function (data) {
        AppCommonScript.HideWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/SystemProfile/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            cache: false,
            success: function (recorde) {
                
               

                alert('Profile created succuessfully!')
                //var results = { Status: true, ReturnMessage: { ReturnMessage: "Profile Created Successfully" }, ErrorType: "Success" };

                //AppCommonScript.HideWaitBlock();

                window.location.href = '/SuperAdmin/SystemProfileEdit/' + UserId;
            },
            error: function (err) {
              
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.Company_Title('');
        self.Owned_By('');
        self.CIN_Number('');
        self.Adr1('');
        self.Adr2('');
        self.Location('');
        self.City('');
        self.Tin_id('');
        self.Phone('');
        self.Mobile('');
        self.Email('');
        self.Sms_Url('');
        self.User_Id('');
    }

}



function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
  
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}