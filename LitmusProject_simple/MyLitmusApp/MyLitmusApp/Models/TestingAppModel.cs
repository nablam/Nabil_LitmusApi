using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLitmusApp.Models
{
    public class TestingAppModel
    {
        public string _AppName { get; }     //application_long_name
        public string _AppCode { get; }     //application_code
        public string _TestLength { get; }  //average_time_to_process
        public string _Status { get; }      //status
        public string _PlatformName { get; }//platform_name
        public string _ResultType { get; }  //platform_name

        public TestingAppModel(string name, string appCode, string testLength, string status, string platformName, string result)
        {
            _AppName = name;
            _AppCode = appCode;
            _TestLength = testLength;
            _Status = status;
            _PlatformName = platformName;
            _ResultType = result;   //needed in case of spam 
        }

        private string LengthConverter(string s) { return ""; }

    }
}