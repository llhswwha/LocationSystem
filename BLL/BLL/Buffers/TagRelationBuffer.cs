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
        //private List<Area> areas;
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
            //areas = bll.Areas.GetWithBoundPoints();
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

        class ArchorDistance:IComparable<ArchorDistance>
        {
            public double Distance { get; set; }
            public Archor Archor { get; set; }

            public ArchorDistance(double d, Archor a)
            {
                Distance = d;
                Archor = a;
            }

            public int CompareTo(ArchorDistance other)
            {
                return this.Distance.CompareTo(other.Distance);
            }
        }

        //public void A

        private void AddSimulateArchor(Position pos)
        {
            if (pos.IsSimulate)//是模拟程序数据,计算并添加基站
            {
                List<Archor> relativeArchors = new List<Archor>();
                List<ArchorDistance> distances = new List<ArchorDistance>();
                foreach (Archor i in archors)
                {
                    var distance = ((i.X - pos.X) * (i.X - pos.X) + (i.Z - pos.Z) * (i.Z - pos.Z));
                    if (distance < 200)
                    {
                        relativeArchors.Add(i);
                    }
                    if (distance < 1000)
                    {
                        distances.Add(new ArchorDistance(distance, i));
                    }
                }
                if (relativeArchors.Count == 0)
                {
                    distances.Sort();
                    pos.AddArchor(distances[0].Archor.Code);
                }
                else
                {
                    foreach (var archor in relativeArchors)
                    {
                        pos.AddArchor(archor.Code);
                    }
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
                var areaCount = new Dictionary<int, int>();
                int maxCount = 0;
                int maxArea = 0;
                foreach (Archor archor in archorList)
                {
                    if (archor.ParentId == null) continue;
                    int parentId = (int)archor.ParentId;
                    if (!areaCount.ContainsKey(parentId))
                    {
                        areaCount[parentId] = 0;
                    }
                    areaCount[parentId]++;
                    if (areaCount[parentId] > maxCount)
                    {
                        maxArea = parentId;
                        maxCount = areaCount[parentId];
                    }
                }
                pos.AreaId = maxArea;

                //var area = areas.Find(i => i.Id == pos.AreaId);
                //if (area != null)
                //{
                //    if (area.IsPark())//电厂园区,基站属于园区或者楼层
                //    {
                //        //var containsAreas = new List<Area>();
                //        //var childrenArea = areas.FindAll(i => i.ParentId == area.Id);//建筑集合
                //        //foreach (var item in childrenArea)
                //        //{
                //        //    var buildings = areas.FindAll(i => i.ParentId == item.Id);//建筑
                //        //    foreach(var building in buildings)
                //        //    {
                //        //        if (building.InitBound.Contains(pos.X, pos.Y))
                //        //        {
                //        //            containsAreas.Add(building);
                //        //        }
                //        //    }
                //        //}
                //        ////todo:加上建筑外的区域
                //        //if (containsAreas.Count > 0)
                //        //{
                //        //    pos.AreaId = containsAreas[0].Id;
                //        //}
                //    }
                //    else if (area.Type == DbModel.Tools.AreaTypes.楼层)
                //    {
                //        var containsAreas = new List<Area>();
                //        var childrenArea = areas.FindAll(i => i.ParentId == area.Id);//机房
                //        foreach (var item in childrenArea)
                //        {
                //            if (item.InitBound.Contains(pos.X, pos.Y))
                //            {
                //                containsAreas.Add(item);
                //            }
                //        }
                //        if (containsAreas.Count > 0)
                //        {
                //            pos.AreaId = containsAreas[0].Id;
                //        }
                //    }
                //}
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
                pos.RoleId = tag.CardRoleId;//角色
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
