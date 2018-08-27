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
using System.Linq;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class GraphNode<T> : Node<T>
    {
		private List<int> costs;

		public GraphNode () : base ()
		{
		}

		public GraphNode (T value) : base (value)
		{
		}

		public GraphNode (T value, NodeList<T> neighbors) : base (value, neighbors)
		{
		}

		new public NodeList<T> Neighbors {
			get {
				if (base.Neighbors == null)
					base.Neighbors = new NodeList<T> ();

				return base.Neighbors;
			}
		}


		/* bidirectional graph of neighbors, added to manage updates */
		new public NodeList<T> BidirectionalNeighbors {
			get {
				if (base.BidirectionalNeighbors == null)
					base.BidirectionalNeighbors = new NodeList<T> ();

				return base.BidirectionalNeighbors;
			}
		}
		public List<int> Costs {
			get {
				if (costs == null)
					costs = new List<int> ();

				return costs;
			}
		}
	}

	public class Node<T>
	{
		//Private member-variables
		private T data;
		private NodeList<T> neighbors = new NodeList<T>();
		private NodeList<T> parents = new NodeList<T>();
        private Transform myTransform = null;
        private NodeList<T> renderList = new NodeList<T>();

		//private bool needsupdate = false;
		//default to false, set to true if this node or it's children need to be updated
		public Node ()
		{
		}

		public Node (T data) : this (data, null)
		{
		}

		public Node (T data, NodeList<T> neighbors) //note, might want to add a parameter for bidirectional neighbors but we can generat one if needed
		{
			this.data = data;
			this.neighbors = neighbors;
		}

        public T Value {
			get {
				return data;
			}
			set {
				data = value;
			}

		}


		public NodeList<T> Neighbors {
			get {
				return neighbors;
			}
			set {
				neighbors = value;
			}
		}
		public NodeList<T> BidirectionalNeighbors {
			get {
				return parents;
			}
			set {
				parents = value;
			}
		}
        public Transform mytransform
        {
            get { return myTransform; }
            set { myTransform = value; }
        }
        public NodeList<T> RenderList
        {
            get { return renderList; }
            set { renderList = value; }
        }
    }

	public class NodeList<T> : Collection<Node<T>>
	{
		public NodeList () : base ()
		{
		}

		public NodeList<T> intersect(NodeList<T> otherList)
		{
			List<Node<T>> listA = new List<Node<T>>();
			List<Node<T>> listB = new List<Node<T>>();

			foreach(Node<T> item in base.Items)
			{
				listA.Add(item);
			}

			foreach(Node<T> item in base.Items)
			{
				listB.Add(item);
			}

			List<Node<T>> listC = (List<Node<T>>) listA.Intersect<Node<T>>(listB);

			NodeList<T> result = new NodeList<T>();

			foreach(Node<T> item in listC)
			{
				result.Add(item);
			}

			return result;
		}

		internal bool checkForMGOmatch(MasterGeoObj mgo)
		{
			return checkForMGOmatch(new List<MasterGeoObj> { mgo });
		}

		internal bool checkForMGOmatch(List<MasterGeoObj> mgoList)
		{
			string[] listA = new string[Items.Count];
			string[] listB = new string[mgoList.Count];

			for (int i = 0; i < listA.Length; i++)
			{
				listA[i] = Items[i].Value.ToString();
			}

			for (int i = 0; i < listB.Length; i++)
			{
				listB[i] = mgoList[i].figName;
			}

			return listA.Intersect<string>(listB).Count() > 0;
		}

		internal MasterGeoObj findMGOmatch(List<MasterGeoObj> mgoList)
		{
			string[] listA = new string[Items.Count];
			string[] listB = new string[mgoList.Count];

			for (int i = 0; i < listA.Length; i++)
			{
				listA[i] = Items[i].Value.ToString();
			}

			for (int i = 0; i < listB.Length; i++)
			{
				listB[i] = mgoList[i].figName;
			}

			IEnumerable<string> temp = listA.Intersect<string>(listB);
			if (temp.Count() > 0)
			{
				string fname = temp.FirstOrDefault<string>();
				return mgoList[Array.IndexOf<string>(listB, fname)];
			}
			return null;
		}

		/**
		 * return true if any node in the list needs to be updated
		 */
		/*
        public bool NeedsUpdate ()
		{
			foreach (Node<T> n in base.Items) {
				if (n.NeedsUpdate ()) {
					return true;
				}
			}
			return false;
		}

		public void SetNeedsUpdate (bool value)
		{
			foreach (Node<T> n in base.Items) {
				n.SetNeedsUpdate (value);
	
			}
		}
        */

		public NodeList (int initialSize)
		{
			// Add the specified number of items
			for (int i = 0; i < initialSize; i++)
				base.Items.Add (default(Node<T>));
		}


        /// <summary>
        /// returns true if all objects in this list are also in the supplied list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool IsSubsetOf(NodeList<T> col)
        {
            foreach (Node<T> n in base.Items)
            {
                if (col.Contains(n) == false) { return false; }
            }
            return true;

        }
		public Node<T> FindByValue (T value)
		{
			//this line is killing us!
			return Items.Where(node => node.Value.Equals(value)).FirstOrDefault();		//returns first occurent of the value passed in. default is null if not within set

			//// search the list for the value
			//foreach (Node<T> node in Items)
			//	if (node.Value.Equals (value))
			//		return node;

			//// if we reached here, we didn't find a matching node
			//return null;
		}


        /// <summary>
        /// copies this list of nodes into the destination list
        /// </summary>
        public NodeList<T> Clone()
        {
            return (NodeList<T>)MemberwiseClone();
        }
    }
}	