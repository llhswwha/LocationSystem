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

    public interface ILeafEntityService<T,TP> : IEntityService<T>
    {
        IList<T> GetListByPid(string pid);
        IList<T> DeleteListByPid(string pid);

        TP GetParent(string id);

        T Post(string pid, T item);
    }

    public interface ITreeEntityService<T> : ILeafEntityService<T,T>
    {
        //List<T> DeleteChildren(string id);
        T GetEntity(string id, bool getChildren);
        
        T GetTree();
        T GetTree(string id);
        
    }
}
