using System;

namespace App.BusinessObject
{
    public class PromotionBo
    {
        public int Promo_Id { get; set; }
        public string Promo_Code { get; set; }
        public string Promo_descr { get; set; }
        public string Promo_Type { get; set; }
        public int Prop_Value { get; set; }
        public DateTime Promo_Start { get; set; }
        public DateTime Promo_End { get; set; }
        public string Promo_Active_flag { get; set; }
    }
}
