using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Location.Model;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IAreaService
    {
        #region 区域

        /// <summary>
        /// 查找区域列表，根据名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [OperationContract]
        IList<Area> FindAreas(string name);

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="mapId">地图Id</param>
        /// <returns></returns>
        [OperationContract]
        IList<Area> GetAreas(int mapId);

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Area GetArea(int id);

        /// <summary>
        /// 添加区域
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddArea(Area area);

        /// <summary>
        /// 编辑区域
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        [OperationContract]
        bool EditArea(Area area);

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteArea(int id);
        #endregion
    }
}
