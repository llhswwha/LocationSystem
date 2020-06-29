using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using WebApiLib.Clients.OpcCliect;
using WebApiLib;
using Newtonsoft.Json;
using Location.TModel.Tools;
using LocationServices.ObjToData;

namespace LocationServices.Locations.Services
{
    public interface IEnergyService
    {
        string GetAllPowerGeneration();

        string GetAllGRLL();  //全场供热流量

        string  GetAllTRQLL();

        List<SisData> GetNOPFL();

        List<SisData> GetEveryCrewFH();

        List<SisData> GetSisDataList(string[] tags);
    }


    //能耗数据
   public  class EnergyService: IEnergyService
    {
        public OPCReadAuto opc;
        public EnergyService()
        {
          
        }

        public   string getOPC(string tagName)
        {
            try
            {
                string aa = "";
                   OPCReadAuto opc = new OPCReadAuto("10.146.33.4"); if (opc.IsConnected == true)
                {
                     aa = opc.GetOpcValueOne(tagName);
                }
               
                string result = aa.ToString();
                if (result == null || result == "")
                {
                    result = "0";
                }
                return aa.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("getOPC:"+ex.ToString());
                return null;
            }

        }

        public List<SisData> getList(string tags)
        {
            try
            {
                tags = tags.Replace(" ", "%20").Replace("#", "%23").Replace("+", "%2B").Replace("/", "%2F");
                string result = WebApiHelper.GetString("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                //List <SisData> sisList = WebApiHelper.GetEntity<List<SisData>>("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                List<SisData> sisList = JsonConvert.DeserializeObject<List<SisData>>(result, setting);
                return sisList;
            }
            catch (Exception ex)
            {
                Log.Error("EnergyService:getList:"+ex.ToString());
                return null;
            }
        }

        public string[] GetHisList(string tag)
        {
            try
            {
                string selectString = "\"SelectedTags\":[\"" + tag + "\"],\"StartTime\":\"2020-05-21 00:00:00\",\"EndTime\":\"2020-05-22 00:00:00\",\"Condition\":{\"IsRowData\":false,\"TimeSpace\":30,\"TimeUnit\":1,\"ValueMode\":1}";
                string resultHistory = WebApiHelper.GetString("http://10.146.33.9:20080/RealtimeQuery/api/GetTagsHistory?jsontext={" + selectString + "}");
                SisHistoryByUrl sisHUrl = WebApiHelper.GetEntity<SisHistoryByUrl>("http://10.146.33.9:20080/RealtimeQuery/api/GetTagsHistory?jsontext={" + selectString + "}");
                object[] objects = sisHUrl.Data[0].Values;

                string vstart = "";  //测点值
                string vend = "";
                for (int i = 0; i < objects.Length; i++)
                {
                    object aaa = objects[i];

                    if (i == 0)
                    {
                        List<object> tagValues = JsonConvert.DeserializeObject<List<object>>(aaa.ToString());
                        DateTime time = Convert.ToDateTime(tagValues[0]);
                        double utc = TimeConvert.DateTimeToInt(time);

                        DateTime dtime = TimeConvert.IntToDatetime(utc);
                        string s2 = tagValues[1].ToString();
                        vstart = s2;
                    }
                    else if (i == objects.Length - 1)
                    {
                        List<object> tagValues = JsonConvert.DeserializeObject<List<object>>(aaa.ToString());
                        DateTime time = Convert.ToDateTime(tagValues[0]);
                        double utc = TimeConvert.DateTimeToInt(time);

                        DateTime dtime = TimeConvert.IntToDatetime(utc);
                        string s2 = tagValues[1].ToString();
                        vend = s2;
                    }
                }
                string[] result =new string[] {vstart,vend };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取全厂发电量
        /// </summary>
        public   string GetAllPowerGeneration()
        {
            try
            {
                //NCS_34_AI67,NCS_34_AI71,2NCS_AI190,2NCS_AI198,2NCS_AI230,2NCS_AI238,2NCS_AI266,2NCS_AI274
                string[] r1 = GetHisList("NCS_34_AI67");
                string[] r2 = GetHisList("NCS_34_AI71");
                string[] r3 = GetHisList("2NCS_AI190");
                string[] r4 = GetHisList("2NCS_AI198");
                //string[] r5 = GetHisList("2NCS_AI230");
                //string[] r6 = GetHisList("2NCS_AI238");
                //string[] r7 = GetHisList("2NCS_AI266");
                //string[] r8 = GetHisList("2NCS_AI274");
                //  【NCS_34_AI67】*684000   //opc中TagName需调整
                double a1 = Convert.ToDouble(r1[1])-Convert.ToDouble(r1[0]);
                //  +【NCS_34_AI71】*684000
                double a2 = Convert.ToDouble(r2[1]) - Convert.ToDouble(r2[0]);
                //  +【2NCS_AI190】*512000                                
                double a3 = Convert.ToDouble(r3[1]) - Convert.ToDouble(r3[0]);
                //  +【2NCS_AI198】*252000                              
                double a4 = Convert.ToDouble(r4[1]) - Convert.ToDouble(r4[0]);
                //  +【2NCS_AI230】*512000
                double a5 = 0;
                //  +【2NCS_AI238】*252000
                double a6 = 0;
                //  +【2NCS_AI266】*512000
                double a7 = 0;
                //  +【2NCS_AI274】*252000
                double a8 = 0;
              


                /* opc方案暂时不用  
                  //计算公式
                  //  【NCS_34_AI67】*684000   //opc中TagName需调整
                  double a1 = Convert.ToDouble(getOPC("NCS_34_AI67"));
                  //  +【NCS_34_AI71】*684000
                  double a2 = Convert.ToDouble(getOPC("NCS_34_AI71"));
                  //  +【2NCS_AI190】*512000
                  double a3 = Convert.ToDouble(getOPC("2NCS_AI190"));
                  //  +【2NCS_AI198】*252000
                  double a4 = Convert.ToDouble(getOPC("2NCS_AI198"));
                  //  +【2NCS_AI230】*512000
                  double a5 = Convert.ToDouble(getOPC("2NCS_AI230"));
                  //  +【2NCS_AI238】*252000
                  double a6 = Convert.ToDouble(getOPC("2NCS_AI238"));
                  //  +【2NCS_AI266】*512000
                  double a7 = Convert.ToDouble(getOPC("2NCS_AI266"));
                  //  +【2NCS_AI274】*252000
                  double a8 = Convert.ToDouble(getOPC("2NCS_AI274"));
                  opc.DisConnected();
                  */
                double result = a1 * 684000 + a2 * 684000 + a3 * 512000 + a4 * 252000 + a5 * 512000 + a6 * 252000 + a7 * 512000 + a8 * 252000;
                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllPowerGeneration：" + ex.ToString());
                return null;
            }

          }
      
        
        
        /// <summary>
        /// 上网电量（全厂）
        /// </summary>
        /// <returns></returns>
        public   string GetdoubleerNetPower()
        {
            try
            {
                string tags = string.Format("NCS_34_AI34,NCS_34_AI36,2NCS_AI210,2NCS_AI218,2NCS_AI250,2NCS_AI258,2NCS_AI286,2NCS_AI294");
                List<SisData> sisList = WebApiHelper.GetEntity<List<SisData>>("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                double p1 = 0;
                double p2 = 0;
                double p3 = 0;
                double p4 = 0;
                double p5 = 0;
                double p6 = 0;
                double p7 = 0;
                double p8 = 0;
                if (sisList != null && sisList.Count > 0)
                {
                    foreach (SisData sisData in sisList)
                    {
                        //NCS_34_AI34,NCS_34_AI36,2NCS_AI210,2NCS_AI218,2NCS_AI250,2NCS_AI258,2NCS_AI286,2NCS_AI294
                        switch (sisData.Name)
                        {
                            case "NCS_34_AI34":
                                p1 = Convert.ToDouble(sisData.Value);
                                break;
                            case "NCS_34_AI36":
                                p2 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI210":
                                p3 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI218":
                                p4 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI250":
                                p5 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI258":
                                p6 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI286":
                                p7 = Convert.ToDouble(sisData.Value);
                                break;
                            case "2NCS_AI294":
                                p8 = Convert.ToDouble(sisData.Value);
                                break;
                        }
                    }
                }
                /*
                //  【NCS_34_AI34】*3300000
                double p1 = Convert.ToDouble(getOPC("NCS_34_AI34"));
                // +【NCS_34_AI36】*3300000
                double p2 = Convert.ToDouble(getOPC("NCS_34_AI36"));
                // +【2NCS_AI210】*3300000
                double p3 = Convert.ToDouble(getOPC("2NCS_AI210"));
                // +【2NCS_AI218】*1320000
                double p4 = Convert.ToDouble(getOPC("2NCS_AI218"));
                // +【2NCS_AI250】*3300000
                double p5 = Convert.ToDouble(getOPC("2NCS_AI250"));
                // +【2NCS_AI258】*1320000
                double p6 = Convert.ToDouble(getOPC("2NCS_AI258"));
                // +【2NCS_AI286】*3300000
                double p7 = Convert.ToDouble(getOPC("2NCS_AI286"));
                // +【2NCS_AI294】*1320000
                double p8 = Convert.ToDouble(getOPC("2NCS_AI294"));
                */
                double result = p1 * 3300000 + p2 * 3300000 + p3 * 3300000 + p4 * 1320000 + p5 * 3300000 + p6 * 1320000 + p7 * 3300000 + p8 * 1320000;
                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetdoubleerNetPower：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 用网电量（全厂）
        /// </summary>
        /// <returns></returns>
        public   string GetUsedoublePower()
        {
            try
            {
                //  【NCS_34_AI41】*3300000
                double p1 = Convert.ToDouble(getOPC("NCS_34_AI41"));
                // +【NCS_34_AI43】*3300000
                double p2 = Convert.ToDouble(getOPC("NCS_34_AI43"));
                // +【2NCS_AI211】*3300000
                double p3 = Convert.ToDouble(getOPC("2NCS_AI211"));
                // +【2NCS_AI219】*1320000
                double p4 = Convert.ToDouble(getOPC("2NCS_AI219"));
                // +【2NCS_AI251】*3300000
                double p5 = Convert.ToDouble(getOPC("2NCS_AI251"));
                // +【2NCS_AI259】*1320000
                double p6 = Convert.ToDouble(getOPC("2NCS_AI259"));
                // +【2NCS_AI287】*3300000
                double p7 = Convert.ToDouble(getOPC("2NCS_AI287"));
                // +【2NCS_AI295】*1320000
                double p8 = Convert.ToDouble(getOPC("2NCS_AI295"));
                double result = p1 * 3300000 + p2 * 3300000 + p3 * 3300000 + p4 * 1320000 + p5 * 3300000 + p6 * 1320000 + p7 * 3300000 + p8 * 1320000;
                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetUsedoublePower："+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 机组供蒸汽热量1（1.3MPa)（单位：吨）
        /// </summary>
        /// <returns></returns>
        public   string GetUnitSteamSupplyHeatT()
        {
            try
            {
                //  【L0LBD30CF101C_RT-TOTAL】
                double h1 = Convert.ToDouble(getOPC("L0LBD30CF101C_RT-TOTAL"));
                // +【M0LBD30CF101C_RT-TOTAL】
                double h2 = Convert.ToDouble(getOPC("M0LBD30CF101C_RT-TOTAL"));
                // +【N0LBD30CF101C_RT-TOTAL】
                double h3 = Convert.ToDouble(getOPC("N0LBD30CF101C_RT-TOTAL"));

                double And = h1 + h2 + h3;
                return And.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetUnitSteamSupplyHeatT:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 机组供蒸汽热量1（1.3MPa) （单位：GJ）
        /// </summary>
        /// <returns></returns>
        public   string GetUnitSteamSupplyHeatGJ()
        {
            try
            {
                // 【L0LBD30CF101C_RT-TOTAL】*2.81
                double h1 = Convert.ToDouble(getOPC("L0LBD30CF101C_RT-TOTAL"));
                // +【M0LBD30CF101C_RT-TOTAL】*2.81
                double h2 = Convert.ToDouble(getOPC("M0LBD30CF101C_RT-TOTAL"));
                // +【N0LBD30CF101C_RT-TOTAL】*2.81
                double h3 = Convert.ToDouble(getOPC("N0LBD30CF101C_RT-TOTAL"));
                double And = h1 * 2.81 + h2 * 2.81 + h3 * 2.81;
                return And.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetUnitSteamSupplyHeatGJ:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 机组供蒸汽热量1压力
        /// </summary>
        /// <returns></returns>
        public   string GetUnitSteamSupplyHeatP()
        {
            try
            {
                // 【L0LBD30CP104_T】
                double p1 = Convert.ToDouble(getOPC("L0LBD30CP104_T"));
                // +【M0LBD30CP104_RT】
                double p2 = Convert.ToDouble(getOPC("M0LBD30CP104_RT"));
                // +【N0LBD30CP104_R】
                double p3 = Convert.ToDouble(getOPC("N0LBD30CP104_R"));
                double and = p1 + p2 + p3;
                return and.ToString();

            }
            catch (Exception ex)
            {
                Log.Error("GetUnitSteamSupplyHeatP:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 机组供蒸汽热量1温度
        /// </summary>
        /// <returns></returns>
        public   string GetUnitSteamSupplyHeatC()
        {
            try
            {
                // 【L0LBD30CT2_T】
                double c1 = Convert.ToDouble(getOPC("L0LBD30CT2_T"));
                //+【M0LBD30CT2_RT】
                double c2 = Convert.ToDouble(getOPC("M0LBD30CT2_RT"));
                //+【N0LBD30CT2_RT】
                double c3 = Convert.ToDouble(getOPC("N0LBD30CT2_RT"));
                double and = c1 + c2 + c3;
                return and.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("GetUnitSteamSupplyHeatC:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 负荷率
        /// </summary>
        /// <returns></returns>
        public   string GetLoadRate()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 发电耗天然气量
        /// </summary>
        /// <returns></returns>
        public   string GetEGNeedNaturalGas()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 供热耗天然气量
        /// </summary>
        /// <returns></returns>
        public   string GetHeatSupplyNeedNaturalGas()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 机组耗标准煤量
        /// </summary>
        /// <returns></returns>
        public   string GetCrewNeedStandardCoal()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 发电热耗
        /// </summary>
        /// <returns></returns>
        public   string GetElectricHeatConsumption()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 发电气耗
        /// </summary>
        /// <returns></returns>
        public   string GetElectricGasConsumption()
        {
            //发电耗天然气量/发电量
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 供电气耗
        /// </summary>
        /// <returns></returns>
        public   string GetPowerSupplyGasConsumption()
        {
            //发电耗天然气量 /（发电量 - 生产厂用电量）
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("GetLoadRate:" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 全场供热流量
        /// </summary>
        /// <returns></returns>
        public string GetAllGRLL()
        {
            //S0LBD30CF101CT
            string tags = "S0LBD30CF101CT";
            List<SisData> sisdata = getList(tags);
            string result = "";
            if (sisdata != null && sisdata.Count == 1)
            {
                result = sisdata[0].Value.ToString();
            }
            return result;
        }

        /// <summary>
        /// 全场天然气流量
        /// </summary>
        /// <returns></returns>
        public string GetAllTRQLL()
        {
            // CDM2CVOLFR
            // 50EKU11CF102
            // 70EKU13CF102
            // 90EKU15CF102
            // 累加 / 10000
            string result = "";
            try
            {
                string tags = "CDM2CVOLFR,50EKU11CF102,70EKU13CF102,90EKU15CF102";
                double values = 0;
                List<SisData> sisdataList = getList(tags);
                if (sisdataList != null && sisdataList.Count > 0)
                {
                    foreach (SisData sis in sisdataList)
                    {
                        values += Convert.ToDouble(sis.Value.ToString());
                    }

                    double resultD = values / 10000;
                    result = resultD.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error("EnergyService.GetAllTRQLL:"+ex.ToString());
            }
            return result;
        }
        /// <summary>
        /// NOx排放量列表
        /// </summary>
        /// <returns></returns>
        public List<SisData> GetNOPFL()
        {
          //            NOx排放量
          //单位:mg / m3
          //机组名	#3机组			  #4机组			  #5/6机组				   #7/8机组			      #9/10机组
          //排量    24.29             0                   16.25                    0                      0
          //测点    30EUE01CQ001B     40EUE01CQ001B       50HNE70GH001NOXJZ        8_70HNE70GH001NOXJZ    A_90HNE70GH001NOXJZ
            try
            {
                string tags = "30EUE01CQ001B,40EUE01CQ001B,50HNE70GH001NOXJZ,8_70HNE70GH001NOXJZ,A_90HNE70GH001NOXJZ";
                List<SisData> sisList = getList(tags);
                if (sisList != null)
                {
                    foreach (SisData sis in sisList)
                    {
                        switch (sis.Name)
                        {
                            case "30EUE01CQ001B":
                                sis.Desc = "#3机组";
                                break;
                            case "40EUE01CQ001B":
                                sis.Desc = "#4机组";
                                break;
                            case "50HNE70GH001NOXJZ":
                                sis.Desc = "#5/6机组";
                                break;
                            case "8_70HNE70GH001NOXJZ":
                                sis.Desc = "#7/8机组";
                                break;
                            case "A_90HNE70GH001NOXJZ":
                                sis.Desc = "#9/10机组";
                                break;
                        }
                    }
                }

                return sisList;
            }
            catch (Exception ex)
            {
                Log.Error("EnergyService.GetNOPFL:"+ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 各机组负荷列表
        /// </summary>
        /// <returns></returns>
        public List<SisData> GetEveryCrewFH()
        {
            //负荷
            //#3机组 M_D_AI01
            //#4机组 40M_D_AI01
            //#5机组 GTLOAD
            //#6机组 60LEDSTLOAD
            //#7机组 8_GTLOAD
            //#8机组 8_80LEDSTLOAD
            //#9机组 A_GTLOAD
            //#10    A_A0LEDSTLOAD
            try
            {
                string tags = "M_D_AI01,40M_D_AI01,GTLOAD,60LEDSTLOAD,8_GTLOAD,8_80LEDSTLOAD,A_GTLOAD,A_A0LEDSTLOAD";
                List<SisData> sisList = getList(tags);
                if (sisList != null)
                {
                    foreach (SisData sis in sisList)
                    {
                        switch (sis.Name)
                        {
                            case "M_D_AI01":
                                sis.Desc = "#3机组";
                                break;
                            case "40M_D_AI01":
                                sis.Desc = "#4机组";
                                break;
                            case "GTLOAD":
                                sis.Desc = "#5机组";
                                break;
                            case "60LEDSTLOAD":
                                sis.Desc = "#6机组";
                                break;
                            case "8_GTLOAD":
                                sis.Desc = "#7机组";
                                break;
                            case "8_80LEDSTLOAD":
                                sis.Desc = "#8机组";
                                break;
                            case "A_GTLOAD":
                                sis.Desc = "#9机组";
                                break;
                            case "A_A0LEDSTLOAD":
                                sis.Desc = "#10机组";
                                break;
                        }
                    }
                }
                return sisList;
            }
            catch (Exception ex)
            {
                Log.Error("EnergyService.GetEveryCrewFH:"+ex.ToString());
                return null;
            }
        }


        public List<SisData> GetSisDataList(string[] tags)
        {
            List<SisData> list = new List<SisData>();
            try
            {
                string tagsString = "";
                if (tags != null && tags.Length > 0)
                {
                    foreach (string tag in tags)
                    {
                        tagsString += tag + ",";
                    }
                    tagsString = tagsString.Substring(0,tagsString.Length-1);
                }
                if (tagsString != "")
                {
                    list = getList(tagsString);
                }

            }
            catch (Exception ex)
            {
                Log.Error("EnergyService.GetSisDataList:"+ex.ToString());
            }
            return list;
        }
       
    }
}
