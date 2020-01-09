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
    public class CityService:RepositoryBase,ICityService
    {
        public CityService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }
        public TransactionStatus CreateCity(CityBo cityBo)
        {
            var transactionStatus = new TransactionStatus();
            var city = BuiltCityDomain(cityBo);

            //CemexDb.City.Add(city);
            //CemexDb.SaveChanges();

            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_AddCity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City_Name", cityBo.City_Name);
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        public TransactionStatus EditCity(CityBo1 cityBo)
        {
            var transactionStatus = new TransactionStatus();
            var city = BuiltCityDomain1(cityBo);

            CemexDb con = new CemexDb();
            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_UpdateCity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City_Id", Convert.ToInt32(cityBo.City_Id));
            cmd.Parameters.AddWithValue("@City_Name", city.City_Name);
            cmd.Parameters.AddWithValue("@opReturnValue", SqlDbType.Int);
            cmd.ExecuteNonQuery();
            return transactionStatus;
        }
        public List<Object> Bind()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectAllCity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstuser = new List<Object>();
            while (reader.Read())
            {
                lstuser.Add(

                    new
                    {
                        City_Id = reader["City_Id"].ToString(),
                        City_Name = reader["City_Name"].ToString()
                    });
            }
            conn.Close();
            return lstuser;
        }
        public List<Object> Edit(int Id)
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectCityById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City_Id", Id);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstuser = new List<Object>();
            while (reader.Read())
            {
                lstuser.Add(

                    new
                    {
                        City_Id = reader["City_Id"].ToString(),
                        City_Name = reader["City_Name"].ToString()
                    });
            }
            conn.Close();
            return lstuser;
        }

        

        //for connection string
        private City BuiltCityDomain(CityBo cityBo)
        {
            return (City)new City().InjectFrom(cityBo);
        }
        private City1 BuiltCityDomain1(CityBo1 userBo)
        {
            return (City1)new City1().InjectFrom(userBo);
        }
        private CityBo BuiltCityBo(City city)
        {
            return (CityBo)new CityBo().InjectFrom(city);
        }
        private CityBo BuiltCityBo1(City city)
        {
            return (CityBo)new CityBo1().InjectFrom(city);
        }


    }
}
