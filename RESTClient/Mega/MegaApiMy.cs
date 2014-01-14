using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTClient.Mega
{
    public class MEGAApi
    {
        MegaApi.Mega client;
        MegaApi.MegaUser user;
        int ErrorNb = -1;

        public void test()
        {
            //var a = client.DownloadFileSync(
            //bool wait = true;
            //MegaApi.Mega.Init(new MegaApi.MegaUser("baks84@g.pl", "Aniolek69"), OnSuccess, OnFailed);
            while (true)
            {
                System.Threading.Thread.Sleep(500);
            }
            
        }

        void TestSuc(long max, long curr)
        {

        }

        void TestFaile(int err)
        {

        }

        #region Constructors
        public MEGAApi(byte[] loginData)
        {
            if (loginData == null || loginData.Length == 0)
            {
                throw new System.IO.InvalidDataException("loginData can not be empty");
            }

            user = MegaApi.Mega.LoadAccount(loginData);
            Init();
        }

        public MEGAApi(string UserEmail, string UserPassword)
        {
            if (String.IsNullOrEmpty(UserEmail) || String.IsNullOrEmpty(UserPassword))
            {
                throw new System.IO.InvalidDataException("UserEmail and UserPassword can not be empty");
            }
            else
            {
                user = new MegaApi.MegaUser(UserEmail, UserPassword);
            }
            Init();
        }

        private void Init()
        {
            MegaApi.Mega.Init(user, OnInitSuccess, OnInitFailed);
            ErrorNb = -1;
            while (ErrorNb < 0)
            {
                System.Threading.Thread.Sleep(100);
            }
            if (ErrorNb > 0)
                throw new Exception("Login Failed");
        }

        private void OnInitSuccess(MegaApi.Mega m)
        {
            client = m;
            ErrorNb = 0;
        }

        private void OnInitFailed(int err)
        {
            ErrorNb = 1;
        }

        #endregion

        #region Public Properties

        public IUserQuota UserQuotas
        {
            get
            {
                var uq = client.GetQuotaSync();
                return new Implements.UserQuota(uq);
            }
        }

        public IUser User
        {
            get 
            {
                return new Implements.User(this.user);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retireves account informations in bytes so it can be saved and used in later time
        /// </summary>
        /// <returns>Byte array</returns>
        public byte[] GetAccountData()
        {
            return client.SaveAccountData();
        }

        /// <summary>
        /// Saves account informations into the file
        /// </summary>
        /// <param name="FilePath">Path and filename in which the account information should be saved</param>
        public void SaveAccountData(string FilePath)
        {
            client.SaveAccount(FilePath);
        }

        /// <summary>
        /// Retrieves all nodes (representaticve element of the files and folders).
        /// </summary>
        /// <returns>Task that will contain the list of Nodes when finished</returns>
        public Task<List<INode>> GetNodesAsync()
        {
            Task<List<INode>> t = Task.Factory.StartNew<List<INode>>(() =>
            {
                return GetNodes();
            });
            return t;
        }

        /// <summary>
        /// Retrieves all nodes (representaticve element of the files and folders).
        /// </summary>
        /// <returns>List of Nodes</returns>
        public List<INode> GetNodes()
        {
            List<INode> result = new List<INode>();
            var list = client.GetNodesSync();
            foreach (var item in list)
            {
                result.Add(new Implements.Node(item));
            }
            return result;
        }

        /// <summary>
        /// Upload a file to the cloud
        /// </summary>
        /// <param name="trgNodeId">ID of the folder into which the file should be uploaded</param>
        /// <param name="FileName">Path and name of the file in the local filesystem</param>
        /// <returns></returns>
        public INode UploadFile(string trgNodeId, string FileName)
        {
            var res = client.UploadFileSync(trgNodeId, FileName);
            return new Implements.Node(res);
        }

        public void DownloadFile(INode node, string fileName)
        {
            var nnode = ((Implements.Node)node).toMega();
            client.DownloadFileSync(nnode, fileName);
        }
      
        #endregion
    }
}
