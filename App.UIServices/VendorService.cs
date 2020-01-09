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
    public class VendorService : RepositoryBase, IVendorService
    {
        public VendorService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        //create Vendor
        public TransactionStatus CreateVendor(VendorBo vendorBo)
        {
            var transactionStatus = new TransactionStatus();
            var vendor = BuiltVendorDomain(vendorBo);
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            int value = 1;
            string flag = "true";
            string LMK = "LMK";
            //Image upload
            #region using  sql helper
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                  new SqlParameter("@opReturnValue", value),//32
            new SqlParameter("@CityID", vendor.City_Id),//0
        
            new SqlParameter("@Vndr_Name", vendor.Vndr_Name),//6
            new SqlParameter("@Vndr_Cinno", vendor.Vndr_Cinno),//7
            new SqlParameter("@Vndr_Addr1", vendor.Vndr_Addr1),//8
       
            new SqlParameter("@Vndr_Gps_Pos", "0"),//10
            new SqlParameter("@Vndr_Lanline_Nos", vendor.Vndr_Lanline_Nos),//11
            new SqlParameter("@Vndr_Mobile_Nos", vendor.Vndr_Mobile_Nos),//12
            new SqlParameter("@Vndr_Overview", "0"),//13
            new SqlParameter("@Vndr_created_on", dt),//14
            new SqlParameter("@Vndr_Activated_on", dt),//15
            new SqlParameter("@Vndr_Commercial_Estab_Flag", flag),//16
            new SqlParameter("@Vndr_Verfied_By", LMK),//17
            new SqlParameter("@Vndr_Verfied_on", dt),//18
            new SqlParameter("@Vndr_Approved_By", LMK),//19
            new SqlParameter("@Vndr_Approved_on",dt ),//20
            new SqlParameter("@Vndr_Active_Flag", flag),//21
            new SqlParameter("@Vndr_Contact_person", vendor.Vndr_Contact_person),//23
            new SqlParameter("@Vndr_Contact_Email", vendor.Vndr_Contact_Email),//24
            new SqlParameter("@Vndr_Contact_Nos", vendor.Vndr_Contact_Nos),//25
            new SqlParameter("@Vndr_Alternate_person", vendor.Vndr_Alternate_person),//26
            new SqlParameter("@Vndr_Alternate_Email", vendor.Vndr_Alternate_Email),//27
            new SqlParameter("@Vndr_Alternate_Nos", vendor.Vndr_Alternate_Nos),//28
         
                new SqlParameter("@Image_Name", "ZXZ"),//29
                   new SqlParameter("@Image_dir", vendorBo.Image_dir),//30
                      new SqlParameter("@Image_Created_on", DateTime.Now),//31
         new SqlParameter("@Vndr_Contact_Mobile", vendor.Vndr_Contact_Mobile),//28
         new SqlParameter("@Vndr_Contact_Designation", vendor.Vndr_Contact_Designation),//28
         new SqlParameter("@Vndr_Alternate_Mobile", vendor.Vndr_Alternate_Mobile),//28
         new SqlParameter("@Vndr_Alternate_Designation", vendor.Vndr_Alternate_Designation),//28
           new SqlParameter("@UserProfile_Id", vendor.UserProfile_Id),//28
        
          
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddVendorDetails", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[0].Value.ToString();
            transactionStatus.Id = Convert.ToInt32(test);
            return transactionStatus;

            #endregion




        }

        //Get Vendor
        public List<Object> Bind(int id)
        {

            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectVendorById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Vndr_Id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstvendor = new List<Object>();
            while (reader.Read())
            {
                lstvendor.Add(

                    new
                    {


                        Vndr_Id = reader["Vndr_Id"].ToString(),
                        Vndr_Name = reader["Vndr_Name"].ToString(),
                        Vndr_Cinno = reader["Vndr_Cinno"].ToString(),
                        Vndr_Addr1 = reader["Vndr_Addr1"].ToString(),
                        Vndr_Gps_Pos = reader["Vndr_Gps_Pos"].ToString(),
                        Vndr_Overview = reader["Vndr_Overview"].ToString(),
                        Vndr_Contact_person = reader["Vndr_Contact_person"].ToString(),
                        Vndr_Contact_Email = reader["Vndr_Contact_Email"].ToString(),
                        Vndr_Contact_Nos = reader["Vndr_Contact_Nos"].ToString(),
                        Vndr_Mobile_Nos = reader["Vndr_Mobile_Nos"].ToString(),
                        Vndr_Lanline_Nos = reader["Vndr_Lanline_Nos"].ToString(),
                        Vndr_Alternate_person = reader["Vndr_Alternate_person"].ToString(),
                        Vndr_Alternate_Email = reader["Vndr_Alternate_Email"].ToString(),
                        Vndr_Alternate_Nos = reader["Vndr_Alternate_Nos"].ToString(),
                          Vndr_Alternate_Mobile = reader["Vndr_Alternate_Mobile"].ToString(),
                            Vndr_Alternate_Designation = reader["Vndr_Alternate_Designation"].ToString(),
                        Vndr_Contact_Mobile = reader["Vndr_Contact_Mobile"].ToString(),
                        Vndr_Contact_Designation = reader["Vndr_Contact_Designation"].ToString(),
                        Vndr_Profile_Image= reader["Image_dir"].ToString(),
                        City_Id = reader["Id"].ToString(),
                        City_Area = reader["Location"].ToString(),
                        City_Name = reader["City"].ToString(),
                        State_Name = reader["State"].ToString(),
                        Pincode = reader["pincode"].ToString(),
                    });


            }
            conn.Close();


            return lstvendor;


        }
        //Edit Vendor
        public TransactionStatus EditVendor(VendorEditBo vendoreditBo)
        {
            var transactionStatus = new TransactionStatus();
            var vendor = BuiltVendorDomain1(vendoreditBo);
            #region using  sql helper

            DateTime dt = new DateTime();
            dt = DateTime.Now;
            int value = 1;
            string flag = "true";
            string LMK = "LMK";
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                   new SqlParameter("@opReturnValue", value),//30
            new SqlParameter("@Vndr_Id", vendor.Vndr_Id),//0
           
            new SqlParameter("@Vndr_Name", vendor.Vndr_Name),//6
            new SqlParameter("@Vndr_Cinno", vendor.Vndr_Cinno),//7
            new SqlParameter("@Vndr_Addr1", vendor.Vndr_Addr1),//8
            new SqlParameter("@Vndr_Gps_Pos", vendor.Vndr_Gps_Pos),//10
         //   new SqlParameter("@Vndr_Lanline_Nos", vendor.Vndr_Lanline_Nos),//11
       
            new SqlParameter("@Vndr_Overview", vendor.Vndr_Overview),//13
            new SqlParameter("@Vndr_created_on", dt),//14
            new SqlParameter("@Vndr_Activated_on", dt),//15
            new SqlParameter("@Vndr_Commercial_Estab_Flag", flag),//16
            new SqlParameter("@Vndr_Verfied_By", LMK),//17
            new SqlParameter("@Vndr_Verfied_on", dt),//18
            new SqlParameter("@Vndr_Approved_By", LMK),//19
            new SqlParameter("@Vndr_Approved_on",dt ),//20
            new SqlParameter("@Vndr_Active_Flag", flag),//21
            new SqlParameter("@Vndr_Contact_person", vendor.Vndr_Contact_person),//23
            new SqlParameter("@Vndr_Contact_Email", vendor.Vndr_Contact_Email),//24
            new SqlParameter("@Vndr_Contact_Nos", vendor.Vndr_Contact_Nos),//25
            new SqlParameter("@Vndr_Alternate_person", vendor.Vndr_Alternate_person),//26
            new SqlParameter("@Vndr_Alternate_Email", vendor.Vndr_Alternate_Email),//27
            new SqlParameter("@Vndr_Alternate_Nos", vendor.Vndr_Alternate_Nos),//28
            new SqlParameter("@Image_dir", vendoreditBo.Image_dir),//29
               new SqlParameter("@Vndr_Alternate_Mobile", vendor.Vndr_Alternate_Mobile),//30
                 new SqlParameter("@Vndr_Alternate_Designation", vendor.Vndr_Alternate_Designation),//31
                  new SqlParameter("@Vndr_Contact_Mobile", vendor.Vndr_Contact_Mobile),//33
                 new SqlParameter("@Vndr_Contact_Designation", vendor.Vndr_Contact_Designation),//33
                   new SqlParameter("@CityID", vendor.City_Id),//33
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateVendorDetails", Params);
            ds.Locale = CultureInfo.InvariantCulture;
            string test = Params[0].Value.ToString();

            return transactionStatus;

            #endregion
        }

        //Delete DeleteVendor
        public TransactionStatus DeleteVendor(VendorEditBo vendorEditBo)
        {
            var transactionStatus = new TransactionStatus();
            var vendor = BuiltVendorDomain1(vendorEditBo);


            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.Proc_DeleteUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(vendorEditBo.Vndr_Id));

            cmd.ExecuteNonQuery();
            return transactionStatus;
        }


        //Get Vendor to Edit
        public List<Object> Edit(int Id)
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("proc_SelectVendorById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Vndr_Id", Id);

            SqlDataReader reader = cmd.ExecuteReader();
            List<Object> lstvendor = new List<Object>();
            while (reader.Read())
            {
                lstvendor.Add(

                    new
                    {
                        Vndr_Id = reader["Vndr_Id"].ToString(),
                        Vndr_Name = reader["Vndr_Name"].ToString(),
                        Vndr_Cinno = reader["Vndr_Cinno"].ToString(),
                        Vndr_Addr1 = reader["Vndr_Addr1"].ToString(),
                        Vndr_Gps_Pos = reader["Vndr_Gps_Pos"].ToString(),
                        Vndr_Overview = reader["Vndr_Overview"].ToString(),
                        Vndr_Contact_person = reader["Vndr_Contact_person"].ToString(),
                        Vndr_Contact_Email = reader["Vndr_Contact_Email"].ToString(),
                        Vndr_Contact_Nos = reader["Vndr_Contact_Nos"].ToString(),
                        Vndr_Alternate_person = reader["Vndr_Alternate_person"].ToString(),
                        Vndr_Alternate_Email = reader["Vndr_Alternate_Email"].ToString(),
                        Vndr_Alternate_Nos = reader["Vndr_Alternate_Nos"].ToString(),
                        Vndr_Alternate_Mobile = reader["Vndr_Alternate_Mobile"].ToString(),
                        Vndr_Alternate_Designation = reader["Vndr_Alternate_Designation"].ToString(),
                        Vndr_Contact_Mobile = reader["Vndr_Contact_Mobile"].ToString(),
                        Vndr_Contact_Designation = reader["Vndr_Contact_Designation"].ToString(),
                        Image_dir = reader["Image_dir"].ToString(),
                        City_Id = reader["Id"].ToString(),
                        City_Area = reader["Location"].ToString(),
                        City_Name = reader["City"].ToString(),
                        State_Name = reader["State"].ToString(),
                        Pincode = reader["pincode"].ToString(),

                    });


            }
            conn.Close();


            return lstvendor;
        }

        //Rate Calender
        public List<Object> BindPropertyByVendorId(int id)
        {

            List<Object> lstprop = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@vendId", id),//0     
            };
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectProperties_ByVendorId", Params);

            while (reader.Read())
            {
                lstprop.Add(
                    new
                    {
                        PropId = reader["Prop_Id"].ToString(),
                        PropName = reader["Prop_Name"].ToString(),
                    });
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return lstprop;
        }

        public void ExecuteAddRoomTimer()
        {
            CemexDb con = new CemexDb();

            SqlConnection conn = con.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_AddRoomInventoryTimer1", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            //  cmd.Parameters.AddWithValue("@vendorId", Convert.ToInt32(vendorId));

            cmd.ExecuteNonQuery();
        }

        private Vendor BuiltVendorDomain(VendorBo vendorBo)
        {
            return (Vendor)new Vendor().InjectFrom(vendorBo);
        }
        private VendorEdit BuiltVendorDomain1(VendorEditBo userBo)
        {
            return (VendorEdit)new VendorEdit().InjectFrom(userBo);
        }
        private VendorBo BuiltVendorBo(Vendor vendor)
        {
            return (VendorBo)new UserBo().InjectFrom(vendor);
        }
        private VendorBo BuiltVendorEditBo(Vendor vendor)
        {
            return (VendorBo)new VendorEditBo().InjectFrom(vendor);
        }

        #region RoomRateCalender
        public DataSet GetRoomsRate(int propId)
        {
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                 new SqlParameter("@prop_Id", propId),//0          		
  			};
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllRoomRatesByPropId", Params);
            ds.Locale = CultureInfo.InvariantCulture;

            return ds;
        }

        public TransactionStatus updaterackRates(int Inv_Id, int Vndr_Amnt)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@inv_Id", Inv_Id),//0        
                     new SqlParameter("@Vndr_Amnt",Vndr_Amnt)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateRack_Amount", Params);

            return transactionStatus;
        }

        public TransactionStatus UpdateRoomRates(int Inv_Id, int Price)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@inv_Id", Inv_Id),//0        
                     new SqlParameter("@lmk_Amnt",Price)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateInventryRoom_Amount", Params);

            return transactionStatus;
        }

        public TransactionStatus UpdateAvailableRoom(int Inv_Id, int Available)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
            { 	
                     new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                     new SqlParameter("@inv_Id", Inv_Id),//0        
                     new SqlParameter("@available",Available)//0        
            };

            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_UpdateInventry_Available_Room", Params);

            return transactionStatus;
        }

        private RateCalender BuiltRoomInventryDomain(RateCalenderBo rateBo)
        {
            return (RateCalender)new RateCalender().InjectFrom(rateBo);
        }
        #endregion


    }
}
