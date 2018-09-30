using Location.BLL.Blls;
using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class LocationBll : IDisposable
    {
        public LocationDb Db = new LocationDb();

        public LocationHistoryDb DbHistory = new LocationHistoryDb();

        public MapBll Maps { get; set; }

        public AreaBll Areas { get; set; }

        public PositionBll Position { get; set; }

        public TagPositionBll TagPositions { get; set; }

        public DepartmentBll Departments { get; set; }

        public UserBll Users { get; set; }

        public TagBll Tags { get; set; }

        public bool AddPositions(List<Position> positions)
        {
            Position.IsAutoSave = false;
            TagPositions.IsAutoSave = false;

            bool r = true;
            foreach (Position position in positions)
            {
                if (AddPosition(position) == false)
                {
                    r = false;
                    break;
                }
            }

            Position.Save();
            TagPositions.Save();

            Position.IsAutoSave = true;
            TagPositions.IsAutoSave = true;
            return r;
        }

        public bool AddPosition(Position position)
        {
            bool r = false;
            if (position == null)
            {
                r= false;
            }
            if (Position.Add(position))//添加历史数据
            {
                r= EditTagPosition(position);//修改实时数据
                //return true;
            }
            else
            {
                ErrorMessage = Position.ErrorMessage;
                r= false;
            }
            //return EditTagPosition(position);//修改实时数据
            return r;
        }

        public bool EditTagPosition(Position position)
        {
            TagPosition tagPos = TagPositions.FindByCode(position.Tag);//判断是否存在实时数据
            if (tagPos == null)
            {
                TagPosition tagPosition = new TagPosition(position);
                if (TagPositions.Add(tagPosition))//添加新的实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Position.ErrorMessage;
                    return false;
                }
            }
            else
            {
                tagPos.Edit(position);
                if (TagPositions.Edit(tagPos))//修改实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Position.ErrorMessage;
                    return false;
                }
            }
        }

        public string ErrorMessage { get; set; }

        public LocationBll()
        {
            Maps = new MapBll(Db);
            Areas = new AreaBll(Db);
            Position = new PositionBll(DbHistory);
            TagPositions = new TagPositionBll(Db);
            Departments = new DepartmentBll(Db);
            Users = new UserBll(Db);
            Tags = new TagBll(Db);
            Init();
        }

        public void Init()
        {
            if (Departments.ToList().Count == 0)
            {
                List<Department> lists = new List<Department>();
                Department dep1 = new Department() { Name = "机构", ShowOrder = 0, Parent = null };
                Department dep2 = new Department() { Name = "机构1", ShowOrder = 0, Parent = dep1 };
                Department dep3 = new Department() { Name = "机构2", ShowOrder = 1, Parent = dep1 };
                Department dep4 = new Department() { Name = "机构3", ShowOrder = 2, Parent = dep1 };
                Department dep5 = new Department() { Name = "机构4", ShowOrder = 0, Parent = dep2 };
                lists.AddRange(new List<Department>() { dep1, dep2, dep3, dep4, dep5 });
                //for (int i = 0; i < 100; i++)
                //{
                //    int id = 5 + i;
                //    lists.Add(new Department() { Id = id, Name = "机构" + id, ShowOrder = i, Parent = dep3 });
                //}
                Departments.AddRange(lists);

                User user1 = new User() { Name = "管理员", LoginName = "admin", Password = "admin" };
                User user2 = new User() { Name = "用户1", LoginName = "user1", Password = "user1" };
                Users.AddRange(new List<Model.User>() { user1, user2 });

                Map map1 = new Map() { Name = "一楼地图", MinX = 0, MinY = 0, MinZ = 0, MaxX = 2000, MaxY = 2000, MaxZ = 200, Dep = dep1 };
                Map map2 = new Map() { Name = "二楼地图", MinX = 0, MinY = 0, MinZ = 0, MaxX = 2000, MaxY = 2000, MaxZ = 200, Dep = dep2 };
                List<Map> maps = new List<Map>() { map1, map2 };
                Maps.AddRange(maps);

                Area area1 = new Area() { Name = "区域1", MinX = 50, MinY = 50, MinZ = 0, MaxX = 100, MaxY = 100, MaxZ = 200, Map = map1 };
                Area area2 = new Area() { Name = "区域2", MinX = 200, MinY = 200, MinZ = 0, MaxX = 400, MaxY = 400, MaxZ = 200, Map = map1 };

                List<Area> areas = new List<Area>() { area1, area2 };
                Areas.AddRange(areas);

                Tag tag1 = new Tag() { Name = "标签1", Code = "0002"};
                Tag tag2 = new Tag() { Name = "标签2", Code = "0003" };
                List<Tag> tags = new List<Tag>() { tag1, tag2 };
                Tags.AddRange(tags);
            }
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
