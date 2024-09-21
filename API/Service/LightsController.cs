using newblinkinglight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace newblinkinglight.API.Service
{
    public class LightsController : ApiController
    {
        /// <summary>
        /// test gọi get
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            return Ok("Hello, World! from .NET Framework 4.7 Web API");
        }
        /// <summary>
        /// Hàm cảnh báo đèn
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult WarningControl([FromBody] LightParam light)
        {
            var res = new LightResult { Success = true };
            try
            {
                if (light != null)
                {
                    switch (light.WarningType)
                    {
                        case Common.EnumWarningType.None:
                            // không có trạng thái thì không xử lý
                            break;
                        case Common.EnumWarningType.OverHeat:
                            // cảnh báo quá nhiệt độ
                            // Todo start đèn cảnh báo
                            break;

                        case Common.EnumWarningType.RedBuzzsLeepTurnOff:
                            // cảnh báo RedBuzzsLeepTurnOff
                            Command.transfercommand("redbuzzsleepturnoff");

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Messagse = ex.ToString();
            }
            // Xử lý dữ liệu
            return Ok(res);
        }
    }
}
