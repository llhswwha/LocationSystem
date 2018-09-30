using Location.IModel;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Web.Mvc
{
    public static class MyHtmlHelper
    {
        public static MvcHtmlString Tree<T1>(this HtmlHelper html, ITreeNode<T1> treeModel) where T1 : ITreeNode<T1>
        {
            return BindTree(treeModel);
        }

        private static MvcHtmlString BindTree<T1>(ITreeNode<T1> treeModel) where T1 : ITreeNode<T1>
        {
            StringBuilder sb = new StringBuilder();
            if (treeModel != null)
            {
                sb.Append("<ul>");

                List<T1> list = treeModel.Children.ToList();
                foreach (T1 item in list)
                {
                    sb.Append("<li>");
                    sb.Append(item.Name);
                    sb.Append("</li>");
                    sb.Append(BindTree(item));
                }
                sb.Append("</ul>");

            }
            MvcHtmlString mstr = new MvcHtmlString(sb.ToString());
            return mstr;
        }

        public static MvcHtmlString TreeEx<T1, T2>(this HtmlHelper html, ITreeNodeEx<T1, T2> treeModel) where T1 : ITreeNodeEx<T1,T2> where T2 : IName
        {
            return BindTreeEx(treeModel,0);
        }

        public static bool HaveChildren<T1, T2>(ITreeNodeEx<T1, T2> item) where T1 : ITreeNodeEx<T1, T2> where T2 : IName
        {
            return (item.Children != null && item.Children.Count > 0) || (item.LeafNodes != null && item.LeafNodes.Count > 0);
        }

        private static MvcHtmlString BindTreeEx<T1, T2>(ITreeNodeEx<T1, T2> treeModel,int layer) where T1 : ITreeNodeEx<T1, T2> where T2: IName
        {
            StringBuilder sb = new StringBuilder();
            if (treeModel != null)
            {

                List<T1> list = treeModel.Children.ToList();

                
                if (layer == 0)
                {
                    sb.Append(string.Format("<ul onclick='{0}'>", "displayOrHide(event);"));
                }
                else
                {
                    if (HaveChildren(treeModel))
                    {
                        sb.Append(string.Format("<ul class='collapse' id='List{0}'>", treeModel.Id));
                    }
                    else
                    {
                        sb.Append("<ul>");
                    }
                }


                foreach (T1 item in list)
                {
                    bool haveChildren = HaveChildren(item);
                    if (!haveChildren)
                    {
                        sb.Append("<li>");
                    }
                    else
                    {
                        sb.Append("<li style='list-style-image: url(\"../Img/TreeIcon/open.png\");cursor: pointer;'>");
                    }

                    sb.Append(item.Name);
                    sb.Append("</li>");
                    sb.Append(BindTreeEx(item, layer + 1));
                }

                    List<T2> leafs = treeModel.LeafNodes.ToList();
                    foreach (T2 leaf in leafs)
                    {
                        sb.Append("<li>");
                        sb.Append(leaf.Name);
                        sb.Append("</li>");
                    }

                    sb.Append("</ul>");
                }  

            
            MvcHtmlString mstr = new MvcHtmlString(sb.ToString());
            return mstr;
        }
    }
}