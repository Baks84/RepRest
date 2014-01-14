using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
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
        public int UID
        {
            get;
            set;
        } 
        #endregion
    }
}
