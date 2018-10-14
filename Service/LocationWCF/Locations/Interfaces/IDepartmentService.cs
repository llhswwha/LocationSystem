using Location.TModel.Location.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IDepartmentService
    {
        [OperationContract]
        IList<Department> GetDepartmentList();

        [OperationContract]
        Department GetDepartmentTree();
    }
}
