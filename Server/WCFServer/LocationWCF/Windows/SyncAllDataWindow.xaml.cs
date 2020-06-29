using ArchorUDPTool.Tools;
using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.BaseData;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using Location.BLL.Tool;
using LocationServer.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using WebApiLib;
using WebApiLib.Clients;
using WPFClientControlLib;

namespace LocationServer.Windows
{
    /// <summary>
    /// SyncAllDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SyncAllDataWindow : Window
    {
        public SyncAllDataWindow()
        {
            InitializeComponent();
            

            logController.Init(TbLogs, LogTags.BaseData);
        }

        LogTextBoxController logController = new LogTextBoxController();

        private void Window_Closed(object sender, EventArgs e)
        {
            logController.Dispose();
        }

        private void MenuSync_Click(object sender, RoutedEventArgs e)
        {
            Sync();
        }

        private void MenuCreateSimulateData_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                BaseDataSimulator simulator = new BaseDataSimulator();
                simulator.SaveDepToOrg();
                simulator.SavePersonnelToUser();
                simulator.SaveAreaToZone();
                simulator.SaveDevInfoToDevice();
                simulator.SaveGuardCardToCard();
            }, () =>
             {
                 Log.Info(LogTags.BaseData, "创建完成!");
                 MessageBox.Show("创建完成");
             });
        }

        private string datacaseUrl = "ipms-demo.datacase.io";
        private string ParkName = "";
        private string suffix = "api";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                datacaseUrl = AppContext.DatacaseWebApiUrl;
                Log.Info(LogTags.BaseData, "datacaseUrl:" + datacaseUrl);
                ParkName = AppContext.ParkName;
                if (ParkName == "中山嘉明电厂")
                {
                    suffix = "zhongshan";
                }
                client = new BaseDataClient(datacaseUrl, null, suffix);

                //Sync();

                //GetDate();

                //SetCameraInfoDataGrid();
            }
            catch (Exception exception)
            {
                Log.Info(LogTags.BaseData, "启动同步窗口出错 url:"+ client.client.BaseUri+" error:" + exception);
            }

        }

        private void SetCameraInfoDataGrid()
        {
            Bll bll = Bll.Instance();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
            dg_camera.ItemsSource = dclst;
            if(dclst!=null)
                LbCameraInfoCount.Content = dclst.Count;
        }

        private void Sync()
        {
            Worker.Run(() =>
            {
                try
                {
                    //bool isSave1 = true;
                    bool isSave2 = true;

                    //区域
                    //zoneList = client.GetZoneList();
                    areaList = client.GetAreaList(isSave2);

                    //组织
                    //orgList = client.GetOrgList();
                    depList = client.GetDepList(isSave2);

                    //人员
                    //userList = client.GetUserList();
                    personnelList = client.GetPersonnelList(isSave2);

                    //设备
                    //deviceList = client.GetDeviceList(null, null, null);
                    devInfoList = client.GetDevInfoList(null, null, null, isSave2);

                    //门禁
                    //cardList = client.GetCardList();
                    guardCardList = client.GetGuardCardList(isSave2);

                    //告警事件
                    eventList = client.GetEventList(null, null, null, null);
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                Log.Info(LogTags.BaseData, "同步完成!");
                MessageBox.Show("同步完成");
            });
        }

        BaseDataClient client;

        List<zone> zoneList;
        List<Area> areaList;
        List<org> orgList;
        List<Department> depList;
        List<user> userList;
        List<Personnel> personnelList;
        List<device> deviceList;
        List<DevInfo> devInfoList;
        List<cards> cardList;
        List<EntranceGuardCard> guardCardList;
        List<events> eventList;

        private void MenuGet_Click(object sender, RoutedEventArgs e)
        {
            GetDate();
        }

        private void GetDate()
        {
            Worker.Run(() =>
            {
                try
                {
                    WebApiHelper.IsSaveJsonToFile = true;//保存数据到文件中
                    bool isSave1 = true;
                    bool isSave2 = false;
                    using (Bll bll = Bll.NewBllNoRelation())
                    {
                        //区域
                        
                        zoneList = client.GetZoneList();
                        Log.Error(LogTags.BaseData, "1 GetZoneList："+ zoneList.Count);
                        //areaList = client.GetAreaList(isSave2);
                        areaList = bll.Areas.Where(i => i.Type != DbModel.Tools.AreaTypes.CAD).ToList();
                        Log.Error(LogTags.BaseData, "2 GetAreaList:" + areaList.Count);
                        
                        //组织
                        orgList = client.GetOrgList();
                        Log.Error(LogTags.BaseData, "3 GetOrgList：" + orgList.Count);
                        //depList = client.GetDepList(isSave2);
                        depList = bll.Departments.ToList();
                        Log.Error(LogTags.BaseData, "4 GetDepList：" + depList.Count);

                        //人员
                        userList = client.GetUserList();
                        Log.Error(LogTags.BaseData, "5 GetUserList：" + orgList.Count);
                        //personnelList = client.GetPersonnelList(isSave2);
                        personnelList = bll.Personnels.ToList();
                        Log.Error(LogTags.BaseData, "6 GetPersonnelList：" + personnelList.Count);

                        //设备
                        deviceList = client.GetDeviceList(null, null, null);
                        Log.Error(LogTags.BaseData, "7 GetDeviceList：" + deviceList.Count);
                        //devInfoList = client.GetDevInfoList(null, null, null, isSave2);
                        devInfoList = bll.DevInfos.ToList();
                        Log.Error(LogTags.BaseData, "8 GetDevInfoList：" + devInfoList.Count);

                        //门禁
                        cardList = client.GetCardList();
                        Log.Error(LogTags.BaseData, "9 GetCardList：" + cardList.Count);
                        //guardCardList = client.GetGuardCardList(isSave2);
                        guardCardList = bll.EntranceGuardCards.ToList();

                        //告警事件
                        eventList = client.GetEventList(null, null, null, null);

                        WebApiHelper.IsSaveJsonToFile = false;
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                dg_zone.ItemsSource = zoneList;
                dg_area.ItemsSource = areaList;

                dg_org.ItemsSource = orgList;
                dg_dep.ItemsSource = depList;

                dg_user.ItemsSource = userList;
                dg_person.ItemsSource = personnelList;

                dg_dev.ItemsSource = deviceList;
                dg_devInfo.ItemsSource = devInfoList;

                dg_card.ItemsSource = cardList;
                dg_guardCards.ItemsSource = guardCardList;

                dg_event.ItemsSource = eventList;
            });
        }

        private void MenuGetRtsp_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                var list1 = client.GetCameraInfoList(null, null, null, true);
                var list2 = client.GetCameraInfoList("1021,1022,1023", null, null, true);
            }, () =>
            {
                SetCameraInfoDataGrid();
                Log.Info(LogTags.BaseData, "完成!");
                MessageBox.Show("完成");
            });
        }

        private void MenuCreateRealData_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                BaseDataInitializer initializer = new BaseDataInitializer();
                deviceList=initializer.InitDevices();

            }, () =>
            {
                dg_zone.ItemsSource = zoneList;
                dg_org.ItemsSource = orgList;
                dg_user.ItemsSource = userList;
                dg_dev.ItemsSource = deviceList;
                dg_event.ItemsSource = eventList;

                Log.Info(LogTags.BaseData, "完成!");
                MessageBox.Show("完成");
            });
        }

        private void BtnShowCameras_OnClick(object sender, RoutedEventArgs e)
        {
            var list = deviceList.Where(i => i.type == 1021 || i.type == 1022 || i.type == 1023 || i.type ==102).ToList();
            LbDevCount.Content = list.Count;
            dg_dev.ItemsSource = list;
        }
        /// <summary>
        /// json获取用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGetUserOfJson_Click(object sender, RoutedEventArgs e)
        {
            personnelList=client.GetPersonnelList(true);
        }

        private void MenuSyncArea_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                try
                {
                    zoneList = client.GetZoneList();
                    Log.Error(LogTags.BaseData, "GetZoneList:" + zoneList.Count);
                    areaList = client.GetAreaList(false);
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                Log.Info(LogTags.BaseData, "同步完成!");
                MessageBox.Show("同步完成");
            });
        }

        private void MenuSyncDep_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                try
                {
                    orgList = client.GetOrgList();
                    Log.Error(LogTags.BaseData, "GetZoneList:" + zoneList.Count);
                    depList = client.GetDepList(true);
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                dg_org.ItemsSource = orgList;
                dg_dep.ItemsSource = depList;
                Log.Info(LogTags.BaseData, "同步完成!");
                MessageBox.Show("同步完成");
            });
        }

        private static void RemoveNoCardAndDepRepeatPerson(Bll bll, List<Personnel> personnelsHaveCard, List<Personnel> personnelsHaveNoCardAndDep)
        {
            List<Personnel> deletePersonnels = new List<Personnel>();
            //foreach (var item in personnelsHaveNoCardAndDep)
            for (int j = 0; j < personnelsHaveNoCardAndDep.Count; j++)
            {
                var item = personnelsHaveNoCardAndDep[j];
                var item2 = personnelsHaveCard.Find(i => i.Name == item.Name);//存在重名的人员
                if (item2 != null)
                {
                    if (!deletePersonnels.Contains(item))
                        deletePersonnels.Add(item);
                }

                var item3 = personnelsHaveNoCardAndDep.Find(i => i.Name == item.Name && i.Id != item.Id);//存在重名的人员
                if (item3 != null)
                {
                    var alarms1 = bll.LocationAlarmHistorys.FindAll(i => i.PersonnelId == item.Id);
                    var alarms2 = bll.LocationAlarms.FindAll(i => i.PersonnelId == item.Id);

                    var alarms3 = bll.LocationAlarmHistorys.FindAll(i => i.PersonnelId == item3.Id);
                    var alarms4 = bll.LocationAlarms.FindAll(i => i.PersonnelId == item3.Id);

                    if (alarms1.Count == 0 && alarms2.Count == 0 && alarms3.Count == 0 && alarms4.Count == 0)
                    {
                        Personnel personnel = item;
                        if (string.IsNullOrEmpty(item3.Mobile))
                        {
                            personnel = item3;
                        }
                        if (!deletePersonnels.Contains(personnel))
                            deletePersonnels.Add(personnel);

                        personnelsHaveNoCardAndDep.Remove(item3);
                    }
                    else if (alarms1.Count == 0 && alarms2.Count == 0)
                    {
                        if (!deletePersonnels.Contains(item))
                            deletePersonnels.Add(item);
                    }
                    else if (alarms3.Count == 0 && alarms4.Count == 0)
                    {
                        if (!deletePersonnels.Contains(item3))
                            deletePersonnels.Add(item3);
                    }
                    else
                    {

                    }


                    personnelsHaveNoCardAndDep.Remove(item3);
                }
            }

            bll.Personnels.RemoveList(deletePersonnels);//
        }

        private static void RemoveNoCardRepeatPerson(Bll bll, List<Personnel> personnelsHaveCard, List<Personnel> personnelsHaveNoCard)
        {
            List<Personnel> deletePersonnels = new List<Personnel>();
            List<Personnel> editPersonnels = new List<Personnel>();
            //foreach (var item in personnelsHaveNoCardAndDep)
            for (int j = 0; j < personnelsHaveNoCard.Count; j++)
            {
                var item = personnelsHaveNoCard[j];
                var item2 = personnelsHaveCard.Find(i => i.Name == item.Name);//存在重名的人员
                if (item2 != null)
                {
                    if (!deletePersonnels.Contains(item))
                        deletePersonnels.Add(item);

                    if (item2.Mobile == null)
                        item2.Mobile = item.Mobile;
                    if (item2.WorkNumber == null)
                        item2.WorkNumber = item.WorkNumber;
                    editPersonnels.Add(item2);
                }

                var item3 = personnelsHaveNoCard.Find(i => i.Name == item.Name && i.Id != item.Id);//存在重名的人员
                if (item3 != null)
                {
                    var alarms1 = bll.LocationAlarmHistorys.FindAll(i => i.PersonnelId == item.Id);
                    var alarms2 = bll.LocationAlarms.FindAll(i => i.PersonnelId == item.Id);

                    var alarms3 = bll.LocationAlarmHistorys.FindAll(i => i.PersonnelId == item3.Id);
                    var alarms4 = bll.LocationAlarms.FindAll(i => i.PersonnelId == item3.Id);

                    if (alarms1.Count == 0 && alarms2.Count == 0 && alarms3.Count == 0 && alarms4.Count == 0)
                    {
                        Personnel personnel = item;
                        if (item.ParentId == 1)
                        {
                            personnel = item;


                            if (item.Mobile == null)
                                item.Mobile = item3.Mobile;
                            if (item.WorkNumber == null)
                                item.WorkNumber = item3.WorkNumber;
                            editPersonnels.Add(item);
                        }
                        else if(item3.ParentId == 1)
                        {
                            personnel = item3;

                            if (item3.Mobile == null)
                                item3.Mobile = item.Mobile;
                            if (item3.WorkNumber == null)
                                item3.WorkNumber = item.WorkNumber;
                            editPersonnels.Add(item3);
                        }
                        else if (string.IsNullOrEmpty(item3.Mobile))
                        {
                            personnel = item3;

                            if (item3.Mobile == null)
                                item3.Mobile = item.Mobile;
                            if (item3.WorkNumber == null)
                                item3.WorkNumber = item.WorkNumber;
                            editPersonnels.Add(item3);
                        }
                        else
                        {

                        }

                        if (!deletePersonnels.Contains(personnel))
                            deletePersonnels.Add(personnel);

                        personnelsHaveNoCard.Remove(item3);
                    }
                    else if (alarms1.Count == 0 && alarms2.Count == 0)
                    {
                        if (!deletePersonnels.Contains(item))
                            deletePersonnels.Add(item);

                        if (item3.Mobile == null)
                            item3.Mobile = item.Mobile;
                        if (item3.WorkNumber == null)
                            item3.WorkNumber = item.WorkNumber;
                        editPersonnels.Add(item3);
                    }
                    else if (alarms3.Count == 0 && alarms4.Count == 0)
                    {
                        if (!deletePersonnels.Contains(item3))
                            deletePersonnels.Add(item3);

                        if (item.Mobile == null)
                            item.Mobile = item3.Mobile;
                        if (item.WorkNumber == null)
                            item.WorkNumber = item3.WorkNumber;
                        editPersonnels.Add(item);
                    }
                    else
                    {

                    }


                    personnelsHaveNoCard.Remove(item3);
                }
            }

            bll.Personnels.RemoveList(deletePersonnels);//
            bll.Personnels.EditRange(editPersonnels);
        }

        private void MenySyncPerson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSyncDev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSyncDoor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSyncEvens_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuClearPerson_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                try
                {
                    orgList = client.GetOrgList();

                    List<member> members = new List<member>();
                    foreach (var org in orgList)
                    {
                        if (org.members != null)
                        {
                            members.AddRange(org.members);
                            foreach (var member in org.members)
                            {
                                member.parent = org;
                            }
                        }
                    }

                    userList = client.GetUserListEx();
                    var userNames = userList.Select(i => i.name).ToList();

                    List<user> userList2 = new List<user>(userList);
                    List<member> membersNoUser = new List<member>();
                    foreach (var member in members)
                    {
                        var user = userList2.Find(i => i.id == member.personId);
                        if (user != null)
                        {
                            member.user = user;
                            user.member = member;
                            //member.parent.AddUser(user);
                            //userList2.Remove(user);//找出多少个没有关联member的user

                        }
                        else
                        {
                            membersNoUser.Add(member);
                        }
                    }
                    List<user> userListNoOrg = new List<user>();

                    foreach (var user in userList)
                    {
                        var org = orgList.Find(i => i.name == user.dep_name);
                        if (org != null)
                        {
                            org.AddUser(user);
                            user.parent = org;
                        }
                        else
                        {
                            userListNoOrg.Add(user);
                        }
                    }

                    List<user> userList11 = userListNoOrg.FindAll(i => i.member == null).OrderBy(i => i.id).ToList();
                    List<user> userList12 = userListNoOrg.FindAll(i => i.member != null).OrderBy(i => i.id).ToList();

                    List<user> userList21 = new List<user>();
                    List<user> userList22 = new List<user>();
                    foreach (var user in userList)
                    {
                        if (user.member != null && user.parent != null)
                        {
                            if (user.member.parent != user.parent)
                            {
                                userList21.Add(user);
                            }
                            else
                            {
                                userList22.Add(user);
                            }
                        }
                    }


                    Bll bll = new Bll();
                    var personnels = bll.Personnels.ToList().OrderBy(i => i.Name).ToList();
                    List<string> personNames = new List<string>();

                    List<string> userNames2 = new List<string>(userNames);
                    foreach (var u1 in personnels)
                    {
                        if (!personNames.Contains(u1.Name))
                        {
                            personNames.Add(u1.Name);
                            userNames2.Remove(u1.Name);
                        }
                    }

                    List<Personnel> addPersonnel = new List<Personnel>();
                    List<Personnel> editPersonnel = new List<Personnel>();

                    var userList222 = userList.FindAll(i => userNames2.Contains(i.name));

                    var deps = bll.Departments.ToList();
                    foreach (var p in personnels)
                    {
                        var u = userList.Find(i => i.name == p.Name);
                        if(u!= null)
                        {
                            userList.Remove(u);
                            if(p.Mobile== null && u.mobile!=null)
                            {
                                p.Mobile = u.mobile;
                                if(!editPersonnel.Contains(p))
                                    editPersonnel.Add(p);
                            }
                            if(p.Abutment_Id== null)
                            {
                                p.Abutment_Id = u.id;
                                if (!editPersonnel.Contains(p))
                                    editPersonnel.Add(p);
                            }
                            if(u.dep_name!= null)
                            {
                                if (p.Parent.Name == "未绑定")
                                {
                                    var dep = deps.Find(i => i.Name == u.dep_name);
                                    if (dep != null)
                                    {
                                        p.ParentId = dep.Id;
                                        if (!editPersonnel.Contains(p))
                                            editPersonnel.Add(p);
                                    }
                                }
                                else if (p.Parent.Name != u.dep_name)
                                {

                                }
                            }

                            
                        }
                        else
                        {
                            
                        }
                    }

                    

                    foreach (var u in userList)
                    {
                        Personnel pNew = new Personnel();
                        pNew.Name = u.name;
                        pNew.Mobile = u.mobile;
                        pNew.Abutment_Id = u.id;
                        if (u.dep_name== null)
                        {
                            pNew.ParentId = 1;
                        }
                        else
                        {
                            var dep = deps.Find(i => i.Name == u.dep_name);
                            if(dep!= null)
                            {
                                pNew.ParentId = dep.Id;
                            }
                            else
                            {
                                pNew.ParentId = 1;
                            }
                        }
                        addPersonnel.Add(pNew);

                    }

                    bll.Personnels.AddRange(addPersonnel);
                    bll.Personnels.EditRange(editPersonnel);
                    
                    var cards = bll.LocationCards.ToList();
                    var c2p = bll.LocationCardToPersonnels.ToList();

                    List<int> pIdList = new List<int>();
                    foreach (var item in c2p)
                    {
                        if (!pIdList.Contains(item.PersonnelId))
                        {
                            pIdList.Add(item.PersonnelId);
                        }
                    }

                    var personnelsHaveCard = personnels.FindAll(i => pIdList.Contains(i.Id));
                    var personnelsHaveNoCard = personnels.FindAll(i => !pIdList.Contains(i.Id));
                    var personnelsHaveNoCardAndDep = personnelsHaveNoCard.FindAll(i => i.ParentId == 1);//没有绑定卡，又没有部门的人员

                    //RemoveNoCardAndDepRepeatPerson(bll, personnelsHaveCard, personnelsHaveNoCardAndDep);//没有绑定卡，又没有部门的人员,重名的人员
                    RemoveNoCardRepeatPerson(bll, personnelsHaveCard, personnelsHaveNoCard);//没有绑定卡，又没有部门的人员,重名的人员


                    Log.Error(LogTags.BaseData, "GetZoneList:" + zoneList.Count);
                    depList = client.GetDepList(false);
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                dg_org.ItemsSource = orgList;
                dg_dep.ItemsSource = depList;
                Log.Info(LogTags.BaseData, "同步完成!");
                MessageBox.Show("同步完成");
            });
        }
    }
}
