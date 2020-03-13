using Newtonsoft.Json.Serialization;
using Owin;
using personal_site_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace personal_site_api
{
    public class Startup
    { 

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);
        }

        public void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }

        //need this to make web api work and to accept json
        //same as having a WebApiConfig class
        private void ConfigureWebApi(HttpConfiguration httpConfig)
        {
            //allow json
            var jsonFormatter = httpConfig.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //allow camelCase
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //make datetime look nicer when sending in json
            jsonFormatter.SerializerSettings.DateFormatString = "M/d/yyyy h:mm tt";

            //allow custom routes
            httpConfig.MapHttpAttributeRoutes();

            //set default api routes
            httpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}