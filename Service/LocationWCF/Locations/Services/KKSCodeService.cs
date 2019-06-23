using BLL;
using BLL.Blls.Location;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Location.Person;
using DbModel.Tools;
using Location.IModel;
using TModel.Location.Nodes;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.KKSCode;
using TEntity = Location.TModel.Location.AreaAndDev.KKSCode;

namespace LocationServices.Locations.Services
{
    public interface IKKSCodeService
    {
        IList<TEntity> GetList();

        IList<TEntity> GetMainType(string mainType);

        IList<TEntity> GetSubType(string subType);

        IList<TEntity> GetParentCode(string parentCode);

        List<TEntity> GetTree();

        TEntity GetTree(int id);

        TEntity GetEntity(int id);

        TEntity GetEntityByCode(string code);

    }

    public class KKSCodeService : IKKSCodeService
    {
        private Bll db;

        private KKSCodeBll dbSet;

        public KKSCodeService()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.KKSCodes;
        }

        public KKSCodeService(Bll bll)
        {
            this.db = bll;
            dbSet = db.KKSCodes;
        }

        public IList<TEntity> GetList()
        {
            var list1 = dbSet.ToList();
            return list1.ToWcfModelList();
        }

        public IList<TEntity> GetMainType(string mainType)
        {
            var list1 = dbSet.DbSet.Where(p=>p.MainType == mainType).ToList();
            return list1.ToWcfModelList();
        }

        public IList<TEntity> GetSubType(string subType)
        {
            var list1 = dbSet.DbSet.Where(p => p.SubType == subType).ToList();
            return list1.ToWcfModelList();
        }

        public IList<TEntity> GetParentCode(string parentCode)
        {
            var list1 = dbSet.DbSet.Where(p => p.ParentCode == parentCode).ToList();
            return list1.ToWcfModelList();
        }

        public List<TEntity> GetTree()
        {
            List<DbEntity> list = dbSet.ToList();
            List<TEntity> Tlist = list.ToWcfModelList();
            List<TEntity> roots = new List<TEntity>();

            foreach (var item in Tlist)
            {
                var parent = Tlist.Find(i => i.Code == item.ParentCode);
                if (parent == null)
                {
                    roots.Add(item);
                }
                else
                {
                    if (parent.Children == null)
                    {
                        parent.Children = new List<TEntity>();
                    }
                    parent.Children.Add(item);
                }
            }

            return Tlist;
        }

        public TEntity GetTree(int id)
        {
            List<DbEntity> list = dbSet.ToList();
            List<TEntity> Tlist = list.ToWcfModelList();
            var item = Tlist.Find(p=>p.Id == id);
            GetChildrenTree(item, Tlist);
            return item;
        }

        private void GetChildrenTree(TEntity entity, List<TEntity> Tlist)
        {
            if (entity == null) return;

            var list = Tlist.FindAll(p => p.ParentCode == entity.Code);
            if (entity.Children == null)
            {
                entity.Children = new List<TEntity>();
            }
            entity.Children = list;
            if (list != null)
            {
                foreach (var item in list)
                {
                    GetChildrenTree(item, Tlist);
                }
            }

        }

        public TEntity GetEntity(int id)
        {
            DbEntity de = db.KKSCodes.DbSet.Where(p => p.Id == id).FirstOrDefault();
            return de.ToTModel();
        }

        public TEntity GetEntityByCode(string code)
        {
            DbEntity de = db.KKSCodes.DbSet.Where(p => p.Code == code).FirstOrDefault();
            return de.ToTModel();
        }
    }
}
