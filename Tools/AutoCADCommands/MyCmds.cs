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
using TModel.Tools;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.EditorInput;

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

        //[CommandMethod("ColumnPoints")]
        //public static void GetColumnPoints()
        //{
        //    CADAreaList areaList = new CADAreaList();
        //    var zero=Interaction.GetPoint("ZeroPoint");
        //    string[] keys = { "0:左下", "1:右下", "2:右上", "3:左上" };
        //    var key = Interaction.GetKeywords("\nChoose2 Zero Type", keys);

        //    var columns = Interaction.GetEntitysByLayers("COLUMN");
        //    var area = columns.ToCADArea(zero.ToCADPoint(true), key,true,false);
        //    area.Name = "主厂房0m层";
        //    areaList.Add(area);
        //    var txt = areaList.ToXml();
        //    Gui.TextReport("Points", txt, 700, 500);

        //}

        [CommandMethod("ColumnPointsEx")]
        public static void GetColumnPointsEx()
        {
            try
            {
                CADAreaList areaList = new CADAreaList();
                var zero = Interaction.GetPoint("ZeroPoint");
                string[] keys = { "0:左下", "1:右下", "2:右上", "3:左上" };
                string p1 = zero.ToString();
                string p2 = zero.ToUCS().ToString();
                string p3 = zero.ToWCS().ToString();
                Interaction.WriteLine(string.Format("\n zero [{0},{1},{2}]", p1, p2, p3));
                var key = Interaction.GetKeywords("\nChoose1 Zero Type ", keys);
                if (string.IsNullOrEmpty(key)) return;
                //var key = Interaction.GetKeywords("\nChoose1 Zero Type ", keys);
                var columns = Interaction.GetEntitysByLayers("COLUMN");
                var area = columns.ToCADArea(zero.ToCADPoint(true), key, true,true);
                area.Name = "主厂房0m层";
                areaList.Add(area);
                var txt = areaList.ToXml();
                Gui.TextReport("Points", txt, 700, 500);
            }
            catch (System.Exception ex)
            {
                Gui.TextReport("Exception", ex.ToString(), 700, 500);
            }
            
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
            try
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

                string typesText = "";
                foreach (var item in types)
                {
                    typesText += item.Key + ",";
                }

                Interaction.Write("Types:" + typesText);

                //var circleList = types["Circle"];
                //var zeroCircle = circleList[0];
                //var zeroP = zeroCircle.GetPoint();

                var pZero = zero.ToCADPoint(false);//获取的坐标原本就是用户坐标系的
                foreach (CADShape sp in sps)
                {
                    sp.SetZero(pZero);
                }

                CADShapeList anchorList = new CADShapeList();
                if (types.ContainsKey("BlockReference"))
                {
                    anchorList = types["BlockReference"];
                }

                CADShapeList textList = new CADShapeList();
                if (types.ContainsKey("MText"))
                {
                    textList = types["MText"];
                }

                CADAnchorList result = new CADAnchorList();

                for (int i = 0; i < anchorList.Count; i++)
                {
                    var anchor = anchorList[i];
                    var text = textList.FindCloset(anchor);
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
            catch (System.Exception ex)
            {

                Gui.TextReport("Exception", ex.ToString(), 700, 500);
            }
            
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
        /// 厂区 //add by qclei 2020-04-30
        /// </summary>
        [CommandMethod("GetBuild")]
        public static void GetBuildInfo()
        {
            GetCADOtherCommands.GetParkBuild("厂区");

            return;
        }
                                  
        /// <summary>
        /// 获取当前图层下面所有“多边型”图形 //add by qclei 2020-04-30
        /// </summary>
        [CommandMethod("GetLayerInfo")]
        public static void GetLayerInfo()
        {            
            GetCADOtherCommands.GetAllShapeByLayer();

            return;
        }        


        /// <summary>
        /// 从一个线获取全部的
        /// </summary>
        [CommandMethod("ShapeInfo")]
        public static void GetShapeInfo()
        {
            var id = Interaction.GetEntity("Select:");
            var sp = id.ToCADShape(true);

            //add by qclei 2020-04-29 判断是否是封闭的图形
            //bool bl = PulicGadget.ifColseShape(sp);
            //if (bl)
            {
                string name = PulicGadget.getShapeName(sp);
                if (name != "") sp.Name = name;
                var xml = sp.ToXml();

                Gui.TextReport("ShapeInfo", xml, 700, 500);
            }

        }

        /// <summary>
        /// 手动连续获取 封闭多边型 //add by qclei 2020-04-30
        /// </summary>
        [CommandMethod("ShapeInfos")]
        public static void GetShapeInfos()
        {
            GetCADOtherCommands.GetAllShapeByManual();
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
            if (entityId == null) return;
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

        /// <summary>
        /// 获取一个机房的坐标
        /// </summary>
        [CommandMethod("GetRoom")]
        public static void GetRoom()
        {
            GetRoomsCommand.GetRoomInfo();
        }

        /// <summary>
        /// 获取机房坐标，创建机房区域
        /// </summary>
        [CommandMethod("GetRooms")]
        public static void GetRooms()
        {
            GetRoomsCommand.GetRoomsInfo();
        }

        /// <summary>
        /// 获取初始化信息，只有一个楼层的简单环境，直接用这个
        /// </summary>
        [CommandMethod("GetInitInfo")]
        public static void GetInitInfo()
        {
            GetRoomsCommand.GetInitInfo();
        }

        /// <summary>
        /// 获取机房坐标，创建机房区域
        /// </summary>
        [CommandMethod("GetRoomsEx")]
        public static void GetRoomsEx()
        {
            GetRoomsCommand.GetRoomsInfoEx();
        }

        /// <summary>
        /// 获取整体园区、大楼的关系  //add by qclei 2020-05-04
        /// </summary>
        [CommandMethod("GetParkInfoEx")]
        public static void GetParkInfoEx()
        {
            GetCADOtherCommands.GetParkInfoEx();
        }
               

        /// <summary>
        /// 获取初始化信息，只有一个楼层的简单环境，直接用这个
        /// </summary>
        [CommandMethod("GetInitInfoEx")]
        public static void GetInitInfoEx()
        {
            GetRoomsCommand.GetInitInfoEx();
        }

        /// <summary>
        /// 获取两点之间的文字，测试用与获取房间名称
        /// </summary>
        [CommandMethod("GetText")]
        public static void GetText()
        {
            var p1 = Interaction.GetPoint("坐标1");
            var p2 = Interaction.GetPoint("坐标2");

            string name = "";
            string txt = "";
            ObjectId[] objs=Interaction.GetCrossingSelection(p1, p2);
            CADShapeList sps = new CADShapeList();
            for (int i = 0; i < objs.Length; i++)
            {
                ObjectId item = objs[i];
                var sp = item.ToCADShape(true);
                sps.Add(sp);
                txt += string.Format("{0}\n", sp);
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

            Gui.TextReport("名称:"+ name, txt, 700, 500);
        }

        [CommandMethod("ShowPoints")]
        public static void ShowPoints()
        {
            var p1 = Interaction.GetPoint("坐标1");
            var p2 = Interaction.GetPoint("坐标2");
            Interaction.WriteLine(string.Format("({0} {1})", p1, p2));
            PointInfo pi1 = GetRoomsCommand.GetPointInfo(p1);
            PointInfo pi2 = GetRoomsCommand.GetPointInfo(p2);
            Interaction.WriteLine(string.Format("({0} {1})", pi1, pi2));
        }
    }
}
