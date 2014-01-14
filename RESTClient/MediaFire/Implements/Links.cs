using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    internal class Links : IFileLinks
    {
        public string quickkey { get; set; }
        public string FileID
        {
            get
            {
                return quickkey;
            }
            set
            {
                quickkey = value;
            }
        }

        public string View
        {
            get;
            set;
        }

        public string Normal_Download
        {
            get;
            set;
        }

        public string Direct_Download
        {
            get;
            set;
        }

        public string Edit
        {
            get;
            set;
        }

        public string One_time_download
        {
            get;
            set;
        }

        public int One_time_download_request_count
        {
            get;
            set;
        }

        public int Direct_download_free_bandwidth
        {
            get;
            set;
        }
    }

    internal class LinksResponse : BaseResponse
    {
        public List<Links> links { get; set; }
        public int One_time_download_request_count
        {
            get;
            set;
        }

        public int Direct_download_free_bandwidth
        {
            get;
            set;
        }
    }

    internal class OneTimeDownload : BaseResponse, IOneTimeDownload
    {
        public int one_time_download_request_count { get; set; }

        public int RequestCount
        {
            get
            {
                return one_time_download_request_count;
            }
        }

        public int one_time_download_request_max_count { get; set; }
        public int RequestCountLimit
        {
            get
            {
                return one_time_download_request_max_count;
            }
        }

        public string one_time_download {get;set;}
        public string Link
        {
            get
            {
                return one_time_download;
            }
        }

        public string token {get;set;}
        public string Token
        {
            get { return token; }
        }
    }
}
