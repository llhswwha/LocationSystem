using System.ServiceModel;
using Location.TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IKKSService
    {
        [OperationContract]
        KKSCode GetKKSInfoByNodeId(int id);

        [OperationContract]
        KKSCode GetKKSInfoByCode(string code);

        [OperationContract]
        KKSCode FindKKSInfoByName(string name);

        [OperationContract]
        string GetKKSCodeByNodeId(int id);

        [OperationContract]
        string FindKKSCodeByName(string name);
    }
}
