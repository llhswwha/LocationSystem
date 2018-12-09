using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.CADEntitys
{
    [XmlRoot("Area")]
    public class CADArea
    {
        [XmlAttribute]
        public string Name { get; set; }

        public CADPoint Zero { get; set; }

        [XmlAttribute]
        public string ZeroType { get; set; }

        public List<CADShape> Shapes { get; set; }

        public void LineToBlock()
        {
            var lines = Shapes.FindAll(i => i.Name == "Line");
            var temps = new List<CADShape>();
            var temps2 = new List<CADShape>();
            var points = new List<CADPoint>();

            while (lines.Count>0)
            {
                if (temps.Count == 0)
                {
                    var line = lines[0];
                    lines.RemoveAt(0);
                    temps.Add(line);
                    points.AddRange(line.Points);
                }
                else
                {

                }
                

                var lines2 = lines.FindAll(i => i.Points[0].IsSamePoint(points.Last()));
                if (lines2.Count == 0)
                {
                    if (temps.Count > 1)
                    {
                        CreateNewShape(temps, points);
                    }
                    else
                    {
                        temps2.Add(temps[0]);
                        temps.Clear();
                        points.Clear();
                    }
                }
                else if (lines2.Count == 1)
                {
                    
                    var line2 = lines2[0];
                    temps.Add(line2);
                    lines.Remove(line2);

                    if (line2.Points[1].IsSamePoint(points[0]))
                    {
                        CreateNewShape(temps, points);
                    }
                    else
                    {
                        points.Add(line2.Points[1]);
                    }
                }
                else
                {

                }
            }

            foreach (var item in temps2)
            {
                Shapes.Add(item);
            }
            

            //for (int i = 0; i < lines.Count; i++)
            //{
            //    var line = lines[i];
            //    if (points.Count == 0)
            //    {
            //        temps.Add(line);
            //        points.AddRange(line.Points);

            //        lines.RemoveAt(i);//取出一条线
            //        i=0;
            //    }
            //    else
            //    {
            //        var start = line.Points[0];
            //        var end = line.Points[1];
            //        if (start.IsSamePoint(points.Last()))//首尾相连
            //        {
            //            temps.Add(line);

            //            lines.RemoveAt(i);//取出一条线
            //            i = 0;

            //            if (end.IsSamePoint(points[0]))//封闭了
            //            {
            //                CreateNewShape(temps, points);
            //            }
            //            else
            //            {
            //                points.Add(end);
            //            }
            //        }
            //        else
            //        {
            //            if(i== lines.Count - 1)//到最后了
            //            {
            //                CreateNewShape(temps, points);
            //                i = 0;
            //            }
            //        }
            //    }
            //}
        }

        private void CreateNewShape(List<CADShape> temps, List<CADPoint> points)
        {
            foreach (var item in temps)
            {
                Shapes.Remove(item);
            }
            temps.Clear();

            CADShape newShape = new CADShape();
            newShape.Points.AddRange(points);
            newShape.Name = "Block";
            newShape.Type = "NewBlock";
            newShape.Layer = "COLUMN";
            Shapes.Add(newShape);
            points.Clear();
        }

        public CADArea()
        {
            Shapes = new List<CADShape>();
        }

        public void SetZero(CADPoint zero,string key)
        {
            Zero = zero;
            ZeroType = key;
            foreach (var sp in Shapes)
            {
                foreach (var pt in sp.Points)
                {
                    double x = pt.X;
                    double y = pt.Y;
                    if(key.Contains("0"))//左下
                    {
                        pt.X -= zero.X;
                        pt.Y -= zero.Y;
                    }
                    else if (key.Contains("1"))//右下
                    {
                        pt.Y = zero.X - x;
                        pt.X = y - zero.Y;
                    }
                    else if (key.Contains("2"))//右上
                    {
                        pt.X = zero.X - x;
                        pt.Y = zero.Y - y;
                    }
                    else if (key.Contains("3"))//左上
                    {
                        pt.X = zero.Y - y;
                        pt.Y = x - zero.X;
                    }
                    if (pt.X < 0 || pt.Y < 0)
                    {

                    }
                }
            }
        }
    }
}
