using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire
{
    internal class MediaFireConfiguration 
    {
        //static public String sOAuth2UserRegistrationBaseURL { get { return "https://www.dropbox.com/1/oauth2/authorize"; } }
        static public String sOAuth2UserRegistrationResponseType { get { return "code"; } }
        static public String ApplicationCode { get { return "39103"; } }
        static public String ApplicationSecret { get { return "99c4245amdkt1wv2y9rr81dwt6wn9k24t7962bw3"; } }

        static public String APIBaseURL { get { return "https://www.mediafire.com/api/"; } }
        static public String BaseURL { get { return "https://www.mediafire.com/"; } }
        static public String GetSessionToken { get { return "user/get_session_token.php"; } }
        static public String RenewSessionToken { get { return "user/renew_session_token.php"; } }
        static public String GetUserInfo { get { return "user/get_info.php"; } }
        static public String GetRoot { get { return "folder/get_content.php"; } }
        static public String GetFolderInfo { get { return "folder/get_info.php"; } }
        static public String GetFolderContent { get { return "folder/get_content.php"; } }
        static public String GetFolderRevision { get { return "folder/get_revision.php"; } }
        static public String UpdateFolder { get { return "folder/update.php"; } }
        static public String CopyFolder { get { return "folder/copy.php"; } }
        static public String CreateFolder { get { return "folder/create.php"; } }
        static public String DeleteFolder { get { return "folder/delete.php"; } }
        static public String PurgeFolder { get { return "folder/purge.php"; } }
        static public String MoveFolder { get { return "folder/move.php"; } }

        static public String CopyFile { get { return "file/copy.php"; } }
        static public String CreateFile { get { return "file/create.php"; } }
        static public String CreateFileSnapshot { get { return "file/create_snapshot.php"; } }
        static public String DeleteFile { get { return "file/delete.php"; } }
        static public String PurgeFile { get { return "file/purge.php"; } }
        static public String GetFileLinks { get { return "file/get_links.php"; } }
        static public String GetFileVersions { get { return "file/get_versions.php"; } }
        static public String GetFileInfo { get { return "file/get_info.php"; } }
        static public String MoveFile { get { return "file/move.php"; } }
        static public String OneTimeDownload { get { return "file/one_time_download.php"; } }
        static public String LastModifiedFiles { get { return "file/recently_modified.php"; } }
        static public String RestoreFile { get { return "file/restore.php"; } }
        static public String UpdateFile { get { return "file/update.php"; } }
        static public String GetZip { get { return "file/zip.php"; } }

        static public String WebUpload { get { return "upload/add_web_upload.php"; } }
        static public String GetWebUpload { get { return "upload/get_web_uploads.php"; } }
        static public String PreUpload { get { return "upload/pre_upload.php"; } }
        static public String Upload { get { return "douploadtoapi/?type=basic"; } }
    }
}
