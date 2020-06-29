using DbModel.LocationHistory.Data;
using DbModel.Tools.InitInfos;
using Location.BLL.Tool;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.AreaAndDev
{
    public class DynamicMonitorAreaUpdate
    {
        private Dictionary<int, DynamicAreaInfos> AreaInfoDic;
        /// <summary>
        /// 缓存最新的位置信息
        /// </summary>
        /// <param name="areaT"></param>
        /// <param name="posT"></param>
        public void SaveTempInfo(Area areaT,Position posT)
        {
            if (areaT == null) return;
            if (AreaInfoDic == null) AreaInfoDic = new Dictionary<int, DynamicAreaInfos>();
            if (!AreaInfoDic.ContainsKey(areaT.Id))
            {
                DynamicAreaInfos infoT = new DynamicAreaInfos();
                infoT.InitPosInfo(areaT,posT);
                AreaInfoDic.Add(areaT.Id,infoT);
            }
            else
            {
                DynamicAreaInfos infoT = AreaInfoDic[areaT.Id];
                infoT.InitPosInfo(areaT,posT);
            }
        }

        /// <summary>
        /// 获取更新顶点和角度后的区域信息
        /// </summary>
        /// <returns></returns>
        public List<Area>AreaInfoUpdate(Dictionary<string, int> cardRelations)
        {
            if (AreaInfoDic == null||AreaInfoDic.Count==0) return null;
            List<Area> newAreaInfoList = new List<Area>();
            foreach(var dyArea in AreaInfoDic.Values)
            {
                Area areaT = dyArea.UpdateAreaInfo(cardRelations);
                if (areaT == null) continue;
                if(!newAreaInfoList.Contains(areaT))
                {
                    newAreaInfoList.Add(areaT);
                }
            }
            return newAreaInfoList;
        }

    }

    public class DynamicAreaInfos
    {
        private Area areaInfo;

        private List<Position> posList;
        /// <summary>
        /// 保存区域对应位置信息
        /// </summary>
        /// <param name="areaT"></param>
        /// <param name="posT"></param>
        public void InitPosInfo(Area areaT,Position posT)
        {
            areaInfo = areaT;
            if (posList == null) posList = new List<Position>();
            if (!posList.Contains(posT)) posList.Add(posT);
        }
        /// <summary>
        /// 更新区域顶点、角度信息
        /// </summary>
        /// <returns></returns>
        public Area UpdateAreaInfo(Dictionary<string, int> cardRelations)
        {
            if (areaInfo == null) return null;
            SortPosByList(cardRelations);
            return CaculateAreaPoint(); ;
        }
        /// <summary>
        /// 通过标签卡设置的顺序，把Postion排序好
        /// </summary>
        private void SortPosByList(Dictionary<string, int> cardRelations)
        {
            if (cardRelations == null || cardRelations.Count == 0||posList==null) return;
            posList.Sort((a, b) => 
            {
                int aIndex = cardRelations.ContainsKey(a.Code) ? cardRelations[a.Code] : 0;
                int bIndex= cardRelations.ContainsKey(b.Code) ? cardRelations[b.Code] : 0;
                return aIndex.CompareTo(bIndex);
            });
        }


        private int InaccurateNum = 10;//角度误差值
        //private int DirectionX = -1;
        //private int DirectionY = 1;
        //private int DirectionZ = -1;
        private Area CaculateAreaPoint()
        {
            if (areaInfo == null) return null;
            SetPosToDynamic();//把pos设置一个动态区域属性，动态区域不参与告警计算。
            //Todo:通过位置列表，计算出区域的顶点和旋转角度
            if (posList!=null&&posList.Count>=2)
            {
                Position p1 = posList[0];                
                Position p2 = posList[1];
                //左右手坐标系转换，X轴对称
                float angleOfLine = GetUnityAngle(Math.Atan2((p2.Z - p1.Z), (p1.X - p2.X)) * 180 / Math.PI);//角度1
                if(posList.Count>=3)
                {
                    Position p3 = posList[2];
                    float angleOfLine2 = GetUnityAngle(Math.Atan2((p3.Z - p2.Z), (p2.X - p3.X)) * 180 / Math.PI);//角度2
                    //double angleOfLine3 = GetUnityAngle(Math.Atan2((p3.Z - p1.Z), (p1.X - p3.X)) * 180 / Math.PI);//角度3
                    //double angleOfLine5 = GetUnityAngle(Math.Atan2((p1.Z - p2.Z), (p2.X - p1.X)) * 180 / Math.PI);
                    //double angleOfLine6 = GetUnityAngle(Math.Atan2((p2.Z - p3.Z), (p3.X - p2.X)) * 180 / Math.PI);
                    //double angleOfLine7 = GetUnityAngle(Math.Atan2((p1.Z - p3.Z), (p3.X - p1.X)) * 180 / Math.PI);
                    if ((angleOfLine2- angleOfLine) <InaccurateNum)
                    {
                        areaInfo.RY = angleOfLine;
                        areaInfo.Z = p2.Z;
                        TransformM m = areaInfo.GetTransformM();
                        areaInfo.InitBound.SetInitBound(m.ToTModel());
                        return areaInfo;
                    }                  
                }
            }
            return null;           
        }

        /// <summary>
        /// 位置设置成动态电子围栏，该卡不触发告警
        /// </summary>
        private void SetPosToDynamic()
        {
            if (posList == null) return;
            foreach(var item in posList)
            {
                item.IsDynamicAreaPos = true;
            }
        }
        /// <summary>
        /// 获取在Unity中角度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private float GetUnityAngle(double angle)
        {
            if (angle >= -90 && angle <= 180) angle = angle + 90;
            else
            {
                angle = 450 + angle;
            }
            return (float)angle;
        }
    }
}
