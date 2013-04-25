using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace s3StaticSitePublisher.AppDomain
{
    public class BuildJobData
    {
        [BsonId]
        public Guid Id { get; set; }

        public string GitRepositoryUrl { get; set; }
        public string GitUsername { get; set; }
        public string GitPassword { get; set; }

        public string AmazonS3Bucket { get; set; }
        public string AmazonKey { get; set; }
        public string AmazonSecretKey { get; set; }
    }
}
