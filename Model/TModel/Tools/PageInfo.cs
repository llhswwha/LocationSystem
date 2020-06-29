using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Tools
{
   public   class PageInfo<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public  int total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public  int  totalPage { get; set; }
        /// <summary>
        /// 页面条数（一页多少条）
        /// </summary>
        public  int pageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public  int pageIndex { get; set; }
        /// <summary>
        /// list
        /// </summary>
        public  List<T> data { get; set; }  
    }
}
