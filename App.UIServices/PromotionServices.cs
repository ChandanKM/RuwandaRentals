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
    public class PromotionServices : RepositoryBase, IPromotionServices
    {
        public PromotionServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        // For Loyalty
        public TransactionStatus AddPromotion(PromotionBo promotiontyBo)
        {
            var transactionStatus = new TransactionStatus();
            var promotion = BuiltPromotionDomain(promotiontyBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 
                 new SqlParameter("@Promo_Code", promotion.Promo_Code),//0
                 new SqlParameter("@Promo_descr", promotion.Promo_descr),//1
                 new SqlParameter("@Promo_Type", promotion.Promo_Type),//2
                 new SqlParameter("@Prop_Value", promotion.Prop_Value),//3
                 new SqlParameter("@Promo_Start", promotion.Promo_Start),//4
                 new SqlParameter("@Promo_End", promotion.Promo_End),//5
                 new SqlParameter("@Promo_Active_flag", "true"),//6
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//7
			};

            Params[7].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPromotion", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }



        public DataSet Bind()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllPromotion");
            return ds;
        }

        public TransactionStatus SuspendPromotion(int promotion_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdatePromotionActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Promo_Id", promotion_Id);
            cmd.Parameters.AddWithValue("@Promo_Active_flag", "false");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        public TransactionStatus ActivePromotion(int promotion_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdatePromotionActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Promo_Id", promotion_Id);
            cmd.Parameters.AddWithValue("@Promo_Active_flag", "true");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        private Promotion BuiltPromotionDomain(PromotionBo promotionBo)
        {
            return (Promotion)new Promotion().InjectFrom(promotionBo);
        }

        private PromotionBo BuiltPromotionBo(Promotion promotion)
        {
            return (PromotionBo)new LoyaltyBo().InjectFrom(promotion);
        }
    }
}
