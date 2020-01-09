
var urlPath = window.location.pathname;
var UserID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

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
            url: '/api/users/Edit',
            data: { ID: UserID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
          
            success: function (data) {
              
                for (var i = 0; i < data.length; i++)
                    self.Users.push(new UserClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    SaveUser: function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            url: '/api/users/Edit',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON(this),
           
            contentType: 'application/json',
            success: function (result) {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(result);
            },
            error: function (err) {
                if (err.responseText == "Creation Failed")
                {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);
                }
                else {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(err);
                   
                }
            },
            complete: function () {
               
            }
        });
    },

};

self.editStudent = function (student) {
  
    window.location.href = '/api/users/Edit/' + student.Id;
};

//Model
function UserClass(data) {
  
    var user = this;
    user.Id = data["Id"];
    user.FirstName = data["FirstName"];
    user.LastName = data["LastName"];
    user.Password = data["Password"];
    user.UserName = data["UserName"];

}

