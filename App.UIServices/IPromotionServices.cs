using System;
using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IPromotionServices
    {
        TransactionStatus AddPromotion(PromotionBo promotionBo);
        TransactionStatus SuspendPromotion(int promotion_Id);
        TransactionStatus ActivePromotion(int promotion_Id);
        DataSet Bind();
    }
}
