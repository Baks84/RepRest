using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using MegaApi.Comms.Converters;


namespace MegaApi.Comms.Requests
{
    public class MResponseGetUserQuota : MegaResponse
    {
        [JsonProperty("cstrg")]
        public long storage_used { get; set; }

        // do not remove! used in the getFiles request
        [JsonProperty("mstrg")]
        public long storage_max { get; set; }
    }
    internal class MRequestGetUserQuota<T> : MegaRequest<T> where T : MegaResponse
    {
        [DataMember]
        public string a = "uq";
        [DataMember]
        public int strg = 1;

        public MRequestGetUserQuota(MegaUser user) : base(user) { }
    }
}
