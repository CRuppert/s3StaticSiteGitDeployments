using System.Configuration;
using System.Web.Mvc;
using MongoDB.Driver;

namespace s3StaticSitePublisher.Web.Controllers
{
    public class ProjectsControllerBase : Controller
    {
        protected MongoDatabase mongoDB;

        public ProjectsControllerBase()
        {
            var wc = ConfigurationManager.AppSettings;
            var mongoc = new MongoClient(wc["connectionString"]);
            this.mongoDB = mongoc.GetServer().GetDatabase(wc["database"]);
            
        }
    }
}