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
    public class ParamServices : RepositoryBase, IParamServices
    {
        public ParamServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public DataSet AddParam(ParamBo paramBo)
        {
            var transactionStatus = new TransactionStatus();
            var param = BuiltParamDomain(paramBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@vend_Id", 4),//0
                 new SqlParameter("@prop_Id", 1),//1
                 new SqlParameter("@room_Id", 1),//2
                 new SqlParameter("@lmk_Id", 1),//3
                 new SqlParameter("@user_Id", 1),//4
                 new SqlParameter("@Vparam_Code", param.Vparam_Code),//5
                 new SqlParameter("@Vparam_Descr", param.Vparam_Descr),//6
                 new SqlParameter("@Vparm_Type", param.Vparm_Type),//7
                 new SqlParameter("@Vparam_Val", param.Vparam_Val),//8
                 new SqlParameter("@Vparam_Active_Flag", "True"),//9
               
              //   new SqlParameter("@opReturnValue", SqlDbType.Int)//10
			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddParam", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return ds;
        }

        public TransactionStatus UpdateParam(ParamBo paramBo)
        {
            var transactionStatus = new TransactionStatus();
            var param = BuiltParamDomain(paramBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@param_Id", param.Id),//0
                 new SqlParameter("@vend_Id", 4),//1
                 new SqlParameter("@Vparam_Code", param.Vparam_Code),//2
                 new SqlParameter("@Vparam_Descr", param.Vparam_Descr),//2
           		 new SqlParameter("@Vparm_Type", param.Vparm_Type),//3   
                 new SqlParameter("@Vparam_Val", param.Vparam_Val),//4                  
               
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//5
			};

            Params[6].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateParam", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }

        public TransactionStatus SuspendParam(int param_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateParamActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", param_Id);
            cmd.Parameters.AddWithValue("@active_flag", "False");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        public DataSet GetParam(int propId, int authId)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@authorityId",authId),
                     new SqlParameter("@prop_Id",propId),
         	};
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Select_Parametersby_Id", Params);
            return ds;
        }

        public TransactionStatus UpdateParamValue(int Id, string value)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@Id", Id),//0        
                     new SqlParameter("@value",value)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateParam_Value", Params);

            return transactionStatus;
        }

        public TransactionStatus UpdateParamHiddenGems(int Id, string value)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@Id", Id),//0        
                     new SqlParameter("@value",value)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_UpdateHiidenGems]", Params);

            return transactionStatus;
        }

        public TransactionStatus UpdateParamType(int Id, string type)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@Id", Id),//0        
                     new SqlParameter("@type",type)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateParam_Type", Params);

            return transactionStatus;
        }

        public TransactionStatus UpdateParamPermission(int Id, string flag)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@Id", Id),//0        
                     new SqlParameter("@flag",flag)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateParam_Permission", Params);

            return transactionStatus;
        }


        private Param BuiltParamDomain(ParamBo paramBo)
        {
            return (Param)new Param().InjectFrom(paramBo);
        }

        private ParamBo BuiltUserProfileBo(Param param)
        {
            return (ParamBo)new ParamBo().InjectFrom(param);
        }
    }
}
