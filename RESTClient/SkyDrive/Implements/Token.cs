using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.SkyDrive.Implements
{
    public class Token
    {
        public string Access_Token
        {
            get;

            set;
        }
        public long Expires_In
        {
            get;

            set;
        }
        public string Refresh_Token
        {
            get;

            set;
        }
        public string Scope
        {
            get;

            set;
        }

        public string Token_Type
        {
            get;

            set;
        }
    }
}
