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
        [CommandMethod("TextReport")]
        public static void TextReport()
        {
            Gui.TextReport("title", "content", 200, 300, false);
        }

        [CommandMethod("ExtentPoints")]
        public static void GetExtentPoints()
        {
            var objId=Interaction.GetEntity("Entity");
            CADShape shape = objId.ToCADShape();
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

        [CommandMethod("ShowLayers")]
        public static void ShowLayers()
        {

        }


        [CommandMethod("ClearLayers")]
        public static void ClearLayers()
        {

        }

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
