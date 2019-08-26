using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.Nodes;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/physicalTopology")]
    public class PhysicalTopologyController : ApiController, IPhysicalTopologyService
    {
        protected IPhysicalTopologyService service;
        [Route("add")]
        [HttpPut]
        public PhysicalTopology AddMonitorRange(PhysicalTopology pt)
        {
            return service.AddMonitorRange(pt);
        }

        public PhysicalTopology AddPhysicalTopology(PhysicalTopology item)
        {
            return service.AddPhysicalTopology(item);
        }
        [Route("delete")]
        [HttpDelete]
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

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            return service.GetPhysicalTopology(id,getChildren);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyList(int view)
        {
            return service.GetPhysicalTopologyList(view);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            return service.GetPhysicalTopologyListByName(name);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string id)
        {
            return service.GetPhysicalTopologyListByPid(id);
        }

        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            return service.GetPhysicalTopologyTree(view);
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            return service.GetPhysicalTopologyTreeById(id);
        }

        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            return service.GetPhysicalTopologyTreeNode(view);
        }

        public IList<PhysicalTopology> GetSwitchAreas()
        {
            return service.GetSwitchAreas();
        }

        public PhysicalTopology RemovePhysicalTopology(string id)
        {
            return service.RemovePhysicalTopology(id);
        }
    }
}
