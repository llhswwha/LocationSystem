using Location.TModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface Ibus_tag
    {
        [OperationContract]
        bool EditBusTag(Tag Tag);
    }
}
