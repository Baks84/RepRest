using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Extensions;

namespace RESTClient.Dropbox
{
    public class DropBoxApiHelper
    {
        static public RestSharp.RestRequest GenerateTokenAuthorisationRequest(string AccessToken)
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.OAuth2Token, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("code", AccessToken);
            rr.AddParameter("grant_type", DropBoxConfiguration.OAuth2GrantType);
            rr.AddParameter("client_id", DropBoxConfiguration.ApplicationCode);
            rr.AddParameter("client_secret", DropBoxConfiguration.ApplicationSecret);

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetUserAccount_Request(string locale)
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.AccountInfo, Method.GET);
            //rr.RequestFormat = DataFormat.Json;
            //rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_DownloadFile_Request(string FilePath, int rev = 0)
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.DownloadFilePath, Method.GET);
            rr.AddUrlSegment("path", FilePath);
            if (rev != 0)
            {
                rr.AddParameter("rev", rev);
            }

            return rr;
        }

        static public RestSharp.RestRequest Generate_UploadFile_Request(string srcFileName, string srcFilePath,
            long srcFileLength, string FilePath, string locale = "en-EN",
            bool overwrite = true, string rev = "")
        {
            StringBuilder toEncode = new StringBuilder(DropBoxConfiguration.UploadFilePath).Append("?locale=\"").Append(locale).Append("\"&overwrite=")
                .Append(overwrite.ToString().ToLower()).Append("&file=\"").Append(UpperCaseUrlEncode( srcFileName)).Append("\"");
            if (rev != String.Empty)
            {
                toEncode.Append("&parent_rev=\"").Append(rev).Append("\"");
            }
            RestRequest rr = new RestRequest(toEncode.ToString(), Method.PUT);
            rr.AddUrlSegment("path", FilePath);
            rr.AddParameter("file", System.IO.File.ReadAllBytes(srcFilePath), ParameterType.RequestBody);
            rr.AddHeader("Content-Type", "text/plain");

            return rr;
        }

        static public RestSharp.RestRequest Generate_ChunkedUpload_Request(byte[] chunkedData, 
            string upload_ID, long offset)
        {
            StringBuilder toEncode = new StringBuilder(DropBoxConfiguration.ChunkedUpload);

            if(upload_ID!= null && upload_ID != String.Empty)
            {
                toEncode.Append("?upload_id=").Append(upload_ID).Append("&offset=")
                .Append(offset.ToString());
            }

            RestRequest rr = new RestRequest(toEncode.ToString(), Method.PUT);
            rr.AddParameter("body", chunkedData, ParameterType.RequestBody);
            rr.AddHeader("Content-Type", "text/plain");

            return rr;
        }

        static public RestSharp.RestRequest Generate_ChunkedUploadCommit_Request(string FilePath, string upload_id,
            string locale = "en-EN", bool overwrite = true, string rev = "", string Root="dropbox")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.ChunkedUploadCommit, Method.POST);
            rr.AddUrlSegment("path", FilePath);
            rr.AddUrlSegment("root", Root);

            rr.AddParameter("locale", locale);
            rr.AddParameter("overwrite", overwrite.ToString().ToLower());
            if (rev != string.Empty) { rr.AddParameter("parent_rev", rev); }
            rr.AddParameter("upload_id", upload_id);

            return rr;
        }

        static public RestSharp.RestRequest Generate_Metadata_Request(string path, int file_limit = 10000, string hash = "",
            bool list = true, bool include_deleted = true, string rev = "", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Metadata, Method.GET);
            if (path == String.Empty) rr = new RestRequest(DropBoxConfiguration.MetadataRoot, Method.GET);
            else rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("file_limit", file_limit);
            if (hash != String.Empty) { rr.AddParameter("hash", hash); }
            rr.AddParameter("list", list.ToString().ToLower());
            rr.AddParameter("include_deleted", include_deleted.ToString().ToLower());
            if (rev != String.Empty) { rr.AddParameter("rev", rev); }
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_Delta_Request(string Cursor="", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Delta, Method.POST);
            rr.AddParameter("cursor", Cursor);
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_Revisions_Request(string path, string root = "dropbox", int limit = 10, string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Revisions, Method.GET);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            //rr.AddParameter("rev_limit", limit);
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_Restore_Request(string path, string rev, string root = "dropbox", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Restore, Method.POST);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("rev", rev);
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_LongpollDelta_Request(string cursor, int timeout = 30)
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.LongpollDelta, Method.GET);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("cursor", cursor);
            rr.AddParameter("timeout", timeout);

            return rr;
        }

        static public RestSharp.RestRequest Generate_ShareLink_Request(string path, string root ="dropbox", bool short_url=true, string locale="en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Shares, Method.POST);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("locale", locale);
            rr.AddParameter("short_url", short_url);

            return rr;
        }

        static public RestSharp.RestRequest Generate_MediaLink_Request(string path, string root = "dropbox", 
            string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Media, Method.POST);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("locale", locale);

            return rr;
        }

        static public RestSharp.RestRequest Generate_Thumbnail_Request(string path, string root = "dropbox",
            string format = "jpeg", string size="s")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Thumbnail, Method.GET);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;
            rr.AddParameter("format", format);
            rr.AddParameter("size", size);

            return rr;
        }

        static public RestSharp.RestRequest Generate_CopyRef_Request(string path, string root = "dropbox")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.CopyRef, Method.GET);
            rr.AddUrlSegment("root", root);
            rr.AddUrlSegment("path", path);

            rr.RequestFormat = DataFormat.Json;

            return rr;
        }

        static public RestSharp.RestRequest Generate_FileCopy_Request(string from_path, string to_path, 
            string root = "dropbox", string locale="en-EN", bool copyByRef = false)
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.FileCopy, Method.POST);
            rr.AddParameter("root", root);
            rr.AddParameter("to_path", to_path);
            if (copyByRef)
            {
                rr.AddParameter("from_copy_ref", from_path);
            }
            else
            {
                rr.AddParameter("from_path", from_path);
            }

            rr.AddParameter("locale", locale);

            rr.RequestFormat = DataFormat.Json;

            return rr;
        }

        static public RestSharp.RestRequest Generate_FileMove_Request(string from_path, string to_path,
            string root = "dropbox", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Move, Method.POST);
            rr.AddParameter("root", root);
            rr.AddParameter("to_path", to_path);
            rr.AddParameter("from_path", from_path);
            rr.AddParameter("locale", locale);

            rr.RequestFormat = DataFormat.Json;

            return rr;
        }

        static public RestSharp.RestRequest Generate_CreateFolder_Request(string path,
            string root = "dropbox", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.CreateFolder, Method.POST);
            rr.AddParameter("root", root);
            rr.AddParameter("path", path);
            rr.AddParameter("locale", locale);

            rr.RequestFormat = DataFormat.Json;

            return rr;
        }

        static public RestSharp.RestRequest Generate_Delete_Request(string path,
            string root = "dropbox", string locale = "en-EN")
        {
            RestRequest rr = new RestRequest(DropBoxConfiguration.Delete, Method.POST);
            rr.AddParameter("root", root);
            rr.AddParameter("path", path);
            rr.AddParameter("locale", locale);

            rr.RequestFormat = DataFormat.Json;

            return rr;
        }

        private static string UpperCaseUrlEncode(string s)
        {

            char[] temp = RestSharp.Contrib.HttpUtility.UrlEncode(s).ToCharArray();

            for (int i = 0; i < temp.Length - 2; i++)
            {

                if (temp[i] == '%')
                {

                    temp[i + 1] = char.ToUpper(temp[i + 1]);

                    temp[i + 2] = char.ToUpper(temp[i + 2]);

                }

            }
            var values = new Dictionary<string, string>()

            {

                { "+", "%20" },

                { "(", "%28" },

                { ")", "%29" }

            };

            var data = new StringBuilder(new string(temp));

            foreach (string character in values.Keys)
            {

                data.Replace(character, values[character]);

            }

            return data.ToString();
        }
    }
}
