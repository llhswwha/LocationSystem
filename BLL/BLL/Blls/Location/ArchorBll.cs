using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public class ArchorBll : BaseBll<Archor, LocationDb>
    {
        public ArchorBll() : base()
        {

        }
        public ArchorBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Archors;
        }

        public Archor FindByCode(string code)
        {
            return DbSet.FirstOrDefault(i => i.Code == code);
        }

        public List<Archor> FindByCodes(List<string> codes)
        {
            return DbSet.Where(i => codes.Contains(i.Code)).ToList();
        }

        public List<Archor> GetListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }

        public void ClearCode()
        {
            var list = ToList();
            foreach (var item in list)
            {
                item.Code = "";
            }
            EditRange(list);
        }

        public void GenerateCode()
        {
            var list = ToList();
            foreach (var item in list)
            {
                if(string.IsNullOrEmpty(item.Code))
                    item.Code = "Code_"+item.Id;
            }
            EditRange(list);
        }

    }
}
