function LoginViewModel() {
    var self = this;
    self.Email = ko.observable('');
    self.Submit = function (data) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Login/forgot",
            data: $.parseJSON(ko.toJSON(data)),
            dataType: "json",
            success: function (response) {
                self.Email('');
                Successed(response);
            },
            error: function (err) {
                self.Email('');
                Failed(JSON.parse(err.responseText));
            }
        });
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