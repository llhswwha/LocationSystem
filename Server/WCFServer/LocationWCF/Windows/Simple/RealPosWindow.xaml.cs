using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BLL;
using Coldairarrow.Util.Sockets;
using DbModel.LocationHistory.Data;
using Location.TModel.Location.Data;
using Location.TModel.Tools;
using LocationServer.Tools;
using LocationServices.Converters;
using LocationServices.Locations.Services;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for RealPosWindow.xaml
    /// </summary>
    public partial class RealPosWindow : Window
    {
        public RealPosWindow()
        {
            InitializeComponent();
        }

        private List<TagPosition> posList = new List<TagPosition>();


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private DispatcherTimer timer;

        private void MenuRefreshOnTheSpot_OnClick(object sender, RoutedEventArgs e)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(300);//300毫秒
                timer.Tick += Timer_Tick;
            }

            timer.Start();
        }

        private int simulateType = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            //SimulateOnTheSpot();
            Worker.Run(() =>
            {
                if (simulateType == 0)
                {
                    SimulateOnTheSpot();
                }
                else
                {
                    MoveRandom();
                }
                
            }, () => { RefreshData();});
        }

        private void SimulateOnTheSpot()
        {
            PosService service = new PosService();
            foreach (TagPosition position in posList)
            {
                if (offlinePos.Contains(position.Tag))
                {
                    continue;//离线不更新
                }
                position.DateTime = DateTime.Now;
                position.Time = DateTime.Now.ToStamp();
                //service.Put(position);
            }

            service.PutRange(posList);
        }

        private Random r = null;

        private void MoveRandom()
        {
            if (r == null)
            {
                r = new Random((int)DateTime.Now.Ticks);
            }

            var speed = r.Next(4);
            PosService service = new PosService();
            foreach (TagPosition position in posList)
            {
                if (offlinePos.Contains(position.Tag))
                {
                    continue;//离线不更新
                }
                position.DateTime = DateTime.Now;
                position.Time = DateTime.Now.ToStamp();

                position.X += r.Next(speed);
                position.Y += (float)r.NextDouble();
                position.Z += r.Next(speed);
                //service.Put(position);
            }

            service.PutRange(posList);
        }

        private void MenuStopSimulate_OnClick(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }  
        }

        private void MenuRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            PosService service = new PosService();
            posList = service.GetList();
            DataGrid1.ItemsSource = posList;
        }

        private void RealPosWindow_OnClosed(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        private void SetFlag(string flag,int power=0)
        {
            TagPosition pos = DataGrid1.SelectedItem as TagPosition;
            if (pos == null) return;
            pos.Flag = flag;
            if (power > 0)
            {
                pos.Power = power;
            }

            Modify(pos);
            RefreshData();
        }

        private void Modify(TagPosition pos)
        {
            //PosService service = new PosService();
            //service.Put(pos);
            SendPos(pos);
        }

        private LightUDP udp = null;

        private string SendPos(TagPosition tagPos)
        {
            var dbPos=tagPos.ToDbModel();

            Position pos = new Position();
            pos.SetProperty(dbPos);

            return SendPos(pos);
        }

        private string SendPos(Position pos)
        {
            if (pos == null) return "";
            string txt = pos.GetText(LocationContext.OffsetX, LocationContext.OffsetY);
            if (udp == null)
            {
                udp = new LightUDP("127.0.0.1", 5678);
            }
            udp.Send(txt, "127.0.0.1", 2323);
            return txt;

            //return "";
        }

        private void MenuSetOffline_OnClick(object sender, RoutedEventArgs e)
        {
            TagPosition pos = DataGrid1.SelectedItem as TagPosition;
            if (pos == null) return;
            //SetFlag("0:0:0:0:1");
            if (!offlinePos.Contains(pos.Tag))
            {
                offlinePos.Add(pos.Tag);
            }
        }

        public List<string> offlinePos = new List<string>();

        private void MenuSetWait_OnClick(object sender, RoutedEventArgs e)
        {
            SetFlag("0:0:0:0:1");
        }

        private void MenuSetPowerAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            SetFlag("0:0:0:0:0",360);
        }

        private void MenuSetNormal_OnClick(object sender, RoutedEventArgs e)
        {
            SetFlag("0:0:0:0:0",380);

            TagPosition pos = DataGrid1.SelectedItem as TagPosition;
            if (pos == null) return;
            offlinePos.Remove(pos.Tag);
        }

        private void MenuRandomMove_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuRefreshOnTheSpotOne_OnClick(object sender, RoutedEventArgs e)
        {
            TagPosition position = DataGrid1.SelectedItem as TagPosition;
            if (position == null) return;
            position.DateTime = DateTime.Now;
            position.Time = DateTime.Now.ToStamp();
            Modify(position);
            RefreshData();
        }
    }
}
