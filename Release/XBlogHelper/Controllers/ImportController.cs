using Sitecore;
using Sitecore.Configuration;
using Sitecore.Controllers;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace XBlogHelper.Controllers
{
    public class ImportController : Controller
    {
        [HttpPost]
        public JsonResult Upload()
        {
            SitecoreViewModelResult result = new SitecoreViewModelResult();
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                    if (hpf.ContentLength == 0 || !ValidateFile(hpf, result))
                        continue;
                    string fileName = Path.GetFileName(hpf.FileName);

                    result.Data = fileName;
                    
                    string path = StringUtil.EnsurePostfix('/', Sitecore.Configuration.Settings.DataFolder.ToString()) + fileName;
                    hpf.SaveAs(path);

                }
            }
            catch (Exception ex)
            {
                Log.Error("could not upload ", ex, this);
            }

            return result;
        }

        public static bool ValidateFile(HttpPostedFileBase file, SitecoreViewModelResult result)
        {
            if (true)
                return true;
        }
    }
}