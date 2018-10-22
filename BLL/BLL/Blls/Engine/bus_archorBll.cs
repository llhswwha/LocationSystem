using System.Collections.Generic;
using DAL;
using DbModel.Engine;

namespace BLL.Blls.Engine
{
    public class bus_anchorBll : BaseBll<bus_anchor, EngineDb>
    {
        public bus_anchorBll() : base()
        {

        }
        public bus_anchorBll(EngineDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.bus_anchors;
        }

        public void UpdateList(List<DbModel.Location.AreaAndDev.Archor> archors2)
        {
            var archors1 = ToList();
            List<bus_anchor> addList = new List<bus_anchor>();
            List<bus_anchor> editList = new List<bus_anchor>();
            foreach (var archor in archors2)
            {
                var bac = archors1.Find(i => i.anchor_id == archor.Code);//bus_archor
                if (bac == null)
                {
                    bac = new bus_anchor();
                    addList.Add(bac);
                }
                else
                {
                    editList.Add(bac);
                }
                if (string.IsNullOrEmpty(archor.Code))
                {
                    archor.Code = archor.Id.ToString();
                }
                bac.anchor_id = archor.Code;
                bac.anchor_x = (int)archor.X;
                bac.anchor_y = (int)archor.Y;
                bac.anchor_z = (int)archor.Z;
                bac.anchor_type = (int)archor.Type;
            }
            AddRange(addList);
            EditRange(editList);
        }

        public bool Update(DbModel.Location.AreaAndDev.Archor archor)
        {
            bool result = false;

            int nFlag = 0;
            var bac = FirstOrDefault(p => p.anchor_id == archor.Code);
            if (bac == null)
            {
                bac = new bus_anchor();
                nFlag = 1;
            }
            bac.anchor_id = archor.Code;
            bac.anchor_x = (int)archor.X;
            bac.anchor_y = (int)archor.Y;
            bac.anchor_z = (int)archor.Z;
            bac.anchor_type = (int)archor.Type;

            if (nFlag == 0)
            {
                result = Edit(bac);
            }
            else
            {
                result = Add(bac);
            }
            return result;
        }

        public bool Update(TModel.Location.AreaAndDev.Archor archor)
        {
            bool result = false;

            int nFlag = 0;
            var bac = FirstOrDefault(p => p.anchor_id == archor.Code);
            if (bac == null)
            {
                bac = new bus_anchor();
                nFlag = 1;
            }
            bac.anchor_id = archor.Code;
            bac.anchor_x = (int)archor.X;
            bac.anchor_y = (int)archor.Y;
            bac.anchor_z = (int)archor.Z;
            bac.anchor_type = (int)archor.Type;
            if (nFlag == 0)
            {
                result = Edit(bac);
            }
            else
            {
                result = Add(bac);
            }
            return result;
        }
    }
}
