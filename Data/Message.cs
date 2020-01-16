using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DevNet.Data
{
    /// <summary>
    /// Creates a message, with a given request type.
    /// </summary>
    /// <see cref="RequestType"/>
    public class Message
    {
        #region Attributes
        private string requestType;
        #endregion

        #region Properties
        public string RequestType
        {
            get { return requestType; }
            private set { requestType = value; }
        }
        #endregion

        #region Constructors
        public Message(string requestType)
        {
            RequestType = requestType;
        }
        #endregion
    }
}
