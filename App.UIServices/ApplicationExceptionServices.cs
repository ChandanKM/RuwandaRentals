using System;
using System.Collections.Generic;
using System.Linq;
using App.BusinessObject;
using App.Common;
using App.DataAccess;
using App.DataAccess.Interfaces;
using App.Domain;
using Omu.ValueInjecter;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using App.UIServices.InterfaceServices;
namespace App.UIServices
{
    public class ApplicationExceptionServices : RepositoryBase, IApplicationExceptionServices
    {
        public ApplicationExceptionServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public TransactionStatus SaveException(ApplicationErrorLogBo appErrorLogBo)
        {
            var transactionStatus = new TransactionStatus();
            var appErrorLog = BuiltApplicationErrorLogDomain(appErrorLogBo);
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_AddApplicationErrors", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@error", appErrorLog.Error);
            cmd.Parameters.AddWithValue("@stackTrace", appErrorLog.Stacktrace);
            cmd.Parameters.AddWithValue("@innerException", appErrorLog.InnerException);
            cmd.Parameters.AddWithValue("@source", appErrorLog.Source);

            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        private ApplicationErrorLog BuiltApplicationErrorLogDomain(ApplicationErrorLogBo appErrorLogBo)
        {
            return (ApplicationErrorLog)new ApplicationErrorLog().InjectFrom(appErrorLogBo);
        }

    }
}
