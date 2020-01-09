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

namespace App.UIServices
{
    public class ApplicationErrorLogServices : RepositoryBase
    {
        public ApplicationErrorLogServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public static void SaveException(ApplicationErrorLog appErrorLog)
        {
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
        }

        public static void AppException(Exception ex)
        {
            ApplicationErrorLog appObj = new ApplicationErrorLog();
            appObj.Error = ex.Message;
            appObj.InnerException = Convert.ToString(ex.InnerException);
            appObj.Source = Convert.ToString(ex.Source);
            appObj.Stacktrace = Convert.ToString(ex.StackTrace);
            SaveException(appObj);
        }

        private ApplicationErrorLog BuiltApplicationErrorLogDomain(ApplicationErrorLogBo appErrorLogBo)
        {
            return (ApplicationErrorLog)new ApplicationErrorLog().InjectFrom(appErrorLogBo);
        }

        private ApplicationErrorLogBo BuiltApplicationErrorLogBo(ApplicationErrorLog appErrorLog)
        {
            return (ApplicationErrorLogBo)new ApplicationErrorLogBo().InjectFrom(appErrorLog);
        }

    }
}
