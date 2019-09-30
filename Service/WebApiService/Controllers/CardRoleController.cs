using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Location.Authorizations;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/cardrole")]
    public class CardRoleController : ApiController, ICardRoleService
    {
        protected ICardRoleService service;

        public CardRoleController()
        {
            service = new CardRoleService();
        }
        [Route("add")]
        [HttpPost]
        public int AddCardRole(CardRole p)
        {
            return service.AddCardRole(p);
        }
        [Route("delete")]
        [HttpDelete]
        public bool DeleteCardRole(int id)
        {
            return service.DeleteCardRole(id);
        }
        [Route("edit")]
        [HttpPut]
        public bool EditCardRole(CardRole p)
        {
            return service.EditCardRole(p);
        }
        [Route("detail/{id}")]
        public CardRole GetCardRole(int id)
        {
            return service.GetCardRole(id);
        }
        [Route("list")]
        public List<CardRole> GetCardRoleList()
        {
            return service.GetCardRoleList();
        }
    }
}
