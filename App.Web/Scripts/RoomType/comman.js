function RoomtypeModel(data) {
    var self = this;
    var data = data || {};
    self.Room_TypeId = ko.observable(data.Room_TypeId || '');
    self.Room_Name = ko.observable(data.Room_Name || '');
    self.Room_Descr = ko.observable(data.Room_Descr || '');

    self.Edit = function (eRow) {
        objRoomtypeVM.Room_TypeId(eRow.Room_TypeId());
        objRoomtypeVM.Room_Name(eRow.Room_Name());
        objRoomtypeVM.Room_Descr(eRow.Room_Descr());
        $('#divCreateRoomType').show();
        $('#btnCreate').hide();
        $("#btnSave").hide();
        $("#btnReset").hide();
        $("#btnUpdate").show();
    }
    self.Suspend = function (eRow) {
        if (confirm('Are you sure ?')) {
            $.ajax({
                type: "POST",
                url: "/api/RoomType/Suspend",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON(eRow),
                cache: false,
                success: function (response) {
                    ;
                    objRoomtypeVM.RoomtypeList.remove(eRow);
                    Successed(JSON.parse('Suspended Successfully'))
                    window.location.reload();
                },
                error: function (err) {
                    Failed(JSON.parse(err.responseText));
                }
            });
        }
    }
}

function RoomTypeViewModel() {
    var self = this;
    self.RoomtypeList = ko.observableArray();

    self.Room_TypeId = ko.observable();
    self.Room_Name = ko.observable();
    self.Room_Descr = ko.observable();

    self.BindRoomtype = function () {
        $.ajax({
            type: "Get",
            url: "/api/RoomType/Bind",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.RoomtypeList.push(new RoomtypeModel(data[i]));
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(objRoomtypeVM, document.getElementById("divRoomTypes"));
            $('.RoomTypeGrid').DataTable({ responsive: true, 'iDisplayLength': 15 });
        });
    }

    self.SaveRoomtype = function () {
        var objRoom = new RoomtypeModel();
        objRoom.Room_TypeId(self.Room_TypeId());
        objRoom.Room_Name(self.Room_Name());
        objRoom.Room_Descr(self.Room_Descr());

        $.ajax({
            type: "POST",
            url: "/api/RoomType/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objRoom),
            cache: false,
            success: function (recorde) {
                OnSuccessSaveRoomtype(recorde);
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Saved Successfully' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.UpdateRoomType = function () {
        var objRoom = new RoomtypeModel();
        objRoom.Room_TypeId(self.Room_TypeId());
        objRoom.Room_Name(self.Room_Name());
        objRoom.Room_Descr(self.Room_Descr());
        $.ajax({
            type: "POST",
            url: "/api/RoomType/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objRoom),
            cache: false,
            success: function (response) {
                 location.reload();
                //self.RoomtypeList.remove(new RoomtypeModel(objRoom))
                //self.RoomtypeList.push(new RoomtypeModel(objRoom))
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Successfully Updated' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.Room_TypeId("");
        self.Room_Name("");
        self.Room_Descr("");
    }

    self.Cancel = function () {

        self.ResetForm();
        $("#btnUpdate").hide();
        $("#btnSave").show();
        $("#btnReset").show();
        $('#divCreateRoomType').hide();
        $('#btnCreate').show();
    }

    OnSuccessSaveRoomtype = function (data) {
        for (var i = 0; i < data.Table.length; i++) {
            self.RoomtypeList.unshift(new RoomtypeModel(data.Table[i]));
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
var objRoomtypeVM = new RoomTypeViewModel();
objRoomtypeVM.BindRoomtype();

$(document).ready(function () {
    $("#btnCreate").click(function () {
        $("#divCreateRoomType").show();
        $("#btnCreate").hide();
    });
});