using Autodesk.AutoCAD.Geometry;
using DbModel.CADEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADCommands
{
    /// <summary>
    /// 共公功能 add by qclei 2020-04-30
    /// </summary>
    public static class PulicGadget
    {
        /// <summary>
        /// 判断是否是封闭的的图形
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public static bool ifColseShape(CADShape sp)
        {
            bool bret = false;
            CADShape sp1 = new CADShape();

            Dictionary<string, CADPoint> dicPoint = new Dictionary<string, CADPoint>();
            Dictionary<string, CADPoint> dicPointtmp = new Dictionary<string, CADPoint>();

            Dictionary<string, int> dicPointtmp1 = new Dictionary<string, int>();

            //去除相同的点
            foreach (CADPoint vl in sp.Points)
            {
                string tmp = vl.ToString();

                if (dicPoint.ContainsKey(tmp))
                {
                    dicPointtmp[tmp] = vl;
                }
                else
                {
                    dicPoint[tmp] = vl;
                }
            }
            //去除相同的点
            foreach (KeyValuePair<string, CADPoint> vl in dicPointtmp)
            {
                sp.Points.Remove(vl.Value);
            }

            //表示线的数量为偶数，如果是奇数的话，表示非 闭环的环境
            if ((sp.Points.Count & 1) != 1)
            {
                foreach (CADPoint vl in sp.Points)
                {

                    string xpos = "x_" +  Math.Round(vl.X).ToString();
                    string ypos = "y_" + Math.Round(vl.Y).ToString();

                    int ncount = 1;
                    if (dicPointtmp1.ContainsKey(xpos))
                    {
                        ncount = dicPointtmp1[xpos];
                        ncount++;
                    }

                    dicPointtmp1[xpos] = ncount;

                    ncount = 1;
                    if (dicPointtmp1.ContainsKey(ypos))
                    {
                        ncount = dicPointtmp1[ypos];
                        ncount++;
                    }

                    dicPointtmp1[ypos] = ncount;
                }

                bret = true;
                foreach (KeyValuePair<string, int> vl in dicPointtmp1)
                {
                    if (vl.Value != 2)
                    {
                        bret = false;
                        break;
                    }
                }
            }

            return bret;
        }


        /// <summary>
        /// 根据图形获取该图形内的名称
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string getShapeName(CADShape sp)
        {
            string name = "";

            double pMinX = double.MaxValue;
            double pMinY = double.MaxValue;
            double pMaxX = double.MinValue;
            double pMaxY = double.MinValue;
            //查找 多边型 最小的点，和最大点的坐标
            foreach (CADPoint p1 in sp.Points)
            {
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

            Point3d pMin = new Point3d(pMinX, pMinY, 0);
            Point3d pMax = new Point3d(pMaxX, pMaxY, 0);

            if(sp.Points.Count == 2)
            {
                pMin = new Point3d(sp.Points[0].X, sp.Points[0].Y, 0);
                pMax = new Point3d(sp.Points[1].X, sp.Points[1].Y, 0);
            }
            name = GetRoomsCommand.GetText(pMin, pMax,sp.Layer);
            return name;
        }
    }
}
