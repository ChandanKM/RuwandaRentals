using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BusinessObject;
using App.Common;
using App.DataAccess;
using App.DataAccess.Interfaces;
using App.Domain;
using Omu.ValueInjecter;


namespace App.UIServices
{
    public class LocationService : RepositoryBase, ILocationService
    {
        public LocationService(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

        public TransactionStatus CreateLocation(LocationBo locationbo)
        {
            var transactionStatus = new TransactionStatus();

            var con = new CemexDb();
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@Location_desc",locationbo.Location_desc),//0
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//1
			};
            Params[1].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddLocation", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }

        public TransactionStatus EditLocation(EditLocationBo editlocationBo)
        {
            var transactionStatus = new TransactionStatus();
            var editlocation = BuiltEditPincodeDomain(editlocationBo);

            var con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateLocation", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Location_Id", Convert.ToInt32(editlocation.Location_Id));
            cmd.Parameters.AddWithValue("@Location_desc", editlocation.Location_desc);
            cmd.Parameters.AddWithValue("@opReturnValue", 1);
            cmd.ExecuteNonQuery();

            return transactionStatus;
        }



        public List<object> BindLocation()
        {
            var con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllLocation");
            var lstlocation = new List<Object>();
            while (reader.Read())
            {
                lstlocation.Add(

                    new
                    {
                        Location_Id = reader["Location_Id"].ToString(),
                        Location_desc = reader["Location_desc"].ToString()
                    });
            }

            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstlocation;
        }

        public List<object> Edit(string id)
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectLocationById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Location_Id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstlocation = new List<Object>();
            while (reader.Read())
            {
                lstlocation.Add(

                    new
                    {
                        Location_Id = reader["Location_Id"].ToString(),
                        Location_desc = reader["Location_desc"].ToString(),

                    });


            }
            conn.Close();


            return lstlocation;
        }

        private Lmk_Location BuiltLocationDomain(LocationBo locationBo)
        {
            return (Lmk_Location)new Lmk_Location().InjectFrom(locationBo);
        }

        private EditLocationBo BuiltEditPincodeDomain(EditLocationBo editpincodeBo)
        {
            return (EditLocationBo)new EditLocationBo().InjectFrom(editpincodeBo);
        }
    }
}
