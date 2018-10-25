using DbModel.Engine;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EngineDb:DbContext
    {
        public static string Name = "EngineLite";

        public EngineDb() : base(Name) { }
        
        public DbSet<bus_anchor> bus_anchors { get; set; }

        public DbSet<bus_tag> bus_tags { get; set; }

    }
}
