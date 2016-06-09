using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using MyLitmusApp.Models;
using System.Net;

namespace MyLitmusApp.RestSharpHelper
{
    public class RestSharpWrapper
    {

        RestClient _restClientn;
        string _apiUr;

        public RestSharpWrapper(RestClient rClient, string ApiUrl) {
            _restClientn = rClient;
            _apiUr = ApiUrl;
        }

        public ResResponseObject ActivateConnection( ) {
            _restClientn.Authenticator = new HttpBasicAuthenticator("lamribenn", "!TempPass8468");
            RestRequest Request = new RestRequest(_apiUr, Method.GET);
            IRestResponse responseRaw = _restClientn.Execute(Request);
            HttpStatusCode code = responseRaw.StatusCode;
            string codeStr = code.ToString();
            return new ResResponseObject(responseRaw, codeStr);
        }
    }
}