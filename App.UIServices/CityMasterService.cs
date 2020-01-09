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

    public class CityMasterService : RepositoryBase, ICityMasterService
    {
        public CityMasterService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        //For City Master
        public TransactionStatus AddCityMaster(CityMasterBo cityMasterBo)
        {

            var transactionStatus = new TransactionStatus();
            var cityMaster = BuiltCityMasterDomain(cityMasterBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@City_Id", cityMaster.City_Id),//0
                new SqlParameter("@location_Id", cityMaster.location_Id),//1
                new SqlParameter("@Pincode_Id", cityMaster.Pincode_Id),//2
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//3
			};

            Params[3].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddCity_Master", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[3].Value.ToString();

            return transactionStatus;


        }

        public DataSet Bind()
        {
            CemexDb con = new CemexDb();

          
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllCity_and_Location");
            return ds;
        }
        private CityMaster BuiltCityMasterDomain(CityMasterBo cityMasterBo)
        {
            return (CityMaster)new CityMaster().InjectFrom(cityMasterBo);
        }
   
        private CityMasterBo BuiltCityMasterBo(CityMaster cityMaster)
        {
            return (CityMasterBo)new CityMasterBo().InjectFrom(cityMaster);
        }
     


    }

}
