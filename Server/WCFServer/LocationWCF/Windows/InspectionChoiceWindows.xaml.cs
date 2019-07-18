using BLL;
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
using DbModel.Location.Work;
using SignalRService.Hubs;
using LocationServices.Converters;

namespace LocationServer.Windows
{
    /// <summary>
    /// InspectionChoiceWindows.xaml 的交互逻辑
    /// </summary>
    public partial class InspectionChoiceWindows : Window
    {
        public InspectionChoiceWindows()
        {
            InitializeComponent();
        }

        private List<InspectionTrack> trackList;

        private void SendByItem_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InspectionTrack it = SendByItem.SelectedItem as InspectionTrack;
            if (it == null) return;

            InspectionTrackList TrackList2 = new InspectionTrackList();
            TrackList2.ReviseTrack.Add(it);
            InspectionTrackHub.SendInspectionTracks(TrackList2.ToTModel());//发送给客户端
            
        }

        private void SendAllItem_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (trackList == null || trackList.Count == 0) return;
            InspectionTrackList TrackList2 = new InspectionTrackList();
            TrackList2.ReviseTrack = trackList;

            InspectionTrackHub.SendInspectionTracks(TrackList2.ToTModel());//发送给客户端
        }

        private void BtnGetAllInspectionTrack_OnClick(object sender, RoutedEventArgs e)
        {
            if (trackList == null)
            {
                Bll bll = Bll.Instance();
                trackList = bll.InspectionTracks.ToList();//从数据库取

                if (trackList == null || trackList.Count() == 0)
                {
                    return;
                }
            }

            SendByItem.ItemsSource = trackList;
            SendAllItem.ItemsSource = trackList;
        }
    }
}
