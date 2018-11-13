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
        private List<Area> areas;
        private Bll bll;


        public TagRelationBuffer(Bll bll)
        {
            this.bll = bll;
            LoadData();
        }

        public TagRelationBuffer()
        {
            this.bll = new Bll(false,false,false,false);
            LoadData();
        }

        protected override void UpdateData()
        {
            personnels = bll.Personnels.ToList();
            tagToPersons = bll.LocationCardToPersonnels.ToList();
            tags = bll.LocationCards.ToList();
            archors = bll.Archors.ToList();//基站
            areas = bll.Areas.GetWithBoundPoints(true);
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

                var area = areas.Find(i => i.Id == pos.AreaId);
                if (area != null)
                {
                    if (area.IsPark())//电厂园区,基站属于园区或者楼层
                    {
                        var containsAreas = new List<Area>();
                        var boundAreas = new List<Area>();
                        foreach (var item in area.Children)
                        {
                            if (item.InitBound != null)
                            {
                                boundAreas.Add(item);
                            }
                            foreach (var building in item.Children)
                            {
                                if (building.InitBound != null)
                                {
                                    boundAreas.Add(building);
                                }
                            }
                        }
                        foreach (var boundArea in boundAreas)
                        {
                            if (boundArea.InitBound.Contains(pos.X, pos.Z))
                            {
                                containsAreas.Add(boundArea);
                            }

                            //if (boundArea.InitBound.ContainsSimple(pos.X, pos.Z))
                            //{
                            //    containsAreas.Add(boundArea);
                            //}
                        }
                        //todo:加上建筑外的区域
                        if (containsAreas.Count > 0)
                        {
                            pos.AreaId = containsAreas[0].Id;
                            pos.AreaPath = containsAreas[0].Name;
                        }
                    }
                    else if (area.Type == DbModel.Tools.AreaTypes.楼层)
                    {
                        var building = area.Parent;
                        var containsAreas = new List<Area>();
                        var childrenArea = area.Children;
                        foreach (var item in childrenArea)
                        {
                            var x = pos.X - area.InitBound.MinX - building.InitBound.MinX;
                            var y = pos.Z - area.InitBound.MinY - building.InitBound.MinY;
                            if (item.InitBound.Contains(x, y))
                            {
                                containsAreas.Add(item);
                            }
                        }
                        if (containsAreas.Count > 0)
                        {
                            pos.AreaPath = building.Name + "." + area.Name + "." + containsAreas[0].Name;
                            pos.AreaId = containsAreas[0].Id;
                        }
                        else
                        {
                            pos.AreaPath = building.Name + "." + area.Name;
                        }
                    }
                }
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
