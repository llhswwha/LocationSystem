using System;
using System.Collections.Generic;
using System.Text;
using DbModel.Tools;
using Location.IModel;

namespace IModel.Extensions
{
    public static class TreeNodeExtensions
    {
        //public static List<T> GetAllChildren<T>(this T node,AreaTypes? type) where T : ITreeNode<T>
        //{
        //    var allChildren = new List<T>();
        //    GetSubChildren(allChildren, node, type);
        //    return allChildren;
        //}

        //public static void GetSubChildren<T>(List<T> list, T node, AreaTypes? type = null)
        //{
        //    foreach (var child in node.Children)
        //    {
        //        if (type == null || type == child.Type)
        //        {
        //            list.Add(child);
        //        }
        //        GetSubChildren(list, child, type);
        //    }
        //}
    }
}
