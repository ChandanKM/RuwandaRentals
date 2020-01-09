
$(document).ready(function () {

    $.localStorage('fromdate', "");
    $.localStorage('todate', "");
    $.localStorage('Checkin', "");
    $.localStorage('Checkout', "");

    //$("#txtFrom").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'dd/mm/yy'

    //});

    //$("#txtTo").trigger('click');
    //$("#txtTo").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'dd/mm/yy'
    //});




    $("#txtFrom").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd M, y'


    });

    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd M, y'
    });

    //$("#txtCheckin").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'dd/mm/yy'

    //});

    //$("#txtCheckOut").trigger('click');
    //$("#txtCheckOut").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'dd/mm/yy'

    //});
    

    var Grid = new BindDataVM();
    Grid.GetAllCorporateCompanies();
    Grid.BindData();
    ko.applyBindings(Grid, document.getElementById("BookingsTable"));
    
});

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function BindDataVM() {




    self = this;
    self.Bookings = ko.observableArray([]);
    self.CorporateUserArray = ko.observableArray();
    self.CorporateCompanyArray = ko.observableArray([{ Id: '', CompanyName: 'Select Corporate' }]);
    self.selectedCompany = ko.observable();
    self.selectedCompany("");
    var data = 90;

    self.BindData = function () {


        var start = document.getElementById("txtFrom").value;
        var end = document.getElementById("txtTo").value;

      
        //var Checkin = $('#txtCheckin').datepicker('getDate');
        //var Checkout = $('#txtCheckOut').datepicker('getDate');
       
        $.localStorage('fromdate', start);
        $.localStorage('todate', end);
        //$.localStorage('Checkin', Checkin);
        //$.localStorage('Checkout', Checkout);


        var Ven_Id = 'temp@' + self.selectedCompany()+ '.com';
        var bookingStatus = $('#BookingStatus').val();
        var Inv_From = $.localStorage("fromdate");
        var Inv_To = $.localStorage("todate");
        //var Check_in = $.localStorage("Checkin");
        //var Check_out = $.localStorage("Checkout");
        //AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: '/api/Booking/CorpBookingList?Cons_Id=' + Ven_Id + '&bookingStatus=' + bookingStatus + '&InvFrom=' + Inv_From + '&InvTo=' + Inv_To,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

         
                
                self.Bookings.removeAll();
               
                var table = $(".Booking-Table").dataTable();
                table.fnDestroy();
                $('.Booking-Table tbody').empty();

             
                for (var i = 0; i < data.Table.length; i++) {
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
                $('.Booking-Table').dataTable({ 'iDisplayLength': 10 });
                $('.dataTables_length').css('display', 'none')
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });

    }

    self.GetAllCorporateCompanies = function () {

        $.getJSON("/api/Corporate/GetAllCorporateCompanies", function (CorpCompany) {

            ko.utils.arrayMap(CorpCompany, function (Company) {

                self.CorporateCompanyArray.push({ Id: Company, CompanyName: Company });

            });
        });

    }



}


function BookingClass(data) {

    var prop = this;


    prop.BookingStatus = data["BookingStatus"];

    prop.Invce_Num = data["Invce_Num"];
    prop.Destination = data["City_Name"];

    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.Room_Type = ko.observable(data["Room_Name"]);



    prop.Invce_date =new Date(data["Invce_date"]).format("dd mmmm yyyy");

    var date = new Date(data["Checkin"]).format("dd mmmm yyyy");

    prop.Checkin = ko.observable(date);

    var date1 = new Date(data["Checkout"]).format("dd mmmm yyyy");

    prop.Checkout = ko.observable(date1);

    prop.NumberofNights = data["Room_Count"];



    prop.Name = ko.observable(data["Name"]);
    prop.Mobile = ko.observable(data["Mobile"]);
    prop.Email = ko.observable(data["cons_mailid"]);
    prop.City = ko.observable(data["cons_city"]);
    prop.Rate = ko.observable(data["camo_room_rate"]);
    prop.Tax = ko.observable(data["tax_amnt"]);
    prop.Amount = ko.observable(data["net_amt"]);

    self.DT = function (data) {
        $.localStorage("Invce_Nums", data.Invce_Num);
        window.location.href = '/Corporate/ReportTransactions/';

    };

   
}


function marginExcel() {
    
    //var data = $("#DataTables_Table_0").html();
    //data = escape(data);


    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('example'); // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    $('body').prepend("<form method='post' action='/Corporate/marginExport' style='top:-3333333333px;' id='tempForm'><input type='hidden' name='data' value='" + escape(tab_text) + "' ></form>");
    $('#tempForm').submit();
    $("tempForm").remove();
    return false;

}