
var signVM = new SignInViewModel();
ko.applyBindings(signVM, document.getElementById('formSignIn'));
function SignInViewModel() {
   
    var self = this;
    self.Corp_mailid = ko.observable('');
    self.Corp_Pswd = ko.observable('');
    
    self.Signin = function (data) {
       
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Corporate/WebLogin",
            data: { Corp_mailid: data.Corp_mailid(), Corp_Pswd: data.Corp_Pswd(), returnUrl: window.location.href },
            dataType: "json",
            success: function (response) {
               
                if (response != null)
                    window.location.href =  response;
                else {
                    var result = { Status: true, ReturnMessage: { ReturnMessage: "Not Valid User" }, ErrorType: "Success" };
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



