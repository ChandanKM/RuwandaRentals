var urlPath = window.location.pathname;
var UserID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
var PropID = $.cookie("Property_Id");
var VendorId = $.cookie("VendId")
$(function () {

   
    PoliciesListVM.getPolicy();
    ko.applyBindings(PoliciesListVM, document.getElementById('policdiv'));
});



//View Model
var PoliciesListVM = {
   
    Policies: ko.observableArray([]),

    getPolicy: function () {
        debugger;
        var self = this;
        $.ajax({

            type: "GET",
            url: '/api/rooms/BindPolicy',
                    data: { Id: UserID },
                    //  data: { Id: array[3] },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        for (var i = 0; i < data.length; i++) {
                            PoliciesListVM.Policies.push(new PolicyClass(data[i])); //Put the response in ObservableArray
                        }

                    },
                    error: function (err) {
                        alert(err.status + " : " + err.statusText);
            }
        });
    },
};




updatepolicy = function () {
    
    AppCommonScript.ShowWaitBlock();
    $.ajax({
        url: '/api/rooms/Updatepolicy',
        type: 'post',
        dataType: 'json',
        data: ko.toJSON(this),
           
        contentType: 'application/json',
        success: function (result) {
            AppCommonScript.HideWaitBlock();
            AppCommonScript.showNotify(result);
            
        },
        error: function (err) {
            if (err.responseText == "Creation Failed")
            {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(err);
            }
            else {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(err);
                   
            }
        },
        complete: function () {
               
        }

    });
    //window.location = 'Rooms/Bind'
    //$(document).ready(function () {
    //    setTimeout(function () { window.location = 'Bind' }, 8000);
    //});
        
};


//Model
function PolicyClass(data) {
    var policy = this;

    policy.Policy_Id = data["Policy_Id"];
    policy.Policy_Name = ko.observable(data.Policy_Name);
    policy.Policy_Descr = ko.observable(data.Policy_Descr);


   
}
