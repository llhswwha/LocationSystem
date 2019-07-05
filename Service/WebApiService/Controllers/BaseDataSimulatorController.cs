using BLL;
using CommunicationClass.SihuiThermalPowerPlant;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DAL;
using DbModel.BaseData;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

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
            //BaseDataDb db = new BaseDataDb();
            //var list=db.users.AsNoTracking().ToList();
            //BaseTran<user> data =new BaseTran<user>(list);
            //if (list.Count==0)
            //{
            //    data = LoadFromFile<user>("users");
            //}
            var data = LoadFromFile<user>("users");
            return data;
        }

        private BaseTran<T> LoadFromFile<T>(string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\BaseData\\" + fileName + ".json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                BaseTran<T> data = JsonConvert.DeserializeObject<BaseTran<T>>(json);
                return data;
            }

            return null;
        }

        [Route("orgs")]
        public BaseTran<org> GetOrgList()
        {
            //BaseDataDb db = new BaseDataDb();
            //var list = db.orgs.AsNoTracking().ToList();
            //var data = new BaseTran<org>(list);
            //if (list.Count == 0)
            //{
            //    data = LoadFromFile<org>("orgs");
            //}
            var data = LoadFromFile<org>("orgs");
            return data;
        }

        [Route("zones")]
        public BaseTran<zone> GetZoneList()
        {
            //BaseDataDb db = new BaseDataDb();
            //var list = db.zones.AsNoTracking().ToList();
            //var data = new BaseTran<zone>(list);
            //if (list.Count == 0)
            //{
            //    data = LoadFromFile<zone>("zones");
            //}

            var data = LoadFromFile<zone>("zones");

            return data;
        }

        [Route("devices")]
        public BaseTran<device> GetDeviceList(Dictionary<string,string> paramList)
        {
            //BaseDataDb db = new BaseDataDb();
            //var list = db.devices.AsNoTracking().ToList();
            //var data = new BaseTran<device>(list);
            //if (list.Count == 0)
            //{
            //    data = LoadFromFile<device>("devices");
            //}
            var data = LoadFromFile<device>("devices");
            return data;
        }

        [Route("cards")]
        public BaseTran<cards> GetCardList()
        {
            //BaseDataDb db = new BaseDataDb();
            //var list = db.cards.AsNoTracking().ToList();
            //var data = new BaseTran<cards>(list);
            //if (list.Count == 0)
            //{
            //    data = LoadFromFile<cards>("cards");
            //}
            var data = LoadFromFile<cards>("cards");
            return data;
        }

        [Route("rt/sis/{tags}")]
        public BaseTran<sis> GetSis(string tags)
        {
            //BaseDataDb db = new BaseDataDb();
            var list = new List<sis>();
            string[] tagList = tags.Split(',');
            Bll bll = new Bll();
            foreach (var tag in tagList)
            {
               var monitor=bll.DevMonitorNodes.Find(p => p.TagName == tag);
                sis sis = new sis();
                sis.kks = tag;
                sis.t = DateTime.Now.ToStamp();
                if (monitor != null)
                {
                    sis.value = monitor.Value;
                    sis.unit = monitor.Unit;
                }

                if (string.IsNullOrEmpty(sis.value))
                {
                    sis.value = "-1";//代表模拟数据
                }
                list.Add(sis);
            }
            var data = new BaseTran<sis>(list);
            return data;
        }
    }
}

