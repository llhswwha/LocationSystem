
using Location.TModel.Location.Person;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/deps")]
    public class DepartmentController : ApiController,IDepartmentService
    {
        IDepartmentService service;

        public DepartmentController()
        {
            service = new DepartmentService();
        }

        [Route("")]
        [Route("list")]
        public IList<Department> GetList()
        {
            return service.GetList();
        }
        public Department GetTree(List<Personnel> leafNodes)
        {
            return service.GetTree(leafNodes);
        }
        [Route("tree")]
        public Department GetTree()
        {
            return service.GetTree();
        }

        [Route("tree/{id}")]
        public Department GetTree(string id)
        {
            return service.GetTree(id);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<Department> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("{id}/children")]
        public IList<Department> GetListByPid(string id)
        {
            return service.GetListByPid(id);
        }

        [Route("{id}/parent")]
        public Department GetParent(string id)
        {
            return service.GetParent(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public Department GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("{id}")]
        public Department GetEntity(string id, bool getChildren)
        {
            return service.GetEntity(id, getChildren);
        }

        [Route]
        public Department Post(Department item)
        {
            return service.Post(item);
        }

        [Route("{pid}")]
        public Department Post(string pid, Department item)
        {
            return service.Post(pid, item);
        }

        [Route]
        public Department Put(Department item)
        {
            return service.Put(item);
        }

        [Route("{id}")]
        public Department Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("{id}/children")]
        public IList<Department> DeleteListByPid(string id)
        {
            return service.DeleteListByPid(id);
        }
    }
}
