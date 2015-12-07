namespace WebAPI
{
    using System.Net.Http.Formatting;
    using System.Web.Http;

    using Newtonsoft.Json;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter
                                      {
                                          SerializerSettings = new JsonSerializerSettings
                                                                   {
                                                                       TypeNameHandling = TypeNameHandling.Objects
                                                                   }
                                      });
            
            config.MapHttpAttributeRoutes();
            
            appBuilder.UseWebApi(config);
        }
    }
}