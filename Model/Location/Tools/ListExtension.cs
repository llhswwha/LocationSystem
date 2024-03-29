﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Tools
{
    public static class ListExtension
    {
        /// <summary>
        /// Unity中WCF不能传递空的数组，只能传递有数量的数组和null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> ToWCFList<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        /// <summary>
        /// Unity中WCF不能传递空的数组，只能传递有数量的数组和null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToWCFList<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }
    }
}
