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
using MyLitmusApp.RestSharpHelper;

namespace MyLitmusApp.Controllers
{
    public class LitmusMainCTRLController : Controller
    {
        private readonly RestClient restClient = new RestClient("https://lamribenn.litmus.com");
        private List<TestingAppModel> CompleteTetingAppsList;

        #region delegate
        delegate ActionResult MYDELEGATE(string responceTEXT);
        ActionResult _ACCOUNTFUINC(string XML)
        {
            AccountDataObject AccountOBJ = ReadXmlBuildAccountData(XML);
            return View(AccountOBJ);
        }
        ActionResult _CLIENTFUNC(string XML)
        {
            List<TestingAppModel> listFromXml = ReadXmlBuildDataList(XML);
            CompleteTetingAppsList = MakesortedEmailList(listFromXml);
            return View(CompleteTetingAppsList);
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            return HANDELANYAPI("accounts.xml", _ACCOUNTFUINC);
        }

        [HttpGet]
        public ActionResult EmailClients() {
            return HANDELANYAPI("/emails/clients.xml", _CLIENTFUNC);
        }
    
        [HttpGet]
        public ActionResult BrowserClients()
        {
            return HANDELANYAPI("/pages/clients.xml", _CLIENTFUNC);
        }

        [HttpGet]
        public ActionResult ErrorPage()
        {
            ViewBag.passedError = TempData["message"];
            return View();
        }
        #endregion

        #region private_functions
        ActionResult HANDELANYAPI(string apiUrl, MYDELEGATE functionPointer )
        {

            RestSharpWrapper Rhelper = new RestSharpWrapper(restClient, apiUrl);
            ResResponseObject robj = Rhelper.ActivateConnection();

            if (robj._ResponceMessage != "OK")
            {
                TempData["message"] = MapErrorMessage(robj._ResponceMessage);
                return RedirectToAction("ErrorPage");
            }
            else
            {
              return functionPointer(robj._ResponseRaw.Content);
            }

        }

        AccountDataObject ReadXmlBuildAccountData(string xml_str)
        {
            string fname = ""; string lname = ""; string datecreated = "";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml_str);

            XmlNodeList parentNode2 = xmlDoc.GetElementsByTagName("account");
            foreach (XmlNode childrenNode2 in parentNode2)
            {
                datecreated = (childrenNode2.SelectSingleNode("created_at").InnerText);
            }

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("account_holder");
            foreach (XmlNode childrenNode in parentNode)
            {
                fname = (childrenNode.SelectSingleNode("first_name").InnerText);
                lname = (childrenNode.SelectSingleNode("last_name").InnerText);
            }

            AccountDataObject accountObject = new AccountDataObject(fname, lname, datecreated);

            return accountObject;
        }

        string MapErrorMessage(string errorMessage)
        {
            string ErrorMessageTOPass = "";
            switch (errorMessage)
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
                default:
                    {
                        ErrorMessageTOPass = " unknown error ";
                        break;
                    }
            }
            return ErrorMessageTOPass;
        }

        List<TestingAppModel> ReadXmlBuildDataList(string xml_str) {
            List<TestingAppModel> UnsortedEmailList = new List<TestingAppModel>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml_str);      
            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("testing_application");
            foreach (XmlNode childrenNode in parentNode)
            {             
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
        #endregion
    }
}