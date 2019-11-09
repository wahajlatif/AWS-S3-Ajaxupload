using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MvcWithS3Api.Models;
using System.Configuration;
namespace MvcWithS3Api.UploadUtility
{
    // This is the main utolity class where function create names and thumbnails of image to upload on S3 
    // There are two class one for image and one for Document to upload on s3

    public class Img : IAwsS3
    {


        public string FileThumbName { get; set; }

        public string ImageBucketName { get; set; }
        public string errorMsg { get; set; }
        public string FileName { get; set; }
        public List<string> ResourceURLs { get; set; }

        private  s3Service AwsS3 = new s3Service();


        public  void UploadImageToServer(Stream ObjectImage, string FileName, string BucketName, FolderName FolderName, SubFolderName? SubFolderName, string ContentType, bool CreateTumb = false)
        {

            ResourceURLs = new List<string>();
            try
            {
                ImageBucketName = BucketName;
                ImageBucketName += "/"+FolderName.ToString();
                if (SubFolderName.Value != UploadUtility.SubFolderName.None)
                {
                    ImageBucketName += "/" + SubFolderName.Value.ToString();
                }

              
                
                if (!string.IsNullOrWhiteSpace(FileName))
                {
                    this.FileName = FileName;
                }
                else { errorMsg = "filename is requried"; return; }

                Stream ThumbNail = CreateThumbnail(ObjectImage, ContentType, CreateTumb);

                // Main funtion to get the details and push request to upload on S3
                AwsS3.UploadS3Object(ImageBucketName, ObjectImage, this.FileName);
                // adds to generate the link to display or save on Database.
                ResourceURLs.Add(this.FileName);


                if (CreateTumb)
                {
                    FileThumbName = ThumbNameFromFileName(this.FileName);

                    AwsS3.UploadS3Object(ImageBucketName, ThumbNail, FileThumbName);
                    ResourceURLs.Add(FileThumbName);
                }
                
            }
            catch (Exception e)
            {

                errorMsg = e.Message;
            }
        }

        private static Stream CreateThumbnail(Stream Object, string ContentType, bool CreateThumb = false)
        {
            var stream = new System.IO.MemoryStream();
            if (CreateThumb)
            {
                // There is a GetThumbnailImage method that is simpler; however image quality is lower
                System.Drawing.Image imgOriginal = System.Drawing.Image.FromStream(Object);
                System.Drawing.Image imgThumb = FixedSize(imgOriginal, 150, 150);

                imgThumb.Save(stream, ImageFormatFromContentType(ContentType));
                imgThumb.Dispose();
                imgOriginal.Dispose();
            }
            return stream;
        }

        private static ImageFormat ImageFormatFromContentType(string ContentType)
        {
            // TODO: Add logic for additional image types, as appropriate for your site
            ImageFormat format;
            switch (ContentType)
            {
                case "image/bmp":
                    format = ImageFormat.Bmp;
                    break;
                case "image/gif":
                    format = ImageFormat.Gif;
                    break;
                case "image/pjpeg":
                case "image/jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case "image/png":
                    format = ImageFormat.Png;
                    break;
                default:
                    format = ImageFormat.Jpeg;
                    break;
            }
            return format;
        }

       

        
        public static string ThumbNameFromFileName(string fileName)
        {
            int i = fileName.LastIndexOf(".");
            string baseFileName = fileName.Substring(0, i);
            int iStartPos = i + 1;
            string fileExtension = fileName.Substring(iStartPos, fileName.Length - iStartPos);
            string newFileName = baseFileName + "_thumb." + fileExtension;
            return newFileName;
        }

        public static string RandomFileName(string fileName)
        {
            Random r = new Random();
            var RandomNumber = r.Next(0, 1000000);
            int i = fileName.LastIndexOf(".");
            string baseFileName = fileName.Substring(0, i);
            int iStartPos = i + 1;
            string fileExtension = fileName.Substring(iStartPos, fileName.Length - iStartPos);
            string newFileName = baseFileName + "_" + RandomNumber.ToString("D4") + "." + fileExtension;
            return newFileName;
        }


        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }


    }

    /// <summary>
    /// Document Class to upload on s3 
    /// it can be customize more as per needs.
    /// </summary>
    public class Document :IAwsS3
    {
        public string FileThumbName { get; set; }

        public string ImageBucketName { get; set; }
        public string errorMsg { get; set; }
        public string FileName { get; set; }
        public List<string> ResourceURLs { get; set; }



        private s3Service AwsS3 = new s3Service();

       
        public void UploadFileToServer(Stream Object, string FileName, string BucketName, FolderName FolderName, SubFolderName? SubFolderName, string ContentType)
        {
            try
            {
                ImageBucketName = BucketName;
                ImageBucketName += FolderName.ToString();
                if (SubFolderName.Value != UploadUtility.SubFolderName.None)
                {
                    ImageBucketName += "/" + SubFolderName.Value.ToString();
                }


                
                if (!string.IsNullOrWhiteSpace(FileName))
                {
                    this.FileName = FileName;
                }
                else { errorMsg = "filename is requried"; return; }

                AwsS3.UploadS3Object(ImageBucketName, Object, this.FileName);
                ResourceURLs.Add(this.FileName);
                 

            }
            catch (Exception e)
            {

                errorMsg = e.Message;
            }
        }



        public static string RandomFileName(string fileName)
        {
            Random r = new Random();
            var RandomNumber = r.Next(0, 1000000);
            int i = fileName.LastIndexOf(".");
            string baseFileName = fileName.Substring(0, i);
            int iStartPos = i + 1;
            string fileExtension = fileName.Substring(iStartPos, fileName.Length - iStartPos);
            string newFileName = baseFileName + "_" + RandomNumber.ToString("D4") + "." + fileExtension;
            return newFileName;
        }










    }

    //if there is new need to add more folders on the s3, here we can add more names.
    public enum FolderName
    {
        Application,
        PublicContent,
        PrivateContent
    }

    //second level of directory
    public enum SubFolderName
    {
        Gallary,
        Project,
        None

    }


    /// <summary>
    /// This class is to generate the links of files uploaded on server.
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// This Method can be use to translate the text coming from views.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetResource(string text)
        {
            return text;
        }

        /// <summary>
        /// Generate URL for the image uploaded on s3
        /// </summary>
        /// <param name="Buketname"></param>
        /// <param name="folderName"></param>
        /// <param name="subFolderName"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GenerateURL(string Buketname, FolderName folderName, SubFolderName subFolderName, string FileName)
        {
            //S3URL is coming from web.config.
            string ServiceURL = ConfigurationManager.AppSettings["S3URL"]; ;
            if (subFolderName != SubFolderName.None)
            {
                return ServiceURL +  folderName + "/" + subFolderName + "/" + FileName;
            }
            else
            {
                return ServiceURL  + folderName + "/" + FileName;
            }
        }
    }
}