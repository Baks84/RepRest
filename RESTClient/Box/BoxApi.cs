using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoxApi;
using BoxApi.V2;
using BoxApi.V2.Authentication;
using BoxApi.V2.Authentication.OAuth2;
using BoxApi.V2.Model;

namespace RESTClient.Box
{
    public class BOXApi
    {
        static string CLIENT_ID = "r35exdxdeu1azaudb4zwpsedt6epxy5d";
        static string CLIENT_SECRET = "ce05pScjc7mLwSI4eUqBKvfgYbsF8Dnx";
        static string CLIENT_RETURN_URL = @"https://www.onet.pl/";

        BoxManager client;
        TokenProvider authProvider;

        #region Public Properties
        
        /// <summary>
        /// Bearer Token used for authorisation.
        /// If the token is not known it will be filled after authorizin application with an account.
        /// It should be saved.
        /// Next this field should be filled before making any other call to Dropbox
        /// </summary>
        public BoxApi.V2.Authentication.OAuth2.OAuthToken Token
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
                if(authProvider == null)
                {
                    authProvider = new TokenProvider(CLIENT_ID, CLIENT_SECRET);
                }
                return authProvider.GetAuthorizationUrl(CLIENT_RETURN_URL);
            }
        }

        public IUserManager ApiClient
        {
            get
            {
                return client;
            }
        }
        #endregion

        #region Constructors
        public BOXApi(OAuthToken token)
        {
            this.Token = token;
            client = new BoxManager(Token.AccessToken);
        }

        public BOXApi(string accessToken, string refreshToken) : this(new OAuthToken(){ AccessToken = accessToken, RefreshToken = refreshToken})
        {
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
            if (authProvider == null)
            {
                authProvider = new TokenProvider(CLIENT_ID, CLIENT_SECRET);
            }
            this.Token = authProvider.GetAccessToken(AccessToken, CLIENT_RETURN_URL);

            return true;
        }

        public bool RefreshToken()
        {
            this.Token = authProvider.RefreshAccessToken(this.Token.RefreshToken);
            return true;
        }

        /// <summary>
        /// Retrieves information about the user's account.
        /// </summary>
        /// <returns>User account information in IUserInfo</returns>
        public User GetUserInfo()
        {
            return client.Me();
        }
        #endregion

        //public void Test()
        //{
        //    BoxApi.V2.Authentication.OAuth2.TokenProvider tp = new BoxApi.V2.Authentication.OAuth2.TokenProvider("r35exdxdeu1azaudb4zwpsedt6epxy5d", "ce05pScjc7mLwSI4eUqBKvfgYbsF8Dnx");
        //    string url = tp.GetAuthorizationUrl(@"http://www.onet.pl?");
        //    string tt = "";
        //    var Token = tp.GetAccessToken(tt);
        //    BoxApi.V2.BoxManager mb = new BoxApi.V2.BoxManager(Token.AccessToken, options: BoxApi.V2.Model.Enum.BoxManagerOptions.RetryRequestOnceWhenHttp500Received);
            
        //}
    }
}
