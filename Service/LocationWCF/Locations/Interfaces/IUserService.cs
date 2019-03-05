using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Manage;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        LoginInfo Login(LoginInfo info);

        [OperationContract]
        LoginInfo Logout(LoginInfo info);

        [OperationContract]
        LoginInfo KeepLive(LoginInfo info);

        [OperationContract]
        VersionInfo GetVersionInfo();
    }
}
