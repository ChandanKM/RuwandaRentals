var SubVM = new SubscribeViewModel();
$(document).ready(function () {

    ko.applyBindings(SubVM);
});

function SubscribeViewModel() {
    var self = this;

    self.Email = ko.observable();
    self.Ipaddress = ko.observable();
    $.getJSON("http://jsonip.com?callback=?", function (data) {
        self.Ipaddress = data.ip;
    });

}

function SubmitSub(subData) {
    AppCommonScript.ShowWaitBlock();
    $.ajax({
        type: "POST",
        url: "/api/Subscribe/create",
        data: $.parseJSON(ko.toJSON(subData)),
        dataType: "json",
        success: function (response) {
            Successed(response)
            SubVM.Email('')
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));

        }
    });
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}




