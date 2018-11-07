using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class BaseBuffer
    {
        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime DataTime;

        /// <summary>
        /// 更新数据间隔
        /// </summary>
        public int UpdateInterval = 60;//todo:应该写到配置文件中

        public bool First = false;

        public void LoadData()
        {
            TimeSpan time = DateTime.Now - DataTime;
            if (time.TotalSeconds > UpdateInterval) //大于该时间则更新数据，否则用老数据
            {
                DataTime = DateTime.Now;
                UpdateData();
            }
        }

        protected abstract void UpdateData();
    }
}
