
var resultVM = new SearchResultViewModel();
var data;
if ($.localStorage('days') == "") {
    data = 90;

}
else {
    data = $.localStorage('days')
}
resultVM.GetRes(data);
ko.applyBindings(resultVM, document.getElementById("divReportTable"));



$(".BookingsDT").DataTable({
    responsive: true,
    //'iDisplayLength': 15,


});
$('#DataTables_Table_0_length').css('display', 'none')


function SearchResultViewModel() {
    var self = this;
    self.TaxList = ko.observableArray();
    self.AverageList = ko.observableArray();
    self.GetRes = function (data)
    {
     

        if (data == undefined)
            data = '30';

        $.ajax({
            type: "GET",
            url: '/api/Booking/Tax_Report',
            //contentType: "application/json; charset=utf-8",
            data: { VendID: '', TaxType: '', days: data },
          //  datatype: "json",
            async: false,
            success: function (data) {

                
                self.TaxList.removeAll();
                for (var i = 0; i < data.Table.length; i++) {

                    self.TaxList.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table1.length; i++) {

                    self.AverageList.push(new BookingClassAverage(data.Table1[i])); //Put the response in ObservableArray
                }
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
    

    prop.V_param_desc = (data["vparam_descr"]);
    //prop.VparamType = (data["vparam_type"]);
    prop.Inv_Num = (data["invce_num"]);
    prop.Tax_Amnt = (data["tax_amnt"]);
    prop.Room_Rate = (data["prop_room_rate"]);
    prop.Camo_Rate = (data["camo_room_rate"]);


}
function BookingClassAverage(data) {
    var prop = this;

    prop.roomrate = (data["roomrate"]).toFixed(2);
    prop.sellingrate = (data["sellingrate"]).toFixed(2);
    prop.tax = (data["tax"]).toFixed(2);


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
