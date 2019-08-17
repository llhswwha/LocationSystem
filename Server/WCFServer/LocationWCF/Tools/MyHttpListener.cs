using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Tools
{
    public class MyHttpListener
    {
        public string url = "";

        public event Func<string,string> OnReceived;

        public MyHttpListener(string url)
        {
            this.url = url;
        }

        HttpListener httpListener;

        public bool Start()
        {
            if (httpListener == null)
            {
                try
                {
                    httpListener = new HttpListener();
                    httpListener.Prefixes.Add(url);
                    httpListener.Start();
                    httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), httpListener);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("MyHttpListener.Start,url:{0},Exception:{1}",url,ex));
                    httpListener = null;
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public void Stop()
        {
            try
            {
                if (httpListener != null)
                {
                    try
                    {
                        if(httpListener.IsListening)
                            httpListener.Stop();
                    }
                    catch (Exception ex)
                    {

                        Log.Error("MyHttpListener.Stop:" + ex);
                    }
                    httpListener = null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
        }

        private void WebRequestCallback(IAsyncResult ar)
        {
            try
            {
                if (httpListener == null)
                {
                    return;
                }
                HttpListenerContext context = httpListener.EndGetContext(ar);
                httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), httpListener);
                ProcessRequest(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Stop();
                Start();//重启
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            //Log.Info(LogTags.Server, "MyHttpListener.ProcessRequest");
            var request = context.Request;
            var response = context.Response;
            ////如果是js的ajax请求，还可以设置跨域的ip地址与参数
            //context.Response.AppendHeader("Access-Control-Allow-Origin", "*");//后台跨域请求，通常设置为配置文件
            //context.Response.AppendHeader("Access-Control-Allow-Headers", "ID,PW");//后台跨域参数设置，通常设置为配置文件
            //context.Response.AppendHeader("Access-Control-Allow-Method", "post");//后台跨域请求设置，通常设置为配置文件
            context.Response.ContentType = "text/plain;charset=UTF-8";//告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
            context.Response.AddHeader("Content-type", "text/plain");//添加响应头信息
            context.Response.ContentEncoding = Encoding.UTF8;
            string returnObj = null;//定义返回客户端的信息
            if (request.HttpMethod == "POST" && request.InputStream != null)
            {
                //处理客户端发送的请求并返回处理信息
                returnObj = HandleRequest(request, response);
            }
            else
            {
                returnObj = $"HttpMethod not POST or InputStream is NULL";
            }
            var returnByteArr = Encoding.UTF8.GetBytes(returnObj);//设置客户端返回信息的编码
            try
            {
                using (var stream = response.OutputStream)
                {
                    //把处理信息返回到客户端
                    stream.Write(returnByteArr, 0, returnByteArr.Length);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log.Info($"Error：{ex.ToString()}");
                }catch(Exception e)
                {

                }             
            }
        }

        private string HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string res = $"接收数据完成";
            try
            {
                StreamReader sr = new StreamReader(request.InputStream);
                var json = sr.ReadToEnd().ToString();
                //
                //获取得到数据data可以进行其他操作
                //todo:发送告警
                if (OnReceived != null)
                {
                    res = OnReceived(json);
                }
            }
            catch (Exception ex)
            {
                response.StatusDescription = "404";
                response.StatusCode = 404;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log.Info($"在接收数据时发生错误:{ex.ToString()}");
                }catch(Exception e)
                {

                }                
                return $"在接收数据时发生错误:{ex.ToString()}";//把服务端错误信息直接返回可能会导致信息不安全，此处仅供参考
            }
            response.StatusDescription = "200";//获取或设置返回给客户端的 HTTP 状态代码的文本说明。
            response.StatusCode = 200;// 获取或设置返回给客户端的 HTTP 状态代码。
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Log.Info($"接收数据完成,时间：{DateTime.Now.ToString()}");
            }catch(Exception e)
            {

            }
            return res;
        }
    }
}
