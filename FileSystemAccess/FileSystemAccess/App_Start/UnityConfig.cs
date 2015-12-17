using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace FileSystemAccess
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            Bootstrapper.Bootstrapper.Initialize(container);
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}