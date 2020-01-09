
var PromotionVM  = function () {
    Prop = this;
    Prop.getpromo = function () 
    {
        var self = this;
        self.PromoList = ko.observableArray([]),
      
        $.ajax({
            type: "GET",
            url: '/api/Promotion/Bind',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.Table.length; i++) {
                    self.PromoList.push(data.Table[i]);
                }
                ko.applyBindings(AllPromostions, document.getElementById("PropertiesDT"));

                $(".PropertiesDT").DataTable({ responsive: true, 'iDisplayLength': 15 });
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};

self.suspendPromo = function (promotion) {
    if (confirm('Are you sure to suspend this promotion ?')) {
        $.ajax({
            type: "POST",
            url: '/api/Promotion/Suspend',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(promotion),
            success: function (response) {
                //location.reload();
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    }
};

$(document).ready(function () {

    AllPromostions = new PromotionVM();

    AllPromostions.getpromo();

});