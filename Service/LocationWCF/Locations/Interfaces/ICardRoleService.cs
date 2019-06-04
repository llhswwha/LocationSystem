using DbModel.Location.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface ICardRoleService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CardRole> GetCardRoleList();

        [OperationContract]
        CardRole GetCardRole(int id);

        [OperationContract]
        int AddCardRole(CardRole p);

        [OperationContract]
        bool EditCardRole(CardRole p);

        [OperationContract]
        bool DeleteCardRole(int id);
    }
}
