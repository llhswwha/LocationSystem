using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using DbModel.CADEntitys;
using DbModel.Tools;
using Dreambuild.AutoCAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADCommands.ModelExtensions
{
    public static class ModelExtension
    {
        public static CADPoint ToCADPoint(this Point3d pt)
        {
            CADPoint point = new CADPoint();
            point.X = pt.X;
            point.Y = pt.Y;
            return point;
        }

        public static CADPoint AddPoint(this CADShape sp, Point3d pt)
        {
            var point = pt.ToCADPoint();
            sp.Points.Add(point);
            return point;
        }

        public static CADShape AddExtents(this CADShape sp, Extents3d extents)
        {
            var p11 = extents.MinPoint;
            var p22 = extents.MaxPoint;
            var p21 = new Point3d(p22.X, p11.Y, p11.Z);
            var p12 = new Point3d(p11.X, p22.Y, p22.Z);
            sp.AddPoint(p11);
            sp.AddPoint(p21);
            sp.AddPoint(p22);
            sp.AddPoint(p12);
            return sp;
        }

        public static CADShape AddExtents(this CADShape sp, Line line)
        {
            sp.AddPoint(line.StartPoint);
            sp.AddPoint(line.EndPoint);
            return sp;
        }

        public static CADShape ToCADShape(this ObjectId objId)
        {
            CADShape sp = new CADShape();
            var dbObject = objId.QOpenForRead();
            var entity = objId.QOpenForRead<Entity>();
            sp.Name = entity.GetType().Name;
            sp.Type = entity.GetType().Name;
            sp.Layer = entity.Layer;
            if (entity is BlockReference)
            {
                sp.Name = "Block";
                sp.AddExtents(entity.GeometricExtents);
            }
            else if (entity is Line)
            {
                sp.AddExtents(entity as Line);
            }
            else
            {

            }

            return sp;
        }

        public static CADArea ToCADArea(this ObjectId[] objIds, Point3d zero)
        {
            CADArea sps = new CADArea();
            
            foreach (var item in objIds)
            {
                var sp = item.ToCADShape();
                sps.Shapes.Add(sp);
                sp.Num = sps.Shapes.Count;
            }
            sps.SetZero(zero.ToCADPoint());
            return sps;
        }

        public static string ToXml(this object obj)
        {
            return XmlSerializeHelper.GetXmlText(obj);
        }
    }
}
