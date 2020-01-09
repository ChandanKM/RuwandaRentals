using App.BusinessObject;
using App.Common;
using App.DataAccess;
using App.DataAccess.Interfaces;
using App.Domain;
using App.UIServices.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace App.UIServices
{
    public class PropertyService : RepositoryBase, IPropertyService
    {
        public PropertyService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        public List<object> Bind(int VendId)
        {
            List<Object> lstcityloc = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@VendId",VendId),//0
              


			};


            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllProperty", Params);

            while (reader.Read())
            {
                lstcityloc.Add(
                    new
                    {
                        PropertyCount = reader["PropertyCount"].ToString(),
                        Prop_Id = reader["Prop_Id"].ToString(),
                        Prop_Name = reader["Prop_Name"].ToString(),
                        City_Name = reader["City_Name"].ToString(),
                        Location = reader["Location_Name"].ToString(),
                        Prop_Booking_MailId = reader["Prop_Booking_MailId"].ToString(),
                        Prop_Booking_Mob = reader["Prop_Booking_Mob"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcityloc;
        }


        public List<object> GetCity()
        {
            List<Object> lstcity = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllCity");

            while (reader.Read())
            {
                lstcity.Add(
                    new
                    {
                        City_Id = reader["Id"].ToString(),
                        CityName = reader["Location"].ToString() + "," + reader["City"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcity;
        }

        public String CreateProperty(PropertyBo propertyBo)
        {
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            if (property.TripAdv == null)
                property.TripAdv = "";
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Name", property.Prop_Name),//0
                new SqlParameter("@Prop_Cin_No", property.Prop_Cin_No),//1
				new SqlParameter("@Prop_Addr1", property.Prop_Addr1),//2
                new SqlParameter("@Prop_Addr2",  property.Prop_Addr2),//3
				new SqlParameter("@CityID", property.City_Id),//5
				new SqlParameter("@Prop_Star_Rating", property.Prop_Star_Rating),//10
                new SqlParameter("@Prop_GPS_Pos", property.Prop_GPS_Pos),//11
                new SqlParameter("@Prop_Booking_MailId", property.Prop_Booking_MailId),//12
				new SqlParameter("@Prop_Booking_Mob", property.Prop_Booking_Mob),//13
                new SqlParameter("@Prop_Pricing_MailId", property.Prop_Pricing_MailId),//14
                new SqlParameter("@Prop_Pricing_Mob", property.Prop_Pricing_Mob),//15
                new SqlParameter("@Prop_Inventory_MailId", property.Prop_Inventory_MailId),//16
                new SqlParameter("@Prop_Inventory_Mob", property.Prop_Inventory_Mob),//17
                new SqlParameter("@Vndr_Id", property.Vndr_Id),//18
                  new SqlParameter("@Image_dir", property.Image_dir),//19
                    new SqlParameter("@Pricing_Type", property.Pricing_Type),//20
                  new SqlParameter("@Prop_Type", property.Prop_Type),//21
                    new SqlParameter("@Prop_Overview", property.Prop_Overview),//22
                     new SqlParameter("@Room_Checkin", property.Room_Checkins),//22
                      new SqlParameter("@Room_Checkout", property.Room_Checkouts),//22
                       new SqlParameter("@Location_Name", property.Location_Name),//22
                        new SqlParameter("@City_Name", property.City_Name),//22
                         new SqlParameter("@State_Name", property.State_Name),//22
                          new SqlParameter("@Pin_Code", property.Pin_Code),//22
                             new SqlParameter("@TripAdv", property.TripAdv)//22

			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddProperty", Params);

            return ds.Tables[0].Rows[0][0].ToString();
        }


        public List<object> BindFacility(string Id)
        {
            List<Object> lstcityloc = new List<Object>();
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", Id),//0
                
			};

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectFacilityById", Params);

            while (reader.Read())
            {
                lstcityloc.Add(
                    new
                    {
                        Facility_Id = reader["Facility_Id"].ToString(),
                        Facility_Type = reader["Facility_Type"].ToString(),
                        Facility_Name = reader["Facility_Name"].ToString(),
                        Facility_Image_dir = reader["Facility_Image_dir"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcityloc;
        }


        public List<object> GetFacilityType()
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllFacilityType");

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Facility_Type = reader["Facility_Type"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }

        public List<object> GetFacilityName()
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllFacilityName");

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Facility_Name = reader["Facility_Name"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }


        public TransactionStatus CreateFacility(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            var property = BuiltPropertyDomain(propertyBo);

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Facility_Name", property.Facility_Name),//0
                new SqlParameter("@Facility_Type", property.Facility_Type),//1
                 new SqlParameter("@Prop_Id", property.Prop_Id),//2
				new SqlParameter("@opReturnValue", SqlDbType.Int),//3
			};

            Params[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPropertyFacility", Params);
            return transactionStatus;
        }


        public TransactionStatus DeleteFacility(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Id", propertyBo.Facility_Id),//0
				new SqlParameter("@opReturnValue", SqlDbType.Int),//1
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeletePropertyFacility", Params);
            return transactionStatus;
        }


        public TransactionStatus DeleteProperty(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", property.Prop_Id),//0
                new SqlParameter("@Prop_Approved_By",property.Prop_Approved_By),//1
                new SqlParameter("@Prop_Approved_on",property.Prop_Approved_on),//2
                new SqlParameter("@Prop_Expires_on",property.Prop_Expires_on),//3
				new SqlParameter("@opReturnValue", SqlDbType.Int),//4
			};

            Params[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeleteProperty", Params);
            return transactionStatus;
        }

        private Property BuiltPropertyDomain(PropertyBo propertyBo)
        {
            return (Property)new Property().InjectFrom(propertyBo);
        }


        public List<object> Edit(string Id)
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Id", Id),//0
			};

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectPropertyById", Params);

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Prop_Id = reader["Prop_Id"].ToString(),
                        Prop_Name = reader["Prop_Name"].ToString(),
                        City_Id = reader["Id"].ToString(),
                        City_Area = reader["Location_Name"].ToString(),
                        City_Name = reader["City_Name"].ToString(),
                        State_Name = reader["State_Name"].ToString(),
                        Pincode = reader["Pin_Code"].ToString(),
                        Prop_Cin_No = reader["Prop_Cin_No"].ToString(),

                        Prop_Addr1 = reader["Prop_Addr1"].ToString(),
                        Prop_Addr2 = reader["Prop_Addr2"].ToString(),
                        Prop_Pricing_MailId = reader["Prop_Pricing_MailId"].ToString(),
                        Prop_Pricing_Mob = reader["Prop_Pricing_Mob"].ToString(),
                        Prop_Inventory_MailId = reader["Prop_Inventory_MailId"].ToString(),
                        Prop_Inventory_Mob = reader["Prop_Inventory_Mob"].ToString(),
                        TripId = reader["TripId"].ToString(),

                        Prop_GPS_Pos = reader["Prop_GPS_Pos"].ToString(),
                        Prop_Star_Rating = reader["Prop_Star_Rating"].ToString(),

                        Prop_Booking_MailId = reader["Prop_Booking_MailId"].ToString(),
                        Prop_Booking_Mob = reader["Prop_Booking_Mob"].ToString(),
                        Image_dir = reader["Image_dir"].ToString(),
                        Pricing_Type = reader["Pricing_Type"].ToString(),
                        Prop_Type = reader["Prop_Type"].ToString(),
                        Prop_Overview = reader["Prop_Overview"].ToString(),
                        Room_Checkin = reader["Room_Checkin"].ToString(),
                        Room_Checkout = reader["Room_Checkout"].ToString(),


                    });
                break;
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }




        public TransactionStatus DeteteEvent(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Event_Id", property.Event_Id),//0
				new SqlParameter("@opReturnValue", SqlDbType.Int),//4
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeleteEvent", Params);
            return transactionStatus;
        }

        public TransactionStatus DeteteImage(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Image_Id", property.Image_Id),//0
				new SqlParameter("@opReturnValue", SqlDbType.Int),//4
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeleteImage", Params);
            return transactionStatus;
        }
        public TransactionStatus Edit(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                new SqlParameter("@Prop_Id", property.Prop_Id),//1
                new SqlParameter("@Prop_Name", property.Prop_Name),//2
                new SqlParameter("@Prop_Cin_No", property.Prop_Cin_No),//3
				new SqlParameter("@Prop_Addr1", property.Prop_Addr1),//4
                new SqlParameter("@Prop_Addr2", property.Prop_Addr2),//5
               
				new SqlParameter("@CityID", property.City_Id),//7 property.CityId
             
				new SqlParameter("@Prop_Star_Rating", property.Prop_Star_Rating),//12
                new SqlParameter("@Prop_GPS_Pos", property.Prop_GPS_Pos),//13
                new SqlParameter("@Prop_Booking_MailId", property.Prop_Booking_MailId),//14
				new SqlParameter("@Prop_Booking_Mob", property.Prop_Booking_Mob),//15
                new SqlParameter("@Prop_Pricing_MailId", property.Prop_Pricing_MailId),//16
                new SqlParameter("@Prop_Pricing_Mob", property.Prop_Pricing_Mob),//17
                new SqlParameter("@Prop_Inventory_MailId", property.Prop_Inventory_MailId),//18
                new SqlParameter("@Prop_Inventory_Mob", property.Prop_Inventory_Mob),//19
            
                 //   new SqlParameter("@Image_dir", property.Image_dir),//20
                new SqlParameter("@Pricing_Type", property.Pricing_Type),//19
                new SqlParameter("@Prop_Type", property.Prop_Type),//19
                   new SqlParameter("@Prop_Overview", property.Prop_Overview),//22
                     new SqlParameter("@Room_Checkin", property.Room_Checkins),//22
                       new SqlParameter("@Room_Checkout", property.Room_Checkouts),//22
                         new SqlParameter("@Location_Name", property.Location_Name),//22
                        new SqlParameter("@City_Name", property.City_Name),//22
                         new SqlParameter("@State_Name", property.State_Name),//22
                          new SqlParameter("@Pin_Code", property.Pin_Code),//22
                            new SqlParameter("@TripAdvi", property.TripAdv),//22
                           
			};


            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateProperty", Params);
            return transactionStatus;
        }


        public string CreateBankDetails(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(propertyBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                new SqlParameter("@Prop_Id", property.Prop_Id),//1
                new SqlParameter("@Bank_Name", property.Bank_Name),//2
                new SqlParameter("@Bank_Branch_Name", property.Bank_Branch_Name),//3
				new SqlParameter("@Bank_Branch_Code", property.Bank_Branch_Code),//4
                new SqlParameter("@Bank_IFC_code", property.Bank_IFC_code),//5
                new SqlParameter("@Bank_Accnt_No", property.Bank_Accnt_No),//6
				new SqlParameter("@Bank_Accnt_Name", property.Bank_Accnt_Name),//7
                new SqlParameter("@Vndr_Id", property.Vndr_Id),//8
                new SqlParameter("@CityID", property.City_Id),//9
                new SqlParameter("@Bank_descr","null"),//2
			};

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddBankDetails", Params);
            return ds.Tables[0].Rows[0][0].ToString();
        }


        public TransactionStatus EditBank(PropertyBo bankBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var property = BuiltPropertyDomain(bankBo);
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@opReturnValue", SqlDbType.Int),//0
              
                new SqlParameter("@Bank_Name", property.Bank_Name),//2
                new SqlParameter("@Bank_Branch_Name", property.Bank_Branch_Name),//3
				new SqlParameter("@Bank_Branch_Code", property.Bank_Branch_Code),//4
                new SqlParameter("@Bank_IFC_code", property.Bank_IFC_code),//5
                new SqlParameter("@Bank_Accnt_No", property.Bank_Accnt_No),//6
				new SqlParameter("@Bank_Accnt_Name", property.Bank_Accnt_Name),//7
                new SqlParameter("@Vndr_Id", property.Vndr_Id),//8
               	new SqlParameter("@CityID", property.City_Id),//9  property.CityId 
                new SqlParameter("@Bank_Id",property.Bank_Id),//10
                 new SqlParameter("PropId",property.Prop_Id)//11
			};

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateBankDetails", Params);
            return transactionStatus;
        }


        public List<object> EditBankDetails(string Id)
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", Id),//0
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectBankByPropertyId", Params);

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Bank_Id = reader["Bank_Id"].ToString(),
                        Bank_Name = reader["Bank_Name"].ToString(),
                        Bank_descr = reader["Bank_descr"].ToString(),
                        Bank_Accnt_No = reader["Bank_Accnt_No"].ToString(),
                        Bank_Accnt_Name = reader["Bank_Accnt_Name"].ToString(),
                        Bank_Branch_Name = reader["Bank_Branch_Name"].ToString(),
                        Bank_Branch_Code = reader["Bank_Branch_Code"].ToString(),
                        City_Id = reader["Id"].ToString(),
                        City_Area = reader["Location"].ToString(),
                        City_Name = reader["City"].ToString(),
                        State_Name = reader["State"].ToString(),
                        Pincode = reader["pincode"].ToString(),
                        Bank_IFC_code = reader["Bank_IFC_code"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }




        public List<object> BindImage(string Id)
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", Id),//0
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectImagesByPropId", Params);

            while (reader.Read())
            {
                lst.Add(
                    new
                    {

                        Image_Id = reader["Image_Id"].ToString(),
                        Image_dir = reader["Image_dir"].ToString(),
                        Image_Name = reader["Image_Name"].ToString(),
                        Image_descr = reader["Image_Remarks"].ToString(),
                        Active_flag = reader["Image_Active_flag"].ToString(),
                        Default_flag = reader["ImageDefault"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }

        public TransactionStatus UpdateImageFlag(int Id, string flag)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@image_Id", Id),//0        
                     new SqlParameter("@active_Flag",flag)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateProperty_Image_Gallery_flag", Params);

            return transactionStatus;
        }

        public TransactionStatus SetDefaultImage(int PropId, int ImageId)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@prop_Id", PropId),//0        
                     new SqlParameter("@image_Id",ImageId)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateProperty_Default_Image", Params);

            return transactionStatus;
        }

        public TransactionStatus createPolicy(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            var property = BuiltPropertyDomain(propertyBo);

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", property.Prop_Id),//2
				new SqlParameter("@Vndr_Id", property.Vndr_Id),//3
                new SqlParameter("@Room_Id","0"),
                new SqlParameter("@Policy_Name", property.Policy_Name),//0
                new SqlParameter("@Policy_descr", property.Policy_descr),//1
               new SqlParameter("@opReturnValue", SqlDbType.Int),//0
			};

            Params[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPolicies", Params);
            //    ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }

        public TransactionStatus createRoomPolicy(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            var property = BuiltPropertyDomain(propertyBo);

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Prop_Id", property.Prop_Id),//2
				new SqlParameter("@Vndr_Id", property.Vndr_Id),//3
                new SqlParameter("@Room_Id",property.Room_Id),
                new SqlParameter("@Policy_Name", property.Policy_Name),//0
                new SqlParameter("@Policy_descr", property.Policy_descr),//1
               new SqlParameter("@opReturnValue", SqlDbType.Int),//0
			};

            Params[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPolicies", Params);
            //    ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }
        public TransactionStatus EditPolicy(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            var property = BuiltPropertyDomain(propertyBo);

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Policy_Id", property.Policy_Id),//2

                new SqlParameter("@Policy_Name", property.Policy_Name),//0
                new SqlParameter("@Policy_descr", property.Policy_descr),//1
               new SqlParameter("@opReturnValue", SqlDbType.Int),//0
			};

            Params[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_UpdatePropertyPolicies]", Params);
            //    ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }
        public List<object> BindRoomPolicy(int PropId, int VendId, int RoomId)
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@PropId", PropId),//0
                new SqlParameter("@VendId", VendId),//1
                 new SqlParameter("@RoomId", RoomId),//1
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllPropertyPolicies", Params);

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Policy_Id = reader["Policy_Id"].ToString(),
                        Policy_Name = reader["Policy_Name"].ToString(),
                        Policy_descr = reader["Policy_descr"].ToString(),
                        PropId = reader["Prop_Id"].ToString(),

                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }
        public List<object> BindPolicy(int PropId, int VendId)
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@PropId", PropId),//0
                new SqlParameter("@VendId", VendId),//1
                 new SqlParameter("@RoomId", "0"),//1
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllPropertyPolicies", Params);

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        Policy_Id = reader["Policy_Id"].ToString(),
                        Policy_Name = reader["Policy_Name"].ToString(),
                        Policy_descr = reader["Policy_descr"].ToString(),
                        PropId = reader["Prop_Id"].ToString(),

                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }
        public TransactionStatus DetetePolicy(PropertyBo propertyBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Id", propertyBo.Policy_Id),//0
				new SqlParameter("@opReturnValue", SqlDbType.Int),//1
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_DeletePropertyPolicy]", Params);
            return transactionStatus;
        }


        #region Facility
        //Get  Facility to bind
        public List<Object> BindFacilityimage(int prop_id)
        {
            try
            {
                CemexDb con = new CemexDb();

                SqlConnection conn = con.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("proc_SelectAllPropertyFacility", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prop_Id", prop_id);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Object> lstpolicy = new List<Object>();


                while (reader.Read())
                {
                    lstpolicy.Add(

                        new
                        {

                            Id = reader["Id"].ToString(),
                            Prop_Id = reader["Prop_Id"].ToString(),
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
                SqlCommand cmd = new SqlCommand("dbo.proc_AddPropertyFacilityimage", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prop_Id", rooms.Prop_Id);
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


        public List<string> GetAutoCompleteLocation(string terms)
        {

            List<string> lstcityloc = new List<string>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@term",terms),//0
              
			};


            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectedCity", Params);

            while (reader.Read())
            {
                lstcityloc.Add(reader["Location"].ToString() + "," + reader["City"].ToString());

            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcityloc;
        }
        
        public List<object> GetAutoCompleteLocationWithId(string terms)
        {

            List<object> lstObj = new List<object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@term",terms),//0
              
			};

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectedCity", Params);

            while (reader.Read())
            {

                lstObj.Add(new
                {
                    Id = reader["Id"].ToString(),
                    City = reader["City"].ToString(),
                    Location = reader["Location"].ToString(),
                    State = reader["State"].ToString(),
                    Pincode = reader["Pincode"].ToString(),

                });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstObj;
        }
        public List<object> PropertyAutoCompleteSearch(string terms)
        {

            List<object> lstObj = new List<object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@term",terms),//0
              
			};

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAutoCompleteProperty]", Params);

            while (reader.Read())
            {

                lstObj.Add(new
                {
                    Prop_Id = reader["Prop_Id"].ToString(),
                    Prop_Name = reader["Prop_Name"].ToString(),
                  

                });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstObj;
        }
    }
}
