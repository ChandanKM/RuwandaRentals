using System;
using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface ILoyaltyServices
    {
        TransactionStatus AddLoyalty(LoyaltyBo loyaltyBo);
        TransactionStatus SuspendLoyalty(int loyalty_Id);
        TransactionStatus ActiveLoyalty(int loyalty_Id);
        DataSet Bind();
    }
}
