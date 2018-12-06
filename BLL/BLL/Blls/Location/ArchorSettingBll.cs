using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Settings;

namespace BLL.Blls.Location
{
    public class ArchorSettingBll : BaseBll<ArchorSetting, LocationDb>
    {
        public ArchorSettingBll() : base()
        {

        }

        public ArchorSettingBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.ArchorSettings;
        }

        public ArchorSetting GetByArchor(Archor archor)
        {
            return GetByCode(archor.Code, archor.Id);
        }

        public ArchorSetting GetByCode(string code,int archorId)
        {
            ArchorSetting item = null;
            item = Find(i => i.ArchorId == archorId);

            if (item == null)
            {
                var list = FindAll(i => i.Code == code);
                if (list != null && list.Count > 0)
                {
                    if (list.Count == 1)
                    {
                        item = list[0];
                        item.ArchorId = archorId;
                    }
                    else
                    {
                        item = list[0];
                        item.Error = true;
                    }
                }
            }
            if (item != null)
            {
                item.IsNew = false;
            }
            return item;
        }
    }
}
