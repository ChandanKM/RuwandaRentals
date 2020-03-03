var urlPath = window.location.pathname;
var UserID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);

var systemprofileVM = new SystemProfileViewModel();
systemprofileVM.GetProfile(UserID);

function SystemProfileModel(data, datafdf) {
    var self = this;
    //self.AllLoc = GetAllLocation();
    self.Id = ko.observable(data.Id);
    self.Company_Title = ko.observable(data.Company_Title);
    self.Owned_By = ko.observable(data.Owned_By);
    self.CIN_Number = ko.observable(data.CIN_Number);
    self.Adr1 = ko.observable(data.Adr1);
    self.Adr2 = ko.observable(data.Adr2);
    self.Location = ko.observable(data.Location);
    self.City = ko.observable(data.City);
    self.Tin_id = ko.observable(data.Tin_id);
    self.Phone = ko.observable(data.Phone);
    self.Mobile = ko.observable(data.Mobile);
    self.Email = ko.observable(data.Email);
    self.Sms_Url = ko.observable(data.Sms_Url);
    self.User_Id = ko.observable(data.User_Id);
    //self.SetupEmail = ko.observable(datafdf.email);
    //self.Password = ko.observable(datafdf.pswd);
    //self.SMTP = ko.observable(datafdf.smtp);
    
    self.Update = function (data) {
        $.ajax({
            type: "POST",
            url: "/api/SystemProfile/update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            cache: false,
            success: function (recorde) {
                location.reload();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
    self.Cancel = function () {
        window.location.href = '';
    }
}


function SystemProfileViewModel() {
    var self = this;
    self.ProfileData = ko.observableArray();

    self.GetProfile = function (Id) {
        var obj = new Model();
        obj.Id = Id;
        $.ajax({
            type: "POST",
            url: "/api/SystemProfile/GetbyId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(obj),
            success: function (data) {
                
              
                self.ProfileData.push(new SystemProfileModel(data.Table[0], data.Table1[0]));
      
               
            },
            error: function (err) {
                alert(err.responseText);
            }
        }).done(function () {
            ko.applyBindings(systemprofileVM);
        });
    }
}

function Model() {
    this.Id = ko.observable('');
}

//function GetAllLocation() {
//    var allcities = ko.observableArray([{ Id: '0', CityName: '--Select--' }]);

//    $.getJSON("/api/property/allcities", function (result) {
//        ko.utils.arrayMap(result, function (item) {
//            allcities.push({ Id: item.City_Id, CityName: item.CityName });
//        });
//    });
//    return allcities;
//}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}