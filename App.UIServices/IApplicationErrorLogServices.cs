using System;
using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IApplicationErrorLogServices
    {
        void SaveException(ApplicationErrorLog applicationerror);
        void AppException(Exception exception);
    }
}
