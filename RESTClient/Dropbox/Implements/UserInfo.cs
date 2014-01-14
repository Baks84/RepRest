using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
{
    /// <summary>
    /// Represents implementation of the IUserInfo
    /// </summary>
    internal class UserInfo : IUserInfo
    {

        #region Public Properties
        /// <summary>
        /// The user's referral link.
        /// Used to invite new people to Dropbox
        /// </summary>
        public string Referral_link
        {
            get;
            set;
        }

        /// <summary>
        /// The user's display name.
        /// </summary>
        public string Display_Name
        {
            get;
            set;
        }

        /// <summary>
        /// The user's two-letter country code, if available.
        /// </summary>
        public string Country
        {
            get;
            set;
        }

        /// <summary>
        /// The user's unique Dropbox ID.
        /// </summary>
        public int UID
        {
            get;
            set;
        }

        /// <summary>
        /// Quota information
        /// </summary>
        public IQuotaInfo Quota
        {
            get { return Quota_Info; }
        }

        #region For deserialisation purpose
        public QuotaInfo Quota_Info
        {
            get;
            set;
        } 
        #endregion 
        #endregion
    }

    /// <summary>
    /// Represents implementaion of the IQuotaInfo
    /// </summary>
    internal class QuotaInfo : IQuotaInfo
    {

        #region Public Properties
        /// <summary>
        /// The user's used quota in shared folders (bytes).
        /// </summary>
        public decimal shared
        {
            get;
            set;
        }

        /// <summary>
        /// The user's total quota allocation (bytes).
        /// </summary>
        public decimal quota
        {
            get;
            set;
        }

        /// <summary>
        /// The user's used quota outside of shared folders (bytes).
        /// </summary>
        public decimal normal
        {
            get;
            set;
        } 
        #endregion
    }
}
