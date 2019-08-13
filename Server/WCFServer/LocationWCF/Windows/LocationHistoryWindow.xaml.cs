using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Threading;
using Coldairarrow.Util.Sockets;
using Location.BLL.Tool;
using WPFClientControlLib;

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
            Log.Info(LogTags.HisPos, string.Format("GetAll Start"));

            BtnGetAllActiveDay.IsEnabled = false;
            Worker.Run(() =>
            {
                try
                {
                    allPoslist = new List<Position>();
                    Bll bll = Bll.NewBllNoRelation();
                    //bll.history

                    DateTime start = DateTime.Now;

                    int count = bll.Positions.DbSet.Count();
                    Log.Info(LogTags.HisPos, string.Format("count:{0}", count));

                    Position first=bll.Positions.DbSet.First();
                    Log.Info(LogTags.HisPos, string.Format("first:{0}", first));

                    List<Position> list1 = bll.Positions.GetPositionsOfDay(DateTime.Now);
                    Log.Info(LogTags.HisPos, string.Format("list1:{0},time:{1}", list1.Count, DateTime.Now - start));
                    start = DateTime.Now;

                    List<Position> list2 = bll.Positions.GetPositionsOfSevenDay(DateTime.Now);
                    Log.Info(LogTags.HisPos, string.Format("list2:{0},time:{1}", list2.Count, DateTime.Now - start));
                    start = DateTime.Now;

                    List<Position> list3 = bll.Positions.GetPositionsOfMonth(DateTime.Now);
                    Log.Info(LogTags.HisPos, string.Format("list3:{0},time:{1}", list3.Count, DateTime.Now - start));
                    start = DateTime.Now;

                    List<Position> list4 = bll.Positions.GetAllPositionsByDay((progress) =>
                    {
                        Log.Info(LogTags.HisPos, string.Format("GetAllPositionsByDay date:{0},count:{1},({2}/{3},{4:p})",
                            progress.Date, progress.Count, progress.Index, progress.Total, progress.Percent));
                    });
                    Log.Info(LogTags.HisPos, string.Format("list4:{0},time:{1}", list4.Count, DateTime.Now - start));
                    start = DateTime.Now;
                    //按天来要1分11s
                    List<Position> posList = list4;

                    //List<Position> list5 = bll.Positions.GetAllPositionsByMonth((progress) =>
                    //{
                    //    Log.Info(LogTags.HisPos, string.Format("GetAllPositionsByMonth date:{0},count:{1},({2}/{3},{4:p})",
                    //        progress.Date, progress.Count, progress.Index, progress.Total, progress.Percent));
                    //});
                    //Log.Info(LogTags.HisPos, string.Format("list5:{0},time:{1}", list5.Count, DateTime.Now - start));
                    //start = DateTime.Now;
                    ////按月来是22s=>把按天的去掉，按月的时间也是一样的50s-70s

                    //List<Position> posList = list5;

                    //List<Position> posList = bll.Positions.ToList();
                    Log.Info(LogTags.HisPos, string.Format("list6:{0},time:{1}", posList.Count, DateTime.Now - start));
                    if (posList == null)
                    {
                        allPoslist=null;
                        Log.Error(bll.Positions.ErrorMessage);
                        return;
                    }
                    var personnels = bll.Personnels.ToDictionary();
                    foreach (var pos in posList)
                    {
                        var pid = pos.PersonnelID;
                        if (pid != null && personnels.ContainsKey((int)pid))
                        {
                            var p = personnels[(int)pid];
                            pos.PersonnelName = string.Format("{0}({1})", p.Name, pos.Code);
                        }
                        else
                        {
                            pos.PersonnelName = string.Format("{0}({1})", pos.Code, pos.Code); ;//有些卡对应的人员不存在
                        }
                    }

                    List<Position> noAreaList = new List<Position>();

                    var areas = bll.Areas.ToDictionary();
                    foreach (var pos in posList)
                    {
                        var areaId = pos.AreaId;
                        if (areaId != null && areas.ContainsKey((int)areaId))
                        {
                            var area = areas[(int)areaId];
                            pos.Area = area;
                            pos.AreaPath = area.GetToBuilding(">");

                            allPoslist.Add(pos);
                        }
                        else
                        {
                            noAreaList.Add(pos);
                        }
                    }
                    Log.Info(LogTags.HisPos, string.Format("GetAll End"));
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }, () =>
            {
                BtnGetAllActiveDay.IsEnabled = true;
                DataGridLocationHistory.ItemsSource = allPoslist;

                if (allPoslist == null)
                {
                    MessageBox.Show("出错了，等待30s后再尝试一下");
                }

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

        private LightUDP udp;

        private void BtnSendCurrentPos_OnClick(object sender, RoutedEventArgs e)
        {
            Position pos = DataGridDayPersonPosList.SelectedItem as Position;
            TbPostion.Text=SendPos(pos);
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
        }

        private void BtnSendNextPos_OnClick(object sender, RoutedEventArgs e)
        {
            SendNextPos();
        }

        private Position SendNextPos()
        {
            //List<Position> list = DataGridDayPersonPosList.ItemsSource as List<Position>;
            if (DataGridDayPersonPosList.SelectedIndex < DataGridDayPersonPosList.Items.Count)
            {
                DataGridDayPersonPosList.SelectedIndex++;
                Position pos = DataGridDayPersonPosList.SelectedItem as Position;
                TbPostion.Text = SendPos(pos);
                return pos;
            }
            else
            {
                return null;
            }
        }

        private Position GetNextPos()
        {
            if (id < list.Count)
            {
                return list[id];
            }
            else
            {
                return null;
            }
        }

        private DispatcherTimer timer;
        private List<Position> list;
        private int id = 0;
        private void BtnStartSendPos_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnStartSendPos.Content.ToString() == "开始模拟数据")
            {
                BtnStartSendPos.Content = "停止模拟数据";
                list = DataGridDayPersonPosList.ItemsSource as List<Position>;
                id = 0;
                startPos = null;
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Tick += Timer_Tick;
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                }
                timer.Start();
            }
            else
            {
                BtnStartSendPos.Content = "开始模拟数据";
                timer.Stop();
            }
        }

        private Position startPos;

        private DateTime startTime;

        private void Timer_Tick(object sender, EventArgs e)
        {
            //Position pos = list[id];
            //id++;
            //TbPostion.Text = SendPos(pos);
            var pos = GetNextPos();
            if (pos == null)
            {
                BtnStartSendPos.Content = "开始模拟数据";
                timer.Stop();
            }
            else
            {
                if (startPos == null)
                {
                    startPos = pos;
                    startTime = DateTime.Now;

                    TbPostion.Text = SendPos(pos);
                    id++;
                    DataGridDayPersonPosList.SelectedIndex = id;
                }
                else
                {
                    TimeSpan t = DateTime.Now - startTime;
                    var ts = pos.DateTimeStamp - startPos.DateTimeStamp;
                    if (t.TotalMilliseconds > ts)
                    {
                        TbPostion.Text = SendPos(pos);
                        id++;
                        DataGridDayPersonPosList.SelectedIndex = id;
                    }
                }
            }
        }

        private void DataGridDayPersonPosList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Position pos = DataGridDayPersonPosList.SelectedItem as Position;
            if (pos == null) return;
            TbPostion.Text = pos.GetText(LocationContext.OffsetX, LocationContext.OffsetY);

        }

        private LogTextBoxController controller;

        private void LocationHistoryWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            controller=new LogTextBoxController(TbLogs,LogTags.HisPos);
        }
    }
}
