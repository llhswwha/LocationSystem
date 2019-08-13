using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PosInfoList : IComparable<PosInfoList>
    {
        public string Name { get; set; }

        public int Count
        {
            get
            {
                if (Items == null) return -1;
                return Items.Count;
            }
        }

        public List<PosInfo> Items { get; set; }

        public PosInfoList(string name)
        {
            this.Name = name;
        }

        public void Add(PosInfo pos)
        {
            if (Items == null)
            {
                Items = new List<PosInfo>();
            }

            Items.Add(pos);
        }

        public static List<PosInfoList> GetList(List<PosInfo> posList, Func<PosInfo, string> actionGetName)
        {
            Dictionary<string, PosInfoList> dateDict = new Dictionary<string, PosInfoList>();
            for (int i = 0; i < posList.Count; i++)
            {
                var pos = posList[i];

                var name = actionGetName(pos);
                if (name == null)
                {
                    name = "NULL";
                }
                if (!dateDict.ContainsKey(name))
                {
                    dateDict.Add(name, new PosInfoList(name));
                }
                var list = dateDict[name];
                list.Add(pos);
            }
            var result = dateDict.Values.ToList();
            result.Sort();
            return result;
        }

        public static List<PosInfoList> GetListByDay(List<PosInfo> posList)
        {
            return GetList(posList, i => i.DateTime.ToString("yyyy-MM-dd"));
        }

        public static List<PosInfoList> GetListByHour(List<PosInfo> posList)
        {
            return GetList(posList, i => i.DateTime.ToString("yyyy-MM-dd HH"));
        }

        public static List<PosInfoList> GetListByCode(List<PosInfo> posList)
        {
            return GetList(posList, i => i.Code);
        }

        public static List<PosInfoList> GetListByPerson(List<PosInfo> posList)
        {
            return GetList(posList, i => i.PersonnelName);
        }

        public static List<PosInfoList> GetListByArea(List<PosInfo> posList)
        {
            return GetList(posList, i => i.AreaPath);
        }

        public int CompareTo(PosInfoList other)
        {
            return other.Count.CompareTo(this.Count);
        }
    }
}
