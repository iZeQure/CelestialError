using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DevNet.Services
{
    public sealed class ConfigurationBuilders
    {
        private string connString;
        private string smsMessageKey;
        private string botToken;
        private string botPrefix;

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        public string SmsMessageKey
        {
            get { return smsMessageKey; }
            set { smsMessageKey = value; }
        }

        public string BotToken
        {
            get { return botToken; }
            set { botToken = value; }
        }

        public string BotPrefix
        {
            get { return botPrefix; }
            set { botPrefix = value; }
        }
    }
}
