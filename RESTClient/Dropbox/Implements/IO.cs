using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.Dropbox.Implements
{
    /// <summary>
    /// Represents implementation of IStoreDataInfo
    /// </summary>
    internal class StoreDataInfo : IStoreDataInfo
    {
        #region Constructors
        public StoreDataInfo()
        {
        }

        public StoreDataInfo(string _Path, bool? _Is_Deleted, string _Rev, bool _Is_Dir, DateTime _Modified,
                    string _Icon, string _Root, string _Size, long _Bytes, bool _Thumb_exists)
        {
            path = _Path;
            is_deleted = _Is_Deleted;
            rev = _Rev;
            is_dir = _Is_Dir;
            modified = _Modified;
            icon = _Icon;
            root = _Root;
            size = _Size;
            bytes = _Bytes;
            thumb_exists = _Thumb_exists;
        } 
        #endregion

        #region Properties

        /// <summary>
        /// Returns the canonical path to the file or directory.
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }

            protected set
            {
                path = value;
            }
        }

        /// <summary>
        /// Whether the given entry is deleted (only not null if deleted files are being returned).
        /// </summary>
        public bool? Is_Deleted
        {
            get
            {
                return is_deleted;
            }

            protected set
            {
                is_deleted= value;
            }
        }

        /// <summary>
        /// A unique identifier for the current revision of a file. 
        /// This field is the same rev as elsewhere in the API and can be used to detect changes and avoid conflicts.
        /// </summary>
        public string Rev
        {
            get
            {
                return rev;
            }

            protected set
            {
                rev = value;
            }
        }

        /// <summary>
        /// Whether the given entry is a folder or not.
        /// </summary>
        public bool Is_Dir
        {
            get
            {
                return is_dir;
            }

            protected set
            {
                is_dir = value;
            }
        }

        /// <summary>
        /// The last time the file was modified on Dropbox, in the standard date format (not included for the root folder).
        /// </summary>
        public DateTime Modified
        {
            get
            {
                return modified;
            }

            protected set
            {
                modified = value;
            }
        }

        /// <summary>
        /// The name of the icon used to illustrate the file type in Dropbox's icon library.
        /// </summary>
        public string Icon
        {
            get
            {
                return icon;
            }

            protected set
            {
                icon = value;
            }
        }

        /// <summary>
        /// The root or top-level folder depending on your access level. All paths returned are relative to this root level. Permitted values are either dropbox or app_folder.
        /// </summary>
        public string Root
        {
            get
            {
                return root;
            }

            protected set
            {
                root = value;
            }
        }

        /// <summary>
        /// A human-readable description of the file size (translated by locale).
        /// </summary>
        public string Size
        {
            get
            {
                return size;
            }

            protected set
            {
                size = value;
            }
        }

        /// <summary>
        /// The file size in bytes.
        /// </summary>
        public long Bytes
        {
            get
            {
                return bytes;
            }

            protected set
            {
                bytes = value;
            }
        }

        /// <summary>
        /// True if the file is an image that can be converted to a thumbnail via the /thumbnails call.
        /// </summary>
        public bool Thumb_exists
        {
            get
            {
                return thumb_exists;
            }

            protected set
            {
                thumb_exists = value;
            }
        }

        #region Elements used for deserialisation
        public string path;

        public bool? is_deleted;

        public string rev;

        public bool is_dir;

        public DateTime modified;

        public string icon;

        public string root;

        public string size;

        public long bytes;

        public bool thumb_exists; 
        #endregion
        #endregion
        
    }

    /// <summary>
    /// Represents the implementation of IFileInfo
    /// </summary>
    internal class FileInfo : StoreDataInfo, IFileInfo
    {
        #region Constructors
        public FileInfo():base()
        {
        }
        
        public FileInfo(string _Size, long _Bytes, bool _Thumb_exists, DateTime _Client_MTime,
            string _Mime_Type, string _Path, bool? _Is_Deleted, string _Rev, bool _Is_Dir, 
            DateTime _Modified, string _Icon, string _Root)
            : base(_Path, _Is_Deleted, _Rev, _Is_Dir, _Modified, _Icon, _Root, _Size, _Bytes, _Thumb_exists)
        {
            client_mtime = _Client_MTime;
            mime_type = _Mime_Type;
        } 
        #endregion

        #region Public Properties

        /// <summary>
        /// For files, this is the modification time set by the desktop client when the file was added to Dropbox, 
        /// in the standard date format. 
        /// Since this time is not verified (the Dropbox server stores whatever the desktop client sends up), 
        /// this should only be used for display purposes (such as sorting) and not, for example, to determine if a file has changed or not.
        /// </summary>
        public DateTime Client_MTime
        {
            get
            {
                return client_mtime;
            }

            protected set
            {
                client_mtime = value;
            }
        }

        /// <summary>
        /// Mime type of the folder or file.
        /// </summary>
        public string Mime_Type
        {
            get
            {
                return mime_type;
            }

            protected set
            {
                mime_type = value;
            }
        }

        #region Elements used for Deserialisation
        public DateTime client_mtime;
        public string mime_type; 
        #endregion
	#endregion
    }

    /// <summary>
    /// Represents the implementation of IFolderInfo
    /// </summary>
    internal class FolderInfo : StoreDataInfo, IFolderInfo
    {
        #region Constructors
        public FolderInfo():base()
        {
        }

        public FolderInfo(List<StoreDataInfo> _Contents, string _Path, bool? _Is_Deleted, string _Rev, 
            bool _Is_Dir, string _Hash, DateTime _Modified,
                    string _Icon, string _Root, string _Size, long _Bytes, bool _Thumb_exists)
            : base(_Path, _Is_Deleted, _Rev, _Is_Dir, _Modified, _Icon, _Root, _Size, _Bytes, _Thumb_exists)
        {
            Contents = _Contents;
            Hash = _Hash;
        } 
        #endregion

        #region Public Properties
        /// <summary>
        /// A folder's hash is useful for indicating changes to the folder's contents in later calls to /metadata. 
        /// This is roughly the folder equivalent to a file's rev.
        /// </summary>
        public string Hash
        {
            get;
            protected set;
        }

        /// <summary>
        /// List of metadata information for elements in the folder
        /// </summary>
        public List<IStoreDataInfo> Elements
        {
            get
            {
                List<IStoreDataInfo> list = new List<IStoreDataInfo>();
                if (Contents != null && Contents.Count > 0)
                {
                    foreach (var item in Contents)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }

            protected set
            {
                Contents = new List<StoreDataInfo>();
                foreach (var item in value)
                {
                    if (item.Is_Dir)
                    {
                        Contents.Add((FolderInfo)item);
                    }
                    else
                    {
                        Contents.Add((FileInfo)item);
                    }

                }
            }
        }

        /// <summary>
        /// For deserialization purposes
        /// </summary>
        public List<StoreDataInfo> Contents
        {
            get;
            set;
        } 
        #endregion
    }
}
