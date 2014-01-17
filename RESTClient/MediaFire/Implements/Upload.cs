using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.MediaFire.Implements
{
    internal class Upload : BaseResponse, IUpload
    {
        public doupload doupload { get; set; }
        public string upload_key { get; set; }

        private Dictionary<int, string> UploadResultCodeToMessage;

        public Upload()
        {
            UploadResultCodeToMessage = new Dictionary<int, string>();
            UploadResultCodeToMessage.Add(14, "Upload succeeded but the folder specified does not exist, so the file was placed in the root folder");
            UploadResultCodeToMessage.Add(-1, "Filedrop key is invalid");
            UploadResultCodeToMessage.Add(-8, "Filedrop key is invalid");
            UploadResultCodeToMessage.Add(-11, "Filedrop key is invalid");
            UploadResultCodeToMessage.Add(-21, "Invalid filedrop configuration");
            UploadResultCodeToMessage.Add(-22, "Invalid filedrop configuration");
            UploadResultCodeToMessage.Add(-31, "Unknown upload error");
            UploadResultCodeToMessage.Add(-40, "Unknown upload error");
            UploadResultCodeToMessage.Add(-32, "Missing file data");
            UploadResultCodeToMessage.Add(-41, "The uploaded file exceeds the upload_max_filesize");
            UploadResultCodeToMessage.Add(-42, "The uploaded file exceeds the MAX_FILE_SIZE directive that was specified in the HTML form");
            UploadResultCodeToMessage.Add(-43, "The uploaded file was only partially uploaded");
            UploadResultCodeToMessage.Add(-44, "No file was uploaded");
            UploadResultCodeToMessage.Add(-45, "Missing a temporary folder");
            UploadResultCodeToMessage.Add(-46, "Failed to write file to disk");
            UploadResultCodeToMessage.Add(-47, "A PHP extension stopped the file upload");
            UploadResultCodeToMessage.Add(-48, "Invalid file size");
            UploadResultCodeToMessage.Add(-49, "Missing file name");
            UploadResultCodeToMessage.Add(-51, "File zise does not match size on disk");
            UploadResultCodeToMessage.Add(-90, "The hash sent does not much the actual file hash");
            UploadResultCodeToMessage.Add(-99, "Missing or invalid session_token");
            UploadResultCodeToMessage.Add(-203, "Invalid quickkey or file does not belong to the session user");
            UploadResultCodeToMessage.Add(-204, "User does not have write permissions to the file");
            UploadResultCodeToMessage.Add(-205, "User does not have write permissions to the destination folder");
            UploadResultCodeToMessage.Add(-302, "User attempted to send a resumable upload unit before calling upload/pre_upload");
            UploadResultCodeToMessage.Add(-303, "Invalid Unit size");
            UploadResultCodeToMessage.Add(-304, "Invalid unit hash");
            UploadResultCodeToMessage.Add(-701, "Maximum file size for free users exceeded");
            UploadResultCodeToMessage.Add(-881, "Maximum file size for free users exceeded");
            UploadResultCodeToMessage.Add(-700, "Maximum file size exceeded");
            UploadResultCodeToMessage.Add(-882, "Maximum file size exceeded");
        }

        public string Upload_Key
        {
            get
            {
                if (doupload != null)
                {
                    return doupload.key;
                }
                return upload_key;
            }
        }

        public int Upload_Result
        {
            get 
            {
                if (doupload != null)
                {
                    return doupload.result;
                }
                return 0;
            }
        }

        public string Upload_Result_Message
        {
            get 
            {
                if (UploadResultCodeToMessage != null && doupload != null && UploadResultCodeToMessage.ContainsKey(Upload_Result))
                {
                    return UploadResultCodeToMessage[Upload_Result];
                }
                return "";
            }
        }


        public string Hash
        {
            get;
            set;
        }
    }

    internal class doupload
    {
        public string key{get;set;}
        public int result {get;set;}
    }

    internal class WebUpload : Upload, IWebUpload
    {
        public string active { get; set; }
        public bool Active
        {
            get 
            {
                return active == "yes" ? true : false;
            }
        }

        public string Filename
        {
            get;
            set;
        }

        public DateTime Created
        {
            get;
            set;
        }

        public int Status_code
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string url { get; set; }
        public Uri URL
        {
            get 
            {
                return new Uri(url);
            }
        }

        public string Eta
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public int Percentage
        {
            get;
            set;
        }
    }

    internal class WebUploads : BaseResponse
    {
        public List<WebUpload> web_uploads { get; set; }
        public List<IWebUpload> GetItems()
        {
            List<IWebUpload> result = new List<IWebUpload>();

            if (web_uploads != null && web_uploads.Count > 0)
            {
                foreach (var item in web_uploads)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }

    internal class PreUpload : BaseResponse, IPreUpload
    {
        public string quickkey { get; set; }
        public string FileID
        {
            get { return quickkey; }
        }

        public string FileName
        {
            get;
            set;
        }

        public string new_hash { get; set; }
        public bool NewHash
        {
            get { return new_hash=="yes" ? true : false; }
        }

        public string duplicate_name { get; set; }
        public bool DuplicateName
        {
            get { return duplicate_name=="yes" ? true : false; }
        }

        public ResumableUpload resumable_upload { get; set; }
        public IResumableUpload Resumable_upload
        {
            get { return resumable_upload; }
        }


        public long Storage_limit
        {
            get;
            set;
        }

        public long Used_storage_size
        {
            get;
            set;
        }

        public string storage_limit_exceeded { get; set; }
        public bool AvailableSpaceExceeded
        {
            get { return storage_limit_exceeded == "yes" ? true : false; }
        }
    }

    internal class ResumableUpload : IResumableUpload
    {
        public string all_units_ready { get; set; }
        public bool AllUnitsUploaded
        {
            get { return all_units_ready == "yes" ? true : false; }
        }

        public int Number_of_units
        {
            get;
            set;
        }

        public int Unit_size
        {
            get;
            set;
        }

        public UploadBitmap bitmap { get; set; }
        public IUploadBitmap UploadBitmap
        {
            get { return bitmap; }
        }
    }

    internal class UploadBitmap : IUploadBitmap
    {

        public int Count
        {
            get;
            set;
        }

        public List<int> Words
        {
            get;
            set;
        }
    }

    internal class UploadPoll : IPollUpload
    {
        public int result { get; set; }
        public int ResultCode
        {
            get { return result; }
        }

        public string description { get; set; }
        public string Result
        {
            get
            {
                string res = "";
                switch (result)
                {
                    case 0:
                        res = "Success";
                        break;
                    case -20:
                        res = "Invalid Upload Key";
                        break;
                    case -80:
                        res = "Upload Key not found";
                        break;
                    default:
                        break;
                }
                return res;
            }
        }

        public int status { get; set; }
        public int StatusCode
        {
            get { return status; }
        }

        public string Status
        {
            get { return description; }
        }

        public int? fileerror { get; set; }
        public int FileErrorCode
        {
            get { return fileerror.HasValue ? fileerror.Value : -1; }
        }

        public string FileError
        {
            get 
            {
                string res = "";
                switch (fileerror)
                {
                    #region Switch
                    case 1:
                        res = "File is larger than the maximum filesize allowed";
                        break;

                    case 2:
                        res = "File size cannot be 0";
                        break;

                    case 3:
                    case 4:
                    case 9:
                        res = "Found a bad RAR file";
                        break;

                    case 5:
                        res = "Virus found";
                        break;

                    case 6:
                    case 8:
                    case 10:
                        res = "Unknown internal error";
                        break;

                    case 7:
                        res = "File hash or size mismatch";
                        break;

                    case 12:
                        res = "Failed to insert data into databse";
                        break;

                    case 13:
                        res = "File name already exists in the same parent folder, skipping";
                        break;

                    case 14:
                        res = "Destination folder does not exist";
                        break;

                    case 15:
                        res = "Account storage limit is reached";
                        break;

                    case 16:
                        res = "There was a file update revision conflict";
                        break;

                    case 17:
                        res = "Error patching delta file";
                        break;

                    case 18:
                        res = "Account is blocked";
                        break;

                    case 19:
                        res = "Failure to create path";
                        break;

                    default:
                        break; 
                    #endregion
                }
                return res;
            }
        }

        public string quickkey { get; set; }
        public string FileID
        {
            get { return quickkey; }
        }

        public int Size
        {
            get;
            set;
        }
    }

    internal class UploadPollResponse : BaseResponse
    {
        public UploadPoll doupload { get; set; }
    }
}
