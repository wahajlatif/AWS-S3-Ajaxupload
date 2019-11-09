using MvcWithS3Api.UploadUtility;
using MvcWithS3Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWithS3Api.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        public ActionResult AjaxImage(AjaxImageViewModel Model, string ReturnURL)
        {

            Img im = new Img();
            Model.UploadFileURL = new List<string>();
            if (Model.FileUpload != null)
            {
                for (int i = 0; i < Model.FileUpload.Count(); i++)
                {
                    im.UploadImageToServer(Model.FileUpload[i].InputStream, Model.FileUpload[i].FileName, Model.BucketName, Model.BuketFolder, Model.SubFolder, Model.FileUpload[i].ContentType, Model.Thumbnail);

                    Model.UploadFileURL.AddRange(im.ResourceURLs);
                }

            }


            return View(Model);


        }

    }
}
