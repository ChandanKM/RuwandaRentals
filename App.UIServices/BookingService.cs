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

    public class BookingService : RepositoryBase, IBookingService
    {
        public BookingService(IDatabaseFactory DbFactory)
            : base(DbFactory)
        {
        }


        //for Booking get

        public DataSet Bind(int VendID, string InvFrom, string InvTo)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            string VendIDs;
            if (VendID == 0)
            {
                VendIDs = "";
            }

            else
            {
                VendIDs = VendID.ToString();
            }
            InvFrom = InvFrom == null ? "" : InvFrom;
            InvTo = InvTo == null ? "" : InvTo;
            //Checkin = Checkin == null ? "" : Checkin;
            //Checkout = Checkout == null ? "" : Checkout;
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendIDs),
                     new SqlParameter("@InvFrom", InvFrom),
                      new SqlParameter("@InvTo", InvTo)//0
                   
                 
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAllTransactionByVendorID]", Params);
            return ds;
        }

        public DataSet corpBookinglist(string Cons_Id, string bookingStatus, string InvFrom, string InvTo)
        {
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
          
           

            
            InvFrom = InvFrom == null ? "" : InvFrom;
            InvTo = InvTo == null ? "" : InvTo;
   
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@Cons_Id", Cons_Id),
                      new SqlParameter("@bookingStatus", bookingStatus),
                     new SqlParameter("@InvFrom", InvFrom),
                      new SqlParameter("@InvTo", InvTo)//0
                   
                 
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectAllTransaction_ByCorporate", Params);
            return ds;
        }

        public DataSet ConsumerReport(string VendID, string cons_id, string cons_name, string cons_mailid, string cons_mobile, string days)
        {
            if (VendID == null)
                VendID = "";
            cons_id = "";
            cons_name = "";
            cons_mailid = "";
            cons_mobile = "";


            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendID),//0
                   
                  new SqlParameter("@cons_id", cons_id),//0
                   new SqlParameter("@cons_name", cons_name),//0
                    new SqlParameter("@cons_mailid", cons_mailid),//0
                     new SqlParameter("@cons_mobile", cons_mobile),//0
                      new SqlParameter("@days", days),//0
                     
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_consumer_report]", Params);
            return ds;
        }

        public DataSet Tax_Report(string VendID, string TaxType, string days)
        {
            TaxType = "";
            VendID = "";
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendID),//0
                   
                  new SqlParameter("@taxtype", TaxType),//0
                   new SqlParameter("@days", days),//0
                   
                     
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Tax_report]", Params);
            return ds;
        }
        public DataSet CCAvenue_Report(string VendID, string days)
        {

            VendID = "";
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendID),//0
                   
              
                   new SqlParameter("@days", days),//0
                   
                     
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_ccavenue_report]", Params);
            return ds;
        }
        public DataSet LMK_Margin_Report(string VendID, string fromdate, string todate)
        {

            VendID = VendID == null ? "" : VendID;
            fromdate = fromdate == null ? "" : fromdate;
            todate = todate == null ? "" : todate;
            var transactionStatus = new TransactionStatus();

            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendID),//0
                   
              
                   new SqlParameter("@invfrom", fromdate),
                   new SqlParameter("@invto", todate),//0
                   
                     
                      
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_margin_report]", Params);
            return ds;
        }

        public DataSet UpcomingBookingList(int VendID)
        {
            var transactionStatus = new TransactionStatus();
            string VendIDs;
            if (VendID == 0)
            {
                VendIDs = "";
            }
            else
            {
                VendIDs = VendID.ToString();
            }
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
                    new SqlParameter("@vndr_Id", VendIDs),//0

			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_Select_UpcomingBookedDeals_ByVendorID]", Params);
            return ds;
        }
        public List<Object> BindProperty()
        {

            List<Object> lstprop = new List<Object>();
            CemexDb con = new CemexDb();
            SqlParameter[] Params = 
			{ 
              //   new SqlParameter("@vendId", id),//0     
            };
            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectProperties");

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

        private PreBookingDomain BuiltPreBookingDomain(PrebookingBo list)
        {
            return (PreBookingDomain)new PreBookingDomain().InjectFrom(list);
        }





        
    }

}
