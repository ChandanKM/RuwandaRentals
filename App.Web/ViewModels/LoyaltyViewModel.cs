using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class LoyaltyViewModel : TransactionStatus
    {
        public int Loyal_Id { get; set; }
        public string Loyal_Desc { get; set; }
        public int Loyal_Max_Allowed { get; set; }
        public int Loyal_Min_redmpt { get; set; }
        public int Loyal_Max_redmpt { get; set; }
        public DateTime Loyal_Start_On { get; set; }
        public DateTime Loyal_End_On { get; set; }
        public string Loyal_Set_Up { get; set; }
        public string Loyal_Checked_By { get; set; }
        public string Loyal_Approved_By { get; set; }
        public string Loyal_Active_flag { get; set; }
    }
}