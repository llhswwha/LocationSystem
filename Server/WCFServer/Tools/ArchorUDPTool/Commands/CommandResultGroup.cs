using ArchorUDPTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public class CommandResultGroup
    {
        public bool IsNew { get; set; }

        public string Id { get; set; }

        public string GetIp()
        {
            string[] parts = Id.Split(':');
            if (parts.Length == 2)
            {
                return parts[0];
            }
            return Id;
        }

        public List<UDPCommandResult> Items { get; set; }

        public UDPArchor Archor { get; set; }

        public CommandResultGroup()
        {
            Items = new List<UDPCommandResult>();
            Archor = new UDPArchor();
            IsNew = true;
        }

        public CommandResultGroup(UDPArchor item)
        {
            Items = new List<UDPCommandResult>();
            Id = item.Client;
            Archor = item;
        }

        public CommandResultGroup(string id) : this()
        {
            Id = id;
            Archor.Client = id;
        }

        public string NewValue { get; set; }

        public string AddData(byte[] bytes)
        {
            Archor.IsConnected = "True";
            var r = new UDPCommandResult(bytes);
            var newValue = r.ToString();
            this.Items.Add(r);
            if (r.CmdHex == UDPCommands.GetServerIpR)
            {
                Archor.ServerIp = r.GetIPValue();
                newValue += "  ServerIP=" + r.GetIPValue();
            }
            else if (r.CmdHex == UDPCommands.GetIdR)
            {
                Archor.Id = r.GetValue();
                newValue += "  ID=" + r.GetValue();
            }
            else if (r.CmdHex == UDPCommands.GetIpR)
            {
                Archor.Ip = r.GetIPValue();
                newValue += "  IP=" + r.GetIPValue();
            }
            else if (r.CmdHex == UDPCommands.GetServerPortR)
            {
                Archor.ServerPort = r.GetIntValue();
                newValue += "  Port=" + r.GetIntValue();
            }
            else if (r.CmdHex == UDPCommands.GetTypeR)
            {
                Archor.Type = r.GetIntValue();
                newValue += "  ArchorType=" + r.GetIntValue();
            }
            else if (r.CmdHex == UDPCommands.GetDHCPR)
            {
                Archor.DHCP = r.GetBoolValue();
                newValue += "  DHCP=" + r.GetBoolValue();
            }
            else if (r.CmdHex == UDPCommands.GetMaskR)
            {
                Archor.Mask = r.GetIPValue();
                newValue += "  子网掩码=" + r.GetIPValue();
            }
            else if (r.CmdHex == UDPCommands.GetGatewayR)
            {
                Archor.Gateway = r.GetIPValue();
                newValue += "  网关=" + r.GetIPValue();
            }
            else if (r.CmdHex.StartsWith(UDPCommands.GetSoftVersionR))
            {
                Archor.SoftVersion = r.GetValue();
                newValue += "  软件版本=" + r.GetValue();
            }
            else if (r.CmdHex.StartsWith(UDPCommands.GetHardVersionR))
            {
                Archor.HardVersion = r.GetValue();
                newValue += "  硬件版本=" + r.GetValue();
            }
            else if (r.CmdHex == UDPCommands.GetPowerR)
            {
                Archor.Power = r.GetIntValue();
                newValue += "  功率=" + r.GetIntValue();
            }
            else if (r.CmdHex == UDPCommands.GetMACR)
            {
                Archor.MAC = r.GetMACValue();
                newValue += "  MAC=" + r.GetMACValue();
            }
            else
            {

            }
            NewValue = newValue;
            return newValue;
        }

        public override string ToString()
        {
            string txt = string.Format("[{0}]:{1}", Id, NewValue);
            if (IsNew)
            {
                txt += " New!";
            }
            return txt;

        }
    }
}
