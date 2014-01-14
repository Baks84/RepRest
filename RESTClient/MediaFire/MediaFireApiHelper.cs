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

        static public RestSharp.RestRequest Generate_Upload_Request(string SessionToken, string FileName, string Upl_FolderID = "", string FileID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, string Path = "", string Previous_Hash = "")
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);

            StringBuilder uploadAddress = new StringBuilder(MediaFireConfiguration.Upload).Append("&session_token=").Append(SessionToken);
            if (!string.IsNullOrEmpty(Upl_FolderID))
            {
                uploadAddress.Append("&upload_key=").Append(Upl_FolderID);
            }

            RestRequest rr = new RestRequest(uploadAddress.ToString(), Method.POST);//;MediaFireConfiguration.Upload, Method.POST);
            rr.RequestFormat = DataFormat.Json;
            rr.AddHeader("Content-Type", "application/octet-stream");
            //rr.AddParameter("session_token", SessionToken);

            if (!System.IO.File.Exists(FileName))
            {
                throw new System.IO.FileNotFoundException("The file " + FileName + " was not found");
            }

            rr.AddHeader("X-Filename", fileInfo.Name);
            rr.AddHeader("X-Filesize", fileInfo.Length.ToString());
            rr.AddHeader("X-Filetype", "text/plain");

            using (System.IO.FileStream fs = fileInfo.OpenRead())
            {
                fs.Position = 0;

                SHA256 mySHA = SHA256Managed.Create();
                byte[] hashValue = mySHA.ComputeHash(fs);
                rr.AddHeader("X-Filehash", BitConverter.ToString(hashValue).Replace("-", ""));
            }

            rr.AddParameter("file", System.IO.File.ReadAllBytes(FileName), ParameterType.RequestBody);

            //if (!string.IsNullOrEmpty(FileID))
            //{
            //    rr.AddParameter("quick_key", FileID);
                
            //    if (!string.IsNullOrEmpty(Previous_Hash))
            //    {
            //        rr.AddParameter("previous_hash", Previous_Hash);
            //    }
            //}

            //if (!string.IsNullOrEmpty(Path))
            //{
            //    rr.AddParameter("path", Path);
            //}

            //switch (ActionOnDuplicate)
            //{
            //    case UploadDuplicateActions.Keep:
            //        rr.AddParameter("action_on_duplicate", "keep");
            //        break;
            //    case UploadDuplicateActions.Skip:
            //        rr.AddParameter("action_on_duplicate", "skip");
            //        break;
            //    case UploadDuplicateActions.Replace:
            //        rr.AddParameter("action_on_duplicate", "replace");
            //        break;
            //}

            //rr.AddParameter("response_format", "json");
            

            return rr;
        }
        #endregion
    }
}
