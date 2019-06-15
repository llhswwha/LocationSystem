using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.BaseData;
using BLL.Blls;
namespace DAL
{
    public class BaseDataDb : DbContext
    {
        public static string Name = "BaseData_MySql";

        public bool IsCreateDb;
        public BaseDataDb() : base(Name)
        {
            IsCreateDb = true;
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (IsCreateDb)
            {
                Database.SetInitializer<LocationDb>(new DropCreateDatabaseIfModelChanges<LocationDb>());//数据模型发生变化是重新创建数据库
                                                                                                        //Database.SetInitializer<LocationDb>(new MigrateDatabaseToLatestVersion<LocationDb, Configuration>());//自动数据迁移
            }
            else
            {
                Database.SetInitializer<LocationDb>(null);
            }
        }

        public DbSet<user> users { get; set; }

        public DbSet<org> orgs { get; set; }

        public DbSet<zone> zones { get; set; }

        public DbSet<device> devices { get; set; }

        public DbSet<cards> cards { get; set; }

        public int SetTable<T>(DbSet<T> ds,List<T> list) where T :class
        {
            this.Clear(ds);
            ds.AddRange(list);
            return this.SaveChanges();
        }
    }
}
