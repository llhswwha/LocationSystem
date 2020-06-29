using AutoCADCommands.ModelExtensions;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DbModel.CADEntitys;
using Dreambuild.AutoCAD;
using Location.Model.InitInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;
using Location.TModel.Tools;
using TModel.Tools;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADCommands
{
    public static class GetRoomsCommand
    {
        private static int roomCount = 0;

        public static void GetInitInfoEx()
        {
            DateTime start = DateTime.Now;

            string txt = Interaction.GetString("输入园区名称");
            if (string.IsNullOrEmpty(txt))
            {
                txt = "园区";
            }

            TopoInfo floor = GetFloorInfoEx();

            InitInfo initInfo = CreateInitInfo(floor, txt);

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;

            string xml = XmlSerializeHelper.GetXmlText(initInfo);
            Gui.TextReport(floor.Name + "|" + t, xml, 700, 500);
        }

        public static void GetInitInfo()
        {
            DateTime start = DateTime.Now;

            string txt = Interaction.GetString("输入园区名称");
            if (string.IsNullOrEmpty(txt))
            {
                txt = "园区";
            }

            TopoInfo floor = GetFloorInfo();

            InitInfo initInfo = CreateInitInfo(floor, txt);

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;

            string xml = XmlSerializeHelper.GetXmlText(initInfo);
            Gui.TextReport(floor.Name + "|" + t, xml, 700, 500);
        }

        private static InitInfo CreateInitInfo(TopoInfo floor,string parkName)
        {
            InitInfo initInfo = new InitInfo();
            TopoInfo root = new TopoInfo("根节点", AreaTypes.区域);
            initInfo.TopoInfo = root;

            TopoInfo park = new TopoInfo(parkName, AreaTypes.园区);//todo:可以扩展成园区、大楼手动设置
            root.AddChild(park);

            //TopoInfo group = new TopoInfo("分组1", AreaTypes.分组);
            //park.AddChild(group);

            TopoInfo building = new TopoInfo("大楼1", AreaTypes.大楼);
            park.AddChild(building);

            building.AddChild(floor);

            BoundInfo buildBound= floor.BoundInfo.CloneByXml();
            buildBound.SetRectangle();//比例调整
            buildBound.Scale(1.05f);//比例调整
            building.BoundInfo = buildBound;//大楼默认和楼层一致

            BoundInfo parkBound= building.BoundInfo.CloneByXml();
            parkBound.SetRectangle();
            parkBound.Scale(2);//园区默认是比大楼周围一圈
            park.BoundInfo = parkBound;

            return initInfo;
        }

        public static void GetRoomInfo()
        {
            DateTime start = DateTime.Now;

            TopoInfo topoInfo = GetRoomEx();

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
            string xml = "";
            if(topoInfo!=null)
                xml = XmlSerializeHelper.GetXmlText(topoInfo,true);
            Gui.TextReport(topoInfo.Name + "|" + t, xml, 700, 500);
        }

        public static void GetRoomsInfo()
        {
            DateTime start = DateTime.Now;

            TopoInfo floor = GetFloorInfo();

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
            string xml = XmlSerializeHelper.GetXmlText(floor);
            Gui.TextReport(floor.Name + "|" + t, xml, 700, 500);
        }

        public static void GetRoomsInfoEx()
        {
            DateTime start = DateTime.Now;

            TopoInfo floor = GetFloorInfoEx();

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
            string xml = XmlSerializeHelper.GetXmlText(floor);
            Gui.TextReport(floor.Name + "|" + t, xml, 700, 500);
        }

        public static TopoInfo GetFloorInfo()
        {
            roomCount = 0;
            TopoInfo floor = GetFloor();
            string txt = "";
            do
            {
                TopoInfo topoInfo = GetRoom();//一个点弄错了要从头开始
                if (topoInfo == null) break;//这里是出口
                floor.AddChild(topoInfo);
            }
            while (string.IsNullOrEmpty(txt) || txt != "n");
            //while (true);
            return floor;
        }

        public static TopoInfo GetFloorInfoEx()
        {
            roomCount = 0;
            TopoInfo floor = GetFloorEx();
            //string xml1 = XmlSerializeHelper.GetXmlText(floor);
            //Gui.TextReport("楼层", xml1, 700, 500);
            string txt = "";
            do
            {
                TopoInfo topoInfo = GetRoomEx();//一个点弄错了要从头开始
                if (topoInfo == null) break;//这里是出口
                floor.AddChild(topoInfo);
                txt = Interaction.GetString("继续？(y)");
            }
            while (string.IsNullOrEmpty(txt) || txt != "n");
            //while (true);
            return floor;
        }

        public static string GetText(Point3d pt1, Point3d pt2)
        {
            //var p1 = Interaction.GetPoint("坐标1");
            //var p2 = Interaction.GetPoint("坐标2");

            string name = "";
            //string txt = "";
            ObjectId[] objs = Interaction.GetCrossingSelection(pt1, pt2);
            CADShapeList sps = new CADShapeList();
            for (int i = 0; i < objs.Length; i++)
            {
                ObjectId item = objs[i];
                var sp = item.ToCADShape(true);
                sps.Add(sp);
                //txt += string.Format("{0}\n", sp);
            }

            sps.SortByXY();//按坐标排序
            foreach (CADShape sp in sps)
            {
                if (sp.Text == "AC")
                {
                    continue; ;
                }
                name += sp.Text;
            }

            //Gui.TextReport("名称:" + name, txt, 700, 500);
            return name.Trim();
        }

        private static TopoInfo GetFloor()
        {
            string txt = Interaction.GetString("输入楼层名称");
            if (string.IsNullOrEmpty(txt))
            {
                txt = "楼层1";
            }

            var p1 = Interaction.GetPoint("楼层坐标1");
            PointInfo pi1 = GetPointInfo(p1);

            var p2 = Interaction.GetPoint("楼层坐标2");
            PointInfo pi2 = GetPointInfo(p2);

            TopoInfo topoInfo = GetTopoInfo(txt, AreaTypes.楼层, pi1, pi2);

            Interaction.WriteLine(string.Format("{0} {1} {2}\n", pi1, pi2, txt));
            //Interaction.WriteLine("");
            return topoInfo;
        }

        private static TopoInfo GetFloorEx()
        {
            string txt = Interaction.GetString("输入楼层名称");
            if (string.IsNullOrEmpty(txt))
            {
                txt = "楼层1";
            }

            int pCout = 0;
            List<PointInfo> pis = new List<PointInfo>();
            while (true)
            {
                pCout++;
                var p1 = Interaction.GetPoint("楼层坐标" + pCout);
                if (double.IsNaN(p1.X))
                {
                    if (pis.Count >= 2)
                    {
                        break; //出口
                    }
                    else
                    {
                        continue;
                    }
                }
                PointInfo pi1 = GetPointInfo(p1);
                pis.Add(pi1);
            }

            //var p2 = Interaction.GetPoint("楼层坐标2");
            //PointInfo pi2 = GetPointInfo(p2);

            TopoInfo topoInfo = GetTopoInfo(txt, AreaTypes.楼层, pis);

            Interaction.WriteLine(string.Format("{0} {1} {2}\n", pis.First(), pis.Last(), txt));
            //Interaction.WriteLine("");
            return topoInfo;
        }

        private static TopoInfo GetTopoInfo(string name, AreaTypes type, List<PointInfo> pis)
        {
            BoundInfo boundInfo = NewBoundInfo();
            boundInfo.Points.AddRange(pis);

            TopoInfo topoInfo = new TopoInfo();
            topoInfo.BoundInfo = boundInfo;
            topoInfo.Name = name;
            topoInfo.Type = type;
            return topoInfo;
        }

        public  static BoundInfo NewBoundInfo()
        {
            BoundInfo boundInfo = new BoundInfo();
            boundInfo.Thickness = 3500;
            boundInfo.IsRelative = false;//都是绝对坐标，不然楼层和大楼会有偏移
            boundInfo.IsCreateAreaByData = true;//必须有
            return boundInfo;
        }

        private static TopoInfo GetTopoInfo(string name, AreaTypes type, params PointInfo[] pis)
        {
            BoundInfo boundInfo = NewBoundInfo();
            boundInfo.Points.AddRange(pis);

            TopoInfo topoInfo = new TopoInfo();
            topoInfo.BoundInfo = boundInfo;
            topoInfo.Name = name;
            topoInfo.Type = type;
            return topoInfo;
        }


        private static TopoInfo GetRoomEx()
        {
            roomCount++;

            int pCout = 0;
            List<Point3d> ps = new List<Point3d>();
            List<PointInfo> pis = new List<PointInfo>();
            double pMinX = double.MaxValue;
            double pMinY = double.MaxValue;
            double pMaxX = double.MinValue;
            double pMaxY = double.MinValue;

            int ibegin = 0;
            Point3d lineBegin = new Point3d();

            while (true)
            {
                pCout++;
                var p1 = Interaction.GetPoint("房间坐标" + pCout);
                if (double.IsNaN(p1.X))
                {
                    if (pis.Count >= 2)
                    {
                        break; //出口
                    }
                    else
                    {
                        continue;
                    }
                }
                ps.Add(p1);
                PointInfo pi1 = GetPointInfo(p1);

                if (ibegin == 0)
                {
                    lineBegin = p1;
                    ibegin = 1;
                }
                else
                {
                    var leftLine = Draw.Line(lineBegin, p1);
                    lineBegin = p1;
                   // leftLine.SetLayer("aaa");

                    leftLine.QOpenForWrite<Entity>(line => line.ColorIndex = 3);
                    var arrow = Modify.Group(new[] { leftLine });   
                }

                pis.Add(pi1);

                if (p1.X < pMinX)
                {
                    pMinX = p1.X;
                }
                if (p1.Y < pMinY)
                {
                    pMinY = p1.Y;
                }

                if (p1.Y > pMaxY)
                {
                    pMaxY = p1.Y;
                }
                if (p1.X > pMaxX)
                {
                    pMaxX = p1.X;
                }

            }

            Point3d pMin = Point3d.Origin;
            Point3d pMax = Point3d.Origin;
            if (pis.Count == 2)
            {
                pMin = ps[0];
                pMax = ps[1];
            }
            else
            {
                pMin=new Point3d(pMinX,pMinY,0);
                pMax=new Point3d(pMaxX,pMaxY,0);
            }

            string room = GetText(pMin, pMax);
            if (string.IsNullOrEmpty(room))
            {
                room = Interaction.GetString("输入房间名称");
                if (string.IsNullOrEmpty(room))
                {
                    room = "房间_" + roomCount;
                }
            }

            TopoInfo topoInfo = GetTopoInfo(room, AreaTypes.机房, pis);

            Interaction.WriteLine(string.Format("{0} {1} {2}", pis.First(), pis.Last(), room));
            Interaction.WriteLine("");

            //string xml = XmlSerializeHelper.GetXmlText(topoInfo);
            //Gui.TextReport("房间:" + txt, xml, 700, 500);

            return topoInfo;
        }

        private static TopoInfo GetRoom()
        {
            roomCount++;

            var p1 = Interaction.GetPoint("房间坐标1");
            if (double.IsNaN(p1.X)) return null;//这里是出口
            var p2 = Interaction.GetPoint("房间坐标2");
            if (double.IsNaN(p2.X)) return null;//这里是出口

            string room = GetText(p1, p2);
            if (string.IsNullOrEmpty(room))
            {
                room = Interaction.GetString("输入房间名称");
                if (string.IsNullOrEmpty(room))
                {
                    room = "房间_" + roomCount;
                }
            }

            PointInfo pi1 = GetPointInfo(p1);
            PointInfo pi2 = GetPointInfo(p2);
            TopoInfo topoInfo = GetTopoInfo(room, AreaTypes.机房, pi1, pi2);

            Interaction.WriteLine(string.Format("{0} {1} {2}", pi1, pi2, room));
            Interaction.WriteLine("");

            //string xml = XmlSerializeHelper.GetXmlText(topoInfo);
            //Gui.TextReport("房间:" + txt, xml, 700, 500);

            return topoInfo;
        }

        public static PointInfo GetPointInfo(Point3d p1)
        {
            PointInfo pi1 = new PointInfo();
            pi1.X = (float)p1.X;
            pi1.Y = (float)p1.Y;
            return pi1;
        }
    }
}
