using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BLL.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using ExcelLib;
using Location.BLL.Tool;
using Location.TModel.Tools;
using System.Threading;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using DbModel.Location.Authorizations;
using DbModel.Location.Work;

namespace BLL
{
    /// <summary>
    /// 数据库初始化类
    /// </summary>
    public class DbInitializer
    {
        private readonly Bll _bll;

        public CardRoleBll CardRoles => _bll.CardRoles;

        public LocationCardBll LocationCards => _bll.LocationCards;

        public LocationCardPositionBll LocationCardPositions => _bll.LocationCardPositions;

        public PositionBll Positions => _bll.Positions;

        public AreaBll Areas => _bll.Areas;

        public AreaAuthorizationBll AreaAuthorizations => _bll.AreaAuthorizations;

        public AreaAuthorizationRecordBll AreaAuthorizationRecords => _bll.AreaAuthorizationRecords;

        public DbInitializer(Bll bll)
        {
            _bll = bll;
        }

        public void InitData()
        {
            //InitKKSCode();
            InitTagPositions();
            //定位标签
            //InitDepartments();
            //机构、地图、区域、标签
            //InitUsers();
            //登录人员
            //InitAreaAndDev();
            //区域、设备
            //InitConfigArgs();
            //配置信息
            //InitDevModelAndType();
            //InitAuthorization();
        }

        private CardRole AddCardRole(string name, string description = "")
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }
            var role1 = new CardRole() { Name = name, Description = description };
            CardRoles.Add(role1);
            return role1;
        }

        public void InitTagPositions()
        {
            DateTime dt = DateTime.Now;
            long TimeStamp = TimeConvert.DateTimeToTimeStamp(dt);

            var role1 = AddCardRole("超级管理员", "特殊角色，可以进入全部区域。");
            var role2 = AddCardRole("管理人员");
            var role3 = AddCardRole("巡检人员");
            var role4 = AddCardRole("操作人员");
            var role5 = AddCardRole("维修人员");
            var role6 = AddCardRole("参观人员");

            Log.InfoStart("InitTagPositions");
            var tag1 = new LocationCard() { Name = "标签1", Code = "0002", CardRoleId = role1.Id };
            var tag2 = new LocationCard() { Name = "标签2", Code = "0003", CardRoleId = role2.Id };
            var tag3 = new LocationCard() { Name = "标签3", Code = "0004", CardRoleId = role3.Id };
            var tag4 = new LocationCard() { Name = "标签4", Code = "0005", CardRoleId = role3.Id };
            var tag5 = new LocationCard() { Name = "标签5", Code = "0006", CardRoleId = role4.Id };
            var tag6 = new LocationCard() { Name = "标签6", Code = "0007", CardRoleId = role4.Id };
            var tag7 = new LocationCard() { Name = "标签7", Code = "0008", CardRoleId = role5.Id };
            var tag8 = new LocationCard() { Name = "标签8", Code = "0009", CardRoleId = role6.Id };
            List<LocationCard> tags = new List<LocationCard>() { tag1, tag2, tag3, tag4, tag5, tag6, tag7, tag8 };
            LocationCards.AddRange(tags);
            List<LocationCard> tagsT = new List<LocationCard>();
            //for (int i = 0; i < 100; i++)
            //{
            //    var tagT = new LocationCard() { Name = "标签T"+ i.ToString(), Code = "0000" + i.ToString() };
            //    tagsT.Add(tagT);
            //}

            LocationCards.AddRange(tagsT);


            var tagposition1 = new LocationCardPosition() { CardId = 1, Code = "0002", X = 2293.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 1 };
            var tagposition2 = new LocationCardPosition() { CardId = 2, Code = "0003", X = 2294.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 2 };
            var tagposition3 = new LocationCardPosition() { CardId = 3, Code = "0004", X = 2295.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 3 };
            var tagposition4 = new LocationCardPosition() { CardId = 4, Code = "0005", X = 2296.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 4 };
            var tagposition5 = new LocationCardPosition() { CardId = 5, Code = "0006", X = 2297.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 5 };
            var tagposition6 = new LocationCardPosition() { CardId = 6, Code = "0007", X = 2298.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 6 };
            var tagposition7 = new LocationCardPosition() { CardId = 7, Code = "0008", X = 2299.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 7 };
            var tagposition8 = new LocationCardPosition() { CardId = 8, Code = "0009", X = 2300.5f, Y = 2, Z = 1715.5f, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0", AreaId = 2, PersonId = 8 };

            List<LocationCardPosition> tagpositions = new List<LocationCardPosition>() { tagposition1, tagposition2, tagposition3, tagposition4, tagposition5, tagposition6, tagposition7, tagposition8 };
            LocationCardPositions.AddRange(tagpositions);

            Position position1 = new Position() { Code = "002", X = -50, Y = -50, Z = -50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position2 = new Position() { Code = "002", X = 0, Y = 0, Z = 0, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position3 = new Position() { Code = "002", X = 50, Y = 50, Z = 50, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position4 = new Position() { Code = "002", X = 100, Y = 100, Z = 100, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position5 = new Position() { Code = "002", X = 150, Y = 150, Z = 150, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position6 = new Position() { Code = "002", X = 200, Y = 200, Z = 200, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position7 = new Position() { Code = "002", X = 250, Y = 250, Z = 250, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position8 = new Position() { Code = "002", X = 300, Y = 300, Z = 300, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position9 = new Position() { Code = "002", X = 350, Y = 350, Z = 350, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position10 = new Position() { Code = "002", X = 400, Y = 400, Z = 400, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position11 = new Position() { Code = "002", X = 500, Y = 500, Z = 450, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position12 = new Position() { Code = "002", X = 600, Y = 600, Z = 500, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position13 = new Position() { Code = "002", X = 700, Y = 700, Z = 550, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position14 = new Position() { Code = "002", X = 800, Y = 800, Z = 600, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position15 = new Position() { Code = "002", X = 900, Y = 900, Z = 650, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:1" };
            Position position16 = new Position() { Code = "002", X = 1100, Y = 1100, Z = 700, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position17 = new Position() { Code = "002", X = 1200, Y = 1200, Z = 750, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position18 = new Position() { Code = "002", X = 1300, Y = 1300, Z = 800, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position19 = new Position() { Code = "002", X = 1400, Y = 1400, Z = 850, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            Position position20 = new Position() { Code = "002", X = 1500, Y = 1500, Z = 900, DateTime = dt, DateTimeStamp = TimeStamp, Power = 0, Number = 0, Flag = "0:0:0:0:0" };
            List<Position> positions = new List<Position>() { position1, position2, position3, position4, position5, position6, position7, position8, position9, position10, position11, position12, position13, position14, position15, position16, position17, position18, position19, position20 };
            Positions.AddRange(positions);
            Log.InfoEnd("InitTagPositions");
        }


        public void InitAuthorization()
        {
            //AreaAccessRule
            var areas = Areas.ToList();
            foreach (var area in areas)
            {
                var aa = new AreaAuthorization();
                aa.AreaId = aa.Id;
                aa.Area = area;
                aa.AccessType = AreaAccessType.Enter;
                aa.RangeType = AreaRangeType.Single;
                aa.Description = string.Format("{0}权限", area.Name);
                aa.Name = string.Format("{0}权限", area.Name);
                aa.CreateTime = DateTime.Now;
                aa.ModifyTime = DateTime.Now;
                aa.RepeatDay = RepeatDay.All;
                aa.TimeType = TimeSettingType.TimeRange;
                aa.StartTime = new DateTime(2000, 0, 0, 8, 30, 0);
                aa.EndTime = new DateTime(2000, 0, 0, 17, 30, 0);
                AreaAuthorizations.Add(aa);

                var aar = new AreaAuthorizationRecord(aa);
                AreaAuthorizationRecords.Add(aar);
            }

            //角色,区域，卡
            //1.可以进入全部区域
            //2.可以进入生产区域
            //3.可以进入非生产区域
            //4.可以进入多个区域
            //5.可以进入某一个楼层
            //6.可以进入某个房间

            //AreaAuthorizations.Add(new AreaAuthorization() {})
        }
    }
}
