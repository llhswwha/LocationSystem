using BLL;
using Location.TModel.Location.Person;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations
{
    //部门相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {

        public IList<Department> GetDepartmentList()
        {
            ShowLogEx(">>>>> GetDepartmentList");
            return new DepartmentService(db).GetList();
        }

        public Department GetDepartmentTree()
        {
            ShowLogEx(">>>>> GetDepartmentTree");
            var leafNodes = GetPersonList(false);
            var s = new DepartmentService(db);
            var dep= s.GetTree(leafNodes);
            return dep;
        }

        
        public Department GetDepartment(int id)
        {
            Bll db = new Bll(false, true, false, true);
            Department dep= new DepartmentService(db).GetEntity(id+"");
            return dep;
        }


        public int AddDepartment(Department p)
        {
            var entity= new DepartmentService(db).Post(p);
            if (entity != null)
            {
                return entity.Id;
            }
            else
            {
                return -1;
            }
        }

        public bool EditDepartment(Department p)
        {
            var entity = new DepartmentService(db).Put(p);
            return entity != null;
        }
     
        public bool DeleteDepartment(int id)
        {
            var entity = new DepartmentService(db).Delete(id+"");
            return entity != null;
        }
    }
}
