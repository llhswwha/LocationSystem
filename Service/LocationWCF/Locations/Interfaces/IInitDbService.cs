using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IInitDbService
    {
        /// <summary>
        /// 初始化KKS表
        /// </summary>
        [OperationContract]
        void InitKksTable();
    }
}
