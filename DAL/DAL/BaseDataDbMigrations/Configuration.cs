namespace DAL.BaseDataDbMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.BaseDataDb>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            //ContextKey = "DAL.BaseDataDb";

            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"BaseDataDbMigrations";

            MySql.Data.Entity.MySqlMigrationSqlGenerator sqlGenerator =
                new MySql.Data.Entity.MySqlMigrationSqlGenerator();
            this.SetSqlGenerator("MySql.Data.MySqlClient", sqlGenerator);
        }

        protected override void Seed(DAL.BaseDataDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
