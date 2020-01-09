var urlPath = window.location.pathname;


//View Model
function BookingListVM() {
    
    var self = this;
    var Invc_Num = $.localStorage("Invce_Nums");
  
    self.Trans = ko.observableArray([]),
       self.TaxTrans = ko.observableArray([]),
     self.FacilityTrans = ko.observableArray([]),

        self.ImageTrans = ko.observableArray([]),
      self.PolicyTrans = ko.observableArray([]),
       self.FinePrintTrans = ko.observableArray([]),
          self.getBooking = function () {
              AppCommonScript.ShowWaitBlock();
              $.ajax({
                  type: "GET",
                  url: '/api/Consumer/GetTransaction',
                  contentType: "application/json; charset=utf-8",
                  data: { Invce_Num: Invc_Num },
                  dataType: "json",
                  success: function (data) {
                      
                      for (var i = 0; i < data.Table.length; i++) {
                          self.Trans.push(new TransClass(data.Table[i])); //Put the response in ObservableArray
                      }
                   
                      for (var i = 0; i < data.Table2.length; i++) {
                          self.FacilityTrans.push(new TransFacilityClass(data.Table2[i])); //Put the response in ObservableArray
                      }
                      for (var i = 0; i < data.Table1.length; i++) {
                          self.TaxTrans.push(new TransTaxClass(data.Table1[i])); //Put the response in ObservableArray
                      }
                      for (var i = 0; i < data.Table3.length; i++) {
                          self.ImageTrans.push(new TransImageClass(data.Table3[i])); //Put the response in ObservableArray
                      }
                      for (var i = 0; i < data.Table4.length; i++) {
                          self.PolicyTrans.push(new TransPolicyClass(data.Table4[i])); //Put the response in ObservableArray
                      }
                      for (var i = 0; i < data.Table5.length; i++) {
                          self.FinePrintTrans.push(new TransFinePrintClass(data.Table5[i])); //Put the response in ObservableArray
                      }
                      ko.applyBindings(AllBooking, document.getElementById("Printdiv"));
                      //ko.applyBindings(AllBooking, document.getElementById("trTax"));
                      //ko.applyBindings(AllBooking, document.getElementById("Booking"));
                      //ko.applyBindings(AllBooking, document.getElementById("inovoice-img"));
                      ////ko.applyBindings(AllBooking, document.getElementById("Inv"));
                      //ko.applyBindings(AllBooking, document.getElementById("Consumer"));
                      //ko.applyBindings(AllBooking, document.getElementById("TotPrice"));
                      //ko.applyBindings(AllBooking, document.getElementById("Rate"));
                      //ko.applyBindings(AllBooking, document.getElementById("Occupant"));
                      //ko.applyBindings(AllBooking, document.getElementById("tblFacility"));
                      //ko.applyBindings(AllBooking, document.getElementById("Inv"));
                      //ko.applyBindings(AllBooking, document.getElementById("invoice_details"));
                      //ko.applyBindings(AllBooking, document.getElementById("FinePrint"));
                      AppCommonScript.HideWaitBlock();
                  },
                  error: function (err) {
                      //  alert(err.status + " : " + err.statusText);

                  }
              });
          }


}


//Model
function TransClass(data) {
    var prop = this;

    prop.Cons_First_Name = data["Cons_First_Name"];
    prop.Prop_Addr1 = data["Prop_Addr1"];
    prop.Days_Count = data["Days_Count"];
    prop.Room_Count = data["Room_Count"];
    prop.camo_room_rate = data["camo_room_rate"];
    prop.tot = data["tot"];
    prop.tax_amnt = data["tax_amnt"];

    prop.tot = prop.camo_room_rate * (prop.Room_Count * prop.Days_Count);

    prop.Prop_Name = data["Prop_Name"];
    prop.Prop_Address = data["Prop_Addr1"];
    prop.CIN ='CIN# '+ data["Prop_Cin_No"];
    prop.Name = data["Cons_First_Name"];
    //prop.Image_dir = data["Image_dir"];
    prop.Room_Name = data["Room_Name"];
    prop.Invce_Num = data["Invce_Num"];
    var invdate = new Date(data["Invce_date"]).toDateString();
    prop.invce_date = invdate;
    prop.Image_dir= data["Image_dir"];
    var date = new Date(data["Checkin"]).toDateString();
    prop.Checkin = date;
    var dateout = new Date(data["Checkout"]).toDateString();
    prop.Checkout = dateout;
    //prop.Facilities = ko.observable('');
    prop.GuestName =data["GuestName"];
    prop.Email =data["Cons_mailid"];
    prop.Mobile =data["Cons_Mobile"];
    prop.Camo_Rate = parseFloat(data["camo_room_rate"]).toFixed(2);
    //prop.Luxury_Tax = ko.observable('1');
    //prop.Service_Tax = ko.observable('1');
    prop.Total = data["net_amt"];



}
function TransTaxClass(data) {
    var prop = this;
  
    prop.TaxType = data["Vparam_Descr"]+' '  +data["Vparam_Val"]+'%';
    prop.TaxAmnt = data["tax_amnt"];
   



}
function TransFacilityClass(data) {
    var facil = this;

   
    facil.Facilities = data["Facility_Name"]+',';
  


}

function TransImageClass(data) {
    
    var prop = this;

   
    prop.Image_dir = data["roomimage"];
  



}
function TransPolicyClass(data) {
    var prop = this;
    

    prop.Policy_Name = data["Policy_Name"];
    prop.Policy_Descr = data["Policy_Descr"];




}
function TransFinePrintClass(data) {
    var prop = this;
    

    prop.Invce_fineprint = data["Invce_fineprint"];
 




}
$(document).ready(function () {

    AllBooking = new BookingListVM();

    AllBooking.getBooking();

});