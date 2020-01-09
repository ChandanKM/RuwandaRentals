var urlPath = window.location.pathname;



//View Model


//var Inv_From = $.localStorage('fromdate') == "" ? "" : $.localStorage('fromdate');
//var Inv_To = $.localStorage('todate') == "" ? "" : $.localStorage('todate');
//var Check_in = $.localStorage('Checkin') == "" ? "" : $.localStorage('Checkin');
//var Check_out = $.localStorage('Checkout') == "" ? "" : $.localStorage('Checkout');

function BookingListVM() {
    debugger;
    AppCommonScript.ShowWaitBlock();
    var Ven_Id = 0;
    var self = this;
    self.Bookings = ko.observableArray([]),

    self.getBooking = function () {
        
        var start = document.getElementById("txtFrom").value;
        var end = document.getElementById("txtTo").value;
        //if (start != "" || end != "")
        //{
        //var Checkin = document.getElementById("txtCheckin").value ;
        //var Checkout = document.getElementById("txtCheckOut").value ;
        //$.localStorage('fromdate', "");
        //$.localStorage('todate', "");
        //$.localStorage('Checkin', "");
        //$.localStorage('Checkout', "");
        //var form = $('#txtFrom').datepicker('getDate');
        //var to = $('#txtTo').datepicker('getDate');
        $.ajax({
            type: "POST",
            url: '/api/Booking/BookingList?VendID=' + Ven_Id + '&InvFrom=' + start + '&InvTo=' + end,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                self.Bookings.removeAll();
                var table = $(".BookingsDT").dataTable();
                table.fnDestroy();
                $('.BookingsDT tbody').empty();

                for (var i = 0; i < data.Table.length; i++) {
                    debugger;
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
               
                $(".BookingsDT").DataTable({
                    responsive: true,
                    'iDisplayLength': 15,
                });
                $('#DataTables_Table_0_length').css('display', 'none')
              

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });
        AppCommonScript.HideWaitBlock();
        //}
        //else
        //{
        //    return;
        //}        
    }
}
function getbookingpost() {
    debugger;
    var self = this;
    self.Bookings = ko.observableArray([]);
    var Ven_Id = 0;
    var start = document.getElementById("txtFrom").value;
    var end = document.getElementById("txtTo").value;
    
    var datestart = Date.parse(start);
    var dateend = Date.parse(end);
    if (isNaN(datestart)) {
        alert('Enter valid datestart');
        return;
    }
    else if (isNaN(dateend)) {
        alert('Enter valid dateend');
        return;
    }
    if (start != "" || end != "") {
        $.ajax({
            type: "POST",
            url: '/api/Booking/BookingList?VendID=' + Ven_Id + '&InvFrom=' + start + '&InvTo=' + end,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                self.Bookings.removeAll();
                var table = $(".BookingsDT").dataTable();
                table.fnDestroy();
                $('.BookingsDT tbody').empty();

                for (var i = 0; i < data.Table.length; i++) {
                    debugger;
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }

                $(".BookingsDT").DataTable({
                    responsive: true,
                    'iDisplayLength': 15,
                });
                $('#DataTables_Table_0_length').css('display', 'none')
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });
        AppCommonScript.HideWaitBlock();
    }
    else {
        alert('Please enter Report From or Report To Date');
        return;
    }
}

function BookingClass(data) {
    var prop = this;

    prop.Invce_Num = data["Invce_Num"];

    var datebook = new Date(data["Invce_date"]).format("dd mmmm yyyy");

    prop.BookingDate = ko.observable(datebook);
    prop.Destination = data["City_Name"];

    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.Room_Type = ko.observable(data["Room_Name"]);
    var date = new Date(data["Checkin"]).format("dd mmmm yyyy");;

    prop.Checkin = ko.observable(date);

    var date1 = new Date(data["Checkout"]).format("dd mmmm yyyy");;

    prop.Checkout = ko.observable(date1);

    prop.NumberofNights = data["Days_Count"];



    prop.Name = ko.observable(data["Name"]);
    prop.Mobile = ko.observable(data["Mobile"]);
    prop.Email = ko.observable(data["cons_mailid"]);
    prop.City = ko.observable(data["cons_city"]);

    self.DT = function (data) {


        $.localStorage("Invce_Nums", data.Invce_Num);
        window.location.href = '/SuperAdmin/ReportTransactions/';


    };
}

function test()
{
    AllBooking.getBooking();
}
//function RefineReport() {
//    ;
//    var x = document.getElementById("txtFrom").value;
//    var Y = document.getElementById("txtTo").value;
//    var start = $('#txtFrom').datepicker('getDate').toString();
//    var end = $('#txtTo').datepicker('getDate').toString();
//    var Checkin = $('#txtCheckin').datepicker('getDate');
//    var Checkout = $('#txtCheckOut').datepicker('getDate');
//    if ((start == null && end == null) && (Checkin == null && Checkout == null))
//    {
//        return;
//    }
//    //if (!start || !end) return;
//    //var days = (end - start) / (1000 * 60 * 60 * 24);
//    //$.localStorage('days', days);
//    $.localStorage('fromdate', start);
//    $.localStorage('todate', end);
//    $.localStorage('Checkin', Checkin);
//    $.localStorage('Checkout', Checkout);


//    window.location.reload();
//}

$(document).ready(function () {

    $("#txtFrom").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd MM, yy',
        onClose: function (selectedDate) {
            $("#txtTo").datepicker("option", "minDate", selectedDate);
        }

    });

    $("#txtTo").trigger('click');
    $("#txtTo").datepicker({
        minDate: '-5M',
        maxDate: '+2D',
        dateFormat: 'd MM, yy',
        onClose: function (selectedDate) {
            $("#txtFrom").datepicker("option", "maxDate", selectedDate);
        }
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

    AllBooking = new BookingListVM();

    AllBooking.getBooking();

    ko.applyBindings(AllBooking, document.getElementById("BookingsDT"));

});