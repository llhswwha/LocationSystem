using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface Ibus_anchor
    {
        [OperationContract]
        bool EditBusAnchor(Archor Archor, int ParentId);
    }
}
