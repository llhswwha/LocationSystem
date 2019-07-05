using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.Alarm
{
    public class CameraAlarmJson:IId
    {
        public int Id { get; set; }
        public byte[] Json { get; set; }
    }
}
