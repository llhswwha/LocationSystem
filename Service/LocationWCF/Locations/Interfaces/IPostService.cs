using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.AreaAndDev;
namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    //[ServiceKnownType(typeof(Post))]
    //[ServiceKnownType(typeof(TPost))]
    public interface IPostService
    {
        [OperationContract]

        List<Post> GetPostList();

        [OperationContract]
        Post GetPost(int id);

        [OperationContract]
        int AddPost(Post p);

        [OperationContract]
        bool EditPost(Post p);

        [OperationContract]
        bool DeletePost(int id);
    }
}
