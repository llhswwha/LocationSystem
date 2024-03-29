﻿using BLL;
using BLL.Initializers;
using LocationServer.Windows.Simple;
using LocationServices.Converters;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for AreaInfoWindow.xaml
    /// </summary>
    public partial class AreaInfoWindow : Window
    {
        public AreaInfoWindow()
        {
            InitializeComponent();
        }

        private void ArchorListWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private object _item;

        public void ShowInfo(TEntity item)
        {
            _item = item;
            PropertyGrid1.SelectedObject = null;
            PropertyGrid1.SelectedObject = _item;

            if (item.Parent != null)
            {
                this.Title = item.Parent.Name + "->" + item.Name;
            }
            else
            {
                this.Title = item.Name;
            }
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            var areaService = new AreaService();
            var area = areaService.Put(_item as TEntity);
            if (area == null)
            {
                MessageBox.Show("保存失败");
            }
            else
            {
                MessageBox.Show("保存成功");
            }

            var areaT = _item as TEntity;
            var bll = BLL.Bll.NewBllNoRelation();
            areaT.InitBound.IsRelative = areaT.IsRelative;
            bll.Bounds.Edit(areaT.InitBound.ToDbModel());
        }

        private void MenuInitBound_Click(object sender, RoutedEventArgs e)
        {
            var area = _item as TEntity;
            var win = new BoundWindow();
            //win.Bound = area.InitBound;
            win.Area = area;
            win.Show();
            win.AreaModified += Win_AreaModified;
        }

        private void Win_AreaModified(TEntity area)
        {
            ShowInfo(area);
            if (AreaModified != null)
            {
                AreaModified(area);
            }
        }

        public event Action<TEntity> AreaModified;

        private void MenuSetAlarmArea_Click(object sender, RoutedEventArgs e)
        {
            var area = _item as TEntity;
            DbInitializer initializer = new DbInitializer(AppContext.GetLocationBll());
            initializer.SetAlamArea(area.Id);
        }
    }
}
