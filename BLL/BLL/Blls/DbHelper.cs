using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Blls
{
    public static class DbHelper
    {
        public static String ErrorMessage = "";

        public static List<T> ToListEx<T>(this DbSet<T> dbSet,bool isTracking = false) where T : class
        {
            ErrorMessage = "";
            if (dbSet == null) return null;
            try
            {
                List<T> list = null;
                if (isTracking)
                {
                    list = dbSet.ToList();
                }
                else
                {
                    list = dbSet.AsNoTracking().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.ToList<T> type=" + typeof(T), ex);
                ErrorMessage = ex.Message;
                return null;
            }
        }

        //public static bool RemoveList<T>(this DbSet<T> dbSet,List<T> list) where T : class
        //{
        //    if (list == null) return false;
        //    if (list.Count == 0) return true;
        //    try
        //    {
        //        //Db.BulkDelete(list);
        //        //return true;

        //        dbSet.RemoveRange(list);
        //        return Save(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("BaseBll.Clear", ex);
        //        //ErrorMessage = ex.Message;
        //        //return false;
        //        foreach (var item in list)
        //        {
        //            T r = dbSet.DeleteById(item.Id);
        //            if (r == null)
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //}

        //public static T DeleteById<T>(this DbSet<T> dbSet,object id) where T : class
        //{
        //    if (id == null) return null;
        //    if (dbSet == null) return null;
        //    try
        //    {
        //        T obj = dbSet.Find(id);
        //        if (obj == null) return null;
        //        dbSet.Remove(obj);
        //        bool r = Save(true);
        //        if (r)
        //        {
        //            return obj;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("BaseBll.DeleteById", ex);
        //        return null;
        //    }

        //}

        //public static bool Save<T>(this DbSet<T> dbSet,bool isSave) where T : class
        //{
        //    if (isSave == false) return true;
        //    try
        //    {
        //        int r = Db.SaveChanges();
        //        return r > 0;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        ErrorMessage = GetErrorMessage(ex);
        //        Log.Error("BaseBll.Save DbEntityValidationException:\n" + ErrorMessage);
        //        return false;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        ErrorMessage = GetErrorMessage(ex);
        //        Log.Error("BaseBll.Save DbUpdateException:\n" + ErrorMessage);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("BaseBll.Save Exception", ex);
        //        ErrorMessage = ex.ToString();
        //        return false;
        //    }
        //}

        public static bool Clear<T>(this DbSet<T> dbSet) where T : class
        {
            List<T> list = dbSet.ToListEx(true);
            dbSet.RemoveRange(list);
            //return dbSet.RemoveList(list);
            return true;
            //老的

            //bool r = Db.DeleteAllRows<T>();
            ////bool r = Db.DropTable<T>();
            //if (r == false)
            //{
            //    var ex = EFExtensions.Exception.Message;
            //    List<T> list = ToList(true);
            //    if (list == null) return false;
            //    DbSet.RemoveRange(list);
            //    return Save(true);
            //}
            //return r;
        }

        public static bool Clear<T>(this DbContext db, DbSet<T> dbSet) where T : class
        {
            List<T> list = dbSet.ToListEx(true);
            dbSet.RemoveRange(list);
            //return dbSet.RemoveList(list);
            db.SaveChanges();
            return true;
            //老的

            //bool r = Db.DeleteAllRows<T>();
            ////bool r = Db.DropTable<T>();
            //if (r == false)
            //{
            //    var ex = EFExtensions.Exception.Message;
            //    List<T> list = ToList(true);
            //    if (list == null) return false;
            //    DbSet.RemoveRange(list);
            //    return Save(true);
            //}
            //return r;
        }

        public static bool AddRange<T>(this DbContext Db, IEnumerable<T> list) where T : class
        {
            if (list == null)
            {
                return false;
            }
            if (Db == null)
            {
                return false;
            }
            try
            {
                Db.BulkInsert(list);

            }
            catch (Exception ex)
            {
                Log.Error(string.Format("BaseBll.AddRange.BulkInsert,Type:{0},Count:{1},Error:{2}", typeof(T), list.Count(), ex.Message));

                try
                {
                    Thread.Sleep(100);
                    Db.BulkInsert(list);
                    return true;//有一定概率先失败后成功
                }
                catch (Exception ex2)
                {
                    Log.Error(string.Format("BaseBll.AddRange.BulkInsert,Type:{0},Count:{1},Error:{2}", typeof(T), list.Count(), ex2.Message));

                    try
                    {
                        Thread.Sleep(100);
                        Db.BulkInsert(list);
                        return true;//有一定概率先失败后成功
                    }
                    catch (Exception ex3)
                    {
                        Log.Error(string.Format("BaseBll.AddRange.BulkInsert,Type:{0},Count:{1},Error:{2}", typeof(T), list.Count(), ex3.Message));
                        return false;
                    }
                }
                return false;
            }

            try
            {
                Db.BulkSaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("BaseBll.AddRange.BulkSaveChanges,Type:{0},Count:{1}", typeof(T), list.Count()), ex);
                return false;
            }
            return true;
        }
    }
}
