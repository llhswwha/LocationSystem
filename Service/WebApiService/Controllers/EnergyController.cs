using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiLib.Clients.OpcCliect;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/energy")]
    public   class EnergyController: ApiController, IEnergyService
    {
        protected IEnergyService service;

        public EnergyController()
        {
            service = new EnergyService();
        }
        /// <summary>
        /// 全厂供热流量
        /// </summary>
        /// <returns></returns>
        [Route("GetAllGRLL")]
        public string GetAllGRLL()
        {
            return service.GetAllGRLL();
        }

        /// <summary>
        /// 全场发电量
        /// </summary>
        /// <returns></returns>
        [Route("GetAllPowerGeneration")]
        public string GetAllPowerGeneration()
        {
            return service.GetAllPowerGeneration();
        }
        /// <summary>
        /// 全厂天然气流量
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTRQLL")]
        public string GetAllTRQLL()
        {
            return service.GetAllTRQLL();
        }
        /// <summary>
        /// 各机组负荷列表
        /// </summary>
        /// <returns></returns>
        [Route("GetEveryCrewFH")]
        public List<SisData> GetEveryCrewFH()
        {
            return service.GetEveryCrewFH();
        }
        /// <summary>
        /// 各机组NOx排放量
        /// </summary>
        /// <returns></returns>
         [Route("GetNOPFL")]
        public List<SisData> GetNOPFL()
        {
            return service.GetNOPFL();
        }
        /// <summary>
        /// 根据测点获取测点数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [Route("list/SisData")]
        public List<SisData> GetSisDataList([FromUri]string[] tags)
        {
            return service.GetSisDataList(tags);
        }
    }
}
