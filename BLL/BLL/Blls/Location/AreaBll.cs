using System.Collections.Generic;
using System.Linq;
using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public class AreaBll : BaseBll<Area, LocationDb>
    {
        public AreaBll():base()
        {

        }
        public AreaBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Areas;
        }

        public Area GetRoot()
        {
            return DbSet.FirstOrDefault();
        }

        public Area FindByName(string name)
        {
            return DbSet.FirstOrDefault(i => i.Name == name);
        }

        public List<Area> FindListByName(string name)
        {
            var list = DbSet.Where(i => i.Name.Contains(name)).ToList();
            foreach(var item in list)
            {
                item.Children = null;//todo:Find一个不会获取Children，但是用Contains查找会有，为什么呢？要再看看EF的书。
            }
            return list;
        }

        public List<Area> FindListByPid(int pid)
        {
            return DbSet.Where(i => i.ParentId==pid).ToList();
        }

        public List<Area> GetWithBoundPoints(bool withPoints)
        {
            var bounds = Db.Bounds.ToList();
            
            List<Area> list = ToList();
            foreach (var area in list)
            {
                if (area.InitBound == null)
                    area.InitBound = bounds.Find(i => i.Id == area.InitBoundId);
                if (area.EditBound == null)
                    area.EditBound = bounds.Find(i => i.Id == area.EditBoundId);
                if(area.Children==null)
                    area.Children = list.FindAll(i => i.ParentId == area.Id);
            }

            if (withPoints)
            {
                var points = Db.Points.ToList();
                foreach (var bound in bounds)
                {
                    if (bound.Points == null)
                        bound.Points = points.FindAll(i => i.BoundId == bound.Id);
                }
            }
            return list;
        } 
    }
}
