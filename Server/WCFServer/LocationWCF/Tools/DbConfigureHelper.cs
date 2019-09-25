using BLL;
using BLL.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public static class DbConfigureHelper
    {
        private static void InitArea(Dictionary<int, Area> areas_ms,string tag)
        {
            foreach (var kv in areas_ms)
            {
                try
                {
                    var item = kv.Value;
                    if (item.ParentId != null)
                    {
                        var pId = (int)item.ParentId;
                        if (areas_ms.ContainsKey(pId))
                        {
                            var parent = areas_ms[pId];
                            if (parent != null)
                            {
                                item.Parent = parent;
                                if (parent.Children == null)
                                {
                                    parent.Children = new List<Area>();
                                }
                                parent.Children.Add(item);
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Log.Error(tag, ex.ToString());
                }


            }
            foreach (var item in areas_ms)
            {
                item.Value.GetPath(AreaTypes.园区, "-");
            }
        }

        public static void LoadArchorList(string tag)
        {
            try
            {
                DateTime start = DateTime.Now;
                Log.Info(tag, "LoadArchorList Start");

                var Path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\DbInfos\\";
                string file1 = Path + "DevInfo.xls";
                string file2 = Path + "Archor.xls";
                string file3 = Path + "ArchorSetting.xls";

                Bll bll = Bll.NewBllNoRelation();
                Log.Info(tag, "archors_ms");
                var archors_ms = DbInfoHelper.GetArchors();//老的SQLServer
                Log.Info(tag, "archors_my");
                var archors_my = bll.Archors.ToList();//现在的mysql中的基站
                Log.Info(tag, "archors2Dict_my");
                var archors2Dict_my = bll.Archors.ToDictionary();
                Log.Info(tag, "archorSettings_my");
                var archorSettings_my = DbInfoHelper.GetArchorSettings();
                Log.Info(tag, "archors_list");

                var archors_list = ArchorHelper.LoadArchoDevInfo().ArchorList;//清单文件
                Log.Info(tag, "archors_engine");
                var archors_engine = bll.bus_anchors.ToList().FindAll(i => i.enabled == 1);//引擎数据
                Log.Info(tag, "archorConfig_engine");
                var archorConfig_engine = bll.bus_anchor_config.ToList().FindAll(i => i.net_gate != null);
                Log.Info(tag, "devs_ms_all");

                var devs_ms_all = DbInfoHelper.GetDevInfos();
                Log.Info(tag, "devs_ms");
                var devs_ms = devs_ms_all.FindAll(i => i.Local_TypeCode == 20180821);

                Log.Info(tag, "devs_my");
                var devs_my = bll.DevInfos.FindAll(i => i.Local_TypeCode == 20180821);
                Log.Info(tag, "archorSettings2");
                var archorSettings2 = bll.ArchorSettings.ToList();

                Log.Info(tag, "areas_ms");
                var areas_ms = DbInfoHelper.GetAreas().ToDictionary(i => i.Id);
                Log.Info(tag, "InitArea(areas_ms)");
                InitArea(areas_ms,tag);

                Log.Info(tag, "areas_my");
                var areas_my_list = bll.Areas.ToList();
                var areas_my = areas_my_list.ToDictionary(i => i.Id);
                Log.Info(tag, "InitArea(areas_my)");
                InitArea(areas_my, tag);

                foreach (var item in archors_my)
                {
                    item.DevInfo = devs_my.Find(i => i.Id == item.DevInfoId);
                }

                foreach (var item in archors_ms)
                {
                    item.DevInfo = devs_ms.Find(i => i.Id == item.DevInfoId);
                }

                bll.Archors.RemoveList(archors_my);
                bll.DevInfos.RemoveList(devs_my);
                bll.ArchorSettings.RemoveList(archorSettings2);

                //foreach (var item in archors_ms)
                for (int j = 0; j < archors_ms.Count; j++)
                {
                    var item = archors_ms[j];
                    //Log.Info(tag, string.Format("SetParent : {0}/{1}", j, archors_ms.Count));

                    var dev = item.DevInfo;
                    if (dev != null)
                    {
                        item.DevInfoId = dev.Id;

                        if (dev.ParentId != null)
                        {
                            var pid = (int)dev.ParentId;

                            var p1 = areas_ms[pid];
                            var p2 = areas_my_list.Find(i => i.Path == p1.Path);

                            dev.ParentId = p2.Id;
                            item.ParentId = p2.Id;
                        }
                    }
                }

                bll.DevInfos.AddRange(devs_ms);

                foreach (var item in archors_ms)
                {
                    item.DevInfoId = item.DevInfo.Id;
                }
                bll.Archors.AddRange(archors_ms);
                archors_my = archors_ms;

                foreach (var item in archorSettings_my)
                {
                    if (item.Archor != null)
                        item.ArchorId = item.Archor.Id;
                }
                bll.ArchorSettings.AddRange(archorSettings_my);

                //{
                //    List<Archor> temp11 = new List<Archor>();
                //    List<Archor> temp12 = new List<Archor>();
                //    foreach (var item in archors_my)
                //    {
                //        var old = archors_ms.Find(i => i.Code == item.Code);
                //        if (old != null)
                //        {
                //            temp11.Add(item);
                //            archors_ms.Remove(old);
                //        }
                //        else
                //        {
                //            temp12.Add(item);
                //        }
                //    }
                //}

                //{
                //    List<Archor> temp11 = new List<Archor>();
                //    List<Archor> temp12 = new List<Archor>();
                //    foreach (var item in archors_my)
                //    {
                //        var old = archors_list.Find(i => i.ArchorID == item.Code);
                //        if (old != null)
                //        {
                //            temp11.Add(item);
                //            archors_list.Remove(old);
                //        }
                //        else
                //        {
                //            temp12.Add(item);
                //        }
                //    }
                //}

                {
                    List<Archor> temp11 = new List<Archor>();
                    List<Archor> temp12 = new List<Archor>();
                    foreach (var item in archors_my)
                    {
                        var old = archorConfig_engine.Find(i => i.anchor_id == item.Code);
                        if (old != null)
                        {
                            temp11.Add(item);
                            archorConfig_engine.Remove(old);
                        }
                        else
                        {
                            temp12.Add(item);
                        }
                    }

                    List<DevInfo> devs = new List<DevInfo>();
                    foreach (var item in temp12)
                    {
                        devs.Add(item.DevInfo);
                    }

                    bll.Archors.RemoveList(temp12);
                    bll.DevInfos.RemoveList(devs);
                }

                var time = DateTime.Now - start;
                Log.Info(tag, "LoadArchorList End:"+time);
            }
            catch (Exception ex)
            {

                Log.Error(tag, ex.ToString());
            }
        }
    }
}
