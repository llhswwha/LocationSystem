using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;
using Location.TModel.Location;

namespace BLL.Blls.Location
{
    public class ConfigArgBll : BaseBll<ConfigArg, LocationDb>
    {
        public ConfigArgBll():base()
        {

        }
        public ConfigArgBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.ConfigArgs;
        }

        public ConfigArg GetConfigArgByKey(string key)
        {
            var query = from item in DbSet where item.Key == key select item;
            return query.FirstOrDefault();
        }

        public List<ConfigArg> FindConfigArgListByKey(string key)
        {
            var query = from item in DbSet where item.Key.Contains(key) select item;
            return query.ToList();
        }

        public List<ConfigArg> FindConfigArgListByClassify(string key)
        {
            var query = from item in DbSet where item.Classify.Contains(key) select item;
            return query.ToList();
        }

        public List<ConfigArg> GetTransferOfAxesConfig()
        {
            List<ConfigArg> args = FindConfigArgListByKey("TransferOfAxes.");
            //TransferOfAxesConfig config = new TransferOfAxesConfig();
            //config.Zero = args.Find(i => i.Key == "TransferOfAxes.Zero");
            //config.Scale = args.Find(i => i.Key == "TransferOfAxes.Scale");
            //config.Direction = args.Find(i => i.Key == "TransferOfAxes.Direction");
            return args;
        }

        public bool SetTransferOfAxesConfig(ConfigArg zero, ConfigArg scale, ConfigArg direction)
        {
            bool r = Edit(zero);
            if (r == false) return false;
            r = Edit(scale);
            if (r == false) return false;
            r = Edit(direction);
            return r;
        }
    }
}
