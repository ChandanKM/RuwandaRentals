
var resultVM = new SearchResultViewModel();
var data;
if ($.localStorage('days') == "") {
    data = 90;

}
else {
    data = $.localStorage('days')
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

        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Booking/CCAvenue_Report',
            contentType: "application/json; charset=utf-8",

            data: { VendID: '', days: data },
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
                $.localStorage('days',90)

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }


        });

    }

}
//Model
function BookingClassAverage(data) {
    var prop = this;

    prop.ccavenuecharges = (data["ccavenuecharges"]).toFixed(2);
    prop.totalnet = (data["totalnet"]).toFixed(2);
   


}
function BookingClass(data) {
    var prop = this;
    
    prop.VendorID = data["vndr_id"];
    prop.Prop_Id = ko.observable(data["prop_id"]);

    prop.Inv_Num = ko.observable(data["invce_num"]);
    prop.Net_Amnt = ko.observable(data["net_amt"]);
    prop.PayType = ko.observable(data["credit_debit_card"]);
    prop.ccavenuepercent = ko.observable(data["ccavenuepercent"]);
    prop.ccavenuecharges = ko.observable(data["ccavenuecharges"]);

    self.DT = function (data) {
        

        //$.localStorage("Invce_Nums",data.Invce_Num);
        //window.location.href = '/Vendor/ReportTransactions/';


    };

}


function RefineReport() {
    var x = document.getElementById("txtFrom").value;
    var Y = document.getElementById("txtTo").value;
    var start = $('#txtFrom').datepicker('getDate');
    var end = $('#txtTo').datepicker('getDate');
    if (!start || !end) return;
    var days = (end - start) / (1000 * 60 * 60 * 24);
    $.localStorage('days', days);
    // alert($.localStorage('days'))
    window.location.reload();

}
$(document).ready(function () {

    //var minday=0;

    $("#txtFrom").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'dd/mm/yy'

    });

    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'dd/mm/yy'

    });
});
