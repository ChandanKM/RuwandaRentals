var urlPath = window.location.pathname;
var PID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function DayModel(data) {
    var self = this;
    self.Room_Id = ko.observable(data.Room_Id || '');
    self.Room_Name = ko.observable(data.Room_Name || '');
    self.Vndr_Amnt = ko.observable(data.Vndr_Amnt || '0');
    self.Inv_Id = ko.observable(data.Id || '0');
    self.Price = ko.observable(data.lmk_Amnt || '0');
    self.change = ko.observable(false);
    self.editPrice = function () {
        self.change(true);
    }
    self.SavePrice = function (eRow) {
        if (eRow.Price() != '') {
            UpdatePrice(eRow);
        }
        else {
            self.Price('0');
            UpdatePrice(eRow);
        }
    }

    self.changeVndr_Amnt = ko.observable(false);
    self.editVndr_Amnt = function () {
            self.changeVndr_Amnt(true);
    }
    self.SaveVndr_Amnt = function (eRow) {
        if (eRow.Vndr_Amnt() != '') {
            UpdateVndr_Amnt(eRow);
        }
        else {
            self.Vndr_Amnt('0');
            UpdateVndr_Amnt(eRow);
        }
    }

    self.Available = ko.observable(data.Inventory_Avail_Rooms || '0');
    self.Hold_Room = ko.observable(data.Inventory_Hold_Rooms || '0');

    self.up = function (eRow) {
        eRow.Available(parseInt(eRow.Available()) + 1);
        UpdateAvailablity(eRow);
    }

    self.down = function (eRow) {
     //   
        if (eRow.Available() > '0') {
            if (eRow.Available() > eRow.Hold_Room()) {
                eRow.Available(parseInt(eRow.Available()) - 1);
                UpdateAvailablity(eRow);
            }
            else { alert('Available Rooms Should Be More Then Hold Rooms!Current Hold room is:' + eRow.Hold_Room()) }
        }
    }
}
function Model() {
    this.Inv_Id = ko.observable()
    this.Available = ko.observable()
    this.Price = ko.observable()
    this.Vndr_Amnt = ko.observable()
}

function RoomRateViewModel() {
    var self = this;
    
    self.DayList = ko.observableArray([]);
    self.DayList1 = ko.observableArray([]);
    self.DayList2 = ko.observableArray([]);
    self.DayList3 = ko.observableArray([]);
    self.DayList4 = ko.observableArray([]);
    self.DayList5 = ko.observableArray([]);
    self.DayList6 = ko.observableArray([]);
    self.DayList7 = ko.observableArray([]);
    self.DayList8 = ko.observableArray([]);
    self.DayList9 = ko.observableArray([]);
    self.DayList10 = ko.observableArray([]);
    self.DayList11 = ko.observableArray([]);
    self.DayList12 = ko.observableArray([]);
    self.DayList13 = ko.observableArray([]);
    self.DayList14 = ko.observableArray([]);
    self.date = ko.observable(GetDate(0));
    self.date1 = ko.observable(GetDate(1));
    self.date2 = ko.observable(GetDate(2));
    self.date3 = ko.observable(GetDate(3));
    self.date4 = ko.observable(GetDate(4));
    self.date5 = ko.observable(GetDate(5));
    self.date6 = ko.observable(GetDate(6));
    self.date7 = ko.observable(GetDate(7));
    self.date8 = ko.observable(GetDate(8));
    self.date9 = ko.observable(GetDate(9));
    self.date10 = ko.observable(GetDate(10));
    self.date11 = ko.observable(GetDate(11));
    self.date12 = ko.observable(GetDate(12));
    self.date13 = ko.observable(GetDate(13));
    self.date14 = ko.observable(GetDate(14));

    self.BindRoomRate = function () {

        var PropID = PID;

        AppCommonScript.ShowWaitBlock();
      

        $.ajax({
            type: "Get",
            url: '/api/vendors/GetRooms',
            data: { propId: PropID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                if (data.Table2.length!= 0) {
                 //   
                    $('#SelectedProperty').html(data.Table[0].Prop_Name)
                    var counter = data.Table1[0].Column1
                    var limit = 15;
                    for (var i = counter + 1; i <= limit; i++) {

                        $('#Row' + i + '').hide()
                        $('#tdRow' + i + '').hide()
                    }

                //    
                    if (data.Table2 != null) {
                        for (var i = 0; i < data.Table2.length; i++) {
                            self.DayList.push(new DayModel(data.Table2[i]));
                        }
                    }
                    if (data.Table3 != null) {
                        for (var i = 0; i < data.Table3.length; i++) {
                            self.DayList1.push(new DayModel(data.Table3[i]));
                        }
                    }
                    if (data.Table4 != null) {
                        for (var i = 0; i < data.Table4.length; i++) {
                            self.DayList2.push(new DayModel(data.Table4[i]));
                        }
                    }
                    if (data.Table5 != null) {
                        for (var i = 0; i < data.Table5.length; i++) {
                            self.DayList3.push(new DayModel(data.Table5[i]));
                        }
                    }
                    if (data.Table6 != null) {
                        for (var i = 0; i < data.Table6.length; i++) {
                            self.DayList4.push(new DayModel(data.Table6[i]));
                        }
                    }
                    if (data.Table7 != null) {
                        for (var i = 0; i < data.Table7.length; i++) {
                            self.DayList5.push(new DayModel(data.Table7[i]));
                        }
                    }
                    if (data.Table8 != null) {
                        for (var i = 0; i < data.Table8.length; i++) {
                            self.DayList6.push(new DayModel(data.Table8[i]));
                        }
                    }
                    if (data.Table9 != null) {
                        for (var i = 0; i < data.Table9.length; i++) {
                            self.DayList7.push(new DayModel(data.Table9[i]));
                        }
                    }
                    if (data.Table10 != null) {
                        for (var i = 0; i < data.Table10.length; i++) {
                            self.DayList8.push(new DayModel(data.Table10[i]));
                        }
                    }
                    if (data.Table11 != null) {
                        for (var i = 0; i < data.Table11.length; i++) {
                            self.DayList9.push(new DayModel(data.Table11[i]));
                        }
                    }
                    if (data.Table12 != null) {
                        for (var i = 0; i < data.Table12.length; i++) {
                            self.DayList10.push(new DayModel(data.Table12[i]));
                        }
                    }
                    if (data.Table13 != null) {
                        for (var i = 0; i < data.Table13.length; i++) {
                            self.DayList11.push(new DayModel(data.Table13[i]));
                        }
                    }
                    if (data.Table14 != null) {
                        for (var i = 0; i < data.Table14.length; i++) {
                            self.DayList12.push(new DayModel(data.Table14[i]));
                        }
                    }
                    if (data.Table15 != null) {
                        for (var i = 0; i < data.Table15.length; i++) {
                            self.DayList13.push(new DayModel(data.Table15[i]));
                        }
                    }
                    if (data.Table16 != null) {
                        for (var i = 0; i < data.Table16.length; i++) {
                            self.DayList14.push(new DayModel(data.Table16[i]));
                        }
                    }
                }
                else
                {

                    $('#RoomTypeGrid').css('display', 'none')
                    $('#nodatavail').css('display', 'block')
                    $('#breadcrumbs').css('display', 'none')
                    $('#btnhide').css('display', 'none')
                    
                    
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(roomrateVM, document.getElementById('RoomTypeGrid'));
            //  $('.RoomTypeGrid').DataTable({ responsive: true });
            AppCommonScript.HideWaitBlock();
        });

    }

    self.Update = function (data) {

        $.ajax({
            type: "POST",
            url: "/api/RoomType/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            cache: false,
            success: function (response) {
                location.reload();
                Successed(JSON.parse('Successfully Updated'))
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    OnSuccessSaveRoomRates = function (data) {
        for (var i = 0; i < data.length; i++) {
            self.RoomRateList.push(new RoomRateCalenderModel(data[i]));
        }
    }

}
function Reload() {

    window.location.reload();
}
function UpdateAvailablity(data) {
    var obj = new Model();
    obj.Inv_Id(data.Inv_Id());
    obj.Available(data.Available());
    obj.Price(data.Price());
    $.ajax({
        type: "GET",
        url: "/api/vendors/updateAvailablity",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Inv_Id: obj.Inv_Id(), Available: obj.Available() },
        success: function (recorde) {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Updated Successfully" }, ErrorType: "Success" };
            Successed(result);
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}

function UpdatePrice(data) {
    var obj = new Model();
    obj.Inv_Id(data.Inv_Id());
    obj.Available(data.Available());
    obj.Price(data.Price());
    
    $.ajax({
        type: "GET",
        url: "/api/vendors/updateRates",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Inv_Id: obj.Inv_Id(), Price: obj.Price() },
        success: function (recorde) {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Update Successfully" }, ErrorType: "Success" };
            Successed(result);
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}


function UpdateVndr_Amnt(data) {
    var obj = new Model();
    obj.Inv_Id(data.Inv_Id());
    obj.Available(data.Available());
    obj.Vndr_Amnt(data.Vndr_Amnt());
    
    $.ajax({
        type: "GET",
        url: "/api/vendors/updaterackRates",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Inv_Id: obj.Inv_Id(), Vndr_Amnt: obj.Vndr_Amnt() },
        success: function (recorde) {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Update Successfully" }, ErrorType: "Success" };
            Successed(result);
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}
var roomrateVM = new RoomRateViewModel();
roomrateVM.BindRoomRate();

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function GetDate(a) {
    var today = new Date();
    var tomorrow = new Date(today);
 tomorrow.setDate(today.getDate() + a);
   
 return tomorrow.toDateString();
};