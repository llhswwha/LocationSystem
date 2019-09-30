using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.LocationHistory.Data;
using Location.TModel.Location.Data;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/position")]
    public class PositionController : ApiController, IPositionService
    {
        protected IPositionService service;
        public PositionController()
        {
            service = new PositionService();
        }
        [Route("addList")]
        public bool AddU3DPosition(List<U3DPosition> pList)
        {
            return service.AddU3DPosition(pList);
        }
        [Route("")]
        public bool AddU3DPositions(List<U3DPosition> list)
        {
            return service.AddU3DPositions(list);
        }   
        [Route("")]
        public U3DPosition Delete(string id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<U3DPosition> DeleteListByPid(string pid)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition GetEntity(string id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition GetEntity(string id, bool getChildren)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<Pos> GetHistoryPositonData(int nFlag, string strName, string strName2, string strName3)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<Position> GetHistoryPositons()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3)
        {
            throw new NotImplementedException();
        }
        [Route("list/u3dBy/tagcode/{tagcode}/start/{start}/end/{end}")]
        public IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            return service.GetHistoryU3DPositonsByTime(tagcode,start,end);
        }
        [Route("")]
        public List<U3DPosition> GetList()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<U3DPosition> GetListByName(string name)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public List<U3DPosition> GetListByPid(string pid)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition GetParent(string id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<TagPosition> GetRealPositons()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public string GetStrs(int n)
        {
            throw new NotImplementedException();
        }

        [Route("")]
        public U3DPosition GetTree()
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition GetTree(string id)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition Post(U3DPosition item)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition Post(string pid, U3DPosition item)
        {
            throw new NotImplementedException();
        }
        [Route("")]
        public U3DPosition Put(U3DPosition item)
        {
            throw new NotImplementedException();
        }
    }
}
