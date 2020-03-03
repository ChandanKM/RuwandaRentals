var urlPath = window.location.pathname;

var RoomID = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
var VendorId = $.localStorage("VendId");
var PropID = $.localStorage("RoomPropertyId");

var RoomId = RoomID


$(document).ready(function () {

    InitializepropertyViewModel();
});

function InitializepropertyViewModel() {


    propertyViewModel = new PropertyViewModel();
    propertyViewModel.getPolicy();
   
    ko.applyBindings(propertyViewModel, document.getElementById('tab_1_3'));

    // viewModel.chosenCountries.push('France');

}


function PropertyViewModel() {
    this.Policies = ko.observableArray([]);
    this.getPolicy = function () {
        getPolicy();
    };
    this.Prop_Id = ko.observable("");
    this.Policy_Id = ko.observable("");
    this.Policy_Name = ko.observable("");
    this.Policy_descr = ko.observable("");
    this.Policy_descrEdit = ko.observable("");
    this.Vend_Id = ko.observable("");
}
this.Cancel = function () {

    window.location.href = "/Property/Bind"
}

function getPolicy() {
     AppCommonScript.ShowWaitBlock();
   
    var self = this;
    
    var VendId = $.localStorage("VendId");
    $.ajax({

        type: "GET",
        url: '/api/property/BindRoomPolicy',
        data: { PropId: PropID, VendId: VendId, RoomId: RoomId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

         
            propertyViewModel.Policies.removeAll();

            for (var i = 0; i < data.length; i++) {
                //find out in facilities whether data existi..
                propertyViewModel.Policies.push(new PolicyClass(data[i]));
            }//Put the response in ObservableArray
            ko.applyBindings(propertyViewModel.Policies, document.getElementById("tab_1_3"));

            $(".PolicyDT").DataTable({ responsive: true });

        },
        error: function (err) {
            //alert(err.status + " : " + err.statusText);

        }

    });
    AppCommonScript.HideWaitBlock();
    this.deletePolicy = function () {
        if (confirm('Are you sure you want to delete this?')) {

            AppCommonScript.ShowWaitBlock();
            $.ajax({
                url: '/api/property/DeletePolicy',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON(this),

                contentType: 'application/json',
                success: function (result) {
                    AppCommonScript.HideWaitBlock();
                    AppCommonScript.showNotify(result);
                    getPolicy();
                },
                error: function (err) {
                    if (err.responseText == "Creation Failed") {
                        AppCommonScript.HideWaitBlock();
                        AppCommonScript.showNotify("Creation Failed");
                    }
                    else {
                        AppCommonScript.HideWaitBlock();
                        AppCommonScript.showNotify(err);

                    }
                },
                complete: function () {
                }
            });
        }
    }
    this.EditPolicy = function (data) {
        propertyViewModel.Policy_Id(data.Policy_Id)
        propertyViewModel.Policy_Name(data.Policy_Name);

        propertyViewModel.Policy_descr(data.Policy_descr);

    }
    this.ResetFormPolicy = function () {

        propertyViewModel.Policy_Name("");
        propertyViewModel.Policy_Id("");
        propertyViewModel.Policy_descr("");


    }
}

function CreatePolicy() {
 
    AppCommonScript.ShowWaitBlock();
    var Policy = new InitializePolicy();
    $.ajax({
        type: "POST",
        url: "/api/property/createRoomPolicy",
        data: $.parseJSON(ko.toJSON(Policy)),
        dataType: "json",
        success: function (response) {
      
            var result = { Status: true, ReturnMessage: { ReturnMessage: "Policy added successfully " }, ErrorType: "Success" };
            Successed(result);
            //    window.location.reload()
            getPolicy()
            $('#myModalPolicy').click('hidden.bs.modal', function () {
                //window.alert('hidden event fired!');
            });
            ResetFormPolicy();
            //getFacility();
        },
        error: function (jqxhr) {
            Failed(JSON.parse(jqxhr.responseText));
        }
    });
}
function PolicyClass(data) {
    var policy = this;
    policy.Policy_Id = data["Policy_Id"];
    policy.Policy_Name = data["Policy_Name"];
    var PD = data["Policy_descr"];
    policy.Policy_descr = data["Policy_descr"];

    policy.Policy_descrEdit = PD.replace(/\n/g, '<br/>');


}
function InitializePolicy() {
    var policy = new function () { };
    var Vid = $.localStorage("VendId");
  
    policy.Vndr_Id = Vid
    policy.Policy_Name = propertyViewModel.Policy_Name();
    policy.Prop_Id = PropID
    policy.Room_Id = RoomID
    policy.Policy_Id = propertyViewModel.Policy_Id();
    policy.Policy_descr = propertyViewModel.Policy_descr();
    policy.Policy_descrEdit = propertyViewModel.Policy_descrEdit();


    return policy
}



function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}
























//$(function () {
//    Policies: ko.observableArray([]),
//    //ko.applyBindings(RoomyListVM);
//    PoliciesListVM.getPolicy();
//    ko.applyBindings(PoliciesListVM, document.getElementById('tab_1_3'));
//    $('#updatepolicy').hide();

//});
////View Model
//var PoliciesListVM = {

//    Policies: ko.observableArray([]),
    
//    getPolicy: function () {
//       // 
//        var self = this;
        
//        $.ajax({
//            type: "GET",
//            url: '/api/rooms/BindPolicy',
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            data: { Prop_Id: '132', Vendor_Id: VendorId },
//            success: function (data) {
//                PoliciesListVM.Policies.removeAll();
//                for (var i = 0; i <= data.length; i++)
//                    PoliciesListVM.Policies.push(new PolicyClass(data[i])); //Put the response in ObservableArray
//            },
//            error: function (err) {
//                alert(err.status + " : " + err.statusText);

//            }
//        });
//    },

//    //Edit Policy
   
    
    

//};

////View Model 1
//var PoliciesListVM1 = {

//    Policies1: ko.observableArray([]),

//    getpolicy1: function (Policy_Id) {
//        
        
//        $.ajax({
//            type: "GET",
//            url: '/api/rooms/Editpolicy',           
//            data: { Id: Policy_Id },
//            //  data: { Id: array[3] },
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                
//                for (var i = 0; i < data.length; i++) {

//                    PoliciesListVM1.Policies1.push(new PolicyClass(data[i])); //Put the response in ObservableArray
                    
//                }

//            },           
//            error: function (err) {
//                alert(err.status + " : " + err.statusText);

//            }
//        });
//    },
//};



//EditPolicy = function (policy) {
    
//    PoliciesListVM1.getpolicy1(policy.Policy_Id);    
//    ko.applyBindings(PoliciesListVM1, document.getElementById('updatepolicy'));
//    $('#updatepolicy').show();
//    $('#tab3').hide();
//};





////Update Policies
//updatepolicies = function () {
//    
//    AppCommonScript.ShowWaitBlock();
//    $.ajax({
//        url: '/api/rooms/Updatepolicy',
//        type: 'post',
//        dataType: 'json',
//        data: ko.toJSON(this),

//        contentType: 'application/json',
//        success: function (result) {
//            AppCommonScript.HideWaitBlock();
//            AppCommonScript.showNotify(result);
//            $('#tab3').show();
//            $("#tab3").load();
//            PoliciesListVM.getPolicy();
//            ko.applyBindings(PoliciesListVM, document.getElementById('tab3'));
//        },
//        error: function (err) {
//            if (err.responseText == "Creation Failed") {
//                AppCommonScript.HideWaitBlock();
//                AppCommonScript.showNotify(err);
//            }
//            else {
//                AppCommonScript.HideWaitBlock();
//                AppCommonScript.showNotify(err);

//            }
//        },
//        complete: function () {

//        }

//    });
  
//    $('#updatepolicy').hide();
//    $(document).ready(function () {
//        setTimeout(function () {
//            PoliciesListVM.getPolicy();
//            ko.applyBindings(PoliciesListVM, document.getElementById('Grid3'));
//        }, 8000);
       
//    });

//    PoliciesListVM.getPolicy().ready;
//};

////Suspend Policies
//var PolicySuspend = {

//    ActiveSuspend: function (data) {
//        $.ajax({
//            type: "POST",
//            url: '/api/rooms/Suspendpolicy',
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            data: ko.toJSON(data),
//            success: function (response) {
               

//                ko.applyBindings(PoliciesListVM.getPolicy(), document.getElementById('tab3'));

//            },
//            error: function (err) {
//                alert(err.status + " : " + err.statusText);
//            }
//        });

//    }
//};


//self.SuspendPolicy = function (room) {
//    if (confirm('Are you sure ?')) {
//        PolicySuspend.ActiveSuspend(room)
//    }
   
//};




////Model
//function PolicyClass(data) {
//    var policy = this;

//        policy.Policy_Id = data.Policy_Id;
//        policy.Policy_Name = ko.observable(data.Policy_Name);
//        policy.Policy_Descr = ko.observable(data.Policy_Descr);
        

//}
    


