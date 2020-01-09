using App.BusinessObject;
using App.Common;
using App.DataAccess;
using App.Domain;
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
    public class CorporateService : RepositoryBase, ICorporateService
    {
        //Consumer Login
        public DataSet CorporateLogin(CorporateLoginBo loginBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltCorporateLoginDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Corp_mailid", consumer.Corp_mailid),//0
                     new SqlParameter("@Corp_Pswd", consumer.Corp_Pswd),//1
                      
			};
            if (!String.IsNullOrEmpty(consumer.Corp_mailid))
            {
                Params[0].Value = consumer.Corp_mailid;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Corp_Pswd))
            {
                Params[1].Value = consumer.Corp_Pswd;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_CorporateLogin", Params);
            return ds;
        }

        //FB Login
        public DataSet FbCorporateLogin(CorporateLoginBo loginBo)
        {
            var transactionStatus = new TransactionStatus();
            var consumer = BuiltCorporateLoginDomain(loginBo);
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Corp_mailid", consumer.Corp_mailid),//0
                     new SqlParameter("@Corp_Pswd", consumer.Corp_Pswd),//1
                      
			};
            if (!String.IsNullOrEmpty(consumer.Corp_mailid))
            {
                Params[0].Value = consumer.Corp_mailid;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(consumer.Corp_Pswd))
            {
                Params[1].Value = consumer.Corp_Pswd;
            }
            else
            {
                Params[1].Value = DBNull.Value;
            }
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_FbConsumerLogin", Params);
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

        private CorporateLogin BuiltCorporateLoginDomain(CorporateLoginBo login)
        {
            return (CorporateLogin)new CorporateLogin().InjectFrom(login);
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



        public List<object> GetAllCorporateUser(string EmailId)
        {

            List<Object> lstcorp = new List<Object>();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_mailid",EmailId),//0                              
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectConsumer_company", Params);

            while (reader.Read())
            {
                lstcorp.Add(
                    new
                    {
                        First_Name = reader["Cons_First_Name"].ToString(),
                        Last_Name = reader["Cons_Last_Name"].ToString(),
                        Mobile = reader["Cons_Mobile"].ToString(),
                        Company = reader["Cons_Company"].ToString(),
                        mailid = reader["Cons_mailid"].ToString()

                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcorp;

        }


        public List<object> GetAllCorporateUserByCompany(string company)
        {

            List<Object> lstcorp = new List<Object>();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_company",company),//0                              
			};
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectConsumerByCompany", Params);

            while (reader.Read())
            {
                lstcorp.Add(
                    new
                    {
                        First_Name = reader["Cons_First_Name"].ToString(),
                        Last_Name = reader["Cons_Last_Name"].ToString(),
                        Mobile = reader["Cons_Mobile"].ToString(),
                        Company = reader["Cons_Company"].ToString(),
                        mailid = reader["Cons_mailid"].ToString(),
                        isAdmin = reader["isAdmin"].ToString()

                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcorp;

        }

        public List<string> GetAllCorporateCompanies()
        {

            List<string> lstcorpCompany = new List<string>();

            CemexDb con = new CemexDb();

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectCoporateCompany");

            while (reader.Read())
            {
                lstcorpCompany.Add(reader["cons_company"].ToString());
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcorpCompany;

        }

        public bool UpdateCorporateUserToAdmin(string CorpEmail, string CorpCompany)
        {

            List<string> lstcorpCompany = new List<string>();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Corp_Email",CorpEmail),//0    
                    new SqlParameter("@Corp_Company",CorpCompany),      
			};

            SqlHelper.ExecuteScalar(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateCoporateUserToAdmin", Params);

           
            return true;

        }
    }
}
