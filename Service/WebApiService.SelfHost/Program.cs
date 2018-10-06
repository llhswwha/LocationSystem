using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace WebApiService.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:1234");
            WebApiConfiguration.Configure(config);
            var host = new HttpSelfHostServer(config);
            host.OpenAsync().Wait();
            Console.WriteLine("host:" + config.BaseAddress);
            Console.Read();
        }
    }
}
