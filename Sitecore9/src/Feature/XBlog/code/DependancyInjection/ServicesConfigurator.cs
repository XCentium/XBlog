using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.XBlog.Controllers;
using Sitecore.Feature.XBlog.Repositories;

namespace Sitecore.Feature.XBlog.DependancyInjection
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBlogRepository, BlogRepository>();
            serviceCollection.AddTransient(typeof(XBlogController));
        }
    }
}