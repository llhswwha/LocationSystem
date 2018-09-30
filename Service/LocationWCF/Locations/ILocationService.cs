using System.Collections.Generic;
using System.ServiceModel;
using LocationServices.Locations.Interfaces;
using Location.TModel.Location.Person;
namespace LocationServices.Locations
{
    [ServiceContract]
    public interface ILocationService: 
        //IAreaService,
        ITagService, 
        IPositionService, 
        IDevService, 
        IKKSService, 
        IPhysicalTopologyService, 
        IConfigArgService, 
        IPersonalService,
        IAlarmService, 
        IPostService,
        IBaseDataService,
        IWorkService,
        IUserService
    {
        [OperationContract]
        string Hello(string msg);

        ///// <summary>
        ///// 获取地图列表
        ///// </summary>
        ///// <param name="depId">所属机构</param>
        ///// <returns></returns>
        //[OperationContract]
        //IList<Map> GetMaps(int? depId);

        ///// <summary>
        ///// 获取一个地图
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[OperationContract]
        //Map GetMap(int id);

        //[OperationContract]
        //IList<User> GetUsers();

        //[OperationContract]
        //User GetUser();

        [OperationContract]
        IList<Department> GetDepartmentList();

        [OperationContract]
        Department GetDepartmentTree();
        
        
    }

   
}
