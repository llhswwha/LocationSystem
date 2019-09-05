using System;
using System.Collections.Generic;
using System.Linq;

namespace DbModel.LocationHistory.Data
{
    public class PositionList : IComparable<PositionList>
    {
        public string Name { get; set; }

        private int _count;

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        public List<Position> Items { get; set; }

        public PositionList()
        {

        }

        public PositionList(string name)
        {
            this.Name = name;
        }

        public void Add(Position pos)
        {
            if (Items == null)
            {
                Items = new List<Position>();
            }

            Items.Add(pos);
            _count = Items.Count;
        }

        public void Add(List<Position> list)
        {
            if (Items == null)
            {
                Items = new List<Position>();
            }

            Items.AddRange(list);
            _count = Items.Count;
        }

        public static List<PositionList> GetList(List<Position> posList, Func<Position, string> actionGetName)
        {
            Dictionary<string, PositionList> dateDict = new Dictionary<string, PositionList>();
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
                    dateDict.Add(name, new PositionList(name));
                }
                var list = dateDict[name];
                list.Add(pos);
            }
            var result = dateDict.Values.ToList();
            result.Sort();
            return result;
        }

        public static List<PositionList> GetListByDay(List<Position> posList)
        {
            return GetList(posList, i => i.DateTime.ToString("yyyy-MM-dd"));
        }

        public static List<PositionList> GetListByHour(List<Position> posList)
        {
            return GetList(posList, i => i.DateTime.ToString("yyyy-MM-dd HH"));
        }

        public static List<PositionList> GetListByCode(List<Position> posList)
        {
            return GetList(posList, i => i.Code);
        }

        public static List<PositionList> GetListByPerson(List<Position> posList)
        {
            return GetList(posList, i => i.PersonnelName);
        }

        public static List<PositionList> GetListByArea(List<Position> posList)
        {
            return GetList(posList, i => i.AreaPath);
        }

        public int CompareTo(PositionList other)
        {
            return other.Count.CompareTo(this.Count);
        }
    }
}
