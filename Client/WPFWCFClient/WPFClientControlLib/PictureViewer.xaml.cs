using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFClientControlLib
{
    /// <summary>
    /// PictureViewer.xaml 的交互逻辑
    /// </summary>
    public partial class PictureViewer : UserControl
    {
        public PictureViewer()
        {
            InitializeComponent();
        }

        private void BtnPre_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPic(id - 1);
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPic(id + 1);
        }

        private int id = 0;

        private List<FileInfo> fileList;
        private DirectoryInfo dir = null;

        public void Show(DirectoryInfo dir)
        {
            this.dir = dir;
            if (dir.Exists)
            {
                TbPath.Text = dir.FullName;
                GetFiles(dir);
                ShowPic(0);
            }
        }

        private void GetFiles(DirectoryInfo dir)
        {
            var files = dir.GetFiles();
            var list = files.ToList();
            list.Sort((a, b) =>
            {
                var r1 = a.Extension.CompareTo(b.Extension);
                if (r1 == 0)
                {
                    return a.LastWriteTime.CompareTo(b.LastWriteTime);
                }
                else
                {
                    return r1;
                }

            });
            fileList = list;
        }

        private void ShowPic(int i)
        {
            this.id = i;
            if (fileList == null) return;
            if (id < 0)
            {
                //id = 0;
                id = fileList.Count - 1;
            }
            if (id > fileList.Count - 1)
            {
                //id = fileList.Count - 1;
                id = 0;
            }

            if (fileList.Count == 0)
            {
                LbState.Content = string.Format("{0}/{1},{2}", 0, 0, "");
                Image1.Source = null;
            }
            else
            {
                currentFile = fileList[id];
                LbState.Content = string.Format("{0}/{1},{2}", id + 1, fileList.Count, currentFile.Name);
                //Image1.Source = new BitmapImage(new Uri(file.FullName));

                MediaElement1.Source=new Uri(currentFile.FullName);
            }
            
        }

        public FileInfo currentFile;

        private void BtnOpenDir_OnClick(object sender, RoutedEventArgs e)
        {
            if(File.Exists(dir.FullName))
            {
                Process.Start(dir.FullName);
            }else
            {
                MessageBox.Show("图片文件夹不存在！");
            }            
        }

        private void BtnDeleteFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定删除?", "确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    MediaElement1.Source = null;
                    if (currentFile != null)
                    {
                        currentFile.Delete();
                    }
                    GetFiles(dir);
                    ShowPic(id);
                    MessageBox.Show("成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
            else
            {
                MessageBox.Show("取消");
            }
            
        }
    }
}
