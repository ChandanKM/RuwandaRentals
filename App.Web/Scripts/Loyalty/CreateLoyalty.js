$(document).ready(function () {
    InitializeLoyaltyViewModel();
});

function LoyaltyViewModel() {
    var self = this;
    self.Loyal_Id = ko.observable('1');
    self.Loyal_Desc = ko.observable();
    self.Loyal_Max_Allowed = ko.observable();
    self.Loyal_Min_redmpt = ko.observable();
    self.Loyal_Max_redmpt = ko.observable();
    self.Loyal_Start_On = ko.observable();
    self.Loyal_End_On = ko.observable();
    self.Loyal_Set_Up = ko.observable();
    self.Loyal_Checked_By = ko.observable();
    self.Loyal_Approved_By = ko.observable();
    self.Loyal_Active_flag = ko.observable('true');
}


function InitializeLoyaltyViewModel() {
    loyaltyVM = new LoyaltyViewModel();
    ko.applyBindings(loyaltyVM, document.getElementById("divCreateLoyalty"));
}

function CreateLoyalty() {
    var loyalty = new InitializeLoyalty();
    
    $.ajax({
        type: "POST",
        url: "/api/Loyalty/create",
        data: $.parseJSON(ko.toJSON(loyalty)),
        dataType: "json",
        success: function (response) {
            Successed(response);
        },
        error: function (err) {
            Failed(err);
        }
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

function InitializeLoyalty() {
    var loyaltyViewMOdel = new function () { };
    loyaltyViewMOdel.Loyal_Id = loyaltyVM.Loyal_Id();
    loyaltyViewMOdel.Loyal_Desc = loyaltyVM.Loyal_Desc();
    loyaltyViewMOdel.Loyal_Max_Allowed = loyaltyVM.Loyal_Max_Allowed();
    loyaltyViewMOdel.Loyal_Max_redmpt = loyaltyVM.Loyal_Max_redmpt();
    loyaltyViewMOdel.Loyal_Min_redmpt = loyaltyVM.Loyal_Min_redmpt();
    loyaltyViewMOdel.Loyal_Start_On = loyaltyVM.Loyal_Start_On();
    loyaltyViewMOdel.Loyal_End_On = loyaltyVM.Loyal_End_On();
    loyaltyViewMOdel.Loyal_Set_Up = loyaltyVM.Loyal_Set_Up();
    loyaltyViewMOdel.Loyal_Checked_By = loyaltyVM.Loyal_Checked_By();
    loyaltyViewMOdel.Loyal_Approved_By = loyaltyVM.Loyal_Approved_By();
    loyaltyViewMOdel.Loyal_Active_flag = loyaltyVM.Loyal_Active_flag();
    return loyaltyViewMOdel;
}


