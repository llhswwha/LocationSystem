using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Interfaces;
using TModel.Location.AreaAndDev;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/picture")]
    public class PictureController : ApiController, IPictureService
    {
        protected IPictureService service;
        [Route("")]
        public bool EditPictureInfo(Picture pc)
        {
            return service.EditPictureInfo(pc);
        }
        [Route("byte/name/{strPictureName}")]
        public byte[] GetHomePageByName(string strPictureName)
        {
            return service.GetHomePageByName(strPictureName);
        }
        [Route("list/all")]
        public List<string> GetHomePageNameList()
        {
            return service.GetHomePageNameList();
        }
        [Route("")]
        public Picture GetPictureInfo(string strPictureName)
        {
            return service.GetPictureInfo(strPictureName);
        }
    }
}
