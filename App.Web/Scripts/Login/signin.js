function LoginViewModel() {
    var self = this;

    self.UserId = ko.observable('');
    self.Pswd = ko.observable('');

    self.Signin = function (data) {
        
        $.ajax({
            type: "POST",
            url: "/api/Login/signin",
            data: $.parseJSON(ko.toJSON(data)),
            dataType: "json",
            success: function (response) {
                window.location.href = '/UserProfile/Index';
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));

            }
        });
    }
    self.Forgot = function () {
        window.location.href = '/Login/Forgot';
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


var loginVM = new LoginViewModel();
$(document).ready(function () {
    ko.applyBindings(loginVM);
});