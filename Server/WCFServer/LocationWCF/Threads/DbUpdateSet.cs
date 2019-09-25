using IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    public class DbUpdateSet<T> where T : IDictEntity
    {
        public List<T> DeleteList = new List<T>();
        public List<T> AddList = new List<T>();
        public List<T> EditList = new List<T>();

        public List<T> CurrentList = null;

        public List<T> NewList = null;

        public List<T> SendList = null;

        public DbUpdateSet(List<T> list)
        {
            CurrentList = new List<T>(list);
        }

        public void Update(List<T> list)
        {
            Clear();

            NewList = list;
            if (CurrentList == null)
            {
                CurrentList = new List<T>(list);
                AddList = new List<T>(list);
                SendList = new List<T>(list);
            }
            else
            {
                DeleteList.AddRange(CurrentList);
                foreach (var item in NewList)
                {
                    var existItem = CurrentList.Find(i => i.DictKey == item.DictKey);
                    if (existItem != null)
                    {
                        DeleteList.Remove(existItem);

                        //existItem.Update(item);
                        //EditList.Add(existItem);
                    }
                    else
                    {
                        AddList.Add(item);
                    }
                }

                SendList = new List<T>();
                SendList.AddRange(AddList);
                SendList.AddRange(EditList);


                CurrentList.AddRange(AddList);
                foreach (var item in DeleteList)
                {
                    CurrentList.Remove(item);
                }

                IsChanged = AddList.Count > 0 || DeleteList.Count > 0;
            }

        }

        public bool IsChanged;

        public void Clear()
        {
            DeleteList = new List<T>();
            AddList = new List<T>();
            EditList = new List<T>();
            //CurrentList = null;
            NewList = null;
            SendList = null;
        }
    }
}
