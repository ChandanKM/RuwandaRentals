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
    public class ManageLocationServices : RepositoryBase, IManageLocationServices
    {
        public ManageLocationServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public DataSet AddLocation(ManageLocationBo locationBo)
        {
            var transactionStatus = new TransactionStatus();
            var location = BuiltManageLocationDomain(locationBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@city", location.City),//1
                 new SqlParameter("@location", location.Location),//2
                 new SqlParameter("@pincode", location.Pincode),//3
                 new SqlParameter("@state", location.State),//4
			};
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddCityLocation_Master", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return ds;
        }

        public TransactionStatus UpdateLocation(ManageLocationBo locationBo)
        {
            var transactionStatus = new TransactionStatus();
            var location = BuiltManageLocationDomain(locationBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@id", location.Id),//0
           		 new SqlParameter("@city", location.City),//1
                 new SqlParameter("@location", location.Location),//2
                 new SqlParameter("@pincode", location.Pincode),//3
                 new SqlParameter("@state", location.State),//4
                 new SqlParameter("@output", SqlDbType.Int),//4
			};

            Params[5].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateCityLocation_Master", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }

        public TransactionStatus Delete(int loc_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@id",loc_Id),//0 
                     new SqlParameter("@output", SqlDbType.Int)//1
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeleteCityLocation_Master", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }

        public DataSet GetAllLocation()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllCity_and_Location");
            return ds;
        }


        private ManageLocation BuiltManageLocationDomain(ManageLocationBo locationBo)
        {
            return (ManageLocation)new ManageLocation().InjectFrom(locationBo);
        }

        private ManageLocationBo BuiltManageLocationBo(ManageLocation location)
        {
            return (ManageLocationBo)new ManageLocationBo().InjectFrom(location);
        }
    }
}
