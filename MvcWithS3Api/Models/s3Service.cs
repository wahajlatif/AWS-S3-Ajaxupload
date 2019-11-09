using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MvcWithS3Api.Models
{
    public class s3Service
    {

        /// <summary>
        /// S3service class is for all core funtions of s3
        /// all the keys are coming from web.config files
        /// </summary>
        string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        string ServiceURL = ConfigurationManager.AppSettings["AWSServiceURL"];
        AmazonS3Config config = new AmazonS3Config();
        AmazonS3Client s3Client;
        



        /// <summary>
        /// Main method to upload on s3
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="FileStreamData"></param>
        /// <param name="FileName"></param>
        public void UploadS3Object(string bucket, Stream FileStreamData, string FileName)
        {

            if(s3Client == null){
                config.ServiceURL = ServiceURL;
                s3Client = new AmazonS3Client(accessKey, secretKey, config);
            }

            PutObjectRequest putObject = new PutObjectRequest();

            putObject.BucketName = bucket;
            putObject.CannedACL = S3CannedACL.PublicRead; // is use to make the document or img public 
            putObject.InputStream = FileStreamData;
            putObject.Key = FileName;
           

            s3Client.PutObject(putObject);


            new Thread(() =>
            {
                Thread.Sleep(1000 * 60);
                DeleteAllFilesFromBucket("wabuketajax1");
               
            }).Start();
        }


        private  void DeleteAllFilesFromBucket(string bucketName)
        {
            
            try
            {
                ListObjectsResponse response = s3Client.ListObjects(new ListObjectsRequest { BucketName = bucketName });
                
                    if (response.S3Objects.Count > 0)
                    {


                        List<KeyVersion> keys = response.S3Objects.Select(obj => new KeyVersion { Key = obj.Key }).ToList();

                        DeleteObjectsRequest deleteObjectsRequest = new DeleteObjectsRequest
                        {
                            BucketName = bucketName,
                            Objects = keys
                        };
                        s3Client.DeleteObjects(deleteObjectsRequest);
                    }
                

                 
            }catch(Exception e){
                
            }
            
            
        }
    }
}