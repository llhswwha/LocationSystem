using AutoCADCommands.ModelExtensions;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DbModel.CADEntitys;
using Dreambuild.AutoCAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace AutoCADCommands
{
    public class MyCmds
    {
        [CommandMethod("MyCmds")]
        public static void Help()
        {
            string txt = "PT:测试获取坐标\n" +
                         "EntityPoints:获取图形的坐标信息\n" +
                         "ColumnPointsEx:获取柱子坐标信息\n" +
                         "Anchors:获取基站坐标信息（某一楼层的)\n" +
                         "AllAnchors:获取基站坐标信息（1F,2F,3F,4F)";
            ;
            Gui.TextReport("Help", txt, 700, 500);
        }

        [CommandMethod("TextReport")]
        public static void TextReport()
        {
            Gui.TextReport("title", "content", 200, 300, false);
        }

        [CommandMethod("ExtentPoints")]
        public static void GetExtentPoints()
        {
            var objId=Interaction.GetEntity("Entity");
            CADShape shape = objId.ToCADShape(true);
            if (shape != null)
            {
                string xml = shape.ToXml();
                Gui.TextReport("Points", xml, 700, 500);
            }
            else
            {
                Gui.TextReport("Points", "NULL", 700, 500);
            }
        }

        [CommandMethod("LinePoints")]
        public static void GetLinePoints()
        {
            var objId = Interaction.GetEntity("Entity");
            var line=objId.QOpenForRead<Line>();
            string xml = string.Format("{0},{1}", line.StartPoint, line.EndPoint);
            Gui.TextReport("Points", xml, 700, 500);
        }

        [CommandMethod("EntityPoints")]
        public static void GetEntityPoints()
        {
            var objId = Interaction.GetEntity("Entity");
            var sp = objId.ToCADShape(true);
            string xml = sp.ToXml();
            Gui.TextReport("Points", xml, 700, 500);
        }

        [CommandMethod("ColumnPoints")]
        public static void GetColumnPoints()
        {
            CADAreaList areaList = new CADAreaList();
            var zero=Interaction.GetPoint("ZeroPoint");
            string[] keys = { "0:左下", "1:右下", "2:右上", "3:左上" };
            var key = Interaction.GetKeywords("\nChoose Zero Type", keys);

            var columns = Interaction.GetEntitysByLayers("COLUMN");
            var area = columns.ToCADArea(zero,key);
            area.Name = "主厂房0m层";
            areaList.Add(area);
            var txt = areaList.ToXml();
            Gui.TextReport("Points", txt, 700, 500);

        }

        [CommandMethod("ColumnPointsEx")]
        public static void GetColumnPointsEx()
        {
            CADAreaList areaList = new CADAreaList();
            var zero = Interaction.GetPoint("ZeroPoint");
            string[] keys = { "0:左下", "1:右下", "2:右上", "3:左上" };
            var key = Interaction.GetKeywords("\nChoose Zero Type", keys);

            var columns = Interaction.GetEntitysByLayers("COLUMN");
            var area = columns.ToCADArea(zero, key);
            area.Name = "主厂房0m层";
            areaList.Add(area);
            var txt = areaList.ToXml();
            Gui.TextReport("Points", txt, 700, 500);
        }

        [CommandMethod("PT")]
        public static void GetPoint1()
        {
            var p = Interaction.GetPoint("Point");
            var p2 = p.ToWCS();
            var p3 = p2.ToUCS();
            string txt = string.Format("P1(Original):{0}\nP2(WCS世界坐标):{1}\nP3(UCS局部坐标):{2}", p, p2, p3);
            Gui.TextReport("PT", txt, 700, 500);
        }

        [CommandMethod("PT2")]
        public static void GetPoint2()
        {
            var p = Interaction.GetPoint("Point");
            Interaction.WriteLine("P1(Original):" + p.ToString());
            MyTool.ShowWCSAndUCS(p);
        }

        [CommandMethod("UCSOrigin")]
        public static void GetUCS()
        {
            var ucs = MyTool.GetUCS();
            Interaction.WriteLine("Origin:" + ucs.Origin);
        }

        [CommandMethod("CopyCircle")]
        public static void CopyCircle()
        {
            var entityId = Interaction.GetEntity("Circle");
            var entity = entityId.QOpenForRead<Circle>();
            if (entity == null)
            {
                Interaction.Write("Not Circle");
                return;
            }
            var p = entity.GetCenter();
            var radius = Interaction.GetString("Radius");
            var cid = Draw.Circle(p, radius.ToInt());
            var zeroSp = cid.ToCADShape(true);
            Interaction.Write(zeroSp.GetPoint().ToString());
        }

        /// <summary>
        /// 设置当前用户坐标系为世界坐标系
        /// </summary>
        [CommandMethod("SetUCS2WCS")]
        public static void SetUCS2WCS()
        {
            MyTool.SetUCS2WCS();
        }

        [CommandMethod("NewUCS")]
        public static void NewUCS()
        {
            MyTool.NewUCS();
        }

        [CommandMethod("AddCircle")]
        public static void AddCircle()
        {
            var p = Interaction.GetPoint("Point");
            var radius = Interaction.GetString("Radius");
            var cid = Draw.Circle(p.ToWCS(), radius.ToInt());
            var zeroSp = cid.ToCADShape(true);
            Interaction.Write(zeroSp.GetPoint().ToString());
        }

        [CommandMethod("Anchors")]
        public static void GetAnchors()
        {
            var zero = Interaction.GetPoint("选择原点");
            //string[] keys = { "1F", "2F", "3F", "4F" };
            //var key = Interaction.GetKeywords("\n选择楼层", keys);
            var anchorObjects = Interaction.GetEntitysByLayers("-人员定位");
            CADShapeList sps = new CADShapeList();
            for (int i = 0; i < anchorObjects.Length; i++)
            {
                ObjectId item = anchorObjects[i];
                var sp = item.ToCADShape(true);
                sps.Add(sp);
                Interaction.WriteLine(string.Format("{0}({1}/{2})", sp, i + 1, anchorObjects.Length));
            }

            var types = sps.GetTypesEx();

            //var circleList = types["Circle"];
            //var zeroCircle = circleList[0];
            //var zeroP = zeroCircle.GetPoint();

            var pZero = zero.ToCADPoint(false);//获取的坐标原本就是用户坐标系的
            foreach (CADShape sp in sps)
            {
                sp.SetZero(pZero);
            }

            var anchorList = types["BlockReference"];
            var textList = types["MText"];

            CADAnchorList result = new CADAnchorList();

            for (int i = 0; i < anchorList.Count; i++)
            {
                var anchor = anchorList[i];
                var text= textList.FindCloset(anchor);
                if (text != null)
                {
                    //if (text.Text.Contains(key))
                    {
                        anchor.Text = text.Text;
                        anchor.Name = text.Text;
                        result.Anchors.Add(anchor);
                    }
                }
            }

            result.Anchors.Sort();
            for (int i = 0; i < result.Anchors.Count; i++)
            {
                result.Anchors[i].Num = i + 1;
            }

            var txt = result.ToXml();
            Gui.TextReport("Anchors", txt, 700, 500);
        }


        [CommandMethod("ShowLayers")]
        public static void ShowLayers()
        {

        }

        [CommandMethod("ClearLayers")]
        public static void ClearLayers()
        {

        }

        /// <summary>
        /// 从一个线获取全部的
        /// </summary>
        [CommandMethod("ShapeInfo")]
        public static void GetShapeInfo()
        {
            var id = Interaction.GetEntity("Select:");
            var sp = id.ToCADShape(true);
            var xml = sp.ToXml();
            Gui.TextReport("ShapeInfo", xml, 700, 500);
        }

        /// <summary>
        /// 从一个线获取全部的
        /// </summary>
        [CommandMethod("PointLine")]
        public static void GetLineOfPoint()
        {
            var p = Interaction.GetPoint("GetLineOfPoint");
            Interaction.WriteLine(p.ToString());
            var p2=Interaction.GetLineEndPoint("otherPoint", p);
            Interaction.WriteLine(p2.ToString());
        }

        /// <summary>
        /// 从一个线获取全部的
        /// </summary>
        [CommandMethod("PointLines")]
        public static void GetLinesOfPoint()
        {
            var p = Interaction.GetPoint("GetLineOfPoint");
            //Interaction.GetEntity()
            //Interaction.
            Interaction.GetEntitysByLayers();
            var ids=QuickSelection.SelectAll();
            foreach (ObjectId id in ids)
            {
                
            }
        }

        /// <summary>
        /// 从一个线获取全部的
        /// </summary>
        [CommandMethod("Bounds")]
        public static void GetBounds()
        {
            //var p = Interaction.GetPoint("StartPoint");
            var entityId = Interaction.GetEntity("StartLine");
            var sp = entityId.ToCADShape(true);
            
        }

        /// <summary>
        /// 将室外基站测量的坐标位置读取并显示
        /// </summary>
        [CommandMethod("LoadPoints")]
        public static void LoadPoints()
        {
            string txt=Gui.InputBox("输入坐标");
            double size=Interaction.GetValue("点大小", 2);
            double height = Interaction.GetValue("文字高度", 2);
            string key = Interaction.GetString("搜索", "");
            string[] lines = txt.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(',');
                string name = parts[0];
                double x = parts[2].ToDouble();
                double y = parts[3].ToDouble();
                double z = parts[4].ToDouble();

                if (string.IsNullOrEmpty(key) || name == key)
                {
                    Point3d point = new Point3d(x, y, z);
                    Draw.Point(point);
                    if (size > 0)
                    {
                        Draw.Circle(point, size);
                    }
                    if (height > 0)
                    {
                        Draw.Text(name, height, point);
                    }
                }
            }
        }
    }
}
