using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DevNet.Services
{
    /// <summary>
    /// Contains properties of configuration strings.
    /// </summary>
    /// <remarks>
    /// Use this class, to provide a configuration on run-time, 
    /// to get values across the classes in an easy and right way.
    /// </remarks>
    public sealed class ConfigurationBuilders
    {
        #region Attributes
        private string connString;
        private string smsMessageKey;
        private string botToken;
        private string botPrefix;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        /// <summary>
        /// Gets or sets the SMS message key.
        /// </summary>
        /// <value>
        /// The SMS message key.
        /// </value>
        public string SmsMessageKey
        {
            get { return smsMessageKey; }
            set { smsMessageKey = value; }
        }

        /// <summary>
        /// Gets or sets the bot token.
        /// </summary>
        /// <value>
        /// The bot token.
        /// </value>
        public string BotToken
        {
            get { return botToken; }
            set { botToken = value; }
        }

        /// <summary>
        /// Gets or sets the bot prefix.
        /// </summary>
        /// <value>
        /// The bot prefix.
        /// </value>
        public string BotPrefix
        {
            get { return botPrefix; }
            set { botPrefix = value; }
        } 
        #endregion
    }
}
