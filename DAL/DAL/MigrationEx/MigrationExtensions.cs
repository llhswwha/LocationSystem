using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MigrationEx
{
    public static class MigrationExtensions
    {
        public static void AddColumnIfNotExists(this DbMigration migration, string table, string name, Func<ColumnBuilder, ColumnModel> columnAction, object anonymousArguments = null)
        {
            ((IDbMigration)migration)
              .AddOperation(new AddColumnIfNotExistsOperation(table, name, columnAction, anonymousArguments));
        }
    }
}
