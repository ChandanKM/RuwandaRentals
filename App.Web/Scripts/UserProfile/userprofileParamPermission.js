var urlPath1 = window.location.pathname;
var authority_Id = urlPath1.substring(urlPath1.lastIndexOf("/") + 1, urlPath1.length);
authority_Id = authority_Id.split('-', 2)

//  alert(AuthIDbyURL[0])

authority_Id = authority_Id[0];

if (authority_Id == 4) {
    $("#VendorCrumbs").append("Admin");
    $('#divProperty').css('display', 'none');
}
else if (authority_Id == 5) {
    $("#VendorCrumbs").append("User");
    $('#divProperty').hide();
}
else if (authority_Id == 1)
    $("#VendorCrumbs").append("Supper Admin");
else if (authority_Id == 2)
    $("#VendorCrumbs").append("Admin");
$('#UserName').empty();
$('#UserName').append('Manage Users');

var userFullName = null;
var userTypeId = null;

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
    self.Usertype_Id = ko.observable(data.User_Type || '0');  // new Added for User_typeId
    self.FullName = ko.computed(function () {
        return self.Firstname() + " " + self.Lastname();
    })

    self.PropertyName = ko.observable(data.Prop_Name || '');  // new Added for PropertyName

    self.isActive = ko.observable(data.Active_flag == 'True' ? 'block' : 'block');

    self.Edit = function (eRow) {
        objUserProfileVM.User_Id(eRow.User_Id());
        objUserProfileVM.Selected_Type(eRow.Authority_Id());    //for selected
        objUserProfileVM.Selected_Prop(eRow.Usertype_Id());     //for selected property
        objUserProfileVM.User_Name(eRow.User_Name());
        objUserProfileVM.Firstname(eRow.Firstname());
        objUserProfileVM.Lastname(eRow.Lastname());
        objUserProfileVM.Pswd(eRow.Pswd());
        $('#EMAIL').attr('readonly', 'readonly')
        $("#btnCancel").show();
        $("#btnUpdate").show();
        $("#btnSave").hide();
        $("#btnReset").hide();
      //  $("#DivPwd").hide();
        $('#divCreateProfile').show();
        $('#btnCreate').hide();
    }
    self.Suspend = function (eRow) {
        if (confirm('You Want to Suspend this Account ?')) {
            $.ajax({
                type: "POST",
                url: "/api/UserParam/suspend",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON(eRow),
                cache: false,
                success: function (response) {
                    objUserProfileVM.UserProfileList.remove(eRow);
                    objUserProfileVM.ResetForm();
                    window.location.reload();
                },
                error: function (err) {
                    alert(err.responseText);
                }
            });
        }
    }

    self.Permission = function (eRow) {
        $('#Tab1').removeClass('active');
        $('#tab_1_1').removeClass('active');
        $('#Tab3').addClass('active');
        $('#tab_1_3').addClass('active');
        objUserProfileVM.PageList.removeAll();
        objUserProfileVM.BindPermission(eRow);
    }

    self.Param_flag = ko.observable(true);
    if (authority_Id == 5 || authority_Id == 4) {
        self.Param_flag(false);
    }

    self.Param = function (eRow) {
        $('#Tab1').removeClass('active');
        $('#tab_1_1').removeClass('active');
        $('#Tab2').addClass('active');
        $('#tab_1_2').addClass('active');
        objUserProfileVM.ParamList.removeAll();
        objUserProfileVM.BindParam(eRow);
    }
}


function UserProfileViewModel() {
    if (authority_Id == 5)
    { $("#Tab2").hide(); }
    $("#Tab2").css("pointer-events", "none");
    $("#Tab3").css("pointer-events", "none");

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
    // self.Department = ko.observable();

    self.BindUserProfile = function () {
        AppCommonScript.ShowWaitBlock();
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
                    $.localStorage("VIDS",response)

                },
                error: function (jqxhr) {

                    Failed(JSON.parse(jqxhr.responseText));
                }
            });

        var AuthId = $.localStorage("AuthId")
        var VendId = $.localStorage("VIDS");
        var urlPath = window.location.pathname;
        var AuthIDbyURL = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
         AuthIDbyURL = AuthIDbyURL.split('-', 2)

        //  alert(AuthIDbyURL[0])
       
         var AdminPropId = AuthIDbyURL[1];
      
        AuthIDbyURL = AuthIDbyURL[0];
       
      
        if (AuthIDbyURL == 5 || AuthIDbyURL == 4 || AuthIDbyURL == 5) {

            $('#divAdmin').hide()
        }
        if (AuthIDbyURL == 5 || AuthIDbyURL == 4 || AuthIDbyURL == 5) {

            $('#divAdmin').hide()
        }
        else if (AuthIDbyURL == 1 || AuthIDbyURL == 2) {

            $('#divProperty').hide()
        }

        $.ajax({
            type: "GET",
            url: "/api/UserParam/Bind",
            contentType: "application/json; charset=utf-8",
            data: { AuthId: AuthIDbyURL, UserId: VendId, PropId: AdminPropId },
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.Table.length; i++) {
                    self.UserProfileList.push(new UserProfileModel(data.Table[i]));
                }
            },
            error: function (err) {
             //   alert(err.responseText);
            }
        }).done(function () {
            ko.applyBindings(objUserProfileVM, document.getElementById("tab-content"));
            $(".ProfileGrid").DataTable({ responsive: true, 'iDisplayLength': 15 });
            $('#DataTables_Table_0_length').css('display', 'none')

            AppCommonScript.HideWaitBlock();
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
            AuthIDbyURL = AuthIDbyURL.split('-', 2)
            //  alert(AuthIDbyURL[0])
            var AdminPropId = AuthIDbyURL[1];
            AuthIDbyURL = AuthIDbyURL[0];
            objUser.Authority_Id(AuthIDbyURL)
            objUser.UserType(AdminPropId)
        }
        objUser.User_Name(self.User_Name());
        objUser.Firstname(self.Firstname());
        objUser.Lastname(self.Lastname());
        objUser.Pswd(self.Pswd());

        AppCommonScript.ShowWaitBlock();
 
        if (objUser.UserType() == -1)
        {
            var result = { Status: true, ReturnMessage: { ReturnMessage: 'Please a select property' }, ErrorType: "Success" };
            Failed(result);
        }
        else
            {
        $.ajax({
            type: "POST",
            url: "/api/UserParam/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objUser),
            cache: false,
            success: function (recorde) {
                OnSuccessSaveUserProfile(recorde);
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Profile Created Successfully' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
        }
    }

    self.UpdateProfile = function () {
        var objUser = new UserProfileModel();
        objUser.User_Id(self.User_Id());
        objUser.User_Name(self.User_Name());
        objUser.Firstname(self.Firstname());
        objUser.Lastname(self.Lastname());
        objUser.Authority_Id(self.Selected_Type());
        objUser.Pswd(self.Pswd());

        $.ajax({
            type: "POST",
            url: "/api/UserParam/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objUser),
            cache: false,
            success: function (response) {
                window.location.reload();
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

        self.Selected_Type("");
        self.Selected_Prop("");
    }

    self.Cancel = function () {
        $("#btnCancel").hide();
        $("#btnUpdate").hide();
        $("#btnSave").show();
        $("#btnReset").show();
        self.ResetForm();
        HideCreate();
    }
    OnSuccessSaveUserProfile = function (data) {
        for (var i = 0; i < data.Table1.length; i++) {
            self.UserProfileList.unshift(new UserProfileModel(data.Table1[i]));
        }
        self.ResetForm();
    }

    // new Modified for Perameters
    self.ParamList = ko.observableArray();
    self.ParamEdit = ko.observableArray();
    self.BindParam = function (data) {
        if (data != null) {
            userTypeId = data.Usertype_Id();
            userFullName = data.FullName();
            $('#UserName').empty();
            $('#UserName').append(data.FullName());
        }
        else {
            $('#UserName').empty();
            $('#UserName').append(userFullName);
        }
        ko.cleanNode(objUserProfileVM.ParamList, document.getElementById("divParamGrid"));
        $.ajax({
            type: "Get",
            url: "/api/UserParam/GetParam",
            contentType: "application/json; charset=utf-8",
            data: { propId: userTypeId, authId: authority_Id },
            dataType: "json",
            success: function (data) {
                self.ParamList.removeAll();
                for (var i = 0; i < data.Table2.length; i++) {
                    self.ParamList.push(new ParamModel(data.Table2[i]));
                }
                self.ParamEdit.removeAll();
                for (var i = 0; i < data.Table3.length; i++) {
                    self.ParamEdit.push(new ParamHiddenGemsModel(data.Table3[i]));
                }
                ko.applyBindings(objUserProfileVM.ParamList, document.getElementById("divParamGrid"));
                ko.applyBindings(objUserProfileVM.ParamEdit, document.getElementById("divParamPopup"));
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {

            $(".ParamGrid").DataTable({ responsive: true, 'iDisplayLength': 15 });
        });
    }

    // new Modified for Permission
    self.PageList = ko.observableArray();

    self.BindPermission = function (data) {
        $('#UserName').empty();
        $('#UserName').append(data.FullName());
        var userId = data.User_Id();
        $.ajax({
            type: "GET",
            url: "/api/UserParam/GetPermission",
            contentType: "application/json; charset=utf-8",
            data: { authId: authority_Id, userId: userId },
            dataType: "json",
            success: function (data) {
                ko.cleanNode(objUserProfileVM.PageList, document.getElementById("divPermissionGrid"))
                for (var i = 0; i < data.Table1.length; i++) {
                    self.PageList.push(new PermissionModel(data.Table1[i], userId));
                }

                ko.applyBindings(objUserProfileVM.PageList, document.getElementById("divPermissionGrid"));
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {

            $(".PermissionGrid").DataTable({ responsive: true, 'iDisplayLength': 15 });
        });
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
    var Props = ko.observableArray([{ PropId: -1, PropName: 'Select Property' }]);
    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        dataType: "json",
        success: function (response) {
            if (response == 0) {
            
                if (authority_Id == 1 )
                    var abc=0
            // window.location.href = '../../Vendor/Create';
                else if (authority_Id == 2)
                    var abc=0
                  // window.location.href = '../../Vendor/Create';
                    else
                window.location.href = '../../Vendor/Create';
            }
            else {

                $.localStorage("VendId", response)
              //  window.location.href = 'Create';
            }
        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
    $.ajax({
        type: "GET",
        url: "/api/vendors/allproperty",
        data: { VendorId:  $.localStorage("VendId") },
        dataType: "json",
        success: function (result) {
            ko.utils.arrayMap(result, function (item) {
                Props.push({ PropId: item.PropId, PropName: item.PropName });
            });
        }
    });
    return Props;
}
// For Perameters

function ParamModel(data) {

    var self = this;
    var data = data || {};
    self.Id = ko.observable(data.Id || '');
    self.Vndr_Id = ko.observable(data.Vndr_Id || '0');
    self.Vparam_Code = ko.observable(data.Vparam_Code || '');
    self.ViewBtnFlag = ko.observable(false);
    if (data.Vparam_Code == 'PC0001') {
        self.PramType = ['Date'];
        self.Vparm_Type = ko.observable(data.Vparm_Type.trim() || 'Date');
        self.Vparam_Val = ko.observable(data.Vparam_Val || '0');
    }
    else if (data.Vparam_Code == 'PC0004') {
        self.PramType = ['ON', 'OFF'];
        self.Vparm_Type = ko.observable(data.Vparm_Type.trim() || 'OFF');
        self.Vparam_Val = ko.observable(data.Vparam_Val || '0');
        self.ViewBtnFlag(true);

    }
    else if (data.Vparam_Code == 'PC0006') {
        self.PramType = ['LMK', 'External'];
        self.Vparm_Type = ko.observable(data.Vparm_Type.trim() || 'External');
        self.Vparam_Val = ko.observable(data.Vparam_Val || '0');
    }
    else {
        self.PramType = ['Value', 'Percentage'];
        self.Vparm_Type = ko.observable(data.Vparm_Type || 'Value');
        self.Vparam_Val = ko.observable(data.Vparam_Val || '0');
    }
    self.Vparam_Descr = ko.observable(data.Vparam_Descr || '');

    self.Permissionselection = ['true', 'false'];
    self.Permission_flag = ko.observable()
    if (data.Permission == null)
        self.Permission_flag('false');
    else
        self.Permission_flag(data.Permission.trim());

    self.Active_flag = ko.observable(self.Permission_flag() == 'true' ? true : false);
    self.Value_ModifyFlag = ko.observable(true);
    if (data.Vparam_Code == 'PC0004' || data.Vparam_Code == 'PC0006') {
        self.Value_ModifyFlag(false);
    }
    self.Param_permission_flag = ko.observable(false);

    if (authority_Id <= 2) {
        self.Param_permission_flag(true);
        $('#param_col_permi').show();
    }

    self.change = ko.observable(false);
    self.editParamValue = function () {
        self.change(true);
    }
    self.SaveValue = function (eRow) {
        if (eRow.Vparam_Val() != '') {
            UpdateParametersValue(eRow)
        }
        else {
            self.Vparam_Val('0');
            UpdateParametersValue(eRow)
        }
    }

    self.selectionChanged = function (eRow) {
        $.ajax({
            type: "Get",
            url: "/api/UserParam/UpdateParamType",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: eRow.Id(), type: eRow.Vparm_Type() },
            success: function (recorde) {
                if (eRow.Vparm_Type() == 'ON' || eRow.Vparm_Type() == 'OFF') {
                    if (eRow.Vparm_Type() == 'ON')
                        eRow.Vparam_Val('1');
                    else
                        eRow.Vparam_Val('0');
                }
                else if (eRow.Vparm_Type() == 'LMK' || eRow.Vparm_Type() == 'External') {
                    if (eRow.Vparm_Type() == 'LMK')
                        eRow.Vparam_Val('1');
                    else
                        eRow.Vparam_Val('0');
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.permissionChanged = function (eRow) {

        UpdateParametersPermission(eRow)
        eRow.Active_flag(eRow.Permission_flag() == 'true' ? true : false);
    }

}
function ParamHiddenGemsModel(data) {

 
    var self = this;
    var data = data || {};
    self.Id = ko.observable(data.Id || '');
    self.P_Descr = ko.observable(data.Vparam_Descr);

    self.editParamHiddenGems = function (eRow) {

        $.ajax({
            type: "Get",
            url: "/api/UserParam/UpdateParamHidden",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: eRow.Id(), value: eRow.P_Descr() },
            success: function (recorde) {
                var result = { Status: true, ReturnMessage: { ReturnMessage: "Updated Successfully" }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }


}
function UpdateParametersPermission(data) {
    $.ajax({
        type: "GET",
        url: "/api/UserParam/UpdateParamPermission",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Id: data.Id(), flag: data.Permission_flag() },
        success: function (recorde) {
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}

function UpdateParametersType(data) {

}

function UpdateParametersValue(data) {
    if (data.Vparam_Val() >= 16) {
     
        var result = { Status: true, ReturnMessage: { ReturnMessage: "Not more then 15" }, ErrorType: "success" };
        Failed(result);
        objUserProfileVM.BindParam(null);
    }
    else {
        $.ajax({
            type: "GET",
            url: "/api/UserParam/UpdateParamValue",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: data.Id(), value: data.Vparam_Val() },
            success: function (recorde) {
                var result = { Status: true, ReturnMessage: { ReturnMessage: "Update Successfully" }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


// For Permission

function PermissionModel(data, userId) {
    
    var self = this;
    var data = data || {};
    self.User_Id = ko.observable(userId);
    self.Page_Id = ko.observable(data.Page_Id || '');
    self.Url = ko.observable(data.Page || '');
    self.Page = ko.observable(data.Page || '');

    self.Active_flag = ko.observable(data.Active_flag == 'false     ' ? false : true);

    self.StatusImage = ko.observable(self.Active_flag() == true ? '/img/Splitted_Images/Active%20Button.png' : '/img/Splitted_Images/Inactive%20Button.png');

    self.Update = function (eRow) {
        if (eRow.Active_flag() != true) {
            eRow.Active_flag(true)
            eRow.StatusImage('/img/Splitted_Images/Active%20Button.png');
            UpdateActivePermission(eRow)
        }
        else {
            eRow.Active_flag(false)
            eRow.StatusImage('/img/Splitted_Images/Inactive%20Button.png');
            UpdateActivePermission(eRow)
        }

    }
}

function UpdateActivePermission(data) {
    
    $.ajax({
        type: "Get",
        url: "/api/UserParam/UpdatePermission",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { userId: data.User_Id(), pageId: data.Page_Id(), flag: data.Active_flag() },
        success: function (response) {
            Successed(response)
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
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


function Create() {
    $('#divCreateProfile').show();
    $('#btnCreate').hide();
    $('#DivPwd').show();
    document.getElementById("EMAIL").readOnly = false;

}
function HideCreate() {
    $('#divCreateProfile').hide();
    $('#btnCreate').show();
    $('#UserName').empty();
    $('#UserName').append('Manage Users');
    $('#DivPwd').hide();
}