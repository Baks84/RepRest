using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RESTClient
{
    public class API
    {
        #region Constructors
        public API()
            : this(String.Empty, String.Empty, String.Empty)
        { }

        public API(string baseURL)
            : this(baseURL, String.Empty, String.Empty)
        { }

        public API(string baseURL, string accountSid, string secretKey)
        {
            BaseUrl = baseURL;
            AccountSid = accountSid;
            SecretKey = secretKey;
        } 
        #endregion

        #region Public Properties
        public string BaseUrl { get; set; }
        public string AccountSid { get; set; }
        public string SecretKey { get; set; } 
        #endregion

        #region Methods

        public virtual RestResponse ExecutePlain(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return (RestResponse)response;
        } 

        public virtual T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;

            var response = client.Execute<T>(request);
            
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }

        public virtual IRestResponse Execute(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response;
        } 

        //public T Execute<T>(RestRequest request) where T : new()
        //{
        //    var client = new RestClient();
        //    client.BaseUrl = BaseUrl;

        //    client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator("6bpoL7QLil8AAAAAAAAAAc8GtVQkhSuq7CRa2ERZe4EEJV3MxB4NzsFo9uykKWg5", "Bearer");

        //    var response = client.Execute<T>(request);

        //    if (response.ErrorException != null)
        //    {
        //        const string message = "Error retrieving response.  Check inner details for more info.";
        //        var twilioException = new ApplicationException(message, response.ErrorException);
        //        throw twilioException;
        //    }
        //    return response.Data;
        //}

        //public void Test()
        //{
        //    RestRequest rr = new RestRequest("oauth2/token", Method.POST);
        //    BaseUrl = "https://api.dropbox.com/1/";
        //    string code = "vfApyXgsg1cAAAAAAAAAAe0GzXcmZUuKf9IS8e_IRRU";
        //    rr.RequestFormat = DataFormat.Json;
        //    rr.AddHeader("Content-Type", "application/x-www-form-urlencoded"); 
        //    rr.AddParameter("code", code);
        //    rr.AddParameter("grant_type", "authorization_code");
        //    rr.AddParameter("client_id", "9pzhjelm0hfzxqx");
        //    rr.AddParameter("client_secret", "j3wrudtrihz4qqg");
        //    var a = ExecutePlain<RestResponse>(rr);
        //}

        //public void Test2()
        //{
        //    RestRequest rr = new RestRequest("account/info", Method.GET);
        //    BaseUrl = "https://api.dropbox.com/1/";
        //    rr.RequestFormat = DataFormat.Json;
        //    //rr.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //    rr.AddParameter("locale", "en-EN");
        //    var a = Execute<RestResponse>(rr);
        //}

        //public void Test3()
        //{
        //    RestRequest rr = new RestRequest("delta", Method.POST);
        //    BaseUrl = "https://api.dropbox.com/1/";
        //    rr.RequestFormat = DataFormat.Json;
        //    //rr.AddParameter("cursor", "myCurs");
        //    rr.AddParameter("locale", "en-EN");
        //    var a = Execute<RestResponse>(rr);
        //}
        #endregion
    }
}
