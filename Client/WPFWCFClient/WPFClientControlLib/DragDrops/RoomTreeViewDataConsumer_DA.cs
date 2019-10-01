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
    public partial class RoomTreeViewDataConsumer<TContainer, TObject>
    {

        private static void MoveRoom(Dep depNew, Room room)
        {
            //ThreadHelper.Run(() =>
            {
                room.PId = depNew.Id;
                RoomDataAccess roomDa = new RoomDataAccess();
                roomDa.Modify(room, false);
                depNew.AddRoom(room,true);
            }//)
            ;
        }

        private static void AddRoomDevToFolder(DeviceFolder folder, RoomDev roomDev)
        {
            //ThreadHelper.Run(() =>
            {
                DeviceFolderDataAccess folderDa = new DeviceFolderDataAccess();
                folderDa.AddNode(folder.Id, roomDev.Id);
                folder.AddChild(roomDev, false);//因为节点已经移动过去了，不用触发事件。
            }//)
            ;
        }

        /// <summary>
        /// 将机房设备从一个文件夹移除
        /// </summary>
        /// <param name="roomDev"></param>
        /// <param name="folderOld"></param>
        private static void RemoveRoomDevFromFolder(RoomDev roomDev, DeviceFolder folderOld)
        {
            //ThreadHelper.Run(() =>
            {
                DeviceFolderDataAccess folderDa = new DeviceFolderDataAccess();
                folderDa.RemoveNode(folderOld.Id, roomDev.Id);
                folderOld.RemoveChild(roomDev);
            }//)
            ;
        }

        private static void MoveRoomDevBetweenFolder(DeviceFolder folder, RoomDev roomDev, DeviceFolder folderOld)
        {
            //ThreadHelper.Run(() =>
            {
                DeviceFolderDataAccess folderDa = new DeviceFolderDataAccess();
                folderDa.MoveChild(folderOld, folder, roomDev);
            }//)
            ;
        }
    }
}
