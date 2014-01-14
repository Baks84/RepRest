using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox
{
    internal class DropBoxConfiguration 
    {
        static public String sOAuth2UserRegistrationBaseURL { get { return "https://www.dropbox.com/1/oauth2/authorize"; } }
        static public String sOAuth2UserRegistrationResponseType { get { return "code"; } }
        static public String ApplicationCode { get { return "9pzhjelm0hfzxqx"; } }
        static public String ApplicationSecret { get { return "j3wrudtrihz4qqg"; } }

        static public String APIBaseURL { get { return "https://api.dropbox.com/1/"; } }
        static public String OAuth2Token { get { return "oauth2/token"; } }
        static public String OAuth2GrantType { get { return "authorization_code"; } }
        static public String Delta { get { return "delta"; } }
        static public String Shares { get { return "shares/{root}/{path}"; } }
        static public String Media { get { return "media/{root}/{path}"; } }
        static public String CopyRef { get { return "copy_ref/{root}/{path}"; } }
        static public String FileCopy { get { return "fileops/copy"; } }
        static public String CreateFolder { get { return "fileops/create_folder"; } }
        static public String Delete { get { return "fileops/delete"; } }
        static public String Move { get { return "fileops/move"; } }

        static public String AccountInfo { get { return "account/info"; } }

        static public String APIContentURL { get { return "https://api-content.dropbox.com/1/"; } }
        static public String DownloadFilePath { get { return "files/dropbox/{path}"; } }
        static public String DownloadSandboxFilePath { get { return "files/sandbox/{path}"; } }
        static public String UploadFilePath { get { return "files_put/dropbox/{path}"; } }
        static public String UploadSandboxFilePath { get { return "files_put/sandbox/{path}"; } }
        static public String Metadata { get { return "metadata/dropbox/{path}"; } }
        static public String MetadataRoot { get { return "metadata/dropbox"; } }
        static public String Revisions { get { return "revisions/{root}/{path}"; } }
        static public String Restore { get { return "restore/{root}/{path}"; } }
        static public String Thumbnail { get { return "thumbnails/{root}/{path}"; } }
        static public String ChunkedUpload { get { return "chunked_upload"; } }
        static public String ChunkedUploadCommit { get { return "commit_chunked_upload/{root}/{path}"; } }

        static public String APINotifyURL { get { return "https://api-notify.dropbox.com/1/"; } }
        static public String LongpollDelta { get { return "longpoll_delta"; } }

        static public String OAuth2RegistrationURL 
        {
            get
            {
                return new StringBuilder()
                    .Append(sOAuth2UserRegistrationBaseURL)
                    .Append("?response_type=").Append(sOAuth2UserRegistrationResponseType)
                    .Append("&client_id=").Append(ApplicationCode).ToString();
            }
        }
    }
}
