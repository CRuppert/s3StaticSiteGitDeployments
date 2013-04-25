using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s3StaticSitePublisher.AppDomain;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace s3StaticSitePublisher.Web.Controllers
{
    [Authorize]
    public class ProjectsController : ProjectsControllerBase
    {
        private MongoCollection<BuildJobData> projs;
        public ProjectsController()
        {
            projs = mongoDB.GetCollection<BuildJobData>("BuildJobData");
        }
        //
        // GET: /Projects/

        public ActionResult Index()
        {
           var projects = projs.FindAll().ToList();
            
            return View(projects);
        }

        public ActionResult Details(Guid id)
        {
            var p = projs.FindOne(Query.EQ("_id", id));

            return View(p);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BuildJobData model)
        {
            projs.Insert(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var p = projs.FindOne(Query.EQ("_id", id));
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Guid id, BuildJobData model)
        {
            projs.Save(model);
            return RedirectToAction("Index");
        }
    }
}
