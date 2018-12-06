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

        T Post(T item);

        T Put(T item);
    }

    public interface INameEntityService<T> : IEntityService<T>
    {
        IList<T> GetListByName(string name);
    }

    public interface ILeafEntityService<T,TP> : INameEntityService<T>
    {
        List<T> GetListByPid(string pid);
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
