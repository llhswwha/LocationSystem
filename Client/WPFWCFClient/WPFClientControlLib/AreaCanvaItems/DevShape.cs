using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
namespace WPFClientControlLib.AreaCanvaItems
{
    public class DevShape
    {
        public int Id { get; set; }

        public Rectangle Rect { get; set; }

        public Label Label { get; set; }

        Canvas _parent;

        public DevShape(Canvas parent)
        {
            _parent = parent;
        }

        public void Remove()
        {
            if (Rect != null)
            {
                _parent.Children.Remove(Rect);
            }
            if (Label != null)
            {
                _parent.Children.Remove(Label);
            }
        }
    }
}
