using App.BusinessObject;
using App.UIServices;
using CCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class CancelController : Controller
    {
          readonly IConsumerService _consumerservices;

          public CancelController(IConsumerService consumerservices)
        {
            _consumerservices = consumerservices;
        }
        // GET: Cancel
        public ActionResult Index()
        {
            try
            {
                string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here
                CCACrypto ccaCrypto = new CCACrypto();
                string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
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
             //   PreBo.AllInfo = segments.ToString();
                _consumerservices.PreBookingUpdate(PreBo);
                return View();


            }
            catch (Exception ex)
            {
                //   Label1.Text = ex.Message.ToString();
                return View();
            }
        }
    }
}