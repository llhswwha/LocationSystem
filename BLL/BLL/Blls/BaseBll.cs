using BLL.Tools;
using Location.BLL.Tool;
using Location.IModel;
using SelfBatchImport;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TModel.Tools;

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
        where TKey: IEquatable<TKey>
    {
        public DbSet<T> DbSet { get; set; }

        public int GetCount()
        {
            try
            {
                return DbSet.Count();
            }
            catch (Exception e)
            {
                Log.Error(LogTags.DbGet,e.ToString());
                return 0;
            }
        }

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
            return Save(isSave);
        }

        public virtual async Task<bool> AddAsync(T item, bool isSave = true)
        {
            if (DbSet == null) return false;
            if (item == null) return false;
            DbSet.Add(item);
            return await SaveAsync(isSave);
        }

        public virtual bool AddOrUpdate(T item, bool isSave = true)
        {
            if (DbSet == null) return false;
            if (item == null) return false;
            DbSet.AddOrUpdate(item);
            return Save(isSave);
        }

        private string GetErrorMessage(DbEntityValidationException ex)
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
            return errors.ToString();
        }

        private string GetErrorMessage(DbUpdateException ex)
        {
            Exception innerEx = ex.InnerException;
            while (innerEx is DbUpdateException || innerEx is UpdateException)
            {
                if (innerEx.InnerException == null) break;
                innerEx = innerEx.InnerException;
            }
            Log.Error("BaseBll.Save DbUpdateException", innerEx);
            return innerEx.ToString();
        }

        public bool Save(bool isSave)
        {
            if (isSave == false) return true;
            try
            {
                int r = Db.SaveChanges();
                return r > 0;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorMessage = GetErrorMessage(ex);
                Log.Error("BaseBll.Save DbEntityValidationException:\n" + ErrorMessage);
                return false;
            }
            catch (DbUpdateException ex)
            {
                ErrorMessage = GetErrorMessage(ex);
                Log.Error("BaseBll.Save DbUpdateException:\n"+ErrorMessage);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Save Exception", ex);
                SetException(ex);
                return false;
            }
        }

        public async Task<bool> SaveAsync(bool isSave)
        {
            if (isSave == false) return true;
            try
            {
                int r = await Db.SaveChangesAsync();
                return r > 0;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorMessage = GetErrorMessage(ex);
                Log.Error("BaseBll.Save DbEntityValidationException:\n" + ErrorMessage);
                return false;
            }
            catch (DbUpdateException ex)
            {
                ErrorMessage = GetErrorMessage(ex);
                Log.Error("BaseBll.Save DbUpdateException:\n" + ErrorMessage);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Save Exception", ex);
                SetException(ex);
                return false;
            }
        }

        public Exception exception;

        public void SetException(Exception ex)
        {
            exception = ex;
            if (ex == null)
            {
                _errorMessage = "";
                stackTrace = "";
            }
            else
            {
                _errorMessage = ex.Message;
                stackTrace = ex.StackTrace;
            }
           
        }

        private string stackTrace;


        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        public virtual bool AddRange(IList<T> list,int maxTryCount = 3)
        {
            if (list == null || list.Count == 0) return true;
            if (DbSet == null) return false;
            //DbSet.AddRange(list);
            //return Save();
            return AddRange(Db, list, maxTryCount);
        }

        public virtual bool AddRange(params T[] list)
        {
            if (list == null || list.Length == 0) return true;
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

        public virtual T FindById(TKey id)
        {
            if (DbSet == null) return default(T);

            //if (id == null)
            //{
            //    return default(T);
            //}
            try
            {
                T obj = DbSet.FirstOrDefault(i => i.Id.Equals(id));
                return obj;
            }
            catch (Exception ex)
            {
                SetException(ex);
                Log.Error("BaseBll.Find", ex);
                return default(T);
            }
        }

        /// <summary>
        /// 实际上上FindByKey，加入有多个主键，也要传入多个参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Find(params object[] keys)
        {
            if (DbSet == null) return default(T);

            //if (id == null)
            //{
            //    return default(T);
            //}
            try
            {
                T obj = DbSet.Find(keys);
                return obj;
            }
            catch (Exception ex)
            {
                SetException(ex);
                Log.Error("BaseBll.Find", ex);
                return default(T);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isTracking">默认false,是为了提高性能。但是只能查询用,有个潜在的危险，关联到一个其他对象的属性上时，添加该属性会导致，添加重复添加数据（重复设备问题）</param>
        /// <returns></returns>
        public virtual List<T> ToList(bool isTracking=false)
        {
            var list = DbSet.ToListEx(isTracking);
            if (list == null)
            {
                ErrorMessage = DbHelper.ErrorMessage;
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isTracking">默认false,是为了提高性能。但是只能查询用,有个潜在的危险，关联到一个其他对象的属性上时，添加该属性会导致，添加重复添加数据（重复设备问题）</param>
        /// <returns></returns>
        public virtual Dictionary<TKey, T> ToDictionary(bool isTracking = false)
        {
            Dictionary<TKey, T> dic=new Dictionary<TKey, T>();
            var list = DbSet.ToListEx(isTracking);
            if (list == null)
            {
                ErrorMessage = DbHelper.ErrorMessage;
                return dic;
            }

            foreach (T item in list)
            {
                var id = item.Id;
                dic.Add(id, item);
            }
            return dic;
        }

        public virtual T DeleteById(TKey id)
        {
            if (id == null) return null;
            if (DbSet == null) return null;
            try
            {
                string strId = id + "";
                T obj = DbSet.FirstOrDefault(i => (i.Id+"") == strId);
                if (obj == null) return null;
                DbSet.Remove(obj);
                bool r= Save(true);
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
                SetException(ex);
                Log.Error("BaseBll.DeleteById", ex);
                return null;
            }
            
        }

        public virtual T DeleteByKeys(params object[] keys)
        {
            if (keys == null) return null;
            if (DbSet == null) return null;
            try
            {
                T obj = DbSet.Find(keys);
                if (obj == null) return null;
                DbSet.Remove(obj);
                bool r = Save(true);
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
                SetException(ex);
                Log.Error("BaseBll.DeleteByKeys", ex);
                return null;
            }

        }

        public bool RemoveList(List<T> list)
        {
            if (list == null) return false;
            if (list.Count == 0) return true;
            try
            {
                Db.BulkDelete(list);
                return true;

                //DbSet.RemoveRange(list);
                //return Save(true);
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
                SetException(null);
                DbSet.Remove(obj);
                return Save(isSave);
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Remove", ex);
                SetException(ex);
                return false;
            }
        }

        public bool Clear(int mode=0)
        {
            if (mode == 0)//RemoveList //老的,但是通用的
            {
                List<T> list = ToList(true);
                return RemoveList(list);
            }
            else
            {
                //新的快速的 但有局限的 可能出错
                if (mode == 1) //DeleteAllRows
                {
                    bool r = Db.DeleteAllRows<T>();
                    if (r == false)
                    {
                        var ex = EFExtensions.Exception.Message;
                        List<T> list = ToList(true);
                        if (list == null) return false;
                        DbSet.RemoveRange(list);
                        return Save(true);
                    }
                    return r;
                }
                else if (mode == 2) //DropTable
                {
                    bool r = Db.DropTable<T>();
                    if (r == false)
                    {
                        var ex = EFExtensions.Exception.Message;
                        List<T> list = ToList(true);
                        if (list == null) return false;
                        DbSet.RemoveRange(list);
                        return Save(true);
                    }
                    return r;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool RemoveHoldingEntityInContext(T entity)
        {
            ObjectContext objContext = ((IObjectContextAdapter)Db).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);
            object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            if (exists)
            {
                objContext.Detach(foundEntity);
            }
            return (exists);
        }

        public virtual bool Edit(T entity,bool isSave=true)
        {
            try
            {
                SetException(null);
                if (DbSet == null) return false;
                //RemoveHoldingEntityInContext(entity);//不知道为什么不起效果
                DbEntityEntry<T> entry = Db.Entry<T>(entity);
                var state1 = entry.State;
                entry.State = EntityState.Modified;
                var state2 = entry.State;
                var result= Save(isSave);
                ObjectContext objContext = ((IObjectContextAdapter)Db).ObjectContext;
                objContext.Detach(entity);//修改完后就从Context中删除
                var state3 = entry.State;
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Edit", ex);
                SetException(ex);
                return false;
            }
        }

        public virtual async Task<bool> EditAsync(T entity, bool isSave = true)
        {
            try
            {
                SetException(null);
                if (DbSet == null) return false;
                DbEntityEntry<T> entry = Db.Entry<T>(entity);
                entry.State = EntityState.Modified;
                return await SaveAsync(isSave);
            }
            catch (Exception ex)
            {
                Log.Error("BaseBll.Edit", ex);
                SetException(ex);
                return false;
            }
        }

        public virtual bool AddRange(TDb Db, IEnumerable<T> list,int maxTryCount=3)
        {
            if (list == null)
            {
                return false;
            }
            if (Db == null)
            {
                return false;
            }
            bool result = false;
            Exception exception = null;
            for (int i = 0; i < maxTryCount; i++)
            {
                try
                {
                    BulkInsert(list);
                    result = true;
                    break;//成功则退出
                }
                catch (Exception ex)
                {
                    /*
                     * 2019-09-02 06:00:34,397 [70] INFO  Logger - RealPos|0 获取定位数据:{"data_type":"1","engine_id":"","tag_id":"08C7","tag_id_dec":"2247","x":"34.60068","y":"7.93283","z":"0.84000","timestamp":"15677375234349","sn":"249","bettery":"4.09","events":[{"event_type":"LOCK"},{"event_type":"MOVE"}
2019-09-02 06:00:34,397 [72] INFO  Logger - RealPos|0 获取定位数据:{"data_type":"1","engine_id":"","tag_id":"08C7","tag_id_dec":"2247","x":"34.60068","y":"7.93283","z":"0.84000","timestamp":"15677375234349","sn":"249","bettery":"4.09","events":[{"event_type":"LOCK"},{"event_type":"MOVE"}]}
2019-09-02 06:00:34,398 [70] INFO  Logger - ParseJson|0 调整json数据: + ]}
2019-09-02 06:00:34,399 [70] INFO  Logger - RealPos|0 获取定位数据:{"data_type":"1","engine_id":"","tag_id":"08BB","tag_id_dec":"2235","x":"45.43393","y":"25.78800","z":"0.98000","timestamp":"1567375234365","sn":"107","bettery":"4.10","events":[{"event_type":"LOCK"},{"event_type":"MOVE"}]}
2019-09-02 06:00:34,398 [72] INFO  Logger - RealPos|0 获取定位数据:{"data_type":"1","engine_id":"","tag_id":"08BB","tag_id_dec":"2235","x":"45.43393","y":"25.78800","z":"0.98000","timestamp":"1567375234365","sn":"107","bettery":"4.10","events":[{"event_type":"LOCK"},{"event_type":"MOVE"}
2019-09-02 06:00:34,403 [72] INFO  Logger - ParseJson|0 调整json数据: + ]}
                     */
                    //Table has no partition for value 15677375234349
                    //Table has no partition for value 15667397966646 //1567375241100
                    exception = ex;
                    if (ex.Message.Contains("Table has no partition for value"))//数据太多了，数据库表分区不够用了
                    {
                        break;
                    }
                   
                    //失败则继续尝试
                    Log.Error("AddRange", string.Format("Try{0},Type:{1},Count:{2},Error:{3}", i,typeof(T), list.Count(), ex.ToString()));
                    Thread.Sleep(100);
                }
            }
            if (result == false && exception!=null)
            {
                SetException(exception);
            }
            return result;
        }

        private void BulkInsert(IEnumerable<T> list)
        {
            //BatchImport.Insert<T>(Db.Database, DbSet, (List<T>)list);
            Db.BulkInsert(list);
        }

        public virtual bool EditRange(List<T> list)
        {
            return this.EditRange(Db, list);
        }

        public virtual bool EditRange(TDb db, List<T> list, int maxTryCount = 3)
        {
            if (db == null)
            {
                return false;
            }
            if(list==null|| list.Count == 0)
            {
                return true;//没有数据也是天
            }
            bool result = false;
            Exception exception = null;
            for (int i = 0; i < maxTryCount; i++)
            {
                try
                {
                    db.BulkUpdate(list);
                    db.BulkSaveChanges();
                    result = true;
                    break;//成功则退出
                }
                catch (Exception ex)
                {
                    exception = ex;
                    //失败则继续尝试
                    Log.Error("EditRange", string.Format("Try{0},Type:{1},Count:{2},Error:{3}",i, typeof(T), list.Count(), ex.Message));
                    Thread.Sleep(100);
                }
            }
            if (result == false && exception != null)
            {
                SetException(exception);
            }
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IQueryable<T> QueryPage(int pageSize, int pageIndex, out int total)
        {
            total = DbSet.Count();
            return DbSet.Take(pageSize * (pageIndex - 1)).Skip(pageSize).AsQueryable();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<T> GetPageList(int pageSize, int pageIndex, out int total)
        {
            return QueryPage(pageSize,pageIndex,out total).ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sqlCount">查询列表总数</param>
        /// <param name="sqlstr">查询列表</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageInfo<T> GetPageList(string sqlCount,string strsql, int pageIndex, int pageSize)
        {
            PageInfo<T> page = new PageInfo<T>();
            DbRawSqlQuery<string> result = Db.Database.SqlQuery<string>(sqlCount);
            List<string> alist = result.ToList();
            page.total =Convert.ToInt32(alist[0].ToString());
            page.pageSize = pageSize;
            page.pageIndex = pageIndex;
            if (page.total % page.pageSize > 0)
            {
                page.totalPage = page.total / pageSize + 1;
            }
            else
            {
                page.totalPage = page.total / pageSize;
            }
            //分页sql
            strsql = strsql + string.Format("  order by id asc limit {0},{1}", pageSize * (pageIndex - 1), pageSize);
            DbRawSqlQuery<T> result1 = Db.Database.SqlQuery<T>(strsql);
            page.data = result1.ToList();
            return page;
        }
        public List<T> GetListBySql(string strsql)
        {
            DbRawSqlQuery<T> result1 = Db.Database.SqlQuery<T>(strsql);
            return result1.ToList();
        }

        public List<int> GetListIntBySql(string strsql)
        {
            DbRawSqlQuery<int> result = Db.Database.SqlQuery<int>(strsql);
            return result.ToList();
        }

        public List<string> GetListStringBySql(string strsql)
        {
            DbRawSqlQuery<string> result = Db.Database.SqlQuery<string>(strsql);
            return result.ToList();
        }

    }
}
