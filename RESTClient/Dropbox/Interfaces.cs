using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RESTClient.Dropbox.Implements;

namespace RESTClient.Dropbox
{
    public interface IToken
    {
        String Access_Token { get; set; }
        String Token_Type { get; set; }
        int UID { get; set; }
    }

    public interface IUserInfo
    {
        string Referral_link { get; set; }
        string Display_Name { get; set; }
        string Country { get; set; }
        int UID { get; set; }
        IQuotaInfo Quota { get;}
    }

    public interface IQuotaInfo
    {
        //The user's used quota in shared folders (bytes)
        decimal shared { get; set; }

        //The user's total quota allocation (bytes).
        decimal quota { get; set; }

        //The user's used quota outside of shared folders (bytes)
        decimal normal { get; set; }
    }

    public interface IFileInfo : IStoreDataInfo
    {
        DateTime Client_MTime { get; }
        string Mime_Type { get; }
    }

    public interface IFolderInfo : IStoreDataInfo
    {
        string Hash { get; }
        List<IStoreDataInfo> Elements { get;}
    }

    public interface IStoreDataInfo
    {
        string Size { get; }
        long Bytes { get; }
        bool Thumb_exists { get; }
        string Path { get; }
        bool? Is_Deleted { get; }
        string Rev { get; }
        bool Is_Dir { get; }
        DateTime Modified { get; }
        //Icon name in dropbox library
        string Icon { get; }
        string Root { get; }
    }

    public interface IDelta
    {
        Dictionary<string, IFileInfo> Entries { get; }
        bool Reset { get; }
        string Cursor { get; }
        bool Has_More { get; }
    }
    
    public interface IDeltaEntry
    {
        String Path { get; }
        //IFileInfo Metadata { get; }
    }

    public interface ILongpollDeltaResult
    {
        bool Changes { get; }
        int Backoff { get; }
    }

    public interface IShareLink
    {
        Uri Url { get; }
        DateTime Expires { get; }
    }

    public interface ICopyRef
    {
        String Copy_ref { get; }
        DateTime Expires { get; }
    }

    public interface IChunkUploadResult
    {
        String Upload_ID { get; }
        long Offset { get; }
        DateTime Expires { get; }
    }
}
