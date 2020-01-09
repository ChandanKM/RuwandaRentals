function ParamModel(data) {
    var self = this;
    var data = data || {};
    self.Id = ko.observable(data.Id || '');
    self.Vndr_Id = ko.observable(data.Vndr_Id || '');
    self.Vparam_Code = ko.observable(data.Vparam_Code || '');
    self.Vparam_Descr = ko.observable(data.Vparam_Descr || '');
    self.Vparm_Type = ko.observable(data.Vparm_Type || '');
    self.Vparam_Val = ko.observable(data.Vparam_Val || '');

    self.isActive = ko.observable(data.Vparam_Active_Flag == "True" ? 'block' : 'block');

    self.Edit = function (eRow) {
        objParamVM.Id(eRow.Id());
        objParamVM.Vndr_Id(eRow.Vndr_Id());
        objParamVM.Vparam_Code(eRow.Vparam_Code());
        objParamVM.Vparam_Descr(eRow.Vparam_Descr());
        objParamVM.SelectedParamType(eRow.Vparm_Type());
        objParamVM.Vparam_Val(eRow.Vparam_Val());
        $("#btnCancelParam").show();
        $("#btnUpdateParam").show();
        $("#btnResetParam").hide();
        $("#btnSaveParam").hide();
    }

    self.Suspend = function (eRow) {
        $.ajax({
            type: "POST",
            url: "/api/UserParam/suspendParam",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(eRow),
            cache: false,
            success: function (response) {
                objParamVM.ParamList.remove(eRow);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
}

function ParamViewModel() {
    var self = this;
    self.ParamList = ko.observableArray();

    self.Id = ko.observable();
    self.Vndr_Id = ko.observable();
    self.Vparam_Code = ko.observable();
    self.Vparam_Descr = ko.observable();
    self.Vparam_Val = ko.observable();
    self.PramType = ko.observableArray(['Value', 'Percentage']);
    self.SelectedParamType = ko.observable("Value");
    self.ParamSign = ko.observable();
    self.ParamSign = self.SelectedParamType() == 'Value' ? 'value' : '%';
    self.BindParam = function () {
        $.ajax({
            type: "POST",
            url: "/api/UserParam/getParam",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.Table.length; i++) {
                    self.ParamList.push(new ParamModel(data.Table[i]));
                }
                $("#btnUpdateParam").hide();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(objParamVM, document.getElementById("tab_1_2"));
            $(".ParamGrid").DataTable({ responsive: true });
        });
    }

    self.SaveParam = function () {
        var objParam = new ParamModel();
        objParam.Id(self.Id());
        objParam.Vndr_Id(self.Vndr_Id());
        objParam.Vparam_Code(self.Vparam_Code());
        objParam.Vparam_Descr(self.Vparam_Descr());
        objParam.Vparm_Type(self.SelectedParamType());
        objParam.Vparam_Val(self.Vparam_Val());

        $.ajax({
            type: "POST",
            url: "/api/UserParam/addParam",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objParam),
            cache: false,
            success: function (crntRow) {
                OnSuccessSaveParam(crntRow);
                Successed(JSON.parse('Parameters Successfully Created'))
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
                ResetForm();
            }
        });
    }

    self.UpdateParam = function () {
        var objParam = new ParamModel();
        objParam.Id(self.Id());
        objParam.Vndr_Id(self.Vndr_Id());
        objParam.Vparam_Code(self.Vparam_Code());
        objParam.Vparam_Descr(self.Vparam_Descr());
        objParam.Vparm_Type(self.SelectedParamType());
        objParam.Vparam_Val(self.Vparam_Val());
        $.ajax({
            type: "POST",
            url: "/api/UserParam/updateParam",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objParam),
            cache: false,
            success: function (crntRow) {
                location.reload();
                ResetForm();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.Id("");
        self.Vndr_Id("");
        self.Vparam_Code("");
        self.Vparam_Descr("");
        self.SelectedParamType("");
        self.Vparam_Val("");
    }
    self.Cancel = function () {
        $("#btnCancelParam").hide();
        $("#btnUpdateParam").hide();
        $("#btnSaveParam").show();
        $("#btnResetParam").show();
        self.ResetForm();
    }
    OnSuccessSaveParam = function (data) {
        for (var i = 0; i < data.Table.length; i++) {
            self.ParamList.unshift(new ParamModel(data.Table[i]));
        }
        self.ResetForm();
    }
}

var objParamVM = new ParamViewModel();
objParamVM.BindParam();


function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}