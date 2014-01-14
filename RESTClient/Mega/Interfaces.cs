using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RESTClient.Mega
{
    public interface IUser
    {
        // used once to decrypt the masterkey
        byte[] PassKey { get; set; }
        string Sid { get; set; }
        string NodeSid { get; set; }
        // rsa
        byte[] PrivateKey { get; set; }
        byte[] PublicKey { get; set; }

        int Status { get; set; }
        string Email { get; set; }
        string Id { get; set; }
    }

    public interface IUserQuota
    {
        /// <summary>
        /// The user's used storage space, in bytes.
        /// </summary>
        long Used { get; set; }

        /// <summary>
        /// The user's total available storage space, in bytes
        /// </summary>
        long Quota { get; set; }
    }

    public interface INode
    {
         int Type { get; set; }
         string Id { get; set; }
         string ParentId { get; set; }
         byte[] encryptedAttributes { get; set; }
         INodeKeys NodeKeys { get; set; }
         string UserId { get; set; }
         long? Size { get; set; }
         DateTime? Timestamp { get; set; }
         INodeAttributes NodeAttributes { get; set; }
    }

    public interface INodeKeys
    {
         Dictionary<string, byte[]> Keys { get; set; }
         byte[] DecryptedKey { get; set; }
         byte[] EncryptedKey { get; set; }
    }

    public interface INodeAttributes
    {
         string Name { get; set; }
    }
}
