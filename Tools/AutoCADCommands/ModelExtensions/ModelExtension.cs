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
        public static CADPoint ToCADPoint(this Point3d pt, bool isToUCS)
        {
            if (isToUCS)
            {
                pt = pt.ToUCS();
            }
            CADPoint point = new CADPoint();
            point.X = pt.X;
            point.Y = pt.Y;
            return point;
        }

        public static Point3d ToPoint3d(this CADPoint pt)
        {
            Point3d point = new Point3d(pt.X, pt.Y,0);
            return point;
        }

        public static CADPoint AddPoint(this CADShape sp, Point3d pt, bool isToUCS)
        {
            var point = pt.ToCADPoint(isToUCS);
            sp.Points.Add(point);
            return point;
        }

        //public static CADShape AddPoints(this CADShape sp, BlockReference block)
        //{
        //    sp.AddPoints(block.GeometricExtents);
        //    sp.Entity = block;
        //    return sp;
        //}

        public static CADShape AddPoints(this CADShape sp, Entity entity, bool isToUCS)
        {
            sp.AddPoints(entity.GeometricExtents, isToUCS);
            sp.Entity = entity;
            return sp;
        }

        public static CADShape AddPoints(this CADShape sp, Extents3d extents, bool isToUCS)
        {
            var p11 = extents.MinPoint;
            var p22 = extents.MaxPoint;
            var p21 = new Point3d(p22.X, p11.Y, p11.Z);
            var p12 = new Point3d(p11.X, p22.Y, p22.Z);
            sp.AddPoint(p11, isToUCS);
            sp.AddPoint(p21, isToUCS);
            sp.AddPoint(p22, isToUCS);
            sp.AddPoint(p12, isToUCS);
            return sp;
        }

        public static CADShape AddPoints(this CADShape sp, Line line, bool isToUCS)
        {
            sp.AddPoint(line.StartPoint, isToUCS);
            sp.AddPoint(line.EndPoint, isToUCS);
            sp.Entity = line;
            return sp;
        }

        public static CADShape AddPoints(this CADShape sp, Circle circle, bool isToUCS)
        {
            sp.AddPoint(circle.Center, isToUCS);
            sp.Entity = circle;
            return sp;
        }

        public static CADShape AddPoints(this CADShape sp, MText mText, bool isToUCS)
        {
            var center = mText.GetCenter();
            sp.AddPoint(mText.Location, isToUCS);
            sp.Entity = mText;
            return sp;
        }

        public static CADShape AddPoints(this CADShape sp, Polyline line, bool isToUCS)
        {
            var points = line.GetPoints();
            foreach (var point in points)
            {
                sp.AddPoint(point, isToUCS);
            }
            sp.Entity = line;
            return sp;
        }

        public static CADShape ToCADShape(this ObjectId objId,bool isToUCS)
        {
            CADShape sp = new CADShape();
            //var dbObject = objId.QOpenForRead();
            var entity = objId.QOpenForRead<Entity>();
            sp.Name = entity.GetType().Name;
            sp.Type = entity.GetType().Name;
            sp.Layer = entity.Layer;
            if (entity is BlockReference)
            {
                sp.Name = "Block";
                sp.AddPoints(entity as BlockReference, isToUCS);
            }
            else if (entity is Line)
            {
                sp.AddPoints(entity as Line, isToUCS);
            }
            else if (entity is Polyline)
            {
                sp.AddPoints(entity as Polyline, isToUCS);
            }
            else if (entity is MLeader)
            {
                MLeader mleader= entity as  MLeader;
                if(mleader.MText!=null)
                    sp.Text = mleader.MText.Text;
                //sp.AddPoints(entity as MLeader);
            }
            else if (entity is Circle)
            {
                Circle circle = entity as Circle;
                //if (mleader.MText != null)
                //    sp.Text = mleader.MText.Text;
                sp.AddPoints(circle, isToUCS);
            }
            else if (entity is MText)
            {
                MText text = entity as MText;
                sp.Text = text.Text;
                //text.InsertionPoint(0);
                sp.AddPoints(text, isToUCS);
            }
            else if (entity is DBText)
            {
                DBText text = entity as DBText;
                sp.Text = text.TextString;
                //text.InsertionPoint(0);
                sp.AddPoints(text, isToUCS);
            }

            else if (entity is Hatch)
            {
                Hatch hatch = entity as Hatch;
                //sp.Text = text.Text;
                //text.InsertionPoint(0);
                sp.AddPoints(hatch, isToUCS);
            }
            else
            {
                //sp = null;
                sp.AddPoints(entity, isToUCS);
            }

            return sp;
        }

        public static CADArea ToCADArea(this ObjectId[] objIds, Point3d zero,string zeroType)
        {
            CADArea sps = new CADArea();
            
            foreach (var item in objIds)
            {
                var sp = item.ToCADShape(false);
                if (sp != null)
                {
                    sps.Shapes.Add(sp);
                    sp.Num = sps.Shapes.Count;
                }
            }
            sps.SetZero(zero.ToCADPoint(false), zeroType);
            return sps;
        }

        public static string ToXml(this object obj)
        {
            return XmlSerializeHelper.GetXmlText(obj);
        }
    }
}
