var urlPath = window.location.pathname;
var PID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

//  alert(AuthIDbyURL[0])
var ParamList = ko.observableArray();
var ParamEdit = ko.observableArray();

var userTypeId = null;

function UserProfileModel(data) {
    var self = this;
    var data = data || {};
   


   

    self.Param_flag = ko.observable(true);


    self.Param = function () {
       
        ParamList.removeAll();
        objUserProfileVM.BindParam();
    }
}


function UserProfileViewModel() {
  
    var self = this;
    
    self.BindParam = function () {
       
       ko.cleanNode(ParamList, document.getElementById("divParamGrid"));       
        $.ajax({
            type: "Get",
            url: "/api/UserParam/GetParam",
            contentType: "application/json; charset=utf-8",
            data: { propId: PID, authId: 4 },
            dataType: "json",
            //async:false,
            success: function (data) {
                
                ParamList.removeAll();
                for (var i = 0; i < data.Table2.length; i++) {
                    ParamList.push(new ParamModel(data.Table2[i]));
                }
                ParamEdit.removeAll();
                for (var i = 0; i < data.Table3.length; i++) {
                    ParamEdit.push(new ParamHiddenGemsModel(data.Table3[i]));
                }
                ko.applyBindings(ParamList, document.getElementById("divParamGrid"));
                ko.applyBindings(ParamEdit, document.getElementById("myModalPolicy"));
                $(".ParamGrid").DataTable({ responsive: true, 'iDisplayLength': 15 });
                $('#DataTables_Table_0_length').css('display', 'none')
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {

          
        });
    }

    // new Modified for Permission
    self.PageList = ko.observableArray();

  
}

var objUserProfileVM = new UserProfileViewModel();
objUserProfileVM.BindParam();

var urlPath = window.location.pathname;
var PID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);


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



function UpdateParametersValue(data) {
    if (data.Vparam_Val() >= 16) {
        
        var result = { Status: true, ReturnMessage: { ReturnMessage: "Not more than 15" }, ErrorType: "success" };
        Failed(result);
        //$('#VpValues').val(data.Vparam_Val());
        //objUserProfileVM.BindParam();
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
function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}