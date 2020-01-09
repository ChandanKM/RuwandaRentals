
$(function () {

    corporate = new CorporateUserViewModel();
    corporate.GetAllCorporateCompanies();
    corporate.GetCorporateUserInfo();
    ko.applyBindings(corporate, document.getElementById("CorporateUser"));


})


function CorporateUserViewModel() {

    var self = this;
    self.CorporateUserArray = ko.observableArray();
    self.CorporateCompanyArray = ko.observableArray([{ Id: 'null', CompanyName: 'Select Corporate' }]);
    self.selectedCompany = ko.observable();
    self.selectedCompany(null);

    self.GetCorporateUserInfo = function () {
        AppCommonScript.ShowWaitBlock();

        $.getJSON("/api/Corporate/GetAllCorporateUserByCompany?CompanyName=" + self.selectedCompany(), function (CorpUser) {

            self.CorporateUserArray.removeAll();
            var table = $(".CorporateUser").dataTable();
            table.fnDestroy();
            $('.CorporateUser tbody').empty();

            for (var i = 0; i < CorpUser.length; i++) {
                self.CorporateUserArray.push(new CorporateUserClass(CorpUser[i]));
            }


            $('.CorporateUser').dataTable({ 'iDisplayLength': 10 });

            AppCommonScript.HideWaitBlock();

        });


    }

    self.GetAllCorporateCompanies = function () {

        $.getJSON("/api/Corporate/GetAllCorporateCompanies", function (CorpCompany) {

            ko.utils.arrayMap(CorpCompany, function (Company) {
                self.CorporateCompanyArray.push({ Id: Company, CompanyName: Company });

            });
        });

    }

    self.ChangeToAdmin = function (item) {



        var result = confirm("Are you Sure!")
        if (result == true) {
            $.getJSON("/api/Corporate/UpdateCorporateUserToAdmin?CorpEmail=" + item.MailId + "&CorpCompany=" + item.Company, function (CorpUser) {

                
                var TdElement = $(event.target).closest('td');
                TdElement.empty();
                TdElement.append('<label class="labelmsg">Admin</label>');
                window.location.reload();
            });

        }

    }
}




function CorporateUserClass(data) {
    var Corp = this;
    Corp.FirstName = data["First_Name"];
    Corp.LastName = data["Last_Name"];
    Corp.MailId = data["mailid"];
    Corp.Mobile = data["Mobile"];
    Corp.Company = data["Company"];
    Corp.isAdmin = data["isAdmin"];

}

