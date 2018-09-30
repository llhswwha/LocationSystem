using System.Collections.Generic;

namespace Location.IModel
{
    /// <summary>
    /// 树节点
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public interface ITreeNode<TNode>: IEntityNode
    {
        //string Name { get; set; }
        List<TNode> Children { get; set; }
    }

    /// <summary>
    /// 树节点，有文件夹节点和文件节点（叶子节点）组成
    /// </summary>
    /// <typeparam name="TD">文件夹节点</typeparam>
    /// <typeparam name="TF">叶子节点</typeparam>
    public interface ITreeNodeEx<TD, TF>: ITreeNode<TD>
    {
        List<TF> LeafNodes { get; set; }
    }
}
