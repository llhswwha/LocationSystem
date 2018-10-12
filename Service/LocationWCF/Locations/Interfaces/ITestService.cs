using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/hello?msg={msg}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        string Hello(string msg);
    }
}
