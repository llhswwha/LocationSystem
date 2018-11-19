using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchorUDPTool.Models;

namespace ArchorUDPTool.Commands
{
    public class UDPCommandResult
    {
        public byte[] Bytes { get; set; }

        public string Hex { get; set; }

        public string ByteStr { get; set; }

        public List<byte> CmdPart { get; set; }

        public string CmdHex { get; set; }

        public List<byte> DataPart { get; set; }

        public string DataHex { get; set; }

        public List<byte> EndPart { get; set; }

        public string EndHex { get; set; }

        public UDPCommandResult()
        {
            CmdPart = new List<byte>();
            DataPart = new List<byte>();
            EndPart = new List<byte>();
        }

        public UDPCommandResult(byte[] bytes):this()
        {
            Parse(bytes);
        }

        public void Parse(byte[] bytes)
        {
            Bytes = bytes;
            Hex = ByteHelper.byteToHexStr(bytes);
            ByteStr = ByteHelper.byteToStr(bytes);

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i < 5)
                {
                    CmdPart.Add(bytes[i]);
                }
                else if (i<bytes.Length-4)
                {
                    DataPart.Add(bytes[i]);
                }
                else
                {
                    EndPart.Add(bytes[i]);
                }
            }

            CmdHex= ByteHelper.byteToHexStr(CmdPart.ToArray());
            DataHex = ByteHelper.byteToHexStr(DataPart.ToArray());
            EndHex = ByteHelper.byteToHexStr(EndPart.ToArray());
        }

        public string GetValue()
        {
            string hex = DataHex;
            return hex;
        }

        public string GetIPValue()
        {
            if (DataPart.Count == 4)
            {
                return DataPart[0] + "." + DataPart[1] + "." + DataPart[2] + "." + DataPart[3];
            }
            return "";
        }

        public long GetLongValue()
        {
            string hex = DataHex;
            long number = Convert.ToInt64(hex, 16);
            return number;
        }

        public int GetIntValue()
        {
            string hex = DataHex;
            int number = Convert.ToInt32(hex, 16);
            return number;
        }

        public bool GetBoolValue()
        {
            string hex = DataHex;
            return hex != "00";
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Hex, ByteStr);
        }
    }
}
