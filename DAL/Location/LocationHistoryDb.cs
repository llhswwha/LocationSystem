using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.Model.Work;

namespace Location.DAL
{
    public class LocationHistoryDb:DbContext
    {
        public LocationHistoryDb():base("LocationHistoryConnection")
        {
            //Database.SetInitializer<LocationHistoryDb>(null); 
            Database.SetInitializer<LocationHistoryDb>(new DropCreateDatabaseIfModelChanges<LocationHistoryDb>());//数据模型发生变化是重新创建数据库

            //this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Position> Position { get; set; }

        public DbSet<U3DPosition> U3DPositions { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.PersonnelMobileInspectionHistory> PersonnelMobileInspectionHistorys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.PersonnelMobileInspectionItemHistory> PersonnelMobileInspectionItemHistorys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.OperationTicketHistory> OperationTicketHistorys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.OperationItemHistory> OperationItemHistorys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.WorkTicketHistory> WorkTicketHistorys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.SafetyMeasuresHistory> SafetyMeasuresHistorys { get; set; }


    }
}
