using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    internal class UserInfo : IUserInfo
    {

        public string Email
        {
            get;
            set;
        }

        public string First_Name
        {
            get;
            set;
        }

        public string Last_Name
        {
            get;
            set;
        }

        public string Display_Name
        {
            get;
            set;
        }

        public string Gender
        {
            get;
            set;
        }

        public DateTime? Birth_Date
        {
            get;
            set;
        }

        public string Premium
        {
            get;
            set;
        }

        public long? Bandwidth
        {
            get;
            set;
        }

        public DateTime? Created
        {
            get;
            set;
        }

        public string Validated
        {
            get;
            set;
        }

        public long? Max_upload_size
        {
            get;
            set;
        }

        public long? Max_instant_upload_size
        {
            get;
            set;
        }

        public string Tos_accpeted
        {
            get;
            set;
        }

        public long? Used_storage_size
        {
            get;
            set;
        }

        public long? Base_storage
        {
            get;
            set;
        }

        public long? Bonus_storage
        {
            get;
            set;
        }

        public long? Storage_limit
        {
            get;
            set;
        }

        public string Storage_limit_exceeded
        {
            get;
            set;
        }
    }

    internal class GetUserInfoResponse : BaseResponse
    {
        public UserInfo User_info { get; set; }
    }
}
