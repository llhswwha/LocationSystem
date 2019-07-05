namespace DAL.LocationDbMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.LocationDb>
    {
        //数据迁移出现already exists问题处理
        //add-migration -ConfigurationTypeName DAL.LocationDbMigrations.Configuration "Reset" -ignorechanges
        //Update-database -ConfigurationTypeName DAL.LocationDbMigrations.Configuration -force

        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"LocationDbMigrations";

            MySql.Data.Entity.MySqlMigrationSqlGenerator sqlGenerator =
                new MySql.Data.Entity.MySqlMigrationSqlGenerator();
            this.SetSqlGenerator("MySql.Data.MySqlClient", sqlGenerator);
        }

        protected override void Seed(DAL.LocationDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


        }
    }
}
