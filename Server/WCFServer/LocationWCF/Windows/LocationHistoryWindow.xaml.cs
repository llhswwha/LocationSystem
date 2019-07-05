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
using DbModel.Location.Person;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using LocationServer.Tools;
using System.Threading;

namespace LocationServer.Windows
{
    /// <summary>
    /// LocationHistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LocationHistoryWindow : Window
    {
        public LocationHistoryWindow()
        {
            InitializeComponent();
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnGetAll_OnClick(object sender, RoutedEventArgs e)
        {
            GetAll();
        }

        private List<Position> allPoslist;

        private void GetAll(Action callback=null)
        {
            BtnGetAllActiveDay.IsEnabled = false;
            Worker.Run(() =>
            {
                Bll bll = Bll.Instance();
                //bll.history
                allPoslist = bll.Positions.ToList();
                var personnels = bll.Personnels.ToDictionary();
                foreach (var pos in allPoslist)
                {
                    var pid = pos.PersonnelID;
                    if (pid != null && personnels.ContainsKey((int)pid))
                    {
                        var p = personnels[(int) pid];
                        pos.PersonnelName = string.Format("{0}({1})",p.Name,pos.Code);
                    }
                    else
                    {
                        pos.PersonnelName = pos.Code;//有些卡对应的人员不存在
                    }

                   
                }

                var areas=bll.Areas.ToDictionary();
                foreach (var pos in allPoslist)
                {
                    var areaId = pos.AreaId;
                    if (areaId != null && areas.ContainsKey((int)areaId))
                    {
                        var area = areas[(int) areaId];
                        pos.Area = area;
                        pos.AreaPath = area.GetToBuilding(">");
                    }
                }


            }, () =>
            {
                BtnGetAllActiveDay.IsEnabled = true;
                DataGridLocationHistory.ItemsSource = allPoslist;
                if (callback != null)
                {
                    callback();
                }
            });
        }

        private void GetAllActiveDay()
        {
            Worker.Run(() => { return PositionList.GetListByDay(allPoslist); }, (result) =>
            {
                DataGridStatisticDay.ItemsSource = result;
                DataGridStatisticDayPerson.ItemsSource = null;
                DataGridStatisticDayPersonTime.ItemsSource = null;
                DataGridDayPersonPosList.ItemsSource = null;
            });

            Worker.Run(() => { return PositionList.GetListByPerson(allPoslist); }, (result) =>
            {
                DataGridStatisticPerson.ItemsSource = result;
                DataGridStatisticPersonDay.ItemsSource = null;
                DataGridStatisticPersonDayHour.ItemsSource = null;
                DataGridPersonDayPosList.ItemsSource = null;
            });

            Worker.Run(() => { return PositionList.GetListByArea(allPoslist); }, (result) =>
            {
                DataGridStatisticArea.ItemsSource = result;
                DataGridStatisticAreaDayHour.ItemsSource = null;
                DataGridStatisticAreaPerson.ItemsSource = null;
                DataGridAreaPosList.ItemsSource = null;
            });

            
        }

        private void BtnGetAllActiveDay_OnClick(object sender, RoutedEventArgs e)
        {
            if (allPoslist == null)
            {
                GetAll(GetAllActiveDay);
            }
            else
            {
                GetAllActiveDay();
            }
        }
        
        private void DataGridStatisticDay_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticDay.SelectedItem as PositionList;
            if (posList == null) return;

            Worker.Run(() =>
            {
                return PositionList.GetListByPerson(posList.Items);
            }, (result) =>
            {
                DataGridStatisticDayPerson.ItemsSource = result;
                DataGridStatisticDayPersonTime.ItemsSource = null;
                DataGridDayPersonPosList.ItemsSource = null;
            });
        }

        private void DataGridStatisticDayPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticDayPerson.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridStatisticDayPersonTime.ItemsSource = PositionList.GetListByHour(posList.Items);
            DataGridDayPersonPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticDayPersonTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticDayPersonTime.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridDayPersonPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticPerson.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridStatisticPersonDay.ItemsSource= PositionList.GetListByDay(posList.Items);
            DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticPersonDay_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticPersonDay.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridStatisticPersonDayHour.ItemsSource = PositionList.GetListByHour(posList.Items);
            DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticPersonDayHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticPersonDayHour.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticArea_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticArea.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridStatisticAreaPerson.ItemsSource = PositionList.GetListByPerson(posList.Items);
            DataGridAreaPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticAreaPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticAreaPerson.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridStatisticAreaDayHour.ItemsSource = PositionList.GetListByDay(posList.Items);
            DataGridAreaPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticAreaDayHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PositionList posList = DataGridStatisticAreaDayHour.SelectedItem as PositionList;
            if (posList == null) return;
            DataGridAreaPosList.ItemsSource = posList.Items;
        }
    }
}
