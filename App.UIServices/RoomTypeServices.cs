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
using System.IO;
using System.Xml.Serialization;
using System.Text;


namespace App.UIServices
{
    public class RoomTypeServices : RepositoryBase, IRoomTypeServices
    {
        public RoomTypeServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        // For Room Type
        public DataSet AddRoomType(RoomTypeBo roomTypeBo)
        {
            var transactionStatus = new TransactionStatus();
            var roomType = BuiltRoomTypeDomain(roomTypeBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
           		 new SqlParameter("@Room_Name", roomType.Room_Name),//0
                 new SqlParameter("@Room_Descr", roomType.Room_Descr),//1
                 new SqlParameter("@Room_Active_flag", "True"),//2
              //   new SqlParameter("@opReturnValue", SqlDbType.Int)//3
			};

            //   Params[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddRoomType", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            //string test = Params[3].Value.ToString();

            return ds;
        }

        public TransactionStatus EditRoomType(RoomTypeBo roomTypeBo)
        {
            var transactionStatus = new TransactionStatus();
            var roomtype = BuiltRoomTypeDomain(roomTypeBo);


            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateRoomTypes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Room_TypeId", Convert.ToInt32(roomtype.Room_TypeId));
            cmd.Parameters.AddWithValue("@Room_Name", roomtype.Room_Name);
            cmd.Parameters.AddWithValue("@Room_Descr", roomtype.Room_Descr);
            cmd.Parameters.AddWithValue("@Room_Active_flag", "true");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);

            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
              
        public List<Object> Bind()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectAllRoomTypes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> roomList = new List<Object>();
            while (reader.Read())
            {
                roomList.Add(

                    new
                    {
                        Room_TypeId = reader["Room_TypeId"].ToString(),
                        Room_Name = reader["Room_Name"].ToString(),
                        Room_Descr = reader["Room_Descr"].ToString(),
                        Room_Active_flag = reader["Room_Active_flag"].ToString(),

                    });
            }
            conn.Close();


            return roomList;
        }
       
        public DataSet GetRoomTypeById(int roomtype_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter param = new SqlParameter("@Room_TypeId", roomtype_Id);
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectRoomTypesById", param);
            return ds;
        }

        public TransactionStatus SuspendRoomType(int roomtype_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateRoomTypesActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Room_TypeId", roomtype_Id);
            cmd.Parameters.AddWithValue("@Room_Active_flag", "False");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        public TransactionStatus ActiveRoomType(int roomtype_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateRoomTypesActive_flag", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Room_TypeId", roomtype_Id);
            cmd.Parameters.AddWithValue("@Room_Active_flag", "True");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        private RoomType BuiltRoomTypeDomain(RoomTypeBo roomTypeBo)
        {
            return (RoomType)new RoomType().InjectFrom(roomTypeBo);
        }

        private RoomTypeBo BuiltRoomTypeBo(RoomType roomType)
        {
            return (RoomTypeBo)new RoomTypeBo().InjectFrom(roomType);
        }


        public roomresponse GetRoomMap(Roomrequest roomrequest)
        {
            //var roomresponse = new List<roomtype>();
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_Staah_roommapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username",roomrequest.username );
            cmd.Parameters.AddWithValue("@password", roomrequest.password);
            cmd.Parameters.AddWithValue("@hotelid", roomrequest.hotel_id);
            cmd.Parameters.AddWithValue("@opReturnValue", 0);

            
            SqlDataReader reader = cmd.ExecuteReader();
            roomresponse roomresponse = new roomresponse();
            
            while (reader.Read())
            {
                roomresponse.roomtypes.Add(

                    new roomtype
                    {
                        roomtypeid =Convert.ToInt32(reader["Room_Id"]),
                        roomtypename = reader["Room_Name"].ToString(),
                        rateplanid = reader["Room_Id"].ToString(),
                        rateplanname = "BAR",

                    });
            }
            conn.Close();
            return roomresponse;
        }


        public TransactionStatus RoomInventry(request request)
        {
            CemexDb con = new CemexDb();
            var transactionStatus = new TransactionStatus();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_UpdateInventryRoom_Staah", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FileDetails", ToXML(request.dates));
            cmd.Parameters.AddWithValue("@Prop_Id", request.hotel_id);
            cmd.Parameters.AddWithValue("@Room_Id", request.room.id);
            cmd.Parameters.AddWithValue("@opReturnValue", 0);
            cmd.ExecuteNonQuery();
            return transactionStatus;
           
        }

        private string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value
                ns.Add("", "");
                xmlSerializer.Serialize(stringWriter, obj, ns);
                return stringWriter.ToString();
            }
        }



        public bool SetDataToEMCLog(string requestFrom, string requestTo, string requestBody, string status)
        {

            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.proc_AddEMCLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestFrom", requestFrom);
                cmd.Parameters.AddWithValue("@RequestTo", requestTo);
                cmd.Parameters.AddWithValue("@MessageBody", requestBody);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

    }
}
