using Location.BLL.Tool;
using Location.IModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;


namespace BLL.Blls
{
    public class BaseBll<T>:IDisposable where T : DbContext
    {
        public T Db { get; set; }

        //public BaseBll()
        //{
        //    Db=new LocationDb();
        //}

        public BaseBll(T db)
        {
            Db = db;
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }

    public abstract class BaseBll<T, TDb> : BaseBll<T,TDb,int>
        where T : class, IId<int>, new()
        where TDb : DbContext, new()
    {
        public BaseBll() : base(new TDb())
        {
            InitDbSet();
        }

        public BaseBll(TDb db) : base(db)
        {
            InitDbSet();
        }
    }

        public abstract class BaseBll<T,TDb,TKey>: BaseBll<TDb> 
        where T :class, IId<TKey>, new()
        where TDb : DbContext, new()
    {
        public DbSet<T> DbSet { get; set; }

        protected abstract void InitDbSet();

        public BaseBll() : base(new TDb())
        {
            InitDbSet();
        }

        public BaseBll(TDb db) : base(db)
        {
            InitDbSet();
        }

        public virtual bool Add(T item,bool isSave=true)
        {
            if (DbSet == null) return false;
            if (item == null) return false;
            DbSet.Add(item);
            if (isSave)
            {
                return Save();
            }
            else
            {
                return true;
            }
        }

        public virtual bool AddOrUpdate(T item, bool isSave = true)
        {
            if (DbSet == null) return false;
            if (item == null) return false;
            DbSet.AddOrUpdate(item);
            if (isSave)
            {
                return Save();
            }
            else
            {
                return true;
            }
        }

        public bool Save()
        {
            try
            {
                int r = Db.SaveChanges();
                return r > 0;
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder errors = new StringBuilder();
                IEnumerable<DbEntityValidationResult> validationResult = ex.EntityValidationErrors;
                foreach (DbEntityValidationResult result in validationResult)
                {
                    ICollection<DbValidationError> validationError = result.ValidationErrors;
                    foreach (DbValidationError err in validationError)
                    {
                        errors.Append(err.PropertyName + ":" + err.ErrorMessage + "\r\n");
                    }
                }
                ErrorMessage = errors.ToString();
                //Console.WriteLine(ErrorMessage);
                Log.Error("BaseBll.Save DbEntityValidationException:\n" + ErrorMessage);
                //简写
                //var validerr = ex.EntityValidationErrors.First().ValidationErrors.First();
                //Console.WriteLine(validerr.PropertyName + ":" + validerr.ErrorMessage);

                return false;
            }
            catch (DbUpdateException ex)
            {
                Exception innerEx = ex.InnerException;
                while (innerEx is DbUpdateException || innerEx is UpdateException)
                {
                    if (innerEx.InnerException == null) break;
                    innerEx = innerEx.InnerException;
                }
                Log.Error("BaseBll.Save DbUpdateException", innerEx);
                ErrorMessage = innerEx.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Save Exception", ex);
                ErrorMessage = ex.ToString();
                return false;
            }
        }

        public string ErrorMessage { get; set; }

        public virtual bool AddRange(IList<T> list)
        {
            if (DbSet == null) return false;
            //DbSet.AddRange(list);
            //return Save();
            return AddRange(Db, list);
        }

        public virtual bool AddRange(params T[] list)
        {
            if (DbSet == null) return false;
            //DbSet.AddRange(list);
            //return Save();
            return AddRange(Db, list);
        }

        public T FirstOrDefault(
            Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public T Find(
            Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public List<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public virtual T Find(object id)
        {
            if (DbSet == null) return default(T);

            if (id == null)
            {
                return default(T);
            }
            try
            {
                T obj = DbSet.Find(id);
                return obj;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Find", ex);
                return default(T);
            }
        }

        public virtual List<T> ToList(bool isTracking=false)
        {
            if (DbSet == null) return null;
            try
            {
                List<T> list = null;
                if (isTracking)
                {
                    list = DbSet.ToList();
                }
                else
                {
                    list = DbSet.AsNoTracking().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.ToList", ex);
                ErrorMessage = ex.Message;
                return null;
            }
        }

        public virtual T DeleteById(object id)
        {
            if (id == null) return null;
            if (DbSet == null) return null;
            try
            {
                T obj = DbSet.Find(id);
                if (obj == null) return null;
                DbSet.Remove(obj);
                bool r= Save();
                if (r)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.DeleteById", ex);
                return null;
            }
            
        }

        public bool RemoveList(List<T> list)
        {
            //foreach (T item in list)
            //{
            //    DbSet.Remove(item);
            //}
            //return Save();

            //Db.BulkDelete(list);

            try
            {
                Db.BulkDelete(list);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Clear", ex);
                //ErrorMessage = ex.Message;
                //return false;
                foreach (var item in list)
                {
                    T r = DeleteById(item.Id);
                    if (r == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool Remove(T obj,bool isSave=true)
        {
            try
            {
                ErrorMessage = "";
                DbSet.Remove(obj);
                if (isSave)
                {
                    return Save();
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Remove", ex);
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Clear()
        {
            List<T> list = ToList(true);
            return RemoveList(list);
        }

        public virtual bool Edit(T entity,bool isSave=true)
        {
            try
            {
                ErrorMessage = "";
                if (DbSet == null) return false;
                DbEntityEntry<T> entry = Db.Entry<T>(entity);
                entry.State = EntityState.Modified;
                if (isSave)
                {
                    return Save();
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Edit", ex);
                ErrorMessage = ex.ToString();
                return false;
            }

        }

        public virtual bool AddRange(TDb Db, IEnumerable<T> list)
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

        public virtual bool EditRange(List<T> list)
        {
            return this.EditRange(Db, list);
        }

        public virtual bool EditRange(TDb db, List<T> list)
        {
            try
            {
                if (db == null)
                {
                    return false;
                }
                db.BulkUpdate(list);
                db.BulkSaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.EditRange", ex);
                return false;
            }
        }
    }
}
