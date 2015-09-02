using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EcolinxCMS.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var json = config.Formatters.JsonFormatter;
            //var resolver = new DefaultContractResolver();
            //resolver.IgnoreSerializableInterface = false;
            //resolver.IgnoreSerializableAttribute = false;
            
            json.SerializerSettings.Culture = new System.Globalization.CultureInfo("pt-BR");
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //json.SerializerSettings.ContractResolver = resolver;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
