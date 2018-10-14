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
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{

    [RoutePrefix("api/areas")]
    public class AreaController: ApiController, IAreaService
    {
        IAreaService service;

        public AreaController()
        {
            service = new AreaService();
        }

        [Route("")]
        [Route("list")]
        public IList<PhysicalTopology> GetList()
        {
            return service.GetList();
        }

        [Route("tree")]
        public PhysicalTopology GetTree()
        {
            return service.GetTree();
        }

        [Route("tree/{id}")]
        public PhysicalTopology GetTree(string id)
        {
            return service.GetTree(id);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<PhysicalTopology> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("{id}/children")]
        public IList<PhysicalTopology> GetListByPid(string id)
        {
            return service.GetListByPid(id);
        }

        [Route("{id}/parent")]
        public PhysicalTopology GetParent(string id)
        {
            return service.GetParent(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public PhysicalTopology GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("{id}")]
        public PhysicalTopology GetEntity(string id,bool getChildren)
        {
            return service.GetEntity(id, getChildren);
        }

        [Route]
        public PhysicalTopology Post(PhysicalTopology item)
        {
            return service.Post(item);
        }

        [Route("{pid}")]
        public PhysicalTopology Post(string pid,PhysicalTopology item)
        {
            return service.Post(pid, item);
        }

        [Route]
        public PhysicalTopology Put(PhysicalTopology item)
        {
            return service.Put(item);
        }

        [Route("{id}")]
        public PhysicalTopology Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("{id}/children")]
        public List<PhysicalTopology> DeleteChildren(string id)
        {
            return service.DeleteChildren(id);
        }
    }
}
