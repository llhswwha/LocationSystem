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
            string xml = shape.ToXml();
            Gui.TextReport("Points", xml, 700, 500);
        }

        [CommandMethod("ColumnPoints")]
        public static void GetColumnPoints()
        {
            CADAreaList areaList = new CADAreaList();
            var zero=Interaction.GetPoint("ZeroPoint");
            var columns = Interaction.GetEntitysByLayer("COLUMN");
            var area = columns.ToCADArea(zero);
            area.Name = "集控楼0m层";
            areaList.Add(area);
            var txt = areaList.ToXml();
            Gui.TextReport("Points", txt, 700, 500);
        }
    }
}
