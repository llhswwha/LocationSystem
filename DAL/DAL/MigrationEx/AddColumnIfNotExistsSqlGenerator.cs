using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MigrationEx
{
    public class AddColumnIfNotExistsSqlGenerator 
        : SqlServerMigrationSqlGenerator
        //: MySql.Data.Entity.MySqlMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            var operation = migrationOperation as AddColumnIfNotExistsOperation;
            if (operation == null) return;

            using (var writer = Writer())
            {
                writer.WriteLine("IF NOT EXISTS(SELECT 1 FROM sys.columns");
                writer.WriteLine($"WHERE Name = N'{operation.Name}' AND Object_ID = Object_ID(N'{Name(operation.Table)}'))");
                writer.WriteLine("BEGIN");
                writer.WriteLine("ALTER TABLE ");
                writer.WriteLine(Name(operation.Table));
                writer.Write(" ADD ");

                var column = operation.ColumnModel;
                Generate(column, writer);

                if (column.IsNullable != null
                    && !column.IsNullable.Value
                    && (column.DefaultValue == null)
                    && (string.IsNullOrWhiteSpace(column.DefaultValueSql))
                    && !column.IsIdentity
                    && !column.IsTimestamp
                    && !column.StoreType.ToLower().Equals("rowversion")
                    && !column.StoreType.ToLower().Equals("timestamp"))
                {
                    writer.Write(" DEFAULT ");

                    if (column.Type == PrimitiveTypeKind.DateTime)
                    {
                        writer.Write(Generate(DateTime.Parse("1900-01-01 00:00:00", CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        writer.Write(Generate((dynamic)column.ClrDefaultValue));
                    }
                }

                writer.WriteLine("END");



                Statement(writer);
            }
        }
    }
}
