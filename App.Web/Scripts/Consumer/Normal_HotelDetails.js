$("#txtLocation").autocomplete({
    source: function (request, response) {
        $('#txtLocation').addClass('loadinggif');
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
            $('#txtLocation').removeClass('loadinggif');
        }
    },
    select: function (e, i) {
        $("#txtLocation").val(i.item.label);
        $("#hdnLocationId").val(i.item.val);
    },
    minLength: 2
});

$('#hdnLocationId').val($.localStorage("City_Id"));
$('#txtLocation').val($.localStorage("Location"));
$('#txtCheckIn').val($.localStorage("CheckInDate"));
$('#txtCheckOut').val($.localStorage("CheckOutDate"));
$('#txtNoOfRoom').val($.localStorage("NoOfRoom"));
SplitTheLocation($.localStorage("Location"));

var Prop_Id = $.localStorage('Prop_Id');

var NoOfNight = $.localStorage("NoOfNight");
var NoOfRoom = $.localStorage("NoOfRoom");

function showDays() {
    var start = $('#txtCheckIn').datepicker('getDate');
    var end = $('#txtCheckOut').datepicker('getDate');
    if (!start || !end) return;
    var days = (end - start) / (1000 * 60 * 60 * 24);
    $('#txtDayCount').val(days + " Night");
    NoOfNight = days;
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
        $('#txtDayCount').val(days + " Night");
        NoOfNight = days;
    }

});
$("#txtCheckOut").datepicker({
    minDate: 1,
    maxDate: '+3D',
    dateFormat: 'dd/mm/yy',
    onSelect: showDays
});

if ($.localStorage("NoOfRoom") == "1")
    var RoomSelectList = ['1 Room', '2 Rooms', '3 Rooms', '4 Rooms', '5 Rooms'];
else if ($.localStorage("NoOfRoom") == "2")
    var RoomSelectList = ['2 Rooms', '1 Room', '3 Rooms', '4 Rooms', '5 Rooms'];
else if ($.localStorage("NoOfRoom") == "3")
    var RoomSelectList = ['3 Rooms', '1 Room', '2 Rooms', '4 Rooms', '5 Rooms'];
else if ($.localStorage("NoOfRoom") == "4")
    var RoomSelectList = ['4 Rooms', '1 Room', '2 Rooms', '3 Rooms', '5 Rooms'];
else if ($.localStorage("NoOfRoom") == "5")
    var RoomSelectList = ['5 Rooms', '1 Room', '2 Rooms', '3 Rooms', '4 Rooms'];
var RoomCount = ko.observable();
function ModifySearchSubmit() {

    $('#formSearch').submit();
    if ($('#formSearch').valid()) {
        SplitTheLocation($('#txtLocation').val());

        $.localStorage("City_Id", $("#hdnLocationId").val());
        $.localStorage("Location", $("#txtLocation").val());
        $.localStorage("CheckInDate", $("#txtCheckIn").val());
        $.localStorage("CheckOutDate", $("#txtCheckOut").val());
        
        var rmCount = ($('#country :selected').text());


        var nor = rmCount.split(' ');
        //searchVM.RoomCount = nor[0];
        // alert(nor[0])
        $.localStorage("NoOfRoom", nor[0]);

        window.location.href = '/Search_Results'

    }
}

var hotelVM = new HotelDetailsViewModel();
hotelVM.GetHotelCompleteDetails();
ko.applyBindings(hotelVM, document.getElementById("divCompleteHotelDetails"));

function HotelDetailsViewModel() {
    debugger;
    var self = this;
    var gp1 = "";
    var gp2 = "";
    self.Property_MapSrc = ko.observable();
    self.Hotel_Overview = ko.observable('');
    self.HotelDetailsList = ko.observableArray();
    self.IsHidden = ko.observable(false);
    self.RoomDetailsList = ko.observableArray();
    self.RoomPolicyList = ko.observableArray();
    self.RoomDTList = ko.observableArray();
    self.RoomDTListother = ko.observableArray();
    self.RoomDTListImage = ko.observableArray();
    self.HotelPoliciesList = ko.observableArray();

    self.GetHotelCompleteDetails = function () {
        debugger;
        AppCommonScript.ShowWaitBlock();
        var searchVM = new SearchViewModel();
        searchVM.PropId = $.localStorage('Prop_Id');
        searchVM.Room_Checkin = ConverToValidFormate($.localStorage("CheckInDate"));
        searchVM.Room_Checkout = ConverToValidFormate($.localStorage("CheckOutDate"));
        searchVM.No_Of_Rooms = $.localStorage("NoOfRoom");
        $.ajax({
            type: "Post",
            url: '/api/Consumer/PropertyDetails',
            data: ko.toJSON(searchVM),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                
                
                self.HotelDetailsList.removeAll();
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
                            Numberreviews = trip.num_reviews + ' reviews';
                            ReviewUrl = trip.reviews;


                            
                            // ReviewUrl =  trip.web_url;
                            //   self.HotelsList.push(new HotelListModel(data.Table[i], data.Table2[i].Roomtype, trip.rating_image_url))
                            if (data.Table1[0].Image_dir == "img/prop_image/hiddengem.jpg") {
                                $('#ifhidden').hide();
                                Numberreviews = '';

                            }
                            // WriteResponse(data);
                        },
                        error: function (x, y, z) {
                            rating = '';
                            Numberreviews = '';
                            ReviewUrl = '';

                            //  alert(x + '\n' + y + '\n' + z);
                        }
                    });
                    
                    self.HotelDetailsList.push(new HotelDetailsModel(data.Table[i], data.Table1, data.Table3, rating, Numberreviews, ReviewUrl)); // (data, dataImage, dataFaci)
                    
                    if (data.Table[i].vndr_amnt >= data.Table[i].ActualRate) {
                        $('#' + data.Table[i].Prop_Id).removeClass('new-price');
                        $('#' + data.Table[i].Prop_Id).css('text-decoration', 'line-through');
                        var a = data.Table[i].Prop_Id;
                    }
                    else {
                        $('#' + data.Table[i].Prop_Id).addClass('new-price');
                    }
                }

                self.RoomDetailsList.removeAll();
                for (var i = 0; i < data.Table4.length; i++) {
                    
                    self.RoomDetailsList.push(new RoomDetailsModel(data.Table4[i], data.Table3, data.Table5));
                }
                self.HotelPoliciesList.removeAll();
                for (var i = 0; i < data.Table2.length; i++) {
                    self.HotelPoliciesList.push(new PolicyModel(data.Table2[i]));
                }
                if (data.Table.length > 0)
                    self.GetMapSrc(data.Table[0].Prop_GPS_Pos);

            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        }).done(function () {
            AppCommonScript.HideWaitBlock();
            // ko.applyBindings(hotelVM, document.getElementById("divHotelDetails"));
            //ko.applyBindings(hotelVM, document.getElementById("divRoomDetailsList"));
            //ko.applyBindings(hotelVM, document.getElementById("divHotelPoliciesList"));
            //ko.applyBindings(hotelVM, document.getElementById("divHotelOverView"));
        });
    }

    self.GetRoomListByPropId = function (Prop_Id) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetBookedDeals',
            data: { Cons_Id: Prop_Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                self.RoomDetailsList.removeAll();
                for (var i = 0; i < data.length; i++) {
                    self.RoomDetailsList.push(new RoomDetailsModel(data));
                }

            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        }).done(function () {
            AppCommonScript.HideWaitBlock();
            ko.applyBindings(hotelVM, document.getElementById("divRoomDetails"));
        });
    }

    self.GetRoomPolicyById = function (Prop_Id, Room_Id) {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetRoomPolicy',
            data: { Prop_Id: Prop_Id, Room_Id: Room_Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                self.RoomPolicyList.removeAll();

                for (var i = 0; i < data.Table.length; i++) {
                    self.RoomPolicyList.push(new RoomPolicyModel(data.Table[i]));
                }
                AppCommonScript.HideWaitBlock();
                $('#modelPolicy').modal('show');
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }

    self.GetRoomDetailsId = function (Prop_Id, Room_Id) {
        
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetRoomDetailsByID',
            data: { Prop_Id: Prop_Id, Room_Id: Room_Id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                
                self.RoomDTList.removeAll();
                self.RoomDTListother.removeAll();
                self.RoomDTListImage.removeAll();
                //if (data.Table3.length <= 0)
                //    $('#rmf').hide();
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
                if (data.Table2.length <= 0)
                    $('#rmg').hide();
                for (var i = 0; i < data.Table2.length; i++) {
                    self.RoomDTListImage.push(new RoomDTmodelImage(data.Table2[i]));
                }
                AppCommonScript.HideWaitBlock();
                $('#roomdt').modal('show');
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }

    self.GetMapSrc = function (gpsData) {
        if (gpsData != '') {
            var gps = gpsData.split(',');
            gps[0] = gps[0].replace('(', '');
            gps[1] = gps[1].replace(')', '');
            gp1 = gps[0];
            gp2 = gps[1];
            InitializeMap1(gps[0], gps[1]);
        } else
            return;
        // var srcIfram = 'https://maps.google.com/maps?q=' + gps[0] + ',' + gps[1] + '&hl=es;z=14&amp;output=embed';
        //  $('#iframe').attr('src', srcIfram)
        //self.Property_MapSrc = 'https://maps.google.com/maps?q=' + gps[0] + ',' + gps[1] + '&hl=es;z=14&amp;output=embed';
    }
    self.GetMapSrcclick = function (gpsData) {
        if (gpsData != '') {
            //var gps = gpsData.split(',');
            //gps[0] = gps[0].replace('(', '');
            //gps[1] = gps[1].replace(')', '');

            InitializeMap1(gp1, gp2);
        } else
            return;
        // var srcIfram = 'https://maps.google.com/maps?q=' + gps[0] + ',' + gps[1] + '&hl=es;z=14&amp;output=embed';
        //  $('#iframe').attr('src', srcIfram)
        //self.Property_MapSrc = 'https://maps.google.com/maps?q=' + gps[0] + ',' + gps[1] + '&hl=es;z=14&amp;output=embed';
    }
}


var StarCount = 0;
function HotelDetailsModel(data, dataImage, dataFaci, rating, Numberreviews, ReviewUrl) {

    var self = this;
    data = data || {};
    dataImage = dataImage || {};
    dataFaci = dataFaci || {};
    self.Trip_Rating = rating;
    self.Trip_Review = Numberreviews;

    
    
    self.Trip_Review_Url = ko.observableArray(ReviewUrl);
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Room_Checkin = ko.observable(data.Room_Checkin);
    self.Room_Checkout = ko.observable(data.Room_Checkout);
    self.HotelImageList = ko.observableArray();
    for (var i = 0; i < dataImage.length; i++) {
        self.HotelImageList.push(new HotelImageModel(dataImage[i]));
    }

    self.Prop_Image = ko.observable(dataImage[0].Image_dir || 'img/prop_image/hiddengem.jpg');
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.IsHiddenGem = ko.observable(true);
    if (dataImage[0].Image_dir == "img/prop_image/hiddengem.jpg")
    { self.IsHiddenGem(false); hotelVM.IsHidden(true); }
    self.Prop_Address = ko.observable('');
    if (!self.IsHiddenGem())
        self.Prop_Address(data.City + ', ' + data.State);
    else
        self.Prop_Address(data.Prop_Addr1 + ', ' + data.location + ', ' + data.City);
    self.PhoneNumber = ko.observable(data.Prop_Addr1 || '');
    self.Prop_GPS_Pos = ko.observable(data.Prop_GPS_Pos || '');

    hotelVM.Hotel_Overview(data.Prop_Overview || '');

    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }
    self.FacilShow_activeFlag = ko.observable(true);
    var j = 3;
    if (dataFaci.length >= 0 && dataFaci.length < 3) {
        j = dataFaci.length;
    }
    if (j <= 1) {

        self.FacilShow_activeFlag = ko.observable(false);

    }
    self.HotelFacilityListtop = ko.observableArray();

    for (var i = 0; i < j; i++) {
        self.HotelFacilityListtop.push(new FacilityModel(dataFaci[i]));
    }

    self.HotelFacilityList = ko.observableArray();
    for (var i = 0; i < dataFaci.length; i++) {
        self.HotelFacilityList.push(new FacilityModel(dataFaci[i]));
    }
    
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.VendorRate = ko.observable(data.vndr_amnt);
    self.Price = ko.observable(data.ActualRate);
    
    if (data.vndr_amnt >= data.ActualRate) {
        $('#' + data.Prop_Id).removeClass('new-price');
        $('#' + data.Prop_Id).css('text-decoration', 'line-through');
        var a = data.Prop_Id;
    }
    else {
        $('#' + data.Prop_Id).addClass('new-price');
    }
    self.ViewRoomDetails = function () {
        //  alert('Hi')
    }
}

function RoomDetailsModel(data, dataFaci, dataPolicy) {
    
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.prop_Id);
    self.Room_Id = ko.observable(data.Room_Id);
    self.RoomName = ko.observable(data.Room_Name);
    self.Room_Image = ko.observable(data.image_dir || '');
    self.Room_Overview = ko.observable(data.Room_Overview);
    self.VendorRate = ko.observable(data.vendor);
    self.RoomPrice = ko.observable(data.camflg_Amnt);


    self.RoomFacilShow_activeFlag = ko.observable(true);
    var j = 3;
    if (dataFaci.length >= 0 && dataFaci.length < 3) {
        j = dataFaci.length;
    }
    if (j <= 1) {

        self.RoomFacilShow_activeFlag = ko.observable(false);

    }
    self.RoomFacilityListtop = ko.observableArray();

    for (var i = 0; i < j; i++) {
        self.RoomFacilityListtop.push(new FacilityModel(dataFaci[i]));
    }

    self.RoomFacilityList = ko.observableArray();
    
    if (dataFaci.length == 0) {
        $('#More').hide()
    }
    else {
        for (var i = 0; i < dataFaci.length; i++) {
            self.RoomFacilityList.push(new FacilityModel(dataFaci[i]));
        }
    }
    self.inclusion = ko.observable(data.inclusion || '');
    self.others = ko.observable(data.others || '');
    self.RoomsLeft = ko.observable(data.Inventory_Real_Bal_Rooms + ' Room Left');
    self.limitRoom = ko.observable(data.Inventory_Real_Bal_Rooms <= 2 ? 'limit-rooms' : '');

    //self.RoomPolicyList = ko.observableArray();
    self.PolicyShow_activeFlag = ko.observable(false);
    for (var i = 0; i < dataPolicy.length; i++) {
        if (dataPolicy[i].room_id == data.Room_Id)
            self.PolicyShow_activeFlag(true);
    }

    self.BookNow = function (eRow) {

        debugger;
        $.localStorage("Prop_Id", eRow.Prop_Id());
        $.localStorage("Room_Id", eRow.Room_Id());
        $.localStorage("CheckInDate", $("#txtCheckIn").val());
        $.localStorage("CheckOutDate", $("#txtCheckOut").val());

        var rmCount = ($('#country :selected').text());
        var nor = rmCount.split(' ');
        //searchVM.RoomCount = nor[0];
        // alert(nor[0])
        //Changed the NoOfRooms value Chandan 11-02-2020
        $.localStorage("NoOfRoom", $('#txtNoOfRoom').val());

        $.localStorage("DayCount", $("#txtDayCount").val());
        window.location.href = '/Booking_payment_method'
    }

    self.RoomPolicies = function (eRow) {
        hotelVM.GetRoomPolicyById(eRow.Prop_Id(), eRow.Room_Id());
    }

    self.RoomDetails = function (eRow) {

        hotelVM.GetRoomDetailsId(eRow.Prop_Id(), eRow.Room_Id());
    }
}

function PolicyModel(data) {
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Policy_Name = ko.observable(data.Policy_Name);
    self.Policy_Descr = ko.observable(data.Policy_Descr);
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
function RoomDTmodelImage(data) {
    
    var self = this;
    data = data || {};

    self.Image_dir = ko.observable(data.Image_dir);

}
function HotelImageModel(data) {
    var self = this;
    data = data || {};
    self.Image_Id = ko.observable(data.Image_Id);
    self.Image_dir = ko.observable(data.Image_dir);
}

function FacilityModel(data) {
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
function RoomPolicyModel(data) {
    var self = this;
    data = data || {};
    self.Policy_Name = ko.observable(data.Policy_Name);
    self.Policy_Descr = ko.observable(data.Policy_Descr);
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

    var data = location

    $('#strongCity').empty();
    $('#strongCity').append(data);
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


function InitializeMap(lat, lon) {

    var myLatlng = new google.maps.LatLng(lat, lon) // This is used to center the map to show our markers
    var mapOptions = {
        center: myLatlng,
        zoom: 15,
        draggable: false,
        scrollwheel: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        marker: true,
        //mapTypeControl: false
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);


    if (hotelVM.IsHidden() != true) {
        var marker = new google.maps.Marker({
            position: myLatlng
        });
        marker.setMap(map);
    }
    else {

        // Add circle overlay and bind to marker
        var circle = new google.maps.Circle({
            map: map,
            center: myLatlng,
            radius: 300,
            fillColor: '#61c419',
            fillOpacity: .6,
            strokeColor: '#313131',
            strokeOpacity: .4,
            strokeWeight: .8
        });
        circle.setMap(map);
    }
}

function showfaicility() {
    $('#modelfacility').modal('show');
}
function showroomfaicility() {
    $('#modelroomfacility').modal('show');
}
function mapclick() {
    $('#modelmap').modal('show');
}
function roomdt() {
    $('#roomdt').modal('show');
}
function InitializeMap1(lat, lon) {
    var myLatlng = new google.maps.LatLng(lat, lon) // This is used to center the map to show our markers
    var mapOptions = {
        center: myLatlng,
        zoom: 15,
        draggable: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        marker: true,
        //mapTypeControl: false
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);


    if (hotelVM.IsHidden() != true) {
        var marker = new google.maps.Marker({
            position: myLatlng
        });
        marker.setMap(map);
    }
    else {

        // Add circle overlay and bind to marker
        var circle = new google.maps.Circle({
            map: map,
            center: myLatlng,
            radius: 300,
            fillColor: '#61c419',
            fillOpacity: .6,
            strokeColor: '#313131',
            strokeOpacity: .4,
            strokeWeight: .8
        });
        circle.setMap(map);
    }
}


$(document).ready(function () {

    $('#TripReview').click(function () {

        $('#TripReviewModal').modal('show');
    })

})