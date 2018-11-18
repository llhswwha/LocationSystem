using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFClientControlLib.Extensions
{
    public static class MenuExtensions
    {
        public static MenuItem AddMenu(this ContextMenu menu,string head, Action<object> action)
        {
            MenuItem menuItem=new MenuItem();
            menuItem.Header = head;
            menuItem.Click += (sender, e) =>
            {
                if (action != null)
                {
                    var tag = ((sender as MenuItem).Parent as ContextMenu).Tag;
                    action(tag);
                }
            };
            menu.Items.Add(menuItem);
            return menuItem;
        }

    }
}
