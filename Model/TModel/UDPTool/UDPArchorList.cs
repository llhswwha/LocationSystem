using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace ArchorUDPTool.Models
{
    [XmlRoot("UDPArchorList")]
    public class UDPArchorList : List<UDPArchor>
    {
        public ServerInfoList ServerList = new ServerInfoList();

        public new void Add(UDPArchor item)
        {
            base.Add(item);
            ServerList.Add(item.ServerIp, item.ServerPort);
        }

        //ArchorStatistics statistics = new ArchorStatistics();

        //Dictionary<string, UDPArchor> index = new Dictionary<string, UDPArchor>();
        //internal int AddOrUpdate(UDPArchor archor)
        //{
        //    if (archor == null) return -1;
        //    lock (index)
        //    {
        //        if (index.ContainsKey(archor.Client))
        //        {
        //            if (DataUpdated != null)
        //            {
        //                DataUpdated(archor);
        //            }
        //            return 0;
        //        }
        //        else
        //        {
        //            index[archor.Client] = archor;
        //            this.Add(archor);
        //            statistics.Add(archor.Client);
        //            if (DataAdded != null)
        //            {
        //                DataAdded(archor);
        //            }
        //            return 1;
        //        }
        //    }

        //}

        //public string GetStatistics()
        //{
        //    return statistics.GetText();
        //}

        //public event Action<UDPArchor> DataUpdated;

        //public event Action<UDPArchor> DataAdded;
        public object GetConnectedCount()
        {
            int count = 0;
            foreach (var i in this)
            {
                if (!string.IsNullOrEmpty(i.IsConnected))
                {
                    count++;
                }
            }
            return count;
        }

        public List<string> GetAreas()
        {
            List<string> areas = new List<string>();
            foreach (var item in this)
            {
                //if (item.RealArea == null) continue;
                var areaName = item.RealArea+"";
                areaName = areaName.Replace("）", "） ");
                string[] parts = areaName.Split(' ');
                if (parts.Length >= 3)
                {
                    //int id = areaName.LastIndexOf(' ');
                    //if (id != -1)
                    //    areaName = areaName.Substring(0, id);
                    areaName = parts[0] + " " + parts[1];
                }

                if (!areas.Contains(areaName))
                {
                    areas.Add(areaName);
                }
            }
            areas.Sort();
            return areas;
        }

        public class AreaCount : IComparable<AreaCount>
        {
            public string Area;
            public int Count1;
            public int Count2;

            public double percent;
            public string flat;

            public AreaCount(string a, int c1, int c2)
            {
                Area = a;
                Count1 = c1;
                Count2 = c2;
                percent = (Count1 + 0.0) / Count2 * 100.0;

                flat = "";
                if (Count1 == Count2)
                {
                    flat = "=";
                }
                else
                {
                    flat = "!!";
                }
            }

            public override string ToString()
            {

                string txt = string.Format("{0}\t{1}/{2}\t{3:F1}\t{4}", flat, Count1, Count2, percent, Area);

                return txt;
            }

            public int CompareTo(AreaCount other)
            {
                return this.percent.CompareTo(other.percent);
            }
        }

        public string GetCountByArea()
        {
            List<AreaCount> areasCount = GetAreaCount();
            string txt = "";

            //
            int c1 = 0;
            int c2 = 0;
            foreach (var item in areasCount)
            {
                txt += item.ToString() + "\n";
                c1 += item.Count1;
                c2 += item.Count2;
            }
            AreaCount all = new AreaCount("全部",c1,c2);
            txt += all.ToString();
            return txt;
        }

        public void ClearInfo()
        {
            foreach (var item in this)
            {
                item.RealArea = "";
            }
        }

        private List<AreaCount> GetAreaCount()
        {
            List<string> areas = GetAreas();
            List<AreaCount> areasCount = new List<AreaCount>();
            foreach (var area in areas)
            {

                int count1 = 0;
                int count2 = 0;
                foreach (var archor in this)
                {
                    if (area == "")
                    {
                        if (archor.RealArea == area)
                        {
                            if (!string.IsNullOrEmpty(archor.IsConnected))
                            {
                                count1++;
                            }
                            count2++;
                        }
                    }
                    else
                    {
                        if (archor.RealArea==area)
                        {
                            if (!string.IsNullOrEmpty(archor.IsConnected))
                            {
                                count1++;
                            }
                            count2++;
                        }
                    }
                }
                AreaCount count = new AreaCount(area, count1, count2);
                areasCount.Add(count);
            }

            areasCount.Sort();
            return areasCount;
        }

        public DataTable GetCountByAreaTable()
        {
            List<AreaCount> areasCount = GetAreaCount();
            DataTable dt = new DataTable();
            dt.Columns.Add("是否完成");
            dt.Columns.Add("百分比");
            dt.Columns.Add("数量");
            dt.Columns.Add("区域");
            foreach (var item in areasCount)
            {
                dt.Rows.Add(item.flat, item.percent.ToString("F1"), string.Format("{0}/{1}", item.Count1, item.Count2), item.Area);
            }
            return dt;
        }
    }
}
