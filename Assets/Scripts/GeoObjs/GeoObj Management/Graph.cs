/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class Graph<T>
    {
        private NodeList<T> nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            nodeSet.Add(new GraphNode<T>(value));
        }

        /// <summary>
        /// This sets the transform reference.  Needs to be implemented in the Lifemanager
        /// </summary>
        /// <param name="node"></param>
        /// <param name="trans"></param>
        public void SetTransformRef(GraphNode<string> node, Transform trans)
        {
            node.mytransform = trans;
        }

        public void AddDirectedEdge(T fromT, T toT, int cost)
        {
            GraphNode<T> from = (GraphNode<T>)nodeSet.FindByValue(fromT);
            GraphNode<T> to = (GraphNode<T>)nodeSet.FindByValue(toT);

            from.Neighbors.Add(to);
            from.Costs.Add(cost);
            to.BidirectionalNeighbors.Add(from);
        }

        public NodeList<T> neighborsOfNode(T nodeNameT)
        {
            GraphNode<T> nodeName = (GraphNode<T>)nodeSet.FindByValue(nodeNameT);
            NodeList<T> neighborlist = nodeName.Neighbors;
            return neighborlist;
        }
        public NodeList<T> bidirectionalNeighborsOfNode(T nodeNameT)
        {
            GraphNode<T> nodeName = (GraphNode<T>)nodeSet.FindByValue(nodeNameT);
            NodeList<T> bneighborlist = nodeName.BidirectionalNeighbors;
            return bneighborlist;
          
        }

        public void AddUndirectedEdge(T fromT, T toT, int cost)
        {
            GraphNode<T> from = (GraphNode<T>)nodeSet.FindByValue(fromT);
            GraphNode<T> to = (GraphNode<T>)nodeSet.FindByValue(toT);

            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public GraphNode<T> findGraphNode(T name)
        {
            GraphNode<T> nodeName = (GraphNode<T>)nodeSet.FindByValue(name);
            return nodeName;
        }

		public bool Contains(T value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index >= 0) // don't use != -1 when you really mean zero or more
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
                index = gnode.BidirectionalNeighbors.IndexOf(nodeToRemove);
                if (index >= 0)
                {
                    gnode.BidirectionalNeighbors.RemoveAt(index);
                }
            }

            return true;
        }

        internal NodeList<T> findEulerPath()
        {
            Graph<T> tempVertexGraph = this;
            NodeList<T> result = new NodeList<T>();
            Node<T> currentNode = Nodes[0];
            for(int i = 0; i< Nodes.Count; i++)
            {
                result.Add(currentNode);
                tempVertexGraph.Remove(currentNode.Value);
                currentNode = tempVertexGraph.neighborsOfNode(currentNode.Value)[0];
            }
            return result;
        }

		public NodeList<T> Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        public int Count
        {
            get { return nodeSet.Count; }
        }
    }
}
