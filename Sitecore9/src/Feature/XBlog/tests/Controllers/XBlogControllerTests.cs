using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Sitecore.Feature.XBlog.Controllers;
using Sitecore.Feature.XBlog.Repositories;

namespace Sitecore.Feature.XBlog.Tests.Controllers
{
    [TestClass]
    public class XBlogControllerTests
    {
        private static IBlogRepository _blogRepository;

        private static XBlogController GetController()
        {
            return new XBlogController(_blogRepository);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _blogRepository = Substitute.For<IBlogRepository>();
        }


        [TestMethod]
        public void Constructor_IsNotNull()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsNotNull(GetController());
        }

        // TODO: Implement unit tests for the _Name_Controller
    }
}
