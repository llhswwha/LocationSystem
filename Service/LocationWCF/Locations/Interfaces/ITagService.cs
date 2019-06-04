using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface ITagService
    {
        [OperationContract]
        Tag GetTag(int id);

        [OperationContract]
        IList<Tag> GetTags();

        [OperationContract]
        int AddTag(Tag tag);

        [OperationContract]
        bool AddTags(List<Tag> tags);

        [OperationContract]
        bool DeleteTag(int id);

        [OperationContract]
        bool DeleteAllTags();

        [OperationContract]
        bool EditTag(Tag tag);

        [OperationContract]
        bool EditTagById(Tag Tag, int? id);
    }

}
