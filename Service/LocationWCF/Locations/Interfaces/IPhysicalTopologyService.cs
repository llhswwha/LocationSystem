using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IPhysicalTopologyService
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<PhysicalTopology> GetPhysicalTopologyList();

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        PhysicalTopology GetPhysicalTopologyTree();

        [OperationContract]
        IList<PhysicalTopology> GetParkMonitorRange();

        [OperationContract]
        IList<PhysicalTopology> GetFloorMonitorRange();

        [OperationContract]
        IList<PhysicalTopology> GetFloorMonitorRangeById(int id);

        [OperationContract]
        bool EditMonitorRange(PhysicalTopology pt);

        [OperationContract]
        bool AddMonitorRange(PhysicalTopology pt);
    }
}
