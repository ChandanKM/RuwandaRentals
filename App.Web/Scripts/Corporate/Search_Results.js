$("#txtLocation").autocomplete({
    source: function (request, response) {

        $('#txtLocation').addClass('loadinggif1');
        $.ajax({
            type: "GET",
            url: "/api/Consumer/GetAutoCompleteSearchResult",
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
            $('#txtLocation').removeClass('loadinggif1');
        }
    },
    select: function (e, i) {
        $("#txtLocation").val(i.item.label);
        $("#hdnLocationId").val(i.item.val);
    },
    minLength: 2
});

var checkIn = new Date($.localStorage("CheckInDate")).toDateString();
var checkOut = new Date($.localStorage("CheckOutDate")).toDateString();
$('#hdnLocationId').val($.localStorage("City_Id"));
$('#txtLocation').val($.localStorage("Location"));
$('#txtCheckIn').val($.localStorage("CheckInDate"));
$('#txtCheckOut').val($.localStorage("CheckOutDate"));
SplitTheLocation($.localStorage("Location"));

var NoOfNight = $.localStorage("NoOfNight");
var NoOfRoom = $.localStorage("NoOfRoom");
function showDays() {
    var start = $('#txtCheckIn').datepicker('getDate');
    var end = $('#txtCheckOut').datepicker('getDate');
    if (!start || !end) return;
    var days = (end - start) / (1000 * 60 * 60 * 24);
    //   $('#txtDayCount').val(days + " Night");
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
        //   $('#txtDayCount').val(days + " Night");
        NoOfNight = days; //for store the no. of Night
    }

});
$("#txtCheckOut").datepicker({
    minDate: 1,
    maxDate: '+3D',
    dateFormat: 'dd/mm/yy',
    onSelect: showDays
});

//min is 500
var minPrice = 1;
var maxPrice = 50000;
var starValue = '%';
var facilityValue = '';
var CityValue = '';

//price range slider
$("#slider-range").slider({
    range: true,
    min: minPrice,
    max: maxPrice,
    step: 500,
    values: [minPrice, maxPrice],
    slide: function (event, ui) {
        $("#amount").val("₹" + ui.values[0] + " - ₹" + ui.values[1]);
    },
    change: function (event, ui) {
        PriceRangeChange(ui.values[0], ui.values[1]);
    },
});

$("#amount").val("₹" + $("#slider-range").slider("values", 0) +
    " - ₹" + $("#slider-range").slider("values", 1));

function PriceRangeChange(min, max) {
    minPrice = min;
    maxPrice = max;
    FilterBy();
}
function resetSlider() {

    var $slider = $("#slider-range");
    $slider.slider("values", 0, 1);
    $slider.slider("values", 1, 50000);
    PriceRangeChange(500, 50000);
    $("#amount").val("₹" + $("#slider-range").slider("values", 0) +
    " - ₹" + $("#slider-range").slider("values", 1));
}
function StarRatingChange() {
    $('#5').removeClass('rest-rating')
    $('#4').removeClass('rest-rating')
    $('#3').removeClass('rest-rating')
    $('#2').removeClass('rest-rating')
    $('#1').removeClass('rest-rating')
    starValue = $("input[name='rating-input-1']:checked").val();
    FilterBy();
}
function StarRatingChangewithnorate() {
    starValue = "%";
    $('#5').addClass('rest-rating')
    $('#4').addClass('rest-rating')
    $('#3').addClass('rest-rating')
    $('#2').addClass('rest-rating')
    $('#1').addClass('rest-rating')
    //   $("input[name='rating-input-1']:checked"==false)
    FilterBy();
}
function ResetLocation() {

    $("input[name='Filterlocation']").prop('checked', false);

    //   $("input[name='rating-input-1']:checked"==false)
    $('#hdnLocationId').val($('#txtLocation').val());
    FilterBy();
}
function StarRatingChangeFacility() {
    $("input[name='checkboxFacility']").prop('checked', false);
    // $('#checkboxFacility').checked == false;
    facilityValue = ""

    FilterBy();
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

function FilterBy() {

    searchVM.CityMasterId = $('#hdnLocationId').val();
    searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
    searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
    //searchVM.No_Of_Rooms = $('#txtNoOfRoom').val();
    var rmCount = ($('#country :selected').text());

    var nor = rmCount.split(' ');
    //searchVM.RoomCount = nor[0];
    // alert(nor[0])
    
    $.localStorage("NoOfRoom", nor[0]);
    
    searchVM.Price1 = minPrice;
    searchVM.Price2 = maxPrice;
    searchVM.Rating = starValue;
    searchVM.Facilities = facilityValue;
    
    searchVM.SortBy = 'Cheapest';
    resultVM.GetResults(searchVM);
}
function FilterByLocation() {
    
    searchVM.CityMasterId = $('#hdnLocationId').val();
    searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
    searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
    //searchVM.No_Of_Rooms = $('#txtNoOfRoom').val();
    var rmCount = ($('#country :selected').text());

    var nor = rmCount.split(' ');
    //searchVM.RoomCount = nor[0];
    // alert(nor[0])
    
    $.localStorage("NoOfRoom", nor[0]);
    
    searchVM.Price1 = minPrice;
    searchVM.Price2 = maxPrice;
    searchVM.Rating = starValue;
    searchVM.Facilities = facilityValue;
    searchVM.SortBy = 'Cheapest';
    resultVM.GetResults(searchVM);
}
var searchVM = new SearchViewModel();
searchVM.CityMasterId = $.localStorage("City_Id");
searchVM.Room_Checkin = ConverToValidFormate($.localStorage("CheckInDate"));
searchVM.Room_Checkout = ConverToValidFormate($.localStorage("CheckOutDate"));
searchVM.No_Of_Rooms = NoOfRoom;

 searchVM.Price1 = minPrice;
searchVM.Price2 = maxPrice;
searchVM.Rating = starValue;
searchVM.Facilities = facilityValue;
searchVM.SortBy = 'Cheapest';


var resultVM = new SearchResultViewModel();
resultVM.GetFaility();
resultVM.GetLocationByCity();

resultVM.GetResults(searchVM);
ko.applyBindings(resultVM, document.getElementById("divSearchResult"));


function SearchResultViewModel() {
    var self = this;

    self.CityMasterId = ko.observable();
    self.Room_Checkin = ko.observable();
    self.Room_Checkout = ko.observable();
    self.No_Of_Rooms = ko.observable($.localStorage("NoOfRoom"));
    self.Price1 = ko.observable();
    self.Price2 = ko.observable();
    self.IsFound = ko.observable(true);
    self.FacilityList = ko.observableArray();
    self.TopFacilityList = ko.observableArray();
    self.FacilitySelectedValue = checkboxValues();
    if ($.localStorage("NoOfRoom") == "1")
        self.RoomSelectList = ['1 Room', '2 Rooms', '3 Rooms', '4 Rooms', '5 Rooms'];
    else if ($.localStorage("NoOfRoom") == "2")
        self.RoomSelectList = ['2 Rooms', '1 Room', '3 Rooms', '4 Rooms', '5 Rooms'];
    else if ($.localStorage("NoOfRoom") == "3")
        self.RoomSelectList = ['3 Rooms', '1 Room', '2 Rooms', '4 Rooms', '5 Rooms'];
    else if ($.localStorage("NoOfRoom") == "4")
        self.RoomSelectList = ['4 Rooms', '1 Room', '2 Rooms', '3 Rooms', '5 Rooms'];
    else if ($.localStorage("NoOfRoom") == "5")
        self.RoomSelectList = ['5 Rooms', '1 Room', '2 Rooms', '3 Rooms', '4 Rooms'];

    self.RoomCount = ko.observable();

    self.GetFaility = function () {
        //  alert('al')
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetAllFacility',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.Table.length > 10) {
                    for (var i = 0; i < 10; i++) {
                        self.TopFacilityList.push(new FacilityModel(data.Table[i]));
                    }
                    for (var i = 10; i < data.Table.length; i++) {
                        self.FacilityList.push(new FacilityModel(data.Table[i]));
                    }
                }
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        }).done(function () {

        });
    }
    self.LocationsList = ko.observableArray();
    self.GetLocationByCity = function () {
        var self = this;
        var Loc_Name = $('#txtLocation').val();




        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetLocationByCity',
            data: { name: Loc_Name },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {


                self.LocationsList.removeAll();

                if (data.Table.length > 0) {



                    for (var i = 0; i < data.Table.length; i++) {
                        self.LocationsList.push(new LocationsListModel(data.Table[i]));

                    }
                }
                //  ko.applyBindings(LocationsList, document.getElementById("locationsbyCity"));
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
    self.ModifySearch = function () {

        if ($('#formSearch').valid()) {
            SplitTheLocation($('#txtLocation').val());
            searchVM.CityMasterId = $('#hdnLocationId').val();
            searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
            searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
            //   searchVM.No_Of_Rooms = $('#txtNoOfRoom').val()
            searchVM.Price1 = minPrice;
            searchVM.Price2 = maxPrice;
            searchVM.Rating = starValue;
            searchVM.Facilities = '';
            searchVM.SortBy = 'Cheapest';

            var nor = self.RoomCount().split(' ');

            searchVM.No_Of_Rooms = nor[0];
            
            self.GetLocationByCity();
            self.GetResults(searchVM);
        }

    }

    self.HotelsList = ko.observableArray();

    self.GetResults = function (searchVM) {
        
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Post",
            url: '/api/Consumer/HotelListing_Sort',
            data: ko.toJSON(searchVM),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                self.HotelsList.removeAll();
                if (data.Table != null) {
                    $('#lblHotelCount').empty();
                    $('#lblHotelCount').append(data.Table.length);

                    self.IsFound(data.Table.length == 0 ? false : true);
                    //if (maxPrice < data.Table[0].ActualRate)
                    //{ maxPrice = data.Table[0].ActualRate; $('#slider-range').slider("values", "1", maxPrice); }                    
                    var rating = '';
                    var Numberreviews = '';
                    var ReviewUrl = '';
                    var AjaxUrl = 'http://api.tripadvisor.com/api/partner/2.0/location/4492794?key=6e6c5fb839154bf3873229158cac5af7';
                    for (var i = 0; i < data.Table.length; i++) {

                        if (data.Table[i].tripid != '' || data.Table[i].tripid != null) {

                            AjaxUrl = data.Table[i].tripid;
                            $.ajax({

                                url: AjaxUrl,
                                type: 'GET',
                                dataType: 'json',
                                async: false,
                                success: function (trip) {


                                    // data.rating_image_url
                                    rating = trip.rating_image_url;
                                    Numberreviews = trip.num_reviews + ' reviews';
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
                        }
                        else {
                            Numberreviews = '';
                            ReviewUrl = '';
                            rating = '';

                        }
                        

                        // alert(rating)
                        self.HotelsList.push(new HotelListModel(data.Table[i], data.Table[i].Roomtype, rating, Numberreviews, ReviewUrl));
                        if (data.Table[i].vndr_amnt >= data.Table[i].ActualRate) {
                            $('#' + data.Table[i].PropId).removeClass('new-price');
                            $('#' + data.Table[i].PropId).addClass('Danger');
                        }
                        else {
                            $('#' + data.Table[i].PropId).addClass('new-price');
                        }
                    }
                    //  alert(data.Table3.length)
                    //for (var i = 0; i < data.Table.length; i++) {
                    //    

                    //}
                }


                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {

        });
    }

    self.sortPrice = function () {
        var icon = $('#liSortPriceIcon').attr('class');
        if (icon == 'asc-order') {
            $('#liSortPriceIcon').addClass('desc-order').removeClass('asc-order');
            searchVM.CityMasterId = $('#hdnLocationId').val();
            searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
            searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
            var nor = self.RoomCount().split(' ');

            searchVM.No_Of_Rooms = nor[0];
            
            searchVM.Price1 = minPrice;
            searchVM.Price2 = maxPrice;
            searchVM.Rating = starValue;
            searchVM.Facilities = facilityValue;
            searchVM.SortBy = 'highest';

            self.GetResults(searchVM);
        }
        else {
            $('#liSortPriceIcon').addClass('asc-order').removeClass('desc-order');
            searchVM.CityMasterId = $('#hdnLocationId').val();
            searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
            searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
            //searchVM.No_Of_Rooms = $('#txtNoOfRoom').val();
            var nor = self.RoomCount().split(' ');

            searchVM.No_Of_Rooms = nor[0];
            
            searchVM.Price1 = minPrice;
            searchVM.Price2 = maxPrice;
            searchVM.Rating = starValue;
            searchVM.Facilities = facilityValue;
            searchVM.SortBy = 'Cheapest';
            self.GetResults(searchVM);
        }
        //self.HotelsList.sort(function (left, right) {
        //    return left.RoomPrice == right.RoomPrice ? 0 : (left.RoomPrice < right.RoomPrice ? -1 : 1);
        //})
    }
    self.sortRating = function () {
        //self.HotelsList.sort(function (left, right) {
        //    
        //    return left.StarRating() == right.StarRating() ? 0 : (left.StarRating() < right.StarRating() ? -1 : 1);
        //});
        if ($('#formSearch').valid()) {
            var icon = $('#liSortRateIcon').attr('class');
            if (icon == 'asc-order') {
                $('#liSortRateIcon').addClass('desc-order').removeClass('asc-order');

                searchVM.CityMasterId = $('#hdnLocationId').val();
                searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
                searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
                //searchVM.No_Of_Rooms = $('#txtNoOfRoom').val();
                var nor = self.RoomCount().split(' ');

                searchVM.No_Of_Rooms = nor[0];
                
                searchVM.Price1 = minPrice;
                searchVM.Price2 = maxPrice;
                searchVM.Rating = starValue;
                searchVM.Facilities = facilityValue;
                searchVM.SortBy = 'highestRating';

                self.GetResults(searchVM);
            }
            else {
                $('#liSortRateIcon').addClass('asc-order').removeClass('desc-order');

                searchVM.CityMasterId = $('#hdnLocationId').val();
                searchVM.Room_Checkin = ConverToValidFormate($('#txtCheckIn').val());
                searchVM.Room_Checkout = ConverToValidFormate($('#txtCheckOut').val());
                //searchVM.No_Of_Rooms = $('#txtNoOfRoom').val();
                var nor = self.RoomCount().split(' ');

                searchVM.No_Of_Rooms = nor[0];
                
                searchVM.Price1 = minPrice;
                searchVM.Price2 = maxPrice;
                searchVM.Rating = starValue;
                searchVM.Facilities = facilityValue;
                searchVM.SortBy = 'rating';

                self.GetResults(searchVM);
            }


        }
    }
}

var StarCount = 0;

function LocationsListModel(data) {

    var self = this;
    data = data || {};
    self.Locations = data.location_name;
    self.Active_flag1 = ko.observable(false);
    self.Selectlocation = function (eRow) {

        eRow.Active_flag1() == true ? false : true;


        $('#hdnLocationId').val($("input[name=Filterlocation]:checked").map(
                             function () { return this.value; }).get().join(","));


        if ($("input[name=Filterlocation]:checked").length > 0) {

        }
        else {
            $('#hdnLocationId').val($('#txtLocation').val())
        }



        //searchVM = new SearchResultViewModel();
        //searchVM.CityMasterId = $('#txtLocation').val();
        //searchVM.GetLocationByCity();
        //searchVM.GetResults(searchVM);

        //  FilterByLocation();

        FilterBy();
    }
}
function HotelListModel(data, NoRoomType, rating, Numberreviews, ReviewUrl) {

    var self = this;
    data = data || {};
    self.Trip_Rating = rating;
    self.Trip_Review = Numberreviews;
    self.Trip_Review_Url = ReviewUrl;
    self.Prop_Id = ko.observable(data.PropId);
    self.Prop_Image = ko.observable(data.Image_dir || '/img/prop_image/hiddengem.jpg');
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.IsHiddenGem = ko.observable(false);
    if (data.Image_dir == 'img/prop_image/hiddengem.jpg') {
        self.IsHiddenGem(true);
    }
    self.Prop_Address = ko.observable('');
    if (self.IsHiddenGem())
        self.Prop_Address(data.city + ', ' + data.state);
    else
        self.Prop_Address(data.location + ', ' + data.city + ', ' + data.state);
    self.Overview = ko.observable(data.Prop_Overview);

    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }


    //self.Room_Type = ko.observable();
    self.RoomType_Count = ko.observable(NoRoomType || '0');
    //self.RoomName = ko.observable(data.Room_Name);
    self.VendorRate = ko.observable(data.vndr_amnt);
    self.RoomPrice = ko.observable(data.ActualRate);
    self.StarRating = ko.observable(parseInt(data.Prop_Star_Rating));

    self.NoOfRoom = 1;
    self.NoOfNight = 1;
    //if (maxPrice < data.ActualRate) {
    //    maxPrice = data.ActualRate;
    //    $('#slider-range').slider("values", "1", maxPrice);
    //}

    self.BookNow = function (eRow) {
        if ($('#formSearch').valid()) {
            $.localStorage("Prop_Id", eRow.Prop_Id());
            $.localStorage("City_Id", $("#hdnLocationId").val());
            $.localStorage("Location", $("#txtLocation").val());
            $.localStorage("CheckInDate", $("#txtCheckIn").val());
            $.localStorage("CheckOutDate", $("#txtCheckOut").val());
            var rmCount = ($('#country :selected').text());

            var nor = rmCount.split(' ');
            //searchVM.RoomCount = nor[0];
            // alert(nor[0])
            
            $.localStorage("NoOfRoom", nor[0]);

            window.location.href = '/hotel_details';
        }
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

//var dateIn = new Date(data.Checkin).toDateString();

function FacilityModel(data) {
    var self = this;
    data = data || {};
    self.Facility_Id = ko.observable(data.Facility_Id);
    self.Facility_Name = ko.observable(data.Facility_Name);
    self.Image_dir = ko.observable(data.Facility_Image_dir);
    self.Active_flag = ko.observable(false);

    self.Select = function (eRow) {
        eRow.Active_flag() == true ? false : true;

        facilityValue = $("input[name=checkboxFacility]:checked").map(
                             function () { return this.value; }).get().join(",");
        FilterBy();
    }

}

$('input[name="checkboxFacility"]:checked')
function checkboxValues() {
    var allVals = [];
    $('input[name="checkboxFacility"]:checked').each(function () {
        allVals.push($(this).val());
    });
    return allVals; // process the array as you wish in the function so it returns what you need serverside
}


function showSelectedValues() {
    alert($("input[name=checkboxFacility]:checked").map(
       function () { return this.value; }).get().join(","));
}

function ConverToValidFormate(formateDate) {
    var date = formateDate.split('/');
    shortDate = date[2] + "/" + date[1] + "/" + date[0];

    return shortDate;
}

function SplitTheLocation(location) {
    var data = location.split(',');
    $('#lblCity').empty();
    $('#lblCity').append(data[1]);

    $('#strongCity').empty();
    $('#strongCity').append(location);
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

$('#btnMoreFacility').click(function () {
    $('#btnMoreFacility').hide();
    $('#moreFacilityList').show();
    $('#btnhideMoreFacility').show();
});
$('#btnhideMoreFacility').click(function () {
    $('#btnMoreFacility').show();
    $('#moreFacilityList').hide();
    $('#btnhideMoreFacility').hide();
});

//$(function () {
//    $('input:text').click(function () {
//        var txtval = $(this).val();
//        $(this).focus(function () {
//            $(this).val('')
//        });
//        $(this).blur(function () {
//            if ($(this).val() == "") {
//                $(this).val(txtval);
//            }
//        });
//    });
//});

