using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Tools.InitInfos
{
    public class HKWebApiCameraList
    {
        public string code;
        public string msg;
        public HKWebApiCameraData data;
    }

    public class HKWebApiCameraData
    {
        public int total;
        public int pageNum;
        public int pageSize;
        public List<HKWebApiCameraDevInfo> list;
    }
    public class HKWebApiCameraDevInfo
    {
        public string cameraIndexCode;
        public string cameraName;
    }
}
