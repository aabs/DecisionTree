using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class Tree<TNodeType, TEdgeLabelType>
        where TNodeType : IEquatable<TNodeType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        /// <summary>
        /// A link to the root node of the object graph that is the tree.
        /// </summary>
        /// <remarks>
        /// The <see cref="Tree{TEdgeLabelType, TNodeType}"/> type is really just a holder 
        /// for the actual tree which is the set of <see cref="TreeNode{TNodeType}"/> 
        /// and <see cref="TreeEdge{TEdgeLabelType}"/>
        /// </remarks>
        public TreeEdge<TNodeType, TEdgeLabelType> Root { get; set; }
        public IEnumerable<TreeNode<TNodeType, TEdgeLabelType>> Nodes
        {
            get
            {
                return GetAllNodes(Root.TargetNode);
            }
        }

        private IEnumerable<TreeNode<TNodeType, TEdgeLabelType>> GetAllNodes(TreeNode<TNodeType, TEdgeLabelType> n)
        {
            yield return n;
            var childNodes = n.Children.SelectMany(c => GetAllNodes(c.TargetNode));
            foreach (var cn in childNodes)
            {
                yield return cn;
            }
        }
    }


    public class TreeNode<TNodeType, TEdgeLabelType>
        where TNodeType : IEquatable<TNodeType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public IEnumerable<TreeEdge<TNodeType, TEdgeLabelType>> Parents { get; }
        public TNodeType Value { get; set; }
        List<TreeEdge<TNodeType, TEdgeLabelType>> children = new List<TreeEdge<TNodeType, TEdgeLabelType>>();

        public IEnumerable<TreeEdge<TNodeType, TEdgeLabelType>> Children
        {
            get
            {
                return children;
            }
        }

        public TreeNode<TNodeType, TEdgeLabelType> AddChild(TNodeType n, TEdgeLabelType e)
        {
            if (children.Any(c => c.LinkLabel.Equals(e)))
            {
                throw new ApplicationException("Child link already exists.  Consider using Replace operation instead.");
            }

            var treeNode = new TreeNode<TNodeType, TEdgeLabelType>
            {
                Value = n
            };
            children.Add(new TreeEdge<TNodeType, TEdgeLabelType>
                {
                LinkLabel = e,
                TargetNode = treeNode
                });
            return treeNode;
        }
    }

    public class TreeEdge<TNodeType, TEdgeLabelType>
        where TNodeType : IEquatable<TNodeType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public TEdgeLabelType LinkLabel { get; set; }
        public TreeNode<TNodeType, TEdgeLabelType> TargetNode { get; set; }
    }
}
