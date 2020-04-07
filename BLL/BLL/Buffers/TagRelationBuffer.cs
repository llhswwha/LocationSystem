using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using DbModel.Location.Data;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.BLL.Tool;

namespace BLL
{
    public class TagRelationBuffer : BaseBuffer
    {
        private Dictionary<int,Personnel> personnels;
        private Dictionary<int,int> tagToPersons;
        private Dictionary<string, LocationCard> tags;
        private List<CardRole> roles;
        private List<Archor> archors;
        private List<Area> areas;
        private Area parkArea;//园区区域
        private Bll bll;
        private Dictionary<string, LocationCard> locationCardDic;
        private List<Department> departments;

        private static TagRelationBuffer Single = null;

        public static TagRelationBuffer Instance()
        {
            if (Single == null)
            {
                Single = new TagRelationBuffer();
            }

            return Single;
        }

        public static TagRelationBuffer Instance(Bll bll)
        {
            if (Single == null)
            {
                Single = new TagRelationBuffer(bll);
            }

            return Single;

        }

        private TagRelationBuffer(Bll bll)
        {
            this.bll = bll;
            LoadData();
        }

        private TagRelationBuffer()
        {
            this.bll = Bll.NewBllNoRelation();
            LoadData();
        }

        public void PuUpdateData()
        {
            UpdateData();
        }

        protected override void UpdateData()
        {
            RefreshTags();

            archors = bll.Archors.ToList();//基站
            areas = bll.Areas.GetWithBoundPoints(true);
            roles = bll.CardRoles.ToList();
            parkArea = areas.FirstOrDefault(i => i.IsPark());
        }

        public void RefreshTags()
        {
            personnels = bll.Personnels.ToDictionary();
            tagToPersons = bll.LocationCardToPersonnels.GetTagToPerson();
            tags = bll.LocationCards.ToDictionaryByCode();
            locationCardDic = bll.LocationCards.ToDictionaryByCode();
        }
        public Dictionary<string, LocationCard> GetLocationCardDic()
        {
            if(locationCardDic==null)
            {
                locationCardDic= bll.LocationCards.ToDictionaryByCode();
            }
            return locationCardDic;
        }

        /// <summary>
        /// 设置标签、人员和区域
        /// </summary>
        /// <param name="positions"></param>
        public void SetPositionInfo(List<Position> positions)
        {
            LoadData();
            foreach (Position pos in positions)
            {
                if (pos == null) continue;
                SetPositionInfo(pos);
            }
        }

        /// <summary>
        /// 设置标签、人员和区域
        /// </summary>
        /// <param name="pos"></param>
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

        class ArchorDistance : IComparable<ArchorDistance>
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
                    if (distances.Count > 0)
                    {
                        pos.AddArchor(distances[0].Archor.Code);
                    }
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

        public Area GetParkArea()
        {
            if (parkArea != null)
            {
                return parkArea;
            }

            return areas[1];
        }

        public int? SetArea(Position pos)
        {
            try
            {
                //AddSimulateArchor(pos);
                if (pos.Code == "092D")
                {
                    int i = 0;
                }
                if (pos.Archors != null && pos.Archors.Count > 0)
                {
                    SetAreaByArchor(pos);
                    var area = areas.Find(i => pos.IsInArea(i.Id));
                    if (area != null)
                    {
                        if (area.IsPark()) //电厂园区,基站属于园区或者楼层
                        {
                            SetAreaInPark(pos, area);
                        }
                        else if (area.Type == DbModel.Tools.AreaTypes.楼层)
                        {
                            SetAreaInFloor(pos, area);
                        }
                    }
                    else
                    {
                        area = GetParkArea();
                        SetAreaByPosition(pos, area);
                    }
                }
                else
                {
                    var area = GetParkArea();
                    SetAreaByPosition(pos, area);
                }

                if (pos.Code == "092D" && pos.AreaId == 2)
                {
                    int i = 0;
                }

                if (pos.IsAreaNull())
                {
                    Log.Info("SetArea", "pos.IsAreaNull() : " + pos.Code + "," + pos.X + "," + pos.Y + "," + pos.Z);
                }
            }
            catch (Exception ex)
            {
                Log.Error("SetArea", ex);
            }

            return pos.AreaId;
        }

        private int? SetAreaByPosition(Position pos, Area area)
        {
            //var floor1 = areas.Find(i => i.Id == 5);
            //if (floor1 != null)
            //{
            //    SetAreaInFloor(pos, floor1);
            //    return pos.AreaId;
            //}
            //else
            //{
            //    pos.AreaId = 5;
            //    return 5;
            //}


            try
            {
                //      if (pos.Code != "0918") return;
                if (area.IsPark()) //电厂园区,基站属于园区或者楼层
                {
                    var inArea = SetAreaInPark(pos, area);
                    if (inArea != null) //某个建筑物
                    {
                        if (inArea.Type == AreaTypes.大楼)
                        {
                            var floor = inArea.GetFloorByHeight(pos.Y);

                            if (floor != null)
                            {
                                SetAreaInFloor(pos, floor);
                            }
                            else//自然通风冷却塔这种的没有楼层
                            {

                            }
                            //if (floor.Id == 62)
                            //{

                            //}
                            if (pos.AreaId == 62)
                            {

                            }
                        }
                        else//园区
                        {

                        }
                    }
                    else
                    {
                        pos.AreaId = area.Id;//设为园区
                        int nn = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("TagRelationBuffer.SetAreaByPosition", ex);
            }

            return pos.AreaId;
        }

        private void SetAreaByArchor(Position pos)
        {
            var archorList = archors.Where(i => pos.Archors.Contains(i.Code)).ToList();
            if (archorList.Count > 0)
            {
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
                //pos.AreaId = maxArea;
                var area = areas.Find(i => i.Id == maxArea);
                pos.SetArea(area);
            }
        }

        private static int? SetAreaInFloor(Position pos, Area area)
        {
            var building = area.Parent;
            var containsAreas = new List<Area>();
            var childrenArea = area.Children;
            foreach (var item in childrenArea)
            {
                if (item.Type == AreaTypes.CAD) continue;//过滤掉柱子等CAD物体
                var x = pos.X - area.InitBound.GetZeroX() - building.InitBound.GetZeroX();
                var y = pos.Z - area.InitBound.GetZeroY() - building.InitBound.GetZeroY();
                if (item == null || item.InitBound == null)
                {
                    continue;
                }
                if (item.InitBound.Contains(x, y) /*&& item.IsOnLocationArea*/)
                {
                    containsAreas.Add(item);
                }
            }
            if (containsAreas.Count > 0)
            {
                //Area areaT = containsAreas.Find((i) => i.IsOnLocationArea == true);
                //if (areaT == null)
                //{
                //    //pos.SetArea(containsAreas[0]);
                //    areaT = containsAreas[0];
                //}
                //pos.SetArea(areaT);
                ////pos.AreaPath = building.Name + "." + area.Name + "." + containsAreas[0].Name;
                //pos.AreaPath = building.Name + "." + area.Name + "." + areaT.Name;

                pos.SetArea(containsAreas.ToArray());
            }
            else
            {
                pos.SetArea(area);
                pos.AreaPath = building.Name + "." + area.Name;
            }

            return pos.AreaId;
        }

        private static Area SetAreaInPark(Position pos, Area park)
        {
            var containsAreas = new List<Area>();
            var boundAreas = new List<Area>();
            foreach (var item in park.Children)
            {
                foreach (var building in item.Children)
                {
                    if (building.InitBound != null)
                    {
                        boundAreas.Add(building);
                    }
                }
                if (item.InitBound != null)
                {
                    boundAreas.Add(item);
                }
            }
            foreach (var boundArea in boundAreas)
            {               
                if (boundArea.InitBound.Contains(pos.X, pos.Z))
                {
                    //以前只考虑园区->区域->建筑结构，现在还会有园区->建筑->楼层。如果是楼层，在这里就要细分是哪一层
                    bool isFloor= boundArea.Type == AreaTypes.楼层;
                    if (isFloor)
                    {
                        if (pos.Y >= boundArea.InitBound.MinZ && pos.Y < boundArea.InitBound.MaxZ)
                        {
                            containsAreas.Add(boundArea);
                        }
                    }
                    else
                    {
                        containsAreas.Add(boundArea);
                    }                   
                }
            }
            Area inArea = null;
            //todo:加上建筑外的区域
            if (containsAreas.Count > 0)
            {
                var areaNode = containsAreas.Find(i => i.Type != AreaTypes.范围);
                if (areaNode == null)
                {
                    containsAreas.Add(park);
                }
                inArea=pos.SetArea(containsAreas.ToArray());
            }
            if (inArea == null)
            {
                if (park.InitBound!=null&&park.InitBound.Contains(pos.X, pos.Z))
                {
                    inArea = park;
                    pos.SetArea(inArea);
                }
            }
            if (inArea == null)
            {
                Log.Info("inArea == null");
            }
            return inArea;
        }

        public void SetTagAndPerson(Position pos)
        {
            if (tags == null) return;
            if (tags.ContainsKey(pos.Code))
            {
                var tag = tags[pos.Code];
                //1.设置标签
                pos.CardId = tag.Id;
                pos.RoleId = tag.CardRoleId;//角色

                if (tagToPersons.ContainsKey(tag.Id))
                {
                    var personId = tagToPersons[tag.Id];
                    if (personnels.ContainsKey(personId))
                    {
                        var personnelT = personnels[personId];
                        //2.设置人员
                        pos.PersonnelID = personnelT.Id;
                    }
                    else//关联的人员不在了 ，误删了的话，先补回来上吧。当前定位的卡应该都有关联的人员的
                    {
                        var person = AddPersonByTag(pos, tag);
                        if (person != null)
                        {
                            pos.PersonnelID = person.Id;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                var tag = AddTagByPos(pos);
                RefreshTags();
            }
        }

        public LocationCard AddTagByPos(Position pos)
        {
            LocationCard tag = new LocationCard();
            tag.Name = pos.Code;
            tag.Code = pos.Code;
            CardRole role = roles.Find(p => p.Name == "管理人员");
            if (role == null)
            {
                role = roles[0];
            }

            tag.CardRoleId = roles[0].Id;

            bool r1 = bll.LocationCards.Add(tag);

            pos.CardId = tag.Id;
            pos.RoleId = tag.CardRoleId;//角色

            if (r1)
            {
                var person = AddPersonByTag(pos, tag);
                if (person != null)
                {
                    pos.PersonnelID = person.Id;
                }
            }
            return tag;
        }

        private Personnel AddPersonByTag(Position pos, LocationCard tag)
        {
            Personnel person = new Personnel();
            person.Name = "Tag_" + pos.Code;
            if (departments == null || departments.Count == 0)
            {
                departments = bll.Departments.ToList();
            }

            if (departments == null || departments.Count == 0)
            {
                return null;
            }

            Department dp = departments.Find(p=>p.Name == "访客");
            if (dp == null)
            {
                dp = departments.Find(p => p.Name == "未绑定");
            }

            if (dp == null)
            {
                dp = departments[0];
            }

            person.ParentId = dp.Id;//访客

            bool r2 = bll.Personnels.Add(person);

            if (r2)
            {
                bll.BindCardToPerson(person, tag);
                return person;
            }
            return null;
        }
    }
}
