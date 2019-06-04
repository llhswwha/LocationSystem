using System.Collections.Generic;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
using TModel.Location.Nodes;
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
using System.Net.Http;
using System.Net;
using System.Text;
using DbModel.Location.AreaAndDev;
using TModel.Location.AreaAndDev;
using Location.BLL.Tool;

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
        public List<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("persons")]
        [Route("list/persons")]
        public IList<TEntity> GetListWithPerson()
        {
            return service.GetListWithPerson();
        }

        [Route("tree/detail")]
        public TEntity GetTree(int view)
        {
            return service.GetTree(view);
        }

        [Route("tree/detail")]
        public TEntity GetTree()
        {
            return service.GetTree();
        }

        [Route("tree")]
        public AreaNode GetBasicTree()
        {
            return service.GetBasicTree(0);
        }

        [Route("tree")]
        public AreaNode GetBasicTree(int view)
        {
            Log.Info(string.Format("[{0}]AreaController.GetBasicTree view={1}:{2}", Request.GetClientIpAddress(),view, count));
            count++;
            return service.GetBasicTree(view);
        }

        [Route("tree/withDev")]
        public TEntity GetTreeWithDev(bool containCAD = false)
        {
            return service.GetTreeWithDev(containCAD);
        }

        [Route("tree/withPerson")]
        public TEntity GetTreeWithPerson()
        {
            return service.GetTreeWithPerson();
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
        public TEntity GetEntity(string id,bool getChildren)
        {
            return service.GetEntity(id, getChildren);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route("{pid}")]
        public TEntity Post(string pid,TEntity item)
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

        [Route("{pid}/children")]
        public IList<TEntity> DeleteListByPid(string pid)
        {
            return service.DeleteListByPid(pid);
        }

        [Route("svg")]
        public HttpResponseMessage GetAreaSvgXml(int Id)
        {
            System.DateTime dt = System.DateTime.Now;
            AreaService asi = new AreaService();
            string strXml = "";
            strXml = asi.GetAreaSvgXml(Id);

            if (strXml == "")
            {
                strXml = "<svg id=\"厂区\" width=\"100%\" height=\"100%\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" > ";
                strXml += "<defs><style>.cls-1{fill:none;stroke:#4d9fb5;}</style></defs> </svg>";

            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(strXml, Encoding.UTF8, "text/xml")
            };

            System.DateTime dt2 = System.DateTime.Now;
            return result;
        }


        static int count = 0;
        [Route("statistics")]
        public AreaStatistics GetAreaStatistics(int id)
        {
            Log.Info(string.Format("[{0}]AreaController.GetAreaStatistics:{1}", Request.GetClientIpAddress(), count));
            count++;
            return service.GetAreaStatistics(id);
        }

        [Route("getArea")]
        public Area GetAreaById(int id)
        {
            return service.GetAreaById(id);
        }

        [Route("getPoints")]
        public AreaPoints GetPoints(string areaId)
        {
            return service.GetPoints(areaId);
        }

        [Route("getPointsByPid")]
        public List<AreaPoints> GetPointsByPid(string pid)
        {
            return service.GetPointsByPid(pid);
        }
    }
}
