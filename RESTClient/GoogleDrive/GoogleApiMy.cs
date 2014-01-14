using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.GoogleDrive
{
    public class GoogleDriveApi
    {
        static String CLIENT_ID = "111280266192-5g1v79cb80gskv9tsiav093j6rjddh1m.apps.googleusercontent.com";
        static String CLIENT_SECRET = "pJ3UASUypy7qaRKyibH_42CV";
        static String REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        static String[] SCOPES = new String[] {
            "https://www.googleapis.com/auth/drive.file",
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/userinfo.profile"
            };
    }
}
