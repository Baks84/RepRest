using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.SkyDrive
{
    public interface ISkyDriveElement
    {

    }

    public interface IFile : ISkyDriveElement
    {
        /// <summary>
        /// An array container for File objects, if a collection of objects is returned.
        /// </summary>
        List<IFile> Items { get; set; }

        /// <summary>
        ///   The size, in bytes, of the file.
        /// </summary>
        long Size { get; set; }

        /// <summary>
        ///   The number of comments that are associated with the file.
        /// </summary>
        long Comments_Count { get; set; }

        /// <summary>
        ///   A value that indicates whether comments are enabled for the file. If comments can be made, this value is true; otherwise, it is false.
        /// </summary>
        bool Comments_Enabled { get; set; }

        /// <summary>
        ///   The URL to use to download the file from SkyDrive. This value is not persistent. Use it immediately after making the request, and avoid caching.  This structure is not available if the file is a Office OneNote notebook.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        ///   The File object's ID.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///   Info about the user who uploaded the file.
        /// </summary>
        IFrom FromUser { get; set; }

        /// <summary>
        ///   The name of the file.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///   A description of the file, or null if no description is specified.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///   The ID of the folder the file is currently stored in.
        /// </summary>
        string Parent_Id { get; set; }

        /// <summary>
        ///   The URL to upload file content hosted in SkyDrive.  This structure is not available if the file is a Microsoft Office OneNote notebook.
        /// </summary>
        string Upload_Location { get; set; }

        /// <summary>
        ///   A value that indicates whether this file can be embedded. If this file can be embedded, this value is true; otherwise, it is false.
        /// </summary>
        bool Is_Embeddable { get; set; }

        /// <summary>
        ///   A URL to view the item on SkyDrive.
        /// </summary>
        string Link { get; set; }

        /// <summary>
        ///   The type of object; in this case, "file". If the file is a Office OneNote notebook, the type structure is set to "notebook".
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///   Object that contains permission info.
        /// </summary>
        ISharedWith Shared_With_User { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was created.
        /// </summary>
        string Created_Time { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was last updated.
        /// </summary>
        string Updated_Time { get; set; }
    }

    public interface IFrom : ISkyDriveElement
    {
        /// <summary>
        /// The name of the user who uploaded the file.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The ID of the user who uploaded the file.
        /// </summary>
        string Id { get; set; }
    }

    public interface ISharedWith : ISkyDriveElement
    {
        ///<summary>
        ///  Info about who can access the folder. The options are:
        ///  - People I selected
        ///  - Just me
        ///  - Everyone (public)
        ///  - Friends
        ///  - My friends and their friends
        ///  - People with a link
        ///  The default is Just me.
        ///</summary>
        string Access { get; set; }
    }

    public interface IFolder : IFile,ISkyDriveElement
    {
        string Root {get;}
        string FolderType{get;}

        /// <summary>
        /// The total number of items in the folder.
        /// </summary>
        long Count { get; set; }
    }

    public interface IAlbum : IFolder, ISkyDriveElement
    {
        string AlbumType { get; }
    }

    public interface IAudio : IFile, ISkyDriveElement
    {
        /// <summary>
        /// A URL of a picture that represents the video.
        /// </summary>
        string Picture { get; set; }

        /// <summary>
        /// The duration, in milliseconds, of the video run time.
        /// </summary>
        long Duration { get; set; }

        /// <summary>
        /// The audio's title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The audio's artist name.
        /// </summary>
        string Artist { get; set; }

        /// <summary>
        /// The audio's album name.
        /// </summary>
        string Album { get; set; }

        /// <summary>
        /// The artist name of the audio's album.
        /// </summary>
        string Album_Artist { get; set; }

        /// <summary>
        /// The audio's genre.
        /// </summary>
        string Genre { get; set; }
    }

    public interface IResource : ISkyDriveElement
    {
        /// <summary>
        ///   The File object's ID.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///   Info about the user who uploaded the file.
        /// </summary>
        IFrom From { get; set; }

        /// <summary>
        ///   The name of the file.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///   A description of the file, or null if no description is specified.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///   The ID of the folder the file is currently stored in.
        /// </summary>
        string Parent_Id { get; set; }

        /// <summary>
        ///   The URL to upload file content hosted in SkyDrive.  This structure is not available if the file is a Microsoft Office OneNote notebook.
        /// </summary>
        string Upload_Location { get; set; }

        /// <summary>
        ///   A value that indicates whether this file can be embedded. If this file can be embedded, this value is true; otherwise, it is false.
        /// </summary>
        bool Is_Embeddable { get; set; }

        /// <summary>
        ///   A URL to view the item on SkyDrive.
        /// </summary>
        string Link { get; set; }

        /// <summary>
        ///   The type of object; in this case, "file". If the file is a Office OneNote notebook, the type structure is set to "notebook".
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///   Object that contains permission info.
        /// </summary>
        ISharedWith Shared_With { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was created.
        /// </summary>
        string Created_Time { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was last updated.
        /// </summary>
        string Updated_Time { get; set; }
    }

    public interface ICollection : IResource, ISkyDriveElement
    {
        /// <summary>
        /// The total number of items in the folder.
        /// </summary>
        long Count { get; set; }
    }

    public interface IImage : ISkyDriveElement
    {
        /// <summary>
        ///   The height, in pixels, of this image of this particular size.
        /// </summary>
        long Height { get; set; }

        /// <summary>
        ///   The width, in pixels, of this image of this particular size.
        /// </summary>
        long Width { get; set; }

        /// <summary>
        ///   A URL of the source file of this image of this particular size.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        ///   The type of this image of this particular size. Valid values are:
        ///   full (max size 2048 x 2048 pixels)
        ///   normal (max size 800 x 800 pixels)
        ///   album (max size 200 x 200 pixels)
        ///   small (max size 100 x 100 pixels)
        /// </summary>
        string Type { get; set; }
    }

    public interface ILocation : ISkyDriveElement
    {
        /// <summary>
        /// The latitude portion of the location where the photo was taken, expressed as positive (north) or negative (south) degrees relative to the equator.
        /// </summary>
        double Lattitude { get; set; }

        /// <summary>
        /// The longitude portion of the location where the photo was taken, expressed as positive (east) or negative (west) degrees relative to the Prime Meridian.
        /// </summary>
        double Longitude { get; set; }
    }

    public interface IPhoto : IFile, ISkyDriveElement
    {
        /// <summary>
        /// The height, in pixels, of the photo.
        /// </summary>
        long Height { get; set; }

        /// <summary>
        /// The width, in pixels, of the photo.
        /// </summary>
        long Width { get; set; }

        /// <summary>
        ///   Info about various sizes of the photo.
        /// </summary>
        List<IImage> Versions { get; set; }

        /// <summary>
        /// The date, in ISO 8601 format, on which the photo was taken, or null if no date is specified.
        /// </summary>
        string When_Taken { get; set; }

        /// <summary>
        /// The location where the photo was taken. The location object is not available for shared photos.
        /// </summary>
        ILocation Where_Taken { get; set; }
    }

    public interface IUserQuota : ISkyDriveElement
    {
        /// <summary>
        /// The user's remaining unused storage space, in bytes.
        /// </summary>
        long Available { get; set; }

        /// <summary>
        /// The user's total available storage space, in bytes
        /// </summary>
        long Quota { get; set; }
    }

    public interface IVideo : IFile, ISkyDriveElement
    {
        /// <summary>
        /// The height, in pixels, of the photo.
        /// </summary>
        long Height { get; set; }

        /// <summary>
        /// The width, in pixels, of the photo.
        /// </summary>
        long Width { get; set; }

        /// <summary>
        /// A URL of a picture that represents the video.
        /// </summary>
        string Picture { get; set; }

        /// <summary>
        /// The duration, in milliseconds, of the video run time.
        /// </summary>
        long Duration { get; set; }

        /// <summary>
        /// The bit rate, in bits per second, of the video.
        /// </summary>
        long BitRate { get; set; }
    }
}
