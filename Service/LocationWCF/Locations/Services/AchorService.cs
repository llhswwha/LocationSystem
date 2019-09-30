using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.Archor;
using TEntity = TModel.Location.AreaAndDev.Archor;
using IModel.Enums;

namespace LocationServices.Locations.Services
{
    public interface IArchorService:INameEntityService<TEntity>
    {
        IList<TEntity> Search(string key,string value);

        TEntity GetArchorByDevId(int devId);

        bool EditArchor(TEntity Archor, int ParentId);

        bool EditBusAnchor(TEntity archor, int ParentId);
    }
    public class ArchorService : IArchorService
    {
        private Bll db;

        private ArchorBll dbSet;

        public ArchorService()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.Archors;
        }

        public ArchorService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Archors;
        }

        public TEntity Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item.ToTModel();
        }

        public TEntity GetEntity(string id)
        {
            var item = dbSet.Find(id.ToInt());
            return item.ToTModel();
        }

        public List<TEntity> GetList()
        {
            var devInfoList = dbSet.ToList().ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> Search(string key,string value)
        {
            var devInfoList = dbSet.Search(key, value).ToTModel();
            return devInfoList.ToWCFList();
        }

        public TEntity Post(TEntity item)
        {
            var dbItem = item.ToDbModel();
            if (item.DevInfoId == 0)
            {
                var dev = new DbModel.Location.AreaAndDev.DevInfo();
                dev.Name = item.Name;
                dev.ParentId = item.ParentId;
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.Local_TypeCode = 20180821;
                dev.ModelName = "定位设备1_3D";
                dev.UserName = "admin";
                dev.IP = "";
                bool r1 = db.DevInfos.Add(dev);//创建基站前先创建设备
                if (r1)
                {
                    dbItem.DevInfo = dev;
                    dbItem.DevInfoId = dev.Id;
                    var result = dbSet.Add(dbItem);
                    return result ? dbItem.ToTModel(true) : null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var result = dbSet.Add(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
        }

        public TEntity Put(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var dbItemOld = dbSet.Find(item.Id);
            dbItemOld.Name = dbItem.Name;
            
            var result = dbSet.Edit(dbItemOld);
            if (result)
            {
                dbItemOld.DevInfo.Name = item.Name;
                db.DevInfos.Edit(dbItemOld.DevInfo);
            }
            return result ? dbItemOld.ToTModel() : null;
        }

        public TEntity GetArchorByDevId(int devId)
        {
            return db.Archors.FirstOrDefault(i => i.DevInfoId == devId).ToTModel();
        }

        public bool EditArchor(TEntity Archor, int ParentId)
        {
            bool bReturn = false;
            DbModel.Location.AreaAndDev.Archor Archor2;
            Archor2 = db.Archors.FirstOrDefault(p => p.Code == Archor.Code);
            if (Archor2 == null)
            {
                Archor2 = db.Archors.FirstOrDefault(p => p.DevInfoId == Archor.DevInfoId);
            }
            if (Archor2 == null)
            {
                LocationService service = new LocationService();
                DbModel.Location.AreaAndDev.Area area = service.GetAreaById(ParentId);
                Archor2 = Archor.ToDbModel();

                DbModel.Location.AreaAndDev.DevInfo dev = new DbModel.Location.AreaAndDev.DevInfo();
                dev.Local_DevID = Guid.NewGuid().ToString();
                dev.IP = "";
                dev.KKS = "";
                dev.Name = Archor2.Name;
                if (area != null)
                {
                    dev.ModelName = area.Name == DepNames.FactoryName ? TypeNames.ArchorOutdoor : TypeNames.Archor;//室外基站||室内基站
                }
                else
                {
                    dev.ModelName = TypeNames.Archor;
                }
                dev.Status = 0;
                dev.ParentId = ParentId;
                dev.Local_TypeCode = TypeCodes.Archor;
                dev.UserName = "admin";
                Archor2.DevInfo = dev;
                Archor2.ParentId = ParentId;

                bReturn = db.Archors.Add(Archor2);
            }
            else
            {
                Archor2.Name = Archor.Name;
                Archor2.X = Archor.X;
                Archor2.Y = Archor.Y;
                Archor2.Z = Archor.Z;
                Archor2.Type = Archor.Type;
                Archor2.IsAutoIp = Archor.IsAutoIp;
                Archor2.Ip = Archor.Ip;
                Archor2.ServerIp = Archor.ServerIp;
                Archor2.ServerPort = Archor.ServerPort;
                Archor2.Power = Archor.Power;
                Archor2.AliveTime = Archor.AliveTime;
                Archor2.Enable = Archor.Enable;
                if (!string.IsNullOrEmpty(Archor.Code)) Archor2.Code = Archor.Code;
                bReturn = db.Archors.Edit(Archor2);
            }
            EditBusAnchor(Archor, ParentId);
            return bReturn;
        }

        public bool EditBusAnchor(TEntity archor, int ParentId)
        {
            bool bDeal = false;

            try
            {
                int nFlag = 0;
                var bac = db.bus_anchors.FirstOrDefault(p => p.anchor_id == archor.Code);
                if (bac == null)
                {
                    bac = new DbModel.Engine.bus_anchor();
                    nFlag = 1;
                }

                bac.anchor_id = archor.Code;
                bac.anchor_x = (int)(archor.X * 100);
                bac.anchor_y = (int)(archor.Z * 100);
                bac.anchor_z = (int)(archor.Y * 100);
                bac.anchor_type = (int)archor.Type;
                bac.anchor_bno = 0;
                bac.syn_anchor_id = null;
                bac.offset = 0;
                bac.enabled = 1;

                if (nFlag == 0)
                {
                    bDeal = db.bus_anchors.Edit(bac);
                }
                else
                {
                    bDeal = db.bus_anchors.Add(bac);
                }

                //if (!bDeal)
                //{
                //    return bDeal;
                //}

                //bDeal = EditArchor(Archor, ParentId);
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return bDeal;
        }

        //todo:1.添加基站到某个区域下；2.获取某个区域下的所有基站
    }
}
