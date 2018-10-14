using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.BaseData
{
    public class EntityList<T>
    {
        public int Total { get; set; }

        public string Msg { get; set; }

        public List<T> Data { get; set; }

        public EntityList()
        {
            Msg = "ok";
            Data = new List<T>();
        }

        public EntityList(List<T> data)
        {
            Data = data;
        }
    }
}
