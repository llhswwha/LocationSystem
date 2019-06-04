using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public static class EFExtensions
    {
        public static Exception Exception = null;

        /// <summary>
        /// 清空表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool DropTable<T>(this DbContext context)
        {
            //List<T> list = ToList(true);
            //return RemoveList(list);
            //老的
            try
            {
                var tableName = context.GetTableName<T>();
                var dbName = context.Database.Connection.Database;
                var r = context.Database.ExecuteSqlCommand(string.Format("DROP TABLE {0}.{1}", dbName, tableName));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("EFExtensions.Truncate", ex);
                Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// 清空表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool DeleteAllRows<T>(this DbContext context)
        {
            //List<T> list = ToList(true);
            //return RemoveList(list);
            //老的
            try
            {
                var tableName = context.GetTableName<T>();
                var dbName = context.Database.Connection.Database;
                var r = context.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}.{1}", dbName, tableName));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("EFExtensions.Truncate", ex);
                Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// 清空表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool Truncate<T>(this DbContext context)
        {
            //List<T> list = ToList(true);
            //return RemoveList(list);
            //老的
            try
            {
                var tableName = context.GetTableName<T>();
                var dbName = context.Database.Connection.Database;
                var r = context.Database.ExecuteSqlCommand(string.Format("TRUNCATE TABLE {0}.{1}", dbName, tableName));
                //有外键关系的就算没有数据也是不能Truncate的
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("EFExtensions.Truncate", ex);
                Exception = ex;
                return false;
            }
        }

        #region GetTableName
        //引用自 https://romiller.com/2014/04/08/ef6-1-mapping-between-types-tables/
        /// <summary>
        /// 获取实体类所对应的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this DbContext context)
        {
            return GetTableName(context, typeof(T));
        }

        /// <summary>
        /// 获取实体类所对应的表名
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(this DbContext context,Type type)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }

        public static IEnumerable<string> GetTableNames<T>(this DbContext context)
        {
            return GetTableNames(context, typeof(T));
        }

        /// <summary>
        /// 一个类分到多个表中的情况
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTableNames(this DbContext context,Type type)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity sets (tables) that the entity is mapped
            var tables = mapping
                .EntityTypeMappings.Single()
                .Fragments;

            // Return the table name from the storage entity set
            return tables.Select(f => (string)f.StoreEntitySet.MetadataProperties["Table"].Value ?? f.StoreEntitySet.Name);
        }
        #endregion
    }
}
