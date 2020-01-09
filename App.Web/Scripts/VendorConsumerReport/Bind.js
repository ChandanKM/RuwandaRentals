
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
    self.GetRes = function (data) {

        AppCommonScript.ShowWaitBlock();
      
        $.ajax({
            type: "GET",
            url: '/Vendor/GetLoginVendorId',
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (datavendor) {
              
                if (datavendor !== 'ss')
                {
                    $.ajax({
                        type: "GET",
                        url: '/api/Booking/ConsumerReport',
                        contentType: "application/json; charset=utf-8",

                        data: { VendID: datavendor, cons_id: '', cons_name: '', cons_mailid: '', cons_mobile: '', days: data },
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            
                            self.TaxList.removeAll();
                            for (var i = 0; i < data.Table.length; i++) {

                                self.TaxList.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                            }
                            AppCommonScript.HideWaitBlock();
                            $.localStorage('days', 90);

                        },
                        error: function (err) {
                            //  alert(err.status + " : " + err.statusText);

                        }


                    });
                }

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
    
    prop.Cons_Id = data["cons_id"];
    prop.Cons_Name = ko.observable(data["cons_first_name"] + data["cons_last_name"]);
    prop.Cons_Email = ko.observable(data["cons_mailid"]);
    
    prop.Mobile = ko.observable(data["cons_mobile"]);
    prop.Orders = ko.observable(data["orders"]);
  
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



    $("#txtFrom").datepicker({
        //minDate: 1,
        //maxDate: '+3M',
        dateFormat: 'dd/mm/yy'

    });
    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        //minDate: 1,
        //maxDate: '+3M',
        dateFormat: 'dd/mm/yy'

    });
});
