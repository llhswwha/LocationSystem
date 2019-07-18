using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WebApiCommunication.ExtremeVision
{
    /// <summary>
    /// 摄像头行为分析告警
    /// </summary>
    [DataContract]
    public class CameraAlarmInfo:IComparable<CameraAlarmInfo>
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonIgnore]
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// 相同告警合并，但是记录一下一系列告警的开始的数据，最终给的是结束的数据
        /// </summary>
        [JsonIgnore]
        [DataMember]
        public CameraAlarmInfo startInfo { get; set; }


        [JsonIgnore]
        [DataMember]
        public DateTime time { get; set; }

        /// <summary>
        /// 算法ID，由极视角定义的算法ID，只读
        /// </summary>
        [JsonProperty("aid")]//因为三维客户端那边自动生成的类里面没有JsonProperty特性，属性名还是和json中的一样好了
        [DataMember]
        public int aid { get; set; }

        /// <summary>
        /// 摄像头ID
        /// 存在两种情况：
        /// 1. 由算法配置工具自动生成，不建议修改；
        /// 2. 算法内自定义的摄像头ID，采用string格式，可以自定义，建议使用数字
        /// </summary>
        [JsonProperty("cid")]
        [DataMember]
        [MaxLength(256)]
        public string cid { get; set; }

        /// <summary>
        /// 摄像头对应拉流地址/rtsp
        /// </summary>
        [JsonProperty("cid_url")]
        [DataMember]
        [MaxLength(512)]
        public string cid_url { get; set; }

        /// <summary>
        /// 算法服务器时间戳，unix标准时间戳格式
        /// </summary>
        [JsonProperty("time_stamp")]
        [DataMember]
        public long time_stamp { get; set; }


        /// <summary>
        /// 状态值，0/无报警，1/有报警
        /// </summary>
        [JsonProperty("status")]
        [DataMember]
        public int status { get; set; }

        /// <summary>
        /// 报警图片命名
        /// 格式为“时间（精确到秒）_us（微秒）_cid（摄像头ID）_fix（输入或输出）.jpg”，例：20180719121005_266236_3_out.jpg
        /// </summary>
        [JsonProperty("pic_name")]
        [DataMember]
        [MaxLength(256)]
        public string pic_name { get; set; }

        /// <summary>
        /// 报警结果图片，base64格式编码
        /// </summary>
        [JsonProperty("pic_data")]
        [DataMember]
        //[NotMapped]
        public string pic_data { get; set; }

        /// <summary>
        /// 算法返回的数据
        /// </summary>
        [JsonProperty("data")]
        [NotMapped]
        public object data { get; set; }

        [DataMember]
        public FlameData FlameData { get; set; }

        [DataMember]
        public HeadData HeadData { get; set; }

        [DataMember]
        [NotMapped]
        public string Error { get; set; }

        public override string ToString()
        {
            return string.Format("Id:{0},Cid:{1} time:{2}", aid, cid,time);
        }

        public FlameData GetFlameData()
        {
            try
            {
                JArray arr = data as JArray;
                if (arr != null)
                {
                    var result = arr.ToObject<FlameData[]>();
                    return result[0];
                }
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                return null;
            }
            return null;
        }

        private bool IsHeadInfo()
        {
            if (data is JArray)//9510安全帽识别算法回调参数说明.md 协议
            {
                JArray arr = data as JArray;
                foreach (JToken child in arr.First.Children())
                {
                    var property = child as JProperty;
                    if (property == null) continue;
                    //var str = property + "";
                    var name = (property).Name;
                    if (name == "headInfo")
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (data is JObject)//实际收到的
            {
                JObject obj = data as JObject;

                foreach (JToken child in obj.Children())
                {
                    var property = child as JProperty;
                    if (property == null) continue;
                    //var str = property + "";
                    var name = (property).Name;
                    if (name == "headInfo")
                    {
                        return true;
                    }
                }

                return false;

            }

            return false;
        }

        public HeadData GetHeadData()
        {
            try
            {
               
                if (data is JArray)
                {
                    JArray obj = data as JArray;
                    var result = obj.ToObject<HeadData[]>();
                    return result[0];
                }
                else if(data is JObject)
                {
                    JObject obj = data as JObject;
                    var result = obj.ToObject<HeadData>();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                return null;
            }
            return null;
        }

        public void ParseData()
        {
            if (IsHeadInfo())//判断是不是头盔告警
            {
                HeadData = GetHeadData();//解析头盔数据
            }
            else
            {
                FlameData = GetFlameData();//解析火焰数据
            }
        }

        public int CompareTo(CameraAlarmInfo other)
        {
            return GetCompareId().CompareTo(other.GetCompareId());
        }

        public string GetCompareId()
        {
            return ("" + cid + time_stamp);
        }
    }
}
