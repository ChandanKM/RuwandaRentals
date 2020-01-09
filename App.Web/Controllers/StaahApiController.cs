using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Domain;
using App.UIServices;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace App.Web.Controllers
{
    [RoutePrefix("api/STAAH")]
    public class StaahApiController : ApiController
    {
        readonly IRoomTypeServices _roomMapping;

        public StaahApiController(IRoomTypeServices roomMapping)
        {
            _roomMapping = roomMapping;
        }

        [HttpPost("RoomMapping")]
        public HttpResponseMessage RoomMapping(Roomrequest roomrequest)
        {
            try
            {
                if (roomrequest.username == "STAAH_LMK" && roomrequest.password == "e467cb5f16efeb277730a9e359ff1d2e")
                {
                    if (roomrequest != null)
                    {
                        var xmlResult = _roomMapping.GetRoomMap(roomrequest);
                        var jsonResult = JsonConvert.SerializeObject(xmlResult);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");

                        //var xEle = ToXML(xmlResult);

                        //var xml = new XElement("Response", xEle).ToString();

                        //var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        //response.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
                        SetDataToEMCLog(Request.RequestUri.OriginalString, "RoomMapping", JsonConvert.SerializeObject(roomrequest), "Success");
                        return response;
                    }
                }
                else
                {
                    SetDataToEMCLog(Request.RequestUri.OriginalString, "RoomMapping", JsonConvert.SerializeObject(roomrequest), "Failed");
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;

            }
        }

        [HttpPost("RoomInventory")]
        public HttpResponseMessage RoomInventry(request request)
        {

            if (request.username == "STAAH_LMK" && request.password == "e467cb5f16efeb277730a9e359ff1d2e")
            {
                var xmlResult = _roomMapping.RoomInventry(request);
                var jsonResult = JsonConvert.SerializeObject(xmlResult);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");

                SetDataToEMCLog(Request.RequestUri.OriginalString, "RoomInventory", JsonConvert.SerializeObject(request), "Success");
                return response;
            }
            else
            {
                SetDataToEMCLog(Request.RequestUri.OriginalString, "RoomInventory", JsonConvert.SerializeObject(request), "Failed");
                return null;
            }

        }

        private string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value
                ns.Add("", "");
                xmlSerializer.Serialize(stringWriter, obj, ns);
                return stringWriter.ToString();
            }
        }

        private bool SetDataToEMCLog(string requestFrom, string requestTo, string requestBody, string status)
        {
            return _roomMapping.SetDataToEMCLog(requestFrom, requestTo, requestBody, status);


        }

    }
}
