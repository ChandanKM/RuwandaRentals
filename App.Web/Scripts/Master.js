

var master = new MasterProfile();
master.GetUserId();

function MasterProfile() {
    
    var self = this;
    self.Vendor_Id = ko.observable();
    self.User_name = ko.observable('Vendor');

    self.GetUserId = function () {
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/Vendor/GetLoginVendorId",
            // dataType: "json",
            success: function (response) {
                
                if (response != 'ss') {
                    $.localStorage("VendId", response)
                    self.Vendor_Id(response)
                    $.ajax({
                        type: "GET",
                        url: '/api/vendors/Bind',
                        data: { ID: self.Vendor_Id() },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.length == 0) {

                            }
                            else {
                                
                                $('#RoomsBind').css('display','none');
                                self.User_name(' ' + data[0].Vndr_Name);
                                $('#UName').empty();
                                $('#UName').append(data[0].Vndr_Name);
                                $('#Vgroup').attr('href', '/Vendor/Bind');

                                var urlPathMaster = window.location.pathname;
                                var authority_Id = urlPathMaster.substring(urlPathMaster.lastIndexOf("/") + 1, urlPathMaster.length);
                                authority_Id = authority_Id.split('-', 2)
                                authority_Id = authority_Id[0];
                                if (authority_Id == 4) {
                                    $('#UsrType').empty();
                                    $('#UsrType').append("Admin's");
                                }
                                if (authority_Id == 5) {
                                    $('#UsrType').empty();
                                    $('#UsrType').append("User's");
                                }
                                //  }
                            }
                            AppCommonScript.HideWaitBlock();
                        },
                        error: function (err) {
                            //  alert(err.status + " : " + err.statusText);
                            AppCommonScript.HideWaitBlock();
                        }
                    })

                    $.ajax({
                        type: "GET",
                        url: '/api/vendors/GetUserPermissions',

                        contentType: "application/json; charset=utf-8",
                        async: false,
                        // dataType: "json",

                        success: function (data) {

                            if (data.Table.length == 0) {
                                
                            }

                            else {
                                for (var i = 0; i < data.Table.length; i++) {
                                    if (data.Table[i].page_id == "1" && data.Table[i].active_flag == "false      ") {
                                        
                                        $('#UserProfileBind').css('display', 'none')
                                       

                                    }
                                    else if (data.Table[i].page_id == "1" && data.Table[i].active_flag == "true      " && data.Table[i].AuthID == "4") {
                                        //$('#UserProfileBind').css('display', 'block')
                                        //$('#UserPLink').attr('href', '/Vendor/UserProfile/5-' + data.Table1[0].User_Type + '');
                                    }
                                    else if (data.Table[i].page_id == "7" && data.Table[i].active_flag == "false      ") {
                                        $('#PropertyBind').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "7" && data.Table[i].active_flag == "true      ") {
                                       // $('#PropertyBind').css('display', 'block')
                                       // $('#PropPage').attr('href', '/' + data.Table[i].Url);
                                    }
                                    else if (data.Table[i].page_id == "10" && data.Table[i].active_flag == "false     ") {
                                        
                                        $('#RoomsBind').css('display', 'none')
                                        $('.PropertyRoom').css('display', 'none')

                                        $('#RoomdtProperty').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "10" && data.Table[i].active_flag == "true      ") {
                                        //$('#RoomsBind').css('display', 'block')
                                        //$('#PropRooms').attr('href', '/' + data.Table[i].Url);
                                      //  $('.PropertyRoom').css('display', 'block')

                                        //$('#RoomdtProperty').css('display', 'block')
                                       
                                    }
                                    else if (data.Table[i].page_id == "11" && data.Table[i].active_flag == "false     ") {
                                       
                                        $('#PropertyRateCal').css('display', 'none')
                                        $('#RateCalendarMenu').css('display', 'none')
                                        $('#RateCalendarTop').css('display', 'none')
                                        $('.PropertyRateDT').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "11" && data.Table[i].active_flag == "true      ") {

                                        //$('#PropertyRateCal').css('display', 'block')
                                        //$('#PropRateCal').attr('href', '/' + data.Table[i].Url);
                                        //$('#RateCal').attr('href', '/' + data.Table[i].Url);
                                        //$('#RateCalendarTopUrl').attr('href', '/' + data.Table[i].Url);

                                        //$('#RateCalendarMenu').css('display', 'block')
                                        //$('#RateCalendarTop').css('display', 'block')
                                        //$('#PropertyRateDT').css('display', 'block')

                                    }
                                    else if (data.Table[i].page_id == "12" && data.Table[i].active_flag == "false     ") {
                                        $('#BookingsBind').css('display', 'none')

                                    }
                                    else if (data.Table[i].page_id == "12" && data.Table[i].active_flag == "true      ") {
                                       // $('#BookingsBind').css('display', 'block')
                                    }
                                    else if (data.Table[i].page_id == "13" && data.Table[i].active_flag == "false     ") {
                                        $('#ReportsBind').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "13" && data.Table[i].active_flag == "true      ") {
                                       // $('#ReportsBind').css('display', 'block')
                                    }
                                    else if (data.Table[i].page_id == "14" && data.Table[i].active_flag == "false     ") {
                                        $('.PropertyAttribute').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "1" && data.Table[i].active_flag == "false     ") {
                                        $('.PropMangr').css('display', 'none')
                                    }
                                }
                            }
                        }
                        });

                    ko.applyBindings(master, document.getElementById("lyoutMaster"));
                }
                else {

                    $.ajax({
                        type: "GET",
                        url: '/api/vendors/GetUserPermissions',

                        contentType: "application/json; charset=utf-8",
                        async: false,
                        // dataType: "json",

                        success: function (data) {

                            if (data.Table.length == 0) {
                                
                            }

                            else {
                                
                                //$('#BookingsBind').css('display', 'none');
                                $('#VGBind').css('display', 'none');
                                $('#VGClick').css('display', 'none');
                                $('#AdminProfileBind').css('display', 'none');
                                //RoomsBind
                                //PropRooms
                                //PropertyRateCal
                                //PropRateCal
                                //UserProfileBind
                                //UserPLink
                                //RateCalendarMenu
                                // DashboardBind
                                //RoomdtProperty
                                //PropertyAttributes
                                //PropertyRateDT
                                //PropertyRoomDT

                                $('#DashboardBind').css('display', 'none')
                                $('#UserProfileBind').css('display', 'none')
                                $('#RateCalendarMenu').css('display', 'none')
                                $('#PropertyBind').css('display', 'none')
                                $('#RoomsBind').css('display', 'none')
                                $('#PropertyRateCal').css('display', 'none')
                                $('#BookingsBind').css('display', 'none')
                                $('#VendProfile').css('display', 'none')
                                $('#RateCalendarTop').css('display', 'none')
                                $('#RmNavMenu').css('display', 'none')
                                $('#PropertyRoomDT').css('display', 'none')
                                $('#PropertyRateDT').css('display', 'none')
                                $('#PropertyAttributes').css('display', 'none')
                                $('#RoomdtProperty').css('display', 'none')

                                for (var i = 0; i < data.Table.length; i++) {
                                    if (data.Table[i].page_id == "1" && data.Table[i].active_flag == "false      ") {
                                        $('#UserProfileBind').css('display', 'none')

                                    }
                                    else if (data.Table[i].page_id == "1" && data.Table[i].active_flag == "true      " && data.Table[i].AuthID == "4") {
                                        $('#UserProfileBind').css('display', 'block')
                                        $('#UserPLink').attr('href', '/Vendor/UserProfile/5-' + data.Table1[0].User_Type + '');
                                    }
                                    else if (data.Table[i].page_id == "7" && data.Table[i].active_flag == "false      ") {
                                        $('#PropertyBind').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "7" && data.Table[i].active_flag == "true      ") {
                                        $('#PropertyBind').css('display', 'block')
                                        $('#PropPage').attr('href', '/' + data.Table[i].Url);
                                    }
                                    else if (data.Table[i].page_id == "10" && data.Table[i].active_flag == "false      ") {
                                        $('#RoomsBind').css('display', 'none')
                                        $('#PropertyRoomDT').css('display', 'none')

                                        $('#RoomdtProperty').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "10" && data.Table[i].active_flag == "true      ") {
                                        $('#RoomsBind').css('display', 'block')
                                        $('#PropRooms').attr('href', '/' + data.Table[i].Url);
                                        $('#PropertyRoomDT').css('display', 'block')

                                        $('#RoomdtProperty').css('display', 'block')
                                    }
                                    else if (data.Table[i].page_id == "11" && data.Table[i].active_flag == "false     ") {
                                        $('#PropertyRateCal').css('display', 'none')
                                        $('#RateCalendarMenu').css('display', 'none')
                                        $('#RateCalendarTop').css('display', 'none')
                                        $('#PropertyRateDT').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "11" && data.Table[i].active_flag == "true      ") {

                                        $('#PropertyRateCal').css('display', 'block')
                                        $('#PropRateCal').attr('href', '/' + data.Table[i].Url);
                                        $('#RateCal').attr('href', '/' + data.Table[i].Url);
                                        $('#RateCalendarTopUrl').attr('href', '/' + data.Table[i].Url);

                                        $('#RateCalendarMenu').css('display', 'block')
                                        $('#RateCalendarTop').css('display', 'block')
                                        $('#PropertyRateDT').css('display', 'block')

                                    }
                                    else if (data.Table[i].page_id == "12" && data.Table[i].active_flag == "false     ") {
                                        $('#BookingsBind').css('display', 'none')

                                    }
                                    else if (data.Table[i].page_id == "12" && data.Table[i].active_flag == "true      ") {
                                        $('#BookingsBind').css('display', 'block')
                                    }
                                    else if (data.Table[i].page_id == "13" && data.Table[i].active_flag == "false     ") {
                                        $('#ReportsBind').css('display', 'none')
                                    }
                                    else if (data.Table[i].page_id == "13" && data.Table[i].active_flag == "true      ") {
                                        $('#ReportsBind').css('display', 'block')
                                    }
                                }

                                $('#UName').empty();
                                $('#UName').append(data.Table1[0].FirstName);

                                self.User_name(' ' + data.Table1[0].FirstName);
                                ko.applyBindings(master, document.getElementById("lyoutMaster"));
                            }



                            AppCommonScript.HideWaitBlock();
                        },
                        error: function (err) {

                            //  alert(err.status + " : " + err.statusText);
                            AppCommonScript.HideWaitBlock();
                        }
                    })
                }

            },


            error: function (response) {
            }
        });


    }
}




