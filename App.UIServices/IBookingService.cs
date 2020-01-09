using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
   public interface  IBookingService
    {
      
       //TransactionStatus AddCityMaster(CityMasterBo cityMaster);
       DataSet Bind(int VendID, string InvFrom, string InvTo);
       DataSet ConsumerReport(string VendID, string cons_id, string cons_name, string cons_mailid, string cons_mobile, string days);
       DataSet Tax_Report(string VendID, string TaxType, string days);
       DataSet CCAvenue_Report(string VendID, string days);
       DataSet LMK_Margin_Report(string VendID, string fromdate, string todate);
       DataSet UpcomingBookingList(int VendID);
       //DataSet CheckMargin(int Inv_Id);
       List<object> BindProperty();
       DataSet corpBookinglist(string Cons_Id, string bookingStatus, string InvFrom, string InvTo);

     
    }

}
