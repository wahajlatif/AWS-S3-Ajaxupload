using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWithS3Api.UploadUtility
{
    interface IAwsS3
    {

         string FileThumbName { get; set; }

         string ImageBucketName { get; set; }
         string errorMsg { get; set; }
         string FileName { get; set; }
         List<string> ResourceURLs { get; set; }

    }
}
