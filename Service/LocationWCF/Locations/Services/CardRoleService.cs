using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.Authorizations;
using BLL;

namespace LocationServices.Locations.Services
{
    public interface ICardRoleService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        List<CardRole> GetCardRoleList();

        CardRole GetCardRole(int id);

        int AddCardRole(CardRole p);

        bool EditCardRole(CardRole p);

        bool DeleteCardRole(int id);
    }

    public class CardRoleService : ICardRoleService
    {
        private Bll db;
        public CardRoleService()
        {
            db = Bll.NewBllNoRelation();
        }
        public int AddCardRole(CardRole p)
        {
            var entity = new TagRoleService(db).Post(p);
            RefreshData();
            if (entity != null)
            {
                return entity.Id;
            }
            else
            {
                return -1;
            }
        }

        public bool DeleteCardRole(int id)
        {
            var entity = new TagRoleService(db).Delete(id + "");
            RefreshData();
            return entity != null;
        }

        public bool EditCardRole(CardRole p)
        {
            var entity = new TagRoleService(db).Put(p);
            RefreshData();
            return entity != null;
        }

        public CardRole GetCardRole(int id)
        {
            return new TagRoleService(db).GetEntity(id + "");
        }

        public List<CardRole> GetCardRoleList()
        {
            return new TagRoleService(db).GetList();
        }


        private void RefreshData()
        {
            try
            {
                BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();
            }
            catch (Exception e)
            {

            }
        }
    }
}
