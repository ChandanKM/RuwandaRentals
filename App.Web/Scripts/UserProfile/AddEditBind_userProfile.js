
function UserProfileModel(data) {
    var self = this;
    var data = data || {};
    self.User_Id = ko.observable(data.User_Id || '');
    self.Pswd = ko.observable(data.Pswd || '');
    self.User_Name = ko.observable(data.User_Name || '');
    self.Firstname = ko.observable(data.Firstname || '');
    self.Lastname = ko.observable(data.Lastname || '');
    self.Authority_Id = ko.observable(data.Authority_Id || '');
    self.UserType = ko.observable();
    self.FullName = ko.computed(function () {
        return self.Firstname() + " " + self.Lastname();
    })
    self.Department = ko.observable(data.Department || '');

    self.isActive = ko.observable(data.Active_flag == 'True' ? 'block' : 'block');

    self.Edit = function (eRow) {
        objUserProfileVM.User_Id(eRow.User_Id());
        objUserProfileVM.Selected_Type(eRow.Authority_Id());  //for selected

        objUserProfileVM.User_Name(eRow.User_Name());
        objUserProfileVM.Firstname(eRow.Firstname());
        objUserProfileVM.Lastname(eRow.Lastname());
        objUserProfileVM.Pswd(eRow.Pswd());
        objUserProfileVM.Department(eRow.Department());
        $("#btnCancel").show();
        $("#btnUpdate").show();
        $("#btnSave").hide();
        $("#btnReset").hide();
    }
    self.Suspend = function (eRow) {
        if (confirm('Are you sure ?')) {
            $.ajax({
                type: "POST",
                url: "/api/UserParam/suspend",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON(eRow),
                cache: false,
                success: function (response) {
                    objUserProfileVM.UserProfileList.remove(eRow);
                },
                error: function (err) {
                    alert(err.responseText);
                }
            });
        }
    }
}


function UserProfileViewModel() {
    var self = this;
    self.UserProfileList = ko.observableArray();

    self.User_Id = ko.observable();
    self.TypesList = GetUserType();
    self.PropList = GetPropLookup();
    self.Selected_Type = ko.observable();
    self.Selected_Prop = ko.observable();
    self.Authority_Id = ko.observable();
    self.User_Name = ko.observable();
    self.Firstname = ko.observable();
    self.Lastname = ko.observable();
    self.Pswd = ko.observable();
    self.Department = ko.observable();

    self.BindUserProfile = function () {
        $.ajax({
            type: "POST",
            url: "/Vendor/GetLoginVendorId",
            dataType: "json",
            success: function (response) {
               
               
                $.localStorage("VendId", response)

            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });
        $.ajax({
            type: "POST",
            url: "/Vendor/GetLoginAuthId",
            dataType: "json",
            success: function (response) {
                $.localStorage("AuthId", response)
            

            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });
        $.ajax({
            type: "POST",
            url: "/Vendor/GetLoginUserId",
            dataType: "json",
            success: function (response) {
                $.localStorage("UserId", response)


            },
            error: function (jqxhr) {

                Failed(JSON.parse(jqxhr.responseText));
            }
        });
        var VendId = $.localStorage("VendId")
        var AuthId = $.localStorage("AuthId")
        var UserProfileID = $.localStorage("UserId");
        var urlPath = window.location.pathname;
        var AuthIDbyURL = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
     
        if ( AuthIDbyURL == 5||AuthIDbyURL == 4 || AuthIDbyURL == 5)
        {
           
            $('#divAdmin').hide()
        }
        else if (AuthIDbyURL == 1 || AuthIDbyURL == 2) {
          
            $('#divProperty').hide()
        }
      

        $.ajax({
            type: "GET",
            url: "/api/UserParam/Bind",
            contentType: "application/json; charset=utf-8",
            data: { AuthId: AuthIDbyURL, UserId: UserProfileID },
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.Table.length; i++) {
                    self.UserProfileList.push(new UserProfileModel(data.Table[i]));
                }
            },
            error: function (err) {
                alert(err.responseText);
            }
        }).done(function () {
            ko.applyBindings(objUserProfileVM, document.getElementById("tab_1_1"));
            $(".ProfileGrid").DataTable({ responsive: true });
        });
    }

    self.SaveUserProfile = function () {
        
        var objUser = new UserProfileModel();
        if ($('#divProperty').css('display') == 'none') {
            
            objUser.UserType(0)
            objUser.Authority_Id(self.Selected_Type());
           

            
        }
        if ($('#divAdmin').css('display') == 'none') {
            var urlPath = window.location.pathname;
            var AuthIDbyURL = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
            objUser.Authority_Id(AuthIDbyURL)
            objUser.UserType(self.Selected_Prop())
        }
        objUser.User_Name(self.User_Name());
        objUser.Firstname(self.Firstname());
        objUser.Lastname(self.Lastname());
    
        objUser.Pswd(self.Pswd());
        objUser.Department(self.Department());

       
        $.ajax({
            type: "POST",
            url: "/api/UserParam/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objUser),
            cache: false,
            success: function (recorde) {
                //location.reload();
                OnSuccessSaveUserProfile(recorde);
                Successed(JSON.parse('User Profile Successfully Created'))
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.UpdateProfile = function () {
        var objUser = new UserProfileModel();
        objUser.User_Id(self.User_Id());
        objUser.User_Name(self.User_Name());
        objUser.Firstname(self.Firstname());
        objUser.Lastname(self.Lastname());
        objUser.Authority_Id(self.Selected_Type());
        objUser.Pswd(self.Pswd());
        objUser.Department(self.Department());
        $.ajax({
            type: "POST",
            url: "/api/UserParam/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objUser),
            cache: false,
            success: function (response) {
                location.reload();
                OnSuccessSaveUserProfile();
                $("#btnUpdate").hide();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.User_Name("");
        self.Firstname("");
        self.Lastname("");
        self.Pswd("");
        self.Department("");
        self.Selected_Type("");
    }

    self.Cancel = function () {
        $("#btnCancel").hide();
        $("#btnUpdate").hide();
        $("#btnSave").show();
        $("#btnReset").show();
        self.ResetForm();
    }
    OnSuccessSaveUserProfile = function (data) {
        for (var i = 0; i < data.Table.length; i++) {
            self.UserProfileList.unshift(new UserProfileModel(data.Table[i]));
        }
        self.ResetForm();
    }
}

var objUserProfileVM = new UserProfileViewModel();
objUserProfileVM.BindUserProfile();


function GetUserType() {
    var usertypes = ko.observableArray([{ Authority_Id: '0', Code: '--Select--' }]);
    $.getJSON("/api/UserParam/GetUserType", function (result) {
        ko.utils.arrayMap(result, function (item) {
            usertypes.push({ Authority_Id: item.Authority_Id, Code: item.Code });
        });
    });
    return usertypes;
}
function GetPropLookup() {
    var Props = ko.observableArray([{ PropId: 0, PropName: 'Select Property' }]);
    $.ajax({
        type: "GET",
        url: "/api/vendors/allproperty",
        data: { VendorId: $.cookie("VendId") },
        dataType: "json",
        success: function (result) {
            ko.utils.arrayMap(result, function (item) {
                Props.push({ PropId: item.PropId, PropName: item.PropName });
            });
        }
    });


    return Props;
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}