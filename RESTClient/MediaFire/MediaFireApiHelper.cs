using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Extensions;
using System.Security.Cryptography;

namespace RESTClient.MediaFire
{
    public class MediaFireApiHelper
    {
        static public RestSharp.RestRequest GenerateGetSessionTokenRequest(string UserEMail, string UserPassword)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetSessionToken, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("email", UserEMail);
            rr.AddParameter("password", UserPassword);
            rr.AddParameter("response_format", "json");
            rr.AddParameter("application_id", MediaFireConfiguration.ApplicationCode);
            rr.AddParameter("signature", Utils.SHA1Helper.SHA1HashStringForUTF8String(
                new StringBuilder(UserEMail).Append(UserPassword).Append(MediaFireConfiguration.ApplicationCode).
                Append(MediaFireConfiguration.ApplicationSecret).ToString()));

            return rr;
        }

        static public RestSharp.RestRequest GenerateRenewSessionTokenRequest(string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.RenewSessionToken, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetUserInfo_Request(string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetUserInfo, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        #region Folder
        static public RestSharp.RestRequest Generate_GetRoot_Request(string SessionToken, bool folders = true)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetRoot, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");
            if (!folders)
            {
                rr.AddParameter("content_type", "files");
            }

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetFolderContent_Request(string FolderKey, string SessionToken, bool folders = true)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFolderContent, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FolderKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");
            if (!folders)
            {
                rr.AddParameter("content_type", "files");
            }

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetFolderInfo_Request(string FolderKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFolderInfo, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FolderKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetFolderRevision_Request(string FolderKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFolderRevision, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FolderKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("return_changes", "yes");
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_UpdateFolder_Request(string FolderKey, string SessionToken, string foldername = "", string description = "",
            string tags = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.UpdateFolder, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FolderKey);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(foldername))
                rr.AddParameter("foldername", foldername);
            if (!string.IsNullOrEmpty(description))
                rr.AddParameter("description", description);
            if (!string.IsNullOrEmpty(tags))
                rr.AddParameter("tags", tags);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_CopyFolder_Request(string SrcFolderKey, string SessionToken, string DstFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.CopyFolder, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key_src", SrcFolderKey);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(DstFolderKey))
                rr.AddParameter("folder_key_dst", DstFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_MoveFolder_Request(string SrcFolderKey, string SessionToken, string DstFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.MoveFolder, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key_src", SrcFolderKey);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(DstFolderKey))
                rr.AddParameter("folder_key_dst", DstFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_CreateFolder_Request(string FolderName, string SessionToken, string ParentFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.CreateFolder, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("foldername", FolderName);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(ParentFolderKey))
                rr.AddParameter("parent_key", ParentFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_DeleteFolder_Request(string FolderKey, string SessionToken, bool purge = false)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.DeleteFolder, Method.POST);
            if (purge)
                rr = new RestRequest(MediaFireConfiguration.PurgeFolder, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FolderKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        } 
        #endregion
        #region File

        static public RestSharp.RestRequest Generate_GetFileInfo_Request(string FileKey)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFileInfo, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", FileKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_CopyFile_Request(string SrcFileKey, string SessionToken, string DstFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.CopyFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", SrcFileKey);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(DstFolderKey))
                rr.AddParameter("folder_key", DstFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_CreateFile_Request(string FileName, string SessionToken, string DstFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.CreateFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("filename", FileName);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(DstFolderKey))
                rr.AddParameter("parent_key", DstFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_CreateFileSnapshot_Request(string FileKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.CreateFileSnapshot, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", FileKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_DeleteFile_Request(string FileKey, string SessionToken, bool purge = false)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.DeleteFile, Method.POST);
            if (purge)
                rr = new RestRequest(MediaFireConfiguration.PurgeFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("folder_key", FileKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetFileLinks_Request(string FileKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFileLinks, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", FileKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetFileVersions_Request(string FileKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFileVersions, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", FileKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_MoveFile_Request(string SrcFileKey, string SessionToken, string DstFolderKey = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.MoveFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", SrcFileKey);
            rr.AddParameter("session_token", SessionToken);
            if (!string.IsNullOrEmpty(DstFolderKey))
                rr.AddParameter("folder_key", DstFolderKey);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetDailyOneTimeDownloadLimit_Request(string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.OneTimeDownload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetDailyOneTimeDownloadLink_Request(string SessionToken, string FileID, int Duration = 0, 
            bool EmailNotification = false, Uri SuccessCallback = null, Uri ErrorCallback = null,
            string BindIP = "", bool BurnAfterUse = true)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.OneTimeDownload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("quick_key", FileID);
            if(Duration > 0)
                rr.AddParameter("duration", Duration);

            if(EmailNotification)
                rr.AddParameter("email_notification", "yes");

            if(SuccessCallback != null)
                rr.AddParameter("success_callback_url", SuccessCallback.AbsoluteUri);

            if(ErrorCallback != null)
                rr.AddParameter("error_callback_url", ErrorCallback.AbsoluteUri);

            if(!string.IsNullOrEmpty(BindIP))
                rr.AddParameter("bind_ip", BindIP);

            if(!BurnAfterUse)
                rr.AddParameter("burn_after_use", "no");
            else
                rr.AddParameter("burn_after_use", "yes");

            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetRecentlyModifiedFileList_Request(string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.LastModifiedFiles, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetRestoreFile_Request(string SessionToken, string FileID, int Revision)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.RestoreFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("quick_key", FileID);
            rr.AddParameter("revision", Revision);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetUpdateFile_Request(string SessionToken, string FileID, string FileName="",
            string Description="", string tags="", bool Public = true)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.UpdateFile, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("quick_key", FileID);
            
            if(!string.IsNullOrEmpty(FileName))
                rr.AddParameter("filename", FileName);

            if(!string.IsNullOrEmpty(Description))
                rr.AddParameter("description", Description);

            if (!string.IsNullOrEmpty(tags))
                rr.AddParameter("tags", tags);

            if (Public)
                rr.AddParameter("privacy", "public");
            else
                rr.AddParameter("privacy", "private");

            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetZip_Request(string SessionToken, List<string> IDs)
        {
            string keys = "";

            foreach (var item in IDs)
            {
                keys += item;
                if (item != IDs.Last())
                {
                    keys += ",";
                }
            }

            RestRequest rr = new RestRequest(MediaFireConfiguration.GetZip, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("keys", keys);
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetDirectDownloadLink_Request(string FileKey, string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetFileLinks, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("quick_key", FileKey);
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("link_type", "direct_download");
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_DownloadFile_Request(string URL)
        {
            RestRequest rr = new RestRequest(URL, Method.POST);
            //rr.RequestFormat = DataFormat.Json;
            //rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //rr.AddParameter("quick_key", FileKey);
            //rr.AddParameter("session_token", SessionToken);
            //rr.AddParameter("link_type", "direct_download");
            //rr.AddParameter("response_format", "json");

            return rr;
        }
        #endregion
        #region Upload

        static public RestSharp.RestRequest Generate_AddWebUpload_Request(string SessionToken, Uri URL, string Filename, string DstFolderID="")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.WebUpload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("filename", Filename);
            rr.AddParameter("url", URL.AbsoluteUri);
            if (!string.IsNullOrEmpty(DstFolderID))
            {
                rr.AddParameter("folder_key", DstFolderID);
            }
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_GetWebUploads_Request(string SessionToken, bool ReturnAll = false)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.GetWebUpload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            if (ReturnAll)
            {
                rr.AddParameter("all_web_uploads", "yes");
            }
            else
            {
                rr.AddParameter("all_web_uploads", "no");
            }
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public RestSharp.RestRequest Generate_PreUpload_Request(string SessionToken, string FileName, bool MakeHash = false, string Upl_FolderID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, bool Resumable = false, string Path = "")
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.PreUpload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);

            if (!System.IO.File.Exists(FileName))
            {
                throw new System.IO.FileNotFoundException("The file " + FileName + " was not found");
            }

            rr.AddParameter("filename", new System.IO.FileInfo(FileName).Name);

            if (MakeHash)
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
                {
                    fs.Position = 0;

                    SHA256 mySHA = SHA256Managed.Create();
                    byte[] hashValue = mySHA.ComputeHash(fs);
                    rr.AddParameter("hash", BitConverter.ToString(hashValue).Replace("-", ""));
                }
                rr.AddParameter("size", new System.IO.FileInfo(FileName).Length);
            }

            if (!string.IsNullOrEmpty(Upl_FolderID))
            {
                rr.AddParameter("upload_folder_key", Upl_FolderID);
            }

            if (!string.IsNullOrEmpty(Path))
            {
                rr.AddParameter("path", Path);
            }

            switch (ActionOnDuplicate)
            {
                case UploadDuplicateActions.Keep:
                    rr.AddParameter("action_on_duplicate", "keep");
                    break;
                case UploadDuplicateActions.Skip:
                    rr.AddParameter("action_on_duplicate", "skip");
                    break;
                case UploadDuplicateActions.Replace:
                    rr.AddParameter("action_on_duplicate", "replace");
                    break;
            }

            if (Resumable)
            {
                if (!MakeHash) throw new ArgumentNullException("If you want to create resumable pre upload hash must be calculated");
                rr.AddParameter("resumable", "yes");
            }
            else
            {
                rr.AddParameter("resumable", "no");
            }

            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public System.Net.HttpWebRequest Generate_Upload_Request(string SessionToken, string BaseUrl, string FileName, string Upl_FolderID = "", string FileID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, string Path = "", string Previous_Hash = "")
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
            //Get API base URL
            
            if (BaseUrl.Last() != '/') BaseUrl += "/";
            StringBuilder uploadAddress = new StringBuilder(BaseUrl).Append(MediaFireConfiguration.Upload).Append("&session_token=").Append(SessionToken);
            if (!string.IsNullOrEmpty(Upl_FolderID))
            {
                uploadAddress.Append("&upload_key=").Append(Upl_FolderID);
            }
            else
            {
                uploadAddress.Append("&upload_key=").Append("myfiles");
            }
            uploadAddress.Append("&filenum=0&uploader=0&response_format=json");

            if (!string.IsNullOrEmpty(FileID))
            {
                uploadAddress.Append("&quick_key=").Append(FileID);
            }

            switch (ActionOnDuplicate)
            {
                case UploadDuplicateActions.Keep:
                    uploadAddress.Append("&action_on_duplicate=").Append("keep");
                    break;
                case UploadDuplicateActions.Skip:
                    uploadAddress.Append("&action_on_duplicate=").Append("skip");
                    break;
                case UploadDuplicateActions.Replace:
                    uploadAddress.Append("&action_on_duplicate=").Append("replace");
                    break;
            }

            if (!string.IsNullOrEmpty(Path))
            {
                uploadAddress.Append("&path=").Append(Path);
            }

            if (!string.IsNullOrEmpty(Previous_Hash))
            {
                uploadAddress.Append("&previous_hash=").Append(Previous_Hash);
            }

            var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uploadAddress.ToString());
            req.Accept = "*/*";
            req.ContentType = "application/octet-stream";
            req.Method = "POST";
            req.SendChunked = false;
            req.Referer = "https://www.mediafire.com/apps/myfiles/?shared=0&multikeys=0";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.72 Safari/537.36";
            req.Headers.Add("Origin", "https://www.mediafire.com");
            req.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8,de;q=0.6,pl;q=0.4");
            req.Headers.Add("X-Filename", fileInfo.Name);
            req.Headers.Add("X-Filesize", fileInfo.Length.ToString());
            req.ContentLength = fileInfo.Length;
            req.Headers.Add("X-Filetype", "text/plain");

            using (System.IO.FileStream fs = fileInfo.OpenRead())
            {
                fs.Position = 0;

                System.Security.Cryptography.SHA256 mySHA = System.Security.Cryptography.SHA256Managed.Create();
                byte[] hashValue = mySHA.ComputeHash(fs);
                req.Headers.Add("X-Filehash", BitConverter.ToString(hashValue).Replace("-", ""));
            }

            using (var rs = req.GetRequestStream())
            {
                rs.Write(System.IO.File.ReadAllBytes(FileName), 0, System.IO.File.ReadAllBytes(FileName).Length);
            }

            return req;
        }

        static public System.Net.HttpWebRequest Generate_PartUpload_Request(string SessionToken, string BaseUrl, string FileName, byte[] buffer, int Unit_ID, string Upl_FolderID = "")
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
            //Get API base URL

            if (BaseUrl.Last() != '/') BaseUrl += "/";
            StringBuilder uploadAddress = new StringBuilder(BaseUrl).Append(MediaFireConfiguration.Upload).Append("&session_token=").Append(SessionToken);
            if (!string.IsNullOrEmpty(Upl_FolderID))
            {
                uploadAddress.Append("&upload_key=").Append(Upl_FolderID);
            }
            else
            {
                uploadAddress.Append("&upload_key=").Append("myfiles");
            }
            uploadAddress.Append("&filenum=0&uploader=0&response_format=json");

            var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uploadAddress.ToString());

            req.Accept = "*/*";
            req.ContentType = "application/octet-stream";
            req.Method = "POST";
            req.SendChunked = false;
            req.Referer = "https://www.mediafire.com/apps/myfiles/?shared=0&multikeys=0";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.72 Safari/537.36";
            req.Headers.Add("Origin", "https://www.mediafire.com");
            req.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8,de;q=0.6,pl;q=0.4");
            req.Headers.Add("X-Filename", fileInfo.Name);
            req.Headers.Add("X-Filesize", fileInfo.Length.ToString());
            req.ContentLength = buffer.Length;
            req.Headers.Add("X-Filetype", "text/plain");

            using (System.IO.FileStream fs = fileInfo.OpenRead())
            {
                fs.Position = 0;

                System.Security.Cryptography.SHA256 mySHA = System.Security.Cryptography.SHA256Managed.Create();
                byte[] hashValue = mySHA.ComputeHash(fs);
                req.Headers.Add("X-Filehash", BitConverter.ToString(hashValue).Replace("-", ""));

                req.Headers.Add("X-unit-id", Unit_ID.ToString());
                req.Headers.Add("X-unit-size", buffer.Length.ToString());
                hashValue = mySHA.ComputeHash(buffer);
                req.Headers.Add("X-unit-hash", BitConverter.ToString(hashValue).Replace("-", ""));
            }

            System.Threading.Thread.Sleep(1000);

            using (var rs = req.GetRequestStream())
            {
                rs.Write(buffer, 0, buffer.Length);
            }

            return req;
        }

        static public RestSharp.RestRequest Generate_PollUpload_Request(string SessionToken, string UploadKey)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.UploadPoll, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);
            rr.AddParameter("key", UploadKey);
            
            rr.AddParameter("response_format", "json");

            return rr;
        }

        static public string GetResponseContent(System.Net.WebResponse res)
        {
            string result = "";
            using (var rs = res.GetResponseStream())
            {
                byte[] buff = new byte[res.ContentLength];
                rs.Read(buff, 0, (int)res.ContentLength);
                result = Encoding.UTF8.GetString(buff);
            }
            return result;
        }

        static public List<bool> decodeBitmap(IUploadBitmap bitmap)
        {
            var uploadUnits = new List<bool>();
            for (int i = 0; i < 16 * bitmap.Count; i++)
            {
                uploadUnits.Add(false);
            }

            for (var i = 0; i < bitmap.Count; i++)
            {
                var word = bitmap.Words[i];
                var bin = Convert.ToString(word, 2);
                while (bin.Length < 16)
                    bin = "0" + bin;
                for (var b = 0; b < bin.Length; b++)
                    uploadUnits[i * 16 + b] = (bin[15 - b] == '1');
            }
            return uploadUnits;
        }
        #endregion

        #region Device

        static public RestSharp.RestRequest Generate_GetTrash_Request(string SessionToken)
        {
            RestRequest rr = new RestRequest(MediaFireConfiguration.Trash, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            rr.AddParameter("session_token", SessionToken);

            rr.AddParameter("response_format", "json");

            return rr;
        }

        #endregion
    }
}
