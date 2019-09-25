using DAL;
using DbModel.Location.AreaAndDev;
using IModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class DevInfoBll : BaseBll<DevInfo, LocationDb>
    {
        public DevInfoBll():base()
        {

        }
        public DevInfoBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevInfos;
        }

        public List<DevInfo> GetListByPid(List<int> pids)
        {
            return DbSet.Where(i => i.ParentId!=null && pids.Contains((int)i.ParentId)).ToList();
        }

        public List<DevInfo> DeleteListByPid(List<int> pids)
        {
            var list = GetListByPid(pids);
            foreach (var item in list)
            {
                Remove(item, false);
            }
            bool r = Save(true);
            if (r)
                return list;
            else
                return null;
        }

        public List<DevInfo> GetListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }

        public List<DevInfo> GetListWithDetail(List<Archor> archors,List<Dev_CameraInfo> cameras)
        {
            var list = this.ToList();
            foreach (var dev in list)
            {
                var type = TypeCodeHelper.GetTypeName(dev.Local_TypeCode + "", dev.ModelName);
                if (type == "基站")
                {
                    dev.DevDetail = archors.FirstOrDefault(i => i.DevInfoId == dev.Id);
                }
                else if (type == "摄像头")
                {
                    dev.DevDetail = cameras.FirstOrDefault(i => i.DevInfoId == dev.Id);
                }
                else if (type == "生产设备")
                {
                    //return (int)Abutment_DevTypes.生产设备;
                }
                else if (type == "门禁")
                {
                    //return (int)Abutment_DevTypes.门禁;
                }
                else if (type == "警报设备")
                {
                    //return (int)Abutment_DevTypes.消防设备;
                }
                else if (type == "其他设备")
                {
                    //return (int)Abutment_DevTypes.无;
                }
            }
            return list;
        }

        public override bool Add(DevInfo item, bool isSave = true)
        {
            return base.Add(item, isSave);
        }

        public override Task<bool> AddAsync(DevInfo item, bool isSave = true)
        {
            return base.AddAsync(item, isSave);
        }

        public override bool AddOrUpdate(DevInfo item, bool isSave = true)
        {
            return base.AddOrUpdate(item, isSave);
        }

        public override bool AddRange(IList<DevInfo> list, int maxTryCount = 3)
        {
            return base.AddRange(list, maxTryCount);
        }

        public override bool AddRange(LocationDb Db, IEnumerable<DevInfo> list, int maxTryCount = 3)
        {
            return base.AddRange(Db, list, maxTryCount);
        }

        public override bool AddRange(params DevInfo[] list)
        {
            return base.AddRange(list);
        }
    }
}
