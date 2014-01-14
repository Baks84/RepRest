using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
{
    /// <summary>
    /// Class represents the Delta information from Dropbox server
    /// </summary>
    internal class Delta : IDelta
    {
        #region Public Properties
        /// <summary>
        /// Each delta entry is a 2-item list of one of the following forms:
        /// [<path>, <metadata>] - Indicates that there is a file/folder at the given path. You should add the entry to your local path. The metadata value is the same as what would be returned by the /metadata call, except folder metadata doesn't have hash or contents fields. To correctly process delta entries:
        /// If the new entry includes parent folders that don't yet exist in your local state, create those parent folders in your local state.
        /// If the new entry is a file, replace whatever your local state has at path with the new entry.
        /// If the new entry is a folder, check what your local state has at <path>. If it's a file, replace it with the new entry. If it's a folder, apply the new <metadata> to the folder, but don't modify the folder's children.
        /// [<path>, null] - Indicates that there is no file/folder at the given path. To update your local state to match, anything at path and all its children should be deleted. Deleting a folder in your Dropbox will sometimes send down a single deleted entry for that folder, and sometimes separate entries for the folder and all child paths. If your local state doesn't have anything at path, ignore this entry.
        /// </summary>
        public Dictionary<string,IFileInfo> Entries
        {
            get;
            private set;
        }

        /// <summary>
        /// Elements needed to perform correct deserialization for entries
        /// </summary>
        public List<object> entries
        {
            get;
            set;
        }

        /// <summary>
        /// For deserialization purposes
        /// </summary>
        public bool reset { get;  set; }

        /// <summary>
        /// For deserialization purposes
        /// </summary>
        public string cursor { get;  set; }

        /// <summary>
        /// For deserialization purposes
        /// </summary>
        public bool has_more { get;  set; }

        /// <summary>
        /// Deserialize the JSON recieved from Dropbox
        /// </summary>
        public void Deserilize()
        {
            // Create new dictionary
            Entries = new Dictionary<string, IFileInfo>();

            if (entries != null)
            {
                foreach (var item in entries)
                {
                    //First deserialize all elements to ICollection
                    var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<object>>(item.ToString());

                    //Now get the value for our Dictionary
                    var fileInfo = temp.ElementAt(1) == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<RESTClient.Dropbox.Implements.FileInfo>
                    (temp.ElementAt(1).ToString());

                    //Add key and value to our dictionary
                    Entries.Add(temp.ElementAt(0).ToString(), fileInfo);
                }
            }
        }

        /// <summary>
        /// If true, clear your local state before processing the delta entries. 
        /// reset is always true on the initial call to /delta (i.e. when no cursor is passed in). 
        /// Otherwise, it is true in rare situations, such as after server or account maintenance, or if a user deletes their app folder.
        /// </summary>
        public bool Reset
        {
            get { return this.reset; }
        }

        /// <summary>
        ///  A string that encodes the latest information that has been returned. On the next call to /delta, pass in this value.
        /// </summary>
        public string Cursor
        {
            get { return this.cursor; }
        }

        /// <summary>
        /// If true, then there are more entries available; you can call /delta again immediately to retrieve those entries. 
        /// If 'false', then wait for at least five minutes (preferably longer) before checking again.
        /// </summary>
        public bool Has_More
        {
            get { return this.has_more; }
        }
        #endregion
    }

    /// <summary>
    /// Represents the result of Longpol Delta.
    /// </summary>
    internal class LongpollDeltaResult : ILongpollDeltaResult
    {

        #region Public Properites
        /// <summary>
        /// The value of the changes field indicates whether new changes are available. 
        /// If this value is true, you should call /delta to retrieve the changes. 
        /// If this value is false, it means the call to /longpoll_delta timed out.
        /// </summary>
        public bool Changes
        {
            get;
            protected set;
        }

        /// <summary>
        /// If present, the value of the backoff field indicates how many seconds your code should wait before calling /longpoll_delta again.
        /// </summary>
        public int Backoff
        {
            get;
            protected set;
        } 
        #endregion
    }
}
