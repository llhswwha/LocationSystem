using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.Data;
using TEntity = Location.TModel.Location.Data.TagPosition;
using Location.TModel.LocationHistory.Data;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/pos")]
    public class PositionController : ApiController, IPosService
    {
        IPosService service;
        public PositionController()
        {
            service = new PosService();
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("list")]
        public IList<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("history")]
        public IList<Position> GetHistoryList()
        {
            return service.GetHistoryList(null,null,null,null);
        }

        [Route("history")]
        public IList<Position> GetHistoryList(string start, string end)
        {
            return service.GetHistoryList(start, end,null,null);
        }

        [Route("history")]
        public IList<Position> GetHistoryList(string start, string end, string tag, string person)
        {
            return service.GetHistoryList(start, end, tag, person);
        }


        [Route("history")]
        public IList<Position> GetHistoryListByTag(string start, string end,string tag)
        {
            return service.GetHistoryList(start, end, tag, null);
        }

        [Route("history")]
        public IList<Position> GetHistoryListByPerson(string start, string end, string person)
        {
            return service.GetHistoryList(start, end, null, person);
        }


        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
    }
}
