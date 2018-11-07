using BLL;
using BLL.Tools;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LocationClient.Tools;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for DbConfigureWindow.xaml
    /// </summary>
    public partial class DbConfigureWindow : Window
    {
        public DbConfigureWindow()
        {
            InitializeComponent();
            //Debug.Listeners.Add(new TraceListener());
            Log.NewLogEvent += Log_NewLogEvent;
            Log.StartWatch();
            this.Closing += DbConfigureWindow_Closing   ;
        }

        private void DbConfigureWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.StopWatch();
            Log.NewLogEvent -= Log_NewLogEvent;
        }

        private void Log_NewLogEvent(string obj)
        {
            this.Dispatcher.Invoke(new Action(() => {
                TbConsole.Text = obj + "\n" + TbConsole.Text;
                //TbConsole.AppendText(obj);
            }));
        }

        private void MenuInitMSSql_Click(object sender, RoutedEventArgs e)
        {

            AppContext.InitDbAsync(0, 0,(bll)=>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");

            });
        }

        private void InitImage(Bll bll)
        {
            string strName = "顶视图";
            //string strInfo = "还贷款萨丹哈";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Images\\顶视图.png";
            byte[] byteArray = ImageHelper.LoadImageFile(path);
            bll.Pictures.Update(strName, byteArray);
        }

        private void MenuInitSqlite_Click(object sender, RoutedEventArgs e)
        {
            AppContext.InitDbAsync(1, 0, (bll) =>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");
            });
        }

        private void MenuInitTopo_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(()=>
            {
                AreaTreeInitializer initializer=new AreaTreeInitializer(new Bll());
                initializer.InitAreaAndDev();
                MessageBox.Show("完成");
            });
            thread.Start();
        }

        private void MenuRemoveArchor_Click(object sender, RoutedEventArgs e)
        {
            DevInfoHelper.RemoveArchorDev();
            MessageBox.Show("完成");
        }

        private void MenuImportDevs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuInitAA_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitAuthorization();
            MessageBox.Show("完成");
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitRealTimePositions();
            MessageBox.Show("完成");
        }

        private void MenuPersonAndCard_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitCardAndPerson();
            MessageBox.Show("完成");
        }
    }
}
