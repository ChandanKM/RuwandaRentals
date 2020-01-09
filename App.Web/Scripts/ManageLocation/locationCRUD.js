function ManageLocationModel(data) {
    var self = this;
    var data = data || {};
    self.Id = ko.observable(data.Id || '');
    self.City = ko.observable(data.City || '');
    self.Location = ko.observable(data.Location || '');
    self.Pincode = ko.observable(data.Pincode || '');
    self.State = ko.observable(data.State || '');

    self.Edit = function (eRow) {
        objLocationVM.Id(eRow.Id());
        objLocationVM.City(eRow.City());
        objLocationVM.Location(eRow.Location());
        objLocationVM.Pincode(eRow.Pincode());
        objLocationVM.State(eRow.State());
        $("#btnCreate").hide();
        $('#divAddNewLocation').show();
        $("#btnSave").hide();
        $("#btnReset").hide();
        $("#btnUpdate").show();
    }
    self.Delete = function (eRow) {
        if (confirm('Are you sure to delete this Location ?')) {
            $.ajax({
                type: "POST",
                url: "/api/ManageLocation/delete",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON(eRow),
                cache: false,
                success: function (response) {
                    objLocationVM.ManageLocationList.remove(eRow);
                    var result = { Status: true, ReturnMessage: { ReturnMessage: 'Delete Successfully' }, ErrorType: "Success" };
                    Successed(result);
                },
                error: function (err) {
                    Failed(JSON.parse(err.responseText));
                }
            });
        }
    }
}

function ManageLocationViewModel() {
    var self = this;
    self.ManageLocationList = ko.observableArray();

    self.Id = ko.observable();
    self.City = ko.observable();
    self.Location = ko.observable();
    self.Pincode = ko.observable();
    self.State = ko.observable();

    self.BindLocation = function () {
        $.ajax({
            type: "Get",
            url: "/api/ManageLocation/Get",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.Table.length; i++) {
                    self.ManageLocationList.push(new ManageLocationModel(data.Table[i]));
                }
            },
            error: function (err) {
                alert(err.responseText);
            }
        }).done(function () {
            ko.applyBindings(objLocationVM, document.getElementById("divManageLocation"));
            $(".LocationGrid").DataTable({ responsive: true });
        });
    }

    self.Save = function () {
        var objLocation = new ManageLocationModel();
        objLocation.Id(self.Id());
        objLocation.City(self.City());
        objLocation.Location(self.Location());
        objLocation.Pincode(self.Pincode());
        objLocation.State(self.State());

        $.ajax({
            type: "POST",
            url: "/api/ManageLocation/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objLocation),
            cache: false,
            success: function (recorde) {
                OnSuccessSaveLocation(recorde);
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Location Added Successfully' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.Update = function () {
        var objLocation = new ManageLocationModel();
        objLocation.Id(self.Id());
        objLocation.City(self.City());
        objLocation.Location(self.Location());
        objLocation.Pincode(self.Pincode());
        objLocation.State(self.State());
        $.ajax({
            type: "POST",
            url: "/api/ManageLocation/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objLocation),
            cache: false,
            success: function (response) {
                location.reload();
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Update Successfully' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.Id("");
        self.City("");
        self.Location("");
        self.Pincode("");
        self.State("");
    }

    self.Cancel = function () {
        self.ResetForm();
        $("#btnUpdate").hide();
        $("#btnSave").show();
        $("#btnReset").show();
        $('#divAddNewLocation').hide();
        $('#btnCreate').show();
    }

    OnSuccessSaveLocation = function (data) {
        for (var i = 0; i < data.Table.length; i++) {
            self.ManageLocationList.unshift(new ManageLocationModel(data.Table[i]));
        }
        self.Cancel();
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
var objLocationVM = new ManageLocationViewModel();
objLocationVM.BindLocation();

$(document).ready(function () {
    $("#btnCreate").click(function () {
        $("#divAddNewLocation").show();
        $("#btnCreate").hide();
    });
});