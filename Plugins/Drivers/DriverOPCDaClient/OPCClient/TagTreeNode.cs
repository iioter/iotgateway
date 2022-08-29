using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automation.OPCClient
{
    public class TagTreeNode
    {
        public string Name
        {
            get;
            set;
        }

        private List<TagTreeNode> nodes = new List<TagTreeNode>();

        public List<TagTreeNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public TagTreeNode Parent
        {
            get;
            private set;
        }

        public bool IsLeaf { get; set; }

        public string FullPath 
        {
            get
            {
                string res = Name;
                TagTreeNode parent = this.Parent;
                while (parent != null)
                {
                    res = string.Format("{0}/{1}", parent.Name, res);
                    parent = parent.Parent;
                }
                return res;
            }
        }

        public TagTreeNode()
        { }

        public TagTreeNode(string name)
        {
            this.Name = name;
        }

        public TagTreeNode AddNode(string name)
        {
            TagTreeNode node = new TagTreeNode(name);
            node.Parent = this;
            this.nodes.Add(node);
            return node;
        }
    }
}
