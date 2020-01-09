using App.BusinessObject;
using App.Common;
using App.DataAccess;
using App.DataAccess.Interfaces;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace App.UIServices
{
    public class CCAvenueServices : RepositoryBase, ICCAvenueServices
    {
        public CCAvenueServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        // For Room Type
        public DataSet AddCCAvenue(CCAvenueBo ccavenueBo)
        {
            var transactionStatus = new TransactionStatus();
            var ccavenue = BuiltRoomTypeDomain(ccavenueBo);



            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@Cav_Name", ccavenue.Cav_Name),//0
                 new SqlParameter("@Cav_Percent", ccavenue.Cav_Percent),//1
                 new SqlParameter("@Cav_Descr", ccavenue.Cav_Descr),//2
                 new SqlParameter("@Cav_Ipaddress", ccavenue.Cav_Ipaddress),//3
                 new SqlParameter("@Cav_Regist_On", ccavenue.Cav_Regist_On),//4
                 new SqlParameter("@Cav_Modified_On", ccavenue.Cav_Modified_On),//5
                 new SqlParameter("@Cav_Active_flag", "True"),//6
                 new SqlParameter("@opReturnValue", "0")//7
			};

            //   Params[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddCCAvenue", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            //string test = Params[3].Value.ToString();

            return ds;
        }

        public TransactionStatus EditCCAvenue(CCAvenueBo ccavenueBo)
        {
            var transactionStatus = new TransactionStatus();
            var ccavenue = BuiltRoomTypeDomain(ccavenueBo);


            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_updateCCAvenue", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cav_Id", Convert.ToInt32(ccavenue.Cav_Id));
            cmd.Parameters.AddWithValue("@Cav_Name", ccavenue.Cav_Name);
            cmd.Parameters.AddWithValue("@Cav_Percent", ccavenue.Cav_Percent);
            cmd.Parameters.AddWithValue("@Cav_Ipaddress", ccavenue.Cav_Ipaddress);
            cmd.Parameters.AddWithValue("@Cav_Descr", ccavenue.Cav_Descr);
            cmd.Parameters.AddWithValue("@Cav_Modified_On", ccavenue.Cav_Modified_On);
            cmd.Parameters.AddWithValue("@Cav_Active_flag", "true");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);

            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
              
        public List<Object> Bind()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectAll_CCAvenue", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> roomList = new List<Object>();
            while (reader.Read())
            {
                roomList.Add(

                    new
                    {
                        Cav_Id = reader["Cav_Id"].ToString(),
                        Cav_Name = reader["Cav_Name"].ToString(),
                        Cav_Percent = reader["Cav_Percent"].ToString(),
                        Cav_Descr = reader["Cav_Descr"].ToString(),
                        Cav_Active_flag = reader["Cav_Active_flag"].ToString(),

                    });
            }
            conn.Close();


            return roomList;
        }

        public DataSet GetCCAvenueById(int roomtype_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter param = new SqlParameter("@Cav_Id", roomtype_Id);
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Select_CCAvenue_id", param);
            return ds;
        }
                
        private CCAvenue BuiltRoomTypeDomain(CCAvenueBo ccavenueBo)
        {
            return (CCAvenue)new CCAvenue().InjectFrom(ccavenueBo);
        }

        private CCAvenueBo BuiltRoomTypeBo(CCAvenue ccavenueBo)
        {
            return (CCAvenueBo)new RoomTypeBo().InjectFrom(ccavenueBo);
        }        
    }
}
