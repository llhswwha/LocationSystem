using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using DbModel.Tools;
using DbModel.Tools.InitInfos;

namespace BLL.Initializers
{
    public class AuthorizationInitializer
    {
        private Bll _bll;
        public AuthorizationInitializer(Bll bll)
        {
            _bll = bll;
        }

        private AreaBll Areas => _bll.Areas;

        private AreaAuthorizationBll AreaAuthorizations => _bll.AreaAuthorizations;

        private AreaAuthorizationRecordBll AreaAuthorizationRecords => _bll.AreaAuthorizationRecords;


        private List<CardRole> roles;

        private List<AreaAuthorization> areaAuthorizations;

        private List<AreaAuthorizationRecord> authorizationRecords;

        private List<AuthorizationArea> authorizationAreas;

        private List<Area> areas; 

        public void InitAuthorization(List<CardRole> roles)
        {
            this.roles = roles;

            areaAuthorizations = new List<AreaAuthorization>();
            authorizationRecords = new List<AreaAuthorizationRecord>();
            //区域权限列表
            authorizationAreas = new List<AuthorizationArea>();
            areas = Areas.ToList();

            AreaAuthorizationRecords.Clear();
            AreaAuthorizations.Clear();

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\AuthorizationTree.xml";

            //if (File.Exists(path))
            //{
            //    InitAuthorizationFromFile(path);
            //}
            //else
            {
                InitAuthorizationFromAreas(path);
            }
        }

        private void InitAuthorizationFromAreas(string path)
        {
            CreateAllAuthorization();//所有的区域创建权限

            //CreateAllAuthorizationRecord();//所有的角色分配权限

            {//AddCardRole("超级管理员", "可以进入全部区域");
                var aa = new AreaAuthorization();
                aa.AreaId = 1;//根节点
                aa.AccessType = AreaAccessType.EnterLeave; //可进入的权限
                aa.RangeType = AreaRangeType.All;
                aa.Name = string.Format("权限[全部区域]");
                aa.Description = string.Format("权限：可以进入全部区域");
                aa.CreateTime = DateTime.Now;
                aa.ModifyTime = DateTime.Now;
                aa.RepeatDay = RepeatDay.All;
                aa.TimeType = TimeSettingType.TimeRange;
                aa.SetTime(0, 0, 23, 59, 59);
                areaAuthorizations.Add(aa);
                AreaAuthorizations.Add(aa);

                var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                var aar = new AreaAuthorizationRecord(aa, roles[0]);
                authorizationRecords.Add(aar);
                aa2.Records.Add(aar);

                AreaAuthorizationRecords.Add(aar);
            }

            var parks = areas.FindAll(i => i.Type == AreaTypes.园区);

            {
                foreach (var area in parks)
                {
                    var role = roles[7];
                    //  role7 = AddCardRole("参观人员(一般)", "能够进入生活区域和少部分生产区域");
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;//根节点
                    aa.AccessType = AreaAccessType.EnterLeave; //可进入的权限
                    aa.RangeType = AreaRangeType.Single;
                    aa.Name = string.Format("权限[园区参观(一般)]");
                    aa.Description = string.Format("权限：可以进入园区参观，不能靠近建筑物。");
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.SetTime(8, 30, 17, 30);
                    areaAuthorizations.Add(aa);
                    AreaAuthorizations.Add(aa);

                    var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);

                    AreaAuthorizationRecords.Add(aar);
                }
                
            }

            var buildGroup = areas.FindAll(i => i.Type == AreaTypes.分组);
            var buildings = areas.FindAll(i => i.Type == AreaTypes.大楼);
            var list = new List<Area>();
            list.AddRange(buildGroup);
            list.AddRange(buildings);

            {
                var role = roles[6];
                //role6 = AddCardRole("参观人员(高级)", "能够进入生活区域和大部分生产区域");
                foreach (var area in buildings)
                {
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;//根节点
                    aa.AccessType = AreaAccessType.EnterLeave; //可进入的权限
                    aa.RangeType = AreaRangeType.WithParent;
                    aa.Name = string.Format("权限[园区参观(高级)]");
                    aa.Description = string.Format("权限：可以进入园区参观，可以靠近建筑物。");
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.SetTime(8, 30, 17, 30);
                    areaAuthorizations.Add(aa);

                    AreaAuthorizations.Add(aa);

                    var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);

                    AreaAuthorizationRecords.Add(aar);
                }
            }


            var floors = areas.FindAll(i => i.Type == AreaTypes.楼层);
            {
                var role = roles[5];
                //role5 = AddCardRole("外维人员", "能够进入生活区域和指定生产区域");
                foreach (var area in floors)
                {
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;//根节点
                    aa.AccessType = AreaAccessType.EnterLeave; //可进入的权限
                    aa.RangeType = AreaRangeType.WithParent;
                    aa.Name = string.Format("权限[大楼内部]");
                    aa.Description = string.Format("权限：可以进入大楼内部，不能进入机房。");
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.SetTime(8, 30, 17, 30);
                    areaAuthorizations.Add(aa);

                    AreaAuthorizations.Add(aa);

                    var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);

                    AreaAuthorizationRecords.Add(aar);
                }
            }

            /*
            role1 = AddCardRole("管理人员", "可以进入大部分区域");
            role2 = AddCardRole("巡检人员", "能够进入生产区域");
            role3 = AddCardRole("操作人员", "能够进入生产区域");
            role4 = AddCardRole("维修人员", "能够进入生产区域");
             */

            var rooms = areas.FindAll(i => i.Type == AreaTypes.机房);
            for (int j = 1; j <= 4 ; j++)
            {
                var role = roles[j];
                foreach (var area in rooms)
                {
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;//根节点
                    aa.AccessType = AreaAccessType.EnterLeave; //可进入的权限
                    aa.RangeType = AreaRangeType.WithParent;
                    aa.Name = string.Format("权限[机房]");
                    aa.Description = string.Format("权限：可以进入机房。");
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.SetTime(8, 30, 17, 30);
                    areaAuthorizations.Add(aa);

                    AreaAuthorizations.Add(aa);

                    var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);

                    AreaAuthorizationRecords.Add(aar);
                }
            }

            var ranges = areas.FindAll(i => i.Type == AreaTypes.范围);

            for (int j = 0; j < roles.Count; j++)
            {
                var role = roles[j];
                foreach (var area in ranges)
                {
                    var aa = new AreaAuthorization();
                    aa.AreaId = area.Id;//根节点
                    aa.AccessType = AreaAccessType.Leave; //不能进入
                    aa.RangeType = AreaRangeType.Single;
                    aa.Name = string.Format("权限[不能进入]");
                    aa.Description = string.Format("权限：告警区域");
                    aa.CreateTime = DateTime.Now;
                    aa.ModifyTime = DateTime.Now;
                    aa.RepeatDay = RepeatDay.All;
                    aa.TimeType = TimeSettingType.TimeRange;
                    aa.SetTime(8, 30, 17, 30);
                    areaAuthorizations.Add(aa);

                    AreaAuthorizations.Add(aa);

                    var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);

                    AreaAuthorizationRecords.Add(aar);
                }
            }

            //roles[0]

            var tree = TreeHelper.CreateTree(authorizationAreas);
            XmlSerializeHelper.Save(tree[0], path);
            //tree[0]

            //角色,区域，卡
            //1.可以进入全部区域
            //2.可以进入生产区域
            //3.可以进入非生产区域
            //4.可以进入多个区域
            //5.可以进入某一个楼层
            //6.可以进入某个房间

            //AreaAuthorizations.Add(new AreaAuthorization() {})
        }

        private void CreateAllAuthorizationRecord()
        {
//权限指派给标签角色
            foreach (var aa in areaAuthorizations)
            {
                var aa2 = authorizationAreas.Find(i => i.Id == aa.AreaId);
                foreach (var role in roles)
                {
                    var aar = new AreaAuthorizationRecord(aa, role);
                    authorizationRecords.Add(aar);
                    aa2.Records.Add(aar);
                }
                //1.超级管理员能够进入全部区间
                //2.管理人员也能进入全部区域
                //3.巡检人员和维修人员能够进入生产区域
                //4.参观人员（高级）能够进入生活区域和大部分生产区域
                //5.参观人员（一般）能够进入生活区域和少部分生产区域
            }

            AreaAuthorizationRecords.AddRange(authorizationRecords);
        }

        private void CreateAllAuthorization()
        {
            foreach (var area in areas)
            {
                var aa2 = new AuthorizationArea();
                aa2.Id = area.Id;
                aa2.Name = area.Name;
                aa2.ParentId = area.ParentId ?? 0;
                authorizationAreas.Add(aa2);
                if (area.InitBound == null) continue;

                var accTypes = Enum.GetValues(typeof (AreaAccessType));
                foreach (AreaAccessType accType in accTypes)
                {
                    var aa = CreateAreaAuthorization(area, accType);
                    areaAuthorizations.Add(aa);
                    aa2.Items.Add(aa);
                }
            }

            bool r1 = AreaAuthorizations.AddRange(areaAuthorizations);
        }

        private static AreaAuthorization CreateAreaAuthorization(Area area, AreaAccessType accType)
        {
            var aa = new AreaAuthorization();
            aa.AreaId = area.Id;
            //aa.Area = area;
            aa.AccessType = accType; //可进入的权限
            aa.RangeType = AreaRangeType.WithParent;
            aa.Description = string.Format("权限[{0}][{1}]", accType, area.Name);
            aa.Name = string.Format("权限：[{0}]区域‘{1}’", accType, area.Name);
            aa.CreateTime = DateTime.Now;
            aa.ModifyTime = DateTime.Now;
            aa.RepeatDay = RepeatDay.All;
            aa.TimeType = TimeSettingType.TimeRange;
            aa.SetTime(8, 30, 17, 30);
            return aa;
        }

        private void InitAuthorizationFromFile(string path)
        {
            var aaList = new List<AreaAuthorization>();
            var aarList = new List<AreaAuthorizationRecord>();
            var tree = XmlSerializeHelper.LoadFromFile<AuthorizationArea>(path);
            var list = tree.GetAllChildren(null);
            foreach (var area in list)
            {
                if (area.Items != null)
                {
                    foreach (var aa in area.Items)
                    {
                        aa.CreateTime = DateTime.Now;
                        aa.ModifyTime = DateTime.Now;
                        aaList.Add(aa);
                    }
                }
            }
            bool r1 = AreaAuthorizations.AddRange(aaList);

            foreach (var area in list)
            {
                if (area.Records != null)
                {
                    foreach (var ar in area.Records)
                    {
                        ar.CreateTime = DateTime.Now;
                        ar.ModifyTime = DateTime.Now;
                        aarList.Add(ar);
                    }
                }
            }
            AreaAuthorizationRecords.AddRange(aarList);
        }
    }
}
