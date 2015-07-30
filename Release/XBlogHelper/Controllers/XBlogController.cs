using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XBlogHelper.Import;

namespace XBlogHelper.Controllers
{
    public class XBlogController : Controller
    {
        public class XBCreator
        {
            public string blogName { get; set; }
            public string blogParent { get; set; }
            public string blogType { get; set; }
            public string uploadFile { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Create(string xBlogData)
        {
            try
            {
                xBlogData = xBlogData.Replace("[","").Replace("]","");

                if (string.IsNullOrEmpty(xBlogData))
                    return Json(String.Format(""));

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                XBCreator x = serializer.Deserialize<XBCreator>(xBlogData);

                BlogCreator.CreateBlog(x.blogName, x.blogType, x.uploadFile, x.blogParent);

                return Json(String.Format("success"));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                return Json(String.Format(ex.Message));
            }
        }
    }
}
