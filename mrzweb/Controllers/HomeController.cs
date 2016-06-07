using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using Newtonsoft.Json;
using Validator;

namespace mrzweb.Controllers
{
    public class HomeController : Controller
    {
        private const bool DEBUG = true;

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Validate(MRZForm x)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();

            try
            {
                var client = new RestClient("http://localhost:5000");
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