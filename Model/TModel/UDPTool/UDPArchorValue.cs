using ArchorUDPTool.ArchorManagers;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Net;

namespace ArchorUDPTool.Models
{
    public class UDPArchorValue
    {
        private string _value;
        public string Id { get; set; }

        public string Ip { get; set; }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                UpdateTime=DateTime.Now;
            }
        }

        public DateTime UpdateTime { get; set; }
    }

    public class UDPArchorValueList : List<UDPArchorValue>
    {
        public UDPArchorValueList()
        {
            
        }

        private UDPArchorList archorList;
        public UDPArchorValueList(UDPArchorList archorList)
        {
            this.archorList = archorList;
        }

        public void Add(IPEndPoint iep, byte[] data)
        {
            UDPArchorValue value = null;
            //if (archorList != null)
            //{
            //    value = this.Find(i => i.Ip == iep.Address.ToString());
            //}
            //else
            {
                string ip = iep.Address.ToString();
                value = this.Find(i => i.Ip == ip);
                if (value == null)
                {
                    value=new UDPArchorValue();
                    value.Ip = iep.Address.ToString();
                    Add(value);
                    statistics.Add(value.Ip);

                    if (archorList != null)
                    {
                        var archor = archorList.Find(i => i.Ip == ip);
                        if (archor != null)
                        {
                            value.Id = archor.Id;
                        }
                    }
                }
                value.Value = ByteHelper.byteToHexStr(data);
            }
        }

        public string GetStatistics()
        {
            return statistics.GetText();
        }

        ArchorStatistics statistics=new ArchorStatistics();
    }
}
