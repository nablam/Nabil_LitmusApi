using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using MyLitmusApp.Models;
using System.Xml;
using System.Net;
namespace MyLitmusApp.Controllers
{
    public class LitmusMainCTRLController : Controller
    {
       private readonly RestClient restClient = new RestClient("https://lamribenn.litmus.com");
        private List<TestingAppModel> CompleteTetingAppsList;

        [HttpGet]
        public ActionResult Index()
        {
            restClient.Authenticator = new HttpBasicAuthenticator("lamribenn", "!TempPass8468");
            RestRequest request_email = new RestRequest("/emails/clients.xml", Method.GET);
            IRestResponse responseRaw = restClient.Execute(request_email);
            HttpStatusCode code = responseRaw.StatusCode;
            string codeStr = code.ToString();
            if (code.ToString() != "OK")
            {
                string ErrorMessageTOPass = "";
                switch (codeStr)
                {
                    case "NotFound":
                        ErrorMessageTOPass=" Could not reach the API, please check the correct API address";
                        break;
                    case "Unauthorized":
                        ErrorMessageTOPass = "You do not have the proper credential.Please check your username and password";
                        break;
                    case "0":
                        ErrorMessageTOPass = "YOU HAVE NO INTERWEBZ";
                        break;
                }

                TempData["message"] = ErrorMessageTOPass;
                return RedirectToAction("ErrorPage");           
            }
            else
            {
                string XML_Response = responseRaw.Content;
               // ViewBag.Mes = code.ToString();



                return View();
            }


        }




        public ActionResult ErrorPage() {
            ViewBag.passedError = TempData["message"];
            return View();
        }

        [HttpGet]
        public ActionResult EmailClients() {
            restClient.Authenticator = new HttpBasicAuthenticator("lamribenn", "!TempPass8468");
            RestRequest request_email = new RestRequest("/emails/clients.xml", Method.GET);
            IRestResponse responseRaw = restClient.Execute(request_email);
            HttpStatusCode code = responseRaw.StatusCode;
            string codeStr = code.ToString();
            if (code.ToString() != "OK")
            {
                string ErrorMessageTOPass = "";
                switch (codeStr)
                {
                    case "NotFound":
                        ErrorMessageTOPass = " Could not reach the API, please check the correct API address";
                        break;
                    case "Unauthorized":
                        ErrorMessageTOPass = "You do not have the proper credential.Please check your username and password";
                        break;
                    case "0":
                        ErrorMessageTOPass = "YOU HAVE NO INTERWEBZ";
                        break;
                }

                TempData["message"] = ErrorMessageTOPass;
                return RedirectToAction("ErrorPage");
            }
            else
            {
                string XML_Response = responseRaw.Content;
                // ViewBag.Mes = code.ToString();

                List<TestingAppModel> listFromXml = ReadXmlBuildDataList(XML_Response);
                CompleteTetingAppsList = MakesortedEmailList(listFromXml);

                return View(CompleteTetingAppsList);
            }
        }


        List<TestingAppModel> ReadXmlBuildDataList(string xml_str) {
            List<TestingAppModel> UnsortedEmailList = new List<TestingAppModel>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml_str);      
            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("testing_application");
            foreach (XmlNode childrenNode in parentNode)
            {
                
               // if (String.Compare(isitspam,"spam")==0) {  continue; }

                string name = (childrenNode.SelectSingleNode("application_long_name").InnerText);
                string code = (childrenNode.SelectSingleNode("application_code").InnerText);
                string time = (childrenNode.SelectSingleNode("average_time_to_process").InnerText);
                string status = (childrenNode.SelectSingleNode("status").InnerText);
                string platform = (childrenNode.SelectSingleNode("platform_name").InnerText);
                string result = (childrenNode.SelectSingleNode("result_type").InnerText);


                UnsortedEmailList.Add(new TestingAppModel(name, code, time,status, platform,result));
            }

            return UnsortedEmailList;
        }

        List<TestingAppModel> MakesortedEmailList(List<TestingAppModel> unsortedlist ) {
            List<TestingAppModel> SortedList = new List<TestingAppModel>();
            IEnumerable<IGrouping<string, TestingAppModel>> IgroupList = unsortedlist.GroupBy(b => b._PlatformName);
            foreach (IGrouping<string, TestingAppModel> subList in IgroupList)
            {
                foreach (TestingAppModel singleT in subList)
                {
                    SortedList.Add(singleT);
                }
            }
            return SortedList;
        }

        [HttpGet]
        public ActionResult BrowserClients()
        {
            restClient.Authenticator = new HttpBasicAuthenticator("lamribenn", "!TempPass8468");
            RestRequest request_email = new RestRequest("/pages/clients.xml", Method.GET);
            IRestResponse responseRaw = restClient.Execute(request_email);
            HttpStatusCode code = responseRaw.StatusCode;
            string codeStr = code.ToString();
            if (code.ToString() != "OK")
            {
                string ErrorMessageTOPass = "";
                switch (codeStr)
                {
                    case "NotFound":
                        ErrorMessageTOPass = " Could not reach the API, please check the correct API address";
                        break;
                    case "Unauthorized":
                        ErrorMessageTOPass = "You do not have the proper credential.Please check your username and password";
                        break;
                    case "0":
                        ErrorMessageTOPass = "YOU HAVE NO INTERWEBZ";
                        break;
                }

                TempData["message"] = ErrorMessageTOPass;
                return RedirectToAction("ErrorPage");
            }
            else
            {
                string XML_Response = responseRaw.Content;
                // ViewBag.Mes = code.ToString();

                List<TestingAppModel> listFromXml = ReadXmlBuildDataList(XML_Response);
                

                return View(listFromXml);
            }
         
        }


        
    }
}