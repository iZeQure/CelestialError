using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace DevNet.Services
{
    /// <summary>
    /// Json Object Handler.
    /// </summary>
    public class JsonHandler
    {
        #region Attributes        
        /// <summary>
        /// The token name
        /// </summary>
        private string tokenName;

        /// <summary>
        /// The file path
        /// </summary>
        private string filePath;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the name of the token.
        /// </summary>
        /// <value>
        /// The name of the token.
        /// </value>
        public string TokenName
        {
            get { return tokenName; }
            set { tokenName = value; }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set { filePath = value; }
        } 
        #endregion

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
