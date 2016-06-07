using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using Validator;

namespace mrzweb.Controllers
{
    public class HomeController : Controller
    {
        private const bool DEBUG = false;

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to validate MRZ data, which makes request to the web api to perform validation
        /// </summary>
        /// <param name="x"></param>
        /// <returns>JSON result in dictionary format, 
        /// eg. if success { "success": 1, "result": MRZ instance }
        /// if error { "error": 1, "message": error message }
        /// </returns>
        [HttpPost]
        public JsonResult Validate(MRZForm x)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();

            try
            {
                string url = DEBUG ? "http://localhost:5000" : "https://mrzapi.apphb.com";
                var client = new RestClient(url);
                var request = new RestRequest("api/mrz/validate", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(x);
                var response = client.Execute<MRZ>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res["result"] = response.Data;
                    res["success"] = 1;
                }
                    
                else
                {
                    res["error"] = 1;
                    res["message"] = response.ErrorMessage;
                }
            }

            catch (Exception e)
            {
                res["error"] = 1;
                res["message"] = e.StackTrace;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}