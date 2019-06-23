using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL;
using DAL;
using DbModel;
using ExcelLib;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for DbBrowserWindow.xaml
    /// </summary>
    public partial class DbBrowserWindow : Window
    {
        public string Path = "";

        public DbBrowserWindow()
        {
            InitializeComponent();
            Path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\DbInfos\\";
        }

        private void DbBrowserWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Type type = typeof (LocationDb);
            var ps=type.GetProperties().Where(i=>i.PropertyType.Name.Contains("DbSet"));
            ListBox1.ItemsSource = ps;
            ListBox1.DisplayMemberPath = "Name";
            ListBox1.SelectionChanged += ListBox1_SelectionChanged;
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var db = GetDb();

            var p = ListBox1.SelectedItem as PropertyInfo;
            var list = GetList(p, db);
            DataGrid1.ItemsSource = list;

            if (list.Count > 0)
            {
                Type type = list[0].GetType();
                ExcelHelper.ExportList(list, new FileInfo(Path + type.Name+".xls"));
            }
            
        }

        private static List<object> GetList(PropertyInfo p, LocationDb db)
        {
            var ds = p.GetValue(db);
            //var ps = ds.GetType().GetProperties();
            //var ms = ds.GetType().GetMethods();
            var list = ds as IEnumerable;
            var list2 = new List<object>();
            foreach (var item in list)
            {
                //if(item is IComparable)
                //{
                //    list2.Add(item as IComparable);
                //}
                //else
                {
                    list2.Add(item);
                }
                
            }
            return list2;
        }

        private static LocationDb GetDb()
        {
            var db = new LocationDb();
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.LazyLoadingEnabled = false; //关闭延迟加载
            db.Configuration.ProxyCreationEnabled = false;
            return db;
        }

        private void MenuExportExcel_OnClick(object sender, RoutedEventArgs e)
        {
            var list=DataGrid1.ItemsSource as List<object>;
            //list.Sort();
            if (list.Count > 0)
            {
                Type type = list[0].GetType();
                ExcelHelper.ExportList(list, new FileInfo(Path + type.Name + ".xls"));
                MessageBox.Show("导出成功");
            }
        }

        private void MenuExportAll_OnClick(object sender, RoutedEventArgs e)
        {
            Type type = typeof(LocationDb);
            var ps = type.GetProperties().Where(i => i.PropertyType.Name.Contains("DbSet"));
            var db = GetDb();
            var ds = new DataSet();
            foreach (var p in ps)
            {
                var list = GetList(p, db);
                if (list.Count > 0)
                {
                    ExcelHelper.ExportList(list, new FileInfo(Path + list[0].GetType().Name + ".xls"));
                }
                var tb = ExcelHelper.ToTable(list);
                if (tb == null) continue;
                ds.Tables.Add(tb);
            }
            ExcelHelper.Save(ds, new FileInfo(Path + "DbInfos.xls"), null);
            MessageBox.Show("导出成功");
        }

        private void MenuOpenDir_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Path);
        }
    }
}
