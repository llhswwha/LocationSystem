using DbModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiService.Controllers
{
    public class AreaController:ApiController
    {
        public Task<IEnumerable<Area>> GetAsync()
        {
            BLL.Bll bll = new BLL.Bll(false,false,false,false);
            var list1 = bll.Areas.ToList();
            //var list2 = list1.ToTModel();
            return Task.FromResult(list1.AsEnumerable());
        }
    }
}
