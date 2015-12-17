using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using FileSystemAccess.Negotiators;

namespace FileSystemAccess
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Setting JsonContentNegotiator to always return the content type of "application/json"
            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Dynamic routing for file/folder paths as part of URL
            config.Routes.MapHttpRoute(
                name: "FileSystem",
                routeTemplate: "api/FileSystem/{*path}",
                defaults: new { controller = "FileSystem" }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}
