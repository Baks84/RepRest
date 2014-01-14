using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
{
    /// <summary>
    /// Class representing result of chunk file upload
    /// Implements IChunkUploadResult
    /// </summary>
    internal class ChunkedUploadResult : IChunkUploadResult
    {
        /// <summary>
        /// Upload ID.
        /// If empty new chunk ID will be given. 
        /// </summary>
        public string Upload_ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Number of bytes correctly received by DropBOX server
        /// </summary>
        public long Offset
        {
            get;
            protected set;
        }

        /// <summary>
        /// Date of expiration of the Upload_ID.
        /// Normally it is 24H from last chunk correctly uploaded.
        /// </summary>
        public DateTime Expires
        {
            get;
            protected set;
        }
    }
}
