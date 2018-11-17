using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public class UDPCommandResult
    {
        public byte[] Bytes { get; set; }

        public string Hex { get; set; }

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

        public long GetNumberValue()
        {
            string hex = DataHex;
            long number = Convert.ToInt64(hex, 16);
            return number;
        }

        public override string ToString()
        {
            return Hex;
        }
    }

    public class CommandResultGroup
    {
        public string Id { get; set; }

        public List<UDPCommandResult> Items { get; set; }

        public string ServerIp { get; set; }
        public long ServerPort { get; set; }

        public string ArchorId { get; set; }

        public string ArchorIp { get; set; }

        public CommandResultGroup()
        {
            Items = new List<UDPCommandResult>();
        }

        public CommandResultGroup(string id)
        {
            Items = new List<UDPCommandResult>();
            Id = id;
        }


        public void AddData(byte[] bytes)
        {
            var r = new UDPCommandResult(bytes);
            this.Items.Add(r);
            if (r.CmdHex == UDPCommands.GetServerIpR)
            {
                ServerIp = r.GetIPValue();
            }
            else if (r.CmdHex == UDPCommands.GetIdR)
            {
                ArchorId = r.GetValue();
            }
            else if (r.CmdHex == UDPCommands.GetIpR)
            {
                ArchorIp = r.GetIPValue();
            }
            else if (r.CmdHex == UDPCommands.GetPortR)
            {
                ServerPort = r.GetNumberValue();
            }
            else
            {

            }
        }

        public override string ToString()
        {
            return Id;
        }
    }

    public class CommandResultManager
    {
        public List<CommandResultGroup> Groups { get; set; }

        public CommandResultManager()
        {
            Groups = new List<CommandResultGroup>();
        }

        public void Add(System.Net.IPEndPoint iep, byte[] data)
        {
            string id = iep.ToString();
            var group = GetById(id);
            group.AddData(data);

        }

        public CommandResultGroup GetById(string id)
        {
            var g = Groups.Find(i => i.Id == id);
            if (g == null)
            {
                g = new CommandResultGroup(id);
                Groups.Add(g);
            }
            else
            {

            }
            return g;
        }
    }
}
