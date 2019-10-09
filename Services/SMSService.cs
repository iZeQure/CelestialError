using DevNet.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;

namespace DevNet.Services
{
    public class SMSService : Message
    {
        private WebClient webClient = new WebClient();

        #region Attributes
        private readonly string smsKey = ConfigurationManager.AppSettings["SmsMessageKey"];
        #endregion

        #region Properties
        public string SmsKey
        {
            get { return smsKey; }
        }
        #endregion

        public SMSService(string requestType) : base(requestType) { }

        public void SendSms(string receiver, string message)
        {
            string sendJsonRequest = webClient.DownloadString($"{RequestType}data.efif.dk/JSON/SMS.ashx?key={SmsKey}&receivers={receiver}&message={message}");
        }
    }
}
