using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLitmusApp.Models
{
    public class ResResponseObject
    {
        public IRestResponse _ResponseRaw { get; }
        public string _ResponceMessage { get; }
        public ResResponseObject(IRestResponse irestresponce, string message) {
            _ResponseRaw = irestresponce;
            _ResponceMessage = message;
        }
    }
}