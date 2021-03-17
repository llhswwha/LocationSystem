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
using Autodesk.AutoCAD.Colors;
using System.Threading;
using DbModel.Tools;

namespace AutoCADCommands
{
    /// <summary>
    /// 由 钱 编写代码。区分蔡的代码
    /// </summary>
    public static class GetCADOtherCommands
    {
        /// <summary>
        /// 根据“大楼”名称获取相应的图层
        /// </summary>
        public static void GetParkBuild(string buildName)
        {
            ShapesDefine shape = new ShapesDefine();
            shape  = GetParkLayerByName(buildName,false);

            if (shape.shapelist.Count() > 0)
            {
                string xml = shape.toXml();
                MyTool.TextReport("GetParkBuild", xml, 700, 500);
            }
        }

        private static ShapesDefine GetParkLayerByName(string name, bool getNewName)
        {
            ShapesDefine shape = new ShapesDefine();
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Interaction.ActiveEditor;// Application.DocumentManager.MdiActiveDocument.Editor;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead, false);

                foreach(ObjectId lid in lt)
                {
                    LayerTableRecord lt1 = (LayerTableRecord)trans.GetObject(lid, OpenMode.ForRead, false);

                    string lName = lt1.Name;

                    if(lName.Contains(name) )
                    {
                        TypedValue[] pTypeValue = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, lName) };

                        SelectionFilter pSelectFilter = new SelectionFilter(pTypeValue);
                        PromptSelectionResult pSelectResult = ed.SelectAll(pSelectFilter);
                        SelectionSet pSelectSet = pSelectResult.Value;

                        if (pSelectSet != null)
                        {
                            ObjectId[] objs = pSelectSet.GetObjectIds();

                            foreach (ObjectId objId in pSelectSet.GetObjectIds())
                            {
                                //某个元素
                                //Entity ent = (Entity)trans.GetObject(objId, OpenMode.ForRead);
                                var ent = objId.ToCADShape(true);
                                //判断是否为多边型元素
                                string pl = ent.Type.ToString();
                                //判断是否为多边型元素
                                if (pl.Contains("Polyline"))// && ent.Points.Count() == 22)
                                {
                                    ent.Name = lName;
                                    if (getNewName)
                                    {
                                        string name1 = PulicGadget.getShapeName(ent);
                                        if (name1 != "") ent.Name = name1;
                                    }


                                    {
                                        //ent.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByBlock, 3); ;

                                        //获取图层的名称
                                        shape.addShape(ent);
                                    }
                                }
                            }
                        }
                       
                    }
                }

                trans.Commit();
            }

            return shape;
        }

        public static ObjectId[] GetAllShape()
        {
            ObjectId[] objs = null;

            string name = "";
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Interaction.ActiveEditor;// Application.DocumentManager.MdiActiveDocument.Editor;
  
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取当前图层
                var layerTableRecord = (LayerTableRecord)trans.GetObject(db.Clayer, OpenMode.ForRead, false);

                //获取图层的名称
                name = layerTableRecord.Name;

                //以下是获取当前图层上所有元素
                TypedValue[] pTypeValue = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, name) };

                SelectionFilter pSelectFilter = new SelectionFilter(pTypeValue);
                PromptSelectionResult pSelectResult = ed.SelectAll(pSelectFilter);
                SelectionSet pSelectSet = pSelectResult.Value;

                objs = pSelectSet.GetObjectIds();

                trans.Commit();
            }

            db.CloseInput(true);
            return objs;

        }

        /// <summary>
        /// 获取某图层下所有多边型元素
        /// </summary>
        public static void GetAllShapeByLayer()
        {
            try
            {
                ShapesDefine shape = new ShapesDefine();
                ObjectId[] objs = GetAllShape();

                if (objs == null) return;

                foreach (ObjectId objId in objs)
                {
                    //某个元素
                    //Entity ent = (Entity)trans.GetObject(objId, OpenMode.ForWrite);
                    var ent = objId.ToCADShape(true);

                    string pl = ent.Type.ToString();
                    //判断是否为多边型元素
                    if (pl.Contains("Polyline"))// && ent.Points.Count() == 22)
                    {
                        //CADShape sp = new CADShape();
                        // sp.AddPoints(ent as Polyline, true);

                        //add by qclei 2020-04-29 判断是否是封闭的图形
                        //bool bl = PulicGadget.ifColseShape(sp);
                        //if (bl)
                        {
                            //ent.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByBlock, 3); ;

                            //获取图层的名称
                            string name1 = PulicGadget.getShapeName(ent);
                            if (name1 != "") ent.Name = name1;
                            shape.addShape(ent);

                        }

                    }

                }

                if (shape.shapelist.Count() > 0)
                {
                    string xml = shape.toXml();
                    MyTool.TextReport("GetAllShapeByLayer", xml, 700, 500);
                }
            }
            catch(Autodesk.AutoCAD.BoundaryRepresentation.Exception ex)
            {
                string exm = ex.Message;
                MyTool.TextReport("GetAllShapeByLayer 出错：", exm, 700, 500);
            }

            return;
        }

        /// <summary>
        /// 手动获取多个“多边型”图形
        /// </summary>
        public static void GetAllShapeByManual()
        {
            ShapesDefine shape = new ShapesDefine();

            int iCount = 1;
            while (true)
            {
                string sel = string.Format("Select {0} :", iCount.ToString());
                var id = Interaction.GetEntity(sel);

                if (id.Handle.Value != 0)
                {
                    var sp = id.ToCADShape(true);
                    
                    //add by qclei 2020-04-29 判断是否是封闭的图形
                    //bool bl = PulicGadget.ifColseShape(sp);
                    //if (bl)
                    {
                        string name = PulicGadget.getShapeName(sp);
                        if (name != "") sp.Name = name;
                        shape.addShape(sp);
                        iCount++;
                    }
                }
                else
                {
                    break;
                }
            }

            if (shape.shapelist.Count() > 0)
            {
                string xml = shape.toXml();
                MyTool.TextReport("ShapeInfos", xml, 700, 500);
            }
        }


        /// <summary>
        /// Thickness:厚度，层高
        /// IsRelative="true"
        /// BottomHeight:离地面高度，0层为0，6.5层为：6500
        /// </summary>
        /// <param name="name"></param>

        //获取楼层内的房间 //add by qclei 2020-07-10
        public static void GetAllFloors(string buildingName,int buildingFloorCount,string parkName)
        {
            DateTime start = DateTime.Now;
            List<TopoInfo> floors = GetFloorList(buildingName, buildingFloorCount);
            string xml = "";
            if (!string.IsNullOrEmpty(parkName))
            {
                InitInfo initInfo = GetRoomsCommand.CreateParkInitInfoByFloors(parkName, buildingName,floors.ToArray());
                xml = XmlSerializeHelper.GetXmlText(initInfo);
                MyTool.TextReport("获取整个大楼和园区", xml, 700, 500);
            }
            else
            {
                TopoInfo initInfo = GetRoomsCommand.CreateBuildingInitInfoByFloors(buildingName, floors.ToArray());
                xml = XmlSerializeHelper.GetXmlText(initInfo, true);
                MyTool.TextReport("获取整个大楼", xml, 700, 500);
            }


            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
        }

        private static List<TopoInfo> GetFloorList(string buildingName, int buildingFloorCount)
        {
            List<TopoInfo> floors = new List<TopoInfo>();
            for (int i = 0; i < buildingFloorCount; i++)
            {
                string floorLayer = string.Format("{0}{1}楼", buildingName, (i + 1));
                TopoInfo floor = GetOneFloorOpt(floorLayer);//大楼X
                if (floor.GetCount() > 0)
                {
                    floors.Add(floor);
                }
            }

            return floors;
        }

        public static TopoInfo GetOneFloorOpt(string floorLayer)
        {
            //InitInfo initInfo = new InitInfo();

            //TopoInfo root = new TopoInfo("根节点", AreaTypes.区域);
            //initInfo.TopoInfo = root;
            string floorBoundLayer = floorLayer + "-边界";
            TopoInfo floor = new TopoInfo(floorLayer, AreaTypes.楼层);//todo:可以扩展成园区、大楼手动设置
            try
            {
                //"园区"
                ShapesDefine parkpoint = GetParkLayerByName(floorBoundLayer, false);
                if (parkpoint.shapelist.Count > 0)
                {
                    List<PointInfo> point = new List<PointInfo>();

                    foreach (CADShape vl in parkpoint.shapelist)
                    {
                        Dictionary<string, int> repoint = new Dictionary<string, int>(); //去除重复的点

                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);

                            string key = p.X.ToString() + "_" + p.Y.ToString();

                            if (!repoint.ContainsKey(key))
                            {
                                repoint[key] = 1;
                                point.Add(sub);
                            }
                        }
                        break;
                    }
                    BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();
                    //boundInfo.Points.AddRange(point);
                    List<PointInfo> newpoint = new List<PointInfo>();
                    ToPointFromClockwise(point, ref newpoint);
                    boundInfo.Points.AddRange(newpoint);
                    boundInfo.IsRelative = true;
                    floor.BoundInfo = boundInfo;
                    floor.Name = floorLayer;
                    floor.Type = AreaTypes.楼层;
                }
                //root.AddChild(floor);

                // TopoInfo building = new TopoInfo("大楼", AreaTypes.大楼);
                //  park.AddChild(building);

                ShapesDefine floorpoint = GetParkLayerByName(floorLayer, true);//房间都在楼层的Layer里面

                if (parkpoint.shapelist.Count > 0)
                {
                    foreach (CADShape vl in floorpoint.shapelist)
                    {
                        if(vl.Layer == floorBoundLayer)
                        {
                            //
                            continue;
                        }

                        List<PointInfo> point = new List<PointInfo>();
                        Dictionary<string, int> repoint = new Dictionary<string, int>(); //去除重复的点

                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);
                            string key = p.X.ToString() + "_" + p.Y.ToString();

                            if (!repoint.ContainsKey(key))
                            {
                                repoint[key] = 1;
                                point.Add(sub);
                            }
                        }
                        //break;

                        if (point.Count > 0)
                        {
                            BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();
                          //  boundInfo.Points.AddRange(point);
                            boundInfo.IsRelative = false;//这里是true的话，楼层有偏移会导致房间整体偏移。

                            List<PointInfo> newpoint = new List<PointInfo>();
                            ToPointFromClockwise(point, ref newpoint);
                            boundInfo.Points.AddRange(newpoint);

                            TopoInfo topoInfo = new TopoInfo();
                            topoInfo.BoundInfo = boundInfo;
                            topoInfo.Name = vl.Name;
                            if(vl.Name== "floorLayer")
                            {
                                topoInfo.Name = "机房_NoName";
                            }
                            topoInfo.Type = AreaTypes.机房;
                            

                            floor.AddChild(topoInfo);
                        }
                    }
                }
            }
            catch
            {

            }

            return floor;
        }

        public static void GetOneFloorWithRoot(string floorLayer, string parkName)
        {
            DateTime start = DateTime.Now;

            TopoInfo floor = GetOneFloorOpt(floorLayer);
            string xml = "";
            if (!string.IsNullOrEmpty(parkName))
            {
                InitInfo initInfo = GetRoomsCommand.CreateParkInitInfoByFloors(parkName, floorLayer,floor);
                xml = XmlSerializeHelper.GetXmlText(initInfo);
                MyTool.TextReport("获取一个楼层和园区", xml, 700, 500);
            }
            else
            {
                xml = XmlSerializeHelper.GetXmlText(floor, true);
                MyTool.TextReport("获取一个楼层", xml, 700, 500);
            }

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
        }


        public static void GetParkInfoEx(string parkName, int maxFloorCount)
        {
            DateTime start = DateTime.Now;            

            InitInfo initInfo = CreateInitInfo(parkName,maxFloorCount);

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;

            string xml = XmlSerializeHelper.GetXmlText(initInfo);
            MyTool.TextReport("获取整个园区",xml, 700, 500);
        }
               
        private static InitInfo CreateInitInfo(string parkName,int maxFloorCount)
        {
            InitInfo initInfo = new InitInfo();
            TopoInfo root = new TopoInfo("根节点", AreaTypes.区域);
            initInfo.TopoInfo = root;

            try
            {

                //"园区"
                ShapesDefine parkpoint = GetParkLayerByName("园区", false);

                TopoInfo park = new TopoInfo(parkName, AreaTypes.园区);//todo:可以扩展成园区、大楼手动设置

                if (parkpoint.shapelist.Count > 0)
                {
                    List<PointInfo> point = new List<PointInfo>();

                    foreach (CADShape vl in parkpoint.shapelist)
                    {
                        Dictionary<string, int> repoint = new Dictionary<string, int>(); //去除重复的点

                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);

                            string key = p.X.ToString() + "_" + p.Y.ToString();

                            if (!repoint.ContainsKey(key))
                            {
                                repoint[key] = 1;
                                point.Add(sub);
                            }
                        }
                        break;
                    }

                    BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();

                    List<PointInfo> newpoint = new List<PointInfo>();
                    ToPointFromClockwise(point, ref newpoint);
                    boundInfo.Points.AddRange(newpoint);
                    //boundInfo.Points.AddRange(point);

                    park.BoundInfo = boundInfo;
                    park.Name = parkName;
                    park.Type = AreaTypes.园区;
                }

                root.AddChild(park);

               // TopoInfo building = new TopoInfo("大楼", AreaTypes.大楼);
              //  park.AddChild(building);

                ShapesDefine floorpoint = GetParkLayerByName("大楼", true);

                TopoInfo group = new TopoInfo();
                group.Type = AreaTypes.分组;
                group.Name = "生产区域";

                park.AddChild(group);

                if (parkpoint.shapelist.Count > 0)
                {
                    foreach (CADShape vl in floorpoint.shapelist)
                    {
                        List<PointInfo> point = new List<PointInfo>();
                        Dictionary<string, int> repoint = new Dictionary<string, int>(); //去除重复的点

                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);
                            string key = p.X.ToString() + "_" + p.Y.ToString();

                            if (!repoint.ContainsKey(key))
                            {
                                repoint[key] = 1;
                                point.Add(sub);
                            }
                        }
                        //break;

                        if (point.Count > 0)
                        {
                            List<PointInfo> newpoint = new List<PointInfo>();
                            ToPointFromClockwise(point, ref newpoint);
                            

                            BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();
                            //boundInfo.Points.AddRange(point);
                            boundInfo.Points.AddRange(newpoint);

                            TopoInfo topoInfo = new TopoInfo();
                            topoInfo.BoundInfo = boundInfo;
                            topoInfo.Name = vl.Name;
                            topoInfo.Type = AreaTypes.大楼;
                            group.AddChild(topoInfo);

                            List<TopoInfo> floors = GetFloorList(vl.Name, maxFloorCount);//cww，合并 全厂_大楼_楼层_房间
                            foreach (var floor in floors)
                            {
                                topoInfo.AddChild(floor);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            root.SetAbsolute();
            return initInfo;
        }

        /// <summary>
        /// 将节点将 顺时针输出
        /// </summary>
        /// <returns></returns>
        private static void  ToPointFromClockwise(List<PointInfo> oldPoint, ref List<PointInfo> newPoint)
        {
            newPoint = new List<PointInfo>();

            List<PointInfo> oldPointtmp = new List<PointInfo>();
            List<PointInfo> retwisetmp = new List<PointInfo>();
            int count = oldPoint.Count;
            bool bl = false;

            int i = 1;
            for (i = 0; i < count - 1; i++)
            {
                if (oldPoint[i + 1].X == oldPoint[i].X)
                {
                    retwisetmp.Add(oldPoint[i]);
                    bl = true;
                }
                else
                {
                    break;
                }
            }

            bool ifc = false;

            if (bl)
            {
                //将X轴上的几个相同的X点，移到 List的最后面
                int j = i;
                for (j = i; j < count; j++)
                {
                    oldPointtmp.Add(oldPoint[j]);
                }

                for (i = retwisetmp.Count - 1; i >= 0; i--)
                {
                    oldPointtmp.Add(retwisetmp[i]);
                }

                ifc = solvelist(oldPointtmp);
                if(!ifc)
                {
                    toClockwise(oldPointtmp, ref newPoint);
                }
                else
                {
                    newPoint = oldPointtmp;
                }
                
                //return true;
            }
            else
            {
                ifc = solvelist(oldPoint);

                if (!ifc)
                {
                    toClockwise(oldPoint, ref newPoint);
                }
                else
                {
                    newPoint = oldPoint;
                }
            }
            
            return ;
        }

        /// <summary>
        /// 判断是否是顺时针，返回true时，为顺时针
        /// </summary>
        /// <param name="wise"></param>
        /// <returns></returns>
        private static bool solvelist(List<PointInfo> wise)
        {
            int count = wise.Count;

            double ans = 0.0;
            //这里有一个约定，当下标大于n的时候X(n+1)=X1,Y(n+1)=Y1
            for (int i = 0; i < count - 1; i++)
            {
                //double tmp = (wise[i + 1].Y + wise[i].Y) * (wise[i + 1].X - wise[i].X);

                ans += -0.5 * (wise[i + 1].Y + wise[i].Y) * (wise[i + 1].X - wise[i].X);//由于推导公式得到需要加一个负号
            }
            if (ans < 0) return true;//小于0就是顺时针，这个可以通过维基百科加以理解
            return false;
        }

        /// <summary>
        /// 将逆时针转成顺时针
        /// </summary>
        /// <param name="wise"></param>
        /// <returns></returns>
        private static void toClockwise(List<PointInfo> oldPoint, ref List<PointInfo> newPoint)
        {
            for (int i = oldPoint.Count - 1; i >= 0; i--)
            {
                newPoint.Add(oldPoint[i]);
            }
        }
    }
}
