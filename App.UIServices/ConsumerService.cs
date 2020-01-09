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

    public class ConsumerService : RepositoryBase, IConsumerService
    {
        public ConsumerService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        //for Add Consumer

        public TransactionStatus AddConsumer(ConsumerBo consumerBo)
        {

            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerDomain(consumerBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@Cons_First_Name", SqlDbType.NVarChar),//0
              new SqlParameter("@Cons_Last_Name", SqlDbType.NVarChar),//1
                new SqlParameter("@Cons_Gender", SqlDbType.NVarChar),//2
                  new SqlParameter("@Cons_Dob", SqlDbType.NVarChar),//3
                    new SqlParameter("@Cons_mailid",SqlDbType.NVarChar),//4
                     new SqlParameter("@Cons_Pswd", SqlDbType.NVarChar),//5
                      new SqlParameter("@Cons_Mobile",SqlDbType.NVarChar),//6
                        new SqlParameter("@Cons_Addr1", SqlDbType.NVarChar),//7
                            new SqlParameter("@Cons_Addr2",SqlDbType.NVarChar),//8
                             new SqlParameter("@Cons_City", SqlDbType.NVarChar),//9
                             new SqlParameter("@Cons_Area",SqlDbType.NVarChar),//10
                             new SqlParameter("@Cons_Pincode", SqlDbType.Int),//11
                              new SqlParameter("@Cons_Company", SqlDbType.NVarChar),//12
                              new SqlParameter("@Cons_Company_Id", SqlDbType.NVarChar),//13
                                new SqlParameter("@Cons_Reference",SqlDbType.NVarChar),//14
                                  new SqlParameter("@Cons_Affiliates_Id", SqlDbType.Int),//15
                                   new SqlParameter("@Cons_Loyalty_Id",SqlDbType.Int),//16
                                    new SqlParameter("@Cons_Earned_Loyalpoints", SqlDbType.Int),//17
                                      new SqlParameter("@Cons_Ipaddress", SqlDbType.NVarChar),//19
                                        new SqlParameter("@Cons_regist_On",DateTime.Now),//20
                                          new SqlParameter("@Cons_Active_flag", 1),//21
                                             new SqlParameter("@opReturnValue", SqlDbType.Int)//22
			};
            if (!String.IsNullOrEmpty(consumer.Cons_First_Name))
            {
                Params[0].Value = consumer.Cons_First_Name;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Last_Name))
            {
                Params[1].Value = consumer.Cons_Last_Name;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Gender))
            {
                Params[2].Value = consumer.Cons_Gender;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Dob))
            {
                Params[3].Value = consumer.Cons_Dob;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_mailid))
            {
                Params[4].Value = consumer.Cons_mailid;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Pswd))
            {
                Params[5].Value = consumer.Cons_Pswd;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Mobile))
            {
                Params[6].Value = consumer.Cons_Mobile;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Addr1))
            {
                Params[7].Value = consumer.Cons_Addr1;
            }
            else
            {
                Params[7].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Addr2))
            {
                Params[8].Value = consumer.Cons_Addr2;
            }
            else
            {
                Params[8].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_City))
            {
                Params[9].Value = consumer.Cons_City;
            }
            else
            {
                Params[9].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Area))
            {
                Params[10].Value = consumer.Cons_Area;
            }
            else
            {
                Params[10].Value = DBNull.Value;
            }
            if (consumer.Cons_Pincode != 0)
            {
                Params[11].Value = consumer.Cons_Pincode;
            }
            else
            {
                Params[11].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Company))
            {
                Params[12].Value = consumer.Cons_Company;
            }
            else
            {
                Params[12].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Company_Id))
            {
                Params[13].Value = consumer.Cons_Company_Id;
            }
            else
            {
                Params[13].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Reference))
            {
                Params[14].Value = consumer.Cons_Reference;
            }
            else
            {
                Params[14].Value = DBNull.Value;
            }
            if (consumer.Cons_Affiliates_Id != 0)
            {
                Params[15].Value = consumer.Cons_Affiliates_Id;
            }
            else
            {
                Params[15].Value = DBNull.Value;
            }
            if (consumer.Cons_Loyalty_Id != 0)
            {
                Params[16].Value = consumer.Cons_Loyalty_Id;
            }
            else
            {
                Params[16].Value = DBNull.Value;
            }
            if (consumer.Cons_Earned_Loyalpoints != 0)
            {
                Params[17].Value = consumer.Cons_Earned_Loyalpoints;
            }
            else
            {
                Params[17].Value = DBNull.Value;
            }

            if (consumer.Cons_Redeemed_Loyalpoints1 != 0)
            {
                Params[18].Value = consumer.Cons_Redeemed_Loyalpoints1;
            }
            else
            {
                Params[18].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Ipaddress))
            {
                Params[19].Value = consumer.Cons_Ipaddress;
            }
            else
            {
                Params[19].Value = DBNull.Value;
            }


            Params[22].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddConsumer", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[22].Value.ToString();

            return transactionStatus;


        }
        //for update
        public TransactionStatus UpdateConsumer(ConsumerBo consumerBo)
        {

            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerDomain(consumerBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
				new SqlParameter("@Cons_First_Name",consumer.Cons_First_Name),//0
              new SqlParameter("@Cons_Last_Name",consumer.Cons_Last_Name),//1
                new SqlParameter("@Cons_Gender",consumer.Cons_Gender),//2
                  new SqlParameter("@Cons_Dob",consumer.Cons_Dob),//3
                    new SqlParameter("@Cons_mailid",consumer.Cons_mailid),//4
                      new SqlParameter("@Cons_Mobile",consumer.Cons_Mobile),//6
                        new SqlParameter("@Cons_Addr1", consumer.Cons_Addr1),//7
                            new SqlParameter("@Cons_Addr2",consumer.Cons_Addr2),//8
                             new SqlParameter("@Cons_City",consumer.Cons_City),//9
                               new SqlParameter("@Cons_Area",consumer.Cons_Area),//9
                                 new SqlParameter("@Cons_Pincode",consumer.Cons_Pincode),//9
                               new SqlParameter("@Cons_Company", consumer.Cons_Company),//12
                              new SqlParameter("@Cons_Company_Id",consumer.Cons_Company_Id),//13
                                new SqlParameter("@Cons_Reference",consumer.Cons_Reference),//14
                                      new SqlParameter("@Cons_Ipaddress",consumer.Cons_Ipaddress),//19
                                      new SqlParameter("@Cons_Id",consumer.Cons_Id)//23
			};

            Params[0].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateConsumer", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[0].Value.ToString();

            return transactionStatus;


        }

        //for reset pwd
        public TransactionStatus UpdateConsumerPswd(ConsumerBo consumerBo)
        {

            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerDomain(consumerBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
			        new SqlParameter("@Cons_Pswd",consumer.Cons_Pswd),//19
                   new SqlParameter("@Cons_Id",consumer.Cons_Id)//23
			};

            Params[0].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateConsumerPswd", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[0].Value.ToString();

            return transactionStatus;


        }


        //City
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
                        CityId = reader["Id"].ToString(),
                        CityName = reader["Location"].ToString() + " , " + reader["City"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcity;
        }


        public List<object> GetLocations()
        {
            List<Object> lst = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllLocation");

            while (reader.Read())
            {
                lst.Add(
                    new
                    {
                        LocationId = reader["Location_Id"].ToString(),
                        LocationName = reader["Location_desc"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lst;
        }
        public DataSet AddConsumerMandet(ConsumerMandetBo consumerBo)
        {

            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerMandetDomain(consumerBo);
            string compName = string.Empty;
            if (consumer.emailCheck)
            {
                var index = consumer.Cons_mailid.IndexOf('.');

                var domain = consumer.Cons_mailid.Split('@')[1];
                compName = domain.Split('.')[0];
                consumer.Cons_Company = compName;
            }
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
              
          
          
             
				 new SqlParameter("@Cons_First_Name",SqlDbType.NVarChar),//0
                 new SqlParameter("@Cons_Last_Name",SqlDbType.NVarChar),//1
                 new SqlParameter("@Cons_mailid",SqlDbType.NVarChar),//2
                 new SqlParameter("@Cons_Pswd",SqlDbType.NVarChar),//3
                 new SqlParameter("@Cons_Mobile",SqlDbType.NVarChar),//4
                 new SqlParameter("@Cons_regist_On", DateTime.Now),//5
                 new SqlParameter("@Cons_Active_flag", 1),//6
                 new SqlParameter("@opReturnValue", SqlDbType.Int),//7
                 new SqlParameter("@cons_company",SqlDbType.NVarChar)
			};


            if (!String.IsNullOrEmpty(consumer.Cons_First_Name))
            {
                Params[0].Value = consumer.Cons_First_Name;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Last_Name))
            {
                Params[1].Value = consumer.Cons_Last_Name;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Last_Name))
            {
                Params[1].Value = consumer.Cons_Last_Name;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_mailid))
            {
                Params[2].Value = consumer.Cons_mailid;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Pswd))
            {
                Params[3].Value = consumer.Cons_Pswd;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Mobile))
            {
                Params[4].Value = consumer.Cons_Mobile;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            Params[7].Direction = ParameterDirection.Output;

            if (!String.IsNullOrEmpty(consumer.Cons_Company))
            {
                Params[8].Value = consumer.Cons_Company;
            }
            else
            {
                Params[8].Value = DBNull.Value;
            }

            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddConsumerMandet", Params);


            return ds;


        }
        //Forgotpwd
        public DataSet ConsumerForgotpwd(ConsumerForgotpwdBo loginBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerForgotpwdDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_mailid", consumer.Cons_mailid),//0
                 
                      
			};
            if (!String.IsNullOrEmpty(consumer.Cons_mailid))
            {
                Params[0].Value = consumer.Cons_mailid;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_ConsumerForgotPwd", Params);
            return ds;
        }
        //Consumer Login
        public DataSet ConsumerLogin(ConsumerLoginBo loginBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerLoginDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_mailid", consumer.Cons_mailid),//0
                     new SqlParameter("@Cons_Pswd", consumer.Cons_Pswd),//1
                      
			};
            if (!String.IsNullOrEmpty(consumer.Cons_mailid))
            {
                Params[0].Value = consumer.Cons_mailid;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Pswd))
            {
                Params[1].Value = consumer.Cons_Pswd;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_ConsumerLogin", Params);
            return ds;
        }

        //FB Login
        public DataSet FbConsumerLogin(ConsumerLoginBo loginBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerLoginDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_mailid", consumer.Cons_mailid),//0
                     new SqlParameter("@Cons_Pswd", consumer.Cons_Pswd),//1
                      
			};
            if (!String.IsNullOrEmpty(consumer.Cons_mailid))
            {
                Params[0].Value = consumer.Cons_mailid;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Cons_Pswd))
            {
                Params[1].Value = consumer.Cons_Pswd;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_FbConsumerLogin", Params);
            return ds;
        }
        //Consumer Details
        public DataSet ConsumerDetails(ConsumerDetailsBo consBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerDetailsDomain(consBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_Id", consumer.Cons_Id),//0
                 
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_ConsumerDetailsById", Params);
            return ds;
        }

        public DataSet GetProfileDetails(string Cons_Id)
        {

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_Id",Cons_Id),//0                              
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_GetConsumerDetails_ById", Params);
            return ds;
        }

        //PropertyListing
        public DataSet PropertyList(ListingBo listBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPropertyListingDomain(listBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@City_Id",Listing.CityMasterId),//0
                  
                       new SqlParameter("@Room_Checkin",Listing.Room_Checkin),//1
                       new SqlParameter("@Room_Checkout",Listing.Room_Checkout),//2
                       new SqlParameter("@No_Of_Rooms",Listing.No_Of_Rooms),//3
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_PropertyListing", Params);
            return ds;
        }
        public DataSet PropertyList_Sort(ListingBo listBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPropertyListingDomain(listBo);

            Listing.Rating = Listing.Rating == "%" ? null : Listing.Rating;
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@City_Id",Listing.CityMasterId),//0
                  
                       new SqlParameter("@Room_Checkin",Listing.Room_Checkin),//1
                       new SqlParameter("@Room_Checkout",Listing.Room_Checkout),//2
                       new SqlParameter("@No_Of_Rooms",Listing.No_Of_Rooms),//3
                       new SqlParameter("@Facilities",Listing.Facilities),//3
                       new SqlParameter("@Price1",Listing.Price1),//3
                       new SqlParameter("@Price2",Listing.Price2),//3
                       new SqlParameter("@Rating",Listing.Rating),//3
                       new SqlParameter("@SortBy",Listing.SortBy),//3
                             
                      
			};

            //DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_PropertyListing_Sort_New3", Params);
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "USP_GetAllRooms", Params);
            return ds;
        }
        public DataSet PropertyListDetails(ListingDetailsBo listBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPropertyListingDetailsDomain(listBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@PropId",Listing.PropId),//0

                       new SqlParameter("@Room_Checkin",Listing.Room_Checkin),//1
                       new SqlParameter("@Room_Checkout",Listing.Room_Checkout),//2
                       new SqlParameter("@No_Of_Rooms",Listing.No_Of_Rooms),//3
                    
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_PropertyListingDetailed", Params);
            return ds;
        }
        public DataSet RoomList(ListingDetailsRoomBo listBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPropertyListingDetailsRoomDomain(listBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@PropId",Listing.PropId),//0
                         new SqlParameter("@Room_Checkin",Listing.Room_Checkin),//0
                       new SqlParameter("@Room_Checkout",Listing.Room_Checkout),//0
                        new SqlParameter("@No_Of_Rooms",Listing.No_Of_Rooms),//0

			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Room_Listing", Params);
            return ds;
        }
        public DataSet PreBooking(PrebookingBo PreBo)
        {

            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPreBookingDomain(PreBo);
            if (Listing.Invce_note == null)
            {
                Listing.Invce_note = "";
            }
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{ 
             
             
               new SqlParameter("@Vndr_ID",Listing.Vndr_ID),//0
              new SqlParameter("@Prop_Id",Listing.PropId),//3
                 new SqlParameter("@Room_Id",Listing.RoomID),//2
                   new SqlParameter("@Cons_Id",Listing.Cons_Id),//3
               
                      new SqlParameter("@Checkin",Listing.Room_Checkin),//5
                       new SqlParameter("@Checkout",Listing.Room_Checkout),//6
                        new SqlParameter("@Room_Count",Listing.Room_Count),//7
                         new SqlParameter("@prop_room_rate",Listing.prop_room_rate),//8
                         new SqlParameter("@camo_room_rate",Listing.camo_room_rate),//9
                           //new SqlParameter("@camo_room_rate",Listing.camo_room_rate),//9
                             new SqlParameter("@tax_amnt",Listing.tax_amnt),//10
                              new SqlParameter("@net_amt",Listing.net_amt),//11
                              new SqlParameter("@Invce_note",Listing.Invce_note),//12
                             
                                               new SqlParameter("@redmpt_points",Listing.redmpt_points),//18
                                               new SqlParameter("@redmpt_value",Listing.redmpt_value),//19
                                               new SqlParameter("@Promo_Type",Listing.Promo_Type),//20
                                                new SqlParameter("@Prop_Value",Listing.Prop_Value),//21
                                                 new SqlParameter("@ipaddress",Listing.ipaddress),//22
                                                    new SqlParameter("@GuestName",Listing.GuestName),//23


			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_AddTransaction]", Params);
            return ds;
        }


        public DataSet PreBookingUpdate(PrebookingBo PreBo)
        {
            try
            {
                var transactionStatus = new TransactionStatus();
                var Listing = BuiltPreBookingDomain(PreBo);
                CemexDb con = new CemexDb();
                SqlParameter[] Params = 

			{ 
              
                                 new SqlParameter("@Trans_No",Listing.Trans_No),//1
                                   new SqlParameter("@paid_status",Listing.paid_status),//2
                                      new SqlParameter("@credit_debit_card",Listing.credit_debit_card),//3
                                         new SqlParameter("@card_no",Listing.card_no),//4
                                           new SqlParameter("@card_type",Listing.card_type),//5
                                                    new SqlParameter("@Invce_Num",Listing.Invce_Num),//6
                                                      new SqlParameter("@BookingStatus",Listing.paid_status),//6
                                                  //   new SqlParameter("@AllInfo",Listing.AllInfo),//6
                                                  


			};


                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_UpdateTransaction]", Params);
                return ds;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        public DataSet GetTransaction(string Invce_Num)
        {
            var transactionStatus = new TransactionStatus();
            //  var Listing = BuiltPreBookingDomain(PreBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{ 
               
                                                    new SqlParameter("@Invce_Num",Invce_Num),//0


			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectTransaction]", Params);
            return ds;
        }
        public DataSet GetAllTransaction(PrebookingBo PreBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPreBookingDomain(PreBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{ 
         
                                                    new SqlParameter("@Cons_Id",Listing.Cons_Id),//0


			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAllTransaction]", Params);
            return ds;
        }

        //new services added for web
        #region WebAppServices

        public TransactionStatus ChangePassword(ConsumerChangePasswordBo changepasswordBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltChangePasswordDomain(changepasswordBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                   new SqlParameter("@Cons_Id",consumer.Cons_Id),//23
			       new SqlParameter("@Cons_Pswd",consumer.Curnt_Pswd),//19
                   new SqlParameter("@New_Pswd",consumer.NewPassword)//23
			};

            Params[0].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Consumer_ChangePassword", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            int IsChange = Convert.ToInt32(Params[0].Value);
            if (IsChange == 1)
                transactionStatus.Status = true;
            else
                transactionStatus.Status = false;

            return transactionStatus;
        }

        public TransactionStatus UpdateConsumerProfile(ConsumerFormBo consumerBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerFormDomain(consumerBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@opReturnValue", SqlDbType.Int),//0
				new SqlParameter("@Cons_First_Name",consumer.Cons_First_Name),//0
               new SqlParameter("@Cons_Last_Name",consumer.Cons_Last_Name),//1
                new SqlParameter("@Cons_Gender",consumer.Cons_Gender),//2
                  new SqlParameter("@Cons_Dob",consumer.Cons_Dob.ToString("dd/MM/yyyy")),//3
                    new SqlParameter("@Cons_mailid",consumer.Cons_mailid),//4
                      new SqlParameter("@Cons_Mobile",consumer.Cons_Mobile),//6
                        new SqlParameter("@Cons_Addr1", consumer.Cons_Addr1),//7
                            new SqlParameter("@Cons_Addr2",consumer.Cons_Addr2),//8
                             new SqlParameter("@Cons_City",consumer.Cons_City),//9
                                 new SqlParameter("@Cons_Area", consumer.Cons_Area),//12
                             
                                new SqlParameter("@Cons_Pincode", consumer.Cons_Pincode),//12
                                  new SqlParameter("@Cons_Company", consumer.Cons_Company),//12
                              new SqlParameter("@Cons_Company_Id",consumer.Cons_Company_Id),//13
                                new SqlParameter("@Cons_Reference",consumer.Cons_Reference),//14
                                      new SqlParameter("@Cons_Ipaddress",consumer.Cons_Ipaddress),//19
                                      new SqlParameter("@Cons_Id",consumer.Cons_Id)//23
			};

            Params[0].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateConsumer", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[0].Value.ToString();

            return transactionStatus;
        }

        public DataSet GetOverviewBookedDealsById(string Cons_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{ 
                     new SqlParameter("@Cons_Id",Cons_Id),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Select_UpcomingBookedDeals_ByConsumerID]", Params);
            return ds;
        }

        public DataSet GetBookedTransactionById(string Cons_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{ 
                     new SqlParameter("@Cons_Id",Cons_Id),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAllTransactionByConsumerID]", Params);
            return ds;
        }

        public TransactionStatus CheckSubscribeEmailLatter(ConsumerSubscribeBo subscribeBo)
        {
            var transactionStatus = new TransactionStatus();
            var subscirbe = BuiltSubscribeDomain(subscribeBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@Cons_Id", subscirbe.Cons_Id),//0
           		 new SqlParameter("@Email", subscirbe.Email),//1
                 //new SqlParameter("@Ipaddress", subscirbe.Ipaddress),//3
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//2
			};

            Params[2].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Consumer_CheckSubscribeEmail", Params);
            int isExist = Convert.ToInt32(Params[2].Value);
            if (isExist == 0)
            {
                transactionStatus.Status = false;
            }
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }


        public TransactionStatus SubscribeEmailLatter(ConsumerSubscribeBo subscribeBo)
        {
            var transactionStatus = new TransactionStatus();
            var subscirbe = BuiltSubscribeDomain(subscribeBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@Cons_Id", subscirbe.Cons_Id),//0
           		 new SqlParameter("@Email", subscirbe.Email),//0
                 new SqlParameter("@Ipaddress", subscirbe.Ipaddress),//3
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//4
			};

            Params[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Consumer_SubscribeEmail", Params);
            int isExist = Convert.ToInt32(Params[3].Value);
            if (isExist == 0)
            {
                transactionStatus.Status = false;
            }
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }
        public TransactionStatus UnSubscribeEmailLatter(ConsumerSubscribeBo subscribeBo)
        {
            var transactionStatus = new TransactionStatus();
            var subscirbe = BuiltSubscribeDomain(subscribeBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@Cons_Id", subscirbe.Cons_Id),//0
           		 new SqlParameter("@Email", subscirbe.Email),//0
                 new SqlParameter("@Ipaddress", subscirbe.Ipaddress),//3
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//4
			};

            Params[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Consumer_UnSubscribeEmail", Params);
            int isExist = Convert.ToInt32(Params[3].Value);
            if (isExist == 0)
            {
                transactionStatus.Status = false;
            }
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }


        public DataSet PropertyComplete_Details(ListingDetailsBo listBo)
        {
            var transactionStatus = new TransactionStatus();
            var Listing = BuiltPropertyListingDetailsDomain(listBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@PropId",Listing.PropId),//0

                       new SqlParameter("@Room_Checkin",Listing.Room_Checkin),//1
                       new SqlParameter("@Room_Checkout",Listing.Room_Checkout),//2
                       new SqlParameter("@No_Of_Rooms",Listing.No_Of_Rooms),//3
                    
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Property_Complete_Detailed", Params);
            return ds;
        }

        public DataSet BookingHotel_Details(BookNowDetailsBo booknowBo)
        {
            var transactionStatus = new TransactionStatus();
            var booknow = BuiltBookNowDetailsDomain(booknowBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@PropId",booknow.Prop_Id),//0
                    new SqlParameter("@RoomId",booknow.Room_Id),//0
                    new SqlParameter("@Room_Checkin",booknow.Room_Checkin),//1
                    new SqlParameter("@Room_Checkout",booknow.Room_Checkout),//2
                    new SqlParameter("@No_Of_Rooms",booknow.No_Of_Rooms),//3
                    
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Booking_HotelDetailes_ById", Params);
            return ds;
        }

        public DataSet GetBookingInvoice(string Invce_Num, int Cons_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{              
                   new SqlParameter("@Invce_Num",Invce_Num),//0
                   new SqlParameter("@Cons_Id",Cons_Id),//0
			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectTransaction_by_ConsId]", Params);
            return ds;
        }

        public DataSet CheckBookingStatus(string Invce_Num, int Cons_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{              
                   new SqlParameter("@Invce_Num",Invce_Num),//0
                   new SqlParameter("@Cons_Id",Cons_Id),//0
			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectBookingStatus_by_ConsId]", Params);
            return ds;
        }

        public DataSet CheckCorporateUser(string Cons_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{          
                   new SqlParameter("@userid",Cons_Id),//0
			};


            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectCorporateUser_By_ConsId]", Params);
            return ds;
        }

        public DataSet GetActiveFacilities()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAllFacility]");
            return ds;
        }

        public DataSet GetLocationByCity(string name)
        {
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 

			{         
                   new SqlParameter("@name",name),//0
                 
       		};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_getlocationbycity]", Params);
            return ds;
        }
        public DataSet GetRoomPolicyById(int Prop_Id, int Room_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{         
                   new SqlParameter("@Prop_Id",Prop_Id),//0
                   new SqlParameter("@Room_Id",Room_Id),//0
       		};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Select_Policies_ByRoomId]", Params);
            return ds;
        }
        public DataSet GetRoomDetailsByID(int Prop_Id, int Room_Id)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 

			{         
                   new SqlParameter("@Prop_Id",Prop_Id),//0
                   new SqlParameter("@Room_Id",Room_Id),//0
       		};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Select_RoomDetailsbyID]", Params);
            return ds;
        }

        public DataSet GetHiddenGems()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_hiddengems]");
            return ds;
        }

        public DataSet GetRecommendedHotels()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_recommended]");
            return ds;
        }

        public DataSet GetBestOffers()
        {
            CemexDb con = new CemexDb();

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_bestoffers]");
            return ds;
        }

        //Service
        public TransactionStatus AddFeedBack(FeedBackBo feedbackBo)
        {
            var transactionStatus = new TransactionStatus();
            var feedback = BuiltFeedBackDomain(feedbackBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@Name", feedback.Name),//0
                 new SqlParameter("@Email", feedback.EmailAddress),//0
                 new SqlParameter("@Mobile", feedback.Mobile),//0
                 new SqlParameter("@Message", feedback.Message),//3
           	     new SqlParameter("@Date", System.DateTime.Now),//2
               
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//4
			};

            Params[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_AddFeedBack]", Params);
            //int isExist = Convert.ToInt32(Params[6].Value);
            //if (isExist == 0)
            //{
            //    transactionStatus.Status = false;
            //}
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }
        //Feedback down
        public TransactionStatus AddFeedBack_Feed(FeedBackBo feedbackBo)
        {
            var transactionStatus = new TransactionStatus();
            var feedback = BuiltFeedBackDomain(feedbackBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@opReturnValue", SqlDbType.Int),//4
                 new SqlParameter("@Name", feedback.Name),//0
                  new SqlParameter("@Email", feedback.EmailAddress),//0
                
                 new SqlParameter("@Message", feedback.Message),//3
           	     new SqlParameter("@Date", System.DateTime.Now)//2
                
                 
			};

            Params[0].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_AddFeedBack_Feed]", Params);

            return transactionStatus;
        }

        public List<object> GetAutoCompleteLocation(string terms)
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
                    City_Loc = reader["Location"].ToString() + ", " + reader["City"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    State = reader["State"].ToString(),
                });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstObj;
        }

        public List<object> GetStates()
        {
            List<Object> lstcity = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllCity");

            while (reader.Read())
            {
                lstcity.Add(
                    new
                    {
                        Id = reader["Id"].ToString(),
                        State = reader["State"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcity;
        }

        public List<object> GetPincodes()
        {
            List<Object> lstcity = new List<Object>();
            CemexDb con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllCity");

            while (reader.Read())
            {
                lstcity.Add(
                    new
                    {
                        Id = reader["Id"].ToString(),
                        Pincode = reader["Pincode"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcity;
        }
        public List<object> GetAutoCompleteLocationSearch(string terms)
        {

            List<object> lstObj = new List<object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@term",terms),//0          
			};

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectConsumerCity]", Params);

            while (reader.Read())
            {
                lstObj.Add(new
                {
                    //Id = reader["Id"].ToString(),
                    //City_Loc = reader["Location"].ToString() + ", " + reader["City"].ToString(),
                    //Pincode = reader["Pincode"].ToString(),
                    //State = reader["State"].ToString(),
                    City_Loc = reader["City_Name"].ToString(),
                });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstObj;
        }
        private ConsumerChangePassword BuiltChangePasswordDomain(ConsumerChangePasswordBo changepwdBo)
        {
            return (ConsumerChangePassword)new ConsumerChangePassword().InjectFrom(changepwdBo);
        }
        private ConsumerSubscribe BuiltSubscribeDomain(ConsumerSubscribeBo consumer)
        {
            return (ConsumerSubscribe)new ConsumerSubscribe().InjectFrom(consumer);
        }

        private ConsumerForm BuiltConsumerFormDomain(ConsumerFormBo consumer)
        {
            return (ConsumerForm)new ConsumerForm().InjectFrom(consumer);
        }

        private BookNowDetails BuiltBookNowDetailsDomain(BookNowDetailsBo booknowVM)
        {
            return (BookNowDetails)new BookNowDetails().InjectFrom(booknowVM);
        }

        private FeedBack BuiltFeedBackDomain(FeedBackBo feedbackBo)
        {
            return (FeedBack)new FeedBack().InjectFrom(feedbackBo);
        }
        #endregion



        private Consumer BuiltConsumerDomain(ConsumerBo consumer)
        {
            return (Consumer)new Consumer().InjectFrom(consumer);
        }
        private ConsumerMandet BuiltConsumerMandetDomain(ConsumerMandetBo consumer)
        {
            return (ConsumerMandet)new ConsumerMandet().InjectFrom(consumer);
        }
        private ConsumerLogin BuiltConsumerLoginDomain(ConsumerLoginBo login)
        {
            return (ConsumerLogin)new ConsumerLogin().InjectFrom(login);
        }
        private PropertyListing BuiltPropertyListingDomain(ListingBo list)
        {
            return (PropertyListing)new PropertyListing().InjectFrom(list);
        }
        private PropertyListingdetailsRoom BuiltPropertyListingDetailsRoomDomain(ListingDetailsRoomBo list)
        {
            return (PropertyListingdetailsRoom)new PropertyListingdetailsRoom().InjectFrom(list);
        }
        private PreBookingDomain BuiltPreBookingDomain(PrebookingBo list)
        {
            return (PreBookingDomain)new PreBookingDomain().InjectFrom(list);
        }
        private PropertyListingdetails BuiltPropertyListingDetailsDomain(ListingDetailsBo list)
        {
            return (PropertyListingdetails)new PropertyListingdetails().InjectFrom(list);
        }
        private ConsumerForgotpwd BuiltConsumerForgotpwdDomain(ConsumerForgotpwdBo fp)
        {
            return (ConsumerForgotpwd)new ConsumerForgotpwd().InjectFrom(fp);
        }
        private ConsumerDetails BuiltConsumerDetailsDomain(ConsumerDetailsBo login)
        {
            return (ConsumerDetails)new ConsumerDetails().InjectFrom(login);
        }
        private ConsumerBo BuiltConsumerBo(Consumer consumer)
        {
            return (ConsumerBo)new ConsumerBo().InjectFrom(consumer);
        }



    }

}
