using DbModel.Location.Authorizations;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations
{
    //定位卡角色相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<CardRole> GetCardRoleList()
        {
            return new TagRoleService(db).GetList();
        }

        public CardRole GetCardRole(int id)
        {
            return new TagRoleService(db).GetEntity(id+"");
        }

        public int AddCardRole(CardRole p)
        {
            var entity= new TagRoleService(db).Post(p);
            if (entity != null)
            {
                return entity.Id;
            }
            else
            {
                return -1;
            }
        }

        public bool EditCardRole(CardRole p)
        {
            var entity = new TagRoleService(db).Put(p);
            return entity != null;
        }

        public bool DeleteCardRole(int id)
        {
            var entity = new TagRoleService(db).Delete(id+"");
            return entity != null;
        }
    }
}
