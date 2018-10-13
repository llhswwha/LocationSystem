using DbModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations;

namespace WebApiService.Controllers
{

    [RoutePrefix("api/area")]
    public class AreaController:ApiController
    {
        LocationService service=new LocationService();
        [Route("")]
        [Route("list")]
        public IList<PhysicalTopology> GetList()
        {
            return service.GetPhysicalTopologyList();
        }

        [Route("tree")]
        public PhysicalTopology GetTree()
        {
            return service.GetPhysicalTopologyTree();
        }

        [Route("tree/{id}")]
        public PhysicalTopology GetTree(string id)
        {
            return service.GetPhysicalTopologyTreeById(id);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<PhysicalTopology> GetListByName(string name)
        {
            return service.GetPhysicalTopologyListByName(name);
        }

        [Route("{pid}/children")]
        public IList<PhysicalTopology> GetListByPid(string pid)
        {
            return service.GetPhysicalTopologyListByPid(pid);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public PhysicalTopology GetEntity(string id)
        {
            return GetEntity(id, false);
        }

        [Route("")]
        [Route("{id}")]
        public PhysicalTopology GetEntity(string id,bool getChildren)
        {
            return service.GetPhysicalTopology(id, getChildren);
        }

        [Route]
        public PhysicalTopology Post(PhysicalTopology item)
        {
            return service.AddPhysicalTopology(item);
        }

        [Route("{id}")]
        public PhysicalTopology Post(string id,PhysicalTopology item)
        {
            return service.AddPhysicalTopology(id,item);
        }

        [Route]
        public PhysicalTopology Put(PhysicalTopology item)
        {
            return service.EditPhysicalTopology(item);
        }

        [Route("{id}")]
        [HttpDelete]
        public PhysicalTopology Delete(string id)
        {
            return service.RemovePhysicalTopology(id);
        }

        [Route("{id}/children")]
        [HttpDelete]
        public List<PhysicalTopology> DeleteChildren(string id)
        {
            return service.RemovePhysicalTopologyChildren(id);
        }
    }
}
