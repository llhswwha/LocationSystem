using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IAuthorizationService
    {
        [OperationContract]
        List<int> GetCardRoleAccessAreas(int role);

        [OperationContract]
        bool SetCardRoleAccessAreas(int role, List<int> areaIds);
    }
}
