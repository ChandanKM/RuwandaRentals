
var urlPathMaster = window.location.pathname;
var CurrentPage = urlPathMaster.substring(urlPathMaster.lastIndexOf("/") + 1, urlPathMaster.length);
if (CurrentPage == 'Facilities' || CurrentPage == 'Room_Type')
{
    $('#Masters').addClass('current');
}
else if (CurrentPage == 'Profiles' || CurrentPage == 'VendorProfiles' || CurrentPage == 'ProfileProperties' || CurrentPage == 'PropertyUserProfiles') {
    $('#SysSetting').addClass('current');
}
else if (CurrentPage == 'Properties' || CurrentPage == 'Upcomming_Bookings') {
    $('#SetSettings').addClass('current');
}

else if (CurrentPage == 'Occupancy' || CurrentPage == 'Consumer_Report' || CurrentPage == 'Tax_Report' || CurrentPage == 'CCAvenue_Charges' || CurrentPage == 'LMK_Margin_Reports') {
    $('#CurrentReport').addClass('current');
}
else if (CurrentPage == 'DashBoard') {
    $('#DashboardBind').addClass('current');
}
else {
    $('#SysSetting').addClass('current');
}
var master = new MasterProfile();
master.GetUserId();

function MasterProfile() {
    
    var self = this;

    self.User_name = ko.observable('Super Admin');

    self.GetUserId = function () {
        AppCommonScript.ShowWaitBlock();

        $.ajax({
            type: "GET",
            url: '/SuperAdmin/GetLoginName',

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
              

                AppCommonScript.HideWaitBlock();
                $('#username').append(data.Table[0].Company_Title)
                //User Permission
                $.ajax({
                    type: "GET",
                    url: '/api/UserParam/GetUserPermissions',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (data) {
                        if (data.Table.length == 0) {
                           alert("test")
                        }
                        else {



                        }



                        AppCommonScript.HideWaitBlock();
                    },
                    error: function (err) {

                        //  alert(err.status + " : " + err.statusText);
                        AppCommonScript.HideWaitBlock();
                    }
                })

                // Failed(JSON.parse(jqxhr.responseText));
            },
            //AppCommonScript.HideWaitBlock();

            error: function (err) {
             
                $('#username').append(err.responseText)
            
                $.ajax({
                    type: "GET",
                    url: '/api/UserParam/GetUserPermissions',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,

                    success: function (data) {
                       
                        if (data.Table.length == 0) {
                          
                        }
                        else {

                            for (var i = 0; i < data.Table.length; i++) {
                                
                                $('#' + data.Table[i].Page_Id + '').hide();
                                if(data.Table[i].Page_Id==13)
                                {
                                    $('#AdminProfile').hide();
                                }
                            }



                        }



                        AppCommonScript.HideWaitBlock();
                    },
                    error: function (err) {
                     

                        //  alert(err.status + " : " + err.statusText);
                        AppCommonScript.HideWaitBlock();
                    }
                })


            }
        });

        ko.applyBindings(master, document.getElementById("lyoutMasterSuperAdmin"));

        AppCommonScript.HideWaitBlock();


    }

}



function Report()
{
    $('#CurrentReport').addClass('current');
}


function expToExcel() {
   
    //var data = $("#DataTables_Table_0").html();
    //data = escape(data);


    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('DataTables_Table_0'); // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    $('body').prepend("<form method='post' action='/SuperAdmin/Export' style='top:-3333333333px;' id='tempForm'><input type='hidden' name='data' value='" + escape(tab_text) + "' ></form>");
    $('#tempForm').submit();
    $("tempForm").remove();
    return false;

}


function marginExcel() {

    //var data = $("#DataTables_Table_0").html();
    //data = escape(data);


    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('DataTables_Table_0'); // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    $('body').prepend("<form method='post' action='/SuperAdmin/marginExport' style='top:-3333333333px;' id='tempForm'><input type='hidden' name='data' value='" + escape(tab_text) + "' ></form>");
    $('#tempForm').submit();
    $("tempForm").remove();
    return false;

}