using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi.DataTypes
{
    public class UserQuota
    {
        public long Max_Quota
        { get; set; }

        public long Used_Quota
        { get; set; }
    }
}
