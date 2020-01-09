
var contactusVM = new ContactUsViewModel();
ko.applyBindings(contactusVM, document.getElementById("formContactUs"));

function ContactUsViewModel() {
    var self = this;
    self.Name = ko.observable('');
    self.EmailAddress = ko.observable('');
    self.Mobile = ko.observable('');
    self.Message = ko.observable('');
   
    self.Submit = function (data) {
        AppCommonScript.ShowWaitBlock();
       
        $.ajax({
            type: "Post",
            url: '/api/Consumer/AddContactUs',
            data: ko.toJSON(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                

                var bdy = '<div style="width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:"segoe UI";"><div style="background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;"><a href="#" style="display:inline-block;"><img src="http://www.lastminutekeys.com/img/Mailer/logo.png" title="Last Minute Keys" alt="" style="display:block;" /></a> </div><p style="color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);"> Dear <b>' + self.Name() + '</b>, <br />Thank you for contacting us. Our customer sales executive will revert back to you as soon as possible.<br />Thank you for choosing Lastminutekeys<br /><br /> <b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style="background:#192b3e;padding:10px;"><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;">Follow Us</h4><ul style="padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;"><li style="float:left;width:33.3%;text-align:left;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;"></i> Follow us on Twitter</a></li><li style="float:left;width:33.3%;text-align:center;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Facebook</a></li><li style="float:left;width:33.3%;text-align:right;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Instagram</a></li><div style="clear:both;"></div></ul><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;">Contact OurCustomer Care</h4><p style="color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;"><a href="www.lastminutekeys.com/Home/Contact_Us" style="color:#FFF;font-size:14px;font-weight:400;">Click here</a></p><p style="color:#F4F4F4;text-align:center;font-size: 12px;line-height: 1.6em;margin:10px 0;"></p></div></div>'
             //   var bdy = '<b>Dear ' + self.Name() + '!</b>,<br/><br/><b>Thank you for contacting us. Our customer sales executive will revert back to you as soon as possible.</b><br/><br/>Thank you for choosing Lastminutekeys<br/><br/>The Lastminutekeys Team<br/><br/>www.lastminutekeys.com';
                var sub = 'Thank you for contacting us ';
                var rcvr = self.EmailAddress();
                
                $.ajax({
                    type: "POST",
                    url: "/api/Consumer/SendMail",
                    data: { Cons_Subject: sub, Cons_Body: bdy, Cons_mailid: rcvr },
                    dataType: "json",
                    success: function (response) {
                      
                    },
                    error: function (err) {
                        Failed(JSON.parse(err.responseText));
                    }
                });
                var result = { Status: true, ReturnMessage: { ReturnMessage: "Thanks!We will get back to you soon!" }, ErrorType: "Success" };
                Successed(result);
                AppCommonScript.HideWaitBlock();
                OnSuccessSubmit();
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        });
    }
    self.Cancel = function () {
        window.location.href = '/Home';
    }

    function OnSuccessSubmit() {
        self.Name('');
        self.EmailAddress('');
        self.Mobile('');
        self.Message('');
    }
}
function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}