using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BLL;
using Location.BLL.Tool;
using DAL;
using Location.Model.InitInfos;
using DbModel.Tools;

namespace WebLocation.Controllers
{
    public class DataInitController : Controller
    {
        TestBll test=new TestBll();
        private LocationDb db = new LocationDb();
        // GET: DataInit
        public ActionResult InitAll()
        {
            return View();
        }

        // GET: DataInit
        public ActionResult InitPhysicalTopologys()
        {
            return View();
        }

        // GET: DataInit
        public ActionResult SaveInitInfoXml()
        {
            Bll bll = new Bll(false, false, false, false);
            AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
            initializer.SaveInitInfoXml();
            return View();
        }

        public ActionResult LoadInitInfoXml()
        {
            test.InitTopoFromXml();
            return View();
        }

        public ActionResult ClearTopoTable()
        {
            test.ClearTopoTable();
            return View();
        }

        public ActionResult InitTopo()
        {
            //Log.Info("导入土建KKS");
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //string filePath = basePath + "Data\\中电四会部件级KKS编码2017.5.24\\土建\\中电四会热电有限责任公司KKS项目-土建系统-B.xls";
            //KKSCodeHelper.ImportKKSFromFile<KKSCode>(new FileInfo(filePath));

            test.InitTopo();
            return View();
        }

        public ActionResult InitDb()
        {
            Log.InfoStart("InitDb");
            test.db.Db.Database.Initialize(true);
            Log.InfoEnd("InitDb");
            return View();
        }

        public ActionResult InitDbData()
        {
            Log.InfoStart("DataInitController.InitDbData");
            int mode = int.Parse(ConfigurationManager.AppSettings["DataInitMode"]);//0:EF,1:Sql
            test.InitDbData(mode,true);
            Log.InfoEnd("DataInitController.InitDbData");
            return View();
        }
    }
}
