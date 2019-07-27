using System.Collections.Generic;
using System.ServiceModel;
using LocationServices.Locations.Interfaces;
using LocationServices.Locations.Plugins;
using TModel.Location.AreaAndDev;
using TModel.Models.Settings;

namespace LocationServices.Locations
{
    [ServiceContract]
    public interface ILocationService:
        ITestService,
        //IAreaService,
        IDepartmentService,
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
        IUserService,
        Ibus_anchor,
        Ibus_tag,
        IPictureService,
        IAreaService,
        IInitDbService,
        ICardRoleService,
        IAuthorizationService,
        ICameraAlarmService,
        INVSPlayer
    {
        [OperationContract]
        UnitySetting GetUnitySetting();

        [OperationContract]
        void DebugMessage(string msg);

        [OperationContract]
        AreaPoints GetPoints(int areaId);

        [OperationContract]
        List<AreaPoints> GetPointsByPid(int pid);
    }

   
}
