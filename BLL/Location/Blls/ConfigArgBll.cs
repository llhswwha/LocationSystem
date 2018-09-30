using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model;
using Location.Model.Locations;
using Location.Model.Tools;

namespace Location.BLL.Blls
{
    public class ConfigArgBll : BaseBll<ConfigArg, LocationDb>
    {
        public ConfigArgBll():base()
        {

        }

        public ConfigArgBll(LocationDb db):base(db)
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
            return query.ToList().ToWCFList();
        }

        public List<ConfigArg> FindConfigArgListByClassify(string key)
        {
            var query = from item in DbSet where item.Classify.Contains(key) select item;
            return query.ToList().ToWCFList();
        }

        public TransferOfAxesConfig GetTransferOfAxesConfig()
        {
            TransferOfAxesConfig config = new TransferOfAxesConfig();
            List<ConfigArg> args = FindConfigArgListByKey("TransferOfAxes.");
            config.Zero = args.Find(i => i.Key == "TransferOfAxes.Zero");
            config.Scale = args.Find(i => i.Key == "TransferOfAxes.Scale");
            config.Direction = args.Find(i => i.Key == "TransferOfAxes.Direction");
            return config;
        }

        public bool SetTransferOfAxesConfig(TransferOfAxesConfig config)
        {
            bool r = Edit(config.Zero);
            if (r == false) return false;
            r = Edit(config.Scale);
            if (r == false) return false;
            r = Edit(config.Direction);
            return r;
        }
    }
}
