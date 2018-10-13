using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
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
        [WebGet(UriTemplate = "/area", ResponseFormat = WebMessageFormat.Json)]
        IList<PhysicalTopology> GetPhysicalTopologyList();

        [WebGet(UriTemplate = "/area/{id}", ResponseFormat = WebMessageFormat.Json)]
        PhysicalTopology GetPhysicalTopology(string id);

        [WebGet(UriTemplate = "/area/search?name={name}", ResponseFormat = WebMessageFormat.Json)]
        IList<PhysicalTopology> GetPhysicalTopologyListByName(string name);

        [WebGet(UriTemplate = "/area/{id}/children", ResponseFormat = WebMessageFormat.Json)]
        IList<PhysicalTopology> GetPhysicalTopologyListByPid(string id);

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/area/tree", ResponseFormat = WebMessageFormat.Json)]
        PhysicalTopology GetPhysicalTopologyTree();

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/area/tree/{id}", ResponseFormat = WebMessageFormat.Json)]
        PhysicalTopology GetPhysicalTopologyTreeById(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/area", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PhysicalTopology AddPhysicalTopology(PhysicalTopology item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/area", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PhysicalTopology EditPhysicalTopology(PhysicalTopology item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/area/{id}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PhysicalTopology RemovePhysicalTopology(string id);

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
