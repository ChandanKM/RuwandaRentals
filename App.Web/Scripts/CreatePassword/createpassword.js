var urlPath = window.location.pathname;
var UserID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

var createpasswordVM = new CreatePasswordViewModel();
ko.applyBindings(createpasswordVM);

function CreatePasswordViewModel() {
    var self = this;
    self.User_Id = UserID;
    self.Password = ko.observable();
    self.ConfirmPassword = ko.observable();
    self.Submit = function (data) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Login/createpassword",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            cache: false,
            success: function (response) {
                self.Reset();
                Successed(response);
             window.location.href = '/Vendor/Login';
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
                self.Reset();
            }
        });
    }
    self.Reset = function () {
        self.Password('');
        self.ConfirmPassword('');
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