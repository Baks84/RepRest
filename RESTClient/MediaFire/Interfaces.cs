using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RESTClient.MediaFire.Implements;

namespace RESTClient.MediaFire
{
    internal interface IResponse
    {
        string result { get; set; }
        string action { get; set; }
        string message { get; set; }
    }

    public interface IToken
    {
        String Access_Token { get; set; }
        String Token_Type { get; set; }
        String PKEY { get; set; }
        DateTime Expires { get; set; }
    }

    public interface IUserInfo
    {
        string Email { get; set; }
        string First_Name { get; set; }
        string Last_Name { get; set; }
        string Display_Name { get; set; }
        string Gender { get; set; }
        DateTime? Birth_Date { get; set; }
        string Premium { get; set; }
        long? Bandwidth { get; set; }
        DateTime? Created { get; set; }
        string Validated { get; set; }
        long? Max_upload_size { get; set; }
        long? Max_instant_upload_size { get; set; }
        string Tos_accpeted { get; set; }
        long? Used_storage_size { get; set; }
        long? Base_storage { get; set; }
        long? Bonus_storage { get; set; }
        long? Storage_limit { get; set; }
        string Storage_limit_exceeded { get; set; }

    }

    public interface IFileInfo : IStoreDataInfo
    {
        int Downloads { get; set; }
        long Size { get; set; }
        string Privacy { get; set; }
        string Password_protected { get; set; }
        string Hash { get; set; }
        string Filetype { get; set; }
        string Mimetype { get; set; }
    }

    public interface IFolderInfo : IStoreDataInfo
    {
        string Tags { get; set; }
        int Epoch { get; set; }
        string Custom_url { get; set; }
        int Dbx_enabled { get; set; }
        int File_Count { get; set; }
        int Folder_Count { get; set; }
        int Total_Folders { get; set; }
        int Total_Files { get; set; }
        long Total_Size { get; set; }
        string Dropbox_enabled { get; set; }
    }

    public interface IStoreDataInfo
    {
        string ID {get;set;}
        string Name {get;set;}
        string Description {get;set;}
        DateTime Created {get;set;}
        string Owner_name{get;set;}
        int Flag {get;set;}
        int? Shared_by_user {get;set;}
        int Permissions{get;set;}
        int Revision { get; set; }
        string Parent_folderkey { get; set; }
    }

    public interface IFolderDelta
    {
        List<IFolderDeltaEntry> Add_Changes { get; }
        List<IFolderDeltaEntry> Remove_Changes { get; }
        List<IFolderDeltaEntry> Update_Changes { get; }
    }
    
    public interface IFolderDeltaEntry
    {
        delta_change_type Type { get; set; }
        string ID { get; set; }
        int Revision { get; set; }
    }

    public interface IFileLinks
    {
        string FileID { get; set; }
        string View { get; set; }
        string Normal_Download { get; set; }
        string Direct_Download { get; set; }
        string Edit { get; set; }
        string One_time_download { get; set; }

        int One_time_download_request_count { get; set; }
        int Direct_download_free_bandwidth { get; set; }
    }

    public interface IFileVersion
    {
        int Revision { get; set; }
        DateTime Date { get; set; }
        string Head { get; set; }
    }

    public interface IFileVersions
    {
        List<IFileVersion> Versions { get; set; }
    }

    public interface IOneTimeDownload
    {
        int RequestCount { get; }
        int RequestCountLimit { get; }
        string Link { get; }
        string Token { get; }
    }

    public interface IUpload
    {
        string Upload_Key { get; }
        int Upload_Result { get; }
        string Upload_Result_Message { get; }
        string Hash { get; set; }
    }

    public interface IWebUpload : IUpload
    {
        bool Active { get; }
        string Filename { get; set; }
        DateTime Created { get; set; }
        int Status_code { get; set; }
        string Status { get; set; }
        Uri URL { get; }
        string Eta { get; set; }
        int Size { get; set; }
        int Percentage { get; set; }
    }

    public interface IPreUpload
    {
        string FileID { get; }
        string FileName { get; set; }
        bool NewHash { get; }
        bool DuplicateName { get; }
        long Storage_limit { get; set; }
        long Used_storage_size { get; set; }
        bool AvailableSpaceExceeded { get; }
        IResumableUpload Resumable_upload { get; }
    }

    public interface IResumableUpload
    {
        bool AllUnitsUploaded { get; }
        int Number_of_unit { get; set; }
        int Unit_size { get; set; }
        IUploadBitmap UploadBitmap { get;}
    }

    public interface IUploadBitmap
    {
        int Count { get; set; }
        List<int> Words { get; set; }
    }
}
