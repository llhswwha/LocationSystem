using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Tools;
using IModel.Enums;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.BaseData
{
    public static class BaseDataHelper
    {
        public static void SetUser(Personnel person, user item)
        {
            item.id = person.Abutment_Id ?? 0;
            item.name = person.Name;
            item.gender = (int)person.Sex;
            item.email = person.Email;
            item.phone = person.Phone;
            item.mobile = person.Mobile;
            item.enabled = person.Enabled;
            if (person.Parent != null)
            { item.dept_name = person.Parent.Name; }
            else
            {
                item.dept_name = person.ParentId + "";
            }
        }

        public static void SetPersonnel(Personnel person, user item)
        {
            Sexs nSex = Sexs.未知;
            if (item.gender != null)
            {
                nSex = (Sexs)item.gender;
            }

            person.Abutment_Id = item.id;
            person.Name = item.name;
            person.Sex = nSex;
            person.Email = item.email;
            person.Phone = item.phone;
            person.Mobile = item.mobile;
            person.Enabled = item.enabled;
        }

        public static void SetDeivInfo(DevInfo devinfo, device item)
        {
            devinfo.Abutment_Id = item.id;
            devinfo.Code = item.code;
            devinfo.Abutment_Type = (Abutment_DevTypes)item.type;
            devinfo.Status = (Abutment_Status)item.state;
            devinfo.RunStatus = (Abutment_RunStatus)item.running_state;
            devinfo.Placed = item.placed;
            devinfo.Abutment_DevID = item.raw_id;
            devinfo.IP = item.ip;
            devinfo.Manufactor = "霍尼韦尔";

            devinfo.ModifyTime = DateTime.Now;
            devinfo.ModifyTimeStamp = TimeConvert.ToStamp(devinfo.ModifyTime);

            //devinfo.ParentId = area.Id;
            devinfo.KKS = item.kks;
            devinfo.Name = item.name;
        }

        private static int GetDeviceType(DevInfo devinfo)
        {
            var type = TypeCodeHelper.GetTypeName(devinfo.Local_TypeCode + "", devinfo.ModelName);
            if (type == "基站")
            {
                return (int)Abutment_DevTypes.定位基站;
            }
            else if (type == "摄像头")
            {
                return (int)Abutment_DevTypes.摄像头;
            }
            else if (type == "生产设备")
            {
                return (int)Abutment_DevTypes.生产设备;
            }
            else if (type == "门禁")
            {
                return (int)Abutment_DevTypes.门禁;
            }
            else if (type == "警报设备")
            {
                return (int)Abutment_DevTypes.消防设备;
            }
            else if (type == "其他设备")
            {
                return (int)Abutment_DevTypes.无;
            }
            return 0;
        }

        public static void SetDevice(device item, DevInfo devinfo)
        {
            item.id = devinfo.Abutment_Id ?? 0;
            item.code = devinfo.Code;
            item.type = (int)devinfo.Abutment_Type;
            if (item.type == 0)
            {
                item.type = GetDeviceType(devinfo);
            }
            item.state = (int)devinfo.Status;
            item.running_state = (int)devinfo.RunStatus;
            item.placed = devinfo.Placed;
            item.raw_id = devinfo.Abutment_DevID;
            item.ip = devinfo.IP;
            //devinfo.Manufactor = "霍尼韦尔";

            //devinfo.ModifyTime = DateTime.Now;
            //devinfo.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(devinfo.ModifyTime);

            //devinfo.ParentId = area.Id;
            item.kks = devinfo.KKS;
            item.name = devinfo.Name;
            item.pid = devinfo.ParentId ?? 0;

            if (devinfo.DevDetail is Dev_CameraInfo)
            {
                Dev_CameraInfo camera = devinfo.DevDetail as Dev_CameraInfo;
                item.uri = camera.RtspUrl;

                if (string.IsNullOrEmpty(item.uri))
                {
                    item.uri = "rtsp://admin:admin12345@192.168.1.56/h264/ch1/main/av_stream";
                }

                item.ip = camera.Ip;
            }
        }
    }
}
