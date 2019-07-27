using System;
using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.Data;
using Location.TModel.LocationHistory.Data;
namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IPositionService
    {
        /// <summary>
        /// 获取标签实时位置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<TagPosition> GetRealPositons();

        [OperationContract]
        IList<TagPosition> GetRealPositonsByTags(List<string> tagCodes);

        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Position> GetHistoryPositons();

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end);

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end);
        /// <summary>
        /// 获取标签历史位置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end);

        /// <summary>
        /// 测试数据量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetStrs(int n);

        ///// <summary>
        ///// 测试用
        ///// </summary>
        ///// <param name="pos"></param>
        //[OperationContract]
        //void AddU3DPos(UPos pos);

        ///// <summary>
        ///// 3D保存历史数据
        ///// </summary>
        ///// <returns></returns>
        //[OperationContract]
        ////[OperationContract(IsOneWay = true)]
        //void AddU3DPosition(U3DPosition p);

        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void AddU3DPosition(List<U3DPosition> pList);

        /// <summary>
        /// 3D保存历史数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[OperationContract(IsOneWay = true)]
        void AddU3DPositions(List<U3DPosition> list);

        /// <summary>
        /// 获取标签3D历史位置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end);

        //获取历史位置信息统计
        [OperationContract]
        IList<PositionList> GetHistoryPositonStatistics(int nFlag, string strName, string strName2, string strName3);

        [OperationContract]
        IList<Pos> GetHistoryPositonData(int nFlag, string strName, string strName2, string strName3);

    }
}
