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
    public class SystemProfileServices : RepositoryBase, ISystemProfileServices
    {
        public SystemProfileServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public String AddSystemProfile(SystemProfileBo systemprofileBo)
        {
            var transactionStatus = new TransactionStatus();
            var systemprofile = BuiltSystemProfileDomain(systemprofileBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@company_Title", systemprofile.Company_Title),//0
           		 new SqlParameter("@owned_By", systemprofile.Owned_By),//1
                 new SqlParameter("@cin_Number", systemprofile.CIN_Number),//2
                 new SqlParameter("@adr1", systemprofile.Adr1),//3
                 new SqlParameter("@adr2", systemprofile.Adr2),//4
                 new SqlParameter("@location", systemprofile.Location),//5
                 new SqlParameter("@city", systemprofile.City),//6
                 new SqlParameter("@tin_id", systemprofile.Tin_id),//7
                 new SqlParameter("@phone", systemprofile.Phone),//8
                 new SqlParameter("@mobile", systemprofile.Mobile),//9
                 new SqlParameter("@email", systemprofile.Email),//10
                 new SqlParameter("@sms_Url", systemprofile.Sms_Url),//11
                 new SqlParameter("@user_Id", systemprofile.User_Id),//112
                 new SqlParameter("@UserProfile_Id", systemprofile.UserProfile_Id),//13   
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//14
         
			};

            Params[14].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddSystemProfile", Params);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public TransactionStatus UpdateSystemProfile(SystemProfileBo systemprofileBo)
        {
            var transactionStatus = new TransactionStatus();
            var systemprofile = BuiltSystemProfileDomain(systemprofileBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@id", systemprofile.Id),//0
                 new SqlParameter("@company_Title", systemprofile.Company_Title),//1
           		 new SqlParameter("@owned_By", systemprofile.Owned_By),//2
                 new SqlParameter("@cin_Number", systemprofile.CIN_Number),//3
                 new SqlParameter("@adr1", systemprofile.Adr1),//4
                 new SqlParameter("@adr2", systemprofile.Adr2),//5
                 new SqlParameter("@location", systemprofile.Location),//6
                 new SqlParameter("@city", systemprofile.City),//7
                 new SqlParameter("@tin_id", systemprofile.Tin_id),//8
                 new SqlParameter("@phone", systemprofile.Phone),//9
                 new SqlParameter("@mobile", systemprofile.Mobile),//10
                 new SqlParameter("@email", systemprofile.Email),//11
                 new SqlParameter("@sms_Url", systemprofile.Sms_Url),//12
                 new SqlParameter("@user_Id", systemprofile.User_Id),//13
                   new SqlParameter("@EmailMaster", systemprofile.SetupEmail),//14
                     new SqlParameter("@Pwd", systemprofile.Password),//15
                       new SqlParameter("@smtp", systemprofile.SMTP),//16
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//17
			};

            Params[17].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateSystemProfile", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }

        public DataSet GetProfileById(int profile_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter param = new SqlParameter("@id", profile_Id);
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectSystemProfileById", param);
            return ds;
        }

        private SystemProfile BuiltSystemProfileDomain(SystemProfileBo systemprofileBo)
        {
            return (SystemProfile)new SystemProfile().InjectFrom(systemprofileBo);
        }

        private SystemProfileBo BuiltSystemProfileBo(SystemProfile systemprofile)
        {
            return (SystemProfileBo)new SystemProfileBo().InjectFrom(systemprofile);
        }
    }
}
