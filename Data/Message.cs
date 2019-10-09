using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DevNet.Data
{
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
