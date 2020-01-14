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
using System.Security.Cryptography;
using System.Text;
using System.IO;
namespace App.UIServices
{
    public class LoginServices : RepositoryBase, ILoginServices
    {
        const string DESKey = "AQWSEDRF";
        const string DESIV = "HGFEDCBA";
        public LoginServices(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        public List<string> AuthenticateUser(LoginBo loginBo)
        {
            var login = BuiltLoginDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_AuthenticateUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", login.UserId);

            //Encryptions and decryptions of the password in the application level.
            //var encrypt = DESEncrypt(login.Pswd);
            //var decrypt = DESDecrypt(encrypt);

            cmd.Parameters.AddWithValue("@pswd", DESEncrypt(login.Pswd));
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> lst = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lst.Add(reader["User_Id"].ToString());
                    lst.Add(reader["Authority_Id"].ToString());
                    lst.Add(reader["User_Type"].ToString());
                }
                conn.Close();
            }
            return lst;
        }
        public List<string> AuthenticateUserRole(string username)
        {
          
            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("[proc_AuthenticateUserRole]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", username);
         
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> lst = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    
                    lst.Add(reader["Authority_Id"].ToString());
                  
                }
                conn.Close();
            }
            return lst;
        }
        public ForgotPassword FindUserByEmail(ForgotPasswordBo forgotpasswordBo)
        {
            var fogotpassword = BuiltForgotPasswordDomain(forgotpasswordBo);
            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_FindUser_byEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", fogotpassword.Email);
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            SqlDataReader reader = cmd.ExecuteReader();
            ForgotPassword lst = new ForgotPassword();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lst.Email = reader["User_Name"].ToString();
                    lst.Id = Convert.ToInt32(reader["User_Id"]);
                }
                conn.Close();
            }
            return lst;
        }

        public EmailMaster EmailCredentials()
        {

            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_GetEmailMaster_Credetials", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            EmailMaster email = new EmailMaster();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    email.Email = reader["email"].ToString();
                    email.Password = reader["pswd"].ToString();
                    email.SMTP = reader["smtp"].ToString();
                    email.Pop = reader["pop"].ToString();
                    email.Port = Convert.ToInt32(reader["port"]);
                }
                conn.Close();
            }
            return email;
        }

        private Login BuiltLoginDomain(LoginBo loginBo)
        {
            return (Login)new Login().InjectFrom(loginBo);
        }

        private LoginBo BuiltLoginBo(Login login)
        {
            return (LoginBo)new LoginBo().InjectFrom(login);
        }

        private ForgotPassword BuiltForgotPasswordDomain(ForgotPasswordBo forgotpasswordBo)
        {
            return (ForgotPassword)new ForgotPassword().InjectFrom(forgotpasswordBo);
        }

        public TransactionStatus ResetPassword(ResetPasswordBo resetpswdBo)
        {
            var transactionStatus = new TransactionStatus();
            var resetpassword = BuiltResetPasswordDomain(resetpswdBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{                
           		 new SqlParameter("@user_Id", resetpassword.User_Id),//0             
                 new SqlParameter("@pswd",DESEncrypt(resetpassword.Password)),//1           
                 new SqlParameter("@pswd_Reset_On",System.DateTime.Now),//2
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//3
			};

            Params[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_Reset_UserPassword", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return transactionStatus;

        }

        private ResetPassword BuiltResetPasswordDomain(ResetPasswordBo passwordBo)
        {
            return (ResetPassword)new ResetPassword().InjectFrom(passwordBo);
        }

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
