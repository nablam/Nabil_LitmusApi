using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;

namespace MyLitmusApp.RestSharpHelper
{
    public class RestSharpWrapper
    {

         string _accountSid;
         string _secretKey;
        const string LitmusApi = "https://lamribenn.litmus.com";

        public RestSharpWrapper( )
        {
            _accountSid = "lamribenn";
            _secretKey = "!TempPass8468";
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(LitmusApi);
            
            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }
    }
}