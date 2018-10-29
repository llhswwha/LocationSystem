using System.Collections.Generic;
using System.ServiceModel;
using LocationServices.Locations.Interfaces;
using Location.TModel.Location.Person;
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
        IAreaService
    {

    }

   
}
