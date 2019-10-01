using Base.Common;
using DataObjects.DevDep;
using DataObjects.DeviceFolders;
using DataObjects.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using U3D.DataAccess.DAs;
using WpfDragAndDropSmorgasbord.DragDropFrameworkData;
using WpfFramework.Common.BusinessControls;
using WpfFramework.Common.BusinessControls.TreeViewItems;
using WpfFramework.Common.BusinessControls.TreeViews;

namespace WpfFramework.Common.DragDrops
{
    public partial class RoomTreeViewDataConsumer<TContainer, TObject> : TreeViewDataConsumer<TContainer, TObject>
        where TContainer : ItemsControl
        where TObject : ItemsControl
    {
        public RoomTreeViewDataConsumer(string[] dataFormats)
            : base(dataFormats)
        {
        }

        public override void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            //base.DragOverOrDrop(false, sender, e);
            if (e.Source is TreeViewItemRoomDev)
            {
                base.DragOverOrDrop(false, sender, e);
            }
            else
            {
                //e.Effects = DragDropEffects.None;
                //e.Handled = true;
            }
        }

        public override void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            if (!DragDropNode(sender, e, false))
            {
                base.DragOverOrDrop(false, sender, e);
            }
        }

        public override void DropTarget_Drop(object sender, DragEventArgs e)
        {
            if (!DragDropNode(sender, e, true))
            {
                base.DragOverOrDrop(true, sender, e);
            }
        }

        /// <summary>
        /// 拖动节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="isDrop">false:能否拖动,true:实际拖动</param>
        /// <returns></returns>
        private bool DragDropNode(object sender, DragEventArgs e,bool isDrop)
        {
            bool isCancel = true;

            TreeViewDataProvider<TContainer, TObject> dataProvider = GetData(e) as TreeViewDataProvider<TContainer, TObject>;
            if (dataProvider == null)
            {
                Log.Alarm("RoomTreeViewDataConsumer.DragDropNode", "dataProvider == null");
                return isCancel;
            }

            TContainer dragSourceContainer = dataProvider.SourceContainer as TContainer;
            TreeViewItemObject dragSourceObject = dataProvider.SourceObject as TreeViewItemObject;//拖动的源节点
            if (dragSourceObject == null)
            {
                Log.Alarm("RoomTreeViewDataConsumer.DragDropNode", "dragSourceObject == null");
                return isCancel;
            }
            TreeViewItemObject newParentNode = e.Source as TreeViewItemObject;//拖动的目标节点
            if (newParentNode == null)
            {
                Log.Alarm("RoomTreeViewDataConsumer.DragDropNode", "newParentNode == null");
                return isCancel;
            }
            dragSourceObject.IsDraging = true;
            newParentNode.IsDraging = true;

            //A.机房设备移入移出文件夹（分组）
            if (newParentNode is TreeViewItemFolder)//拖动机房设备到文件夹
            {
                DeviceFolder folder = newParentNode.Tag as DeviceFolder;
                if (dragSourceObject.Tag is RoomDev)//拖动的是机房设备
                {
                    RoomDev roomDev = dragSourceObject.Tag as RoomDev;
                    if (dragSourceObject.Parent is TreeViewItemRoom)//A.1 将机房设备移入文件夹
                    {
                        if (roomDev.PId == folder.PId)//必须是同一个机房内的操作
                        {
                            if (isDrop)
                            {
                                AddRoomDevToFolder(folder, roomDev);
                                //base.DragOverOrDrop(true, sender, e);
                            }
                            isCancel = false;
                        }
                    }
                    else if (dragSourceObject.Parent is TreeViewItemFolder)//A.2 将机房设备从一个文件夹移动到另一个文件夹
                    {
                        DeviceFolder folderOld = (dragSourceObject.Parent as TreeViewItemFolder).Tag as DeviceFolder;
                        if (roomDev.PId == folder.PId && folderOld.Id != folder.Id)
                        {
                            if (isDrop)
                            {
                                MoveRoomDevBetweenFolder(folder, roomDev, folderOld);
                                //base.DragOverOrDrop(true, sender, e);
                            }
                            isCancel = false;
                        }
                    }
                    else
                    {
                    }
                }
            }
            else if (newParentNode is TreeViewItemRoom)
            //1.将机房设备从文件夹中移动到机房下
            {
                Room room = newParentNode.Tag as Room;
                if (dragSourceObject.Tag is RoomDev)
                {
                    RoomDev roomDev = dragSourceObject.Tag as RoomDev;
                    if (dragSourceObject.Parent is TreeViewItemFolder)//A.3 将机房设备从一个文件夹移除
                    {
                        DeviceFolder folderOld = (dragSourceObject.Parent as TreeViewItem).Tag as DeviceFolder;
                        if (folderOld.PId == roomDev.PId && roomDev.PId == room.Id)//必须是同一个机房内的操作
                        {
                            if (isDrop)
                            {
                                RemoveRoomDevFromFolder(roomDev, folderOld);//将机房设备从一个文件夹移除
                                //base.DragOverOrDrop(true, sender, e);
                            }
                            isCancel = false;
                        }
                    }
                    else
                    {
                    }
                }
            }
            else if (e.Source is TreeViewItemDep)
            //1.将机房节点拖动到一个区域节点下
            {
                Dep depNew = newParentNode.Tag as Dep;//新区域
                if (dragSourceObject.Tag is Room)//拖动机房
                {
                    Room room = dragSourceObject.Tag as Room;
                    if (depNew.Id != room.PId)//不同区域节点
                    {
                        if (isDrop)
                        {
                            MoveRoom(depNew, room);
                        }
                        isCancel = false;
                    }
                }
                if (dragSourceObject.Tag is Dep)//拖动区域
                {
                    Dep dep = dragSourceObject.Tag as Dep;
                    if (depNew.Id != dep.PId)//不同区域节点
                    {
                        if (isDrop)
                        {
                            ThreadHelper.Run(() =>
                            {
                                dep.PId = depNew.Id;
                                DepDevDataAccess depDa = new DepDevDataAccess();
                                depDa.Modify(dep);
                                depNew.AddDep(dep);
                            });
                        }
                        isCancel = false;
                    }
                }
            }
            else
            {

            }

            dragSourceObject.IsDraging = false;
            newParentNode.IsDraging = false;

            return isCancel;
        }

        private T GetEntityData<T>(DragEventArgs e) where T :class
        {
            TreeViewDataProvider<TContainer, TObject> dataProvider = GetData(e) as TreeViewDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TreeViewItem dragSourceObject = dataProvider.SourceObject as TreeViewItem;
                if (dragSourceObject !=null )
                {
                    T entityData = dragSourceObject.Tag as T;
                    return entityData;
                }
            }
            return null;
        }

        private T GetEntityDataParent<T>(DragEventArgs e) where T : class
        {
            TreeViewDataProvider<TContainer, TObject> dataProvider = GetData(e) as TreeViewDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TreeViewItem dragSourceObject = dataProvider.SourceObject as TreeViewItem;
                
                if (dragSourceObject != null)
                {
                    TreeViewItem parent = dragSourceObject.Parent as TreeViewItem;
                    if (parent != null)
                    {
                        T entityData = parent.Tag as T;
                        return entityData;
                    }
                   
                }
            }
            return null;
        }
    }
}
