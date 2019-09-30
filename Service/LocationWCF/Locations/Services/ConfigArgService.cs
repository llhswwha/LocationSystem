using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using BLL;
using LocationServices.Converters;

namespace LocationServices.Locations.Services
{
    public interface IConfigArgService
    {
        bool AddConfigArg(ConfigArg config);

        bool EditConfigArg(ConfigArg config);

        bool DeleteConfigArg(ConfigArg config);

        ConfigArg GetConfigArg(int id);

        List<ConfigArg> GetConfigArgList();

        ConfigArg GetConfigArgByKey(string key);

        List<ConfigArg> FindConfigArgListByKey(string key);

        List<ConfigArg> FindConfigArgListByClassify(string key);

        TransferOfAxesConfig GetTransferOfAxesConfig();

        bool SetTransferOfAxesConfig(TransferOfAxesConfig config);
    }

    public class ConfigArgService : IConfigArgService
    {
        private Bll db;
        public ConfigArgService()
        {
            db = Bll.NewBllNoRelation();
        }
        public bool AddConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.Add(config.ToDbModel());
        }

        public bool DeleteConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.DeleteById(config.Id) != null;
        }

        public bool EditConfigArg(ConfigArg config)
        {
            return db.ConfigArgs.Edit(config.ToDbModel());
        }

        public List<ConfigArg> FindConfigArgListByClassify(string key)
        {
            return db.ConfigArgs.FindConfigArgListByClassify(key).ToWcfModelList();
        }

        public List<ConfigArg> FindConfigArgListByKey(string key)
        {
            return db.ConfigArgs.FindConfigArgListByKey(key).ToWcfModelList();
        }

        public ConfigArg GetConfigArg(int id)
        {
            return db.ConfigArgs.Find(id).ToTModel();
        }

        public ConfigArg GetConfigArgByKey(string key)
        {
            return db.ConfigArgs.GetConfigArgByKey(key).ToTModel();
        }

        public List<ConfigArg> GetConfigArgList()
        {
            return db.ConfigArgs.ToList().ToWcfModelList();
        }

        public TransferOfAxesConfig GetTransferOfAxesConfig()
        {
            var args = db.ConfigArgs.GetTransferOfAxesConfig();
            TransferOfAxesConfig config = new TransferOfAxesConfig();
            config.Zero = args.Find(i => i.Key == "TransferOfAxes.Zero").ToTModel();
            config.Scale = args.Find(i => i.Key == "TransferOfAxes.Scale").ToTModel();
            config.Direction = args.Find(i => i.Key == "TransferOfAxes.Direction").ToTModel();
            return config;
        }

        public bool SetTransferOfAxesConfig(TransferOfAxesConfig config)
        {
            return db.ConfigArgs.SetTransferOfAxesConfig(config.Zero.ToDbModel(), config.Scale.ToDbModel(),
               config.Direction.ToDbModel());
        }
    }
}
