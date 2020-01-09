
var LoyaltyVM = function () {
    Prop = this;
    Prop.getLoyalty = function () {
        var self = this;
        self.LoyaltyList = ko.observableArray([]),

        $.ajax({
            type: "GET",
            url: '/api/Loyalty/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.Table.length; i++) {
                    var obj = {};
                    obj.Loyal_Id = data.Table[i].Loyal_Id;
                    obj.Loyal_Desc = data.Table[i].Loyal_Desc;
                    obj.Loyal_Max_Allowed = data.Table[i].Loyal_Max_Allowed;
                    var value = new Date(parseInt(data.Table[i].Loyal_Start_On.substr(6)));
                    obj.Loyal_Start_On = value.getMonth()+'/'+value.getDate()+'/'+value.getFullYear();
                    obj.Loyal_End_On = data.Table[i].Loyal_End_On;
                    self.LoyaltyList.push(obj);
                }
                ko.applyBindings(AllLoyalty, document.getElementById("divLoyaltyGrid"));

                $(".divLoyaltyGrid").DataTable({ responsive: true, 'iDisplayLength': 15 });
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};

self.suspendLoyalty = function (loyalty) {
    if (confirm('Are you sure to suspend this loyalty ?')) {
        $.ajax({
            type: "POST",
            url: '/api/Loyalty/Suspend',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(loyalty),
            success: function (response) {
                alert('Suspended Succesfully');
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};



$(document).ready(function () {

    AllLoyalty= new LoyaltyVM();

    AllLoyalty.getLoyalty();

});