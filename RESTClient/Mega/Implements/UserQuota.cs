using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Mega.Implements
{
    class UserQuota : IUserQuota
    {
        public UserQuota() { }
        public UserQuota(MegaApi.DataTypes.UserQuota uq)
        {
            this.Quota = uq.Max_Quota;
            this.Used = uq.Used_Quota;
        }

        public long Used
        {
            get;
            set;
        }

        public long Quota
        {
            get;
            set;
        }
    }
}
