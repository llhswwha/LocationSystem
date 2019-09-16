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
using LocationServices.Locations.Services;

//using PosInfo = DbModel.LocationHistory.Data.PosInfo;
using PosInfo = DbModel.LocationHistory.Data.PosInfo;
//using PosInfoList = DbModel.LocationHistory.Data.PosInfoList;
using PosInfoList = DbModel.LocationHistory.Data.PosInfoList;
using Location.TModel.Tools;
using TModel.Tools;
using LocationServices.Tools;
using BLL.Tools;
using Base.Common.Tools;
using Location.IModel;

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
            Bll bll = Bll.NewBllNoRelation();
            //var person = bll.Personnels.Find(i => i.Name == "Tag_08BA");
            //if (person != null)
            //{
            //    PosHistoryService service = new PosHistoryService();
            //    var list=service.GetHistoryByPerson(person.Id, new DateTime(2019, 9, 5, 13, 10, 0), new DateTime(2019, 9, 5, 13, 20, 0));
            //    DataGridDayPersonPosList.ItemsSource = list;
            //}

            var person = CbPersonList.SelectedItem as Personnel;
            var date = (DateTime)DpDay.SelectedDate;
            var startTime = (DateTime)TpStart.Value;
            var endTime = (DateTime)TpEnd.Value;

            PosHistoryService service = new PosHistoryService();
            var list = service.GetHistoryByPerson(person.Id,
                new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, 0),
                 new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, 0));
            DataGridDayPersonPosList.ItemsSource = list;
        }

        private void LocationHistoryWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            controller = new LogTextBoxController(TbLogs, LogTags.HisPos);

            Worker.Run(() =>
            {
                DateTime start = DateTime.Now;

                int count = bll.Positions.DbSet.Count();
                return count;
            }, (count) =>
            {
                this.Title += " total:" + count;
            });

            TbMaxSpeed.Text = AppContext.MoveMaxSpeed + "";

            //var personnels = bll.Personnels.ToList();
            //CbPersonList.ItemsSource = personnels;
            //var person = personnels.Find(i => i.Name == "Tag_08BA");
            //CbPersonList.SelectedItem = person;

            //DpDay.SelectedDate = new DateTime(2019, 9, 5).Date;

            //TpStart.Value = new DateTime(2019, 9, 5, 13, 10, 0);
            //TpEnd.Value = new DateTime(2019, 9, 5, 13, 20, 0);

            //SetSearchInfo("Tag_08BA", new DateTime(2019, 9, 5, 13, 10, 0), new DateTime(2019, 9, 5, 13, 20, 0));

            SetSearchInfo("Tag_08BA", new DateTime(2019, 9, 5, 11, 04, 0), new DateTime(2019, 9, 5, 11, 10, 0));
        }

        private void SetSearchInfo(string name,DateTime start,DateTime end)
        {
            var personnels = bll.Personnels.ToList();
            CbPersonList.ItemsSource = personnels;
            var person = personnels.Find(i => i.Name == name);
            CbPersonList.SelectedItem = person;

            DpDay.SelectedDate = start.Date;

            TpStart.Value = start;
            TpEnd.Value = end;


        }

        //private void BtnGetAll_OnClick(object sender, RoutedEventArgs e)
        //{
        //    GetAll(true);
        //}

        private List<PosInfo> allPoslist;

        private void GetAll(bool useBuffer,Action callback=null)
        {
            Log.Info(LogTags.HisPos, string.Format("GetAll Start"));

            MenuGetAllData.IsEnabled = false;
            Worker.Run(() =>
            {
                try
                {
                    allPoslist = new List<PosInfo>();
                    PosHistoryService phs = new PosHistoryService();
                    allPoslist=phs.GetAllData(LogTags.HisPos, useBuffer);
                    DateTime start = DateTime.Now;
                    Log.Info(LogTags.HisPos, string.Format("GetAll End"));
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }


            }, () =>
            {
               
                //DataGridLocationHistory.ItemsSource = allPoslist;

                if (allPoslist == null)
                {
                    MessageBox.Show("出错了，等待30s后再尝试一下");
                }

                MenuGetAllData.IsEnabled = true;

                if (callback != null)
                {
                    callback();
                }
            });
        }

        private void GetAllActiveDay(Action callback)
        {
            Worker.Run(() => { return PosInfoListHelper.GetListByDay(allPoslist); }, (result) =>
            {
                DataGridStatisticDay.ItemsSource = result;
                DataGridStatisticDayPerson.ItemsSource = null;
                DataGridStatisticDayPersonTime.ItemsSource = null;
                DataGridDayPersonPosList.ItemsSource = null;

                Worker.Run(() => { return PosInfoListHelper.GetListByPerson(allPoslist); }, (result2) =>
                {
                    DataGridStatisticPerson.ItemsSource = result2;
                    DataGridStatisticPersonDay.ItemsSource = null;
                    DataGridStatisticPersonDayHour.ItemsSource = null;
                    DataGridPersonDayPosList.ItemsSource = null;

                    Worker.Run(() => { return PosInfoListHelper.GetListByArea(allPoslist); }, (result3) =>
                    {
                        DataGridStatisticArea.ItemsSource = result3;
                        DataGridStatisticAreaDayHour.ItemsSource = null;
                        DataGridStatisticAreaPerson.ItemsSource = null;
                        DataGridAreaPosList.ItemsSource = null;

                        if (callback != null)
                        {
                            callback();
                        }
                    });
                });
            });

            

            

            
        }
        private void DataGridStatisticDay_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticDay.SelectedItem as PosInfoList;
            if (posList == null) return;

            Worker.Run(() =>
            {
                return PosInfoListHelper.GetListByPerson(posList.Items);
            }, (result) =>
            {
                DataGridStatisticDayPerson.ItemsSource = result;
                DataGridStatisticDayPersonTime.ItemsSource = null;
                DataGridDayPersonPosList.ItemsSource = null;
            });
        }

        private void DataGridStatisticDayPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticDayPerson.SelectedItem as PosInfoList;
            if (posList == null) return;
            var list= PosInfoListHelper.GetListByHour(posList.Items);
            DataGridStatisticDayPersonTime.ItemsSource = list;

            var list2 = posList.Items;

            list2.Sort((a, b) =>
            {
                return a.DateTimeStamp.CompareTo(b.DateTimeStamp);
            });

            DataGridDayPersonPosList.ItemsSource = list2;
        }

        private void DataGridStatisticDayPersonTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Info(LogTags.HisPos, string.Format("DataGridStatisticDayPersonTime_OnSelectionChanged"));

            PosInfoList posList = DataGridStatisticDayPersonTime.SelectedItem as PosInfoList;

            if (posList == null)
            {
                Log.Info(LogTags.HisPos, string.Format("posList == null"));
                return;
            }
            else
            {
                Log.Info(LogTags.HisPos, string.Format("posList:"+ posList.Name));
            }

            var list = posList.Items;

            list.Sort((a, b) =>
            {
                return a.DateTimeStamp.CompareTo(b.DateTimeStamp);
            });

            DataGridDayPersonPosList.ItemsSource = list;

            if (posList.Items != null)
            {
                var count=posList.Items.Count;
                Log.Info(LogTags.HisPos, string.Format("count:" + count));
                if (posList.Items.Count > 0)
                {
                    Log.Info(LogTags.HisPos, string.Format("Items[0]:" + posList.Items[0]));
                }
            }

            
        }

        private void DataGridStatisticPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticPerson.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridStatisticPersonDay.ItemsSource= PosInfoListHelper.GetListByDay(posList.Items);
            //DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticPersonDay_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticPersonDay.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridStatisticPersonDayHour.ItemsSource = PosInfoListHelper.GetListByHour(posList.Items);
            //DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticPersonDayHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticPersonDayHour.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridPersonDayPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticArea_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticArea.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridStatisticAreaPerson.ItemsSource = PosInfoListHelper.GetListByPerson(posList.Items);
            //DataGridAreaPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticAreaPerson_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticAreaPerson.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridStatisticAreaDayHour.ItemsSource = PosInfoListHelper.GetListByDay(posList.Items);
            //DataGridAreaPosList.ItemsSource = posList.Items;
        }

        private void DataGridStatisticAreaDayHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticAreaDayHour.SelectedItem as PosInfoList;
            if (posList == null) return;
            DataGridAreaPosList.ItemsSource = posList.Items;
        }

        private LightUDP udp;

        private void BtnSendCurrentPos_OnClick(object sender, RoutedEventArgs e)
        {
            Position pos = GetPos(DataGridDayPersonPosList.SelectedItem);
            TbPostion.Text=SendPos(pos);
        }

        private string SendPos(PosInfo pos)
        {
            if (pos == null) return "";
            Position pos2 = bll.Positions.FindById(pos.Id);
            return SendPos(pos2);
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

        private void BtnSendNextPos_OnClick(object sender, RoutedEventArgs e)
        {
            SendNextPos();
        }

        private void SendNextPos()
        {
            //List<PosInfo> list = DataGridDayPersonPosList.ItemsSource as List<PosInfo>;
            if (DataGridDayPersonPosList.SelectedIndex < DataGridDayPersonPosList.Items.Count)
            {
                DataGridDayPersonPosList.SelectedIndex++;
                var pos = GetPos(DataGridDayPersonPosList.SelectedItem);
                TbPostion.Text = SendPos(pos);
            }
            else
            {
                //return null;
            }
        }

        private Position GetNextPos()
        {
            if (list != null && id < list.Count)
            {
                var pos = list[id];
                Position pos2 = bll.Positions.FindById(pos.Id);
                return pos2;
            }
            else if (list2 != null && id < list2.Count)
            {
                Location.TModel.LocationHistory.Data.Position pos1 = list2[id];
                Position pos2= bll.Positions.FindById(pos1.Id);
                return pos2;
            }
            else
            {
                return null;
            }
        }

        private DispatcherTimer timer;
        private List<PosInfo> list;
        private List<Location.TModel.LocationHistory.Data.Position> list2;
        private int id = 0;
        private void BtnStartSendPos_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnStartSendPos.Content.ToString() == "开始模拟数据")
            {
                BtnStartSendPos.Content = "停止模拟数据";
                list = DataGridDayPersonPosList.ItemsSource as List<PosInfo>;
                list2 = DataGridDayPersonPosList.ItemsSource as List<Location.TModel.LocationHistory.Data.Position>;
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
            //PosInfo pos = list[id];
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

        Bll bll = Bll.NewBllNoRelation();

        private void DataGridDayPersonPosList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Position pos = GetPos(DataGridDayPersonPosList.SelectedItem);
            if (pos != null)
            {
                TbPostion.Text = pos.GetText(LocationContext.OffsetX, LocationContext.OffsetY);
            }
            else
            {
                TbPostion.Text = "";
            }
            
        }

        private Position GetPos(object selectedItem)
        {
            Position pos = null;
            IId id = DataGridDayPersonPosList.SelectedItem as IId;
            if (id != null)
            {
                pos = bll.Positions.FindById(id.Id);
            }
            return pos;
        }

        private LogTextBoxController controller;



        private void MenuGetAllData_Click(object sender, RoutedEventArgs e)
        {
            GetAll(true, ()=>
            {
                GetAllActiveDay(null);
            });
        }

        private void MenuGetAllDataRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetAll(false, () =>
            {
                GetAllActiveDay(null);
            });
        }

        private void MenuRemoveRepeatData_Click(object sender, RoutedEventArgs e)
        {
            RemoveRepeatData(true);

        }

        private void SetPositionInfo()
        {
            List<PosInfoList> list = DataGridStatisticDay.ItemsSource as List<PosInfoList>;
            for (int i = 0; i < list.Count; i++)
            {
                PosInfoList posList = list[i];
                string progress1 = string.Format("Progress1 》》 Name:{0},Count:{1:N} ({2}/{3})", posList.Name,
                    posList.Count, (i + 1), list.Count);
                Log.Info(LogTags.HisPos, progress1);

                var noAreaIdList = posList.Items.Where(p => p.AreaId == null).Select(p => p.Id).ToList();
                if (noAreaIdList.Count > 0)
                {
                    var tagRelation = TagRelationBuffer.Instance();
                    var posList2 = bll.Positions.Where(p => noAreaIdList.Contains(p.Id));
                    foreach (Position position in posList2)
                    {
                        //tagRelation.SetArea(position);//设置区域
                        //if (!newAreas.Contains(position.AreaId))
                        //{
                        //    newAreas.Add(position.AreaId);
                        //}
                        position.AreaId = tagRelation.GetParkArea().Id;//直接设置
                    }
                    bool r = bll.Positions.EditRange(posList2);//保存到数据库
                }

                var noPersonList = posList.Items.Where(p => p.PersonnelID == null).Select(p => p.Id).ToList();
                if (noPersonList.Count > 0)
                {
                    Log.Info(LogTags.HisPos, "SetTagAndPerson Start:" + noPersonList.Count);

                    int pageSize = 5000;
                    int pageTotal = noPersonList.Count / pageSize + 1;
                    Log.Info(LogTags.HisPos, "pageTotal:" + pageTotal);

                    for (int page = 0; page < pageTotal; page++)
                    {
                        var pageList = noPersonList.Skip(page * pageSize).Take(pageSize).ToList();

                        

                        var tagRelation = TagRelationBuffer.Instance();
                        var posList2 = bll.Positions.Where(p => pageList.Contains(p.Id));
                        //Log.Info(LogTags.HisPos, "GetPositionList:" + posList2.Count);

                        for (int i1 = 0; i1 < posList2.Count; i1++)
                        {
                            Position position = posList2[i1];

                            tagRelation.SetTagAndPerson(position);

                            //if (i1 % 1000 == 0)
                            //{
                            //    Log.Info(LogTags.HisPos, string.Format("SetTagAndPerson  ({0}/{1})", (i1 + 1), posList2.Count));
                            //}
                        }
                        bool r = bll.Positions.EditRange(posList2);//保存到数据库

                        Log.Info(LogTags.HisPos, string.Format("SetTagAndPerson {3} Page  ({0}/{1}) {2} {4}", (page + 1), pageTotal, pageList.Count, progress1,r));
                    }
                    Log.Info(LogTags.HisPos, "SetTagAndPerson End");

                    //Log.Info(LogTags.HisPos, "SetTagAndPerson Start:"+ noPersonList.Count);
                    //var tagRelation = TagRelationBuffer.Instance();
                    //var posList2 = bll.Positions.Where(p => noPersonList.Contains(p.Id));
                    //Log.Info(LogTags.HisPos, "GetPositionList:" + posList2.Count);

                    //for (int i1 = 0; i1 < posList2.Count; i1++)
                    //{
                    //    Position position = posList2[i1];

                    //    tagRelation.SetTagAndPerson(position);

                    //    if (i1 % 1000 == 0)
                    //    {
                    //        string progress2 = string.Format("SetTagAndPerson  ({0}/{1})", (i1 + 1), posList2.Count);
                    //        Log.Info(LogTags.HisPos, progress2);
                    //    }
                    //}
                    //bool r = bll.Positions.EditRange(posList2);//保存到数据库
                }
            }
        }

        private void RemoveRepeatData(bool isDelete)
        {
            List<PosInfoList> list = DataGridStatisticDay.ItemsSource as List<PosInfoList>;
            if (list == null) return;
            Worker.Run(() =>
            {
                int removeCount = 0;
                Log.Info(LogTags.HisPos, "RemoveRepeatData Start");
                for (int i = 0; i < list.Count; i++)
                {
                    PosInfoList posList = list[i];
                    string progress1 = string.Format("Progress1 》》 Name:{0},Count:{1:N} ({2}/{3})", posList.Name,
                        posList.Count, (i + 1), list.Count);
                    Log.Info(LogTags.HisPos, progress1);

                    var groupby = posList.Items.GroupBy(p => new { p.DateTimeStamp, p.PersonnelID }).Select(p => new
                    {
                        p.Key.DateTimeStamp,
                        p.Key.PersonnelID,
                        Id = p.First(w => true).Id,
                        list = p.Select(w => w.Id).ToList(),
                        total = p.Count()
                    });
                    var groupList = groupby.Where(k => k.total > 1).ToList();
                    if (groupList.Count == 0) continue;

                    var groupList1 = groupby.Where(k => k.total ==2).ToList();
                    var groupList2 = groupby.Where(k => k.total == 3).ToList();
                    var groupList3 = groupby.Where(k => k.total == 4).ToList();
                    var groupList4 = groupby.Where(k => k.total >1 && k.total <= 10).ToList();
                    var groupList5 = groupby.Where(k => k.total > 10).ToList();

                    Log.Info(LogTags.HisPos, string.Format("groupList:{0}", groupList.Count));

                    posList.Items.Clear();
                    //GC.Collect();
                    //return 0;

                    List<Position> removeListTemp = new List<Position>();
                    int maxPageCount = 10000;
                    int packageCount = maxPageCount;
                    List<long> timestampList = new List<long>();
                    List<int> idList = new List<int>();
                    for (int k = 0; k < groupList.Count; k++)
                    {
                        var item = groupList[k];
                        if (item.total > 1)
                        {
                            item.list.Remove(item.Id);
                            timestampList.Add(item.DateTimeStamp);
                            idList.AddRange(item.list);

                            //IQueryable<Position> query2=new 
                        }
                        if (idList.Count >= packageCount || 
                            (k== groupList.Count-1 && idList.Count>0)//最后一组
                            )
                        {
                            try
                            {
                                var query = bll.Positions.DbSet.Where(j => idList.Contains(j.Id));
                                //var sql = query.ToString();
                                var count2 = idList.Count();
                                if (isDelete)
                                {
                                    //query.DeleteFromQuery();
                                    bll.Positions.RemovePoints(query,false);
                                }
                                removeCount += count2;
                                Log.Info(LogTags.HisPos, string.Format("{5} || Progress2 》》 count:{0},total:{1:N} ({2}/{3},{4:F3})", count2, removeCount, (k + 1), groupList.Count, (k + 1.0) / groupList.Count, progress1));
                                timestampList = new List<long>();
                                idList = new List<int>();

                                if (packageCount < maxPageCount)
                                {
                                    packageCount *= 2;
                                    if (packageCount > maxPageCount)
                                    {
                                        packageCount = maxPageCount;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                //Log.Info(LogTags.HisPos, string.Format("{5} || Progress2 》》 count:{0},total:{1:N} ({2}/{3},{4:F3})", "NULL", removeCount, (k + 1), groupList.Count, (k + 1.0) / groupList.Count, progress1));

                                Log.Info(LogTags.HisPos, string.Format("Progress2 Error:{0}", e));
                                //return -1;//暂停

                                Thread.Sleep(50);
                                packageCount /= 2;
                                k--;//将包的大小减少 重新尝试
                                timestampList = new List<long>();
                                idList = new List<int>();
                            }
                            //Log.Info(LogTags.HisPos, string.Format("removeGroupCount:{0}", timestampList.Count));
                        }
                    }
                }
                Log.Info(LogTags.HisPos, "RemoveRepeatData End TotolCount:"+removeCount);
                return removeCount;
            }, (removeCount) =>
            {
                if (removeCount == -1) return;//暂停
                GetAll(false, () =>
                {
                    GetAllActiveDay(() =>
                    {
                        if (removeCount > 0)//直到没有重复的数据为止，一直循环。
                        {
                            RemoveRepeatData(isDelete);//递归
                        }
                        else
                        {
                            //Worker.Run(() =>
                            //{
                            //    SetPositionInfo(); //删除重复数据后再设置信息，不然很费时间

                            //}, () => { Log.Info(LogTags.HisPos, "完成"); });
                            MessageBox.Show("完成");
                        }
                    });
                });
            });
        }

        //private void GetRepeatDataCount(Action<long> callback)
        //{
        //    List<PosInfoList> list = DataGridStatisticDay.ItemsSource as List<PosInfoList>;
        //    if (list == null) return;
        //    Worker.Run(() =>
        //    {
        //        int removeCount = 0;
        //        Log.Info(LogTags.HisPos, "GetRepeatDataCount Start");
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            PosInfoList posList = list[i];
        //            string progress1 = string.Format("Progress1 》》 Name:{0},Count:{1:N} ({2}/{3})", posList.Name,
        //                posList.Count, (i + 1), list.Count);
        //            Log.Info(LogTags.HisPos, progress1);

        //            var groupList = posList.Items.GroupBy(p => new { p.DateTimeStamp }).Select(p => new
        //            {
        //                p.Key.DateTimeStamp,
        //                Id = p.First(w => true).Id,
        //                total = p.Count()
        //            }).Where(k => k.total > 1).ToList();
        //            Log.Info(LogTags.HisPos, string.Format("groupList:{0}", groupList.Count));

        //            List<Position> removeListTemp = new List<Position>();
        //            int maxPageCount = 128;
        //            int packageCount = maxPageCount;
        //            List<long> timestampList = new List<long>();
        //            List<int> idList = new List<int>();
        //            for (int k = 0; k < groupList.Count; k++)
        //            {
        //                var item = groupList[k];
        //                if (item.total > 1)
        //                {
        //                    timestampList.Add(item.DateTimeStamp);
        //                    idList.Add(item.Id);
        //                }
        //                if (timestampList.Count >= packageCount ||
        //                    (k == groupList.Count - 1 && timestampList.Count > 0)//最后一组
        //                    )
        //                {
        //                    try
        //                    {
        //                        var query = bll.Positions.DbSet.Where(j => timestampList.Contains(j.DateTimeStamp) && !idList.Contains(j.Id));
        //                        var sql = query.ToString();
        //                        var count2 = query.Count();
        //                        //query.DeleteFromQuery();
        //                        removeCount += count2;

        //                        Log.Info(LogTags.HisPos, string.Format("{5} || Progress2 》》 count:{0},total:{1:N} ({2}/{3},{4:F3})", count2, removeCount, (k + 1), groupList.Count, (k + 1.0) / groupList.Count, progress1));

        //                        timestampList = new List<long>();
        //                        idList = new List<int>();

        //                        if (packageCount < maxPageCount)
        //                        {
        //                            packageCount *= 2;
        //                            if (packageCount > maxPageCount)
        //                            {
        //                                packageCount = maxPageCount;
        //                            }
        //                        }
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        //Log.Info(LogTags.HisPos, string.Format("{5} || Progress2 》》 count:{0},total:{1:N} ({2}/{3},{4:F3})", "NULL", removeCount, (k + 1), groupList.Count, (k + 1.0) / groupList.Count, progress1));

        //                        Log.Info(LogTags.HisPos, string.Format("Progress2 Error:{0}", e));
        //                        //return -1;//暂停

        //                        Thread.Sleep(50);
        //                        packageCount /= 2;
        //                        k--;//将包的大小减少 重新尝试
        //                        timestampList = new List<long>();
        //                        idList = new List<int>();
        //                    }
        //                    //Log.Info(LogTags.HisPos, string.Format("removeGroupCount:{0}", timestampList.Count));
        //                }
        //            }
        //        }
        //        Log.Info(LogTags.HisPos, "RemoveRepeatData End");
        //        return removeCount;
        //    }, (removeCount) =>
        //    {
                
        //    });
        //}

        private void MenuGetRepeatDataCount_Click(object sender, RoutedEventArgs e)
        {
            RemoveRepeatData(false);
        }

        private void MenuGetTestData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuGetCount_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            int count = bll.Positions.DbSet.Count();
        }

        private void MenuGetTimestamp_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                Bll bll = Bll.NewBllNoRelation();

                //DateTime date2 = date.AddHours(2);
                DateTime now = DateTime.Now;
                DateTime last = now.AddDays(-1);
                DateTime date = last.Date;
                var stamp = date.ToStamp();
                Log.Info(LogTags.HisPos, string.Format("time:{0},stamp:{1}", date, stamp));

                string count = "";
                string delete = "";
                //for (int i = 0; i < 24; i++)
                //{
                //    DateTime date1 = date.AddHours(i);
                //    DateTime date2 = date.AddHours(i+1);
                //    stamp = date1.ToStamp();
                //    Log.Info(LogTags.HisPos, string.Format("[{0}]time:{1},stamp:{2}\n", i, date, stamp));

                //    //txt += string.Format("[{0}]time:{1},stamp:{2}\n", i, date, stamp);
                //    count += string.Format("{0}\n", bll.Positions.GetQuery_Count(date, date2));
                //    delete += string.Format("{0}\n", bll.Positions.GetQuery_Delete(date, date2));
                //    //var count= bll.Positions.GetPositionsCountOfDate(date, date2);
                //    //Log.Info(LogTags.HisPos, string.Format("count:{0}", count));
                //}

                for (int i = 0; i < 1440; i++)
                {
                    DateTime date1 = date.AddMinutes(i);
                    DateTime date2 = date.AddMinutes(i + 1);
                    stamp = date1.ToStamp();
                    Log.Info(LogTags.HisPos, string.Format("[{0}]time:{1},stamp:{2}\n", i, date, stamp));

                    //txt += string.Format("[{0}]time:{1},stamp:{2}\n", i, date, stamp);
                    count += string.Format("{0}\n", bll.Positions.GetQuery_Count(date, date2));
                    delete += string.Format("{0}\n", bll.Positions.GetQuery_Delete(date, date2));
                    //var count= bll.Positions.GetPositionsCountOfDate(date, date2);
                    //Log.Info(LogTags.HisPos, string.Format("count:{0}", count));
                }

                Log.Info(LogTags.HisPos, count);
                Log.Info(LogTags.HisPos, delete);

                //Bll bll = Bll.NewBllNoRelation();
                //DateTime date3 = date.AddHours(2);
                //var count = bll.Positions.GetPositionsCountOfDate(date, date2);
                //Log.Info(LogTags.HisPos, string.Format("count:{0}", count));

            }, () =>
            {

            });
           
        }

        private void FindErrorPoints_Click(object sender, RoutedEventArgs e)
        {
            List<PosInfo> posInfoList = DataGridDayPersonPosList.ItemsSource as List<PosInfo>;
            var errorPoints = PosDistanceHelper.FilterErrorPoints(posInfoList);
            foreach (var item in errorPoints)
            {
                var p = bll.Positions.DeleteById(item.Id);
                if (p != null)
                {
                    Log.Info(LogTags.HisPos, "删除点:" + p);
                }
            }
        }

        

        private void BtnSetMaxSpeed_Click(object sender, RoutedEventArgs e)
        {
            AppContext.MoveMaxSpeed = TbMaxSpeed.Text.ToDouble();
        }

        private void FindErrorPoints_Day_Click(object sender, RoutedEventArgs e)
        {
            PosInfoList posList = DataGridStatisticDay.SelectedItem as PosInfoList;//一天
            if (posList == null) return;

            Worker.Run(() =>
            {
                bll.Positions.RemoveErrorPoints(posList.Items);
            }, () =>
            {
                GetAll(false,null);
            });
        }

        private void FindErrorPoints_All_Click(object sender, RoutedEventArgs e)
        {
            PosHistoryService phs = new PosHistoryService();
            allPoslist = phs.GetAllData(LogTags.HisPos, true);
            Worker.Run(() =>
            {
                bll.Positions.RemoveErrorPoints(allPoslist);
            }, () =>
            {
                GetAll(false, null);
            });
        }
    }
}
