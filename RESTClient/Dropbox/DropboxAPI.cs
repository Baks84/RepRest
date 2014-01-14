using RESTClient.Dropbox.Implements;
using RestSharp.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace RESTClient.Dropbox
{
    /// <summary>
    /// Wraps Dropbox API into C# code.
    /// Written in .NET 4.0
    /// </summary>
    public class DropboxAPI : API
    {
        /// <summary>
        /// Possible thumbnails sizes
        /// </summary>
        public enum Thumbnail_Sizes{xs, s,m,l,xl};

        #region Public Properties
        
        /// <summary>
        /// Bearer Token used for authorisation.
        /// If the token is not known it will be filled after authorizin application with an account.
        /// It should be saved.
        /// Next this field should be filled before making any other call to Dropbox
        /// </summary>
        public Token Token
        {
            get;
            set;
        }

        /// <summary>
        /// URL which should be copied by the user to the browser to perform application authorization
        /// </summary>
        public String RegistrationURL
        {
            get
            {
                return DropBoxConfiguration.OAuth2RegistrationURL;
            }
        }
        
        #endregion

        #region Constructors
        public DropboxAPI()
            : base()
        {
        }

        public DropboxAPI(Token token)
            : base()
        {
            this.Token = token;
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Authorizes an AccessToken and recieves the Bearer Token.
        /// </summary>
        /// <param name="AccessToken">String that will be displayed on the Dropbox site after authorizating an application with user account</param>
        /// <returns>True if Token was authroized successfully, False elsewhere</returns>
        public bool AuthorizeToken(string AccessToken)
        {
            //Get API base URL
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;

            //Create an Authorization request and Execute it.
            Token = base.Execute<Token>(DropBoxApiHelper.GenerateTokenAuthorisationRequest(AccessToken));

            //Check if received Token is valid
            if (Token == null || Token.Token_Type != "bearer")
            {
                //Not valid
                return false;
            }
            else
            {
                //Valid
                return true;
            }
        }

        /// <summary>
        /// Retrieves information about the user's account.
        /// </summary>
        /// <param name="Locale">Dropbox uses the locale parameter to specify language settings of content responses. 
        /// If your app supports any language other than English, insert the appropriate IETF language tag
        /// <example> (en-EN) </example>
        /// </param>
        /// <returns>User account information in IUserInfo</returns>
        public IUserInfo GetUserInfo(string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_GetUserAccount_Request(Locale); 
            return this.Execute<UserInfo>(rr);
        }

        /// <summary>
        /// Downloads a file. 
        /// </summary>
        /// <param name="FilePath">The path to the file you want to retrieve.</param>
        /// <param name="rev">The revision of the file to retrieve. This defaults to the most recent revision.</param>
        /// <returns>File in bytes array</returns>
        public byte[] DownloadFile(string FilePath, int rev = 0)
        {
            //Get API Content URL
            this.BaseUrl = DropBoxConfiguration.APIContentURL;

            //Create request
            RestRequest rr = DropBoxApiHelper.Generate_DownloadFile_Request(FilePath, rev);
            
            //Execute request
            return this.DownloadFile(rr);
        }

        /// <summary>
        /// Downloads a file. 
        /// </summary>
        /// <param name="FilePath">The path to the file you want to retrieve.</param>
        /// <param name="rev">The revision of the file to retrieve. This defaults to the most recent revision.</param>
        /// <returns>File in bytes array</returns>
        public Task<byte[]> DownloadFileAsync(string FilePath, int rev = 0)
        {
            Task<byte[]> t = Task.Factory.StartNew(() =>
                {
                    return DownloadFile(FilePath, rev);
                });
            return t;
        }

        /// <summary>
        /// Uploads a file using PUT semantics.
        /// <remarks> UploadFile has a maximum file size limit of 150 MB. 
        /// Larger files will be uploaded with UploadAsync() but it will be done in
        /// synchronues way (function will block execution until file is uploaded).
        /// This function is good for small files with no progress information needed.
        /// For greater files and if progress information is needed please use UploadAsync()
        /// </remarks>
        /// </summary>
        /// <param name="locFileName">Path to the file, that should be transmitted, with its name, on the local computer</param>
        /// <param name="trgFileName">The full path on the Dropbox to the file you want to write to. This parameter should not point to a folder.</param>
        /// <param name="locale">The metadata returned on successful upload will have its size field translated based on the given locale.</param>
        /// <param name="overwrite">This value, either true (default) or false, determines what happens when there's already a file at the specified path. \
        /// If true, the existing file will be overwritten by the new one. 
        /// If false, the new file will be automatically renamed (for example, test.txt might be automatically renamed to test (1).txt). The new name can be obtained from the returned metadata.</param>
        /// <param name="parent_rev">The revision of the file you're editing. 
        /// If parent_rev matches the latest version of the file on the user's Dropbox, that file will be replaced. 
        /// Otherwise, the new file will be automatically renamed (for example, test.txt might be automatically renamed to test (conflicted copy).txt). 
        /// If you specify a revision that doesn't exist, the file won't save (error 400). Get the most recent rev by performing a call to /metadata.
        /// </param>
        /// <returns>The metadata for the uploaded file. If null there was an error</returns>
        public IFileInfo UploadFile(string locFileName, string trgFileName, string locale = "en-EN", bool overwrite = true,
            string parent_rev = "")
        {
            IFileInfo result = null;

            //Check if file exists
            if (File.Exists(locFileName))
            {
                //Calculate its size in MB
                float SizeInMB = ((float)new System.IO.FileInfo(locFileName).Length / 1024f) / 1024f;

                //If zise is greater than 150MB (Dropbox Limit)
                if (SizeInMB >= 150)
                {
                    //Upload it Asyncrously
                    var t = UploadAsync(null,
                        (fi) => { result = fi; }, locFileName, trgFileName, locale, overwrite, parent_rev);

                    //Wait for background process to complete
                    while (!t.IsCompleted)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                }
                else
                {
                    //Upload file in one PUT request

                    //Get API Content URL
                    this.BaseUrl = DropBoxConfiguration.APIContentURL;

                    //Create request
                    RestRequest rr = DropBoxApiHelper.Generate_UploadFile_Request(
                        trgFileName,
                        locFileName,
                        File.ReadAllBytes(locFileName).Length,
                        trgFileName, locale, overwrite, parent_rev);

                    //Execute request
                    result = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);
                }
            }
            return result;
        }

        /// <summary>
        /// Uploads large files to Dropbox in multiple chunks. 
        /// Also has the ability to resume if the upload is interrupted. 
        /// This allows for uploads larger than the UploadFile() maximum of 150 MB.
        /// </summary>
        /// <param name="Progress">Action that will be invoked when the progress changes.It takes a float argument with the actual progress value in %</param>
        /// <param name="Finished">Action that will be invoked when the upload finishes.It takes a IFileInfo argument with the uploaded file metadata</param>
        /// <param name="locFileName">Path to the file, that should be transmitted, with its name, on the local computer</param>
        /// <param name="trgFileName">The full path on the Dropbox to the file you want to write to. This parameter should not point to a folder.</param>
        /// <param name="locale">The metadata returned on successful upload will have its size field translated based on the given locale.</param>
        /// <param name="overwrite">This value, either true (default) or false, determines what happens when there's already a file at the specified path. \
        /// If true, the existing file will be overwritten by the new one. 
        /// If false, the new file will be automatically renamed (for example, test.txt might be automatically renamed to test (1).txt). The new name can be obtained from the returned metadata.</param>
        /// <param name="parent_rev">The revision of the file you're editing. 
        /// If parent_rev matches the latest version of the file on the user's Dropbox, that file will be replaced. 
        /// Otherwise, the new file will be automatically renamed (for example, test.txt might be automatically renamed to test (conflicted copy).txt). 
        /// If you specify a revision that doesn't exist, the file won't save (error 400). Get the most recent rev by performing a call to /metadata.
        /// <param name="reuploadData">Information that is used to resume the upload. Element contains UploadID, offset in bytes and expiration date</param>
        /// <returns>Information about the upload. They can be used to check if all bytes have been uploaded or to resume an upload in later time</returns>
        public Task<IChunkUploadResult> UploadAsync(
            Action<float> Progress, 
            Action<IFileInfo> Finished,
            string locFileName, 
            string trgFileName, 
            string locale = "en-EN", 
            bool overwrite = true,
            string parent_rev = "",
            IChunkUploadResult reuploadData = null)
        {
            //Create a background task and start it
            Task<IChunkUploadResult> t = Task.Factory.StartNew(() =>
            {
                //Create a result of the upload
                IChunkUploadResult result = new ChunkedUploadResult();

                //CHeck the file
                if (File.Exists(locFileName))
                {
                    bool wasError = false;
                    this.BaseUrl = DropBoxConfiguration.APIContentURL;
                    System.IO.FileInfo fi = new System.IO.FileInfo(locFileName);
                    float SizeInMB = ((float)fi.Length / 1024f) / 1024f;
                    int bytesSend = 0;
                    int bytesRead = 0;
                    int chunkSize = 0;

                    //if file is greater or equal to 42MB use 4MB chunks
                    if (SizeInMB >= 42f)
                    {
                        chunkSize = 4 * 1024 * 1024;    //in bytes offset
                    }
                    else
                    {
                        //Otherwise calculate chunk size to be equal to 1%
                        chunkSize = (int)((((SizeInMB * 1024 * 1024) - 1f) / 100f) + 1f);
                    }

                    //Buffer used to be filled with chunks
                    byte[] buffer = new byte[chunkSize];

                    //Open a file for reading
                    using (var file = fi.OpenRead())
                    {
                        //If we have a reupload data we can use them to resume the upload
                        if (reuploadData != null)
                        {
                            do
                            {
                                //Read the file up to the place the upload was stopped.
                                bytesSend += file.Read(buffer, 0, buffer.Length);
                            } while (bytesSend < reuploadData.Offset);

                            //Set the current status
                            result = reuploadData;
                        }

                        //Read file in chunks till the end of the file
                        while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            //The last chunk can be smaller than others.
                            // Therefor the size of the buffer should be updated
                            if (bytesRead < chunkSize)
                            {
                                //Create temporary buffer
                                byte[] temp = new byte[bytesRead];

                                //Copy all data to temporary buffer
                                Array.Copy(buffer, temp, bytesRead);

                                //Change size of the buffer
                                buffer = new byte[bytesRead];

                                //Copy content back to our buffer
                                Array.Copy(temp, buffer, bytesRead);
                            }

                            //Create a request
                            RestRequest rr = DropBoxApiHelper.Generate_ChunkedUpload_Request(
                                buffer, result.Upload_ID, bytesSend);

                            //Execute request
                            result = this.Execute<ChunkedUploadResult>(rr);

                            //Check if the chunk was uploaded and recieved by server
                            if (result.Upload_ID != String.Empty && result.Offset == bytesSend + bytesRead)
                            {
                                //Update current state of uploaded bytes
                                bytesSend += bytesRead;

                                //If user gave an action to be taken when progress is changed run it
                                if (Progress != null)
                                {
                                    //Calculate the current progress in %
                                    float prog = ((float)bytesSend / (float)fi.Length) * 100f;

                                    //Invoke users action
                                    Progress.Invoke(prog);
                                }
                            }
                            else
                            {
                                //Chunk was not correctly uploaded
                                //Break uploadin
                                //TODO: Create option to automatically retry in some other time
                                wasError = true;
                                break;
                            }
                        }
                    }

                    //If there was not an error we need to commit the chunks
                    if (!wasError)
                    {
                        //Get API Content URL
                        this.BaseUrl = DropBoxConfiguration.APIContentURL;

                        //Create Request
                        RestRequest rr2 = DropBoxApiHelper.Generate_ChunkedUploadCommit_Request(
                            trgFileName, result.Upload_ID);

                        //Execute request
                        var a = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr2);

                        //If request finished with success and user gave an action to be invoked when all is finished
                        if (a != null && a.Bytes > 0 && Finished != null)
                        {
                            //Invoke users action
                            Finished.Invoke(a);
                        }
                    }
                    else if (Finished != null)
                    {
                        //Invoke users Action saying that there wasError an error
                        Finished.Invoke(null);
                    }
                }
                //Return Upload information
                return result;
            });

            //Return task that works in background
            return t;
        }

        /// <summary>
        /// Gets the root folder
        /// </summary>
        public IFolderInfo Root
        {
            get
            { 
                return GetFolderInfo(String.Empty, 25000);
            }
        }

        /// <summary>
        /// Gets the folder info of the given folder
        /// </summary>
        /// <param name="FolderPath">Path to the folder. When "Empty" it tooks root</param>
        /// <param name="fileLimit">Limits the number of files info that should be also read.
        /// Default: 10000. Max: 25000</param>
        /// <returns></returns>
        public IFolderInfo GetFolderInfo(string FolderPath, int fileLimit = 10000)
        {
            //Run function to get metadata for a folder
            var result = GetMetadata(FolderPath, isFile:false, file_limit:fileLimit);
            if (result is IFolderInfo) return (IFolderInfo)result;
            return null;
        }

        public IFolderInfo GetFolderInfo(IStoreDataInfo folderInfo, int fileLimit = 10000)
        {
            if (folderInfo.Is_Dir)
            {
                var result = GetMetadata(folderInfo.Path, isFile: false, file_limit: fileLimit);
                if (result is IFolderInfo) return (IFolderInfo)result;
            }
            else
            {
                throw new InvalidDataException("Parameter \"folderInfo\" is not a folder");
            }
            return null;
        }

        public IFileInfo GetFileInfo(string FilePath)
        {
            var result = GetMetadata(FilePath);
            if (result is IFileInfo) return (IFileInfo)result;
            return null;
        }

        public IFileInfo GetFileInfo(IStoreDataInfo FileInfo)
        {
            if (!FileInfo.Is_Dir)
            {
                var result = GetMetadata(FileInfo.Path);
                if (result is IFileInfo) return (IFileInfo)result;
            }
            else
            {
                throw new InvalidDataException("Parameter \"FileInfo\" is not a file");
            }
            return null;
        }

        public IDelta Delta(string Cursor = "", string locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_Delta_Request(Cursor, locale);
            var response = this.Execute(rr);
            var delta = Newtonsoft.Json.JsonConvert.DeserializeObject<Delta>(response.Content);
            delta.Deserilize();
            return delta;
        }

        public List<IFileInfo> Revisions(string Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_Revisions_Request(Path, Root, locale: Locale);
            var response = this.Execute<List<RESTClient.Dropbox.Implements.FileInfo>>(rr);

            List<IFileInfo> result = new List<IFileInfo>();

            foreach (var item in response)
            {
                result.Add(item);
            }

            return result;
        }

        public IFileInfo RestoreFile(string Path, string Rev, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_Restore_Request(Path, Rev, root: Root, locale: Locale);
            var response = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);

            return response;
        }

        public ILongpollDeltaResult LongpollDelta(string Cursor, int Timeout = 30)
        {
            this.BaseUrl = DropBoxConfiguration.APINotifyURL;
            RestRequest rr = DropBoxApiHelper.Generate_LongpollDelta_Request(Cursor, Timeout);
            var response = this.Execute<LongpollDeltaResult>(rr, false);

            return response;
        }

        public IShareLink GetShareLink(string Path, string Root = "dropbox", bool Short_Url = true, string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_ShareLink_Request(Path, root: Root, short_url:Short_Url, locale: Locale);
            var response = this.Execute<ShareLink>(rr);

            return response;
        }

        public IShareLink GetMediaShareLink(string Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_MediaLink_Request(Path, root: Root, locale: Locale);
            var response = this.Execute<ShareLink>(rr);

            return response;
        }

        public ICopyRef GetFileCopyReference(string Path, string Root = "dropbox")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_CopyRef_Request(Path, root: Root);
            var response = this.Execute<CopyRefLink>(rr);

            return response;
        }

        public Bitmap GetFileThumbnail(string Path, string Root = "dropbox", bool Jpeg_format = true, 
            Thumbnail_Sizes size = Thumbnail_Sizes.s)
        {
            Bitmap result = null;

            this.BaseUrl = DropBoxConfiguration.APIContentURL;
            RestRequest rr = null;
            if (Jpeg_format)
            {
                rr = DropBoxApiHelper.Generate_Thumbnail_Request(Path, root: Root, 
                    size:Enum.GetName(typeof(Thumbnail_Sizes), size));
            }
            else
            {
                rr = DropBoxApiHelper.Generate_Thumbnail_Request(Path, root: Root, format:"png",
                    size: Enum.GetName(typeof(Thumbnail_Sizes), size));
            }
            var response = this.DownloadFile(rr);
            
            if (response != null && response.Length > 0)
            {
                result = new Bitmap(new MemoryStream(response));
            }

            return result;
        }
       
        public IFileInfo CopyFile(string From_Path, string To_Path, string Root = "dropbox", string Locale="en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_FileCopy_Request(From_Path, To_Path, root: Root,
                locale: Locale, copyByRef: false);
            var response = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);

            return response;
        }
        
        public IFileInfo CopyFileByRef(string Ref_Path, string To_Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_FileCopy_Request(Ref_Path, To_Path, root: Root,
                locale: Locale, copyByRef: true);
            var response = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);

            return response;
        }

        public IFileInfo MoveFile(string From_Path, string To_Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_FileMove_Request(From_Path, To_Path, root: Root,
                locale: Locale);
            var response = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);

            return response;
        }

        public IFolderInfo CreateFolder(string Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_CreateFolder_Request(Path, root: Root,
                locale: Locale);
            var response = this.Execute<RESTClient.Dropbox.Implements.FolderInfo>(rr);

            return response;
        }

        public IStoreDataInfo Delete(string Path, string Root = "dropbox", string Locale = "en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_Delete_Request(Path, root: Root,
                locale: Locale);
            IStoreDataInfo response = null;

            if (Path.Length > 4 && Path[Path.Length - 4] == '.')
            {
                response = this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);
            }
            else
            {
                response = this.Execute<RESTClient.Dropbox.Implements.FolderInfo>(rr);
            }

            return response;
        }

        private IStoreDataInfo GetMetadata(string path, bool isFile = true, int file_limit = 10000, string hash = "",
            bool list = true, bool include_deleted = true, string rev="", string locale="en-EN")
        {
            this.BaseUrl = DropBoxConfiguration.APIBaseURL;
            RestRequest rr = DropBoxApiHelper.Generate_Metadata_Request(path, file_limit, hash, list, include_deleted,
                rev, locale);
            
            IStoreDataInfo result = null;
            
            if(isFile)
                result = (IStoreDataInfo)this.Execute<RESTClient.Dropbox.Implements.FileInfo>(rr);
            else
                result = (IStoreDataInfo)this.Execute<RESTClient.Dropbox.Implements.FolderInfo>(rr);
            
            return result;
        }

        public T Execute<T>(RestSharp.RestRequest request, bool Authorize) where T : new()
        {
            if (Authorize && (this.Token == null || this.Token.Token_Type.ToLower() != "bearer"))
            {
                throw new DataMisalignedException("Missing Token information or Token is incorrect");
            }
            else
            {
                var client = new RestClient();
                client.ClearHandlers();
                client.AddHandler("*", new RestSharp.Deserializers.JsonDeserializer());
                client.BaseUrl = BaseUrl;

                if (Authorize)
                {
                    client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token.Access_Token, "Bearer");
                }

                var response = client.Execute<T>(request);

                if (response.ErrorException != null)
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var twilioException = new ApplicationException(message, response.ErrorException);
                    throw twilioException;
                }
                return response.Data;
            }
        }

        #region Override
        public override T Execute<T>(RestSharp.RestRequest request)
        {
            return this.Execute<T>(request, true);
        }

        public override IRestResponse Execute(RestSharp.RestRequest request)
        {
            if (this.Token == null || this.Token.Token_Type.ToLower() != "bearer")
            {
                throw new DataMisalignedException("Missing Token information or Token is incorrect");
            }
            else
            {
                var client = new RestClient();
                client.ClearHandlers();
                client.AddHandler("*", new RestSharp.Deserializers.JsonDeserializer());
                client.BaseUrl = BaseUrl;
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token.Access_Token, "Bearer");

                var response = client.Execute(request);

                if (response.ErrorException != null)
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var twilioException = new ApplicationException(message, response.ErrorException);
                    throw twilioException;
                }
                return response;
            }
        }

        private byte[] DownloadFile(RestSharp.RestRequest request)
        {
            if (this.Token == null || this.Token.Token_Type.ToLower() != "bearer")
            {
                throw new DataMisalignedException("Missing Token information or Token is incorrect");
            }
            else
            {
                var client = new RestClient();
                client.BaseUrl = BaseUrl;
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token.Access_Token, "Bearer");

                byte[] result = client.DownloadData(request);

                //if (response.ErrorException != null)
                //{
                //    const string message = "Error retrieving response.  Check inner details for more info.";
                //    var twilioException = new ApplicationException(message, response.ErrorException);
                //    throw twilioException;
                //}
                //return response.Data;
                return result;
            }
        }
        #endregion 
        #endregion
    }
}
