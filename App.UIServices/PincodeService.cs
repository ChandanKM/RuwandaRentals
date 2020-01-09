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
    public class PincodeService : RepositoryBase, IPincodeService
    {
        public PincodeService(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        public TransactionStatus CreatePincode(PincodeBo pincodeBo)
        {
            var transactionStatus = new TransactionStatus();

            var con = new CemexDb();
            //var pinCode = BuiltPincodeDomain(pincodeBo);

            //CemexDb.Pincode.Add(pinCode);
            //CemexDb.SaveChanges();

            SqlParameter[] Params = 
			{ 
				 new SqlParameter("@Pincode",SqlDbType.NVarChar),//0
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//1
			};

            if (!String.IsNullOrEmpty(pincodeBo.Pincode))
            {
                Params[0].Value = pincodeBo.Pincode;
            }
            else
            {
                Params[0].Value = DBNull.Value;
            }

            Params[1].Direction = ParameterDirection.Output;
            //string conStr = ConfigurationManager.ConnectionStrings["conCityOfLongBeach"].ToString();
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPincode", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            //return Params[5].Value.ToString();

            return transactionStatus;
        }

        public TransactionStatus EditPincode(EditPincodeBo editpincodeBo)
        {
            var transactionStatus = new TransactionStatus();
            var editPincode = BuiltEditPincodeDomain(editpincodeBo);

            var con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Pincode_Id",editpincodeBo.PincodeId),//0
                 new SqlParameter("@Pincode",editPincode.Pincode),//1
                 new SqlParameter("@opReturnValue", SqlDbType.Int)//2
			};

            Params[2].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdatePincode", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            //return Params[5].Value.ToString();
            return transactionStatus;
        }

        public List<object> BindPincode()
        {
            var con = new CemexDb();
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllPincode");
            var lstPincode = new List<Object>();
            while (reader.Read())
            {
                lstPincode.Add(

                    new
                    {
                        PincodeId = reader["Pincode_Id"].ToString(),
                        Pincode = reader["Pincode"].ToString()
                    });
            }

            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstPincode;
        }

        public List<object> Edit(string id)
        {
            var con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Pincode_Id",id),//0
                
			};

           
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectPincodeById", Params);
            var lstPincode = new List<Object>();
            while (reader.Read())
            {
                lstPincode.Add(

                    new
                    {
                        PincodeId = reader["Pincode_Id"].ToString(),
                        Pincode = reader["Pincode"].ToString()
                    });
            }

            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstPincode;
        }

        private Lmk_Pincode BuiltPincodeDomain(PincodeBo pincodeBo)
        {
            return (Lmk_Pincode)new Lmk_Pincode().InjectFrom(pincodeBo);
        }

        private EditPincode BuiltEditPincodeDomain(EditPincodeBo editpincodeBo)
        {
            return (EditPincode)new EditPincode().InjectFrom(editpincodeBo);
        }
    }
}
