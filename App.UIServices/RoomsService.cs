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
    public class RoomsService : RepositoryBase, IRoomsService
    {
        public RoomsService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }



        //create Room
        public string GetRoomName(int TypeId)
        {
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@TypeId",TypeId),//0
			};
            string RoomName = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectRoomName", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                RoomName = reader["Room_Name"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return RoomName;
        }
        public List<string> GetAutoCompleteRoom(string terms)
        {

            List<string> lstroomtype = new List<string>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@term",terms),//0
              
			};


            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectRoomTypes]", Params);

            while (reader.Read())
            {
                lstroomtype.Add(reader["Room_Name"].ToString());

            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstroomtype;
        }

        public string CreateRooms(RoomsBo roomsBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                var rooms = BuiltRoomsDomain(roomsBo);

                var Room_Checkin = Convert.ToDateTime(rooms.Room_Checkin);
                var Room_Checkout = Convert.ToDateTime(rooms.Room_Checkout);
                var Room_Grace_time = Convert.ToDateTime(rooms.Room_Grace_time);


                if( rooms.Room_Extra_Adul==null)
                    rooms.Room_Extra_Adul="";
                string Room_Checkintime = Room_Checkin.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);
                string Room_Checkouttime = Room_Checkout.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);
                string Room_Grace_timetime = Room_Grace_time.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);

                string testing = "testingRoom";

                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string Flags = "true";
                int value = 1;
                int defaultvalue = 0;
                //if (rooms.Room_Standard_rate != "0" || rooms.Room_Standard_rate != "" || rooms.Room_Standard_rate != null)
                //{
                //    defaultvalue = Convert.ToInt32(rooms.Room_Standard_rate);
                //}
                //  string RoomName =GetRoomName(rooms.Type_Id);
                #region using  sql helper
                CemexDb con = new CemexDb();
                if (rooms.Image_dir == "null" || rooms.Image_dir == null)
                {
                    rooms.Image_dir = "/img/Room-image.png";
                }
                SqlParameter[] Params = 
            { 	
                new SqlParameter("@opReturnValue", value),//26
                   
                new SqlParameter("@Room_Name",rooms.Room_Name),//1 rooms.Room_Name
                new SqlParameter("@Room_Overview", rooms.Room_Overview),//2
                new SqlParameter("@Room_Adult_occup", defaultvalue),//3 rooms.Room_Adult_occup
                new SqlParameter("@Room_Child_occup", defaultvalue),//4 rooms.Room_Child_occup
                new SqlParameter("@Room_Extra_Adul", rooms.Room_Extra_Adul),//5 rooms.Room_Extra_Adul
           
                new SqlParameter("@Room_Standard_rate", Convert.ToInt32(rooms.Room_Standard_rate)),//6 rooms.Room_Standard_rate
                new SqlParameter("@Room_Agreed_Availability", defaultvalue),//7
                new SqlParameter("@Room_Lmk_Rate",defaultvalue),//8
                    new SqlParameter("@Room_camflg",defaultvalue),//9
               // new SqlParameter("@Room_camflg",rooms.Room_camflg),//9
                new SqlParameter("@Room_Checkin",Room_Checkintime ),//10 
                new SqlParameter("@Room_Checkout",Room_Checkouttime ), //11 
                new SqlParameter("@Room_Grace_time", Room_Grace_timetime), //12 
                new SqlParameter("@Room_Max_Thrshold_Disc", rooms.Room_Max_Thrshold_Disc),//13 rooms.Room_Max_Thrshold_Disc
                new SqlParameter("@Tax_Id",defaultvalue),//14 rooms.Tax_Id
                new SqlParameter("@Room_Active_flag",Flags),//15

                new SqlParameter("@Image_Name ", testing),//16
	            new SqlParameter("@Image_dir ", rooms.Image_dir),//17
                new SqlParameter("@Image_Remarks ", testing),//18
	            new SqlParameter("@Image_Created_By ", testing),//19
	            new SqlParameter("@Image_Created_on" ,dt),//20
	            new SqlParameter("@Image_Verified_By ",testing),//21
	            new SqlParameter("@Image_Verified_on" ,dt),//22
	            new SqlParameter("@Image_Active_From " ,dt),//23
	            new SqlParameter("@Image_Expires_on ",dt),//24	
                new SqlParameter("@Prop_Id", rooms.Prop_Id),//25   
              


            };

                Params[0].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddRooms", Params);
                // DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddProperty", Params);

                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Get room
        public List<Object> Bind(int Prop_Id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectAllRooms", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prop_Id", Prop_Id);

                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstroomtypes = new List<Object>();
                while (reader.Read())
                {
                    lstroomtypes.Add(

                        new
                        {
                            Prop_Id = reader["Prop_Id"].ToString(),
                            Room_Id = reader["Room_Id"].ToString(),
                            Room_Name = reader["Room_Name"].ToString(),
                            Room_Standard_rate = reader["Room_Standard_rate"].ToString(),
                            Room_Descr = reader["Room_Descr"].ToString(),
                            Room_Agreed_Availability = reader["Room_Agreed_Availability"].ToString(),
                            Image_dir = reader["Image_dir"].ToString(),
                            Prop_Name = reader["Prop_Name"].ToString()
                        });


                }
                conn.Close();


                return lstroomtypes;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //Update Room
        public TransactionStatus EditRooms(RoomsEditBo roomsEditBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                var rooms = BuiltRoomsDomain1(roomsEditBo);

                var Room_Checkin = Convert.ToDateTime(rooms.Room_Checkin);
                var Room_Checkout = Convert.ToDateTime(rooms.Room_Checkout);
                var Room_Grace_time = Convert.ToDateTime(rooms.Room_Grace_time);

                string Room_Checkintime = Room_Checkin.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);
                string Room_Checkouttime = Room_Checkout.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);
                string Room_Grace_timetime = Room_Grace_time.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);

                string testing = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string Flags = "true";
                int value = 1;
                int defaultvalue = 0;

                #region using  sql helper
                CemexDb con = new CemexDb();
                if (rooms.Image_dir == null)
                {
                    rooms.Image_dir = "/img/Room-image.png";
                }
                SqlParameter[] Params = 
            { 	
                    new SqlParameter("@opReturnValue", value),//27
                new SqlParameter("@Room_Id", rooms.Room_Id),//0
              
                new SqlParameter("@Room_Name",rooms.Room_Name),//2
                new SqlParameter("@Room_Overview", rooms.Room_Overview),//3
                new SqlParameter("@Room_Adult_occup",defaultvalue ),//4 rooms.Room_Adult_occup
                new SqlParameter("@Room_Child_occup", defaultvalue),//5 rooms.Room_Child_occup
                new SqlParameter("@Room_Extra_Adul",rooms.Room_Extra_Adul ),//6 rooms.Room_Extra_Adul
                new SqlParameter("@Room_Standard_rate", rooms.Room_Standard_rate),//7 rooms.Room_Standard_rate
                //new SqlParameter("@Room_Agreed_Availability", rooms.Room_Agreed_Availability),//8
                //new SqlParameter("@Room_Lmk_Rate", rooms.Room_Lmk_Rate),//9
                //new SqlParameter("@Room_camflg",rooms.Room_Lmk_Rate),//10
                new SqlParameter("@Room_Checkin",Room_Checkintime ),//11 
                new SqlParameter("@Room_Checkout",Room_Checkouttime ), //12 
                new SqlParameter("@Room_Grace_time", Room_Grace_timetime), //13 
                new SqlParameter("@Room_Max_Thrshold_Disc", rooms.Room_Max_Thrshold_Disc),//14 rooms.Room_Max_Thrshold_Disc
                new SqlParameter("@Tax_Id",defaultvalue),//15 rooms.Tax_Id
                new SqlParameter("@Room_Active_flag",Flags),//16 
                  new SqlParameter("@Image_Id",rooms.Image_Id),//26 
                new SqlParameter("@Image_Name ", testing),//17
	            new SqlParameter("@Image_dir ", rooms.Image_dir),//18
                new SqlParameter("@Image_Remarks ", testing),//19
	            new SqlParameter("@Image_Created_By ", testing),//20
	            new SqlParameter("@Image_Created_on" ,dt),//21
	            new SqlParameter("@Image_Verified_By ",testing),//22
	            new SqlParameter("@Image_Verified_on" ,dt),//23
	            new SqlParameter("@Image_Active_From " ,dt),//24
	            new SqlParameter("@Image_Expires_on ",dt)//25	
              
               // new SqlParameter("@Prop_Id", rooms.Prop_Id),//27  
            
        
            };

                Params[0].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateRooms", Params);
                ds.Locale = CultureInfo.InvariantCulture;
                string test = Params[0].Value.ToString();
                if (test == "0")
                    transactionStatus.Status = false;
                #endregion

                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }


        //Delete DeleteRoom
        public TransactionStatus DeleteRooms(RoomsEditBo roomsEditBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                var vendor = BuiltRoomsDomain1(roomsEditBo);


                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.Proc_DeleteUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(roomsEditBo.Vndr_Id));

                cmd.ExecuteNonQuery();
                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        //Get Room to Edit
        public List<Object> Edit(int Id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectRoomsbyId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Room_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstroom = new List<Object>();
                while (reader.Read())
                {
                    lstroom.Add(

                        new
                        {

                            Room_Id = reader["Room_Id"].ToString(),
                            Prop_Id = reader["Prop_Id"].ToString(),
                            Type_Id = reader["Type_Id"].ToString(),
                            Room_Name = reader["Room_Name"].ToString(),
                            Room_Overview = reader["Room_Overview"].ToString(),
                            Room_Adult_occup = reader["Room_Adult_occup"].ToString(),
                            Room_Child_occup = reader["Room_Child_occup"].ToString(),
                            Room_Extra_Adul = reader["Room_Extra_Adul"].ToString(),
                            Room_Standard_rate = reader["Room_Standard_rate"].ToString(),
                            Room_Agreed_Availability = reader["Room_Agreed_Availability"].ToString(),
                            Room_Lmk_Rate = reader["Room_Lmk_Rate"].ToString(),
                            Room_camflg = reader["Room_camflg"].ToString(),
                            Room_Checkin = reader["Room_Checkin"].ToString(),
                            Room_Checkout = reader["Room_Checkout"].ToString(),
                            Room_Grace_time = reader["Room_Grace_time"].ToString(),
                            Room_Max_Thrshold_Disc = reader["Room_Max_Thrshold_Disc"].ToString(),
                            Tax_Id = reader["Tax_Id"].ToString(),
                            Room_Active_flag = reader["Room_Active_flag"].ToString(),
                            Image_dir = reader["Image_dir"].ToString(),
                            Image_Id = reader["Image_Id"].ToString()
                        });


                }
                conn.Close();


                return lstroom;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        #region fill dropdown for Room And Room Type

        public List<object> GetRoomType()
        {
            List<Object> lstRoomType = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllRoomTypes");

            while (reader.Read())
            {
                lstRoomType.Add(
                    new
                    {
                        Type_Id = reader["Room_TypeId"].ToString(),
                        Room_Name = reader["Room_Name"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstRoomType;
        }




        #endregion

        public TransactionStatus UpdateRackPrice(int Inv_Id, int race_price)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@inv_Id", Inv_Id),//0        
                     new SqlParameter("@Room_Standard_rate",race_price)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateRacePrice", Params);

            return transactionStatus;
          
        }

        public TransactionStatus SuspendRoom(int room_id)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.proc_UpdateRoomActive_flag", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Room_Id", room_id);
                cmd.Parameters.AddWithValue("@Room_Active_flag", "false");
                cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
                cmd.ExecuteNonQuery();
                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //for connection string

        private Rooms BuiltRoomsDomain(RoomsBo roomsBo)
        {
            return (Rooms)new Rooms().InjectFrom(roomsBo);
        }
        private RoomsEdit BuiltRoomsDomain1(RoomsEditBo roomseditBo)
        {
            return (RoomsEdit)new RoomsEdit().InjectFrom(roomseditBo);
        }
        private RoomsBo BuiltRoomsBo(Rooms rooms)
        {
            return (RoomsBo)new RoomsBo().InjectFrom(rooms);
        }
        private RoomsBo BuiltRoomsEditBo(Rooms rooms)
        {
            return (RoomsBo)new RoomsEditBo().InjectFrom(rooms);
        }

                #endregion

        #region POLICY



        //Get  Policies to bind
        public List<Object> Bindpolicies(int Prop_Id, int Vendor_Id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectAllPolicies", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prop_Id", Prop_Id);
                cmd.Parameters.AddWithValue("@VendorId", Vendor_Id);
                //cmd.Parameters.AddWithValue("@Room_Id", Room_Id);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstpolicy = new List<Object>();
                while (reader.Read())
                {
                    lstpolicy.Add(

                        new
                        {
                            Policy_Id = reader["Policy_Id"].ToString(),
                            Policy_Name = reader["Policy_Name"].ToString(),
                            Policy_Descr = reader["Policy_Descr"].ToString()
                        });


                }
                conn.Close();


                return lstpolicy;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //create Policies
        public TransactionStatus Createpolicies(PoliciesBo policyBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                var policy = BuiltpolicyDomain(policyBo);

                string time = "16:23:01";
                var result = Convert.ToDateTime(time);
                string testtime = result.ToString("hh:mm:ss tt", CultureInfo.CurrentCulture);
                DateTime dt = new DateTime();
                dt = DateTime.Now;




                #region using  sql helper
                CemexDb con = new CemexDb();
                SqlParameter[] Params = 
            { 	
            
                //new SqlParameter("@Type_Id", policy.Type_Id),//0
                //new SqlParameter("@Room_Name", testing),//1 rooms.Room_Name
                //new SqlParameter("@Room_Overview", policy.Room_Overview),//2
                //new SqlParameter("@Room_Adult_occup", defaultvalue),//3 rooms.Room_Adult_occup
                //new SqlParameter("@Room_Child_occup", defaultvalue),//4 rooms.Room_Child_occup
                //new SqlParameter("@Room_Extra_Adul", defaultvalue),//5 rooms.Room_Extra_Adul
                //new SqlParameter("@Room_Standard_rate", defaultvalue),//6 rooms.Room_Standard_rate
                //new SqlParameter("@Room_Agreed_Availability", policy.Room_Agreed_Availability),//7
                //new SqlParameter("@Room_Lmk_Rate", policy.Room_Lmk_Rate),//8
                //new SqlParameter("@Room_camflg",policy.Room_camflg),//9
                //new SqlParameter("@Room_Checkin",testtime ),//10 rooms.Room_Checkin
                //new SqlParameter("@Room_Checkout",testtime ), //11 rooms.Room_Checkout
                //new SqlParameter("@Room_Grace_time", testtime), //12 rooms.Room_Grace_time
                //new SqlParameter("@Room_Max_Thrshold_Disc", defaultvalue),//13 rooms.Room_Max_Thrshold_Disc
                //new SqlParameter("@Tax_Id",defaultvalue),//14 rooms.Tax_Id
                //new SqlParameter("@Room_Active_flag",Flags),//15

                //new SqlParameter("@Image_Name ", testing),//16
                //new SqlParameter("@Image_dir ", policy.Image_dir),//17
                //new SqlParameter("@Image_Remarks ", testing),//18
                //new SqlParameter("@Image_Created_By ", testing),//19
                //new SqlParameter("@Image_Created_on" ,dt),//20
                //new SqlParameter("@Image_Verified_By ",testing),//21
                //new SqlParameter("@Image_Verified_on" ,dt),//22
                //new SqlParameter("@Image_Active_From " ,dt),//23
                //new SqlParameter("@Image_Expires_on ",dt),//24	
                     
                //new SqlParameter("@opReturnValue", value)//25


            };

                Params[25].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddRooms", Params);
                ds.Locale = CultureInfo.InvariantCulture;
                string test = Params[25].Value.ToString();
                // transactionStatus.id = Convert.ToInt32(test);
                #endregion

                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Update  Policies
        public TransactionStatus Editpolicies(PoliciesEditBo policyEditBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                int value = 0;
                CemexDb con = new CemexDb();
                SqlParameter[] Params = 
            { 	
            
                new SqlParameter("@Policy_Id",policyEditBo.Policy_Id ),//0 model.Prop_Id
                new SqlParameter("@Prop_Id",value ),//1 model.Prop_Id
                new SqlParameter("@Room_Id",value ),//2 model.Room_Id
                new SqlParameter("@Vndr_Id", value),//3 model.Vndr_Id
                new SqlParameter("@Policy_Name", policyEditBo.Policy_Name),//4
                new SqlParameter("@Policy_Descr", policyEditBo.Policy_Descr),//5 
                new SqlParameter("@opReturnValue", 1)//6


            };

                Params[6].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdatePolicies", Params);
                string test = Params[6].Value.ToString();




                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //Get  Policies to Edit
        public List<Object> Editpoliciesbyid(int Id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectPoliciesById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Policy_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstpolicy = new List<Object>();
                while (reader.Read())
                {
                    lstpolicy.Add(

                        new
                        {
                            Policy_Id = reader["Policy_Id"].ToString(),
                            Policy_Name = reader["Policy_Name"].ToString(),
                            Policy_Descr = reader["Policy_Descr"].ToString(),

                        });


                }
                conn.Close();


                return lstpolicy;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Policy Suspend
        public TransactionStatus Suspendpolicy(int id)
        {
            try
            {

                var transactionStatus = new TransactionStatus();
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.proc_UpdatePolicyActive_flag", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Policy_Id", id);
                cmd.Parameters.AddWithValue("@Policy_Active_Flag", "false");
                cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
                cmd.ExecuteNonQuery();
                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //for connection string

        private Policies BuiltpolicyDomain(PoliciesBo policyBo)
        {
            return (Policies)new Policies().InjectFrom(policyBo);
        }
        private PoliciesEdit BuiltpolicyDomain1(PoliciesEditBo policyeditBo)
        {
            return (PoliciesEdit)new PoliciesEdit().InjectFrom(policyeditBo);
        }
        private PoliciesBo BuiltpolicyBo(Policies policies)
        {
            return (PoliciesBo)new PoliciesBo().InjectFrom(policies);
        }
        private PoliciesEditBo BuiltpolicyEditBo(PoliciesEdit policiesedit)
        {
            return (PoliciesEditBo)new PoliciesEditBo().InjectFrom(policiesedit);
        }

        #endregion


        #region Facility
        //Get  Facility to bind
        public List<Object> BindFacility(int Room_id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectAllRoomsFacility", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Room_Id", Room_id);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstpolicy = new List<Object>();


                while (reader.Read())
                {
                    lstpolicy.Add(

                        new
                        {

                            Id = reader["Id"].ToString(),
                            Room_Id = reader["Room_Id"].ToString(),
                            Facility_Id = reader["Facility_Id"].ToString(),
                            Facility_Name = reader["Facility_Name"].ToString(),
                            Facility_Type = reader["Facility_Type"].ToString(),
                            //Facility_descr = reader["Facility_descr"].ToString(),
                            Facility_Image_dir = reader["Facility_Image_dir"].ToString(),
                            Active_flag = reader["Active_flag"].ToString(),
                            IsHeader = reader["IsHeader"].ToString(),
                            FTypecount = reader["FTypecount"].ToString(),


                        });


                }
                conn.Close();



                return lstpolicy;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }





        //Activate facility
        public TransactionStatus ActivateFacility(RoomFacilityEditBo rooms)
        {
            try
            {

                var transactionStatus = new TransactionStatus();
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.proc_AddRoomsFacility", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Room_Id", rooms.Room_Id);
                cmd.Parameters.AddWithValue("@Facility_Id", rooms.Facility_Id);
                cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
                cmd.ExecuteNonQuery();
                return transactionStatus;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        #endregion

    }
}
