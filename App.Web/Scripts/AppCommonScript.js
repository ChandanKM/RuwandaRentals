var AppCommonScript = function () {
    return {
        showNotify: function(resposeData) {

            var message = "";
            $('input,select,textarea,option').removeClass('input-error');
            if (resposeData.Status == true) {
                $.each(resposeData.ReturnMessage, function (index) {
                    message += "<li style= 'text-align: left !important'> " + resposeData.ReturnMessage[index] + " </li>";
                });
                showNotification({ message: message, type: resposeData.ErrorType, autoClose: true, duration: 5 });
            } else {
                $.each(resposeData.Errors, function(index) {
                    $("#" + resposeData.Errors[index].ControlName).addClass('input-validation-error');
                    message += "<li style= 'text-align: left !important'> " + resposeData.Errors[index].Message + " </li>";
                });
                showNotification({ message: message, type: resposeData.ErrorType, autoClose: true, duration: 5 });
            }
        },
        ShowWaitBlock: function() {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    //backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: '<h5 style="font-size:20px;">  <img src="/img/sub_Images/Clock-Rotating.gif" style="width:50px;height:50px;"/><br/> Please wait...</h5>'
            });

        },

        HideWaitBlock: function () {
            $.unblockUI();
        }
    };
}();
