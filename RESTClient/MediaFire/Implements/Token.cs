using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    /// <summary>
    /// Token used for authorisation with Dropbox using OAuth 2
    /// </summary>
    public class Token : IToken
    {
        #region Public Properties
        /// <summary>
        /// Access token. Actual value of the Token
        /// </summary>
        public string Access_Token
        {
            get;
            set;
        }

        /// <summary>
        /// The token type will always be "bearer".
        /// </summary>
        public string Token_Type
        {
            get;
            set;
        }

        /// <summary>
        /// Dropbox user ID (uid). 
        /// </summary>
        public string PKEY
        {
            get;
            set;
        }

        /// <summary>
        /// Tells when the token is expired. If time left is less than 5 minuts Token can be refreshed
        /// </summary>
        public DateTime Expires
        {
            get;
            set;
        } 
        #endregion
    }

    internal class GetSessionTokenResponse : BaseResponse
    {
        public string session_token { get; set; }
        public string pkey { get; set; }
    }
}
