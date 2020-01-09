
var signVM = new SignInViewModel();
ko.applyBindings(signVM, document.getElementById('formSignIn'));
function SignInViewModel() {
    var self = this;
    self.Cons_mailid = ko.observable('');
    self.Cons_Pswd = ko.observable('');
    self.Signin = function (data) {
       
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Consumer/WebLogin",
            data: { Cons_mailid: data.Cons_mailid(), Cons_Pswd: data.Cons_Pswd(), returnUrl: window.location.href },
            dataType: "json",
            success: function (response) {
               
                if (response != null)
                    window.location.href =  response;
                else {
                    var result = { Status: true, ReturnMessage: { ReturnMessage: "Please Enter Valid User Name and Password" }, ErrorType: "error" };
                      Failed(result);
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.FbLogin = function () {
        //FB.login(function (response) {
        //    console.log(response);
        //}, {
        //    scope: 'user_likes',
        //    auth_type: 'rerequest'
        //});
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



