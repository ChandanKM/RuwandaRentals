
var searchVM = new SearchHotelViewModel();

var Corp_Id = $.localStorage("Corporate_Id");

if (Corp_Id != undefined && Corp_Id != "")
{
    $('#Email').hide();
}
else
{
    $('#Email').show();
}
$("#txtLocation").autocomplete({

    source: function (request, response) {
        $('#txtLocation').addClass('loadinggif');
        $.ajax({
            type: "GET",
            url: "/api/Corporate/GetAutoCompleteSearchResult",
            data: { terms: request.term },
            dataType: "json",
            cacheResults: true,
            contentType: "application/json; charset=utf-8",
            success: OnSuccess,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // alert(textStatus);
            }
        });
        function OnSuccess(r) {
            response($.map(r, function (item) {
                return {
                    value: item.City_Loc,
                    label: item.City_Loc,
                    val: item.City_Loc
                }
            }))
            $('#txtLocation').removeClass('loadinggif');
        }
    },
    select: function (e, i) {
        $("#txtLocation").val(i.item.label);
        $("#hdnLocationId").val(i.item.val);
    },
    minLength: 1
});
ko.applyBindings(searchVM, document.getElementById("formSearch"));
var SubVM = new SubscribeViewModel();
ko.applyBindings(SubVM, document.getElementById("formSubscribe"));

var indexVM = new IndexHomeViewModel();
indexVM.GetHiddenGems();
indexVM.GetRecommendedHotels();
indexVM.GetBestOffers();




var NoOfNight = 1;
function showDays() {
    var start = $('#txtCheckIn').datepicker('getDate');
    var end = $('#txtCheckOut').datepicker('getDate');
    if (!start || !end) return;
    var days = (end - start) / (1000 * 60 * 60 * 24);
    NoOfNight = days; //for store the no. of Night
}
$("#txtCheckIn").datepicker({
    minDate: 0,
    maxDate: '+3D',
    dateFormat: 'dd/mm/yy',
    onSelect: function () {
        var minDate = $("#txtCheckIn").datepicker('getDate');
        var newMin = new Date(minDate.setDate(minDate.getDate() + 1));
        $("#txtCheckOut").datepicker("option", "minDate", newMin);
        var start = $('#txtCheckIn').datepicker('getDate');
        var end = $('#txtCheckOut').datepicker('getDate');
        if (!start || !end) return;
        var days = (end - start) / (1000 * 60 * 60 * 24);
        NoOfNight = days; //for store the no. of Night
    }
});
$("#txtCheckOut").datepicker({
    minDate: 1,
    maxDate: '+3D',
    dateFormat: 'dd/mm/yy',
    //showOn: "both",
    //buttonImage: "http://jqueryui.com/demos/datepicker/images/calendar.gif",
    //buttonImageOnly: true
});
$("#txtCheckOut1").click(function (e) {
    
    $("#txtCheckOut").trigger('click');
})
$('#txtCheckIn').val(GetDate(0));
$('#txtCheckOut').val(GetDate(1));

var searchVM = new SearchViewModel();

function SearchHotelViewModel() {
    var self = this;

    self.Location = ko.observable();
    self.City_Id = ko.observable();
    self.CheckIn = ko.observable();
    self.CheckOut = ko.observable();
    self.RoomSelectList = ['1 Room', '2 Rooms', '3 Rooms', '4 Rooms', '5 Rooms'];
    self.RoomCount = ko.observable();

    self.SearchSubmit = function () {

        if ($('#formSearch').valid()) {

            //$.localStorage("City_Id", $("#hdnLocationId").val());
            //$.localStorage("Location", $("#txtLocation").val());
            //$.localStorage("CheckInDate", $("#txtCheckIn").val());
            //$.localStorage("CheckOutDate", $("#txtCheckOut").val());
            //$.localStorage("NoOfRoom", self.RoomCount());


            //window.location.href = '/Search_Results'

            // For
            searchVM.CityMasterId = $("#hdnLocationId").val();
            var din = $("#txtCheckIn").val();
            var dout = $("#txtCheckOut").val();
            din = ConverToValidFormate(din);
            var dout = ConverToValidFormate(dout);
            searchVM.Room_Checkin = din;
            searchVM.Room_Checkout = dout;
            searchVM.Room_Checkout = ConverToValidFormate($("#txtCheckOut").val());
            var rmCount = ($('#country :selected').text());
            var nor = rmCount.split(' ');
            //searchVM.RoomCount = nor[0];
            // alert(nor[0])
            // $.localStorage("NoOfRoom", nor[0]);
            searchVM.No_Of_Rooms = nor[0];

            searchVM.Price1 = 0;
            searchVM.Price2 = 90000;
            searchVM.Rating = '%';
            searchVM.Facilities = '';
            searchVM.SortBy = 'rating';
            self.GetResults(searchVM);
        }
    }


    self.BindCountries = function () {
        $.ajax({
            url: "api/Property/GetCountries",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            type: 'GET',
            success: function (country) {
                OnSuccessBindCountries(country);
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }

    function OnSuccessBindCountries(country) {
        for (var item = 0 ; item < country.length ; item++) {
            self.Countries.push(new BindCountries(country[item].Country_Name, country[item].Country_Code));
        }
    }

    function myfunction() {
        $.ajax({
            type: "POST",
            url: "api/Corporate/SearchHotels",
            data: ko.toJSON(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                Successed(response);
                window.location.href = '/Search_Results'
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }

    self.GetResults = function (searchVM) {
        $.ajax({
            type: "Post",
            url: '/api/Corporate/HotelListing_Sort',
            data: ko.toJSON(searchVM),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var rmCount = ($('#country :selected').text());
                var nor = rmCount.split(' ');
                //searchVM.RoomCount = nor[0];
                // alert(nor[0])
                
                $.localStorage("NoOfRoom", nor[0]);

                $.localStorage("City_Id", $("#hdnLocationId").val());
                $.localStorage("Location", $("#txtLocation").val());
                $.localStorage("CheckInDate", $("#txtCheckIn").val());
                $.localStorage("CheckOutDate", $("#txtCheckOut").val());
                //$.localStorage("NoOfRoom", nor[0]);
                $.localStorage("NoOfNight", NoOfNight);

                window.location.href = '/Search_Results'
                //}
                //else {
                //    var result = { Status: true, ReturnMessage: { ReturnMessage: "No Result Found.. Please try with Different values" }, ErrorType: "Success" };
                //    Failed(result);
                //}
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
}

function SearchViewModel() {
    var self = this;
    self.CityMasterId = ko.observable();
    self.Room_Checkin = ko.observable();
    self.Room_Checkout = ko.observable();
    self.No_Of_Rooms = ko.observable();
    self.Facilities = ko.observable();
    self.Price1 = ko.observable();
    self.Price2 = ko.observable();
    self.Rating = ko.observable();
    self.SortBy = ko.observable();
}

function IndexHomeViewModel() {
    var self = this;
    self.HiddenGems = ko.observable();
    self.HiddenGemsDescr = ko.observable();
    self.RecommendedHotelsList = ko.observableArray();
    self.BestOffersList = ko.observableArray();

    self.GetHiddenGems = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Corporate/GetHiddenGems',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.Table != null) {
                    if (data.Table.length > 0) {
                        self.HiddenGemsDescr(data.Table[0].Vparam_Descr + '. ' + data.Table[0].Prop_Overview + '. ' + data.Table[0].city + ', ' + data.Table[0].state);
                    }
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
    self.GetRecommendedHotels = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Corporate/GetRecommendedHotels',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.RecommendedHotelsList.removeAll();
                if (data.Table != null) {
                    if (data.Table.length > 7) {
                        for (var i = 0; i < 7; i++) {

                            var rating = '';
                            var Numberreviews = '';
                            var AjaxUrl = 'http://api.tripadvisor.com/api/partner/2.0/location/4492794?key=6e6c5fb839154bf3873229158cac5af7';
                            for (var i = 0; i < data.Table.length; i++) {

                                AjaxUrl = data.Table[i].tripid;
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
                                self.RecommendedHotelsList.push(new HotelModel(data.Table[i], rating, Numberreviews, ReviewUrl));
                            }
                        }
                    }
                    ko.applyBindings(indexVM, document.getElementById("divHiddenRecommended"));
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
    self.GetBestOffers = function () {

        var rating = '';
        var Numberreviews = '';
        var ReviewUrl = '';
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Corporate/GetBestOffers',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                self.BestOffersList.removeAll();
                if (data.Table != null) {
                    if (data.Table.length > 6) {
                       // 
                        for (var i = 0; i < 6; i++) {
                            AjaxUrl = data.Table[i].tripid;
                            $.ajax({

                                url: AjaxUrl,
                                type: 'GET',
                                dataType: 'json',
                                async: false,
                                success: function (trip) {

                                    //    
                                    // data.rating_image_url
                                    rating = trip.rating_image_url;
                                    Numberreviews = trip.num_reviews;
                                    ReviewUrl = trip.web_url;
                                    //   self.HotelsList.push(new HotelListModel(data.Table[i], data.Table2[i].Roomtype, trip.rating_image_url))

                                    // WriteResponse(data);
                                    if (data.Table[i].Image_dir == "img/prop_image/hiddengem.jpg") {

                                        Numberreviews = '';
                                        //  ReviewUrl = '';

                                    }
                                },
                                error: function (x) {
                                    // alert(x);
                                }
                            });
                            self.BestOffersList.push(new HotelModel(data.Table[i], rating, Numberreviews, ReviewUrl));
                        }
                    }
                }
                ko.applyBindings(indexVM, document.getElementById("divBestOffers"));
                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
}

function RecommendedModel(data) {
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_OverView = ko.observable(data.Prop_Overview);
}

var StarCount = 0;
function HotelModel(data, rating, Numberreviews, ReviewUrl) {
    var self = this;
    data = data || {};

    self.Trip_Rating = rating;
    self.Trip_Review = Numberreviews + ' reviews';
    self.Trip_Review_Url = ReviewUrl;
    self.Prop_Id = ko.observable(data.PropId);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_OverView = ko.observable(data.Prop_Overview);
    self.City = ko.observable(data.city);
    self.Location = ko.observable(data.location + ', ' + data.city + ', ' + data.state);
    self.TempAllInOne = ko.observable(data.city + ', ' + data.state);
    self.StarRate = ko.observable(data.Prop_Star_Rating);
    self.Image_Url = ko.observable(data.Image_dir);
    if (data.Image_dir == 'img/prop_image/hiddengem.jpg') {
        self.Trip_Review = "";
    }
    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }
    self.BookNow = function (data) {
        

        $.localStorage("Prop_Id", data.Prop_Id());
        $.localStorage("City_Id", data.City());
        $.localStorage("Location", data.City());
        $.localStorage("CheckInDate", GetDate(0));
        $.localStorage("CheckOutDate", GetDate(1));
        $.localStorage("NoOfRoom", "1");

        window.location.href = '/hotel_details';
    }

}
function RatingModel(data) {
    var self = this;
    self.Star_Id = ko.observable(data);
    self.MyStarClass = ko.observable('s0');
    if (data > 0) {
        self.MyStarClass('s1');
    }
}
function BindCountries(ddlText, ddlValue) {
    this.CountryValue = ko.observable(ddlValue);
    this.CountryName = ko.observable(ddlText);
}

function GetDate(a) {
    var fullDate = new Date();
    var twoDigitMonth = fullDate.getMonth() + 1 + "";
    if (twoDigitMonth.length == 1)
        twoDigitMonth = "0" + twoDigitMonth;
    var twoDigitDate = fullDate.getDate() + a + "";
    if (twoDigitDate.length == 1)
        twoDigitDate = "0" + twoDigitDate;
    return currentDate = twoDigitDate + "/" + twoDigitMonth + "/" + fullDate.getFullYear();
    //New Code
    // var today = new Date();
    // var tomorrow = new Date(today);
    // tomorrow.setDate(today.getDate() + a);
    // var days = tomorrow.toLocaleDateString().split('/');
    //var newdt=(days[1]+'/'+days[0]+'/'+days[2])
    //return newdt;
};
//   var dt = data.DOB;
//var date = ConvertJsonDateString(dt);
function ConvertJsonDateString(jsonDate) {
    var shortDate = null;
    if (jsonDate) {
        var regex = /-?\d+/;
        var matches = regex.exec(jsonDate);
        var dt = new Date(parseInt(matches[0]));
        var month = dt.getMonth() + 1;
        var monthString = month > 9 ? month : '0' + month;
        var day = dt.getDate();
        var dayString = day > 9 ? day : '0' + day;
        var year = dt.getFullYear();
        shortDate = monthString + '-' + dayString + '-' + year;
    }

    return shortDate;
};

function ConverToValidFormate(formateDate) {

    var date = formateDate.split('/');
    shortDate = date[2] + "/" + date[1] + "/" + date[0];
    //var dt = new Date(formateDate.toString());
    //var month = dt.getMonth() + 1;
    //var monthString = month > 9 ? month : '0' + month;
    //var day = dt.getDate();
    //var dayString = day > 9 ? day : '0' + day;
    //var year = dt.getFullYear();
    //shortDate = monthString + '/' + dayString + '/' + year;

    return shortDate;
}

function SubscribeViewModel() {
    
    var self = this;
    self.Email = ko.observable();
    self.Ipaddress = ko.observable();
    $.getJSON("http://jsonip.com?callback=?", function (data) {
        self.Ipaddress = data.ip;
    });

    self.SubscribeEmail = function (subData) {
        
        if ($('#Email').val() != "")
        {
            var Emailsendid = subData.Email();
            //   alert(Emailsendid);


            
            var email = Emailsendid;
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(email.value)) {
                alert('Please provide a valid email address');
                email.focus;
                return false;
            }
            
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                type: "Post",
                url: '/api/Corporate/SubscribeEmailLatter',
                data: ko.toJSON(subData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    if (response.ReturnMessage[0] == "You are allready Subscribed.")
                    {
                        //alert('You are successfully subscribed')
                        if (confirm("Do you want to UnSubscribe?") == false) {

                            AppCommonScript.HideWaitBlock();

                            $.ajax({
                                type: "Post",
                                url: '/api/Corporate/SubscribeEmailLatter',
                                data: ko.toJSON(subData),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    alert('You are successfully subscribed')
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
                                url: '/api/Corporate/UnSubscribeEmailLatter',
                                data: ko.toJSON(subData),
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
                        
                        var bdy = '<div style="width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:"segoe UI";"><div style="background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;"><a href="#" style="display:inline-block;"><img src="http://www.lastminutekeys.com/img/Mailer/logo.png" title="Last Minute Keys" alt="" style="display:block;" /></a> </div><p style="color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);"><b>Thank you for subscribing to our newsletter</b>.<br/><br>You have been successfully added to our mailing list. We will keep you updated with our latest news and our deals.<br/><br/>Thank you for choosing Lastminutekeys<br/><br/>The Lastminutekeys Team<br/>www.lastminutekeys.com</p><div style="background:#192b3e;padding:10px;"><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;">Follow Us</h4><ul style="padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;"><li style="float:left;width:33.3%;text-align:left;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-iCorp.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;"></i> Follow us on Twitter</a></li><li style="float:left;width:33.3%;text-align:center;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-iCorp.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Facebook</a></li><li style="float:left;width:33.3%;text-align:right;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-iCorp.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Instagram</a></li><div style="clear:both;"></div></ul><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;">Contact OurCustomer Care</h4><p style="color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;"><a href="www.lastminutekeys.com/Home/Contact_Us" style="color:#FFF;font-size:14px;font-weight:400;">Click here</a></p><p style="color:#F4F4F4;text-align:center;font-size: 12px;line-height: 1.6em;margin:10px 0;"></p></div></div>'
                        //   var bdy = '<b>Thank you for subscribing to our newsletter</b>.<br/><br>You have been successfully added to our mailing list. We will keep you updated with our latest news and our deals.<br/><br/>Thank you for choosing Lastminutekeys<br/><br/>The Lastminutekeys Team<br/>www.lastminutekeys.com';
                        var sub = 'Newsletter Subscription';
                        var rcvr = Emailsendid;
                        // Successed(response.ReturnMessage[0])
                        $.ajax({
                            type: "POST",
                            url: "/api/Corporate/SendMail",
                            data: { Corp_Subject: sub, Corp_Body: bdy, Corp_mailid: rcvr },
                            dataType: "json",
                            async: false,
                            success: function (response) {

                                AppCommonScript.HideWaitBlock();
                                alert('You are successfully Subscribed')

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
        else
        {
            alert('Please enter Email Id');
        }        
    }

    self.UnSubscribeEmail = function (subData) {
        
        if ($('#Email').val() != "") {
            var Emailsendid = subData.Email();
            //   alert(Emailsendid);


            
            AppCommonScript.ShowWaitBlock();
            $.ajax({
                type: "Post",
                url: '/api/Corporate/SubscribeEmailLatter',
                data: ko.toJSON(subData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    AppCommonScript.HideWaitBlock();

                    $.ajax({
                        type: "Post",
                        url: '/api/Corporate/UnSubscribeEmailLatter',
                        data: ko.toJSON(subData),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert('You are successfully Unsubscribed')
                        },
                        error: function (response) {
                        }
                    });
                },
                error: function (err) {

                    Failed(JSON.parse(err.responseText));
                }
            });
            AppCommonScript.HideWaitBlock();
        }
        else {
            alert('Please enter Email Id');
        }
    }
}

function GemsMore() {
    $.localStorage("ViewMore", 'Gems');
    window.location.href = '/Search_Results/More_Results'
}
function RecommendMore() {
    $.localStorage("ViewMore", 'Recommend');
    window.location.href = '/Search_Results/More_Results'
}
function offerMore() {
    $.localStorage("ViewMore", 'Best');
    window.location.href = '/Search_Results/More_Results'
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
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