using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface ITagService
    {
        [OperationContract]
        IList<Tag> GetTags();

        [OperationContract]
        bool AddTags(List<Tag> tags);

        [OperationContract]
        bool DeleteTag(int id);

        [OperationContract]
        bool DeleteAllTags();
    }

}
