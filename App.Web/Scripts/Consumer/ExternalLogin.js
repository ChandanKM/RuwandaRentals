window.fbAsyncInit = function () {
    FB.init({
        appId: '679372372191024', // Set YOUR APP ID
        channelUrl: 'http://www.lastminutekeys.com/SignIn', // Channel File
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true , // parse XFBML
       
    });

    FB.Event.subscribe('auth.authResponseChange', function (response) {
        if (response.status === 'connected') {
            // document.getElementById("message").innerHTML += "<br>Connected to Facebook";
            //SUCCESS

        }
        else if (response.status === 'not_authorized') {
            //   document.getElementById("message").innerHTML += "<br>Failed to Connect";

            //FAILED
        } else {
            //  document.getElementById("message").innerHTML += "<br>Logged Out";

            //UNKNOWN ERROR
        }
    });
};

function FbLogin() {
    FB.login(function (response) {
        if (response.authResponse) {
            CreateUserProfile();
        } else {
            console.log('User cancelled login or did not fully authorize.');
        }
    }, { scope: 'email,user_photos,user_videos' });
}

function PopUpFbLogin() {
    
    FB.login(function (response) {
        
        if (response.authResponse) {
            
            CreateUserProfile();
        } else {
            
            console.log('User cancelled login or did not fully authorize.');
        }
    }, { scope: 'email,user_photos,user_videos' });
}

function GetUserInfo() {
    FB.api('/me', function (response) {

        var str = "<b>Name</b> : " + response.name + "<br>";
        str += "<b>Link: </b>" + response.link + "<br>";
        str += "<b>Username:</b> " + response.username + "<br>";
        str += "<b>id: </b>" + response.id + "<br>";
        str += "<b>Email:</b> " + response.email + "<br>";
        str += "<input type='button' value='Get Photo' onclick='GetPhoto();'/>";
        str += "<input type='button' value='Logout' onclick='Logout();'/>";
        document.getElementById("status").innerHTML = str;

    });
}
function GetPhoto() {
    FB.api('/me/picture?type=normal', function (response) {

        var str = "<br/><b>Pic</b> : <img src='" + response.data.url + "'/>";
        document.getElementById("status").innerHTML += str;

    });

}
function Logout() {
    FB.logout(function () { document.location.reload(); });
}

// Load the SDK asynchronously
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    // js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&appId=1087433841273430&version=v2.0";
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));

function CreateUserProfile() {
    AppCommonScript.ShowWaitBlock();
    FB.api('/me', function (response) {
        
        $.ajax({
            type: "GET",
            url: "/api/Consumer/AuthenticateUser",
            data: { UserId: response.id, Name: response.name, EmailId: response.email, returnUrl: window.location.href },
            dataType: "json",
            async: false,
            success: function (response) {
                
                AppCommonScript.HideWaitBlock();
                if (response != null)
                    window.location.href =response;
            },
            error: function (err) {
                AppCommonScript.HideWaitBlock();
                Failed(JSON.parse(err.responseText));
            }
        });
    });
}

function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}