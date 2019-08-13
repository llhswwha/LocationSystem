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
        private List<Personnel> personnels;
        private List<LocationCardToPersonnel> tagToPersons;
        private List<LocationCard> tags;
        private List<CardRole> roles;
        private List<Archor> archors;
        private List<Area> areas;
        private Bll bll;
        private Dictionary<string, LocationCard> locationCardDic;

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

        }

        public void RefreshTags()
        {
            personnels = bll.Personnels.ToList();
            tagToPersons = bll.LocationCardToPersonnels.ToList();
            tags = bll.LocationCards.ToList();
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

        private void SetArea(Position pos)
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
                        area = areas[1];
                        SetAreaByPosition(pos, area);
                    }
                }
                else
                {
                    var area = areas[1];
                    SetAreaByPosition(pos, area);
                }

                if (pos.Code == "092D" && pos.AreaId == 2)
                {
                    int i = 0;
                }

                if (pos.IsAreaNull())
                {
                    Log.Info("pos.IsAreaNull()");
                }
            }
            catch (Exception ex)
            {
                Log.Error("TagRelationBuffer.SetArea", ex);
            }
            
        }

        private static void SetAreaByPosition(Position pos, Area area)
        {
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
                        pos.AreaId = 2;//设为园区
                        int nn = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("TagRelationBuffer.SetAreaByPosition", ex);
            }
      
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

        private static void SetAreaInFloor(Position pos, Area area)
        {
            var building = area.Parent;
            var containsAreas = new List<Area>();
            var childrenArea = area.Children;
            foreach (var item in childrenArea)
            {
                if (item.Type == AreaTypes.CAD) continue;//过滤掉柱子等CAD物体
                var x = pos.X - area.InitBound.MinX - building.InitBound.MinX;
                var y = pos.Z - area.InitBound.MinY - building.InitBound.MinY;
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
            //boundAreas.Add(area);

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

            Area inArea = null;
            //todo:加上建筑外的区域
            if (containsAreas.Count > 0)
            {
                ////inArea = containsAreas[0];
                ////pos.SetArea(inArea);
                //if (containsAreas.Count == 1)
                //{
                //    inArea = containsAreas[0];
                //}
                //else
                //{
                //    Area areaT = containsAreas.Find((i) => i.IsOnLocationArea == true);
                //    if (areaT == null)
                //    {
                //        //pos.SetArea(containsAreas[0]);
                //        areaT = containsAreas[0];
                //    }
                //    inArea = areaT;
                //}
                //pos.SetArea(inArea);//同时处于一个告警区域和一个定位区域时 人员区域怎么判断？ 同时处于两个区域时 人员区域怎么判断?

                var areaNode = containsAreas.Find(i => i.Type != AreaTypes.范围);
                if (areaNode == null)
                {
                    containsAreas.Add(park);
                    if (pos.Code == "092D" && pos.AreaId == 2)
                    {
                        int i = 0;
                    }
                }
                inArea=pos.SetArea(containsAreas.ToArray());
            }
            if (inArea == null)
            {
                if (park.InitBound.Contains(pos.X, pos.Z))
                {
                    inArea = park;
                    pos.SetArea(inArea);
                    if (pos.Code == "092D" && pos.AreaId == 2)
                    {
                        int i = 0;
                    }
                }
            }
            if (inArea == null)
            {
                Log.Info("inArea == null");
            }
            return inArea;
        }

        private void SetTagAndPerson(Position pos)
        {
            if (tags == null) return;
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
                    else//关联的人员不在了 ，误删了的话，先补回来上吧
                    {
                        var person = AddPersonByTag(pos, tag);
                        if (person != null)
                        {
                            pos.PersonnelID = person.Id;
                        }
                    }
                }
            }
            else//新的定位卡 不存在人员 ；策略，添加一个标签 同时绑定一个人员 ，后续可以改成绑定其他人员。
            {
                tag = AddTagByPos(pos);
                RefreshTags();
            }
        }

        public LocationCard AddTagByPos(Position pos)
        {
            LocationCard tag = new LocationCard();
            tag.Name = pos.Code;
            tag.Code = pos.Code;
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
            person.ParentId = 6;//访客
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
