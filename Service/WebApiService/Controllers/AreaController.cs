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
    public class AreaController:ApiController
    {
        LocationService service=new LocationService();
        public IList<PhysicalTopology> GetList()
        {
            return service.GetPhysicalTopologyList();
        }

        public PhysicalTopology GetTree()
        {
            return service.GetPhysicalTopologyTree();
        }

        public PhysicalTopology GetTree(string id)
        {
            return service.GetPhysicalTopologyTreeById(id);
        }

        public IList<PhysicalTopology> GetListByName(string name)
        {
            return service.GetPhysicalTopologyListByName(name);
        }

        public IList<PhysicalTopology> GetListByPid(string pid)
        {
            return service.GetPhysicalTopologyListByPid(pid);
        }

        public PhysicalTopology GetEntity(string id)
        {
            return service.GetPhysicalTopology(id);
        }

        public PhysicalTopology Post(PhysicalTopology item)
        {
            return service.AddPhysicalTopology(item);
        }

        public PhysicalTopology Put(PhysicalTopology item)
        {
            return service.EditPhysicalTopology(item);
        }

        public PhysicalTopology Delete(string id)
        {
            return service.RemovePhysicalTopology(id);
        }
    }
}
