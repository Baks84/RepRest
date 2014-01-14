using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RESTClient.MediaFire.Implements;
using System.Threading.Tasks;

namespace RESTClient.MediaFire
{
    public enum UploadDuplicateActions { None, Keep, Skip, Replace };

    public class MediaFireApi : API
    {
        #region Public Properties

        public enum GetFolderContentSettings {Both, Folders, Files}

        /// <summary>
        /// Bearer Token used for authorisation.
        /// If the token is not known it will be filled after authorizin application with an account.
        /// It should be saved.
        /// Next this field should be filled before making any other call to Dropbox
        /// </summary>
        public IToken Token
        {
            get;
            set;
        }

        public List<IStoreDataInfo> Root
        {
            get
            {
                List<IStoreDataInfo> result = new List<IStoreDataInfo>();

                //Get API base URL
                this.BaseUrl = MediaFireConfiguration.APIBaseURL;

                //Create an Authorization request and Execute it.
                var rresult = base.Execute(MediaFireApiHelper.Generate_GetRoot_Request(this.Token.Access_Token));
                var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderContentResponse>(BaseResponse.ClearResponse(rresult.Content));

                rresult = base.Execute(MediaFireApiHelper.Generate_GetRoot_Request(this.Token.Access_Token, false));
                var res2file = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderContentResponse>(BaseResponse.ClearResponse(rresult.Content));

                //Check if received Token is valid
                if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/get_content" || res2.result != "Success"
                    || res2file == null || res2file.action == null || res2file.result == null || res2file.action != "folder/get_content" || res2file.result != "Success")
                {
                    if (res2.result == "Error")
                    {
                        throw new Exception(res2.message);
                    }
                    if (res2file.result == "Error")
                    {
                        throw new Exception(res2file.message);
                    }
                    //Not valid
                    return null;
                }
                else 
                {
                    foreach (var item in res2.folder_content.folders)
                    {
                        result.Add(item);
                    }
                    foreach (var item in res2file.folder_content.files)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
        }

        #endregion

        #region Constructors
                public MediaFireApi()
                    : base()
                {
                    InitializeDict();
                }

                public MediaFireApi(IToken token)
                    : this()
                {
                    this.Token = token;
                }

                private void InitializeDict()
                {
                }
                #endregion

        #region Methods
        /// <summary>
        /// Authorizes an AccessToken and recieves the Bearer Token.
        /// </summary>
        /// <param name="AccessToken">String that will be displayed on the Dropbox site after authorizating an application with user account</param>
        /// <returns>True if Token was authroized successfully, False elsewhere</returns>
        public bool GetSessionToken(string UserEmail, string UserPassword)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.GenerateGetSessionTokenRequest(UserEmail, UserPassword));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSessionTokenResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "user/get_session_token" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }
            else
            {
                this.Token = new MediaFire.Implements.Token();
                this.Token.Access_Token = res2.session_token;
                this.Token.Token_Type = "bearer";
                this.Token.PKEY = res2.pkey;
                //Valid
                return true;
            }
        }

        /// <summary>
        /// Refreshes the Token.
        /// </summary>
        /// <param name="AccessToken">String that will be displayed on the Dropbox site after authorizating an application with user account</param>
        /// <returns>True if Token was authroized successfully, False elsewhere</returns>
        public bool RenewSessionToken()
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.GenerateRenewSessionTokenRequest(this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSessionTokenResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "user/renew_session_token" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }
            else
            {
                this.Token.Access_Token = res2.session_token;
                //Valid
                return true;
            }
        }

        /// <summary>
        /// Retrieves information about users account
        /// </summary>
        /// <returns>IUserInfo object with user account information</returns>
        public IUserInfo GetUserInfo()
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetUserInfo_Request(this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetUserInfoResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "user/get_info" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.User_info;
        }

        #region Folder
        /// <summary>
        /// Get the detailed information about the folder
        /// </summary>
        /// <param name="Folder_Key">Id of the folder</param>
        /// <returns>IFolderInfo object</returns>
        public IFolderInfo GetFolderInfo(string Folder_Key)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetFolderInfo_Request(Folder_Key, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderInfoResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/get_info" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.Folder_Info;
        }

        /// <summary>
        /// Gets the list and information about the files and folders inside the folder
        /// </summary>
        /// <param name="Folder_Key"></param>
        /// <returns></returns>
        public List<IStoreDataInfo> GetFolderContent(string Folder_Key, GetFolderContentSettings settings = GetFolderContentSettings.Both)
        {
            List<IStoreDataInfo> result = new List<IStoreDataInfo>();

            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            RestSharp.IRestResponse rresult = null;
            GetFolderContentResponse res2 = null;
            GetFolderContentResponse res2file = null;

            if (settings != GetFolderContentSettings.Files)
            {
                rresult = base.Execute(MediaFireApiHelper.Generate_GetFolderContent_Request(Folder_Key, this.Token.Access_Token));
                res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderContentResponse>(BaseResponse.ClearResponse(rresult.Content));

                //Check if received Token is valid
                if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/get_content" || res2.result != "Success")
                {
                    if (res2.result == "Error")
                    {
                        throw new Exception(res2.message);
                    }
                    return null;
                }
                else
                {
                    foreach (var item in res2.folder_content.folders)
                    {
                        result.Add(item);
                    }
                }
            }
            if (settings != GetFolderContentSettings.Folders)
            {
                rresult = base.Execute(MediaFireApiHelper.Generate_GetFolderContent_Request(Folder_Key, this.Token.Access_Token, false));
                res2file = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderContentResponse>(BaseResponse.ClearResponse(rresult.Content));

                //Check if received Token is valid
                if (res2file == null || res2file.action == null || res2file.result == null || res2file.action != "folder/get_content" || res2file.result != "Success")
                {
                    if (res2file.result == "Error")
                    {
                        throw new Exception(res2.message);
                    }
                    return null;
                }
                else
                {
                    foreach (var item in res2file.folder_content.files)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves folder Delta information with Revision
        /// !!! In January 2014 retrieving detailed informations about what changed is not working from the API side !!!
        /// </summary>
        /// <param name="Folder_Key">ID of the folder</param>
        /// <returns>Delta information of the folder</returns>
        public IFolderDelta GetFolderRevision(string Folder_Key)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetFolderRevision_Request(Folder_Key, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFolderRevisionResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/get_revision" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.Changes;
        }

        /// <summary>
        /// Updates folder data
        /// </summary>
        /// <param name="Folder_Key">ID of the folder</param>
        /// <param name="FolderName">The new name for the folder. If leaved empty it will be not changed</param>
        /// <param name="Description">THe new description of the folder. If leaved Empty will be not changed</param>
        /// <param name="Tags">THe new tags for the folder. If leaved empty will be not changed.</param>
        /// <returns>True if success, false if something went wrong</returns>
        public bool UpdateFolder(string Folder_Key, string FolderName = "", string Description = "", string Tags = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_UpdateFolder_Request(Folder_Key, this.Token.Access_Token, FolderName, Description, Tags));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/update" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Copies folder to another location.
        /// </summary>
        /// <param name="Src_Folder_Key">ID of the folder that should be copied</param>
        /// <param name="Dst_Folder_Key">ID of the destination folder. If leaved empty folder will be moved to the root.</param>
        /// <returns>IFolderInfo object with new ID for the folder</returns>
        public IFolderInfo CopyFolder(string Src_Folder_Key, string Dst_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_CopyFolder_Request(Src_Folder_Key, this.Token.Access_Token, Dst_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CopyFolderResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/copy" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return new FolderInfo()
            {
                folderkey = res2.new_folderkeys[0]
            };
        }

        /// <summary>
        /// Moves folder to another location.
        /// </summary>
        /// <param name="Src_Folder_Key">ID of the folder that should be moved</param>
        /// <param name="Dst_Folder_Key">ID of the destination folder. If leaved empty folder will be moved to the root.</param>
        /// <returns>True if all ok.</returns>
        public bool MoveFolder(string Src_Folder_Key, string Dst_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_MoveFolder_Request(Src_Folder_Key, this.Token.Access_Token, Dst_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/move" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates new folder
        /// </summary>
        /// <param name="FolderName">Name of the new folder</param>
        /// <param name="Parent_Folder_Key">ID of the folder in which the folder should be created. If empty the folder will be created in the root.</param>
        /// <returns>IFolderInfo with id of the new folder. In CustomUrl field is the UploadID which can be used to check if folder was really phisically uploaded.</returns>
        public IFolderInfo CreateFolder(string FolderName, string Parent_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_CreateFolder_Request(FolderName, this.Token.Access_Token, Parent_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateFolderResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "folder/create" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return new FolderInfo()
            {
                Created = res2.created,
                Custom_url = res2.upload_key,
                folderkey = res2.folder_key,
                name = res2.name,
                Revision = res2.revision
            };
        }

        /// <summary>
        /// Deletes a folder. Normally it only moves it to the trash. If "Permamently" flag is set to true, it will be permamently deleted.
        /// </summary>
        /// <param name="FolderKey">ID of the folder</param>
        /// <param name="Permamently">If false - folder is moved to trash can, if True folder is deleted permamently</param>
        /// <returns>True if success.</returns>
        public bool DeleteFolder(string FolderKey, bool Permamently = false)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_DeleteFolder_Request(FolderKey, this.Token.Access_Token, Permamently));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || (res2.action != "folder/delete" && res2.action != "folder/purge") || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        } 
        #endregion

        #region File

        /// <summary>
        /// Copies file to another location.
        /// </summary>
        /// <param name="Src_File_Key">ID of the file that should be copied</param>
        /// <param name="Dst_Folder_Key">ID of the destination folder. If leaved empty folder will be moved to the root.</param>
        /// <returns>IFileInfo object with new ID for the file</returns>
        public IFileInfo CopyFile(string Src_File_Key, string Dst_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_CopyFile_Request(Src_File_Key, this.Token.Access_Token, Dst_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CopyFileResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/copy" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return new FileInfo()
            {
                quickkey = res2.new_quickkeys[0]
            };
        }

        /// <summary>
        /// Creates file.
        /// </summary>
        /// <param name="FileName">Name of the file to be created</param>
        /// <param name="Dst_Folder_Key">ID of the destination folder. If leaved empty folder will be moved to the root.</param>
        /// <returns>IFileInfo object with new file information</returns>
        public IFileInfo CreateFile(string FileName, string Dst_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_CreateFile_Request(FileName, this.Token.Access_Token, Dst_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFileInfoResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/create" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.File_Info == null ? res2.FileInfo : res2.File_Info;
        }

        /// <summary>
        /// Creates file snapshot (new revision).
        /// </summary>
        /// <param name="FileKey">ID of the file</param>
        /// <returns>new revision number</returns>
        public int CreateFileSnapshot(string FileKey)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_CreateFileSnapshot_Request(FileKey, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateSnapshotResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/create_snapshot" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return -1;
            }

            return res2.new_revision;
        }

        /// <summary>
        /// Deletes a file. Normally it only moves it to the trash. If "Permamently" flag is set to true, it will be permamently deleted.
        /// </summary>
        /// <param name="FileKey">ID of the file</param>
        /// <param name="Permamently">If false - file is moved to trash can, if True file is deleted permamently</param>
        /// <returns>True if success.</returns>
        public bool DeleteFile(string FileKey, bool Permamently = false)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_DeleteFile_Request(FileKey, this.Token.Access_Token, Permamently));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || (res2.action != "file/delete" && res2.action != "file/purge") || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the links to the file: View, Edit, Normal Download, Direct Download
        /// </summary>
        /// <param name="FileID">Id of the file</param>
        /// <returns>IFileLinks object</returns>
        public IFileLinks GetFileLinks(string FileID)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetFileLinks_Request(FileID, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<LinksResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/get_links" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            res2.links[0].Direct_download_free_bandwidth = res2.Direct_download_free_bandwidth;
            res2.links[0].One_time_download_request_count = res2.One_time_download_request_count;

            return res2.links[0];
        }

        /// <summary>
        /// Returns the list of the files versions
        /// </summary>
        /// <param name="FileID">Id of the file</param>
        /// <returns>IFileVersions object</returns>
        public IFileVersions GetFileVersions(string FileID)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetFileVersions_Request(FileID, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<FileVersions>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/get_versions" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2;
        }

        /// <summary>
        /// Moves file to another location.
        /// </summary>
        /// <param name="Src_File_Key">ID of the file that should be moved</param>
        /// <param name="Dst_Folder_Key">ID of the destination folder. If leaved empty file will be moved to the root.</param>
        /// <returns>IFileInfo object with new revision number for the file</returns>
        public bool MoveFile(string Src_File_Key, string Dst_Folder_Key = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_MoveFile_Request(Src_File_Key, this.Token.Access_Token, Dst_Folder_Key));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/move" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the daily limits for "One Time Download" links
        /// </summary>
        /// <returns>IOneTimeDownload object only with limits filled up</returns>
        public IOneTimeDownload GetFileOneTimeDownloadDailyLimit()
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetDailyOneTimeDownloadLimit_Request(this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<OneTimeDownload>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/one_time_download" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2;
        }

        /// <summary>
        /// Creates a one-time download link. A one time download link is a download key that can only be used by a specific recipient. Once it has been used it can optionally be destroyed after us.
        /// </summary>
        /// <param name="FileID">The quickkey of the file to generate the one-time download link.</param>
        /// <param name="Duration">The number of minutes this link is valid. If not passed, then the link expires after 30 days.</param>
        /// <param name="EmailNotification">Indicates whether or not to send an email notification to the file owner.(default 'no').</param>
        /// <param name="SuccessCallback">An absolute URL which is called when the user successfully downloads the file.</param>
        /// <param name="ErrorCallback">An absolute URL which is called when the download fails.</param>
        /// <param name="BindIP">An IP mask/range or a comma-separated list of IP masks/ranges to restrict the download to matching user's IP addresses. (e.g. '68.154.11.0/8, 145.230.230.115-145.230.240.33, 78.192.10.10').</param>
        /// <param name="BurnAfterUse">Whether or not to invalidate the one-time link once it's used. This parameter is ignored if bind_ip is not passed. (default 'yes').</param>
        /// <returns></returns>
        public IOneTimeDownload GetFileOneTimeDownloadLink(string FileID, int Duration = 0, bool EmailNotification = false, Uri SuccessCallback = null, Uri ErrorCallback = null,
            string BindIP = "", bool BurnAfterUse = true)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetDailyOneTimeDownloadLink_Request(this.Token.Access_Token, FileID, Duration, EmailNotification, SuccessCallback, ErrorCallback,
                BindIP, BurnAfterUse));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<OneTimeDownload>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/one_time_download" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2;
        }

        /// <summary>
        /// Returns a list of quickkeys of the recently modified files
        /// </summary>
        /// <returns>List of IFIleInfo elements with modified files IDs</returns>
        public List<IFileInfo> GetRecentlyModifiedFiles()
        {
            List<IFileInfo> Tresult = new List<IFileInfo>();

            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetRecentlyModifiedFileList_Request(this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<RecentlyModifiedFilesResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/recently_modified" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            foreach (var item in res2.quickkeys)
            {
                Tresult.Add(new FileInfo() { quickkey = item });
            }

            return Tresult;
        }

        /// <summary>
        /// Restores an old file revision and makes it the current head. (It does not changes back the file name - only the content)
        /// </summary>
        /// <param name="FileID">File ID</param>
        /// <param name="Revision">Revision of the file to be restored to</param>
        /// <returns></returns>
        public bool RestoreFileToRevision(string FileID, int Revision)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetRestoreFile_Request(this.Token.Access_Token, FileID, Revision));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/restore" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update a file's information.
        /// </summary>
        /// <param name="FileID">ID of the file</param>
        /// <param name="FileName">The Name of the file (Should have same file type as the old file). The filename should be 3 to 255 in length.</param>
        /// <param name="Description">The description of the file</param>
        /// <param name="Tags">A space-separated list of tags.</param>
        /// <param name="Public">Public or Private</param>
        /// <returns>True if all ok</returns>
        public bool UpdateFile(string FileID, string FileName = "", string Description = "", string Tags = "", bool Public = true)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetUpdateFile_Request(this.Token.Access_Token, FileID, FileName, Description, Tags, Public));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/update" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return false;
            }

            return true;
        }

        /// <summary>
        /// Bulk-download multiple files and folders into one single zip file. (Only Premium Accounts)
        /// </summary>
        /// <param name="IDs">a comma-separated list of quickkeys and folderkeys to be zipped.</param>
        /// <returns></returns>
        public byte[] GetZip(List<string> IDs)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var rr = MediaFireApiHelper.Generate_GetZip_Request(this.Token.Access_Token, IDs);

            var client = new RestSharp.RestClient();
            client.BaseUrl = BaseUrl;

            byte[] result = client.DownloadData(rr);

            try
            {
                string txt = Encoding.UTF8.GetString(result);
                var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(BaseResponse.ClearResponse(txt));

                //Check if received Token is valid
                if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/zip" || res2.result != "Success")
                {
                    if (res2.result == "Error")
                    {
                        throw new ApplicationException(res2.message);
                    }
                    //Not valid
                    return null;
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                //Propably correct zip file
            }

            return result;
        }

        public String GetDirectDownloadLink(string FileID)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetDirectDownloadLink_Request(FileID, this.Token.Access_Token));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<LinksResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "file/get_links" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            res2.links[0].Direct_download_free_bandwidth = res2.Direct_download_free_bandwidth;
            res2.links[0].One_time_download_request_count = res2.One_time_download_request_count;

            return res2.links[0].Direct_Download;
        }

        /// <summary>
        /// Downloads a file and returns it in a byte array
        /// </summary>
        /// <param name="FileData">It is FileID or Direct Download Link. If IsID is True this should be FileID, else this should be Direct Download Link</param>
        /// <param name="IsID">If True the FileData should be the FileID. Otherwise FileID should be Direct Download Link</param>
        /// <returns>Bytes array</returns>
        public byte[] DownloadFile(string FileData, bool IsID = true)
        {
            string DDLink = "";
            if (IsID)
            {
                DDLink = GetDirectDownloadLink(FileData);
            }

            if (!string.IsNullOrEmpty(DDLink))
            {
                var client = new RestSharp.RestClient();
                client.BaseUrl = "";
                var rr = MediaFireApiHelper.Generate_DownloadFile_Request(DDLink);
                byte[] testRes = client.DownloadData(rr);
                return testRes;
            }
            return null;
        }

        /// <summary>
        /// Downloads File Asynchronously.
        /// </summary>
        /// <param name="FileData">It is FileID or Direct Download Link. If IsID is True this should be FileID, else this should be Direct Download Link</param>
        /// <param name="IsID">If True the FileData should be the FileID. Otherwise FileID should be Direct Download Link</param>
        /// <returns>Asynchronous task. When the task is finished it has the file in Result property</returns>
        public Task<byte[]> DownloadFileAsync(string FileData, bool IsID = true)
        {
            Task<byte[]> t = Task.Factory.StartNew(() =>
                {
                    return DownloadFile(FileData, IsID);
                });
            return t;
        }

        /// <summary>
        /// Download a file and saves it under given path
        /// </summary>
        /// <param name="FileData">It is FileID or Direct Download Link. If IsID is True this should be FileID, else this should be Direct Download Link</param>
        /// <param name="IsID">If True the FileData should be the FileID. Otherwise FileID should be Direct Download Link</param>
        /// <param name="Path">The path where the file should be saved with its name and extension</param>
        public void DownloadFile(string FileData, string Path, bool IsID = true)
        {
            byte[] file = DownloadFile(FileData, IsID);
            System.IO.File.WriteAllBytes(Path, file);
        }

        /// <summary>
        /// Download file asynchronously and saves it to the given Path
        /// </summary>
        /// <param name="FileData">It is FileID or Direct Download Link. If IsID is True this should be FileID, else this should be Direct Download Link</param>
        /// <param name="IsID">If True the FileData should be the FileID. Otherwise FileID should be Direct Download Link</param>
        /// <param name="Path">The path where the file should be saved with its name and extension</param>
        /// <returns>Asynchronous task.</returns>
        public Task DownloadFileAsync(string FileData, String Path, bool IsID = true)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                DownloadFile(FileData, Path, IsID);
            });
            return t;
        }

        #endregion

        #region Upload

        /// <summary>
        /// Adds a new web upload and returns the Upload Key on success.
        /// </summary>
        /// <param name="URL">The URL of the file you wish to be placed into your MediaFire account.</param>
        /// <param name="FileName">The name you want MediaFire to assign to the file from the given URL.</param>
        /// <param name="FolderID">The key that identifies the destination folder. If not passed, then it will return the root folder</param>
        /// <returns>IUpload with UploadID</returns>
        public IUpload AddWebUpload(Uri URL, string FileName, string FolderID="")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_AddWebUpload_Request(this.Token.Access_Token, URL, FileName, FolderID));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Upload>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "upload/add_web_upload" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2;
        }

        /// <summary>
        /// Returns a list of web uploads currently in progress or all web uploads.
        /// </summary>
        /// <param name="GetAll">Whether or not to return all web uploads, or just the active ones.</param>
        /// <returns>List of the WebUploads</returns>
        public List<IWebUpload> GetWebUploads(bool GetAll = false)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_GetWebUploads_Request(this.Token.Access_Token, GetAll));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<WebUploads>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "upload/get_web_uploads" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.GetItems();
        }

        /// <summary>
        /// Determines if instant upload is possible, if a duplicate filename exists in the destination folder, 
        /// and also verifies folder permissions for non-owner uploads. This returns a 'quickkey' on successful instant upload. 
        /// Otherwise, 'new_hash' and 'duplicate_name' are returned, which can be 'yes' or 'no'. 
        /// Based on those values, the uploader performs a regular upload or resends the same pre_upload request with the desired action. 
        /// If 'path' is specified and an instant upload was not possible, the a 'folder_key' will also be returned to be used for a regular upload.
        /// </summary>
        /// <param name="FileName">
        /// Path, name and extension of the file for which the upload should be prepared
        /// </param>
        /// <param name="MakeHash">
        /// True if SHA256 hash of file should be calculated, if false, only duplicate name is checked and no resumable upload is possible.
        /// </param>
        /// <param name="Upl_FolderID">
        /// The folderkey of the folder you wish to upload your file to. If not passed, the destination folder is the root folder.
        /// </param>
        /// <param name="ActionOnDuplicate">
        /// This is used in the case where there are duplicate file names in the same upload folder. 
        /// The values are 'keep' (to keep both files with the same name, the new file will have a numeric digit added to it), 
        /// 'skip' (this will ignore the upload), and 'replace' (this will override the original file with the new file). 
        /// If not passed, then no action will be performed on duplicate names and a flag 'duplicate_name' is set to 'yes' requesting the action to be passed.
        /// </param>
        /// <param name="Resumable"> 
        /// In the circumstance of the upload being interrupted, this indicates whether this upload can be resumed.
        /// </param>
        /// <param name="Path">
        /// The path relative to 'upload_folder_key' where the file should be uploaded. Any folders specified will be created if they do not exist.
        /// </param>
        /// <returns>IPreUpload object</returns>
        public IPreUpload PreUpload(string FileName, bool MakeHash = false, string Upl_FolderID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, bool Resumable = false, string Path = "")
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_PreUpload_Request(this.Token.Access_Token, FileName, MakeHash, Upl_FolderID, ActionOnDuplicate, Resumable, Path));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PreUpload>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "upload/pre_upload" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2;
        }

        /// <summary>
        /// Upload a file through POST to the user's account. This api returns the upload key and hash when successful. 
        /// You will have to pass this key to upload/poll_upload to get the FileID.
        /// </summary>
        /// <param name="FileName">Path, name and extension of the file to be uploaded.</param>
        /// <param name="Upl_FolderID">The folderkey of the folder where to store the file. If it's not passed, then the file will be stored in the root folder.</param>
        /// <param name="FileID">If a FileID is passed, the uploaded file content will overwrite an existing file's current revision defined by the passed FileID.</param>
        /// <param name="ActionOnDuplicate">This parameter can take 'skip', 'keep', or 'replace' as values. This will be honored only if a file with the same name in the same folder already exists. This will determine whether the file should be skipped, kept (with an appended number), or replaced.</param>
        /// <param name="Path">The path relative to 'upload_folderid' where the file should be uploaded. Any folders specified will be created if they do not exist.</param>
        /// <param name="Previous_Hash">This is the hash of the last known hash to the client of the file before it's modified. This is honored only on update uploads, that is, when passing 'quickkey' to override an existing file on the cloud. If the previous hash is different than the current version on the cloud, then this is a conflict and the file will be uploaded as a new file with new quickkey and filename.</param>
        /// <returns>IUpload object with UploadKey and Hash if succesfull and Upload Result Code and Message if not.</returns>
        public IUpload UploadFile(string FileName, string Upl_FolderID = "", string FileID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, string Path = "", string Previous_Hash = "")
        {
            var req = MediaFireApiHelper.Generate_Upload_Request(this.Token.Access_Token, MediaFireConfiguration.BaseURL, FileName,
                    Upl_FolderID, FileID, ActionOnDuplicate, Path, Previous_Hash);

            var res = req.GetResponse();

            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Upload>(BaseResponse.ClearResponse(MediaFireApiHelper.GetResponseContent(res)));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "upload/upload.php" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            res2.Hash = req.Headers["X-Filehash"];

            return res2;
        }

        /// <summary>
        /// Upload a file through POST to the user's account. This api returns the upload key and hash when successful. 
        /// You will have to pass this key to upload/poll_upload to get the FileID.
        /// </summary>
        /// <param name="FileName">Path, name and extension of the file to be uploaded.</param>
        /// <param name="Upl_FolderID">The folderkey of the folder where to store the file. If it's not passed, then the file will be stored in the root folder.</param>
        /// <param name="FileID">If a FileID is passed, the uploaded file content will overwrite an existing file's current revision defined by the passed FileID.</param>
        /// <param name="ActionOnDuplicate">This parameter can take 'skip', 'keep', or 'replace' as values. This will be honored only if a file with the same name in the same folder already exists. This will determine whether the file should be skipped, kept (with an appended number), or replaced.</param>
        /// <param name="Path">The path relative to 'upload_folderid' where the file should be uploaded. Any folders specified will be created if they do not exist.</param>
        /// <param name="Previous_Hash">This is the hash of the last known hash to the client of the file before it's modified. This is honored only on update uploads, that is, when passing 'quickkey' to override an existing file on the cloud. If the previous hash is different than the current version on the cloud, then this is a conflict and the file will be uploaded as a new file with new quickkey and filename.</param>
        /// <returns>IUpload object with UploadKey and Hash if succesfull and Upload Result Code and Message if not.</returns>
        public Task<IUpload> UploadFileAsync(string FileName, string Upl_FolderID = "", string FileID = "",
                UploadDuplicateActions ActionOnDuplicate = UploadDuplicateActions.None, string Path = "", string Previous_Hash = "")
        {
            Task<IUpload> t = Task<IUpload>.Factory.StartNew(() =>
            {
                return UploadFile(FileName, Upl_FolderID, FileID, ActionOnDuplicate, Path, Previous_Hash);
            });
            return t;
        }

        public IPollUpload Upload_Poll(string UploadKey)
        {
            //Get API base URL
            this.BaseUrl = MediaFireConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            var result = base.Execute(MediaFireApiHelper.Generate_PollUpload_Request(this.Token.Access_Token, UploadKey));
            var res2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadPollResponse>(BaseResponse.ClearResponse(result.Content));

            //Check if received Token is valid
            if (res2 == null || res2.action == null || res2.result == null || res2.action != "upload/poll_upload" || res2.result != "Success")
            {
                if (res2.result == "Error")
                {
                    throw new Exception(res2.message);
                }
                //Not valid
                return null;
            }

            return res2.doupload;
        }
        #endregion
        
        #endregion
    }
}
