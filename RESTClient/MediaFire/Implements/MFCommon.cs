using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    public enum delta_change_type { file, folder }

    internal class BaseResponse : IResponse
    {
        public string result
        {
            get;
            set;
        }

        public string action
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }

        public static string ClearResponse(string response)
        {
            string result = response.Remove(response.Length-1).Replace("{\"response\":", "");
            return result;
        }
    }
}
