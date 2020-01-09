

    var master = new Consumer_AccountMaster();
    ko.applyBindings(master, document.getElementById("user_name"));
    master.GetUserId();

    

function Consumer_AccountMaster() {
    
    var self = this;
    self.Consumer_Id = ko.observable();
    self.Firstname = ko.observable();
    self.Lastname = ko.observable();
    self.Logoff = ko.observable();
    self.User_Name = ko.computed(function () {
        return self.Firstname() + " " + self.Lastname();
    });

    self.GetUserId = function () {
        
        AppCommonScript.ShowWaitBlock();
        $.ajax({
            type: "POST",
            url: "/Signin/GetLoggedInUserId",
            //dataType: "json",
            async: false,
            success: function (response) {

            
                if (response == 0) {
                    $('#liSignIn').show();
                    $('#liUserName').hide();
                    AppCommonScript.HideWaitBlock();
                }
                else {
                 
                    $('#liSignIn').hide();
                    $('#liUserName').show();
                    self.Consumer_Id(response);
                    $.localStorage("Consumer_Id", response);
                    OnSuccessGetDetails();

                    AppCommonScript.HideWaitBlock();
                }
            },
            error: function (err) {
               
                AppCommonScript.HideWaitBlock();
            }
        });
    }

    function OnSuccessGetDetails() {
      
        $.ajax({
            type: "GET",
            url: '/api/Consumer/GetProfileDetails',
            data: { Cons_Id: self.Consumer_Id() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.Table.length > 0)
                    OnSuccessBindDetails(data.Table[0])
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        }).done(function () {
          
        });
    }

    function OnSuccessBindDetails(data) {
        self.Firstname(data.Cons_First_Name || '');
        self.Lastname(data.Cons_Last_Name || '')
    }
    //
    //self.Logout = function () {
    //    
    //    AppCommonScript.ShowWaitBlock();
    //    window.localStorage.clear();
    //    window.location.href = '/Signin/Logoff';
    //}
}

$("#aLogout").click(function () {
    
    AppCommonScript.ShowWaitBlock();
    window.localStorage.clear();
    localStorage.clear();
    $.localStorage("Consumer_Id","");
    //window.location.href = '/Signin/Logoff';
    //var a = 0;

    $.ajax({
        type: "POST",
        url: "/Signin/Logoff",
        //dataType: "json",
        async: false,
        success: function (response) {
            if (response == 0) {
                $('#liSignIn').show();
                $('#liUserName').hide();
                AppCommonScript.HideWaitBlock();
            }
        },
        error: function (err) {

            AppCommonScript.HideWaitBlock();
        }
    });

});







