using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    internal class StorageInfo : IStoreDataInfo
    {
        public string folderkey { get; set; }
        public string quickkey { get; set; }

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(folderkey))
                {
                    return quickkey;
                }
                else
                {
                    return folderkey;
                }
            }
            set
            {
                folderkey = value;
                quickkey = value;
            }
        }

        public string name { get; set; }
        public string filename { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(filename))
                {
                    return name;
                }
                else
                {
                    return filename;
                }
            }
            set
            {
                filename = value;
                name = value;
            }
        }

        public string desc { get; set; }
        public string description { get; set; }

        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(desc))
                {
                    return description;
                }
                else
                {
                    return desc;
                }
            }
            set
            {
                desc = value;
                description = value;
            }
        }

        public DateTime Created
        {
            get;
            set;
        }

        public string Owner_name
        {
            get;
            set;
        }

        public int Flag
        {
            get;
            set;
        }

        public int? Shared_by_user
        {
            get;
            set;
        }

        public int Permissions
        {
            get;
            set;
        }

        public int Revision
        {
            get;
            set;
        }

        public string Parent_folderkey
        {
            get;
            set;
        }
    }

    internal class FileInfo : StorageInfo, IFileInfo
    {

        public int Downloads
        {
            get;
            set;
        }

        public long Size
        {
            get;
            set;
        }

        public string Privacy
        {
            get;
            set;
        }

        public string Password_protected
        {
            get;
            set;
        }

        public string Hash
        {
            get;
            set;
        }

        public string Filetype
        {
            get;
            set;
        }

        public string Mimetype
        {
            get;
            set;
        }
    }

    internal class FolderInfo : StorageInfo, IFolderInfo
    {

        public string Tags
        {
            get;
            set;
        }

        public int Epoch
        {
            get;
            set;
        }

        public string Custom_url
        {
            get;
            set;
        }

        public int Dbx_enabled
        {
            get;
            set;
        }

        public int File_Count
        {
            get;
            set;
        }

        public int Folder_Count
        {
            get;
            set;
        }

        public int Total_Folders
        {
            get;
            set;
        }

        public int Total_Files
        {
            get;
            set;
        }

        public long Total_Size
        {
            get;
            set;
        }


        public string Dropbox_enabled
        {
            get;
            set;
        }
    }

    internal class GetFolderInfoResponse : BaseResponse
    {
        public FolderInfo Folder_Info { get; set; }
    }

    internal class folder : FolderInfo
    {
    }

    internal class file : FileInfo
    {
    }

    internal class folder_content
    {
        public int chunk_size { get; set; }
        public int chunk_number { get; set; }
        public string content_type { get; set; }
        public List<folder> folders { get; set; }
        public List<file> files { get; set; }
    }

    internal class GetFolderContentResponse : BaseResponse
    {
        public folder_content folder_content { get; set; }
    }

    internal class GetFileInfoResponse : BaseResponse
    {
        public FileInfo File_Info { get; set; }
        public FileInfo FileInfo { get; set; }
    }

    internal class Changes : IFolderDelta
    {
        public string add { get; set; }
        public string remove { get; set; }
        public string update { get; set; }

        public List<IFolderDeltaEntry> Add_Changes
        {
            get
            {
                List<IFolderDeltaEntry> result = new List<IFolderDeltaEntry>();

                if (!string.IsNullOrEmpty(add))
                {
                    string[] items = add.Split(',');
                    foreach (var item in items)
                    {
                        string[] elements = item.Split('-');
                        result.Add(new ChangeDetail()
                        {
                            ID = elements[1],
                            Revision = int.Parse(elements[2]),
                            Type = elements[0] == "file" ? delta_change_type.file : delta_change_type.folder
                        });
                    }
                }
                return result;
            }
        }
        public List<IFolderDeltaEntry> Remove_Changes
        {
            get
            {
                List<IFolderDeltaEntry> result = new List<IFolderDeltaEntry>();

                if (!string.IsNullOrEmpty(remove))
                {
                    string[] items = remove.Split(',');
                    foreach (var item in items)
                    {
                        string[] elements = item.Split('-');
                        result.Add(new ChangeDetail()
                        {
                            ID = elements[1],
                            Revision = int.Parse(elements[2]),
                            Type = elements[0] == "file" ? delta_change_type.file : delta_change_type.folder
                        });
                    }
                }

                return result;
            }
        }
        public List<IFolderDeltaEntry> Update_Changes
        {
            get
            {
                List<IFolderDeltaEntry> result = new List<IFolderDeltaEntry>();

                if (!string.IsNullOrEmpty(update))
                {
                    string[] items = update.Split(',');
                    foreach (var item in items)
                    {
                        string[] elements = item.Split('-');
                        result.Add(new ChangeDetail()
                        {
                            ID = elements[1],
                            Revision = int.Parse(elements[2]),
                            Type = elements[0] == "file" ? delta_change_type.file : delta_change_type.folder
                        });
                    }
                }
                return result;
            }
        }

        public class ChangeDetail : IFolderDeltaEntry
        {
            public delta_change_type Type { get; set; }
            public string ID { get; set; }
            public int Revision { get; set; }
        }
    }

    internal class GetFolderRevisionResponse : BaseResponse
    {
        public int Revision { get; set; }
        public int Epoch { get; set; }
        public Changes Changes { get; set; }
    }
    
    internal class CopyFolderResponse : BaseResponse
    {
        public List<string> new_folderkeys { get; set; } 
    }

    internal class CopyFileResponse : BaseResponse
    {
        public List<string> new_quickkeys { get; set; }
    }
    
    internal class CreateFolderResponse : BaseResponse
    {
        public string folder_key { get; set; }
        public string name { get; set; }
        public DateTime created { get; set; }
        public int revision { get; set; }
        public string upload_key { get; set; }
    }

    internal class CreateSnapshotResponse : BaseResponse
    {
        public int new_revision { get; set; }
        public int old_revision { get; set; }
    }

    internal class FileVersion : IFileVersion
    {

        public int Revision
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
        public string Head { get; set; }
    }

    internal class FileVersions : BaseResponse, IFileVersions
    {
        public List<FileVersion> File_versions
        {
            get;
            set;
        }
        public List<IFileVersion> Versions
        {
            get
            {
                List<IFileVersion> result = new List<IFileVersion>();
                foreach (var item in File_versions)
                {
                    result.Add(item);
                }

                return result;
            }
            
            set
            {
                File_versions.Clear();
                foreach (var item in value)
                {
                    File_versions.Add((FileVersion)item);
                }
            }
        }
    }

    internal class RecentlyModifiedFilesResponse : BaseResponse
    {
        public List<string> quickkeys { get; set; }
    }
}