function CC_AvenueModel(data) {
    
    var self = this;
    var data = data || {};
    self.Cav_Id = ko.observable(data.Cav_Id || '');
    self.Cav_Name = ko.observable(data.Cav_Name || '');
    self.Cav_Percent = ko.observable(data.Cav_Percent || '');
    self.Cav_Descr = ko.observable(data.Cav_Descr || '');
    self.Cav_Ipaddress = ko.observable(data.Cav_Ipaddress || '');
    self.Cav_Regist_On = ko.observable(data.Cav_Ipaddress || '');
    self.Cav_Modified_On = ko.observable(data.Cav_Ipaddress || '');
    //self.Cav_Active_flag = ko.observable(data.Cav_Ipaddress || '');

    self.Edit = function (eRow) {
        objCC_AvenueVM.Cav_Id(eRow.Cav_Id());
        objCC_AvenueVM.Cav_Name(eRow.Cav_Name());
        objCC_AvenueVM.Cav_Percent(eRow.Cav_Percent());
        objCC_AvenueVM.Cav_Descr(eRow.Cav_Descr());
        $('#divCreateCC_Avenue').show();
        $("#btnSave").hide();
        $("#btnReset").hide();
        $("#btnUpdate").show();
    }
}

function CC_AvenueViewModel() {
    
    var self = this;
    self.CC_AvenueList = ko.observableArray();

    self.Cav_Id = ko.observable();
    self.Cav_Name = ko.observable();
    self.Cav_Percent = ko.observable();
    self.Cav_Descr = ko.observable();

    self.BindCC_Avenue = function () {
       
        $.ajax({
            type: "Get",
            url: "/api/CCAvenue/Bind",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {
                    self.CC_AvenueList.push(new CC_AvenueModel(data[i]));
                }
            },
            error: function (err) {
                
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(objCC_AvenueVM, document.getElementById("divCC_Avenue"));
            $('.CC_AvenueGrid').DataTable({ responsive: true, 'iDisplayLength': 15 });
        });
    }

    self.SaveCC_Avenue = function () {
        
        var objCC_Avenue = new CC_AvenueModel();
        objCC_Avenue.Cav_Id(self.Cav_Id());
        objCC_Avenue.Cav_Name(self.Cav_Name());
        objCC_Avenue.Cav_Percent(self.Cav_Percent());
        objCC_Avenue.Cav_Descr(self.Cav_Descr());
        objCC_Avenue.Cav_Ipaddress('10.10.10.01');
        //$.getJSON("http://jsonip.com?callback=?", function (data) {
        //    
        //    objCC_Avenue.Cav_Ipaddress(data.ip);
        //});
        var date = new Date();
        objCC_Avenue.Cav_Regist_On = ko.observable(date);
        objCC_Avenue.Cav_Modified_On = ko.observable(date);       
        //objCC_Avenue.Cav_Active_flag = 1;

        $.ajax({
            type: "POST",
            url: "/api/CCAvenue/create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objCC_Avenue),
            cache: false,
            success: function (recorde) {
                
                OnSuccessSaveCC_Avenue(recorde);
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Saved Successfully' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.UpdateCC_Avenue = function () {
        
        var objCC_Avenue = new CC_AvenueModel();
        objCC_Avenue.Cav_Id(self.Cav_Id());
        objCC_Avenue.Cav_Name(self.Cav_Name());
        objCC_Avenue.Cav_Percent(self.Cav_Percent());
        objCC_Avenue.Cav_Descr(self.Cav_Descr());
        objCC_Avenue.Cav_Ipaddress('10.10.10.01');
        //$.getJSON("http://jsonip.com?callback=?", function (data) {
        //    
        //    objCC_Avenue.Cav_Ipaddress(data.ip);
        //});
        var date = new Date();
        objCC_Avenue.Cav_Modified_On = ko.observable(date);

        $.ajax({
            type: "POST",
            url: "/api/CCAvenue/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(objCC_Avenue),
            cache: false,
            success: function (response) {
                location.reload();
                //self.CC_AvenueList.remove(new RoomtypeModel(objRoom))
                //self.CC_AvenueList.push(new RoomtypeModel(objRoom))
                var result = { Status: true, ReturnMessage: { ReturnMessage: 'Successfully Updated' }, ErrorType: "Success" };
                Successed(result);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.ResetForm = function () {
        self.Cav_Id("");
        self.Cav_Name("");
        self.Cav_Descr("");
    }

    self.Cancel = function () {
        
        self.ResetForm();
        $("#btnUpdate").hide();
        $("#btnSave").show();
        $("#btnReset").show();
        $('#divCreateCC_Avenue').hide();
        $('#btnCreate').show();
    }

    OnSuccessSaveCC_Avenue = function (data) {
        self.CC_AvenueList.removeAll();
        for (var i = 0; i < data.Table.length; i++) {            
            self.CC_AvenueList.unshift(new CC_AvenueModel(data.Table[i]));
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
var objCC_AvenueVM = new CC_AvenueViewModel();
objCC_AvenueVM.BindCC_Avenue();

$(document).ready(function () {
    $("#btnCreate").click(function () {
        $("#divCreateCC_Avenue").show();
        $("#btnCreate").hide();
    });
});