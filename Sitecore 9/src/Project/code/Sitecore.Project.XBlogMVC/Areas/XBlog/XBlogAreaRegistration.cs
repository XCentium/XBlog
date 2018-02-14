using System.Web.Mvc;

namespace Sitecore.Project.XBlogMVC.Areas.XBlog
{
    public class XBlogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "XBlog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "XBlog_default",
                "XBlog/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}