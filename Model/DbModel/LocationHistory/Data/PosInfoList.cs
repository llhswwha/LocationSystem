using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PosInfoList : IComparable<PosInfoList>
    {
        private List<PosInfo> _items;

        public string Name { get; set; }

        public int Count { get; set; }

        public List<PosInfo> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                if (_items != null)
                {
                    Count = _items.Count;
                }
                else
                {
                    Count = -1;
                }
            }
        }

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
            Count++;
        }


        public int CompareTo(PosInfoList other)
        {
            return other.Count.CompareTo(this.Count);
        }
    }

    public static class PosInfoListHelper
    {
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
            return GetList(posList, i => i.GetDateTime_Date());
        }

        public static List<PosInfoList> GetListByHour(List<PosInfo> posList)
        {
            return GetList(posList, i => i.GetDateTime_Hour());
        }

        public static List<PosInfoList> GetListByCode(List<PosInfo> posList)
        {
            return GetList(posList, i => i.Code);
        }

        public static List<PosInfoList> GetListByPerson(List<PosInfo> posList)
        {
            return GetList(posList, i => i.GetPersonnelName());
        }

        public static List<PosInfoList> GetListByArea(List<PosInfo> posList)
        {
            return GetList(posList, i => i.GetAreaPath());
        }

        public static List<PosInfoList> GetSubList(List<PosInfo> list,int flag)
        {
            List<PosInfoList> Send = null;
            switch (flag)
            {
                case 1:
                    Send = GetListByDay(list);//按天
                    if(Send!=null)
                    {
                        Send.Sort((a, b) =>
                        {
                            return b.Name.CompareTo(a.Name);
                        });
                    }                   
                    break;
                case 2:
                    Send = GetListByPerson(list);//按人
                    break;
                case 3:
                    Send = GetListByArea(list);//按区域
                    break;
                case 4:
                    Send = GetListByHour(list);//按小时
                    if (Send != null)
                    {
                        Send.Sort((a, b) =>
                        {
                            return b.Name.CompareTo(a.Name);
                        });
                    }
                    break;
                default:
                    break;
            }
            return Send;
        }
    }
}
