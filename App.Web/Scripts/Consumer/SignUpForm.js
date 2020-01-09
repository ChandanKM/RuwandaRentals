var signupVM = new SignUpViewModel();
ko.applyBindings(signupVM, document.getElementById('signupForm'));

function SignUpViewModel() {

    var self = this;
    self.Cons_First_Name = ko.observable('');
    self.Cons_Last_Name = ko.observable('');
    self.Cons_mailid = ko.observable('');
    self.Cons_Pswd = ko.observable('');
    self.ConfirmPassword = ko.observable('');
    self.Cons_Mobile = ko.observable('');
    self.Check = ko.observable(true);
    self.emailCheck = ko.observable(false);

    self.Submit = function (data) {

        if (data.emailCheck() == true) {

        }

        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/api/Consumer/SignUpConsumer",
            data: $.parseJSON(ko.toJSON(data)),
            dataType: "json",
            success: function (response) {

                if (response.Table[0].Result == 0) {
                    var result = { Status: true, ReturnMessage: { ReturnMessage: 'You are registered already!!' }, ErrorType: "error" };
                    Successed(result);
                    //  window.location.href = '/Consumer/Signin'
                }
                else {

                    var bdy = '<div style="width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:"segoe UI";"><div style="background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;"><a href="#" style="display:inline-block;"><img src="http://www.lastminutekeys.com/img/Mailer/logo.png" title="Last Minute Keys" alt="" style="display:block;" /></a> </div><p style="color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);"> Dear <b>' + self.Cons_First_Name() + '</b>, <br />You have successfully set up your Lastminutekeys.com account<br /> <br />Your registered email address is <b>' + self.Cons_mailid() + '</b><br /><br />Lastminutekeys is a one stop shop mobile & web based solution to get you quality rooms at the last minute with the best price available. <br /><br /> <span style="color:#006ECB;">If you have any questions about our services, please go through our <a href="http://www.lastminutekeys.com/Home/Faq" style="color:#006ECB;">FAQs</a></span> <br /><br />Look forward in making your last minute booking experience a memorable one.<br />Thank you for choosing Lastminutekeys<br /><br /> <b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style="background:#192b3e;padding:10px;"><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;">Follow Us</h4><ul style="padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;"><li style="float:left;width:33.3%;text-align:left;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;"></i> Follow us on Twitter</a></li><li style="float:left;width:33.3%;text-align:center;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Facebook</a></li><li style="float:left;width:33.3%;text-align:right;margin-left:0;"><a href="#" style="display:block;color:#F5F5F5;font-size:12px;text-decoration:none;"><i style="width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;"></i> Follow us on Instagram</a></li><div style="clear:both;"></div></ul><h4 style="margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;">Contact OurCustomer Care</h4><p style="color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;"><a href="www.lastminutekeys.com/Home/Contact_Us" style="color:#FFF;font-size:14px;font-weight:400;">Click here</a></p><p style="color:#F4F4F4;text-align:center;font-size: 12px;line-height: 1.6em;margin:10px 0;"></p></div></div>'
                    //var bdy = "<b>Dear " + self.Cons_First_Name() + "!, <br/><br/>You have successfully set up your Lastminutekeys.com account<br/><br>Your registered email address is " + self.Cons_mailid() + "<br/><br/>Lastminutekeys is a one stop shop mobile & web based solution to get you quality rooms at the last minute with the best price available. <br/><br/><br/>If you have any questions about our services, please go through our FAQs <br/><br/><br/>Look forward in making your last minute booking experience a memorable one.<br/>Thank you for choosing Lastminutekeys<br/><br/> The Lastminutekeys Team<br/><br/>www.lastminutekeys.com";
                    var sub = 'Registration confirmation';
                    var rcvr = self.Cons_mailid();
                    var pswd = self.Cons_Pswd();
                    $.ajax({
                        type: "POST",
                        url: "/api/Consumer/SendMail",
                        data: { Cons_Subject: sub, Cons_Body: bdy, Cons_mailid: rcvr },
                        dataType: "json",
                        success: function (response) {
                            //  alert('Worked')
                            // Successed(response)

                            // SubVM.Email('')                                
                            
                        },
                        error: function (err) {
                            
                            Failed(JSON.parse(err.responseText));
                        }
                    });

                    //$('#signupForm').reset();
                    
                    //var result = { Status: true, ReturnMessage: { ReturnMessage: 'SignUp Successfully' }, ErrorType: "Success" };

                    //Successed(result);
                    
                    var r = confirm("SignUp Successfull. Do you want continue?");
                    if (r == true) {
                        $.ajax({
                            type: "POST",
                            url: "/api/Consumer/WebLogin",
                            data: { Cons_mailid: rcvr, Cons_Pswd: pswd, returnUrl: window.location.href },
                            dataType: "json",
                            success: function (response) {
                                
                                if (response != null) {
                                    var vars = [], hash;
                                    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                                    for (var i = 0; i < hashes.length; i++) {
                                        hash = hashes[i].split('=');
                                        vars.push(hash[0]);
                                        vars[hash[0]] = hash[1];
                                    }
                                  var returnurl =  hash[1]
                                  if (returnurl == undefined)
                                  {
                                      window.location.href = "/home/index";
                                  }
                                  else
                                  {
                                      returnurl = returnurl.replace('%2F', '');
                                      window.location.href = returnurl;
                                  }
                                    
                                    //var result = { Status: true, ReturnMessage: { ReturnMessage: 'SignUp Successfully' }, ErrorType: "Success" };
                                    //Successed(result);
                                }
                                else {

                                    var result = { Status: true, ReturnMessage: { ReturnMessage: "Please Enter Valid User Name and Password" }, ErrorType: "error" };
                                    Failed(result);
                                }
                            },
                            error: function (err) {
                                Failed(JSON.parse(err.responseText));
                            }
                        });
                    } else {
                        
                        //window.location.href = '/Signup/index'
                        var result = { Status: true, ReturnMessage: { ReturnMessage: 'SignUp Successfull' }, ErrorType: "Success" };
                        Successed(result);
                    }

                }

            },
            error: function (err) {
                
                Failed(JSON.parse(err.responseText));
            }
        });

    }


    self.Change = function () {

        if ($('#chkcorp').prop("checked") == true) {
            var freeServices = ["yahoo.com", "gmail.com"];
            var email = $('#txtemail').val();
            for (var i = 0; i < freeServices.length; i++) {
                if (email.indexOf(freeServices[i]) != -1) {
                    var result = { Status: true, ReturnMessage: { ReturnMessage: "Business email is required" }, ErrorType: "error" };
                    Failed(result);

                    $('#txtemail').val('');
                }
            }



        }
    }

    self.checkemail = function () {

        if ($('#chkcorp').prop("checked") == true) {
            var freeServices = ["yahoo", "gmail"];
            var email = $('#txtemail').val();
            for (var i = 0; i < freeServices.length; i++) {
                if (email.indexOf(freeServices[i]) != -1) {

                    var result = { Status: true, ReturnMessage: { ReturnMessage: "Business email is required" }, ErrorType: "success" };
                    Failed(result);

                    $('#txtemail').val('');
                }
            }



        }
    }


    self.Reset = function () {
        self.Cons_First_Name('');
        self.Cons_Last_Name('');
        self.Cons_mailid('');
        self.Cons_Pswd('');
        self.ConfirmPassword('');
        self.Cons_Mobile('');
        self.Check(false);
    }
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
    //var signupVM = new SignUpViewModel();
    ko.cleanNode(document.getElementById('signupForm'));
    signupVM.Reset();
    ko.applyBindings(signupVM, document.getElementById('signupForm'));
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

