using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MvcWithS3Api.UploadUtility;
using MvcWithS3Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWithS3Api.Controllers
{
    public class HomeController : Controller
    {
    

        public ActionResult Index()
        {

            return View();
        }
        
        

    }
}
