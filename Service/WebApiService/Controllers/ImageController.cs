using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BLL;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/image")]
    public class ImageController:ApiController
    {
        public HttpResponseMessage Get(string name)
        {
            //string name = "顶视图";
            Bll bll = new Bll();
            var picture =bll.Pictures.Find(name);
            if (picture != null)
            {
                //从图片中读取流
                var imgStream = new MemoryStream(picture.Info);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(imgStream)
                    //或者
                    //Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return resp;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(string.Format("未找到图片:{0}",name), Encoding.UTF8, "text/json")
                };
                return result;

            }
        }
    }
}
