using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using s3StaticSitePublisher.AppDomain;
using s3StaticSitePublisher.GitPublisher;

namespace s3StaticSitePublisher.Web.Controllers
{
    public class DeployApiController : ProjectsControllerBase
    {
        private MongoCollection<BuildJobData> projs;
        public DeployApiController()
            : base()
        {
            projs = mongoDB.GetCollection<BuildJobData>("BuildJobData");
        }



        //
        // GET: /Api/
        //this should be put on a new thread initiated through signalR. As project size increases this will be important.
        //For now this functions.
        [HttpPost]
        public ActionResult Deploy(Guid id)
        {
            var p = projs.FindOne(Query.EQ("_id", id));
            
            if (p != null)
            {
                

                string workingPath = null;
                var wc = ConfigurationManager.AppSettings;
                if (wc.AllKeys.Contains("workingDirectory"))
                {
                    workingPath = wc["workingDirectory"];
                }
                
                
                var publisher = new Publisher(workingPath);

                string path = string.Empty;
                publisher.CloneRepo(p.GitRepositoryUrl, out path, p.GitUsername, p.GitPassword);
                publisher.UploadFiles(path, p.AmazonKey, p.AmazonSecretKey, p.AmazonS3Bucket);
                try
                {
                    Directory.Delete(path, true);
                } catch{EventLog.WriteEntry("Application", "Unable to delete temp directory: "+ path, EventLogEntryType.Warning);}
            }
            
            return new EmptyResult();
        }

    }
}
