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


    public class FacilityService : RepositoryBase, IFacilityService  
    {
        public List<Object> BindFacility()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectAllFacility", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstfacility = new List<Object>();
            while (reader.Read())
            {
                lstfacility.Add(

                    new
                    {
                        Facility_Id = reader["Facility_Id"].ToString(),
                        Facility_Name = reader["Facility_Name"].ToString(),
                        Facility_Type = reader["Facility_Type"].ToString(),
                        Facility_descr = reader["Facility_descr"].ToString(),
                        Facility_Image_dir = reader["Facility_Image_dir"].ToString(),
                        //Facility_Active_flag = reader["Facility_Active_flag"].ToString()
                    });


            }
            conn.Close();


            return lstfacility;
        }

        public TransactionStatus CreateFacility(FacilityBo facilityBo)
        {
            var transactionStatus = new TransactionStatus();
            var facility = BuiltFacilityDomain(facilityBo);
            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_AddFacility", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Facility_Name", facilityBo.Facility_Name);
            cmd.Parameters.AddWithValue("@Facility_Type", facilityBo.Facility_Type);
            cmd.Parameters.AddWithValue("@Facility_descr", facilityBo.Facility_descr);
            cmd.Parameters.AddWithValue("@Facility_Image_dir", facilityBo.Facility_Image_dir);
            cmd.Parameters.AddWithValue("@Facility_Active_flag", "true");
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }

        public TransactionStatus DeleteFacility(FacilityBo facilityBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Facility_Id", facilityBo.Facility_Id),//0
				new SqlParameter("@opReturnValue", SqlDbType.Int),//1
			};

            Params[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_DeleteFacility", Params);
            return transactionStatus;
        }

        public List<object> Edit(string Id)
        {
            List<Object> lstcityloc = new List<Object>();
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Facility_Id", Id),//0
                
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
                        Facility_descr = reader["Facility_descr"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstcityloc;
        }

        public TransactionStatus EditFacility(FacilityBo bankBo)
        {
            var transactionStatus = new TransactionStatus();
            CemexDb con = new CemexDb();

            var facil = BuiltFacilityDomain(bankBo);
            
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                new SqlParameter("@Facility_Name", facil.Facility_Name),//2
                new SqlParameter("@Facility_Type", facil.Facility_Type),//3
				new SqlParameter("@Facility_descr", facil.Facility_descr),//4
                new SqlParameter("@Facility_Image_dir",facil.Facility_Image_dir),//5
               new SqlParameter("@Facility_Id",facil.Facility_Id),//5
			};

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateFacility", Params);
            return transactionStatus;
        }
        private Facility BuiltFacilityDomain(FacilityBo facilityBo)
        {
            return (Facility)new Facility().InjectFrom(facilityBo);
        }


      
    }
}
