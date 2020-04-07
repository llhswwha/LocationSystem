using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Settings;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.Tools;
using TModel.Tools;
using Newtonsoft.Json;
using Base.Common.Tools;
using System.Xml.Serialization;

namespace DbModel.LocationHistory.Data
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    [DataContract]
    public class Position : IId, IPosInfo
    {
        private DateTime _dateTime;

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        [Display(Name = "主键Id")]
        [XmlAttribute]
        public int Id { get; set; }

        [Index]
        [DataMember]
        [Display(Name = "标签卡Id")]
        //[XmlAttribute]
        public int? CardId { get; set; }

        [DataMember]
        [Display(Name = "标签角色Id")]
        //[XmlAttribute]
        public int? RoleId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [Index]
        [DataMember]
        [Display(Name = "人员ID")]
        //[XmlAttribute]
        public int? PersonnelID { get; set; }

        [NotMapped]
        [XmlIgnore]
        public string PersonnelName { get; set; }

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡编号")]
        [MaxLength(16)]
        [Required]
        [XmlAttribute]
        public string Code { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        [Display(Name = "X")]
        [XmlAttribute]
        public float X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        [Display(Name = "Y")]
        [XmlAttribute]
        public float Y { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [DataMember]
        [Display(Name = "Z")]
        [XmlAttribute]
        public float Z { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        [Display(Name = "时间")]
        [XmlAttribute]
        public DateTime DateTime { get; set; }
        //{
        //    get { return _dateTime; }
        //    set
        //    {
        //        _dateTime = value;
        //        DateTimeStamp = TimeConvert.DateTimeToTimeStamp(value);
        //    }
        //}

        ///// <summary>
        ///// 时间戳（毫秒）
        ///// </summary>
        [Key, Column(Order = 2)]
        [Index]
        [DataMember]
        [Display(Name = "时间戳")]
        [XmlAttribute]
        public long DateTimeStamp { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
        [Display(Name = "电量")]
        [XmlAttribute]
        public int Power { get; set; }


        public Area SetArea(params Area[] areas)
        {
            //if (ToString() == "0918")
            //{

            //}
            Area areaNode = null;
            if (areas == null)
            {
                AllAreaId = null;
                AreaPath = "";
                AreaState = 1;
                return areaNode;
            }
            Areas = areas;
            //AreaId = areas.Id;
            //AreaPath = area.Name;
            //AreaState = area.IsOnLocationArea ? 0 : 1;
            AreaState = 1;
            AllAreaId = "";
            AreaPath = "";
            foreach (var area in Areas)//同时处于一个告警区域和一个定位区域时 人员区域怎么判断？ 同时处于两个区域时 人员区域怎么判断
            {
                if (area.IsOnLocationArea)
                {
                    AreaState = 0;
                }
                if (area.Type != Tools.AreaTypes.范围)
                {
                    AreaId = area.Id;
                    areaNode = area;
                }
                AllAreaId += area.Id + ";";
                AreaPath = area.Name + ";";
            }
            return areaNode;
        }

        /// <summary>
        /// 电量状态,0表示正常，1表示弱电
        /// </summary>
        [Display(Name = "电量状态")]
        [XmlAttribute]
        public int PowerState { get; set; }

        /// <summary>
        /// 区域状态，0:在定位区域，1:不在定位区域
        /// </summary>
        [Display(Name = "区域状态")]
        [XmlAttribute]
        public int AreaState { get; set; }

        /// <summary>
        /// 运动状态，0:运动，1:待机状态，2:静止状态
        /// </summary>
        [Display(Name = "运动状态")]
        [XmlAttribute]
        public int MoveState { get; set; }

        /// <summary>
        /// 
        /// 
        /// 序号（新的卡才有的）
        /// </summary>
        [DataMember]
        [Display(Name = "序号")]
        [XmlAttribute]
        public int Number { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// 补充：卡大约20秒中不动后，会发0:0:0:0:1，然后再不动大约10秒后，不发位置信息
        /// </summary>
        [DataMember]
        [Display(Name = "信息")]
        [MaxLength(16)]
        [XmlAttribute]
        public string Flag { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        [Display(Name = "参与计算的基站编号")]
        [NotMapped]
        [XmlIgnore]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        [Display(Name = "参与计算的基站编号")]
        [MaxLength(64)]
        [XmlAttribute]
        public string ArchorsText { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层编号Id
        /// </summary>
        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层编号Id")]
        [XmlAttribute]
        public string AllAreaId { get; set; }

        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层编号Id")]
        //[XmlAttribute]
        public int? AreaId { get; set; }

        [NotMapped]
        [XmlIgnore]
        public Area Area { get; set; }

        public bool IsAreaNull()
        {
            //return (AreaId == null || AreaId == 0);
            return (AllAreaId == null || AllAreaId == "");
        }

        public bool IsInArea(int areaId)
        {
            if (Areas != null)
            {
                foreach (var item in Areas)
                {
                    if (item.Id == areaId)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(AllAreaId)) return false;
                string[] parts = AllAreaId.Split(';');
                foreach (var item in parts)
                {
                    if (item == areaId + "")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsInArea(List<int> areaIds)
        {
            foreach (var areaId in areaIds)
            {
                if (Areas != null)
                {
                    foreach (var item in Areas)
                    {
                        if (item.Id == areaId)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    string[] parts = AllAreaId.Split(';');
                    foreach (var item in parts)
                    {
                        if (item == areaId + "")
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 基站所在的区域
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public Area[] Areas { get; set; }


        [DataMember]
        [MaxLength(64)]
        [XmlAttribute]
        public string AreaPath { get; set; }

        /// <summary>
        /// 模拟数据
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public bool IsSimulate { get; set; }

        /// <summary>
        /// 事件类型，默认为0，当位置信息中有SOS时，为1
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public int EventType { get; set; }


        public Position()
        {
            //Archors = new List<string>();
        }

        public void SetTime()
        {
            DateTime now = DateTime.Now;
            DateTimeStamp = TimeConvert.ToStamp(now);
            DateTime now2 = TimeConvert.ToDateTime(DateTimeStamp);
        }

        public void AddArchor(string archor)
        {
            if (Archors == null)
            {
                Archors = new List<string>();
            }
            if (string.IsNullOrEmpty(archor)) return;
            Archors.Add(archor);
        }

        public string _info;

        /// <summary>
        /// 解析位置信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="offsetX">偏移量X 和定位引擎约定好具体偏移数值</param>
        /// <param name="offsetY">偏移量Y 和定位引擎约定好具体偏移数值</param>
        /// <returns></returns>
        public bool Parse(string info, float offsetX, float offsetY)
        {
            if (info == null) return false;
            if (info.Length <= 1)
            {
                LogEvent.Info("RealPos", "心跳包:" + info);
                return false;
            }
            else
            {
                LogEvent.Info("RealPos", "获取定位数据:" + info);
            }

     
            bool r = false;
            info = info.Trim();
            if (info.Contains("{"))
            {
                r= ParseJson(info, offsetX, offsetY,true);
            }
            else
            {
                r= ParseText(info, offsetX, offsetY);
            }

            if (r == true)
            {
                TimeSpan t = DateTime.Now - this.DateTime;
                if (t.TotalSeconds > 100000)//如果实时数据收到的时候和服务端所在电脑的时间有时间差，说明两台电脑自己的时间差别很大了，需要调整。
                {
                    //this.DateTime = DateTime.Now;
                    //this.DateTimeStamp = this.DateTime.ToStamp();
                    LogEvent.Info("RealPos","定位数据解析异常:" + info + ",时间相差:" + t.TotalSeconds);
                }
                else if (t.TotalSeconds > 30)//如果实时数据收到的时候和服务端所在电脑的时间有时间差，说明两台电脑自己的时间差别很大了，需要调整。
                {
                    //this.DateTime = DateTime.Now;
                    //this.DateTimeStamp = this.DateTime.ToStamp();
                    LogEvent.Info("RealPos", "定位引擎电脑和服务端电脑时间相差很大:" + t.TotalSeconds + "s");
                }
            }
            else
            {
                
            }
            return r;
        }

        public string GetText(float offsetX, float offsetY,int timeMode)
        {
            DateTime now = DateTime.Now;
            long t = 0;
            if (timeMode == 0)//当前时间作为发送时间
            {
                
                t = TimeConvert.ToStamp(now);
            }
            else if (timeMode == 1)//当前日期作为发送时间
            {
                DateTime t1 = TimeConvert.ToDateTime(DateTimeStamp);
                DateTime tNew = new DateTime(now.Year, now.Month, now.Day, t1.Hour, t1.Minute,
                    t1.Second, t1.Millisecond);
                t = TimeConvert.ToStamp(tNew);
            }
            
            double x = (X - offsetX) / AppSetting.PositionPower;
            double y = (Z - offsetY) / AppSetting.PositionPower;
            double z = Y / AppSetting.PositionPower;
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Code, x, y, z,
                t, Power, Number, Flag, ArchorsText);
        }

        public string GetJson()
        {
            return "";
        }

        private bool ParseText(string info, float offsetX, float offsetY)
        {
            try
            {
                _info = info;
                string[] parts = info.Split(new[] { ',' });
                int length = parts.Length;
                if (length <= 1) return false;//心跳包回拨
                Code = parts[0];
                //if (Code.StartsWith("1"))
                //{
                //    LogEvent.Info("RealPos,", "Code.StartsWith(1)：" + info);
                //}
                if (parts[1] == "-1.#IND0")
                {
                    parts[1] = "-1.0";
                }
                if (parts[2] == "-1.#IND0")
                {
                    parts[2] = "-1.0";
                }
                float x = parts[1].ToFloat() * AppSetting.PositionPower;
                float y = parts[2].ToFloat() * AppSetting.PositionPower;
                if (x < offsetX)
                {
                    X = x + offsetX; //平面位置
                    Z = y + offsetY; //平面位置
                }
                else //模拟数据是可以没有偏移量的 看模拟程序的版本
                {
                    X = x;
                    Z = y;
                }

                Y = parts[3].ToFloat() * AppSetting.PositionPower;//高度位置，为了和Unity坐标信息一致，Y为高度轴
                DateTimeStamp = parts[4].ToLong();
                DateTime = TimeConvert.ToDateTime(DateTimeStamp);
                //TimeSpan time1 = DateTime.Now - DateTime;
                //long DateTimeStamp2 = TimeConvert.DateTimeToTimeStamp(DateTime);
                if (length > 5)
                {
                    Power = parts[5].ToInt();
                    if (Power >= AppSetting.LowPowerFlag)
                    {
                        PowerState = 0;
                    }
                    else
                    {
                        PowerState = 1;
                    }
                }

                if (length > 6)
                    Number = parts[6].ToInt();
                if (length > 7)
                    Flag = parts[7];
                if (length > 8)
                {
                    ArchorsText = parts[8];
                    Archors = ArchorsText.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (Archors.Count > 1)
                    {
                        Console.Write("Archors.Count > 1");
                    }
                    IsSimulate = ArchorsText == "@0000" || string.IsNullOrEmpty(ArchorsText);
                }

                //if (Code == "092D" && (ArchorsText == null || Archors == null))
                //{
                //    int i = 0;
                //}

                if (length > 8)
                {
                }
                //else if(length == 8)
                //{

                //}
                
                else
                {
                    LogEvent.Info("位置信息数据可能被截断！！！！:" + info);
                    //return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogEvent.Info("定位数据解析失败:"+ info +"\n"+ ex.ToString());
                return false;
            }
        }

        private bool ParseJson(string json, float offsetX, float offsetY,bool parseErrorJson)
        {
            try
            {
                return ParseJsonInner(json, offsetX,offsetY);
            }
            catch (Exception e)
            {
                string msg = e.Message;

                //有些json数据能够简单的处理一下
                if(msg.Contains("Additional text encountered after finished reading JSON content"))
                {
                    //"}]}]}
                    if (json.EndsWith("\"}]}}"))
                    {
                        json = json.Substring(0, json.Length - 1);
                        LogEvent.Info("ParseJson", "调整json数据: - }");
                        return ParseJson(json, offsetX, offsetY,false);
                    }
                    else if (json.EndsWith("\"}]}]}"))
                    {
                        json = json.Substring(0, json.Length - 2);
                        LogEvent.Info("ParseJson", "调整json数据: - }");
                        return ParseJson(json, offsetX, offsetY, false);
                    }
                    else
                    {
                        LogEvent.Info("ParseJsonError", "其他情况1:" + msg+"\n"+json); 
                    }
                }
                else if (msg.Contains("Unexpected end when deserializing object"))
                {
                    if (json.EndsWith("\"}]"))
                    {
                        json += "}";
                        LogEvent.Info("ParseJson", "调整json数据: + }");
                        return ParseJson(json, offsetX, offsetY,false);
                    }
                    else
                    {
                        LogEvent.Info("ParseJsonError", "其他情况2:" + msg + "\n" + json); ;
                    }
                }
                else if(msg.Contains("Unexpected end when deserializing array"))
                {
                    if (json.EndsWith("\"}"))
                    {
                        json += "]}";
                        LogEvent.Info("ParseJson", "调整json数据: + ]}");
                        return ParseJson(json, offsetX, offsetY, false);
                    }
                    else
                    {
                        LogEvent.Info("ParseJsonError", "其他情况3:" + msg + "\n" + json);
                    }
                }
                else
                {
                    LogEvent.Info("ParseJsonError", string.Format("Json:{0}\nError->DbModel.Position.Exception:{1}", json, e.ToString()));
                }
                
                return false;
            }          
        }

        private bool ParseJsonInner(string json, float offsetX, float offsetY)
        {
            PositionJson p = JsonConvert.DeserializeObject<PositionJson>(json);
            Code = p.tag_id;
            X = p.x.ToFloat() * AppSetting.PositionPower;
            Y = p.z.ToFloat() * AppSetting.PositionPower;
            Z = p.y.ToFloat() * AppSetting.PositionPower;

            //Y = p.y.ToFloat() * 10;
            //Z = p.z.ToFloat() * 10;
            X += offsetX;
            Z += offsetY;

            DateTimeStamp = p.timestamp.ToLong();
            DateTime = DateTimeStamp.ToDateTime();
            Number = p.sn.ToInt();
            Power = (int)(p.bettery.ToFloat() * 100);
            EventType = 0;
            if (p.events != null && p.events.Count() > 0)
            {
                for (int i = 0; i < p.events.Count(); i++)
                {
                    string strEventType = p.events[i].event_type;
                    if (strEventType == "SOS")
                    {
                        EventType = 1;
                    }
                }
            }
            return true;
        }

        public string GetText()
        {
            string archors = "";
            if (Archors != null)
            {
                archors = string.Join(",", Archors.ToArray());
            }
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Code, X, Y, Z, DateTimeStamp, Power, Number,
                Flag, archors);
        }

        public override string ToString()
        {
            //return Code;
            return Code + "," + DateTime;
        }

        public Position Clone()
        {
            Position copy = this.CloneObjectByBinary();
            return copy;
        }

        public void SetProperty(LocationCardPosition pos)
        {
            Code = pos.Id;
            X = pos.X;
            Y = pos.Z;
            Z = pos.Y;
            Power = pos.Power;
            Number = pos.Number;
            Flag = pos.Flag;
            Archors = pos.Archors;
        }
    }




}
