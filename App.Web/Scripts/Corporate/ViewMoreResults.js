
var moreVM = new ViewMoreViewModel();
ko.applyBindings(moreVM, document.getElementById('divMoreResult'));

var Is = $.localStorage("ViewMore");
if (Is == 'Gems') {
    moreVM.GetHiddenGems();
}
else if (Is == 'Best') {
    moreVM.GetBestOffers();
}
else {
    moreVM.GetRecommendedHotels();
}
function ViewMoreViewModel() {
    var self = this;
    self.IsFound = ko.observable(true);
    self.HiddenGemsList = ko.observableArray();
    self.RecommendedHotelsList = ko.observableArray();
    self.BestOffersList = ko.observableArray();
    self.HotelsList = ko.observableArray();

    self.GetHiddenGems = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetHiddenGems',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.HotelsList.removeAll();
                if (data.Table != null) {
                    $('#lblViewType').empty();
                    $('#strongCity').append('Hidden Gems');
                    $('#lblViewType').append('Hidden Gems For You');
                    $('#lblHotelCount').empty();
                    $('#lblHotelCount').append(data.Table.length);
                    self.IsFound(data.Table.length == 0 ? false : true);
                    if (data.Table.length > 0) {
                        for (var i = 0; i < data.Table.length ; i++) {
                            self.HotelsList.push(new GemsHotelModel(data.Table[i]));
                        }
                    }
                }
                AppCommonScript.HideWaitBlock();
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
            url: '/api/Consumer/GetRecommendedHotels',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.HotelsList.removeAll();
                if (data.Table != null) {
                    $('#lblViewType').empty();
                    $('#strongCity').append('Recommended Hotels');
                    $('#lblViewType').append('Recommended Hotels For You');
                    $('#lblHotelCount').empty();
                    $('#lblHotelCount').append(data.Table.length);
                    self.IsFound(data.Table.length == 0 ? false : true);
                    if (data.Table.length > 0) {
                        for (var i = 0; i < data.Table.length; i++) {
                            self.HotelsList.push(new NormalHotelModel(data.Table[i]));
                        }
                    }
                }
                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
    self.GetBestOffers = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "Get",
            url: '/api/Consumer/GetBestOffers',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.HotelsList.removeAll();
                if (data.Table != null) {
                    $('#lblViewType').empty();
                    $('#strongCity').append('Best Offers');
                    $('#lblViewType').append('Best Offers For You');
                    $('#lblHotelCount').empty();
                    $('#lblHotelCount').append(data.Table.length);
                    self.IsFound(data.Table.length == 0 ? false : true);
                    if (data.Table.length > 0) {
                        for (var i = 0; i < data.Table.length; i++) {
                            self.HotelsList.push(new NormalHotelModel(data.Table[i]));
                        }
                    }
                }
                AppCommonScript.HideWaitBlock();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
}

var StarCount = 0;
function GemsHotelModel(data) {
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Prop_Name = ko.observable(data.Vparam_Descr);
    self.Prop_OverView = ko.observable(data.Prop_Overview);
    self.Location = ko.observable(data.city + ', ' + data.state);
    self.StarRate = ko.observable(data.Prop_Star_Rating);
    self.Image_Url = ko.observable('/img/prop_image/hiddengem.jpg');
    self.CityId = ko.observable(data.city_id);
    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }
    self.BookNow = function (eRow) {
        $.localStorage("Prop_Id", eRow.Prop_Id());
        $.localStorage("City_Id", self.CityId());
        $.localStorage("Location", self.Location());
        $.localStorage("CheckInDate", GetDate(0));
        $.localStorage("CheckOutDate", GetDate(1));
        $.localStorage("NoOfRoom", "1");

        window.location.href = '/hotel_details';
    }
}

function NormalHotelModel(data) {
    
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.Prop_Id);
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.Prop_OverView = ko.observable(data.Prop_Overview);
   
    self.StarRate = ko.observable(data.Prop_Star_Rating);
    if (data.Image_dir == "img/prop_image/hiddengem.jpg") {
        data.Image_dir = '/img/prop_image/hiddengem.jpg'
        self.Location = ko.observable(data.city + ', ' + data.state);
        self.Image_Url = ko.observable(data.Image_dir);
    }
    else{
        self.Location = ko.observable(data.location + ', ' + data.city + ', ' + data.state);
        self.Image_Url = ko.observable(data.Image_dir);
    }
    self.CityId = ko.observable(data.city_id);
    self.StarRatingList = ko.observableArray();
    StarCount = parseInt(data.Prop_Star_Rating);
    for (var i = 0; i < 5; i++) {
        self.StarRatingList.push(new RatingModel(StarCount--));
    }
    self.BookNow = function (eRow) {
        $.localStorage("Prop_Id", eRow.Prop_Id());
        $.localStorage("City_Id", self.CityId());
        $.localStorage("Location", self.Location());
        $.localStorage("CheckInDate", GetDate(0));
        $.localStorage("CheckOutDate", GetDate(1));
        $.localStorage("NoOfRoom", "1");

        window.location.href = '/hotel_details';
    }
}

function HotelListModel(data, NoRoomType) {
    var self = this;
    data = data || {};
    self.Prop_Id = ko.observable(data.PropId);
    self.Prop_Image = ko.observable(data.Image_dir || '/img/prop_image/hiddengem.jpg');
    self.Prop_Name = ko.observable(data.Prop_Name);
    self.IsHiddenGem = ko.observable(false);
    if (!data.Image_dir) {
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

    self.BookNow = function (eRow) {

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

function GetDate(a) {

    var fullDate = new Date();
    var twoDigitMonth = fullDate.getMonth() + 1 + "";
    if (twoDigitMonth.length == 1)
        twoDigitMonth = "0" + twoDigitMonth;
    var twoDigitDate = fullDate.getDate() + a + "";
    if (twoDigitDate.length == 1)
        twoDigitDate = "0" + twoDigitDate;
    return currentDate = twoDigitDate + "/" + twoDigitMonth + "/" + fullDate.getFullYear();
    // return currentDate = fullDate.getFullYear() + "/" + twoDigitMonth + "/" + twoDigitDate;
};