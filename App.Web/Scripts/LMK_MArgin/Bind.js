
var resultVM = new SearchResultViewModel();
var data;
var fromdate;
var todate;
//if ($.localStorage('days') == "") {
//    data = 90;

//}
//else {
//    data = $.localStorage('days')
//}

if ($.localStorage('fromdate') == "") {
    fromdate = null;

}
else {
    fromdate = $.localStorage('fromdate')
}
if ($.localStorage('todate') == "") {
    todate = null;
}
else {
    todate = $.localStorage('todate')
}
resultVM.GetRes(data);

ko.applyBindings(resultVM, document.getElementById("BookingsDT"));



$(".BookingsDT").DataTable({
    responsive: true,
    //'iDisplayLength': 15,
});
$('#DataTables_Table_0_length').css('display', 'none')


function SearchResultViewModel() {
    var self = this;
    self.TaxList = ko.observableArray();
    self.AverageList = ko.observableArray();

    self.GetRes = function (data) {

        $.localStorage('fromdate', "");
        $.localStorage('todate', "");
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Booking/CCAvenue_Report_Margin',
            contentType: "application/json; charset=utf-8",

            data: { VendID: '', fromdate: fromdate, todate: todate },
            dataType: "json",
            async: false,
            success: function (data) {

                
                self.TaxList.removeAll();
                for (var i = 0; i < data.Table.length; i++) {

                    self.TaxList.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table1.length; i++) {

                    self.AverageList.push(new BookingClassAverage(data.Table1[i])); //Put the response in ObservableArray
                }
                AppCommonScript.HideWaitBlock();
                $.localStorage('days', 90)

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }


        });

    }

}
//Model
function BookingClass(data) {
    var prop = this;
    prop.VendorID = data["vndr_name"];
    prop.Prop_Id = ko.observable(data["prop_name"]);
    prop.Inv_Num = ko.observable(data["invce_num"]);
    prop.ClientName = ko.observable(data["Name"]);
    prop.PropName = ko.observable(data["prop_name"]);
    prop.Markup = ko.observable(data["markup"]);
    var date = new Date(data["invce_date"]).format("dd mmmm yyyy");;
    prop.Inv_Date = ko.observable(date);
    prop.Room_Rate = ko.observable(data["prop_room_rate"]);
    prop.Camo_Rate = ko.observable(data["camo_room_rate"]);
    prop.TaxAmnt = ko.observable(data["tax_amnt"]);
    prop.Net_Amnt = ko.observable(data["net_amnt"]);
    prop.PayType = ko.observable(data["credit_debit_card"]);
    prop.ccavenuepercent = ko.observable(data["ccavenuepercent"]);
    prop.ccavenuecharges = ko.observable(data["ccavenuecharges"]);

    var lmkmargin = 0.0;
    
    if (data["lmkmargin"] != null) {
         lmkmargin = data["lmkmargin"].toFixed(2);
    }
   
    prop.lmkmargin = ko.observable(lmkmargin);
    self.DT = function (data) {

    };

}
function BookingClassAverage(data) {
    var prop = this;
    
    prop.roomrate = data["margin"] == null ? 0.0 : (data["roomrate"]).toFixed(2);
    prop.sellingrate = data["sellingrate"] == null ? 0.0 : (data["sellingrate"]).toFixed(2);
    prop.tax = data["tax"] == null ? 0.0 : (data["tax"]).toFixed(2);
    prop.netamount = data["netamount"] == null ? 0.0 : (data["netamount"]).toFixed(2);
    prop.ccavenuecharges = data["ccavenuecharges"] == null ? 0.0 : (data["ccavenuecharges"]).toFixed(2);
    prop.margin = data["margin"] == null ? 0.0: (data["margin"]).toFixed(2);


}


function RefineReport() {
    var x = document.getElementById("txtFrom").value;
    var Y = document.getElementById("txtTo").value;
    var start = $('#txtFrom').datepicker('getDate');
    var end = $('#txtTo').datepicker('getDate');
    //if (!start || !end) return;
    //var days = (end - start) / (1000 * 60 * 60 * 24);
    //$.localStorage('days', days);
    $.localStorage('fromdate', start);
    $.localStorage('todate', end);
    // alert($.localStorage('days'))
    window.location.reload();

}
$(document).ready(function () {



    $("#txtFrom").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'dd/mm/yy',
        onClose: function (selectedDate) {
            $("#txtTo").datepicker("option", "minDate", selectedDate);
        }

    });

    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'dd/mm/yy',
        onClose: function (selectedDate) {
            $("#txtFrom").datepicker("option", "maxDate", selectedDate);
        }

    });
});
