using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.topviewxp;

namespace Location.DAL
{
    public class topviewxpDb : DbContext
    {
        public topviewxpDb():base("topviewxpConnection")
        {
            Database.SetInitializer<topviewxpDb>(null);
            //Database.SetInitializer<topviewxpDb>(new DropCreateDatabaseIfModelChanges<topviewxpDb>()); //数据模型发生变化是重新创建数据库 
            //DropCreateDatabaseAlways
            //DropCreateDatabaseIfModelChanges
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<t_KKSCode> t_KKSCodes { get; set; }

        public DbSet<t_SetModel> t_SetModelS { get; set; }

        public DbSet<t_Template_TypeProperty> t_Template_TypeProperties { get; set; }
    }
}
