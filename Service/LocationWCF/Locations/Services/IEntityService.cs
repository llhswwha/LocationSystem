using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Services
{
    public interface IEntityService<T>
    {
        T Delete(string id);

        T GetEntity(string id);

        IList<T> GetList();
        IList<T> GetListByName(string name);

        T Post(T item);

        T Put(T item);
    }
}
