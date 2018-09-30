using System.Collections.Generic;
using Location.IModel;

namespace DbModel.Tools
{
    public static class TreeHelper
    {
        public static List<T> CreateTree<T, T2>(List<T> list, List<T2> leafNodes) where T : class, ITreeNodeEx<T, T2>, new() where T2 : class,INode, new()
        {
            List<T> roots = new List<T>();
            foreach (var item in list)
            {
                {
                    var parent = list.Find(i => i.Id == item.ParentId);
                    if (parent == null)
                    {
                        roots.Add(item);
                    }
                    else
                    {
                        if (parent.Children == null)
                        {
                            parent.Children = new List<T>();
                        }
                        parent.Children.Add(item);
                    }
                }               
            }
            if (leafNodes != null)
            {
                for (int i = 0; i < leafNodes.Count; i++)
                {
                    var parent = list.Find(k => k.Id == leafNodes[i].ParentId);
                    if (parent != null)
                    {
                        if (parent.LeafNodes == null)
                        {
                            parent.LeafNodes = new List<T2>();
                        }
                        parent.LeafNodes.Add(leafNodes[i]);
                    }
                }
            }
            return roots;
        }

        public static List<T> CreateTree<T>(List<T> list) where T : class, ITreeNode<T>, new()
        {
            List<T> roots = new List<T>();
            foreach (var item in list)
            {
                {
                    var parent = list.Find(i => i.Id == item.ParentId);
                    if (parent == null)
                    {
                        roots.Add(item);
                    }
                    else
                    {
                        if (parent.Children == null)
                        {
                            parent.Children = new List<T>();
                        }
                        parent.Children.Add(item);
                    }
                }

                
            }
            return roots;
        }

        public static IList<T> CloneTreeListEx<T, T2>(this IList<T> list) where T : class, ITreeNodeEx<T, T2>, new()
        {
            IList<T> list2 = list.CloneObjectList();
            foreach (T item in list2)
            {
                item.Children = null;
                item.LeafNodes = null;
            }
            return list2;
        }

        public static T CloneTreeRootEx<T, T2>(this T root) where T : class, ITreeNodeEx<T, T2>, new() where T2 : class, new()
        {
            T rootClone = root.CloneObject();
            if (root.Children != null)
            {
                if (root.Children.Count > 0)
                {
                    rootClone.Children = new List<T>();
                    foreach (T child in root.Children)
                    {
                        T childClone = CloneTreeRootEx<T, T2>(child);
                        rootClone.Children.Add(childClone);
                    }
                }
                else
                {
                    rootClone.Children = null;
                }

                if (root.LeafNodes.Count > 0)
                {
                    rootClone.LeafNodes = new List<T2>();
                    foreach (T2 child in root.LeafNodes)
                    {
                        T2 childClone = child.CloneObject();
                        rootClone.LeafNodes.Add(childClone);
                    }
                }
                else
                {
                    rootClone.LeafNodes = null;
                }
            }
            return rootClone;
        }

        public static IList<T> CloneTreeList<T>(this IList<T> list) where T : class, ITreeNode<T>, new()
        {
            IList<T> list2 = list.CloneObjectList();
            foreach (T item in list2)
            {
                item.Children = null;
            }
            return list2;
        }

        public static T CloneTreeRoot<T>(this T root) where T : class, ITreeNode<T>, new()
        {
            T rootClone = root.CloneObject();
            if (root.Children != null)
            {
                if (root.Children.Count > 0)
                {
                    rootClone.Children = new List<T>();
                    foreach (T child in root.Children)
                    {
                        T childClone = CloneTreeRoot(child);
                        rootClone.Children.Add(childClone);
                    }
                }
                else
                {
                    rootClone.Children = null;
                }
            }
            return rootClone;
        }
    }
}
