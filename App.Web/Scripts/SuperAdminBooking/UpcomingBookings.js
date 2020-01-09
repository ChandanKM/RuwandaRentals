var urlPath = window.location.pathname;



//View Model
function BookingListVM() {
    var VendID = '';
    var self = this;
    self.Bookings = ko.observableArray([]),
   
    self.getBooking = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Booking/UpcomingBookingList',
            contentType: "application/json; charset=utf-8",
            data: { VendID:0},
            dataType: "json",
            success: function (data) {
                
                
                for (var i = 0; i < data.Table.length; i++) {
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
                ko.applyBindings(AllBooking, document.getElementById("BookingsDT"));

                $(".BookingsDT").DataTable({
                    responsive: true,
                    'iDisplayLength':15,
                 

                });
                $('#DataTables_Table_0_length').css('display','none')
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });
        AppCommonScript.HideWaitBlock();
    }
   
    
}


//Model
function BookingClass(data) {
    var prop = this;
    
    prop.Invce_Num = data["Invce_Num"];
    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.Room_Type = ko.observable(data["Room_Name"]);
    var date = new Date(data["Checkin"]).toDateString();

    prop.Checkin = ko.observable(date);
    prop.Name = ko.observable(data["Cons_First_Name"]);
    prop.Mobile = ko.observable(data["Cons_Mobile"]);
    self.DT = function (data) {
 
        
        $.localStorage("Invce_Nums",data.Invce_Num);
        window.location.href = "../SuperAdmin/ReportTransactions";
        return false;
  
    };
}

$(document).ready(function () {

    AllBooking = new BookingListVM();

    AllBooking.getBooking();

});