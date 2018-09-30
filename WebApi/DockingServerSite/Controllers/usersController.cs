using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using CommunicationClass.SihuiThermalPowerPlant;

namespace SihuiThermalPowerPlant.Controllers
{
    public class usersController : ApiController
    {
        [HttpGet]
        public BaseTran<users> GetUserList()
        {
            BaseTran<users> send = new BaseTran<users>();
            List<users> lst = new List<users>();
            lst.Add(new users() { id = 1, name = "蔡万伟", gender = 1, email = "caiwanwei@qq.com", phone = "111111111", mobile = "aaaaaaaaa", enabled = true, dept_name = "研发" });
            lst.Add(new users() { id = 2, name = "叶宗雷", gender = 1, email = "yezonglei@qq.com", phone = "222222222", mobile = "bbbbbbbbb", enabled = true, dept_name = "研发" });
            lst.Add(new users() { id = 3, name = "郑国涛", gender = 1, email = "zhengguotao@qq.com", phone = "333333333", mobile = "ccccccccc", enabled = true, dept_name = "研发" });
            lst.Add(new users() { id = 4, name = "王朝武", gender = 1, email = "wangchaowu@qq.com", phone = "444444444", mobile = "ddddddddd", enabled = true, dept_name = "研发" });
            lst.Add(new users() { id = 5, name = "王锴", gender = 1, email = "wukai@qq.com", phone = "555555555", mobile = "eeeeeeeee", enabled = true, dept_name = "研发" });

            send.total = 5;
            send.msg = "ok";
            send.data = lst;

            return send;
        }


    }
}
