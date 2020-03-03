var urlPath = window.location.pathname;

//var Inv_From = $.localStorage('fromdate') == "" ? null : $.localStorage('fromdate');
//var Inv_To = $.localStorage('todate') == "" ? null : $.localStorage('todate');
//var Check_in = $.localStorage('Checkin') == "" ? null : $.localStorage('Checkin');
//var Check_out = $.localStorage('Checkout') == "" ? null : $.localStorage('Checkout');

//var Inv_From = $.localStorage('fromdate') == "" ? "" : $.localStorage('fromdate');
//var Inv_To = $.localStorage('todate') == "" ? "" : $.localStorage('todate');
//var Check_in = $.localStorage('Checkin') == "" ? "" : $.localStorage('Checkin');
//var Check_out = $.localStorage('Checkout') == "" ? "" : $.localStorage('Checkout');



function SearchResultViewModel() {
    AppCommonScript.ShowWaitBlock();
    var self = this;
    self.Bookings = ko.observableArray([]),

    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginAuthId",
        dataType: "json",
        async:false,
        success: function (response) {
            
            $.localStorage("AuthId", response.split(',')[0]);
            $.localStorage("VendId", response.split(',')[2]);

        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
    
    var Auth_Id = $.localStorage("AuthId")

    var Ven_Id = 0
    var Prop_Id = 0
    if (Auth_Id == 3) {

        Prop_Id = $.localStorage("VendId")
    }
    if (Auth_Id == 4 || Auth_Id == 5) {

        Prop_Id = $.localStorage("VendId")
        Ven_Id = $.localStorage("VendId")
    }


    self.getBooking = function () {

   
        var Inv_From = document.getElementById("txtFrom").value;
        var Inv_To = document.getElementById("txtTo").value;

        //$.localStorage('fromdate', "");
        //$.localStorage('todate', "");
        //$.localStorage('Checkin', "");
        //$.localStorage('Checkout', "");

        
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: '/api/Booking/BookingList?VendID=' + Ven_Id + '&InvFrom=' + Inv_From + '&InvTo=' + Inv_To,
            contentType: "application/json; charset=utf-8",
            async:false,
            dataType: "json",
            success: function (data) {

                self.Bookings.removeAll();
                //var table = $(".BookingsDT").dataTable();
                //table.fnDestroy();
                //$('.BookingsDT tbody').empty();

                
                for (var i = 0; i < data.Table.length; i++) {
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
              


                //$(".BookingsDT").DataTable({
                //    responsive: true,
                //    'iDisplayLength': 15,
                //});
                //$('#DataTables_Table_0_length').css('display', 'none')
            

            
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });
        AppCommonScript.HideWaitBlock();

    }

}
function RefineReport() {
    resultVM.getBooking();
}

$(document).ready(function () {



    $("#txtFrom").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd MM, yy'

    });

    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd MM, yy'
    });


    //$("#txtCheckin").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'd M, y'

    //});

    //$("#txtCheckOut").trigger('click');
    //$("#txtCheckOut").datepicker({
    //    minDate: '-5M',
    //    maxDate: '+2D',
    //    dateFormat: 'd M, y'

    //});


    //if ($.localStorage('fromdate') != '' && $.localStorage('fromdate') != null) {
    //    var d = new Date($.localStorage('fromdate'));
    //    $('#txtFrom').val(formattedDate(d));
    //}
    //if ($.localStorage('todate') != '' && $.localStorage('todate') != null) {
    //    var d = new Date($.localStorage('todate'));
    //    $('#txtTo').val(formattedDate(d));
    //}
    //if ($.localStorage('Checkin') != '' && $.localStorage('Checkin') != null) {
    //    var d = new Date($.localStorage('Checkin'));
    //    $('#txtCheckin').val(formattedDate(d));
    //}
    //if ($.localStorage('Checkout') != '' && $.localStorage('Checkout') != null) {
    //    var d = new Date($.localStorage('Checkout'));
    //    $('#txtCheckOut').val(formattedDate(d));
    //}
    resultVM = new SearchResultViewModel();
    resultVM.getBooking();
    ko.applyBindings(resultVM, document.getElementById("BookingsDT"));
});
//View Model

function formattedDate(date) {
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}

//Model
function BookingClass(data) {
    var prop = this;

    prop.Invce_Num = data["Invce_Num"];
    var datebook = new Date(data["Invce_date"]).format("dd mmmm yyyy");;

    prop.BookingDate = ko.observable(datebook);
    prop.Destination = data["City_Name"];

    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.Room_Type = ko.observable(data["Room_Name"]);
    var date = new Date(data["Checkin"]).format("dd mmmm yyyy");

    prop.Checkin = ko.observable(date);

    var date1 = new Date(data["Checkout"]).format("dd mmmm yyyy");

    prop.Checkout = ko.observable(date1);

    prop.NumberofNights = data["Days_Count"];



    prop.Name = ko.observable(data["Name"]);
    prop.Mobile = ko.observable(data["Mobile"]);
    prop.Email = ko.observable(data["cons_mailid"]);
    prop.City = ko.observable(data["cons_city"]);

    self.DT = function (data) {


        $.localStorage("Invce_Nums", data.Invce_Num);
        window.location.href = '/Vendor/ReportTransactions/';


    };
}




function expToExcel() {

    //var data = $("#DataTables_Table_0").html();
    //data = escape(data);

    
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('DataTables_Table_0'); // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    $('body').prepend("<form method='post' action='/Vendor/Export' style='top:-3333333333px;' id='tempForm'><input type='hidden' name='data' value='" + escape(tab_text) + "' ></form>");
    $('#tempForm').submit();
    $("tempForm").remove();
    return false;

}


function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}


