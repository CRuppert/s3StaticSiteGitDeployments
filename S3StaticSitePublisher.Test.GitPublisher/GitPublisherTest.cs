using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using s3StaticSitePublisher.GitPublisher;

namespace S3StaticSitePublisher.Test.GitPublisher
{
    [TestFixture]
    public class GitPublisherTest
    {
        //[Test]
        //public void CanCheckoutFromPrivateRepoWithCredentials()
        //{
        //    var pub = new Publisher();

        //    string tempDirPath = string.Empty;
        //    var r = pub.CloneRepo("https://path.to.repo.git", out tempDirPath, "user@mail.com", "password");

        //    Assert.IsNotEmpty(tempDirPath);
        //}

        [Test]
        public void CanCheckoutFromPublicRepoWithoutCredentials()
        {
            var pub = new Publisher();

            string tempDirPath = string.Empty;
            var r = pub.CloneRepo("https://path.to.repo.git", out tempDirPath);

            Assert.IsNotEmpty(tempDirPath);
        }

        //[Test]
        //public void TempDirectoryIsFilled()
        //{
        //    var pub = new Publisher();

        //    string tempDirPath = string.Empty;
        //    var r = pub.CloneRepo("https://path.to.repo.git", out tempDirPath, "gituser@mail.com", "password");

        //    Assert.IsNotEmpty(tempDirPath);

        //    Assert.Greater(Directory.GetFiles(tempDirPath).Count(), 0);
        //    try
        //    {
        //        Directory.Delete(tempDirPath, true);
        //    }catch{/*  */}
        //}

        //[Test]
        //public void CanUploadSite()
        //{
        //    var pub = new Publisher();

        //    string tempDirPath = string.Empty;
        //    var r = pub.CloneRepo("https://path.to.repo.git", out tempDirPath, "user@mail.com", "password");

        //    pub.UploadFiles(tempDirPath, "awsKey", "awsSecret", "testGitDeploy");
        //    //Directory.Delete(tempDirPath, true);
        //}

    }
}
