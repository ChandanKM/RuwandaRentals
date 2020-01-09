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
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace App.UIServices
{
    public class UserProfileServices : RepositoryBase, IUserProfileServices
    {
        const string DESKey = "AQWSEDRF";
        const string DESIV = "HGFEDCBA";
        string oldPassword = "";
        public UserProfileServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }

        public DataSet AddUserProfile(UserProfileBo userprofileBo)
        {
            var transactionStatus = new TransactionStatus();
            var userprofile = BuiltUserProfileDomain(userprofileBo);
            string sbc = DESEncrypt(userprofile.Pswd);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@opReturnValue", SqlDbType.Int),//0       
                 new SqlParameter("@authority_Id", userprofile.Authority_Id),//0          		
                 new SqlParameter("@user_Name", userprofile.User_Name),//3
                 new SqlParameter("@firstname", userprofile.Firstname),//4
                 new SqlParameter("@lastname", userprofile.Lastname),//5
                 new SqlParameter("@pswd", DESEncrypt(userprofile.Pswd)),//6
                 new SqlParameter("@activated_By", "admin"),//7
                 new SqlParameter("@department","LMK"),//8
                 new SqlParameter("@created_On",System.DateTime.Now),//9
                 new SqlParameter("@modified_On",System.DateTime.Now),//10
                 new SqlParameter("@pswd_Reset_On",System.DateTime.Now),//11
                   new SqlParameter("@UserType",userprofile.UserType),//12    
			};
            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddUserProfile", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return ds;
        }

        public TransactionStatus UpdateUserProfile(UserProfileBo userprofileBo)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params1 = 
			{ 
                   //  new SqlParameter("@Authority_Id",AuthId),//10 @
                     new SqlParameter("@user_Id",userprofileBo.User_Id),//10 @
                    //  new SqlParameter("@PropId",PropId),//10 @
                  
                  
			};

            DataSet ds1 = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_GetUserPwsd", Params1);
            string oldpwd = ds1.Tables[0].Rows[0][0].ToString();
          
          //  return ds;
            var transactionStatus = new TransactionStatus();
            var userprofile = BuiltUserProfileDomain(userprofileBo);
              if(oldpwd==userprofile.Pswd)
              {
                  oldPassword = userprofile.Pswd;
              }
              else
              {
                  oldPassword = DESEncrypt(userprofile.Pswd);
              }
          //  string abc = DESEncrypt(userprofile.Pswd);
           // CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@opReturnValue", SqlDbType.Int),//12
                 new SqlParameter("@user_Id", userprofile.User_Id),//0
                 new SqlParameter("@authority_Id", userprofile.Authority_Id),//0
           		
                 new SqlParameter("@user_Name", userprofile.User_Name),//3
                 new SqlParameter("@firstname", userprofile.Firstname),//4
                 new SqlParameter("@lastname", userprofile.Lastname),//5
                 new SqlParameter("@pswd",oldPassword),//6
                 new SqlParameter("@activated_By", "admin"),//7
                 new SqlParameter("@department", "LMK"),//8
            
                 new SqlParameter("@modified_On",System.DateTime.Now),//10
                 new SqlParameter("@pswd_Reset_On",System.DateTime.Now)//11

                 
			};

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateUserProfile", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;
        }

        public TransactionStatus SuspendUserProfile(int userprofile_Id)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@user_id",userprofile_Id),//10 @
                     new SqlParameter("@active_flag","False"),//10 @
                     new SqlParameter("@modified_On",System.DateTime.Now),//10
                     new SqlParameter("@opReturnValue", SqlDbType.Int)//3
			};

            Params[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateUserProfileActive_flag", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            return transactionStatus;
        }

        public DataSet GetUserProfile(string AuthId, string UserId, string PropId)
        {
            if (UserId == null)
                UserId = "";
            if (PropId == null)
                PropId = "";
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@Authority_Id",AuthId),//10 @
                     new SqlParameter("@User_Type",UserId),//10 @
                      new SqlParameter("@PropId",PropId),//10 @
                  
                  
			};
          
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllUserProfile", Params);
            return ds;
        }


        public List<object> GetProfileMaster()
        {
            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectAllUser_Profile_Master", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> roomList = new List<Object>();
            while (reader.Read())
            {
                roomList.Add(
                    new
                    {
                        Authority_Id = reader["Authority_Id"].ToString(),
                        Code = reader["Code"].ToString(),
                    });
            }
            conn.Close();
            return roomList;
        }

        private UserProfile BuiltUserProfileDomain(UserProfileBo userprofileBo)
        {
            return (UserProfile)new UserProfile().InjectFrom(userprofileBo);
        }

        private UserProfileBo BuiltUserProfileBo(UserProfile userprofile)
        {
            return (UserProfileBo)new UserProfileBo().InjectFrom(userprofile);
        }


        #region Permission

        public DataSet GetUserPermission(int authId, int userId)
        {
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@authorityId",authId),
                      new SqlParameter("@userId",userId),
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Select_PagesbyUserId", Params);
            return ds;
        }

        public DataSet GetSuperAdminsPermission(int authId, int userId)
        {
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                     new SqlParameter("@authorityId",authId),
                      new SqlParameter("@userId",userId),
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Select_SuperAdminPagesbyUserId]", Params);
            return ds;
        }

        public TransactionStatus UpdatePermissionFlag(int userId, int pageId, string flag)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@user_Id", userId),//0     
                     new SqlParameter("@page_Id", pageId),//0        
                     new SqlParameter("@active_Flag",flag)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdatePermission_flag", Params);

            return transactionStatus;
        }
        public TransactionStatus UpdateSuperAdminPermissionFlag(int userId, int pageId, string flag)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@user_Id", userId),//0     
                     new SqlParameter("@page_Id", pageId),//0        
                     new SqlParameter("@active_Flag",flag)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateSuperAdminPermission_flag", Params);

            return transactionStatus;
        }
        #endregion

        #region Encryption
        public byte[] GetEncryptedValue(string value)
        {
            try
            {
                byte[] hashedByte;
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                UTF8Encoding encoder = new UTF8Encoding();
                hashedByte = md5Hasher.ComputeHash(encoder.GetBytes(value.Trim()));
                return hashedByte;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }




        public static string DESEncrypt(string stringToEncrypt)// Encrypt the content
        {


            byte[] key;
            byte[] IV;


            byte[] inputByteArray;
            try
            {


                key = Convert2ByteArray(DESKey);


                IV = Convert2ByteArray(DESIV);


                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                MemoryStream ms = new MemoryStream(); CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);


                cs.FlushFinalBlock();


                return Convert.ToBase64String(ms.ToArray());
            }


            catch (System.Exception ex)
            {


                throw ex;
            }


        }


        public static string DESDecrypt(string stringToDecrypt)//Decrypt the content
        {


            byte[] key;
            byte[] IV;


            byte[] inputByteArray;
            try
            {


                key = Convert2ByteArray(DESKey);


                IV = Convert2ByteArray(DESIV);


                int len = stringToDecrypt.Length; inputByteArray = Convert.FromBase64String(stringToDecrypt);




                DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                MemoryStream ms = new MemoryStream(); CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);


                cs.FlushFinalBlock();


                Encoding encoding = Encoding.UTF8; return encoding.GetString(ms.ToArray());
            }


            catch (System.Exception ex)
            {


                throw ex;
            }










        }
        static byte[] Convert2ByteArray(string strInput)
        {


            int intCounter; char[] arrChar;
            arrChar = strInput.ToCharArray();


            byte[] arrByte = new byte[arrChar.Length];


            for (intCounter = 0; intCounter <= arrByte.Length - 1; intCounter++)
                arrByte[intCounter] = Convert.ToByte(arrChar[intCounter]);


            return arrByte;
        }
        #endregion
    }
}
