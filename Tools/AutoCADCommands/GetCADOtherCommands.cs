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
                Gui.TextReport("GetParkBuild", xml, 700, 500);
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
                    Gui.TextReport("GetAllShapeByLayer", xml, 700, 500);
                }
            }
            catch(Autodesk.AutoCAD.BoundaryRepresentation.Exception ex)
            {
                string exm = ex.Message;
                Gui.TextReport("GetAllShapeByLayer 出错：", exm, 700, 500);
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
                Gui.TextReport("ShapeInfos", xml, 700, 500);
            }
        }


        public static void GetParkInfoEx()
        {
            DateTime start = DateTime.Now;            

            InitInfo initInfo = CreateInitInfo();

            DateTime end = DateTime.Now;
            TimeSpan t = end - start;

            string xml = XmlSerializeHelper.GetXmlText(initInfo);
            Gui.TextReport("获取整个园区",xml, 700, 500);
        }
               
        private static InitInfo CreateInitInfo()
        {
            InitInfo initInfo = new InitInfo();
            TopoInfo root = new TopoInfo("根节点", AreaTypes.区域);
            initInfo.TopoInfo = root;

            try
            {

                //"园区"
                ShapesDefine parkpoint = GetParkLayerByName("园区", false);

                TopoInfo park = new TopoInfo("园区", AreaTypes.园区);//todo:可以扩展成园区、大楼手动设置

                if (parkpoint.shapelist.Count > 0)
                {
                    List<PointInfo> point = new List<PointInfo>();

                    foreach (CADShape vl in parkpoint.shapelist)
                    {
                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);
                            point.Add(sub);
                        }
                        break;
                    }

                    BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();
                    boundInfo.Points.AddRange(point);

                    park.BoundInfo = boundInfo;
                    park.Name = "园区";
                    park.Type = AreaTypes.园区;
                }

                root.AddChild(park);

               // TopoInfo building = new TopoInfo("大楼", AreaTypes.大楼);
              //  park.AddChild(building);

                ShapesDefine floorpoint = GetParkLayerByName("大楼", true);

                if (parkpoint.shapelist.Count > 0)
                {
                    foreach (CADShape vl in floorpoint.shapelist)
                    {
                        List<PointInfo> point = new List<PointInfo>();

                        foreach (CADPoint p in vl.Points)
                        {
                            PointInfo sub = new PointInfo((float)p.X, (float)p.Y);
                            point.Add(sub);
                        }
                        //break;

                        if (point.Count > 0)
                        {
                            BoundInfo boundInfo = GetRoomsCommand.NewBoundInfo();
                            boundInfo.Points.AddRange(point);

                            TopoInfo topoInfo = new TopoInfo();
                            topoInfo.BoundInfo = boundInfo;
                            topoInfo.Name = vl.Name;
                            topoInfo.Type = AreaTypes.大楼;
                            park.AddChild(topoInfo);
                        }
                    }
                }
            }
            catch
            {

            }            

            return initInfo;
        }

    }
}
