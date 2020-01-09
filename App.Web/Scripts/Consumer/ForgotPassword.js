

function ForgotPwdViewModel() {
    var self = this;
    self.Cons_mailid = ko.observable('');
    self.Submit = function (data) {
        if ($('#Email').val() != "")
        {
            
            var email = self.Cons_mailid();
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(email.value)) {
                alert('Please provide a valid email address');
                email.focus;
                return false;
            }
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                type: "POST",
                url: "/api/Consumer/consumerForgotPassword",
                data: $.parseJSON(ko.toJSON(data)),
                dataType: "json",
                success: function (response) {
                    self.Cons_mailid('');
                    Successed(response);
                },
                error: function (err) {
                    self.Cons_mailid('');
                    Failed(JSON.parse(err.responseText));
                }
            });
        }
        else
        {
            alert('Please enter Email Id');
        }        
    }
    self.Back = function () {
        window.location.href='/Signin'
    }
}

var forgotpwdVM = new ForgotPwdViewModel();
ko.applyBindings(forgotpwdVM, document.getElementById('forgotForm'));

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}