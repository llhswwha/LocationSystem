using CommunicationClass.SihuiThermalPowerPlant;
using DAL;
using DbModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiService.Controllers
{
    /// <summary>
    /// 模拟光谱基础数据平台WebApi
    /// </summary>
    [RoutePrefix("datacase/api")]
    public class BaseDataSimulatorController : ApiController
    {

        [Route("users")]
        public BaseTran<user> GetUserList()
        {
            BaseDataDb db = new BaseDataDb();
            var list=db.users.AsNoTracking().ToList();
            BaseTran<user> data = new BaseTran<user>(list);
            return data;
        }

        [Route("orgs")]
        public BaseTran<org> GetOrgList()
        {
            BaseDataDb db = new BaseDataDb();
            var list = db.orgs.AsNoTracking().ToList();
            var data = new BaseTran<org>(list);
            return data;
        }

        [Route("zones")]
        public BaseTran<zone> GetZoneList()
        {
            BaseDataDb db = new BaseDataDb();
            var list = db.zones.AsNoTracking().ToList();
            var data = new BaseTran<zone>(list);
            return data;
        }

        [Route("devices")]
        public BaseTran<device> GetDeviceList(string types, string code, string name)
        {
            BaseDataDb db = new BaseDataDb();
            var list = db.devices.AsNoTracking().ToList();
            var data = new BaseTran<device>(list);
            return data;
        }

        [Route("cards")]
        public BaseTran<cards> GetCardList()
        {
            BaseDataDb db = new BaseDataDb();
            var list = db.cards.AsNoTracking().ToList();
            var data = new BaseTran<cards>(list);
            return data;
        }
    }
}

