
using Location.BLL.Tool;
using Location.TModel.Location.Person;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TEntity = Location.TModel.Location.Person.Department;

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
        public List<TEntity> GetList()
        {
            return service.GetList();
        }
        public TEntity GetTree(List<Personnel> leafNodes)
        {
            return service.GetTree(leafNodes);
        }

        public static int count;
        [Route("tree")]
        public TEntity GetTree()
        {

            Log.Info(LogTags.WebApi, string.Format("[{0}]DepartmentController.GetTree:{1}", Request.GetClientIpAddress(), count));
            count++;
            return service.GetTree();
        }

        [Route("tree")]
        public TEntity GetTree(int view)
        {
            Log.Info(LogTags.WebApi, string.Format("[{0}]DepartmentController.GetTree view={1}:{2}", Request.GetClientIpAddress(), view, count));
            count++;
            return service.GetTree(view);
        }

        [Route("tree/{id}")]
        public TEntity GetTree(string id)
        {
            return service.GetTree(id);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("{id}/children")]
        public List<TEntity> GetListByPid(string id)
        {
            return service.GetListByPid(id);
        }

        [Route("{id}/parent")]
        public TEntity GetParent(string id)
        {
            return service.GetParent(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("{id}")]
        public TEntity GetEntity(string id, bool getChildren)
        {
            return service.GetEntity(id, getChildren);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route("{pid}")]
        public TEntity Post(string pid, TEntity item)
        {
            return service.Post(pid, item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("{id}/children")]
        public IList<TEntity> DeleteListByPid(string id)
        {
            return service.DeleteListByPid(id);
        }
    }
}
