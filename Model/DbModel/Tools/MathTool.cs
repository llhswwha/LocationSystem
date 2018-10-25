using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2=DbModel.Location.AreaAndDev.Point;
namespace DbModel.Tools
{
    public static class MathTool
    {
        //判断点在线的一边
        public static int IsLeft(Vector2 P0, Vector2 P1, Vector2 P2)
        {
            int abc = (int)((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
            return abc;

        }

        //判断点pnt是否在region内主程序
        public static bool IsInRegion(Vector2 pnt, List<Vector2> region)
        {

            int wn = 0, j = 0; //wn 计数器 j第二个点指针
            for (int i = 0; i < region.Count; i++)
            {
                //开始循环
                if (i == region.Count - 1)
                {
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点
                }
                else
                {
                    j = j + 1; //如果不是 ，则找下一点
                }

                if (region[i].Y <= pnt.Y) // 如果多边形的点 小于等于 选定点的 Y 坐标
                {
                    if (region[j].Y > pnt.Y) // 如果多边形的下一点 大于于 选定点的 Y 坐标
                    {
                        if (IsLeft(region[i], region[j], pnt) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (region[j].Y <= pnt.Y)
                    {
                        if (IsLeft(region[i], region[j], pnt) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
