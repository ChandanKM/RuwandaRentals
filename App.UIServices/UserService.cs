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

    public class UserService : RepositoryBase, IUserService
    {
        public UserService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }
        //create User
        public TransactionStatus CreateUser(UserBo userBo)
        {
            var transactionStatus = new TransactionStatus();
            var user = BuiltUserDomain(userBo);


            CemexDb.User.Add(user);
            CemexDb.SaveChanges();

            return transactionStatus;
        }
        //Edit User
        public TransactionStatus EditUser(UserBo1 userBo)
        {
            var transactionStatus = new TransactionStatus();
            var user = BuiltUserDomain1(userBo);


            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_EditUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(userBo.Id));
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        //Delete User
        public TransactionStatus DeleteUser(UserBo1 userBo)
        {
            var transactionStatus = new TransactionStatus();
            var user = BuiltUserDomain1(userBo);


            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.Proc_DeleteUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(userBo.Id));

            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        //Get Users
        public List<Object> Bind()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_GetUsers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstuser = new List<Object>();
            while (reader.Read())
            {
                lstuser.Add(

                    new
                    {
                        Id = reader["Id"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Password = reader["Password"].ToString(),
                        UserName = reader["UserName"].ToString()
                    });


            }
            conn.Close();


            return lstuser;
        }

        //Get User to Edit
        public List<Object> Edit(int Id)
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_GetUsersById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstuser = new List<Object>();
            while (reader.Read())
            {
                lstuser.Add(

                    new
                    {
                        Id = reader["Id"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Password = reader["Password"].ToString(),
                        UserName = reader["UserName"].ToString()
                    });


            }
            conn.Close();


            return lstuser;
        }


        //for connection string

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
        //for Add Consumer

        public TransactionStatus AddConsumer(ConsumerBo consumerBo)
        {

            var transactionStatus = new TransactionStatus();
            var consumer = BuiltConsumerDomain(consumerBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@Cons_First_Name", consumer.Cons_First_Name),//0
              new SqlParameter("@Cons_Last_Name", consumer.Cons_Last_Name),//1
                new SqlParameter("@Cons_Gender", consumer.Cons_Gender),//2
                  new SqlParameter("@Cons_Dob", consumer.Cons_Dob),//3
                    new SqlParameter("@Cons_mailid", consumer.Cons_mailid),//4
                     new SqlParameter("@Cons_Pswd", consumer.Cons_Pswd),//5
                      new SqlParameter("@Cons_Mobile", consumer.Cons_Mobile),//6
                        new SqlParameter("@Cons_Addr1", consumer.Cons_Addr1),//7
                            new SqlParameter("@Cons_Addr2", consumer.Cons_Addr2),//8
                             new SqlParameter("@Cons_City", consumer.Cons_City),//9
                             new SqlParameter("@Cons_Area", consumer.Cons_Area),//10
                             new SqlParameter("@Cons_Pincode", consumer.Cons_Pincode),//11
                              new SqlParameter("@Cons_Company", consumer.Cons_Company),//12
                              new SqlParameter("@Cons_Company_Id", consumer.Cons_Company_Id),//13
                                new SqlParameter("@Cons_Reference", consumer.Cons_Reference),//14
                                  new SqlParameter("@Cons_Affiliates_Id", consumer.Cons_Affiliates_Id),//15
                                   new SqlParameter("@Cons_Loyalty_Id", consumer.Cons_Loyalty_Id),//16
                                    new SqlParameter("@Cons_Earned_Loyalpoints", consumer.Cons_Earned_Loyalpoints),//17
                                     new SqlParameter("@Cons_Redeemed_Loyalpoints1", consumer.Cons_Redeemed_Loyalpoints1),//18
                                      new SqlParameter("@Cons_Ipaddress", consumer.Cons_Ipaddress),//19
                                        new SqlParameter("@Cons_regist_On", consumer.Cons_regist_On),//20
                                          new SqlParameter("@Cons_Active_flag", consumer.Cons_Active_flag),//21
                                             new SqlParameter("@opReturnValue", SqlDbType.Int)//22
			};

            Params[22].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddConsumer", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[22].Value.ToString();

            return transactionStatus;


        }
        private User BuiltUserDomain(UserBo userBo)
        {
            return (User)new User().InjectFrom(userBo);
        }
        private User1 BuiltUserDomain1(UserBo1 userBo)
        {
            return (User1)new User1().InjectFrom(userBo);
        }

        private UserBo BuiltUserBo(User user)
        {
            return (UserBo)new UserBo().InjectFrom(user);
        }
        private UserBo BuiltUserBo1(User user)
        {
            return (UserBo)new UserBo1().InjectFrom(user);
        }

        private CityMaster BuiltCityMasterDomain(CityMasterBo cityMasterBo)
        {
            return (CityMaster)new CityMaster().InjectFrom(cityMasterBo);
        }
        private Consumer BuiltConsumerDomain(ConsumerBo consumer)
        {
            return (Consumer)new Consumer().InjectFrom(consumer);
        }
        private CityMasterBo BuiltCityMasterBo(CityMaster cityMaster)
        {
            return (CityMasterBo)new CityMasterBo().InjectFrom(cityMaster);
        }
        private ConsumerBo BuiltConsumerBo(Consumer consumer)
        {
            return (ConsumerBo)new ConsumerBo().InjectFrom(consumer);
        }


    }

}
