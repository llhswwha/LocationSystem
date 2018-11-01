using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Data;
using Location.BLL.Tool;

namespace BLL
{
    public class TagRelationBuffer: BaseBuffer
    {
        private List<Personnel> personnels;
        private List<LocationCardToPersonnel> tagToPersons;
        private List<LocationCard> tags;
        private List<Archor> archors;
        private Bll bll;


        public TagRelationBuffer(Bll bll)
        {
            this.bll = bll;
            LoadData();
        }

        protected override void UpdateData()
        {
            personnels = bll.Personnels.ToList();
            tagToPersons = bll.LocationCardToPersonnels.ToList();
            tags = bll.LocationCards.ToList();
            archors = bll.Archors.ToList();//基站
        }

        public void SetPositionInfo(List<Position> positions)
        {
            LoadData();
            foreach (Position pos in positions)
            {
                if (pos == null) continue;
                SetPositionInfo(pos);
            }
        }

        public void SetPositionInfo(Position pos)
        {
            try
            {
                //设置标签和人员
                SetTagAndPerson(pos);
                //设置区域
                SetArea(pos);
            }
            catch (Exception ex)
            {
                Log.Error("SetPositionInfo", ex);
            }
        }

        private void AddSimulateArchor(Position pos)
        {
            if (pos.IsSimulate)//是模拟程序数据,计算并添加基站
            {
                var relativeArchors = archors.FindAll(i => ((i.X - pos.X) * (i.X - pos.X) + (i.Z - pos.Z) * (i.Z - pos.Z)) < 100).ToList();
                foreach (var archor in relativeArchors)
                {
                    pos.AddArchor(archor.Code);
                }
            }
        }

        private void SetArea(Position pos)
        {
            AddSimulateArchor(pos);

            if (pos.Archors != null && pos.Archors.Count > 0)
            {
                //List<Archor> archorList = Archors.Buffer.FindByCodes(pos.Archors);
                var archorList = archors.Where(i => pos.Archors.Contains(i.Code));
                var areas = new Dictionary<int, int>();
                int maxCount = 0;
                int maxArea = 0;
                foreach (Archor archor in archorList)
                {
                    if (archor.ParentId == null) continue;
                    int parentId = (int)archor.ParentId;
                    if (!areas.ContainsKey(parentId))
                    {
                        areas[parentId] = 0;
                    }
                    areas[parentId]++;
                    if (areas[parentId] > maxCount)
                    {
                        maxArea = parentId;
                    }
                }
                pos.AreaId = maxArea;
            }
            else
            {
                pos.AreaId = null;
            }
        }

        private void SetTagAndPerson(Position pos)
        {
            var tag = tags.Find(i => i.Code == pos.Code); //标签
            if (tag != null)
            {
                //1.设置标签
                pos.CardId = tag.Id;
                var ttp = tagToPersons.Find(i => i.LocationCardId == tag.Id); //关系
                if (ttp != null)
                {
                    var personnelT = personnels.Find(i => i.Id == ttp.PersonnelId); //人员
                    if (personnelT != null)
                    {
                        //2.设置人员
                        pos.PersonnelID = personnelT.Id;
                    }
                }
            }
        }
    }
}
