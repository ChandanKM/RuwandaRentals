var urlPath = window.location.pathname;


//var Consumer_Id = $.localStorage("Consumer_Id");
$(document).ready(function () {


     AppCommonScript.ShowWaitBlock();
     Consumer_Id = $.localStorage("Cons_Id");
     Invc_Num = $.localStorage("Invce_Num");
     bookingInvoiceVM = new BookingInvoiceViewModel();
    bookingInvoiceVM.GetBookingInvoice(Invc_Num, Consumer_Id);

})




function BookingInvoiceViewModel() {
    var self = this;
    self.Trans = ko.observableArray([]),
    self.TaxTrans = ko.observableArray([]),
    self.FacilityTrans = ko.observableArray([]),
    self.ImageTrans = ko.observableArray([]),
    self.PolicyTrans = ko.observableArray([]),
    self.FinePrintTrans = ko.observableArray([]),
    self.smsgateway = ko.observableArray([]),
    //new modified
    self.Days_Count = ko.observable();
    self.Prop_Name = ko.observable();
    self.Prop_Address = ko.observable();
    self.CIN = ko.observable();
    self.Cons_Name = ko.observable();
    self.Room_Name = ko.observable();
    self.Invce_Num = ko.observable();
    self.Image_Url = ko.observable();
    self.Checkin = ko.observable();
    self.Checkout = ko.observable();
    self.GuestName = ko.observable();
    self.Email = ko.observable();
    self.Mobile = ko.observable();
    self.Camo_Rate = ko.observable();
    self.Total = ko.observable();
    self.TaxType = ko.observable();
    self.TaxAmnt = ko.observable();

    self.GetBookingInvoice = function (Invc_Num, Cons_Id) {

        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetBookingInvoice',
            contentType: "application/json; charset=utf-8",
            data: { Invce_Num: Invc_Num, Cons_Id: Cons_Id },
            async: false,
            dataType: "json",
            success: function (data) {


                for (var i = 0; i < data.Table.length; i++) {
                    self.Trans.push(new TransClass(data.Table[i])); //Put the response in ObservableArray
                }

                for (var i = 0; i < data.Table2.length; i++) {
                    self.FacilityTrans.push(new TransFacilityClass(data.Table2[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table1.length; i++) {
                    self.TaxTrans.push(new TransTaxClass(data.Table1[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table3.length; i++) {
                    self.ImageTrans.push(new TransImageClass(data.Table3[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table4.length; i++) {
                    self.PolicyTrans.push(new TransPolicyClass(data.Table4[i])); //Put the response in ObservableArray
                }
                for (var i = 0; i < data.Table5.length; i++) {
                    self.FinePrintTrans.push(new TransFinePrintClass(data.Table5[i])); //Put the response in ObservableArray
                }

                for (var i = 0; i < data.Table6.length; i++) {

                    self.smsgateway.push(new TransSMSGatewayPrintClass(data.Table6[i])); //Put the response in ObservableArray
                }

                ko.applyBindings(bookingInvoiceVM, document.getElementById("Printdiv"));
             
            },
            error: function (err) {
                //  alert(err.status + " : " + err.statusText);

            }
        });
        AppCommonScript.HideWaitBlock();
    }

  
}


$("#hrefEmail").click(function () {

    Invc_Num = $.localStorage("Invce_Num");
    Consumer_Id = $.localStorage("Cons_Id");
    var self = this;
    self.Trans = ko.observableArray([]),
    self.TaxTrans = ko.observableArray([]),
    self.FacilityTrans = ko.observableArray([]),
    self.ImageTrans = ko.observableArray([]),
    self.PolicyTrans = ko.observableArray([]),
    self.FinePrintTrans = ko.observableArray([]),
    self.smsgateway = ko.observableArray([]),
    //new modified
    self.Prop_Name = ko.observable();
    self.Prop_Address = ko.observable();
    self.CIN = ko.observable();
    self.Cons_Name = ko.observable();
    self.Room_Name = ko.observable();
    self.Invce_Num = ko.observable();
    self.Image_Url = ko.observable();
    self.Checkin = ko.observable();
    self.Checkout = ko.observable();
    self.GuestName = ko.observable();
    self.Email = ko.observable();
    self.Mobile = ko.observable();
    self.Camo_Rate = ko.observable();
    self.Total = ko.observable();
    self.TaxType = ko.observable();
    self.TaxAmnt = ko.observable();
    //Email
    $.ajax({
        type: "GET",
        url: '/api/Consumer/GetBookingInvoice',
        contentType: "application/json; charset=utf-8",
        data: { Invce_Num: Invc_Num, Cons_Id: Consumer_Id },
        dataType: "json",
        success: function (data) {

            for (var i = 0; i < data.Table.length; i++) {
                self.Trans.push(new TransClass(data.Table[i])); //Put the response in ObservableArray
            }

            for (var i = 0; i < data.Table2.length; i++) {
                self.FacilityTrans.push(new TransFacilityClass(data.Table2[i])); //Put the response in ObservableArray
            }
            for (var i = 0; i < data.Table1.length; i++) {
                self.TaxTrans.push(new TransTaxClass(data.Table1[i])); //Put the response in ObservableArray
            }
            for (var i = 0; i < data.Table3.length; i++) {
                self.ImageTrans.push(new TransImageClass(data.Table3[i])); //Put the response in ObservableArray
            }
            for (var i = 0; i < data.Table4.length; i++) {
                self.PolicyTrans.push(new TransPolicyClass(data.Table4[i])); //Put the response in ObservableArray
            }
            for (var i = 0; i < data.Table5.length; i++) {
                self.FinePrintTrans.push(new TransFinePrintClass(data.Table5[i])); //Put the response in ObservableArray
            }

            for (var i = 0; i < data.Table6.length; i++) {

                self.smsgateway.push(new TransSMSGatewayPrintClass(data.Table6[i])); //Put the response in ObservableArray
            }

            var Invce_date = new Date(data.Table[0].Invce_date);
            var d1 = Invce_date.getDate();
            var m1 = Invce_date.getMonth() + 1;
            var y1 = Invce_date.getFullYear();
            var cur_date = new Date();
            var d2 = cur_date.getDate();
            var m2 = cur_date.getMonth() + 1;
            var y2 = cur_date.getFullYear();
            var smsdate = d1 + '-' + m1 + '-' + y1;
            //if (d1 == d2 && m1 == m2 && y1 == y2 && d1 == y1) {
            var tot = data.Table[0].camo_room_rate * (data.Table[0].Room_Count * data.Table[0].Days_Count);
            var bdy = '<div style="width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:"segoe UI";"><div style="background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;"><a href="#" style="display:inline-block;"><img src="http://www.lastminutekeys.com/img/Mailer/logo.png" title="Last Minute Keys" alt="" style="display:block;" /></a> </div><p style="color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);">';
            bdy += '<div style="width:100%;margin:0 auto;font-family: Segoe UI, Tahoma, Geneva, Verdana, sans-serif;font-size:14px;color:#353535;">'
            bdy += '<p>Thanks, <strong style="font-size:18px;color:#42b760;"><span>' + data.Table[0].Cons_First_Name + '</span></strong>! Your reservation is now confirmed.</p>'
            bdy += '<div><strong>Hotel Name:</strong>&nbsp;&nbsp;&nbsp; <span style="color:#2781CD;font-weight: bold;font-style: italic;margin:0;font-size:15px;">' + data.Table[0].Prop_Name + '</span></div><br>'
            bdy += '<div><strong>Address:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <ul style="font-size:14px;line-height:1.8em;color:#565656;list-style-type:none;padding:0;margin-top:0;display:inline-block;vertical-align:top;"><li>' + data.Table[0].Prop_Addr1 + '</li><li> Phone: <strong>022 6668 1234</strong></li></ul></div>'
            bdy += '<br/>'
            bdy += '<table width="100%" border="1" bordercolor="#eee" cellspacing="1" cellpadding="5">'
            bdy += '<tr><td colspan="4" rowspan="1" align="center" bgcolor="#2781cd"><h2 style="font-size: 20px;color: #FFF;margin: 5px 0;font-weight: 600;">Reservation Confirmation</h2></td></tr>'
            bdy += '<tr><td style="width:200px;"> <span style="font-weight: bold;">Occupant Names</span></td><td colspan="3" rowspan="1">' + data.Table[0].GuestName + '</td></tr>'
            bdy += '<tr><td><strong>Booking ID</strong></td><td colspan="3" rowspan="1">' + data.Table[0].Invce_Num + '</td></tr>'
            bdy += '<tr><td><strong>Check-in Date</strong></td><td>' + data.Table[0].Checkin + '</td>'
            bdy += '<td><strong>Check-out Date</strong></td><td>' + data.Table[0].Checkout + '</td></tr>'
            bdy += '<tr><td><strong>Room Type </strong></td><td>' + data.Table[0].Room_Name + '</td><td><strong>No of Rooms</strong></td><td>' + data.Table[0].Room_Count + '</td></tr>'
            bdy += '<tr><td><strong>No of Nights</strong></td><td>' + data.Table[0].Days_Count + '</td><td><strong>Number of Guests</strong></td><td colspan="3" rowspan="1">' + data.Table[0].Room_Count + '</td></tr>'
            bdy += '</table>'
            bdy += '<br />'
            bdy += '<table width="100%" border="1" bordercolor="#eee" cellspacing="1" cellpadding="5">'
            bdy += '<tr><td colspan="2" bgcolor="#eee" align="center"><h4 style="font-size: 18px;color: #42b760;text-transform: uppercase;margin:0px;font-weight:600;">Summary Of Charges</h4></td></tr>'
            bdy += '<tr><th align="left">Room Cost</th><td>&#8377; <span>' + parseFloat(data.Table[0].camo_room_rate).toFixed(2) + '</span></td></tr>'
            bdy += '<tr><th align="left">Number of Room</th><td><span>' + data.Table[0].Room_Count + '</span></td></tr>'
            bdy += '<tr><th align="left">Nights</th><td><span>' + data.Table[0].Days_Count + '</span></td></tr>'
            bdy += '<tr><th align="left">Total</th><td>&#8377; <span>' + parseFloat(tot).toFixed(2) + '</span></td></tr>'
            bdy += '<tr><th align="left"> Taxes</th><td>&#8377; <span>' + parseFloat(data.Table[0].tax_amnt).toFixed(2) + '</span></td></tr>'
            bdy += '<tr><th align="left">Price Payable</th><td style="font-size: 24px;color: #42b760;"><strong>&#8377; <span>' + parseFloat(data.Table[0].net_amt).toFixed(2) + '</span></strong> </td></tr>'
            bdy += '</table>'
            bdy += '<br/>'
            bdy += '<p><strong><font color="red">IMP NOTE:</font></strong> - Please carry Photo ID proof along with a photocopy for the tour.</p>'
            bdy += '<p><strong>Need help with your reservation?</strong></p><ul style="font-size:14px;line-height:1.8em;color:#353535;list-style-type:none;padding:0;"><li>Contact LMK&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Phone: +912243438150</li><li> Contact the property &nbsp;&nbsp;&nbsp;&nbsp;Phone: +912223462244</li></ul>'
            bdy += '<br/><br />'
            bdy += '<div style="text-align:right;border-top:3px double #eee;padding-top:20px;"><strong>Thanks & Regards</strong>,<br><font size="3">Lastminutekeys.com</font></div>'
            bdy += '<br/>'
            bdy += '<div style="background:#192b3e;padding:10px;"><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;">Follow Us</h4><ul style="padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;"><li style="float:left;width:33.3%;text-align:left;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;"></i> Follow us on Twitter</a></li><li style="float:left;width:33.3%;text-align:center;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Facebook</a></li><li style="float:left;width:33.3%;text-align:right;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Instagram</a></li><div style="clear:both;"></div></ul><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;">Contact OurCustomer Care</h4><p style="color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;"><a href="www.lastminutekeys.com/Home/Contact_Us" style="color:#FFF;font-size:14px;font-weight:400;">Click here</a></p><p style="color:#F4F4F4;text-align:center;font-size: 12px;line-height: 1.6em;margin:10px 0;"></p></div></div></div>'
            var sub = 'Booking Confirmation';
            var rcvr = data.Table[0].Cons_mailid;
            $.ajax({
                type: "POST",
                url: "/api/Consumer/SendMail",
                data: { Cons_Subject: sub, Cons_Body: bdy, Cons_mailid: rcvr },
                dataType: "json",
                async: false,
                success: function (response) {

                    AppCommonScript.HideWaitBlock();
                    //  Successed(response)

                    // SubVM.Email('')
                },
                error: function (err) {
                    AppCommonScript.HideWaitBlock();
                    Failed(JSON.parse(err.responseText));
                }
            });
            

            ko.applyBindings(bookingInvoiceVM, document.getElementById("Printdiv"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("trTax"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("Booking"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("inovoice-img"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("Inv"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("Consumer"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("TotPrice"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("Rate"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("Occupant"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("tblFacility"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("invoice_details"));
            //ko.applyBindings(bookingInvoiceVM, document.getElementById("FinePrint"));

        },
        error: function (err) {
            //  alert(err.status + " : " + err.statusText);

        }
    });

});

$("#hrefGeneratePDF").click(function () {

    window.open($.localStorage("bloburl"));
});

//Model
function TransClass(data) {
    
    var prop = this;

    

    prop.Cons_First_Name = data["Cons_First_Name"];
    prop.Prop_Addr1 = data["Prop_Addr1"];
    prop.Days_Count = data["Days_Count"];
    prop.Room_Count = data["Room_Count"];
    prop.camo_room_rate = data["camo_room_rate"];
    prop.tot = data["tot"];
    prop.tax_amnt = data["tax_amnt"];
    
    prop.tot = prop.camo_room_rate * (prop.Room_Count * prop.Days_Count);

    prop.Prop_Name = data["Prop_Name"];
    prop.Prop_Address = data["Prop_Addr1"];
    prop.CIN = 'CIN# ' + data["Prop_Cin_No"];
    prop.Name = data["Cons_First_Name"];
    //prop.Image_dir = data["Image_dir"];
    prop.Room_Name = data["Room_Name"];
    prop.Invce_Num = data["Invce_Num"];
    prop.Image_dir = data["Image_dir"];
    var date = new Date(data["Checkin"]).toDateString();
    prop.Checkin = date;
    var dateout = new Date(data["Checkout"]).toDateString();
    prop.Checkout = dateout;
    //prop.Facilities = ko.observable('');
    prop.GuestName = data["GuestName"];
    prop.Email = data["Cons_mailid"];
    prop.Mobile = data["Cons_Mobile"];
    prop.Camo_Rate = parseFloat(data["camo_room_rate"]).toFixed(2);
    //prop.Luxury_Tax = ko.observable('1');
    //prop.Service_Tax = ko.observable('1');
    prop.Total = data["net_amt"];
    



}
function TransTaxClass(data) {
    var prop = this;

    prop.TaxType = data["Vparam_Descr"] + ' ' + data["Vparam_Val"] + '%';
    prop.TaxAmnt = data["tax_amnt"];//(data["tax_amnt"]).toFixed(2));

}
function TransFacilityClass(data) {
    var facil = this;

    facil.Facilities = data["Facility_Name"] + ',';
}

function TransImageClass(data) {
    var prop = this;
    prop.Image_dir = data["Image_dir"];
}

function TransPolicyClass(data) {
    var prop = this;
    prop.Policy_Name = data["Policy_Name"];
    prop.Policy_Descr = data["Policy_Descr"];

}
function TransFinePrintClass(data) {
    var prop = this;
    prop.Invce_fineprint = data["Invce_fineprint"];

}

function TransSMSGatewayPrintClass(data) {

    var prop = this;
    prop.Sms_Url = data["Sms_Url"];

}











