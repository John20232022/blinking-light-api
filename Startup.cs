using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;


namespace newblinkinglight
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Cấu hình Web API
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            // Định nghĩa route mặc định cho Web API
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Cấu hình để luôn trả về JSON
            var jsonFormatter = config.Formatters.JsonFormatter;

            // Đặt tên thuộc tính theo camelCase (tuỳ chọn)
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Loại bỏ XML Formatter để chỉ trả về JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);


            // Sử dụng Web API với OWIN
            appBuilder.UseWebApi(config);
        }
    }
}
