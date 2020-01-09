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
    public class LoyaltyServices : RepositoryBase, ILoyaltyServices
    {
        public LoyaltyServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        // For Loyalty
        public TransactionStatus AddLoyalty(LoyaltyBo loyaltyBo)
        {
            var transactionStatus = new TransactionStatus();
            var loyalty = BuiltLoyaltyDomain(loyaltyBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@Loyal_Desc", loyalty.Loyal_Desc),//0
                 new SqlParameter("@Loyal_Max_Allowed", loyalty.Loyal_Max_Allowed),//1
                 new SqlParameter("@Loyal_Min_redmpt", loyalty.Loyal_Min_redmpt),//2
                 new SqlParameter("@Loyal_Max_redmpt", loyalty.Loyal_Max_redmpt),//3
                 new SqlParameter("@Loyal_Start_On", loyalty.Loyal_Start_On),//4
                 new SqlParameter("@Loyal_End_On", loyalty.Loyal_End_On),//5
                 new SqlParameter("@Loyal_Set_Up", loyalty.Loyal_Set_Up),//6
                 new SqlParameter("@Loyal_Checked_By", loyalty.Loyal_Checked_By),//7
                 new SqlParameter("@Loyal_Approved_By", loyalty.Loyal_Approved_By),//8
                 new SqlParameter("@Loyal_Active_flag", "true"),//9
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//10
			};

            Params[10].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddLoyalty", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }



        public DataSet Bind()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllLoyalty");
            return ds;
        }

      
        public TransactionStatus SuspendLoyalty(int loyalty_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateLoyaltyActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Loyal_Id", loyalty_Id);
            cmd.Parameters.AddWithValue("@Loyal_Active_flag", "false");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        public TransactionStatus ActiveLoyalty(int loyalty_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateLoyaltyActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Loyal_Id", loyalty_Id);
            cmd.Parameters.AddWithValue("@Loyal_Active_flag", "true");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        private Loyalty BuiltLoyaltyDomain(LoyaltyBo loyaltyBo)
        {
            return (Loyalty)new Loyalty().InjectFrom(loyaltyBo);
        }

        private LoyaltyBo BuiltLoyaltyBo(Loyalty loyalty)
        {
            return (LoyaltyBo)new LoyaltyBo().InjectFrom(loyalty);
        }
    }
}
