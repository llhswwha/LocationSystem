using System.Collections.Generic;
using System.Data.Entity;
using DbModel.Location.Person;
using SQLite.CodeFirst;
namespace DAL
{
    public class LocationDbLite : DbContext
    {
        public static string Name = "LocationLite";

        public LocationDbLite() : base(Name)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SqliteDropCreateDatabaseWhenModelChanges<LocationDbLite>(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }

    public class Book
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
