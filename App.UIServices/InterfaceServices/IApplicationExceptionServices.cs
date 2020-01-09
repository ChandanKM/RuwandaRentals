using System;
using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;


namespace App.UIServices.InterfaceServices
{
    public interface IApplicationExceptionServices
    {
        TransactionStatus SaveException(ApplicationErrorLogBo appErrorBo);
    }
}
