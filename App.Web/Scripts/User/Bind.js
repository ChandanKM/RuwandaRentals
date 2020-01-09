var urlPath = window.location.pathname;

$(function () {
   
    ko.applyBindings(UserListVM);
    UserListVM.getUsers();
});
//View Model
var UserListVM = {
 
    Users: ko.observableArray([]),
    getUsers: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: '/api/users/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                self.Users(data);
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    },
    DeleteUser: function () {
      
        if (confirm('Are you sure you want to delete this?')) {
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/users/Editd',
                type: 'Delete',
                dataType: 'json',
                data: ko.toJSON(this),

                contentType: 'application/json',
                success: function (result) {
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
    },
  
  
};
self.editEdit = function (user) {
    
    window.location.href = 'User/Edit/' + user.Id;
};


//Model
function Users(data) {
    //Model
    
        this.Id = ko.observable(data.Id);
        this.FirstName= ko.observable(data.FirstName);
        this.LastName= ko.observable(data.LasttName);
        this.Password= ko.observable(data.Password);
        this.UserName= ko.observable(data.UserName);
    }
 