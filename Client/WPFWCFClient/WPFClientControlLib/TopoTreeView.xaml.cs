﻿using Location.TModel.Location.AreaAndDev;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DbModel.Location.AreaAndDev;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for TopoTreeView.xaml
    /// </summary>
    public partial class TopoTreeView : UserControl
    {
        public TopoTreeView()
        {
            InitializeComponent();
        }

        public void LoadData(PhysicalTopology root)
        {
            

            if (root == null)
            {
                TreeView1.ItemsSource = null;
            }
            else
            {
                TreeView1.ItemsSource = root.Children;
            }
        }

        public void LoadData(Area root,bool onlyBuilding=false)
        {
            //if (root == null)
            //{
            //    TreeView1.ItemsSource = null;
            //}
            //else
            //{
            //    if (onlyBuilding)
            //    {
            //        TreeView1.ItemsSource = root.GetBuildings();
            //    }
            //    else
            //    {
            //        TreeView1.ItemsSource = root.Children;
            //    }
            //}

            if (root == null)
            {
                ShowTree(TreeView1, null);
            }
            else
            {
                if (onlyBuilding)
                {
                    ShowTree(TreeView1, root.GetBuildings());
                    
                }
                else
                {
                    ShowTree(TreeView1, root.Children);
                }
            }
        }

        public void SelectFirst()
        {
            if(TreeView1.Items.Count>0)
                (TreeView1.Items[0] as TreeViewItem).IsSelected = true;
        }

        public void ShowTree(ItemsControl control,List<Area> list)
        {
            control.Items.Clear();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();
                node.Header = item.Name;
                node.Tag = item;
                control.Items.Add(node);
                ShowTree(node, item.Children);
            }
        }

        public void ExpandLevel(int level)
        {
            ExpandChildren(TreeView1, level);
            this.UpdateLayout();
        }

        public void ExpandChildren(ItemsControl control, int level)
        {
            if (level <= 0) return;
            foreach (var item in control.Items)
            {
                TreeViewItem node = item as TreeViewItem;
                //if(node==null)
                //    node = control.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;//子节点就不行了，不知道为什么
                if (node == null) continue;
                node.IsExpanded = true;
                ExpandChildren(node, level - 1);
            }
            this.UpdateLayout();
        }

        private void TreeView1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }

        public TreeView Tree
        {
            get
            {
                return TreeView1;
            }
        }

        public object SelectedObject
        {
            get
            {
                TreeViewItem node = TreeView1.SelectedItem as TreeViewItem;
                if (node != null)
                {
                    return node.Tag;
                }
                return TreeView1.SelectedItem;
            }
        }
    }
}
