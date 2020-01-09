var urlPath = window.location.pathname;

$(function () {
    Policies: ko.observableArray([]),
    //ko.applyBindings(RoomyListVM);
    PoliciesListVM.getPolicy();
    ko.applyBindings(PoliciesListVM, document.getElementById('tab3'));
    $('#updatepolicy').hide();

});
//View Model
var PoliciesListVM = {

    Policies: ko.observableArray([]),
    
    getPolicy: function () {
        
        var self = this;
        
        $.ajax({
            type: "GET",
            url: '/api/rooms/BindPolicy',
            contentType: "application/json; charset=utf-8",
            dataType: "json",           
            success: function (data) {
                PoliciesListVM.Policies.removeAll();
                for (var i = 0; i <= data.length; i++)
                    PoliciesListVM.Policies.push(new PolicyClass(data[i])); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    },

    //Edit Policy
   
    
    

};

//View Model 1
var PoliciesListVM1 = {

    Policies1: ko.observableArray([]),

    getpolicy1: function (Policy_Id) {
        
        
        $.ajax({
            type: "GET",
            url: '/api/rooms/Editpolicy',           
            data: { Id: Policy_Id },
            //  data: { Id: array[3] },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {

                    PoliciesListVM1.Policies1.push(new PolicyClass(data[i])); //Put the response in ObservableArray
                    
                }

            },           
            error: function (err) {
                alert(err.status + " : " + err.statusText);

            }
        });
    },
};



EditPolicy = function (policy) {
    
    PoliciesListVM1.getpolicy1(policy.Policy_Id);    
    ko.applyBindings(PoliciesListVM1, document.getElementById('updatepolicy'));
    $('#updatepolicy').show();
    $('#tab3').hide();
};





//Update Policies
updatepolicies = function () {
    
    AppCommonScript.ShowWaitBlock();
    $.ajax({
        url: '/api/rooms/Updatepolicy',
        type: 'post',
        dataType: 'json',
        data: ko.toJSON(this),

        contentType: 'application/json',
        success: function (result) {
            AppCommonScript.HideWaitBlock();
            AppCommonScript.showNotify(result);
            $('#tab3').show();
            $("#tab3").load();
            PoliciesListVM.getPolicy();
            ko.applyBindings(PoliciesListVM, document.getElementById('tab3'));
        },
        error: function (err) {
            if (err.responseText == "Creation Failed") {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(err);
            }
            else {
                AppCommonScript.HideWaitBlock();
                AppCommonScript.showNotify(err);

            }
        },
        complete: function () {

        }

    });
  
    $('#updatepolicy').hide();
    $(document).ready(function () {
        setTimeout(function () {
            PoliciesListVM.getPolicy();
            ko.applyBindings(PoliciesListVM, document.getElementById('Grid3'));
        }, 8000);
       
    });

    PoliciesListVM.getPolicy().ready;
};

//Suspend Policies
var PolicySuspend = {

    ActiveSuspend: function (data) {
        $.ajax({
            type: "POST",
            url: '/api/rooms/Suspendpolicy',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(data),
            success: function (response) {
               

                ko.applyBindings(PoliciesListVM.getPolicy(), document.getElementById('tab3'));

            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });

    }
};


self.SuspendPolicy = function (room) {
    if (confirm('Are you sure to Suspend the Policy ?')) {
        PolicySuspend.ActiveSuspend(room)
    }
   
};




//Model
function PolicyClass(data) {
    var policy = this;

        policy.Policy_Id = data.Policy_Id;
        policy.Policy_Name = ko.observable(data.Policy_Name);
        policy.Policy_Descr = ko.observable(data.Policy_Descr);
        

}
    


