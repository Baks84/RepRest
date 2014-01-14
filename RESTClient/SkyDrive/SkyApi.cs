using SkyNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;
using System.Reflection;

namespace RESTClient.SkyDrive
{
    public class SkyApi
    {
        SkyNet.Client.Client client = null;

        #region Public Properties

        /// <summary>
        /// Bearer Token used for authorisation.
        /// If the token is not known it will be filled after authorizin application with an account.
        /// It should be saved.
        /// Next this field should be filled before making any other call to Dropbox
        /// </summary>
        public UserToken Token
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
                if (client != null)
                {
                    return client.GetAuthorizationRequestUrl(
                        new List<SkyNet.Client.Scope>() { SkyNet.Client.Scope.OfflineAccess, SkyNet.Client.Scope.SkyDrive, SkyNet.Client.Scope.SkyDriveUpdate });
                }
                return null;
            }
        }

        public IUserQuota UserQuotas
        {
            get
            {
                var uq = client.Quota();
                return new Implements.UserQuota(uq);
            }
        }

        #endregion

        #region Constructors
        public SkyApi() : this(null)
        {
        }

        public SkyApi(RESTClient.SkyDrive.Implements.Token token)
        {
            if (token != null)
            {
                client = new SkyNet.Client.Client(SkyDriveConfiguration.ClientID, 
                    SkyDriveConfiguration.ClientSecret, SkyDriveConfiguration.ReturnURL, 
                    token.Access_Token, token.Refresh_Token);
                
                this.Token = new UserToken() {
                    Access_Token=token.Access_Token,
                    Refresh_Token = token.Refresh_Token,
                    Expires_In = token.Expires_In,
                    Scope = token.Scope,
                    Token_Type = token.Token_Type};
            }
            else
            {
                client = new SkyNet.Client.Client(SkyDriveConfiguration.ClientID, 
                    SkyDriveConfiguration.ClientSecret, SkyDriveConfiguration.ReturnURL);
            }

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

            if (client != null)
            {
                this.Token = client.GetAccessToken(AccessToken);
            }

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

        public bool RefreshToken()
        {
            if (client != null)
            {
                Token = client.RefreshAccessToken();
                if (Token != null && Token.Access_Token != null)
                    return true;
            }

            return false;
        }

        //public void Test()
        //{
        //    var fInfo = GetElement("AllShare Play/AntekPhotosPublic");///DSC_2115.jpg");
        //    var f = client.Get<Album>(fInfo.Id);
        //    //var ff = f as SkyNet.Model.Photo;
        //}

        #region Do testow z dllki
        //public SkyNet.Model.File Get(string id = null)
        //{
        //    var _requestGenerator = new SkyNet.Client.RequestGenerator();
        //    return ExecuteContentRequest<Photo>(_requestGenerator.Get(id));
        //}

        //private T ExecuteContentRequest<T>(IRestRequest restRequest) where T : new()
        //{
        //    var _restContentClient = new RestClient(@"https://apis.live.net/v5.0/");
        //    _restContentClient.ClearHandlers();
        //    _restContentClient.AddHandler("*", new JsonDeserializer());
        //    _restContentClient.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token.Access_Token, "Bearer");
        //    var restResponse = _restContentClient.Execute<T>(restRequest);
        //    return restResponse.Data;
        //}
        //public void GetAccessToken(string authorizationCode)
        //{
        //    var _requestGenerator = new SkyNet.Client.RequestGenerator();
        //    var getAccessToken = _requestGenerator.GetAccessToken(SkyDriveConfiguration.ClientID,
        //            SkyDriveConfiguration.ClientSecret, SkyDriveConfiguration.ReturnURL, authorizationCode);
        //    var token = ExecuteAuthorizationRequest<UserToken>(getAccessToken);

        //    //return token;
        //}

        //public void RefreshToken()
        //{
        //    var _requestGenerator = new SkyNet.Client.RequestGenerator();
        //    var refreshAccessToken = _requestGenerator.RefreshAccessToken(SkyDriveConfiguration.ClientID,
        //            SkyDriveConfiguration.ClientSecret, SkyDriveConfiguration.ReturnURL, Token.Refresh_Token);
        //    var token = ExecuteAuthorizationRequest<UserToken>(refreshAccessToken);
        //    //SetUserToken(token);
        //}

        //private T ExecuteAuthorizationRequest<T>(IRestRequest restRequest) where T : new()
        //{
        //    var _restAuthorizationClient = new RestClient(@"https://login.live.com");
        //    _restAuthorizationClient.ClearHandlers();
        //    _restAuthorizationClient.AddHandler("*", new JsonDeserializer());
        //    return ExecuteRequest<T>(_restAuthorizationClient, restRequest);
        //}

        //private static T ExecuteRequest<T>(RestClient restContentClient, IRestRequest restRequest) where T : new()
        //{
        //    var res = restContentClient.Execute<T>(restRequest);
        //    return res.Data;
        //}

        //private void SetUserToken(UserToken token)
        //{
        //    string _refreshToken = token.Refresh_Token;
        //    //_restContentClient.Authenticator = new AccessTokenAuthenticator(token.Access_Token);
        //} 
        #endregion

        /// <summary>
        /// Downloads a specified file. 
        /// </summary>
        /// <param name="FileID">The ID of the file you want to retrieve.</param>
        /// <param name="length">The length of the file to retrieve. </param>
        /// <returns>File in bytes array</returns>
        public byte[] DownloadFile(string FileID, long length)
        {
            return client.Read(FileID, 0, length);
        }

        /// <summary>
        /// Finds the file in the Api and downloads it. 
        /// </summary>
        /// <param name="FilePath">The path of the file you want to retrieve.</param>
        /// <returns>File in bytes array</returns>
        public byte[] DownloadFile(string FilePath)
        {
            var file = GetElement(FilePath);
            return client.Read(file.Id, 0, file.Size);
        }

        /// <summary>
        /// Finds the file in the Api, downloads it and saves in the given location. 
        /// </summary>
        /// <param name="FilePath">The path of the file you want to retrieve.</param>
        /// <param name="LocalLocation">Path to the folder where file should be saved. If also a file name will be given file will be saved under that filename</param>
        /// <param name="CreatePath">Default: True. If true it will create the full path for a local file if it not exists. If false Exception will be thrown(DirectoryNotFoundException).</param>
        /// <param name="overwrite">Default: False. If true it iwll overwrite the local file if it exists. If false it will throw FileAlreadyEzxists Exception</param>
        /// <returns>File in bytes array</returns>
        public void DownloadAndSaveFile(string FilePath, string LocalLocation, bool CreatePath=true, bool overwrite = false)
        {
            var folderName = "";
            var fileName = "";

            if (LocalLocation[LocalLocation.Length - 4] == '.')
            {
                var fileInfo = new System.IO.FileInfo(LocalLocation);
                folderName = fileInfo.DirectoryName;
                fileName = fileInfo.Name;
            }
            else
            {
                folderName = LocalLocation;
            }

            if (!System.IO.Directory.Exists(folderName) && !CreatePath)
            {
                throw new System.IO.DirectoryNotFoundException(folderName + " not found");
            }
            else if(!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }

            var file = GetElement(FilePath);

            if (fileName == "")
            {
                fileName = file.Name;
            }

            if (System.IO.File.Exists(fileName) && !overwrite)
            {
                throw new System.IO.InvalidDataException("File already exists and the option \"Overwrite\" is disabled");
            }

            var bytes = client.Read(file.Id, 0, file.Size);

            System.IO.File.WriteAllBytes(folderName + "\\" + fileName, bytes);
        }

        /// <summary>
        /// Finds the file in the Api, downloads it and saves in the given location. 
        /// </summary>
        /// <param name="FilePath">The path of the file you want to retrieve.</param>
        /// <param name="LocalLocation">Path to the folder where file should be saved. If also a file name will be given file will be saved under that filename</param>
        /// <param name="CreatePath">Default: True. If true it will create the full path for a local file if it not exists. If false Exception will be thrown(DirectoryNotFoundException).</param>
        /// <param name="overwrite">Default: False. If true it iwll overwrite the local file if it exists. If false it will throw FileAlreadyEzxists Exception</param>
        /// <returns>File in bytes array</returns>
        public Task DownloadAndSaveFileAsync(string FilePath, string LocalLocation, bool CreatePath = true, bool overwrite = false)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                DownloadAndSaveFile(FilePath, LocalLocation, CreatePath, overwrite);
            });
            return t;

        }

        /// <summary>
        /// Upload a file to the specified folder. 
        /// </summary>
        /// <param name="FolderID">The ID of the folder in which the file should be saved.</param>
        /// <param name="FileName">Name under which the file will be saved in the cloud</param>
        /// <param name="locFilePath">Localization of the file in the local hard drive so it can be opened to read.</param>
        /// <returns>True if uploaded, false if there was a problem</returns>
        public bool UploadFileR(string FolderID, string FileName, string locFilePath)
        {
            bool result = false;

            if (System.IO.File.Exists(locFilePath))
            {
                byte[] content = System.IO.File.ReadAllBytes(locFilePath);
                File f = client.Write(FolderID, content, FileName, "text/plain");
                if (f != null && f.Parent_Id == FolderID)
                    result = true;
            }
            else
            {
                throw new System.IO.FileNotFoundException("Cannot find file locally", locFilePath);
            }

            return result;
        }

        /// <summary>
        /// Upload a file to the specified folder. Folder is being searched in the cloud first. 
        /// </summary>
        /// <param name="FolderPath">The path to the folder in which the file should be saved.</param>
        /// <param name="FileName">Name under which the file will be saved in the cloud</param>
        /// <param name="locFilePath">Localization of the file in the local hard drive so it can be opened to read.</param>
        /// <returns>True if uploaded, false if there was a problem</returns>
        public bool UploadFile(string FolderPath, string FileName, string locFilePath)
        {
            var folder = GetElement(FolderPath);
            return UploadFileR(folder.Id, FileName, locFilePath);
        }

        public Task<bool> UploadFileAsync(string FolderPath, string FileName, string locFilePath)
        {
            Task<bool> t = Task.Factory.StartNew(() =>
            {
                return UploadFile(FolderPath, FileName, locFilePath);
            });
            return t;
        }

        public Task<byte[]> DownloadFileAsync(string FileID, long length)
        {
            Task<byte[]> t = Task.Factory.StartNew(() =>
            {
                return DownloadFile(FileID,length);
            });
            return t;
        }

        public Task<byte[]> DownloadFileAsync(string FilePath)
        {
            Task<byte[]> t = Task.Factory.StartNew(() =>
            {
                return DownloadFile(FilePath);
            });
            return t;
        }
        
        /// <summary>
        /// Retrieves informations about the element in the given Path
        /// </summary>
        /// <param name="FilePath">Path of the Element</param>
        /// <returns>IFile object that represents data about the elements</returns>
        public IFile GetElement(string FilePath)
        {
            SkyDrive.Implements.File result = null;
            if (client != null)
            {
                string[] PathElements = null;
                try
                {
                    PathElements = FilePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    throw new System.IO.DirectoryNotFoundException("Path was not found. Check if path is in the correct format like: \"/Folder1/Folder2/File\" or \"/Folder1/Folder2\"",
                        ex);
                }

                if (PathElements == null || PathElements.Length == 0)
                {
                    throw new System.IO.DirectoryNotFoundException("Path was not found. Check if path is in the correct format like: \"/Folder1/Folder2/File\" or \"/Folder1/Folder2\"");
                }

                File f = FindElement<File>(PathElements[0], "");
                for (int i = 1; i < PathElements.Length; i++)
                {
                    f = FindElement<File>(PathElements[i], f.Id);
                }
                result = new Implements.File(f);
            }
            return result;
        }

        /// <summary>
        /// Retrieves informations about the element in the given Path and given type
        /// </summary>
        /// <typeparam name="T">Must be inherited type from ISkyDriveElement. That is all interfaces of the model in SkyDrive api like IFile, IFolder, IPhoto or IAlbum</typeparam>
        /// <param name="FilePath">Path of the Element</param>
        /// <returns>Object that represents data about the element in the given type</returns>
        public T GetElement<T>(string FilePath) where T : SkyDrive.ISkyDriveElement
        {
            SkyDrive.ISkyDriveElement result = null;

            if (client != null)
            {
                string[] PathElements = null;
                try
                {
                    PathElements = FilePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    throw new System.IO.DirectoryNotFoundException("Path was not found. Check if path is in the correct format like: \"/Folder1/Folder2/File\" or \"/Folder1/Folder2\"",
                        ex);
                }

                if (PathElements == null || PathElements.Length == 0)
                {
                    throw new System.IO.DirectoryNotFoundException("Path was not found. Check if path is in the correct format like: \"/Folder1/Folder2/File\" or \"/Folder1/Folder2\"");
                }

                Dictionary<Type, Type> types = InitializeDictionary();
                Dictionary<Type, Type> innerTypes = InitializeInnerDictionary();

                MethodInfo mFindElement = this.GetType().GetMethod("FindElement", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo genericMethod = mFindElement.MakeGenericMethod(new Type[] { types[typeof(T)] });
                var f = genericMethod.Invoke(this, new object[] { PathElements[0], "" });
                for (int i = 1; i < PathElements.Length; i++)
                {
                    f = genericMethod.Invoke(this, new object[] { PathElements[i], ((File) f).Id });
                }
                result = (T)innerTypes[typeof(T)].GetConstructor(new Type[] { types[typeof(T)] }).Invoke(new object[] { f });
                
            }

            return (T)result;
        }

        /// <summary>
        /// Gets the root folder
        /// </summary>
        public List<IFile> Root
        {
            get
            {
                List<IFile> result = new List<IFile>();
                var cloudContent = client.GetContents("");
                foreach (var item in cloudContent)
                {
                    result.Add(new Implements.File(item));
                }
                return result;
            }
        }

        /// <summary>
        /// Copies a file from source path to target folder
        /// </summary>
        /// <param name="srcFilePath">Full path with file name of the file that should be copied</param>
        /// <param name="trgFolderPath">Full path to the folder (no file name) to the place where file should be copied</param>
        /// <returns>IFile - as the information of the copied file</returns>
        public IFile Copy(string srcFilePath, string trgFolderPath)
        {
            if (client != null)
            {
                IFile iFolder = null;
                var ifile = GetElement(srcFilePath);
                if (trgFolderPath == "" || trgFolderPath == "/")
                {
                    iFolder = new Implements.Folder(client.Get<Folder>(""));
                }
                else
                {
                    iFolder = GetElement(trgFolderPath);
                }
                if (ifile != null && iFolder != null)
                {
                    var file = client.Copy(ifile.Id, iFolder.Id);
                    return new Implements.File(file);
                }
            }
            return null;
        }

        /// <summary>
        /// Copies a file from source path to target folder
        /// </summary>
        /// <param name="srcFileId">ID of the file which should be copied</param>
        /// <param name="trgFolderId">ID of the target folder.</param>
        /// <returns></returns>
        public IFile CopyByID(string srcFileId, string trgFolderId)
        {
            if (client != null)
            {
                var file = client.Copy(srcFileId, trgFolderId);
                if(file != null)
                    return new Implements.File(file);
            }
            return null;
        }

        /// <summary>
        /// Create folder with given name and description under given parent folder
        /// </summary>
        /// <param name="ParentFolderPath">Full path to the folder in which new folder should be created</param>
        /// <param name="Name">Name of the new folder</param>
        /// <param name="Description">Description of the new folder</param>
        /// <returns></returns>
        public IFolder CreateFolder(string ParentFolderPath, string Name, string Description = null)
        {
            if (client != null)
            {
                if (ParentFolderPath == "" || ParentFolderPath == "/")
                {
                    var iFolder = client.Get<Folder>("");
                    var folder = client.CreateFolder(iFolder.Id, Name, Description);
                    return new Implements.Folder(folder);
                }
                else
                {
                    var iFolder = GetElement<IFolder>(ParentFolderPath);
                    var folder = client.CreateFolder(iFolder.Id, Name, Description);
                    return new Implements.Folder(folder);
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes element speciefied by its ID
        /// </summary>
        /// <param name="ElementID">ID of the element to be deleted</param>
        /// <returns>False if connection is bad established, else true</returns>
        public bool DeleteElementByID(string ElementID)
        {
            if (client != null)
            {
                client.Delete(ElementID);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes element under the path given
        /// </summary>
        /// <param name="ElementPath">Path to the element</param>
        /// <returns>False if connection is bad established, else true</returns>
        public bool DeleteElement(string ElementPath)
        {
            var element = GetElement(ElementPath);
            return DeleteElementByID(element.Id);
        }

        /// <summary>
        /// Moves an element pointed by ID tinto element pointed by second ID
        /// </summary>
        /// <param name="srcId">ID of the element to be moved</param>
        /// <param name="trgId">ID of the new parent element for the element being moved</param>
        /// <returns></returns>
        public bool MoveByID(string srcId, string trgId)
        {
            if (client != null)
            {
                client.Move(srcId, trgId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves an element given in path into another element
        /// </summary>
        /// <param name="srcPath">Full path to the element that must be moved</param>
        /// <param name="trgPath">Full path to the element that is new parent for the element being moved</param>
        /// <returns></returns>
        public bool Move(string srcPath, string trgPath)
        {
            var iFile = GetElement(srcPath);
            IFile iFolder = null;
            if (trgPath == "" || trgPath == "/")
            {
                iFolder = new Implements.Folder(client.Get<Folder>(""));
            }
            else
            {
                iFolder = GetElement(trgPath);
            }

            if (iFile != null && iFolder != null)
            {
                return MoveByID(iFile.Id, iFolder.Id);
            }
            return false;
        }

        /// <summary>
        /// Moves a file to another folder
        /// </summary>
        /// <param name="srcPath">Full path to the file with its name</param>
        /// <param name="trgPath">Full path to the target folder</param>
        /// <returns>IFile as the information of the moved file</returns>
        public IFile MoveFile(string srcPath, string trgPath)
        {
            var iFile = GetElement<IFile>(srcPath);
            IFile iFolder = null;
            if (trgPath == "" || trgPath == "/")
            {
                iFolder = new Implements.Folder(client.Get<Folder>(""));
            }
            else
            {
                iFolder = GetElement<IFile>(trgPath);
            }

            if (iFile != null && iFolder != null)
            {
                var file = client.MoveFile(iFile.Id, iFolder.Id);
                return new Implements.File(file);
            }
            return null;
        }

        /// <summary>
        /// Moves a folder to another folder
        /// </summary>
        /// <param name="srcPath">Full path to the folder</param>
        /// <param name="trgPath">Full path to the target folder</param>
        /// <returns>IFolder as the information of the moved folder</returns>
        public IFolder MoveFolder(string srcPath, string trgPath)
        {
            var iFile = GetElement<IFile>(srcPath);
            IFile iFolder = null;
            if (trgPath == "" || trgPath == "/")
            {
                iFolder = new Implements.Folder(client.Get<Folder>(""));
            }
            else
            {
                iFolder = GetElement<IFile>(trgPath);
            }

            if (iFile != null && iFolder != null)
            {
                var file = client.MoveFolder(iFile.Id, iFolder.Id);
                return new Implements.Folder(file);
            }
            return null;
        }

        /// <summary>
        /// Renames a given element
        /// </summary>
        /// <param name="elementID">Element ID</param>
        /// <param name="newName">New name for an element</param>
        /// <returns></returns>
        public bool RenameElement(string elementID, string newName)
        {
            if (client != null)
            {
                client.Rename(elementID, newName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Renames a file
        /// </summary>
        /// <param name="filePath">Full path to the file with its current name</param>
        /// <param name="newName">New file name</param>
        /// <returns></returns>
        public IFile RenameFile(string filePath, string newName)
        {
            var ifile = GetElement<IFile>(filePath);
            if (ifile != null)
            {
                var file = client.RenameFile(ifile.Id, newName);
                return new Implements.File(file);
            }
            return null;
        }

        /// <summary>
        /// Renames given folder
        /// </summary>
        /// <param name="folderPath">Path to the folder</param>
        /// <param name="newName">New name of the folder</param>
        /// <returns></returns>
        public IFolder RenameFolder(string folderPath, string newName)
        {
            var ifile = GetElement<IFolder>(folderPath);
            if (ifile != null)
            {
                var file = client.RenameFolder(ifile.Id, newName);
                return new Implements.Folder(file);
            }
            return null;
        }

        #endregion

        #region Private Methods
        T FindElement<T>(string Name, string ParentID) where T : new()
        {
            T result = default(T);

            foreach (var item in client.GetContents(ParentID))
            {
                if (item.Name.ToLower() == Name.ToLower())
                {
                    if (typeof(T).Equals(typeof(SkyNet.Model.File)))
                    {
                        result = (T)Convert.ChangeType(item, typeof(T));
                    }
                    else
                    {
                        result = client.Get<T>(item.Id);
                    }
                    break;
                }
            }

            return result;
        }

        Dictionary<Type, Type> InitializeDictionary()
        {
            return new Dictionary<Type, Type>()
            {
                {typeof(IAlbum), typeof(SkyNet.Model.Album)},
                {typeof(IFile), typeof(SkyNet.Model.File)},
                {typeof(IFolder), typeof(SkyNet.Model.Folder)},
                {typeof(IAudio), typeof(SkyNet.Model.Audio)},
                {typeof(IImage), typeof(SkyNet.Model.Image)},
                {typeof(IFrom), typeof(SkyNet.Model.From)},
                {typeof(ILocation), typeof(SkyNet.Model.Location)},
                {typeof(IPhoto), typeof(SkyNet.Model.Photo)},
                {typeof(ISharedWith), typeof(SkyNet.Model.SharedWith)},
                {typeof(IUserQuota), typeof(SkyNet.Model.UserQuota)},
                {typeof(IVideo), typeof(SkyNet.Model.Video)}
            };
        }

        Dictionary<Type, Type> InitializeInnerDictionary()
        {
            return new Dictionary<Type, Type>()
            {
                {typeof(IAlbum), typeof(Implements.Album)},
                {typeof(IFile), typeof(Implements.File)},
                {typeof(IFolder), typeof(Implements.Folder)},
                {typeof(IAudio), typeof(Implements.Audio)},
                {typeof(IImage), typeof(Implements.Image)},
                {typeof(IFrom), typeof(Implements.From)},
                {typeof(ILocation), typeof(Implements.Location)},
                {typeof(IPhoto), typeof(Implements.Photo)},
                {typeof(ISharedWith), typeof(Implements.SharedWith)},
                {typeof(IUserQuota), typeof(Implements.UserQuota)},
                {typeof(IVideo), typeof(Implements.Video)}
            };
        } 
        #endregion
    }
}
