using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public interface IArchorBll
    {
        Archor FindByCode(string code);

        List<Archor> FindByCodes(List<string> codes);

        List<Archor> GetListByName(string name);
    }

    public class ArchorBll : BaseBll<Archor, LocationDb>, IArchorBll
    {
        public ArchorBuffer Buffer { get; set; }

        public ArchorBll() : base()
        {
            Buffer = new ArchorBuffer(this);
        }
        public ArchorBll(LocationDb db) : base(db)
        {
            Buffer = new ArchorBuffer(this);
        }

        protected override void InitDbSet()
        {
            DbSet = Db.Archors;
        }

        public Archor FindByCode(string code)
        {
            return DbSet.FirstOrDefault(i => i.Code == code);
        }

        public Archor FindByIp(string ip)
        {
            return DbSet.FirstOrDefault(i => i.Ip == ip);
        }

        public Archor FindByDev(int devId)
        {
            return DbSet.FirstOrDefault(i => i.DevInfoId == devId);
        }

        public Archor DeleteByDev(int devId)
        {
            var archor = FindByDev(devId);
            if (archor == null) return null;
            return DeleteById(archor.Id);
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

    /// <summary>
    /// 基站信息缓存
    /// </summary>
    public class ArchorBuffer: IArchorBll
    {
        ArchorBll bll;

        public List<Archor> DbSet { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime DataTime;

        /// <summary>
        /// 更新数据间隔
        /// </summary>
        public int UpdateInterval = 10;//todo:应该写到配置文件中

        public void LoadData()
        {
            if (DbSet == null)
            {
                UpdateData();
            }
            else
            {
                TimeSpan time = DateTime.Now - DataTime;
                if (time.TotalSeconds > UpdateInterval)//大于该时间则更新数据，否则用老数据
                {
                    UpdateData();
                }
            }
        }

        private void UpdateData()
        {
            DataTime = DateTime.Now;
            DbSet = bll.ToList();
        }

        public ArchorBuffer(ArchorBll bll)
        {
            this.bll = bll;
        }

        public Archor FindByCode(string code)
        {
            LoadData();
            return DbSet.FirstOrDefault(i => i.Code == code);
        }

        public List<Archor> FindByCodes(List<string> codes)
        {
            LoadData();
            return DbSet.Where(i => codes.Contains(i.Code)).ToList();
        }

        public List<Archor> GetListByName(string name)
        {
            LoadData();
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }
    }
}
