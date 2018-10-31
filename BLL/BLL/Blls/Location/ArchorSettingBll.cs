using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
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

        public ArchorSetting GetByCode(string code)
        {
            return Find(i => i.Code == code);
        }

        
    }
}
