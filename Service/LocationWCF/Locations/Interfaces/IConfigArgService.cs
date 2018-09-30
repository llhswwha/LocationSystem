using System.Collections.Generic;
using System.ServiceModel;
using Location.Model;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IConfigArgService
    {
        [OperationContract]
        bool AddConfigArg(ConfigArg config);

        [OperationContract]
        bool EditConfigArg(ConfigArg config);

        [OperationContract]
        bool DeleteConfigArg(ConfigArg config);

        [OperationContract]
        ConfigArg GetConfigArg(int id);

        [OperationContract]
        List<ConfigArg> GetConfigArgList();

        [OperationContract]
        ConfigArg GetConfigArgByKey(string key);

        [OperationContract]
        List<ConfigArg> FindConfigArgListByKey(string key);

        [OperationContract]
        List<ConfigArg> FindConfigArgListByClassify(string key);

        [OperationContract]
        TransferOfAxesConfig GetTransferOfAxesConfig();

        [OperationContract]
        bool SetTransferOfAxesConfig(TransferOfAxesConfig config);
    }
}
