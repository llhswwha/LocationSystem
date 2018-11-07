using System;
using System.Linq;
using Location.BLL.Tool;
using System.Threading;

namespace BLL
{
    public partial class Bll
    {

        public void Init(int mode)
        {
            InitDb();
            InitDbData(mode);
        }

        public void InitAsync(int mode,Action<Bll> callBack)
        {
            Thread thread = new Thread(() =>
              {
                  InitDb();
                  InitDbData(mode);
                  if (callBack != null)
                  {
                      callBack(this);
                  }
              });
            thread.Start();
        }

        public void InitDb()
        {
            Log.InfoStart("InitDb");
            //List<Department> list = Departments.ToList();
            int count = Departments.DbSet.Count();//调试时，第一次使用EF获取数据要占用15-20s的时间，部署后不会。
            Log.Info("Count:" + count);
            int count2 = DbHistory.U3DPositions.Count();
            Log.Info("Count2:" + count2);
            Log.InfoEnd("InitDb");
        }

        public bool HasData()
        {
            return Departments.ToList().Count > 0;
        }

        public void InitDbData(int mode, bool isForce = false)
        {
            DbInitializer initializer = new DbInitializer(this);
            initializer.InitDbData(mode, isForce);
        }
    }
}
