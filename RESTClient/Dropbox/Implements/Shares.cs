using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
{
    /// <summary>
    /// Represents the implementation of the IShareLink
    /// </summary>
    internal class ShareLink : IShareLink
    {
        private string url = String.Empty;

        #region Public Properties
        /// <summary>
        /// A Dropbox link to the given path. The link can be used publicly and directs to a preview page of the file.
        /// </summary>
        public Uri Url
        {
            get { return new Uri(url); }
            protected set { url = value.AbsoluteUri; }
        }

        /// <summary>
        /// For compatibility reasons, it returns the link's expiration date in Dropbox's usual date format. 
        /// All links are currently set to expire far enough in the future so that expiration is effectively not an issue.
        /// </summary>
        public DateTime Expires
        {
            get;
            protected set;
        } 
        #endregion
    }

    /// <summary>
    /// Represents the ICopyRef implementation
    /// </summary>
    internal class CopyRefLink : ICopyRef
    {
        #region Public Properties

        /// <summary>
        /// A copy_ref to the specified file.
        /// </summary>
        public String Copy_ref
        {
            get;
            protected set;
        }

        /// <summary>
        /// For compatibility reasons, it returns the link's expiration date in Dropbox's usual date format. 
        /// All links are currently set to expire far enough in the future so that expiration is effectively not an issue.
        /// </summary>
        public DateTime Expires
        {
            get;
            protected set;
        } 
        #endregion
    }
}
