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
using BLL;
using DbModel.LocationHistory.Data;
using Location.BLL;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for LocationHistoryTestWindow.xaml
    /// </summary>
    public partial class LocationHistoryTestWindow : Window
    {
        public LocationHistoryTestWindow()
        {
            InitializeComponent();
        }


        private void BtnCreateHistoryPos_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();

            Position pos = PositionMocker.GetRandomPosition("223");
            pos.PersonnelID = 112;
            bll.Positions.Add(pos);

            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }

        private void BtnGetHistoryList_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();
            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }
    }
}
