using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location.Data;
using Location.TModel.LocationHistory.Data;
using BLL;
using LocationServices.Locations.Services;
using LocationWCFService.ServiceHelper;
using LocationServices.Converters;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IPositionService : ITreeEntityService<U3DPosition>
    {
        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        IList<TagPosition> GetRealPositons();

        IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes);

        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        IList<Position> GetHistoryPositons();

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <returns></returns>
        IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end);

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id列表
        /// </summary>
        /// <returns></returns>
        IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end);
        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end);

        /// <summary>
        /// 测试数据量
        /// </summary>
        /// <returns></returns>
        string GetStrs(int n);

        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        /// <returns></returns>
        bool AddU3DPosition(List<U3DPosition> pList);

        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        /// <returns></returns>
        //[OperationContract(IsOneWay = true)]
        bool AddU3DPositions(List<U3DPosition> list);

        /// <summary>
        /// 获取标签3D历史位置
        /// </summary>
        /// <returns></returns>
        IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end);

        //获取历史位置信息统计
        IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3);

        IList<Pos> GetHistoryPositonData(int nFlag, string strName, string strName2, string strName3);
    }

    public class PositionService : IPositionService
    {
        public static U3DPositionSP u3dositionSP;
        private Bll db;
        public PositionService()
        {
            db = Bll.NewBllNoRelation();
        }
        public bool AddU3DPosition(List<U3DPosition> pList)
        {
            try
            {
                //if (list == null || list.Count == 0) return;
                if (u3dositionSP == null)
                {
                    u3dositionSP = new U3DPositionSP();
                }
                u3dositionSP.AddU3DPositions(pList.ToDbModel());
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("AddU3DPosition", ex);
                return false;
            }
        }

        public bool AddU3DPositions(List<U3DPosition> list)
        {
            try
            {
                //if(list==null|| list.Count==0)return;
                //Log.Info("AddU3DPositions");
                if (u3dositionSP == null)
                {
                    u3dositionSP = new U3DPositionSP();
                }
                u3dositionSP.AddU3DPositions(list.ToDbModel());
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.DbGet, "AddU3DPositions", "Exception:" + ex);
                return false;
            }
        }  
        public U3DPosition Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<U3DPosition> DeleteListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        public U3DPosition GetEntity(string id)
        {
            throw new NotImplementedException();
        }

        public U3DPosition GetEntity(string id, bool getChildren)
        {
            throw new NotImplementedException();
        }

        public IList<Pos> GetHistoryPositonData(int nFlag, string strName, string strName2, string strName3)
        {
            throw new NotImplementedException();
        }

        public IList<Position> GetHistoryPositons()
        {
            var hisService = new PosHistoryService();
            var list = hisService.GetHistory();
            hisService.Dispose();
            return list;
        }

        public IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end)
        {
            var hisService = new PosHistoryService();
            var list = hisService.GetHistoryByPerson(personnelID, start, end);
            hisService.Dispose();
            return list;
        }

        public IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            var hisService = new PosHistoryService();
            var list = hisService.GetHistoryByPersonAndArea(personnelID, topoNodeIds, start, end);
            hisService.Dispose();
            return list;
        }

        public IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            var hisService = new PosHistoryService();
            var list = hisService.GetHistoryByTag(tagcode, start, end);
            hisService.Dispose();
            return list;
        }

        public IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3)
        {
            DateTime dt1 = DateTime.Now;

            var hisService = new PosHistoryService();

            IList<PositionList> send = hisService.GetHistoryPositonStatistics(nFlag, strName, strName2, strName3);
            DateTime dt2 = DateTime.Now;

            //string xml = XmlSerializeHelper.GetXmlText(send);

            var time = dt2 - dt1;

            hisService.Dispose();
            return send;
        }

        public IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            var hisService = new PosHistoryService();
            var list = hisService.GetHistoryU3DPositonsByTime(tagcode, start, end);
            hisService.Dispose();
            return list;
        }

        public List<U3DPosition> GetList()
        {
            throw new NotImplementedException();
        }

        public IList<U3DPosition> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<U3DPosition> GetListByPid(string pid)
        {
            throw new NotImplementedException();
        }

        public U3DPosition GetParent(string id)
        {
            throw new NotImplementedException();
        }

        public IList<TagPosition> GetRealPositons()
        {
            return new PosService(db).GetList();
        }

        public IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes)
        {
            return new PosService(db).GetRealPositonsByTags(tagCodes);
        }

        public string GetStrs(int n)
        {
            throw new NotImplementedException();
        }

        public U3DPosition GetTree()
        {
            throw new NotImplementedException();
        }

        public U3DPosition GetTree(string id)
        {
            throw new NotImplementedException();
        }

        public U3DPosition Post(U3DPosition item)
        {
            throw new NotImplementedException();
        }

        public U3DPosition Post(string pid, U3DPosition item)
        {
            throw new NotImplementedException();
        }

        public U3DPosition Put(U3DPosition item)
        {
            throw new NotImplementedException();
        }
    }
}
