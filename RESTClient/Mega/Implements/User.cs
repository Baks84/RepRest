using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Mega.Implements
{
    internal class User: MegaApi.MegaUser, IUser
    {
        public User() : base() { }
        public User(byte[] userId, byte[] passKey) : base(userId, passKey) { }
        public User(string email, string password) : base(email, password) { }

        public User(MegaApi.MegaUser mu)
        {
            this.Email = mu.Email;
            this.Id = mu.Id;
            this.NodeSid = mu.NodeSid;
            this.PassKey = mu.PassKey;
            this.PrivateKey = mu.PrivateKey;
            this.PublicKey = mu.PublicKey;
            this.Sid = mu.Sid;
            this.Status = mu.Status;
        }

        public MegaApi.MegaUser toMega()
        {
            return ((MegaApi.MegaUser)this);
        }
    }
}
