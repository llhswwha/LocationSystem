using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.Nodes;
using LocationServices.Locations.Services;
using TModel.Location.AreaAndDev;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/physicalTopology")]
    public class PhysicalTopologyController : ApiController, IPhysicalTopologyService
    {
        protected IPhysicalTopologyService service;

        public PhysicalTopologyController()
        {
            service = new PhysicalTopologyService();
        }
        [Route("add")]
        [HttpPost]
        public PhysicalTopology AddMonitorRange(PhysicalTopology pt)
        {
            return service.AddMonitorRange(pt);
        }

        public PhysicalTopology AddPhysicalTopology(PhysicalTopology item)
        {
            return service.AddPhysicalTopology(item);
        }
        [Route("delete")]
        [HttpPost]
        public bool DeleteMonitorRange(PhysicalTopology pt)
        {
            return service.DeleteMonitorRange(pt);
        }
        [Route("edit")]
        [HttpPut]
        public bool EditMonitorRange(PhysicalTopology pt)
        {
            return service.EditMonitorRange(pt);
        }

        [Route("editDynamicRange")]
        [HttpPut]
        public TopologyToLocationCards EditDynamicRange(TopologyToLocationCards pt)
        {
            return service.EditDynamicRange(pt);
        }
        [Route("GetCardToAreaInfo")]
        [HttpGet]
        public List<DbModel.Location.Relation.LocationCardToArea> GetCardToAreaInfo()
        {
            return service.GetCardToAreaInfo();
        }

        [Route("GetAreaLocationCardById/{areaId}")]
        [HttpGet]
        public TopologyToLocationCards GetAreaLocationCardById(int areaId)
        {
            return service.GetAreaLocationCardById(areaId);
        }

    
        public PhysicalTopology EditPhysicalTopology(PhysicalTopology item)
        {
            return service.EditPhysicalTopology(item);
        }
        [Route("floor/list")]
        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            return service.GetFloorMonitorRange();
        }
        [Route("floor/list/{id}")]
        public IList<PhysicalTopology> GetFloorMonitorRangeById(int id)
        {
            return service.GetFloorMonitorRangeById(id);
        }
        [Route("park/list")]
        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            return service.GetParkMonitorRange();
        }
        [Route("")]
        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            return service.GetPhysicalTopology(id,getChildren);
        }
        [Route("")]
        public IList<PhysicalTopology> GetPhysicalTopologyList(int view)
        {
            return service.GetPhysicalTopologyList(view);
        }
        [Route("")]
        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            return service.GetPhysicalTopologyListByName(name);
        }
        [Route("")]
        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string id)
        {
            return service.GetPhysicalTopologyListByPid(id);
        }
        [Route("")]
        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            return service.GetPhysicalTopologyTree(view);
        }
        [Route("")]
        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            return service.GetPhysicalTopologyTreeById(id);
        }
        [Route("")]
        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            return service.GetPhysicalTopologyTreeNode(view);
        }
        [Route("")]
        public IList<PhysicalTopology> GetSwitchAreas()
        {
            return service.GetSwitchAreas();
        }
        [Route("")]
        public PhysicalTopology RemovePhysicalTopology(string id)
        {
            return service.RemovePhysicalTopology(id);
        }
    }
}
