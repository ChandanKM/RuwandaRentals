
function PromotionViewModel() {
    var self = this;
    self.Promo_Id = ko.observable('1');
    self.Promo_Code = ko.observable();
    self.Promo_descr = ko.observable();
    self.Promo_Type = ko.observable();
    self.Prop_Value = ko.observable();
    self.Promo_Start = ko.observable();
    self.Promo_End = ko.observable();
    self.Promo_Active_flag = ko.observable('true');
}


function CreatePromotion(promotion) {
    
    $.ajax({
        type: "POST",
        url: "/api/Promotion/create",
        data: $.parseJSON(ko.toJSON(promotion)),
        dataType: "json",
        success: function (response) {
            alert('Work');
        },
        error: function (err) {
            alert(err.status + " : " + err.statusText);
        }
    });
}

$(document).ready(function () {
    var promotionVM = new PromotionViewModel();
    ko.applyBindings(promotionVM, document.getElementById("divCreatePromotion"));
});