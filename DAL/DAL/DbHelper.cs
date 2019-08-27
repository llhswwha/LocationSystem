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

            int tryCount = 3;
            for (int i = 0; i < tryCount; i++)
            {
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
                    if (i < tryCount - 1)
                    {
                        Thread.Sleep(100);
                        Log.Error(LogTags.DbGet, "BaseBll.ToList<T> type=" + typeof(T) + ",try again:" + i+1);
                    }
                    else
                    {
                        Log.Error(LogTags.DbGet,"BaseBll.ToList<T> type=" + typeof(T)+"\n"+ ex);
                        ErrorMessage = GetExceptionMessage(ex);
                    }
                }
            }
            return null;

        }

        public static string GetExceptionMessage(Exception ex)
        {
            string msg = "";
            msg = ex.Message;
            if (ex.Message ==
                "An error occurred while reading from the store provider's data reader. See the inner exception for details."
            )
            {
                msg = ex.InnerException.Message;
            }
            return msg;
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
            try
            {
                List<T> list = dbSet.ToListEx(true);
                dbSet.RemoveRange(list);
                //return dbSet.RemoveList(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
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
    }
}
