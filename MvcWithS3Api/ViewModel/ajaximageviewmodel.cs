using MvcWithS3Api.UploadUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcWithS3Api.ViewModel
{
    /// <summary>
    /// Use to transfer data between views to model and controller 
    /// it can be more customize.
    /// </summary>
    public class AjaxImageViewModel
    {


        private bool? Thumb;
        private bool? Mulitple;
        private bool? _OnlyOneImage;
        private string Title = "Ajax Image Uploader";
        private List<string> _UploadFileURL = new List<string>();
         

        public HttpPostedFileBase[] FileUpload { get; set; }

         
        [Required]
        public string BucketName { get; set; }
        [Required]
        public FolderName BuketFolder { get; set; }
        [Required]
        public SubFolderName SubFolder { get; set; }

        public string UpdateTarget { get; set; }

        public string ControllerTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Title)) return Title;

                return Title;
            }
            set
            {
                Title = value;
            }
        }
      
         
        public bool MulitpleImages
        {
            get
            {
                if (Mulitple.HasValue) return Mulitple.Value;

                return false;
            }
            set
            {
                Mulitple = value;
            }
        }

        public bool Thumbnail
        {
            get
            {
                if (Thumb.HasValue) return Thumb.Value;

                return false;
            }
            set
            {
                Thumb = value;
            }
        }

        public List<string> UploadFileURL
        {

            get {
                return _UploadFileURL;
            }
            set { _UploadFileURL = value; }

        }

    }

}