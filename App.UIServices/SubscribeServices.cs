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
    public class SubscribeServices : RepositoryBase, ISubscribeServices
    {
        public SubscribeServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        // For Subscription

        public TransactionStatus AddSubscribe(SubscribeBo subscribeBo)
        {
            var transactionStatus = new TransactionStatus();
            var subscirbe = BuiltSubscribeDomain(subscribeBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@Email", subscirbe.Email),//0
                 new SqlParameter("@Date", System.DateTime.Now),//2
                 new SqlParameter("@Ipaddress", subscirbe.Ipaddress),//3
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//4
			};

            Params[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddSubscribe", Params);
            int isExist = Convert.ToInt32(Params[3].Value);
            if (isExist == 0)
            {
                transactionStatus.Status = false;
            }
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }



        private Subscribe BuiltSubscribeDomain(SubscribeBo subscribeBo)
        {
            return (Subscribe)new Subscribe().InjectFrom(subscribeBo);
        }

        private SubscribeBo BuiltLoyaltyBo(Subscribe subscribe)
        {
            return (SubscribeBo)new SubscribeBo().InjectFrom(subscribe);
        }
    }
}
