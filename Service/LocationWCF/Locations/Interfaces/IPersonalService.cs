using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.Person;
using TModel.Location.Person;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IPersonalService
    {

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Personnel> GetPersonList(bool isFilterByTag);

        /// <summary>
        /// 人员查找
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        [OperationContract]
        List<Personnel> FindPersonList(string key);

        [OperationContract]
        Personnel GetPerson(int id);

        [OperationContract]
        int AddPerson(Personnel p);

        [OperationContract]
        bool EditPerson(Personnel p);

        [OperationContract]
        bool DeletePerson(int id);

        /// <summary>
        /// 附近人员（通用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<NearbyPerson> GetNearbyPerson_Currency(int id, float fDis);

        /// <summary>
        /// 附近人员（告警）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<NearbyPerson> GetNearbyPerson_Alarm(int id, float fDis);
    }
}
