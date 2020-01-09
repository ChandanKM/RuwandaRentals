using App.UIServices;
using CCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.UIServices;
using App.BusinessObject;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace App.Web.Controllers
{
    public class ResponseController : Controller
    {

        readonly IConsumerService _consumerservices;

        public ResponseController(IConsumerService consumerservices)
        {
            _consumerservices = consumerservices;
        }
        // GET: Respone
        public async Task<ActionResult> Index()
        
        {


            try
            {
                string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here
                CCACrypto ccaCrypto = new CCACrypto();
                string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
                reservationsresponse res = await SendJsonToStaah(encResponse);
                //reservationsresponse res = await SendJsonToStaah("");

                //if (res.updated == "Success")
                //{

                //}

                //string[] segments = { "Information" };
                //string[] ordrid = { "", "LMK/ORDR/20052015/1534" };
                //string[] TranNo = { "", "1234123455" };
                //string[] CardNumber = { "", "1234123455" };
                //string[] ordrstatus = { "", "Success" };
                //string[] Credit_Debit = { "", "1234123455" };
                //string[] card_type = { "", "1234123455" };

                //uncomment

                string[] segments = encResponse.Split('&');
                string[] ordrid = segments[0].Split('=');
                string[] TranNo = segments[1].Split('=');
                string[] CardNumber = segments[2].Split('=');
                string[] ordrstatus = segments[3].Split('=');

                string[] Credit_Debit = segments[5].Split('=');

                string[] card_type = segments[4].Split('=');
                PrebookingBo PreBo = new PrebookingBo();
                PreBo.Trans_No = TranNo[1];
                PreBo.paid_status = ordrstatus[1];
                PreBo.credit_debit_card = Credit_Debit[1];
                PreBo.card_no = CardNumber[1];
                PreBo.card_type = card_type[1];
                PreBo.Invce_Num = ordrid[1];
                _consumerservices.PreBookingUpdate(PreBo);

                if (ordrstatus[1] == "Success")
                {
                    string SendTo = AuthorityEnum.Admin.ToString()+","+AuthorityEnum.Vendor.ToString()+","+AuthorityEnum.Consumer.ToString();
                    new ConsumerApiController().SendInvoiceEmail(ordrid[1], SendTo);
                    new ConsumerApiController().SendInvoiceSMS(ordrid[1]);

                }



                //ViewData["ordrid"] = ordrid[1];
                //ViewData["TranNo"] = TranNo[1];
                //ViewData["CardNumber"] = CardNumber[1];
                //ViewData["ordrstatus"] = ordrstatus[1];
                //ViewData["card_type"] = card_type[1];
                //ViewData["Credit_Debit"] = Credit_Debit[1];

                return View();
            }
            catch (Exception ex)
            {
                //   Label1.Text = ex.Message.ToString();
                return View();
            }

        }


        public async Task<reservationsresponse> SendJsonToStaah(string encResponse)
        {

            //string[] segments = { "Information" };
            //string[] ordrid = { "", "LMK/ORDR/20052015/1534" };
            //string[] TranNo = { "", "1234123455" };
            //string[] CardNumber = { "", "1234123455" };
            //string[] ordrstatus = { "", "1234123455" };
            //string[] Credit_Debit = { "", "1234123455" };
            //string[] card_type = { "", "1234123455" };

            string[] segments = encResponse.Split('&');
            string[] ordrid = segments[0].Split('=');
            string[] TranNo = segments[1].Split('=');
            string[] CardNumber = segments[2].Split('=');
            string[] ordrstatus = segments[3].Split('=');
            string[] Credit_Debit = segments[5].Split('=');
            string[] card_type = segments[4].Split('=');
            PrebookingBo PreBo = new PrebookingBo();
            PreBo.Trans_No = TranNo[1];
            PreBo.paid_status = ordrstatus[1];
            PreBo.credit_debit_card = Credit_Debit[1];
            PreBo.card_no = CardNumber[1];
            PreBo.card_type = card_type[1];
            PreBo.Invce_Num = ordrid[1];
            //PreBo.AllInfo = segments.ToString();
            DataSet ds = _consumerservices.PreBookingUpdate(PreBo);//  bhargav sir code will come here
            Customer objCustomer = new Customer();
            objCustomer.address = ds.Tables["Table4"].Rows[0]["Cons_Addr1"].ToString();
            objCustomer.city = ds.Tables["Table4"].Rows[0]["cons_city"].ToString();
            objCustomer.company = "www.lastminutekeys.com";
            objCustomer.countrycode = "INR";
            objCustomer.email = ds.Tables["Table4"].Rows[0]["cons_mailid"].ToString();
            objCustomer.first_name = ds.Tables["Table4"].Rows[0]["cons_first_name"].ToString();
            objCustomer.last_name = ds.Tables["Table4"].Rows[0]["cons_last_name"].ToString();
            objCustomer.remarks = ds.Tables["Table4"].Rows[0]["AllInfo"].ToString();
            objCustomer.telephone = ds.Tables["Table4"].Rows[0]["Mobile"].ToString();
            objCustomer.zip = ds.Tables["Table4"].Rows[0]["cons_pincode"].ToString();


            List<Price> objPriceList = new List<Price>();


            Price objPrice1 = new Price();
            DateTime date = Convert.ToDateTime(ds.Tables["Table4"].Rows[0]["checkin"].ToString());
            objPrice1.date = date.ToString("yyyy-MM-dd");
            objPrice1.rate_id = ds.Tables["Table4"].Rows[0]["Room_id"].ToString();
            objPrice1.amount = ds.Tables["Table4"].Rows[0]["prop_room_rate"].ToString();
            objPriceList.Add(objPrice1);

            int daysCount;
            decimal totalPrice;
            daysCount = Convert.ToInt32(ds.Tables["Table4"].Rows[0]["Days_Count"].ToString());
            totalPrice = Convert.ToDecimal(objPrice1.amount);


            for (int i = 1; i < daysCount; i++)
            {
                date = date.AddDays(1);
                Price objPrice2 = new Price();
                objPrice2.date = date.ToString("yyyy-MM-dd");
                objPrice2.rate_id = ds.Tables["Table4"].Rows[0]["Room_id"].ToString();
                objPrice2.amount = ds.Tables["Table4"].Rows[0]["prop_room_rate"].ToString();
                totalPrice = totalPrice + Convert.ToDecimal(objPrice2.amount);
                objPriceList.Add(objPrice2);

            }



            List<Room> objRoomList = new List<Room>();
            Room objRoom = new Room();

            objRoom.arrival_date = Convert.ToDateTime(ds.Tables["Table4"].Rows[0]["checkin"].ToString()).ToString("yyyy-MM-dd");
            objRoom.currencycode = "INR";
            objRoom.departure_date = Convert.ToDateTime(ds.Tables["Table4"].Rows[0]["checkout"].ToString()).ToString("yyyy-MM-dd");
            objRoom.id = ds.Tables["Table4"].Rows[0]["Room_id"].ToString();
            objRoom.name = ds.Tables["Table4"].Rows[0]["Room_Count"].ToString();
            objRoom.price = objPriceList;
            objRoom.totalprice = totalPrice.ToString();
            objRoom.remarks = ds.Tables["Table4"].Rows[0]["AllInfo"].ToString();
            objRoom.guest_name = ds.Tables["Table4"].Rows[0]["GuestName"].ToString();
            objRoom.numberofguests = ds.Tables["Table4"].Rows[0]["Room_Count"].ToString();
            objRoomList.Add(objRoom);

            Reservation objReservation = new Reservation();
            objReservation.commissionamount = "0.00";
            objReservation.deposit = "0.00";
            objReservation.currencycode = "India";
            objReservation.customer = objCustomer;
            objReservation.date = Convert.ToDateTime(ds.Tables["Table4"].Rows[0]["invce_date"].ToString()).ToString("yyyy-MM-dd"); ;
            objReservation.hotel_id = ds.Tables["Table4"].Rows[0]["prop_id"].ToString();
            objReservation.hotel_name = ds.Tables["Table4"].Rows[0]["prop_name"].ToString();
            objReservation.id = ds.Tables["Table4"].Rows[0]["invce_num"].ToString();
            objReservation.room = objRoomList;
            objReservation.status = "new";
            objReservation.time = DateTime.Now.ToShortTimeString();
            objReservation.totalprice = totalPrice.ToString();


            List<Reservation> objReservatiosList = new List<Reservation>();
            objReservatiosList.Add(objReservation);

            Reservations objReserve = new Reservations();
            objReserve.reservation = objReservatiosList;

            RootObject objRoot = new RootObject();
            objRoot.reservations = objReserve;

            string url = "https://lmk.staah.net/common-cgi/mail/booking_lmk.pl";

            var jsonInput = new JavaScriptSerializer().Serialize(objRoot);

            HttpResponseMessage response = await CallApi(url, "", jsonInput);
            string json = await response.Content.ReadAsStringAsync();

            json = json.Substring(json.IndexOf(':') + 1, json.Length - json.IndexOf(':') - 2);
            reservationsresponse objResponse = new reservationsresponse();


            objResponse = new JavaScriptSerializer().Deserialize<reservationsresponse>(json);
            return objResponse;

        }

        public async Task<HttpResponseMessage> CallApi(string serviceMethod, string method, string jsonInput = "")
        {
            var url = new Uri(serviceMethod);
            HttpResponseMessage response = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Host = url.Host;

                using (var stream = GenerateStreamFromString(jsonInput))
                {
                    var sr = new StreamReader(stream);
                    var input = new StringContent(sr.ReadToEnd(), Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(url, input);
                }
            }
            return response;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}