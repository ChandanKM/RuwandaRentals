$(document).ready(function () {
    
    InitializeuserViewModel();
});

function UserViewModel() {
    
  
    this.FirstName = ko.observable("");
    this.LastName = ko.observable("");
    this.UserName = ko.observable("");
    this.Password = ko.observable("");

    this.CreateUser = function () {
        CreateUser();
    };
}

//Load country



function InitializeuserViewModel() {

    userViewModel = new UserViewModel();
    ko.applyBindings(userViewModel); 
}

function CreateUser() {
    AppCommonScript.ShowWaitBlock();
    var user = new InitializeUser();
   
    $.ajax({
        type: "POST",
        url: "/api/users/create",
        data: $.parseJSON(ko.toJSON(user)),
        dataType: "json",
        success: function (response) {
           
            Successed(response);
        },
        error: function (jqxhr) {
           
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function InitializeUser() {
    
    var user = new function () { };
  
    user.FirstName = userViewModel.FirstName();
    user.LastName = userViewModel.LastName();
    user.UserName = userViewModel.UserName();
    user.Password = userViewModel.Password();
    return user;
}
