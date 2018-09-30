using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DbModel.Tools
{
    public class EnumToList
    {
        public static IList<SelectListItem> EnumToListChoice<T>()
        {
            IList<SelectListItem> listItem = new List<SelectListItem>();
            List<T> fg = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            foreach (T item in fg)
            {
                listItem.Add(new SelectListItem { Value = item.ToString(), Text = Enum.GetName(typeof(T), item) });
            }

            return listItem;
        }
    }
}
