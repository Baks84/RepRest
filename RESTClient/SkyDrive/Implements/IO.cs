using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.SkyDrive.Implements
{
    internal class File : SkyNet.Model.File, IFile
    {
        public List<IFile> Items
        {
            get
            {
                return ConvertFromSkyNetModel(this.Data);
            }
            set
            {
                Data = ConvertToSkyNetModel(value);
            }
        }

        List<SkyNet.Model.File> ConvertToSkyNetModel(List<IFile> files)
        {
            List<SkyNet.Model.File> result = new List<SkyNet.Model.File>();

            if (files != null)
            {

                foreach (var item in files)
                {
                    result.Add(new SkyNet.Model.File()
                    {
                        Comments_Count = item.Comments_Count,
                        Comments_Enabled = item.Comments_Enabled,
                        Created_Time = item.Created_Time,
                        Data = ((File)item).Data,
                        Description = item.Description,
                        From = ((File)item).From,
                        Id = item.Id,
                        Is_Embeddable = item.Is_Embeddable,
                        Link = item.Link,
                        Name = item.Name,
                        Parent_Id = item.Parent_Id,
                        Shared_With = ((File)item).Shared_With,
                        Size = item.Size,
                        Source = item.Source,
                        Type = item.Type,
                        Updated_Time = item.Updated_Time,
                        Upload_Location = item.Upload_Location
                    });
                }
            }
            else
            {
                result = null;
            }

            return result;
        }

        List<IFile> ConvertFromSkyNetModel(List<SkyNet.Model.File> files)
        {
            List<IFile> result = new List<IFile>();

            if (files != null)
            {
                foreach (var item in files)
                {
                    result.Add(new File(item));
                    //{
                    //    Comments_Count = item.Comments_Count,
                    //    Comments_Enabled = item.Comments_Enabled,
                    //    Created_Time = item.Created_Time,
                    //    Data = item.Data,
                    //    Description = item.Description,
                    //    From = item.From,
                    //    Id = item.Id,
                    //    Is_Embeddable = item.Is_Embeddable,
                    //    Link = item.Link,
                    //    Name = item.Name,
                    //    Parent_Id = item.Parent_Id,
                    //    Shared_With = item.Shared_With,
                    //    Size = item.Size,
                    //    Source = item.Source,
                    //    Type = item.Type,
                    //    Updated_Time = item.Updated_Time,
                    //    Upload_Location = item.Upload_Location
                    //});
                }
            }
            else
            {
                result = null;
            }

            return result;
        }

        public IFrom FromUser
        {
            get
            {
                return new From() { Id = this.From.Id, Name = this.From.Name };
            }
            set
            {
                this.From = new SkyNet.Model.From()
                {
                    Id=value.Id,
                    Name = value.Name
                };
            }
        }

        public ISharedWith Shared_With_User
        {
            get
            {
                return new SharedWith() { Access = this.Shared_With.Access};
            }
            set
            {
                this.Shared_With = new SkyNet.Model.SharedWith()
                {
                    Access = value.Access
                };
            }
        }

        public File() : base()
        {
        }

        public File(SkyNet.Model.File fromFile)
            : base()
        {
                    this.Comments_Count = fromFile.Comments_Count;
                    this.Comments_Enabled = fromFile.Comments_Enabled;
                    this.Created_Time = fromFile.Created_Time;
                    this.Data = fromFile.Data;
                    this.Description = fromFile.Description;
                    this.From = fromFile.From;
                    this.Id = fromFile.Id;
                    this.Is_Embeddable = fromFile.Is_Embeddable;
                    this.Link = fromFile.Link;
                    this.Name = fromFile.Name;
                    this.Parent_Id = fromFile.Parent_Id;
                    this.Shared_With = fromFile.Shared_With;
                    this.Size = fromFile.Size;
                    this.Source = fromFile.Source;
                    this.Type = fromFile.Type;
                    this.Updated_Time = fromFile.Updated_Time;
                    this.Upload_Location = fromFile.Upload_Location;
        }
    }

    internal class Resource : IResource
    {
        /// <summary>
        ///   The File object's ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///   Info about the user who uploaded the file.
        /// </summary>
        public IFrom From { get; set; }

        /// <summary>
        ///   The name of the file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   A description of the file, or null if no description is specified.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   The ID of the folder the file is currently stored in.
        /// </summary>
        public string Parent_Id { get; set; }

        /// <summary>
        ///   The URL to upload file content hosted in SkyDrive.  This structure is not available if the file is a Microsoft Office OneNote notebook.
        /// </summary>
        public string Upload_Location { get; set; }

        /// <summary>
        ///   A value that indicates whether this file can be embedded. If this file can be embedded, this value is true; otherwise, it is false.
        /// </summary>
        public bool Is_Embeddable { get; set; }

        /// <summary>
        ///   A URL to view the item on SkyDrive.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///   The type of object; in this case, "file". If the file is a Office OneNote notebook, the type structure is set to "notebook".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///   Object that contains permission info.
        /// </summary>
        public ISharedWith Shared_With { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was created.
        /// </summary>
        public string Created_Time { get; set; }

        /// <summary>
        /// The time, in ISO 8601 format, at which the file was last updated.
        /// </summary>
        public string Updated_Time { get; set; }
    }

    internal class SharedWith : SkyNet.Model.SharedWith, ISharedWith { }

    internal class From : SkyNet.Model.From, IFrom { }

    internal class Folder : File, IFolder
    {
        public Folder()
            : base()
        { }

        public Folder(SkyNet.Model.Folder folder)
            : base(folder)
        {
            this.Count = folder.Count;
        }

        public string Root { get { return ""; } }
        public string FolderType { get { return "folder"; } }

        /// <summary>
        /// The total number of items in the folder.
        /// </summary>
        public long Count { get; set; }
    }

    internal class Album : Folder, IAlbum
    {
        public Album() { }
        public Album(SkyNet.Model.Album album):base(album)
        {
            
        }

        public string AlbumType { get { return "album"; } }
    }

    internal class Audio : File, IAudio
    {
        public Audio()
            : base()
        {
        }

        public Audio(SkyNet.Model.Audio element)
            : base(element)
        {
            this.Picture = element.Picture;
            this.Duration = element.Duration;
            this.Title = element.Title;
            this.Artist = element.Artist;
            this.Album = element.Album;
            this.Album_Artist = element.Album_Artist;
            this.Genre = element.Genre;
        }

        /// <summary>
        /// A URL of a picture that represents the video.
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// The duration, in milliseconds, of the video run time.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// The audio's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The audio's artist name.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The audio's album name.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// The artist name of the audio's album.
        /// </summary>
        public string Album_Artist { get; set; }

        /// <summary>
        /// The audio's genre.
        /// </summary>
        public string Genre { get; set; }
    }

    internal class Collection : Resource, ICollection
    {
        /// <summary>
        /// The total number of items in the folder.
        /// </summary>
        public long Count { get; set; }
    }

    internal class Image : IImage
    {
        public Image() { }
        public Image(SkyNet.Model.Image img)
        {
            this.Height = img.Height;
            this.Source = img.Source;
            this.Type = img.Type;
            this.Width = img.Width;
        }

        public SkyNet.Model.Image SkyNetVersion
        {
            get
            {
                return new SkyNet.Model.Image()
                {
                    Height=this.Height,
                    Source = this.Source,
                    Type = this.Type,
                    Width = this.Width
                };
            }
        }

        /// <summary>
        ///   The height, in pixels, of this image of this particular size.
        /// </summary>
        public long Height { get; set; }

        /// <summary>
        ///   The width, in pixels, of this image of this particular size.
        /// </summary>
        public long Width { get; set; }

        /// <summary>
        ///   A URL of the source file of this image of this particular size.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///   The type of this image of this particular size. Valid values are:
        ///   full (max size 2048 x 2048 pixels)
        ///   normal (max size 800 x 800 pixels)
        ///   album (max size 200 x 200 pixels)
        ///   small (max size 100 x 100 pixels)
        /// </summary>
        public string Type { get; set; }
    }

    internal class Location : SkyNet.Model.Location, ILocation
    {
        public Location() :base() { }
        public Location(SkyNet.Model.Location loc) : base() 
        {
            this.Lattitude = loc.Lattitude;
            this.Longitude = loc.Longitude;
        }
    }

    internal class Photo : File, IPhoto
    {
        public Photo()
            : base()
        {
        }

        public Photo(SkyNet.Model.Photo photo)
            : base(photo)
        {
            this.Height = photo.Height;
            this.Width = photo.Width;
            this.Images = photo.Images;
            this.When_Taken = photo.When_Taken;
            this.Location = photo.Location;
        }

        /// <summary>
        /// The height, in pixels, of the photo.
        /// </summary>
        public long Height { get; set; }

        /// <summary>
        /// The width, in pixels, of the photo.
        /// </summary>
        public long Width { get; set; }

        /// <summary>
        ///   Info about various sizes of the photo.
        /// </summary>
        public List<IImage> Versions 
        {
            get
            {
                List<IImage> result = new List<IImage>();

                foreach (var item in this.Images)
                {
                    result.Add(new Image(item));
                }

                return result;
            }
            
            set
            {
                this.Images.Clear();
                foreach (var item in value)
                {
                    this.Images.Add(((Image)item).SkyNetVersion);
                }
            }
        }

        public List<SkyNet.Model.Image> Images { get; set; }

        /// <summary>
        /// The date, in ISO 8601 format, on which the photo was taken, or null if no date is specified.
        /// </summary>
        public string When_Taken { get; set; }

        /// <summary>
        /// The location where the photo was taken. The location object is not available for shared photos.
        /// </summary>
        public SkyNet.Model.Location Location { get; set; }
        public ILocation Where_Taken 
        {
            get
            {
                Location loc = new Location(this.Location);
                return loc;
            }
            set
            {
                this.Location = new SkyNet.Model.Location()
                {
                    Lattitude = value.Lattitude,
                    Longitude = value.Longitude
                };
            }
        }
    }

    internal class UserQuota : SkyNet.Model.UserQuota, IUserQuota
    {
        public UserQuota() : base() { }
        public UserQuota(SkyNet.Model.UserQuota uq)
            : base()
        {
            this.Available = uq.Available;
            this.Quota = uq.Quota;
        }
    }

    internal class Video : File, IVideo
    {
        public Video()
            : base()
        { }

        public Video(SkyNet.Model.Video element)
            : base(element)
        {
            this.Height = element.Height;
            this.Width = element.Width;
            this.Picture = element.Picture;
            this.Duration = element.Duration;
            this.BitRate = element.BitRate;
        }

        /// <summary>
        /// The height, in pixels, of the photo.
        /// </summary>
        public long Height { get; set; }

        /// <summary>
        /// The width, in pixels, of the photo.
        /// </summary>
        public long Width { get; set; }

        /// <summary>
        /// A URL of a picture that represents the video.
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// The duration, in milliseconds, of the video run time.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// The bit rate, in bits per second, of the video.
        /// </summary>
        public long BitRate { get; set; }
    }
}
