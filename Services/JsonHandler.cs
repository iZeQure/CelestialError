using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DevNet.Services
{
    /// <summary>
    /// Json Object Handler.
    /// </summary>
    public class JsonHandler
    {
        // Attributes
        private string tokenName;
        private string filePath;

        // Properties
        public string TokenName
        {
            get { return tokenName; }
            set { tokenName = value; }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set { filePath = value; }
        }

        /// <summary>
        /// Get Json Token
        /// </summary>
        /// <returns></returns>
        public string GetJsonToken()
        {
            // Check on fields if they are null.
            if (TokenName != null && FilePath != null)
            {
                // Create json object with fields.
                JObject jsonObject = JObject.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath)));
                // Get json token from object.
                string jsonToken = (string)jsonObject.SelectToken(TokenName);

                // return token.
                return jsonToken;
            }
            // return null on error.
            return null;
        }
    }
}
