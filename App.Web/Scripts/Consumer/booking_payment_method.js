

var Consumer_Id = 0;

var StarCount = 0;
var bookpaymentVM = new BookingPaymentViewModel();
bookpaymentVM.GetBookingOrderDetails();
bookpaymentVM.GetLoggedInUserId();

function OrderViewModel() {
    var self = this;
    self.Prop_Id = ko.observable();
    self.RoomId = ko.observable();
    self.Room_Checkin = ko.observable();
    self.Room_Checkout = ko.observable();
    self.No_Of_Rooms = ko.observable();
}

function BookingPaymentViewModel() {
    var self = this;
    self.Vndr_ID = ko.observable();
    self.PropId = ko.observable();
    self.Prop_Name = ko.observable();
    self.Prop_Image = ko.observable('/img/prop_image/hiddengem.jpg');
    self.Prop_Address = ko.observable();
    self.prop_room_rate = ko.observable();

    self.RoomID = ko.observable();
    self.RoomName = ko.observable();
    self.Room_Overview = ko.observable();
    var inDate = ConverToValidFormate($.localStorage("CheckInDate"));
    var checkIn = new Date(inDate).toDateString();
    checkIn = checkIn.split(' 2015');
    checkIn = checkIn[0];
    var outDate = ConverToValidFormate($.localStorage("CheckOutDate"));
    var checkOut = new Date(outDate).toDateString();
    checkOut = checkOut.split(' 2015');
    checkOut = checkOut[0];
    self.Room_Checkin = ko.observable(checkIn);
    self.Room_Checkout = ko.observable(checkOut);
    var orderVM = new OrderViewModel();
    orderVM.Prop_Id = $.localStorage("Prop_Id");
    orderVM.Room_Id = $.localStorage("Room_Id");
    orderVM.Room_Checkin = ConverToValidFormate($.localStorage("CheckInDate"));
    orderVM.Room_Checkout = ConverToValidFormate($.localStorage("CheckOutDate"));

    orderVM.No_Of_Rooms = $.localStorage("NoOfRoom"); 
    self.RoomDTList = ko.observableArray();
    self.RoomDTListother = ko.observableArray();
    self.RoomDTListImage = ko.observableArray();
    self.Trip_Rating = ko.observable();
    self.Trip_Review = ko.observable();
    self.Trip_Review_Url = ko.observable();
    self.StarRatingList = ko.observableArray();
   
    $.ajax({
        type: "Post",
        url: '/api/Consumer/BookingDetails',
        data: ko.toJSON(orderVM),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {            

            $.localStorage("Vndr_ID", data.Table[0].Vndr_Id);
            $.localStorage("camo_room_rate", data.Table[0].camflg_Amnt);

            dayCount = data.Table2[0].Column1;
        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);
        }
    })
    var dayCount = GetNightCount(inDate, outDate);//$.localStorage("DayCount");
    self.Day_Count = ko.observable(dayCount);
    self.RoomPrice = ko.observable();
    
    self.camo_room_rate = ko.observable();
    self.ServiceTax = ko.observable();
    self.LuxuryTax = ko.observable();
    //  self.VAT = ko.observable();
    //  self.Discount = ko.observable();
    self.Room_Count = ko.observable($.localStorage("NoOfRoom"));
    self.RoomFacilityList = ko.observableArray();
    
    self.total_room_rate = ko.computed(function () {        
        return  (self.camo_room_rate() * parseInt(self.Room_Count()) * self.Day_Count()).toFixed(2);
    });

    self.tax_amnt = ko.computed(function ()
    {
        return ((self.ServiceTax() + self.LuxuryTax()) * parseInt(self.Room_Count()) * self.Day_Count()).toFixed(2);
    });
   
    self.net_amt = ko.computed(function () {
        return parseFloat(self.total_room_rate()) + parseFloat(self.tax_amnt());
    });

    self.Invce_note = ko.observable();
    self.redmpt_points = ko.observable('0');
    self.redmpt_value = ko.observable('0');
    self.Promo_Type = ko.observable('Value');
    self.Prop_Value = ko.observable('7');
    self.ipaddress = ko.observable();
    $.getJSON("http://jsonip.com?callback=?", function (data) {
        self.ipaddress(data.ip);
    });

    self.Cons_Id = ko.observable();
    self.Cons_First_Name = ko.observable();
    self.Cons_Last_Name = ko.observable();
    self.Cons_mailid = ko.observable();
    self.Cons_Mobile = ko.observable();

    self.Occupant_firstname = ko.observable();
    self.Occupant_lastname = ko.observable();
    self.Occupant_firstname1 = ko.observable();
    self.Occupant_lastname1 = ko.observable();
    self.Occupant_firstname2 = ko.observable();
    self.Occupant_lastname2 = ko.observable();
    self.Occupant_firstname3 = ko.observable();
    self.Occupant_lastname3 = ko.observable();
    self.Occupant_firstname4 = ko.observable();
    self.Occupant_lastname4 = ko.observable();
    self.Occupant_firstname5 = ko.observable();
    self.Occupant_lastname5 = ko.observable();
    if ($.localStorage("NoOfRoom") == 1) {
        self.GuestName = ko.computed(function () {
            return $('#fn1').val() +" "+ $('#ln1').val();
        });
    }

    else if ($.localStorage("NoOfRoom") == 2) {
        $('#Guest2').css('display','block')
        self.GuestName = ko.computed(function () {
            return $('#fn1').val() +" "+ $('#ln1').val() + ',' + $('#fn2').val() +" "+ $('#ln2').val() ;
        });
    }

    else if ($.localStorage("NoOfRoom") == 3) {
        $('#Guest2').css('display', 'block')
        $('#Guest3').css('display', 'block')
        self.GuestName = ko.computed(function () {
            return $('#fn1').val() +" "+ $('#ln1').val() + ',' + $('#fn2').val() +" "+ $('#ln2').val() + ',' + $('#fn3').val() +" "+ $('#ln3').val();
        });
    }
    else if ($.localStorage("NoOfRoom") == 4) {
        $('#Guest2').css('display', 'block')
        $('#Guest3').css('display', 'block')
        $('#Guest4').css('display', 'block')
       
        self.GuestName = ko.computed(function () {
            return $('#fn1').val() +" "+ $('#ln1').val() + ',' + $('#fn2').val() +" "+ $('#ln2').val() + ',' + $('#fn3').val() +" "+ $('#ln3').val() + ',' + $('#fn4').val() +" "+ $('#ln4').val();
        });
    }
    else if ($.localStorage("NoOfRoom") == 5) {
        $('#Guest2').css('display', 'block')
        $('#Guest3').css('display', 'block')
        $('#Guest4').css('display', 'block')
        $('#Guest5').css('display', 'block')
        self.GuestName = ko.computed(function () {
            return $('#fn1').val() +" "+ $('#ln1').val() + ',' + $('#fn2').val() +" "+ $('#ln2').val() + ',' + $('#fn3').val() +" "+ $('#ln3').val() + ',' + $('#fn4').val() +" "+ $('#ln4').val() + ',' + $('#fn5').val() +" "+ $('#ln5').val();
        });
    }

    self.BookingOrderDetails = ko.observableArray();

    self.GetBookingOrderDetails = function () {
        
       // AppCommonScript.ShowWaitBlock();
        var orderVM = new OrderViewModel();
        orderVM.Prop_Id = $.localStorage("Prop_Id");
        orderVM.Room_Id = $.localStorage("Room_Id");
        orderVM.Room_Checkin = ConverToValidFormate($.localStorage("CheckInDate"));
        orderVM.Room_Checkout = ConverToValidFormate($.localStorage("CheckOutDate"));
        orderVM.No_Of_Rooms = $.localStorage("NoOfRoom");
        $.ajax({
            type: "Post",
            url: '/api/Consumer/BookingDetails',
            data: ko.toJSON(orderVM),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {


                if (data.Table.length > 0)
                    
          var      AjaxUrl = data.Table[0].tripid;
                $.ajax({

                    url: AjaxUrl,
                    type: 'GET',
                    dataType: 'json',
                    async: false,
                    success: function (trip) {
                        
                        
                        // data.rating_image_url
                        rating = trip.rating_image_url;
                        Numberreviews = trip.num_reviews + ' reviews';;
                        ReviewUrl = trip.reviews;
                        //   self.HotelsList.push(new HotelListModel(data.Table[i], data.Table2[i].Roomtype, trip.rating_image_url))
                       
                        // WriteResponse(data);
                    },
                    error: function (x, y, z) {
                        rating = '';
                        Numberreviews = '';
                        ReviewUrl = '';

                        //  alert(x + '\n' + y + '\n' + z);
                    }
                });
               
                OnSuccessGetBookingOrderDetails(data.Table[0], data.Table1,rating,Numberreviews,ReviewUrl);
                self.Day_Count(data.Table2[0].Column1);
            },
            error: function (err) {
                // alert(err.status + " : " + err.statusText);
            }
        }).done(function () {
            ko.applyBindings(bookpaymentVM, document.getElementById("divBookingdetails"));
           // AppCommonScript.HideWaitBlock();
        });
    }

    self.GetLoggedInUserId = function () {
    //    AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/Consumer/GetLoggedInUserId",
           // dataType: "json",
            success: function (response) {
                Consumer_Id = response;

                OnSuccessGetUserId(response);
            },
            error: function (err) {
            }
        });
    }
    self.GetRoomDetailsId = function (Prop_Id, Room_Id) {
       // AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetRoomDetailsByID',
            data: { Prop_Id: $.localStorage("Prop_Id"), Room_Id: $.localStorage("Room_Id") },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                
                self.RoomDTList.removeAll();
                self.RoomDTListother.removeAll();
               
                for (var i = 0; i < data.Table.length; i++) {


                    self.RoomDTList.push(new RoomDTmodelFacil(data.Table[i], data.Table3));
                }
                for (var i = 0; i < data.Table3.length; i++) {
                    self.RoomDTList.push(new RoomDTmodelPropFacil(data.Table3[i]));
                }
                if (data.Table1.length <= 0)
                    $('#inc').hide();
                for (var i = 0; i < data.Table1.length; i++) {
                    self.RoomDTListother.push(new RoomDTmodelothers(data.Table1[i]));
                }

              //  AppCommonScript.HideWaitBlock();
                $('#roomdt').modal('show');
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
    self.GetUserProfileDetails = function (Id) {
        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetProfileDetails',
            data: { Cons_Id: Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.Table.length > 0) {

                    self.Cons_Id(data.Table[0].Cons_Id);
                    $.localStorage("Cons_Id", data.Table[0].Cons_Id);
                    self.Cons_First_Name(data.Table[0].Cons_First_Name || '');
                    self.Cons_Last_Name(data.Table[0].Cons_Last_Name || '');
                    self.Cons_mailid(data.Table[0].Cons_mailid || '');
                    self.Cons_Mobile(data.Table[0].Cons_Mobile || '');

                    //self.Occupant_firstname(data.Table[0].Cons_First_Name || '');
                    //self.Occupant_lastname(data.Table[0].Cons_Last_Name || '');
                    $('#fn1').val(data.Table[0].Cons_First_Name)
                    + $('#ln1').val(data.Table[0].Cons_Last_Name)
                }

            },
            error: function (err) {
                // alert(err.status + " : " + err.statusText);
            }
        }).done(function () {
            ko.applyBindings(bookpaymentVM, document.getElementById("formUserDetails"));
           // AppCommonScript.HideWaitBlock();
        });
    }

    self.ContinueToPayment = function (data) {
        if ($('#fn1').val() == '') {
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Please Enter FirstName" }, ErrorType: "error" };
            Failed(result)
            return false;
        }
       
        data.Vndr_ID = $.localStorage("Vndr_ID");
        data.camo_room_rate = $.localStorage("camo_room_rate");
        if ($.localStorage("NoOfRoom") == 1) {
            self.GuestName = ko.computed(function () {
                return $('#fn1').val() +" "+ $('#ln1').val();
            });
        }

        else if ($.localStorage("NoOfRoom") == 2) {
         
            self.GuestName = ko.computed(function () {
                return $('#fn1').val() + " " + $('#ln1').val() + ',' + $('#fn2').val() + " " + $('#ln2').val();
            });
        }

        else if ($.localStorage("NoOfRoom") == 3) {

            self.GuestName = ko.computed(function () {
                return $('#fn1').val() + " " + $('#ln1').val() + ',' + $('#fn2').val() + " " + $('#ln2').val() + ',' + $('#fn3').val() + " " + $('#ln3').val();
            });
        }
        else if ($.localStorage("NoOfRoom") == 4) {


            self.GuestName = ko.computed(function () {
                return $('#fn1').val() + " " + $('#ln1').val() + ',' + $('#fn2').val() + " " + $('#ln2').val() + ',' + $('#fn3').val() + " " + $('#ln3').val() + ',' + $('#fn4').val() + " " + $('#ln4').val();
            });
        }
        else if ($.localStorage("NoOfRoom") == 5) {

            self.GuestName = ko.computed(function () {
                return $('#fn1').val() + " " + $('#ln1').val() + ',' + $('#fn2').val() + " " + $('#ln2').val() + ',' + $('#fn3').val() + " " + $('#ln3').val() + ',' + $('#fn4').val() + " " + $('#ln4').val() + ',' + $('#fn5').val() + " " + $('#ln5').val();
            });
        }
        
        if ($.localStorage("NoOfRoom") != 1) {
            var str = data.GuestName();
            var match = str.match(/,/g);
            if (str == "") {
                // data.GuestName = '';
            }
            else if (match.length == 1) {
                var partsOfguestname = data.GuestName().split(',');
                if (partsOfguestname[0] == '' || partsOfguestname[1] == '') {
                    data.GuestName = '';
                }
                else {
                    // data.GuestName = partsOfguestname;
                }
            }
            else if (match.length == 2) {
                var partsOfguestname = data.GuestName().split(',');
                if (partsOfguestname[0] == '' || partsOfguestname[1] == '' || partsOfguestname[2] == '') {
                    data.GuestName = '';
                }
                else {
                    // data.GuestName = partsOfguestname;
                }
            }
            else if (match.length == 3) {
                var partsOfguestname = data.GuestName().split(',');
                if (partsOfguestname[0] == '' || partsOfguestname[1] == '' || partsOfguestname[2] == '' || partsOfguestname[3] == '') {
                    data.GuestName = '';
                }
                else {
                    // data.GuestName = partsOfguestname;
                }
            }
            else if (match.length == 4) {
                var partsOfguestname = data.GuestName().split(',');
                if (partsOfguestname[0] == '' || partsOfguestname[1] == '' || partsOfguestname[2] == '' || partsOfguestname[3] == '' || partsOfguestname[4] == '') {
                    data.GuestName = '';
                }
                else {
                    // data.GuestName = partsOfguestname;
                }
            }
        }
      //  AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Post",
            url: '/api/Consumer/WebPreBooking',
            data: ko.toJSON(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                
                if (response.Table.length > 0) {

                    
                    $.localStorage("Invce_Num", response.Table[0].Invce_Num);
                    if (response.Table[0].Invce_Num != '0' && response.Table[0].net_amt != 0)
                        window.location.href = '/Request?Invce_Num=' + response.Table[0].Invce_Num + '&Amount=' + response.Table[0].net_amt;
                    else {
                        var responseText = "Invoice Number or Amount Not Geting Properly";
                        //var result = { Status: true, ReturnMessage: { ReturnMessage: "Invoice Number or Amount Not Geting Properly" }, ErrorType: "Success" };
                        //  Failed(result);
                      //  alert(responseText)
                    }
                }
                else {
                    var responseText = "Not Geting Proper Response";
                    //Failed(JSON.parse(responseText));
                }

                //AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
              //  alert(err.responseText)
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
          //  window.location.reload();

        });
    }
    function RatingModel(data) {
        var self = this;
        self.MyStarClass = ko.observable('s0');
        if (data > 0) {
            self.MyStarClass('s1');
        }
    }
    function OnSuccessGetBookingOrderDetails(data, dataFaci,rating,Numberreviews,ReviewUrl) {
       
        
        var Stax = data.servicetax;
        Stax = Stax.toFixed(2);
        self.Vndr_ID(data.vndr_id);
        self.PropId(data.Prop_Id);
        self.Trip_Rating = rating;
        self.Trip_Review = Numberreviews;
        self.Trip_Review_Url = ko.observable(ReviewUrl);
        self.Prop_Image(data.Image_dir || '/img/prop_image/hiddengem.jpg');
        if (data.Image_dir == "img/prop_image/hiddengem.jpg") {
                            $('#ifhidden').hide();
                            Numberreviews = '';

                        }
        self.Prop_Name(data.Prop_Name);
        if (data.Image_dir == 'img/prop_image/hiddengem.jpg') {
            self.Prop_Address(data.City + ', ' + data.state);
            self.Trip_Review = "";
        }
        else {
            self.Prop_Address(data.Prop_Addr1 + ', ' + data.Location + ', ' + data.City);
        }
        StarCount = parseInt(data.prop_star_rating);
        for (var i = 0; i < 5; i++) {
            self.StarRatingList.push(new RatingModel(StarCount--));
        }
       
        self.Room_Checkin_Time = data.Room_checkin;
        self.Room_Checkout_Time = data.Room_checkout;
        self.Room_Checkin(self.Room_Checkin())
        self.Room_Checkout(self.Room_Checkout())
        self.RoomID(data.Room_Id);
        self.RoomName(data.Room_Name);
        self.Room_Overview(data.Room_Overview);

        self.RoomPrice(data.camflg_Amnt);
        self.prop_room_rate(data.Vndr_Amnt);
        
        self.camo_room_rate(data.camflg_Amnt+'.00');
        self.ServiceTax(Stax);//.toFixed(2)
        self.LuxuryTax(data.luxurytax);//(data.luxurytax).toFixed(2));

        for (var i = 0; i < dataFaci.length; i++) {
            self.RoomFacilityList.push(new FacilityModel(dataFaci[i]));
        }

    }

    function OnSuccessGetUserId(Id) {
        
       var a= self.GetUserProfileDetails(Id);
    }
}

function RoomDTmodelFacil(data) {
    
    var self = this;
    data = data || {};
    self.Facility_Id = ko.observable(data.Facility_Id);
    self.Facility_Name = ko.observable(data.Facility_Name);
    self.Facility_Image_dir = ko.observable(data.Facility_Image_dir);

}
function RoomDTmodelPropFacil(data) {
    
    var self = this;
    data = data || {};
    self.Facility_Id = ko.observable(data.Facility_Id);
    self.Facility_Name = ko.observable(data.Facility_Name);
    self.Facility_Image_dir = ko.observable(data.Facility_Image_dir);

}
function RoomDTmodelothers(data) {
    
    var self = this;
    data = data || {};

    self.inclusion = ko.observable(data.inclusion);
    self.others = ko.observable(data.others);
}
function FacilityModel(data) {
    var self = this;
    data = data || {};
    self.FacilityName = ko.observable(data.Facility_Name);
    self.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
}


function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}


function HotelModel() {
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.PropId);
    self.Prop_Image = ko.observable(data.Image_dir);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_Address = ko.observable(data.Prop_Addr1 + ', ' + data.City);

    self.RoomPrice = ko.observable(data.ActualRate);
}


function HotelRoomDetailsModel(data, dataFaci) {
    
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_Image = ko.observable(data.Image_dir || '/img/prop_image/hiddengem.jpg');
    self.Prop_Address = ko.observable(data.Prop_Addr1 + ', ' + data.City);

    self.Room_Id = ko.observable(data.Room_Id);
    self.RoomName = ko.observable(data.Room_Name);
    self.Room_Overview = ko.observable(data.Room_Overview);
    self.Room_Checkin = ko.observable();
    self.Room_Checkout = ko.observable();
    self.Room_Checkout_Time = ko.observable();
    self.Room_Checkin_Time = ko.observable();
    self.RoomPrice = ko.observable(data.camflg_Amnt);
    
    self.camo_room_rate = ko.observable(data.camflg_Amnt);
    self.ServiceTax = ko.observable(data.camflg_Amnt);
    //  self.VAT = ko.observable(data.camflg_Amnt);
    //  self.Discount = ko.observable(data.camflg_Amnt);

    self.RoomFacilityList = ko.observableArray();
    for (var i = 0; i < dataFaci.length; i++) {
        self.RoomFacilityList.push(new FacilityModel(dataFaci[i]));
    }

    self.GrandTotal = ko.computed(function () {
        return self.camo_room_rate() + self.ServiceTax();
    });
    //self.ContinueToPayment = function () {

    //}
}


function ConverToValidFormate(formateDate) {
    var date = formateDate.split('/');
    shortDate = date[2] + "/" + date[1] + "/" + date[0];

    return shortDate;
}
function GetNightCount(start, end) {
    var dateIn = start.split('/');
    var dateOut = end.split('/');
    var count = dateOut[2] - dateIn[2];
    return count;
}
$(document).ready(function () {

    $('#TripReview').click(function () {
        
        $('#TripReviewModal').modal('show');
    })

})