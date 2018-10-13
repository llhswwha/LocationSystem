using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Services
{
    public interface ITreeEntityService<T>:IEntityService<T>
    {
        List<T> DeleteChildren(string id);
        T GetEntity(string id, bool getChildren);
        IList<T> GetListByPid(string pid);
        T GetTree();
        T GetTree(string id);
        T Post(string pid, T item);
    }
}
