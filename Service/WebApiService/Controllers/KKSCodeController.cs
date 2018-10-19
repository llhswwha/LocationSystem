using LocationServices.Locations.Services;
using System.Collections.Generic;
using System.Web.Http;
using TEntity = Location.TModel.Location.AreaAndDev.KKSCode;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/kks")]
    public class KKSCodeController : ApiController, IKKSCodeService
    {
        IKKSCodeService service;

        public KKSCodeController()
        {
            service = new KKSCodeService();
        }


        [Route("")]
        [Route("list")]
        public IList<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("search")]
        public IList<TEntity> GetMainType(string mainType)
        {
            return service.GetMainType(mainType);
        }

        [Route("search")]
        public IList<TEntity> GetSubType(string subType)
        {
            return service.GetSubType(subType);
        }

        [Route("search")]
        public IList<TEntity> GetParentCode(string parentCode)
        {
            return service.GetParentCode(parentCode);
        }

        //单独一棵树
        [Route("tree")]
        public List<TEntity> GetTree()
        {
            return service.GetTree();
        }

        [Route("tree/{id}")]
        public TEntity GetTree(int id)
        {
            return service.GetTree(id);
        }

        [Route("{id}")]
        public TEntity GetEntity(int id)
        {
            return service.GetEntity(id);
        }

        [Route("")]//search/?name=主
        public TEntity GetEntityByCode(string code)
        {
            return service.GetEntityByCode(code);
        }
    }
}
