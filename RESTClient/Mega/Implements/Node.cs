using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Mega.Implements
{
    internal class Node : MegaApi.MegaNode, INode
    {
        public Node() : base() { }

        public Node(MegaApi.MegaNode mn):base()
        {
            this.Attributes = mn.Attributes;
            this.encryptedAttributes = mn.encryptedAttributes;
            this.Id = mn.Id;
            this.NodeKey = mn.NodeKey;
            this.ParentId = mn.ParentId;
            this.Size = mn.Size;
            this.Timestamp = mn.Timestamp;
            this.Type = mn.Type;
            this.UserId = mn.UserId;
        }

        public MegaApi.MegaNode toMega()
        {
            return ((MegaApi.MegaNode)this);
        }

        public INodeKeys NodeKeys
        {
            get
            {
                return new NodeKeys(this.NodeKey);
            }
            set
            {
                this.NodeKey = ((NodeKeys)value).toMega();
            }
        }

        public INodeAttributes NodeAttributes
        {
            get
            {
                return new NodeAttributes(this.Attributes);
            }
            set
            {
                this.Attributes = ((NodeAttributes)value).toMega();
            }
        }
    }

    internal class NodeKeys : MegaApi.DataTypes.NodeKeys, INodeKeys
    {
        public NodeKeys():base() { }

        public NodeKeys(MegaApi.DataTypes.NodeKeys nk)
        {
            this.DecryptedKey = nk.DecryptedKey;
            this.EncryptedKey = nk.EncryptedKey;
            this.Keys = nk.Keys;
        }

        public MegaApi.DataTypes.NodeKeys toMega()
        {
            return new MegaApi.DataTypes.NodeKeys()
            {
                DecryptedKey = this.DecryptedKey,
                EncryptedKey = this.EncryptedKey,
                Keys = this.Keys
            };
        }
    }

    internal class NodeAttributes : INodeAttributes
    {
        public string Name { get; set; }

        public NodeAttributes()
        {
        }

        public NodeAttributes(MegaApi.DataTypes.NodeAttributes na)
        {
            this.Name = na.Name;
        }

        public MegaApi.DataTypes.NodeAttributes toMega()
        {
            return new MegaApi.DataTypes.NodeAttributes()
            {
                Name = this.Name
            };
        }
    }
}
