$(document).ready(function () {



    $.localStorage('fromdate', "");
    $.localStorage('todate', "");
    //$.localStorage('Checkin', "");
    //$.localStorage('Checkout', "");


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

    var Id = $.localStorage("Consumer_Id");
    $.ajax({
        type: "GET",
        url: '/api/Consumer/CheckCorporateUser',
        data: { Cons_Id: Id },
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.Table.length > 0) {
                $('#liMybookingCorporate').show();
                $('#liReportCorporate').show();
                AllComUsers = new ComUsersListVM();
                AllComUsers.getAllComUsers();
                var a = new BindDataVM();
                a.BindData();
                ko.applyBindings(a, document.getElementById("BookingsDT"));
            }
            else {
                $('#liMybookingCorporate').hide();
                $('#liReportCorporate').hide();
            }
        },
        error: function (err) {
            // alert(err.status + " : " + err.statusText);
        }
    })

});

$("#txtLocation").autocomplete({

    //source: function (request, response) {
    //    $.ajax({
    //        type: "GET",
    //        url: "/api/Consumer/GetAutoCompleteSearch",
    //        data: { terms: request.term },
    //        dataType: "json",
    //        cacheResults: true,
    //        contentType: "application/json; charset=utf-8",
    //        success: OnSuccess,
    //        error: function (XMLHttpRequest, textStatus, errorThrown) {
    //            // alert(textStatus);
    //        }
    //    });
    //    function OnSuccess(r) {
    //        response($.map(r, function (item) {
    //            return {
    //                value: item.City_Loc,
    //                label: item.City_Loc,
    //                val: item.Id,
    //                state: item.State,
    //                pine: item.Pincode,
    //            }
    //        }))
    //    }
    //},
    //select: function (e, i) {
    //    $("#txtLocation").val(i.item.label);
    //    $("#hdnLocationId").val(i.item.val);
    //    $("#txtState").val(i.item.state);
    //    $("#txtPincode").val(i.item.pine);
    //},
    //minLength: 2
});

$('#liProfile').click(function () {
    $('#divProfileInfo').show();
    $('#divEditProfile').hide();
    profileVM.GetUserProfileDetails(Consumer_Id);
});
$('#liSettings').click(function () {
    $('#txtCurnt_Pswd').val('');
    $('#txtNewPassword').val('');
    $('#txtConfirmPassword').val('');

});
$('#liMybooking').click(function () {

    profileVM.GetMyBooking(Consumer_Id);
});

$('#liMybookingCorporate').click(function () {

    $('#divMyBooking').hide();
    $('#divMyBookingCorporate').show();
});

//$('#liReportCorporate').click(function () {
//    
//    //$('#txtFrom').datepicker('getDate', null);
//    //$('#txtTo').datepicker('getDate', null);
//    //$('#txtCheckin').datepicker('getDate', null);
//    //$('#txtCheckOut').datepicker('getDate', null);
//    var a = new BindDataVMdefault();
//    a.BindData();
//    //ko.cleanNode(document.getElementById("BookingsDT"));
//    //ko.applyBindings(a, document.getElementById("BookingsDT"));
//});

var Cons_Id = $.localStorage("Consumer_Id");
var Consumer_Id = 0;

var profileVM = new Consumer_ProfileViewModel();
profileVM.GetUserId();


ko.applyBindings(profileVM, document.getElementById("headerName"));

//ko.applyBindings(profileVM, document.getElementById("user_profile"));
ko.applyBindings(profileVM, document.getElementById("divProfileInfo"));
ko.applyBindings(profileVM, document.getElementById("divEditProfile"));
ko.applyBindings(profileVM, document.getElementById("divSetting"));

function Consumer_ProfileViewModel() {
    var self = this;
    self.Cons_Id = ko.observable('');
    self.PrefixList = ko.observableArray(["Mr.", "Mrs", "Miss"]);
    self.Prefix = ko.observable();
    self.Cons_First_Name = ko.observable('');
    self.Cons_Last_Name = ko.observable('');
    self.User_Name = ko.computed(function () {
        return "" + self.Cons_First_Name() + " " + self.Cons_Last_Name();
    });
    self.Cons_mailid = ko.observable('');
    self.Cons_Mobile = ko.observable('');
    self.Points = ko.observable();

    self.Cons_Gender = ko.observable('');
    self.Cons_Dob = ko.observable('');
    self.Cons_Addr1 = ko.observable('');
    self.Cons_Addr2 = ko.observable('nan');
    self.Cons_State = ko.observable();
    self.Cons_City = ko.observable();
    self.Cons_Area = ko.observable('');
    self.Cons_Pincode = ko.observable('');
    self.Cons_Company = ko.observable('nan');
    self.Cons_Company_Id = ko.observable('nan');
    self.Cons_Reference = ko.observable('nan');
    self.Cons_Affiliates_Id = ko.observable('nan');
    self.Cons_Loyalty_Id = ko.observable('nan');
    self.Cons_Earned_Loyalpoints = ko.observable('nan');
    self.Cons_Redeemed_Loyalpoints1 = ko.observable('nan');
    self.Cons_Ipaddress = ko.observable('nan');
    self.OverviewCounter = ko.observable('0');
    self.OverviewDealsCount = ko.computed(function () {
        return self.OverviewCounter() + " Deals";
    });
    self.OverviewList = ko.observableArray(); // booked Overview List

    self.ProfileDataList = ko.observableArray();

    self.BookingCounter = ko.observable('0');
    self.BookingDealsCount = ko.computed(function () {
        return self.BookingCounter() + " Deals";
    });
    self.BookingList = ko.observableArray();  //My Booking

    self.SettingList = ko.observableArray();

    self.GetUserId = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/Signin/GetLoggedInUserId",
            // dataType: "json",
            async: false,
            success: function (response) {
                Consumer_Id = response;

                OnSuccessGetUserId(response);

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);
            }
        });
    }

    self.GetBookedDeals = function (Id) {

        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetBookedOverviewDeals',
            data: { Cons_Id: Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.OverviewCounter(data.Table.length);

                self.OverviewList.removeAll();
                for (var i = 0; i < data.Table.length; i++) {
                    self.OverviewList.push(new OverviewModel(data.Table[i], data.Table1));
                }

            },
            error: function (err) {
                // alert(err.status + " : " + err.statusText);
            }
        }).done(function () {

            //ko.applyBindings(profileVM, document.getElementById("divOverview"));

        });
    }

    self.GetMyBooking = function (Id) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetBookedDeals',
            data: { Cons_Id: Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                self.BookingCounter(data.Table.length);
                self.BookingList.removeAll();
                var rating = '';
                var Numberreviews = '';
                var ReviewUrl = '';
                var AjaxUrl = 'http://api.tripadvisor.com/api/partner/2.0/location/4492794?key=6e6c5fb839154bf3873229158cac5af7';
                for (var i = 0; i < data.Table.length; i++) {

                    AjaxUrl = data.Table[i].TripId;
                    $.ajax({

                        url: AjaxUrl,
                        type: 'GET',
                        dataType: 'json',
                        async: false,
                        success: function (trip) {


                            // data.rating_image_url
                            rating = trip.rating_image_url;
                            Numberreviews = trip.num_reviews;
                            ReviewUrl = trip.web_url;
                            //   self.HotelsList.push(new HotelListModel(data.Table[i], data.Table2[i].Roomtype, trip.rating_image_url))

                            // WriteResponse(data);
                        },
                        error: function (x, y, z) {
                            //  alert(x + '\n' + y + '\n' + z);
                        }
                    });
                    self.BookingList.push(new OverviewModel(data.Table[i], data.Table1, rating, Numberreviews, ReviewUrl));
                }

                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                //   alert(err.status + " : " + err.statusText);
            }
        }).done(function () {

            ko.applyBindings(profileVM, document.getElementById("divMyBooking"));
        });
        //$('#divMyBooking').show();
        //$('#divMyBookingCorporate').hide();
    }

    self.GetUserProfileDetails = function (Id) {

        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetProfileDetails',
            data: { Cons_Id: Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.ProfileDataList.removeAll();
                if (data.Table.length > 0) {


                    self.ProfileDataList.push(new UserProfileModel(data.Table[0]));

                    self.Cons_Id(data.Table[0].Cons_Id);
                    self.Cons_First_Name(data.Table[0].Cons_First_Name || '');
                    self.Cons_Last_Name(data.Table[0].Cons_Last_Name || '');
                    self.Cons_mailid(data.Table[0].Cons_mailid || '');
                    self.Cons_Mobile(data.Table[0].Cons_Mobile || '');
                    self.Cons_Gender(data.Table[0].Cons_Gender || '');
                    if (data.Table[0].Cons_Dob == null)
                        self.Cons_Dob('');
                    else {

                        //var date = new Date(parseInt(data.Table1[0].Cons_Dob.substr(6)));
                        //var displayDate = $.datepicker.formatDate("YYYY/MM/DD", date);
                        //self.Cons_Dob(displayDate);
                        self.Cons_Dob(data.Table[0].Cons_Dob);
                    }
                    self.Cons_Addr1(data.Table[0].Cons_Addr1 || '');
                    self.Cons_City(data.Table[0].Cons_City || '');
                    self.Cons_Area(data.Table[0].Cons_Area || '');
                    self.Cons_Pincode(data.Table[0].Cons_Pincode || '');
                    self.Cons_Company(data.Table[0].Cons_Company || '');
                    self.Cons_Company_Id(data.Table[0].Cons_Company_Id || '');
                    self.Cons_Reference(data.Table[0].Cons_Reference || '');
                    self.Cons_Affiliates_Id(data.Table[0].Cons_Affiliates_Id || '');
                    self.Cons_Loyalty_Id(data.Table[0].Cons_Loyalty_Id || '');
                    self.Cons_Earned_Loyalpoints(data.Table[0].Cons_Earned_Loyalpoints || '');
                    self.Cons_Redeemed_Loyalpoints1(data.Table[0].Cons_Redeemed_Loyalpoints1 || '');

                    self.SettingList.removeAll();
                    self.SettingList.push(new SettingModel(data.Table[0].Cons_Id, data.Table[0].Cons_mailid));     // Setting
                }

                //if (data.Table1 != null) {
                //    if (data.Table1.length > 0) {
                //        $("#hdnLocationId").val(data.Table1[0].Id || '');
                //        $("#txtLocation").val(data.Table1[0].Location + ', ' + data.Table1[0].City);
                //        $("#txtState").val(data.Table1[0].State || '');
                //        $("#txtPincode").val(data.Table1[0].Pincode || '');
                //    }
                //}

                AppCommonScript.HideWaitBlock(); //hide wait block
            },
            error: function (err) {
                // alert(err.status + " : " + err.statusText);
            }
        }).done(function () {

        });
    }

    self.EditProfile = function () {
        $('#divProfileInfo').hide();
        $('#divEditProfile').show();

    }

    self.SaveChanges = function (data) {


        if ($.isNumeric($('#txtPincode').val()) == true) {
            AppCommonScript.ShowWaitBlock();
            data.Cons_Dob = ConverToValidFormate($('#peekDob').val());
            data.Cons_City = $('#txtCity').val();
            data.Cons_Pincode = $('#txtPincode').val();
            data.Cons_Area = $('#txtLocation').val();
            $.ajax({
                type: "Post",
                url: '/api/Consumer/UpdateConsumerProfile',
                data: ko.toJSON(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Successed(response);

                    $('#divEditProfile').hide();
                    $('#divProfileInfo').show();

                    self.GetUserProfileDetails(Consumer_Id);

                    AppCommonScript.HideWaitBlock();
                },
                error: function (err) {
                    Failed(JSON.parse(err.responseText));
                }
            });
        }
        else {
            alert('Invalid Pincode');
            $('#txtPincode').val('');
            $('#txtPincode').focus();
        }
    }

    self.DiscardChanges = function () {
        $('#divProfileInfo').show();
        $('#divEditProfile').hide();
    }

    function CleansNodes() {
        ko.cleanNode(document.getElementById("headerName"));
        ko.cleanNode(document.getElementById("user_profile"));
        ko.cleanNode(document.getElementById("divProfileInfo"));
        ko.cleanNode(document.getElementById("divEditProfile"));
    }

    function OnSuccessGetUserId(Id) {

        self.GetBookedDeals(Id);

        self.GetUserProfileDetails(Id);

    }
}

function UserProfileModel(data) {
    var self = this;
    data = data || {};
    self.Cons_Id = ko.observable(data.Cons_Id);
    self.Cons_First_Name = ko.observable(data.Cons_First_Name);
    self.Cons_Last_Name = ko.observable(data.Cons_Last_Name);
    self.User_Name = ko.computed(function () {
        return "Mr. " + self.Cons_First_Name() + " " + self.Cons_Last_Name();
    });
    self.Cons_mailid = ko.observable(data.Cons_mailid);
    self.Cons_Mobile = ko.observable(data.Cons_Mobile);
    self.Points = ko.observable();
    self.Cons_Gender = ko.observable(data.Cons_Gender);
    self.Cons_Dob = ko.observable(data.Cons_Dob);
    self.Cons_Addr1 = ko.observable(data.Cons_Addr1);
    self.Cons_City = ko.observable(data.Cons_City);
    self.Cons_Area = ko.observable(data.Cons_Area);
    self.Cons_Pincode = ko.observable(data.Cons_Pincode);
    self.Cons_Company = ko.observable(data.Cons_Company);
    self.Cons_Company_Id = ko.observable(data.Cons_Company_Id);
    self.Cons_Reference = ko.observable(data.Cons_Reference);
    self.Cons_Affiliates_Id = ko.observable(data.Cons_Affiliates_Id);
    self.Cons_Loyalty_Id = ko.observable(data.Cons_Loyalty_Id);
    self.Cons_Earned_Loyalpoints = ko.observable(data.Cons_Earned_Loyalpoints);
    self.Cons_Redeemed_Loyalpoints1 = ko.observable(data.Cons_Redeemed_Loyalpoints1);

}

function SettingModel(Cons_Id, Email) {
    var self = this;
    self.Cons_Id = ko.observable(Cons_Id);
    self.Curnt_Pswd = ko.observable();
    self.NewPassword = ko.observable();
    self.ConfirmPassword = ko.observable();

    self.Submit = function (data) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Consumer/ChangePassword",
            dataType: "json",
            data: $.parseJSON(ko.toJSON(data)),
            success: function (response) {
                Successed(response);
                self.Cancel();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.Cancel = function () {
        self.Curnt_Pswd('');
        self.NewPassword('');
        self.ConfirmPassword('');
    }

    self.IsChecked = ko.observable(true);
    self.EmailLatter = ko.observable();
    self.NoEmailLatter = ko.observable();

    self.Email = ko.observable(Email);
    self.Ipaddress = ko.observable();
    $.getJSON("http://jsonip.com?callback=?", function (data) {
        self.Ipaddress = data.ip;
    });



    self.ChangeEmailLatter = function (data) {



        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Post",
            url: '/api/Consumer/SubscribeEmailLatter',
            data: ko.toJSON(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.ReturnMessage[0] == "You are allready Subscribed.") {
                    if (confirm("Do you want to UnSubscribe?") == false) {

                        AppCommonScript.HideWaitBlock();

                        $.ajax({
                            type: "Post",
                            url: '/api/Consumer/SubscribeEmailLatter',
                            data: ko.toJSON(data),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                alert('You are successfully Subscribed')
                            },
                            error: function (response) {
                            }
                        });

                        //  self.EmailLatter(true);
                    }
                    else {
                        AppCommonScript.HideWaitBlock();

                        $.ajax({
                            type: "Post",
                            url: '/api/Consumer/UnSubscribeEmailLatter',
                            data: ko.toJSON(data),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                alert('You are successfully Unsubscribed')
                            },
                            error: function (response) {
                            }
                        });

                    }

                }
                else {

                    var bdy = '<b>Thank you for subscribing to our newsletter</b>.<br/><br>You have been successfully added to our mailing list. We will keep you updated with our latest news and our deals.<br/><br/>Thank you for choosing Lastminutekeys<br/><br/>The Lastminutekeys Team<br/>www.lastminutekeys.com';
                    var sub = 'Newsletter Subscription';
                    var rcvr = self.Email();
                    // Successed(response.ReturnMessage[0])
                    $.ajax({
                        type: "POST",
                        url: "/api/Consumer/SendMail",
                        data: { Cons_Subject: sub, Cons_Body: bdy, Cons_mailid: rcvr },
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            AppCommonScript.HideWaitBlock();
                            //  Successed(response)

                            // SubVM.Email('')
                        },
                        error: function (err) {
                            AppCommonScript.HideWaitBlock();
                            Failed(JSON.parse(err.responseText));
                        }
                    });
                }

            },
            error: function (err) {

                Failed(JSON.parse(err.responseText));
            }
        });
        AppCommonScript.HideWaitBlock();
    }

}

function SubscribeEmailLatter(data) {


}

function MyBookingModel() {
    var self = this;
    data = data || {};
}
var StarCount = 0;
function OverviewModel(data, data2, rating, Numberreviews, ReviewUrl) {



    var self = this;



    self.Cons_Id = data.Cons_Id;
    self.Invce_Num = data.Invce_Num;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Prop_Image = ko.observable(data.Image_dir);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_Address = ko.observable(data.Prop_Addr1);
    self.Trip_Rating = rating;
    self.Trip_Review = Numberreviews + ' reviews';
    self.Trip_Review_Url = ReviewUrl;
    if (data.Image_dir == 'img/prop_image/hiddengem.jpg') {
        self.Trip_Review = "";
    }
    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }

    var dateIn = new Date(data.Checkin).toDateString();
    self.CheckIn = ko.observable(dateIn);
    var dateOut = new Date(data.Checkout).toDateString();
    self.CheckOut = ko.observable(dateOut);
    self.Prop_Star_Rating = ko.observable(data.Prop_Star_Rating);

    self.InvoiceDate = ko.observable(data.Invce_date);

    self.RoomName = ko.observable(data.Room_Name);
    var NetAmnt = data.net_amt
    //NetAmnt = NetAmnt.toFixed(2);



    self.RoomPrice = ko.observable(NetAmnt);

    self.RoomFacilityList = ko.observableArray();

    for (var i = 0; i < data2.length; i++) {
        if (data2[i].Room_Id == data.Room_Id)
            self.RoomFacilityList.push(new RoomFacilityModel(data2[i]));
    }
    //DT(data);


    self.isEmail = ko.observable(false);
    self.isSMS = ko.observable(false);

    self.DT = function (self) {

        $.localStorage("Cons_Id", self.Cons_Id);
        $.localStorage("Invce_Num", self.Invce_Num);
        window.location.href = '/Consumer/Invoice/';
    };


    self.SendEmail = function (self) {


        var InvoiceNo = data.Invce_Num;
        var Authority = "3";

        $.ajax({
            type: "GET",
            url: "/api/Consumer/SendInvoiceEmail",
            data: { InvoiceNumber: InvoiceNo, Email: Authority },
            dataType: "json",
            success: function (response) {
                Successed(response);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.SendSMS = function (self) {

        var InvoiceNo = data.Invce_Num;

        $.ajax({
            type: "GET",
            url: "/api/Consumer/SendInvoiceSMS",
            data: { InvoiceNumber: InvoiceNo },
            dataType: "json",
            success: function (response) {
                Successed(response);
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
};




function RoomFacilityModel(data) {
    var self = this;
    data = data || {};
    self.FacilityName = ko.observable(data.Facility_Name);
    self.Facility_Image_dir = ko.observable(data.Facility_Image_dir);
}

function RatingModel(data) {
    var self = this;
    self.MyStarClass = ko.observable('s0');
    if (data > 0) {
        self.MyStarClass('s1');
    }
}

function GetAllLocation() {
    var allcities = ko.observableArray([{ Id: '0', CityName: '--Select--' }]);
    $.getJSON("/api/property/allcities", function (result) {
        ko.utils.arrayMap(result, function (item) {
            allcities.push({ Id: item.City_Id, CityName: item.CityName });
        });
    });
    return allcities;
}

function GetAllStates() {
    var allcities = ko.observableArray([{ Id: '0', StateName: '--Select--' }]);
    $.getJSON("/api/Consumer/AllStates", function (result) {
        ko.utils.arrayMap(result, function (item) {
            allcities.push({ Id: item.Id, StateName: item.State });
        });
    });
    return allcities;
}

function GetAllPincodes() {
    var allcities = ko.observableArray([{ Id: '0', Pincode: '--Select--' }]);
    $.getJSON("/api/Consumer/AllPincodes", function (result) {
        ko.utils.arrayMap(result, function (item) {
            allcities.push({ Id: item.Id, Pincode: item.Pincode });
        });
    });
    return allcities;
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function ConverToValidFormate(formateDate) {
    var date = formateDate.split('/');
    shortDate = date[2] + "/" + date[1] + "/" + date[0];

    return shortDate;
}

function SplitTheLocation(location) {
    var data = location.split(',');
}
function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

//View Model
var ComUsersListVM = function () {

    Prop = this;
    Prop.getAllComUsers = function () {
        var Cons_Id = $.localStorage("Consumer_Id");
        var self = this;
        self.corporates = ko.observableArray([]),

        $.ajax({
            type: "GET",
            url: '/api/Corporate/AllCompanyUser',
            data: { EmailId: Cons_Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                self.corporates(data);
                ko.applyBindings(AllComUsers, document.getElementById("corporateTd"));

                //   $(".corporateTd").DataTable({ responsive: true, 'iDisplayLength': 15 });
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    }
}

function BindDataVM() {




    var self = this;
    self.Bookings = ko.observableArray();
    var data = 90;

    self.BindData = function () {

        var Inv_From = document.getElementById("txtFrom").value;
        var Inv_To = document.getElementById("txtTo").value;
        //var start = $('#txtFrom').datepicker('getDate');
        //var end = $('#txtTo').datepicker('getDate');
        //var Checkin = $('#txtCheckin').datepicker('getDate');
        //var Checkout = $('#txtCheckOut').datepicker('getDate');
        //if (!start || !end) return;
        //var days = (end - start) / (1000 * 60 * 60 * 24);
        //$.localStorage('days', days);
        //$.localStorage('fromdate', start);
        //$.localStorage('todate', end);
        //$.localStorage('Checkin', "");
        //$.localStorage('Checkout', "");

        var Ven_Id = $.localStorage("Consumer_Id");
        var bookingStatus = $('#BookingStatus').val();
        //var Inv_From = $.localStorage("fromdate");
        //var Inv_To = $.localStorage("todate");
        //var Check_in = $.localStorage("Checkin");
        //var Check_out = $.localStorage("Checkout");
        //AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: '/api/Booking/CorpBookingList?Cons_Id=' + Ven_Id + '&bookingStatus=' + bookingStatus + '&InvFrom=' + Inv_From + '&InvTo=' + Inv_To,
            contentType: "application/json; charset=utf-8",
            async: false,
            //data: { VendID: Ven_Id, InvFrom: Inv_From, InvTo: Inv_To, Checkin: Check_in, Checkout: Check_out },
            dataType: "json",
            success: function (data) {

                //var table = $("#example").dataTable();
                //table.fnDestroy();

                self.Bookings.removeAll();

                
                for (var i = 0; i < data.Table.length; i++) {
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }
                //$('#example').dataTable({ 'iDisplayLength': 10 });
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });

    }
}

function BindDataVMdefault() {




    var self = this;
    self.Bookings = ko.observableArray();
    var data = 90;

    self.BindData = function () {


        //    var start = $('#txtFrom').datepicker('getDate');
        //    var end = $('#txtTo').datepicker('getDate');
        //    var Checkin = $('#txtCheckin').datepicker('getDate');
        //    var Checkout = $('#txtCheckOut').datepicker('getDate');
        ////if (!start || !end) return;
        ////var days = (end - start) / (1000 * 60 * 60 * 24);
        ////$.localStorage('days', days);
        //$.localStorage('fromdate', start);
        //$.localStorage('todate', end);
        //$.localStorage('Checkin', Checkin);
        //$.localStorage('Checkout', Checkout);

        var Ven_Id = $.localStorage("Consumer_Id");
        var Inv_From = $.localStorage("fromdate", "");
        var Inv_To = $.localStorage("todate", "");
        var Check_in = $.localStorage("Checkin", "");
        var Check_out = $.localStorage("Checkout", "");
        //AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: '/api/Booking/CorpBookingList?Cons_Id=' + Ven_Id + '&InvFrom=' + Inv_From + '&InvTo=' + Inv_To + '&Checkin=' + Check_in + '&Checkout=' + Check_out,
            contentType: "application/json; charset=utf-8",
            async: false,
            //data: { VendID: Ven_Id, InvFrom: Inv_From, InvTo: Inv_To, Checkin: Check_in, Checkout: Check_out },
            dataType: "json",
            success: function (data) {

                self.Bookings.removeAll();
                for (var i = 0; i < data.Table.length; i++) {
                    self.Bookings.push(new BookingClass(data.Table[i])); //Put the response in ObservableArray
                }

            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }

        });
        //ko.cleanNodetElementById("BookingsDT"));
        ko.applyBindings(a, document.getElementById("BookingsDT"));
    }
}

function SearchResultViewModel() {

    // AppCommonScript.ShowWaitBlock();




    self.getBooking = function () {


    }

}

function BookingClass(data) {
    
    var prop = this;

    prop.Invce_Num = data["Invce_Num"];
    prop.Destination = data["City_Name"];

    prop.Prop_Name = ko.observable(data["Prop_Name"]);
    prop.Room_Type = ko.observable(data["Room_Name"]);
    var date = new Date(data["Checkin"]).toDateString();
    
    prop.Checkin = ko.observable(date);
    var date1 = new Date(data["Checkout"]).toDateString();

    prop.Checkout = ko.observable(date1);
    
    prop.NumberofNights = data["Days_Count"];

    prop.Invce_date = new Date(data["Invce_date"]).toDateString();



    prop.Name = ko.observable(data["Name"]);
    prop.Mobile = ko.observable(data["Mobile"]);
    prop.Email = ko.observable(data["cons_mailid"]);
    prop.City = ko.observable(data["cons_city"]);
    prop.Rate = ko.observable(data["camo_room_rate"]);
    prop.Tax = ko.observable(data["tax_amnt"]);
    prop.Amount = ko.observable(data["net_amt"]);

    prop.BookingStatus = ko.observable(data["BookingStatus"]);

    self.DT = function (data) {


        $.localStorage("Invce_Nums", data.Invce_Num);
        window.location.href = '/Corporate/ReportTransactions/';


    };
}

//function RefineReport() {
//    var a = new BindDataVM();
//    a.BindData();
//    ko.cleanNode(document.getElementById("BookingsDT"));
//    ko.applyBindings(a, document.getElementById("BookingsDT"));

//    //  window.location.reload();
//}

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

    $('body').prepend("<form method='post' action='/Consumer/marginExport' style='top:-3333333333px;' id='tempForm'><input type='hidden' name='data' value='" + escape(tab_text) + "' ></form>");
    $('#tempForm').submit();
    $("tempForm").remove();
    return false;

}