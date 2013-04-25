using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using LibGit2Sharp;

namespace s3StaticSitePublisher.GitPublisher
{
    public class Publisher
    {
        private string checkoutDirectory = string.Empty;
        public Publisher(string checkoutPath = null)
        {
            checkoutDirectory = checkoutPath ?? Path.GetTempPath();
        }

        public Repository CloneRepo(string srcUrl, out string pathToCode, string username = null, string pass = null)
        {
            string checkoutPath = string.Format("{0}gitCheckout-{1}", checkoutDirectory, DateTime.Now.Ticks);
        
            var tempDir = Directory.CreateDirectory(checkoutPath);
            
            Credentials creds = null;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass))
            {
                creds = new Credentials() {Username = username, Password = pass};
            }

            var repo = Repository.Clone(srcUrl, checkoutPath, false, true, null, null, null, creds);
            pathToCode = repo.Info.WorkingDirectory;
            return repo;
        }

        public void UploadFiles(string path, string accessKeyID, string secretAccessKeyID, string bucket)
        {
            using (var client = AWSClientFactory.CreateAmazonS3Client(
                accessKeyID, secretAccessKeyID))
            {
                WalkAndUpload(path, bucket, client);
            }
        }

        public void WalkAndUpload(string path, string bucket, AmazonS3 client)
        {

            foreach (var f in Directory.GetFiles(path))
            {
                var fi = new FileInfo(f);
                if (fi.Name.StartsWith("."))
                {
                    continue;
                } //ignore ".gitignore"

                string fileToPush = f;
                string destinationName =
                    Regex.Split(fileToPush, @"gitCheckout-[\d]*")[1].Replace("\\", "/").TrimStart("/".ToCharArray());

                var poR = new PutObjectRequest()
                    {
                        BucketName = bucket,
                        FilePath = fileToPush,
                        Key = destinationName,
                        Timeout = -1,
                        ReadWriteTimeout = 300000,
                        CannedACL = S3CannedACL.PublicRead
                    };
                var res = client.PutObject(poR);

            }


            foreach (var d in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(d);
                if (di.Name.StartsWith("."))
                {
                    continue;
                } //ignore ".git"
                WalkAndUpload(d, bucket, client);
            }
        }
    }
}
