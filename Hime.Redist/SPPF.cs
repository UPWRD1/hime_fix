﻿namespace Hime.Redist.Parsers
{

    public class SPPFNode
    {
        protected ISymbol p_Symbol;
        protected SyntaxTreeNodeAction p_Action;
        protected int p_Generation;
        protected System.Collections.Generic.List<SPPFNodeFamily> p_Families;

        public ISymbol Symbol { get { return p_Symbol; } }
        public SyntaxTreeNodeAction Action
        {
            get { return p_Action; }
            set { p_Action = value; }
        }
        public int Generation { get { return p_Generation; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily> Families { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily>(p_Families); } }

        public SPPFNode(ISymbol symbol, int gen)
        {
            p_Symbol = symbol;
            p_Action = SyntaxTreeNodeAction.Nothing;
            p_Generation = gen;
            p_Families = new System.Collections.Generic.List<SPPFNodeFamily>();
        }
        public SPPFNode(ISymbol symbol, int gen, SyntaxTreeNodeAction action)
        {
            p_Symbol = symbol;
            p_Action = action;
            p_Generation = gen;
            p_Families = new System.Collections.Generic.List<SPPFNodeFamily>();
        }

        public void AddFamily(System.Collections.Generic.List<SPPFNode> nodes) { p_Families.Add(new SPPFNodeFamily(this, nodes)); }
        public SPPFNodeFamily NewFamily() {
            SPPFNodeFamily family = new SPPFNodeFamily(this);
            p_Families.Add(family);
            return family;
        }

        public bool EquivalentTo(SPPFNode node)
        {
            if (this.p_Symbol != node.p_Symbol)
                return false;
            return (this.p_Generation == node.p_Generation);
        }

        public bool HasEquivalentFamily(System.Collections.Generic.List<SPPFNode> nodes)
        {
            foreach (SPPFNodeFamily family in p_Families)
                if (family.EquivalentTo(nodes))
                    return true;
            return false;
        }

        public SyntaxTreeNode GetFirstTree()
        {
            SyntaxTreeNode me = new SyntaxTreeNode(p_Symbol, p_Action);
            if (p_Families.Count >= 1)
            {
                foreach (SPPFNode child in p_Families[0].Children)
                    me.AppendChild(child.GetFirstTree());
            }
            return me;
        }
    }

    public class SPPFNodeFamily
    {
        protected SPPFNode p_Parent;
        protected System.Collections.Generic.List<SPPFNode> p_Children;

        protected SPPFNode Parent { get { return p_Parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode> Children { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode>(p_Children); } }

        public SPPFNodeFamily(SPPFNode parent)
        {
            p_Parent = parent;
            p_Children = new System.Collections.Generic.List<SPPFNode>();
        }
        public SPPFNodeFamily(SPPFNode parent, System.Collections.Generic.List<SPPFNode> nodes)
        {
            p_Parent = parent;
            p_Children = new System.Collections.Generic.List<SPPFNode>(nodes);
        }

        public void AddChild(SPPFNode child) { p_Children.Add(child); }

        public bool EquivalentTo(System.Collections.Generic.List<SPPFNode> nodes)
        {
            if (p_Children.Count != nodes.Count)
                return false;
            for (int i = 0; i != p_Children.Count; i++)
                if (!p_Children[i].EquivalentTo(nodes[i]))
                    return false;
            return true;
        }
    }
}