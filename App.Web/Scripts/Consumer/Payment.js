var paymentVM = new PaymentViewModel();
paymentVM.MakePayment();
ko.applyBindings(paymentVM, document.getElementById('nonseamless'));
var urlPath;
var Invce_Nums;
var DataArray;
var InvoiceArray;
var AmountArray;
var Amount;

function PaymentViewModel() {
    var self = this;
    self.PaymentCodes = ko.observable(),
    urlPath = document.URL;

    Invce_Nums = urlPath.substring(urlPath.lastIndexOf("Invce_Num=") + 0, urlPath.length);
    DataArray =Invce_Nums.split('&','2')
    InvoiceArray = DataArray[0].split('=', '2');
    AmountArray = DataArray[1].split('=', '2');
    Invce_Nums = InvoiceArray[1].toString();
    Amount =parseFloat(AmountArray[1]);//parseFloat(Amount)+parseFloat(flt);
    self.MakePayment = function () {
        $.ajax({
            type: "GET",
            url: "/api/consumer/CCAvenue?Invce_Num=" + Invce_Nums + "&Amount=" + Amount,
            //data: { Invce_Num: Invce_Nums, Amount: Amount },
            dataType: "json",
            success: function (data) {
                self.PaymentCodes(data);

            },
        }).done(function () {
            $("#nonseamless").submit();
        });
    }
}


function PmntClass(data) {
    var pmnt = this;
    pmnt.EncodeRequest = $.data
}
