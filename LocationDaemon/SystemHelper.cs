using System;
using System.Management;
using System.Runtime.InteropServices;

namespace LocationDaemon
{
    public static class SystemHelper
    {
        public struct MemoryStatus
        {
            public uint Length;
            public uint MemoryLoad;
            public uint TotalPhysical;
            public uint AvailablePhysical;
            public uint TotalPageFile;
            public uint AvailablePageFile;
            public uint TotalVirtual;
            public uint AvailableVirtual;
        }

        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(out MemoryStatus stat);

        public static long GetPhisicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(); //用于查询一些如系统信息的管理对象 
            searcher.Query = new SelectQuery("Win32_PhysicalMemory", "", new string[] { "Capacity" });//设置查询条件 
            ManagementObjectCollection collection = searcher.Get(); //获取内存容量 
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();

            long capacity = 0;
            while (em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if (baseObj.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("有错误发生！", "错误信息:" + ex.Message);
                        return 0;
                    }
                }
            }
            return capacity / (1024 * 1024 * 1024);
        }

    }
}
