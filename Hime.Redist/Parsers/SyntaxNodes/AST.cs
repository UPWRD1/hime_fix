﻿using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Specifies the tree action for a given node
    /// </summary>
    public enum SyntaxTreeNodeAction
    {
        /// <summary>
        /// Promote the node to the immediately upper level in the tree
        /// </summary>
        Promote,
        /// <summary>
        /// Drop the node and all the children from the tree
        /// </summary>
        Drop,
        /// <summary>
        /// Replace the node by its children
        /// </summary>
        Replace,
        /// <summary>
        /// Default action for a node, do nothing
        /// </summary>
        Nothing
    }

    /// <summary>
    /// Represents an abstract syntax tree node
    /// </summary>
    public sealed class SyntaxTreeNode
    {
        private Dictionary<string, object> properties;
        private List<SyntaxTreeNode> children;
        private SyntaxTreeNode parent;
        private Symbol symbol;
        private SyntaxTreeNodeAction action;
        private System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> readonlyChildren;

        /// <summary>
        /// Gets a dictionary of user properties attached to this node
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get
            {
                if (properties == null)
                    properties = new Dictionary<string, object>();
                return properties;
            }
        }
        /// <summary>
        /// Gets the symbol attached to this node
        /// </summary>
        public Symbol Symbol { get { return symbol; } }
        /// <summary>
        /// Gets the parent node
        /// </summary>
        public SyntaxTreeNode Parent { get { return parent; } }
        /// <summary>
        /// Gets a read-only list of the children nodes
        /// </summary>
        public IList<SyntaxTreeNode> Children
        {
            get
            {
                if (readonlyChildren == null)
                    readonlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(children);
                return readonlyChildren;
            }
        }

        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        public SyntaxTreeNode(Symbol symbol)
        {
            this.children = new List<SyntaxTreeNode>();
            this.symbol = symbol;
            this.action = SyntaxTreeNodeAction.Nothing;
        }
        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol and action
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        /// <param name="action">The action for this node</param>
        public SyntaxTreeNode(Symbol symbol, SyntaxTreeNodeAction action)
        {
            this.children = new List<SyntaxTreeNode>();
            this.symbol = symbol;
            this.action = action;
        }

        /// <summary>
        /// Adds a node as a child after removing it from its original tree if needed
        /// </summary>
        /// <param name="node">The node to append</param>
        public void AppendChild(SyntaxTreeNode node)
        {
            if (node.parent != null)
                node.parent.children.Remove(node);
            node.parent = this;
            children.Add(node);
        }
        /// <summary>
        /// Adds a node as a child with the given action after removing it from its original tree if needed
        /// </summary>
        /// <param name="node">The node to append</param>
        /// <param name="action">The action for the node</param>
        public void AppendChild(SyntaxTreeNode node, SyntaxTreeNodeAction action)
        {
            if (node.parent != null)
                node.parent.children.Remove(node);
            node.parent = this;
            node.action = action;
            children.Add(node);
        }
        /// <summary>
        /// Adds a range of nodes as children
        /// </summary>
        /// <param name="nodes">The nodes to append</param>
        public void AppendRange(ICollection<SyntaxTreeNode> nodes)
        {
            List<SyntaxTreeNode> Temp = new List<SyntaxTreeNode>(nodes);
            foreach (SyntaxTreeNode Node in Temp)
                AppendChild(Node);
        }

        private class StackNode
        {
            public SyntaxTreeNode astNode;
            public bool visited;
            public StackNode parentNode;
            public StackNode(SyntaxTreeNode ast, StackNode parent)
            {
                this.astNode = ast;
                this.visited = false;
                this.parentNode = parent;
            }
        }

        /// <summary>
        /// Apply actions to this node and all its children
        /// </summary>
        /// <returns>The new root</returns>
        internal SyntaxTreeNode ApplyActions()
        {
            LinkedList<StackNode> stack = new LinkedList<StackNode>();
            stack.AddLast(new StackNode(this, null));
            StackNode current = null;

            while (stack.Count != 0)
            {
                current = stack.Last.Value;
                if (current.visited)
                {
                    stack.RemoveLast();
                    // post-order
                    // Drop replaced node
                    if (current.astNode.action == SyntaxTreeNodeAction.Replace)
                        continue;
                    if (current.astNode.action == SyntaxTreeNodeAction.Promote)
                    {
                        StackNode parentNode = current.parentNode;
                        SyntaxTreeNode oldParent = parentNode.astNode;
                        current.astNode.action = oldParent.action;
                        if (current.astNode.parent == oldParent)
                        {
                            current.astNode.parent = oldParent.parent;
                            foreach (SyntaxTreeNode left in oldParent.children)
                                left.parent = current.astNode;
                            current.astNode.children.InsertRange(0, oldParent.children);
                        }
                        else
                        {
                            current.astNode.parent = oldParent.parent;
                            current.astNode.children.Insert(0, oldParent);
                            oldParent.parent = current.astNode;
                        }
                        parentNode.astNode = current.astNode;
                    }
                    else
                    {
                        if (current.parentNode != null)
                        {
                            current.astNode.parent = current.parentNode.astNode;
                            current.astNode.parent.children.Add(current.astNode);
                        }
                    }
                }
                else
                {
                    current.visited = true;
                    // Pre-order
                    for (int i = current.astNode.children.Count - 1; i != -1; i--)
                    {
                        StackNode parentToPush = current;
                        SyntaxTreeNode child = current.astNode.children[i];
                        // prepare replace => setup parency
                        if (current.astNode.action == SyntaxTreeNodeAction.Replace)
                        {
                            child.parent = current.astNode.parent;
                            parentToPush = current.parentNode;
                        }
                        // if action is drop => drop the child now by not adding it to the stack
                        if (child.action == SyntaxTreeNodeAction.Drop)
                            continue;
                        else if (child.symbol is SymbolTokenText)
                        {
                            SymbolTokenText TokenText = (SymbolTokenText)child.symbol;
                            if (TokenText.SubGrammarRoot != null)
                            {
                                // there is a subgrammar => build parency and add to the stack
                                child = TokenText.SubGrammarRoot;
                                child.parent = current.astNode;
                            }
                        }
                        stack.AddLast(new StackNode(child, parentToPush));
                    }
                    // clear the children => rebuild in postorder
                    current.astNode.children.Clear();
                }
            }
            return current.astNode;
        }


        /// <summary>
        /// Builds an XML node representing this node in the context of the given XML document
        /// </summary>
        /// <param name="doc">The document that will own the XML node</param>
        /// <returns>The XML node representing this node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode Node = doc.CreateElement(symbol.Name);
            if (symbol is SymbolToken)
            {
                SymbolToken Token = (SymbolToken)symbol;
                Node.AppendChild(doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = doc.CreateAttribute(Property);
                Attribute.Value = properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (SyntaxTreeNode Child in children)
                Node.AppendChild(Child.GetXMLNode(doc));
            return Node;
        }

        /// <summary>
        /// Gets a string representation of this node
        /// </summary>
        /// <returns>The name of the current node's symbol; or "null" if there the node does not have a symbol</returns>
        public override string ToString()
        {
            if (symbol != null)
                return symbol.ToString();
            else
                return "null";
        }
    }
}