var urlPath = window.location.pathname;



//View Model
function BookingListVM() {
    var self = this;
    self.Bookings = ko.observableArray([]),
    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginAuthId",
        dataType: "json",
        success: function (response) {
            $.localStorage("AuthId", response)

        },
        error: function (jqxhr) {

            Failed(JSON.parse(jqxhr.responseText));
        }
    });
  
    var Auth_Id = $.localStorage("AuthId")
 
    var Ven_Id = 0
    var Prop_Id = 0
    if (Auth_Id == 3)
    {
       
        Ven_Id= $.localStorage("VendId")
    }
    if (Auth_Id == 4 || Auth_Id == 5) {
       
        Prop_Id=  $.localStorage("VendId")
    }
    self.getBooking = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Booking/UpcomingBookingList',
            contentType: "application/json; charset=utf-8",
            data: { VendID: Ven_Id },
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
        window.location.href = '/Booking/Transactions/';


    };
}

$(document).ready(function () {

    AllBooking = new BookingListVM();

    AllBooking.getBooking();

});