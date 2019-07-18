using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SelfBatchImport
{
    public static class BatchImport
    {

        public static void Insert<T>(Database Db, DbSet<T> Ds, List<T> list) where T : class
        {
            Stopwatch watch4 = Stopwatch.StartNew();
            
            string strTableName = GetTableName(Ds);
            string strKeyName = "";
            int nId = 0;
            bool bFlag = GetMaxKey<T>(Db, strTableName, ref strKeyName, ref nId);
            string strSql = BulkInsertSql<T>(list, strTableName);
            Db.ExecuteSqlCommand(strSql);
            if (bFlag)
            {
                GetAddRows<T>(Db, strTableName, strKeyName, nId, list);
            }
            watch4.Stop();
            long tt = watch4.ElapsedMilliseconds;
            return;
        }

        private static string GetTableName<T>(DbSet<T> Ds) where T : class
        {
            string strSql = Ds.Sql;
            Match Ma = Regex.Match(strSql, @"FROM(.*)?AS");
            string Mas = Ma.ToString();
            string[] Mas2 = Mas.Split('`');
            string strTableName = Mas2[1];
            return strTableName;
        }

        private static bool GetMaxKey<T>(Database Db, string strTableName, ref string strKeyName, ref int nId) where T : class
        {
            bool bFlag = false;

            try
            {
                PropertyInfo[] PList = typeof(T).GetProperties();
                PropertyInfo pkProp = PList.Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0).FirstOrDefault();
                if (pkProp == null)
                {
                    foreach (PropertyInfo item in PList)
                    {
                        if (String.Compare(item.Name, "ID", true) == 0)
                        {
                            pkProp = item;
                            break;
                        }
                    }
                }

                if (pkProp == null)
                {
                    return bFlag;
                }

                strKeyName = pkProp.Name;
                string strTypeName = pkProp.PropertyType.FullName;

                if (pkProp.PropertyType.FullName != typeof(Int32).FullName)
                {
                    return bFlag;
                }

                string strSql = "select MAX(" + strKeyName + ") from " + strTableName;
                nId = Db.SqlQuery<int>(strSql).FirstOrDefault();

                bFlag = true;
            }
            catch (Exception ex)
            {
                bFlag = false;
                string strError = ex.Message;
            }

            return bFlag;
        }

        public static string BulkInsertSql<T>(List<T> objs, string tableName) where T : class
        {
            if (objs == null || objs.Count == 0 || string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }
            string columns = GetColmons(objs[0]);
            if (string.IsNullOrEmpty(columns))
            {
                return string.Empty;
            }
            //string values = string.Join(",", objs.Select(p => string.Format("({0})", GetValues(p))).ToArray());
            string values = string.Join(",", objs.Select(p => string.Format("({0})", GetValues(p))).ToArray());
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into " + tableName);
            sql.Append("(" + columns + ")");
            sql.Append(" values " + values + "");
            return sql.ToString();
        }

        private static string GetColmons<T>(T obj) where T : class
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return string.Join(",", obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0).Select(p => p.Name).ToList());
        }


        /// <summary>
        /// 获取变量的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private static string GetValues<T>(T obj) where T : class
        {
            try
            {
                IEnumerable<System.Reflection.PropertyInfo> dd;

                if (obj == null)
                {
                    return string.Empty;
                }

                string str = string.Join(",", obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0).Select(p =>
                {
                    var v = p.GetValue(obj);
                    if (v == null)
                    {
                        v = GetDefaultValue(p.PropertyType);
                    }
                    if (v != null)
                    {
                        return string.Format("'{0}'",
                            Type.Equals(v.GetType(), typeof(DateTime))
                                ? Convert.ToDateTime(v).ToString("yyyy-MM-dd HH:mm:ss")
                                : v);
                    }
                    else
                    {
                        v = "";
                        return string.Format("'{0}'",
                            Type.Equals(v.GetType(), typeof(DateTime))
                                ? Convert.ToDateTime(v).ToString("yyyy-MM-dd HH:mm:ss")
                                : v);
                    }
                    
                }).ToArray());
                return str;
                return string.Join(",", obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0).Select(p => string.Format("'{0}'", Type.Equals(p.GetValue(obj).GetType(), typeof(DateTime)) ? Convert.ToDateTime(p.GetValue(obj)).ToString("yyyy-MM-dd HH:mm:ss") : p.GetValue(obj))).ToArray());
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private static void GetAddRows<T>(Database Db, string strTableName, string strKeyName, int nId, List<T> objs)
        {
            try
            {
                string strSql = "select * from " + strTableName + " where " + strKeyName + " > " + Convert.ToString(nId);
                List<T> list = Db.SqlQuery<T>(strSql).ToList();
                objs.Clear();


                foreach (T item in list)
                {
                    objs.Add(item);
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;

            }

            return;
        }
    }
}
