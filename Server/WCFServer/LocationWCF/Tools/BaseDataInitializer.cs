using CommunicationClass.SihuiThermalPowerPlant;
using DAL;
using DbModel.BaseData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public class BaseDataInitializer
    {
        public List<device> GetDevices()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\BaseData\\devices.json";
            string json = File.ReadAllText(path);
            BaseTran<device> obj = JsonConvert.DeserializeObject<BaseTran<device>>(json);
            return obj.data;
        }

        public List<device> InitDevices()
        {
            var devices = GetDevices();
            BaseDataDb db = new BaseDataDb();
            db.SetTable(db.devices, devices);
            return devices;
        }
    }
}
