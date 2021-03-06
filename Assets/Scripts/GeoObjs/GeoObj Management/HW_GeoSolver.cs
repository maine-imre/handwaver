using Leap.Unity.Interaction;

using System.Linq;
using System.Collections.Generic;
/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System;

namespace IMRE.HandWaver.Solver
{
/// <summary>
/// The heart of Greg's geometery kernel.
/// Holds an node record of every geo obj
/// Uses basic graph theory to find a path to update all nececssary objects.
/// Will be depreciated with new geometery kernel.
/// Depending on constraints either doesn't iterate enough to have a reliable image (current) or doesn't terminate and figures drift into space.
/// </summary>
	public class HW_GeoSolver : MonoBehaviour
    {
        #region PositionTransformations
        Quaternion globalRotation = Quaternion.identity;
        float globalScale
		{
			get
			{
				return 1f;
				//we need an event so that this updates on the fly!  (the value does but we need to re-render everything).
				//return Interface.worldScaleModifier.ins.AbsoluteScale;
			}
		}
        Vector4 globalTranslate4 = Vector4.zero;
        Vector3 globalTranslate = Vector3.zero;

        public Vector3 localPosition(Vector3 systemPosition)
        {
            return globalRotation * (globalScale * systemPosition + globalTranslate);
        }

        public Vector3 systemPosition(Vector3 localPosition)
        {
            return ((Quaternion.Inverse(globalRotation) * localPosition - globalTranslate) / globalScale);
        }
#endregion

        public enum IntersectionMode { glue, intersect, snap, none };

        internal IntersectionMode thisMode = IntersectionMode.none;

		public enum InteractionMode { rigid, freeform, lattice };
		public InteractionMode thisInteractionMode = InteractionMode.freeform;

		private int _currentIndex = 4;
		public Graph<string> geomanager = new Graph<string>();

        public NodeList<string> rManList = new NodeList<string>();
        private NodeList<string> updateNodeList = new NodeList<string>();

        //public Transform rControll;
        //public Transform lControll;


        public bool blockDelete = false;
		public Material standardMaterial;
		public Color defaultColor;
		public Color selectedColor;
		public Color activeColor;
		public Color canidateColor;
		public Color staticColor;
		public Color dependentColor;


		public static HW_GeoSolver ins;

		public int currentIndex
		{
			get
			{
				return _currentIndex++;
			}
		}

		// Use this for initialization
		void Start()
        {
			ins = this;
			AbstractGeoObj.parentGeoObj = globalParentObj;
			//we need to have an event here that tells us to log the scale change.
			Interface.worldScaleModifier.ins.OnGestureDeactivated += logScaleChange;
        }

		public Transform toolbox;
		public Transform globalParentObj;

		private void LoadToolbox()
		{
			toolbox.gameObject.SetActive(true);
		}

		public void addToReactionManager(Node<string> node)
        {
            if (!rManList.Contains(node))
            {
                rManList.Add(node);
            }
        }

        public void LateUpdate()
        {
            //before calculating other figures, update intersection figures.

            intersectionManager.ins.UpdateIntersections(rManList);

            if (rManList.Count > 0)
            {
                updateNodeList = new NodeList<string>();
                updateNodeList = rManList.Clone();

                ReactionManager(rManList, rManList);
                rManList.Clear();
            }
        }

		private void logScaleChange()
		{
			ScaleHasChanged = true;
		}
		private bool ScaleHasChanged = false;

		/// <summary>
		/// During update, this recursively handles chagnes to other figures from the user's input.
		/// It goes up and down the dependency tree.
		/// </summary>
		/// <param name="nodeList"></param>
		/// <param name="newItems">List of items that returned true or ween't in the nodeList</param>
		public void ReactionManager(NodeList<string> nodeList, NodeList<string> LastPassTrue)
        {
            if (nodeList == null)
            {
				if (ScaleHasChanged)
				{
					ScaleHasChanged = false;
					GameObject.FindObjectsOfType<AbstractGeoObj>().ToList().ForEach(mgo => mgo.updateFigure());
				}
                return;
            }
            NodeList<string> thisPassTrue = new NodeList<string>();
            NodeList<string> thisPassFalse = new NodeList<string>();
            NodeList<string> newNodeList = new NodeList<string>();

            LastPassTrue.Where(n => n.mytransform.gameObject.activeSelf == true).ToList().ForEach(p => newNodeList.Add(p));
            newNodeList.ToList().ForEach(p => CheckIfNeighborsAreAffected(newNodeList, p.Neighbors, thisPassTrue, thisPassFalse, updateNodeList));
            newNodeList.ToList().ForEach(p => CheckIfNeighborsAreAffected(newNodeList, p.BidirectionalNeighbors, thisPassTrue, thisPassFalse, updateNodeList));


            if (!newNodeList.IsSubsetOf(nodeList))// if this node list is not a subset of the original it contains additional nodes and we must iterate again
            {
                ReactionManager(newNodeList, thisPassTrue);
                // here we pray that it will eventually converge and stop!
                // well, based on the persistant stack overflows from this line of code it appears that your prayers went unanswered
                // those are gone now, so now we just pray for less than 11 ms runtime...
            }
            else
            {
				//try moving this to the top.
				//intersectionManager.ins.UpdateIntersections(updateNodeList);
				//;for some reason we neglect figures that are built on intersections.
				//this needs to be refactored to consider that inersections are not necessairly the end.
				updateManager(updateNodeList);
				if (ScaleHasChanged)
				{
					ScaleHasChanged = false;
					GameObject.FindObjectsOfType<AbstractGeoObj>().ToList().ForEach(mgo => mgo.updateFigure());
				}
			}
        }

        /// <summary>
        /// checks if any of the items in the second list need to react to items in the first list
        /// recursively adds items from second list to first list if they need to react
        /// </summary>
        /// <param name="nodesmovinglist"></param>
        /// <param name="potentiallyaffectednodes"></param>
        private static void CheckIfNeighborsAreAffected(NodeList<string> nodesmovinglist, NodeList<string> potentiallyaffectednodes, NodeList<string> newItems, NodeList<string> falseItems, NodeList<string> updateNodeList)
        {
            if (potentiallyaffectednodes != null)
            {
                foreach (Node<string> neighbor in potentiallyaffectednodes.Where(p => !(newItems.Contains(p) || falseItems.Contains(p))))
                {
                    UpdatableFigure neighborUF = (UpdatableFigure)neighbor.mytransform.GetComponent<AbstractGeoObj>();

                    if (neighborUF.reactMotion(nodesmovinglist))
                    {
                        updateNodeList.Add(neighbor);
                        nodesmovinglist.Add(neighbor);
                        newItems.Add(neighbor);
                    }
                    else
                    {
                        falseItems.Add(neighbor);
                    }

                }
            }
        }

        /// <summary>
        /// Calls for each figure that needs to be updated to have it's vertex and triangles recalculated.
        /// </summary>
        /// <param name="rManNodeList"></param>
        public void updateManager(NodeList<string> rManNodeList)
        {
            List<Node<string>> NeedUpdate = rManNodeList.ToList();

            rManNodeList.ToList().ForEach(n => NeedUpdate.AddRange(n.RenderList));
            NeedUpdate.ForEach(n => n.mytransform.GetComponent<AbstractGeoObj>().updateFigure());
        }

        internal void addComponent(AbstractGeoObj geoComp)
        {
            //set index
            GeoObjType type = geoComp.figType;
            geoComp.figIndex = currentIndex;
            string newmyNAME = type.ToString() + _currentIndex.ToString("000");//changed to include padding for ordering the objects nicely
            geoComp.figName = newmyNAME;

            geoComp.gameObject.name = newmyNAME;

            geomanager.AddNode(newmyNAME);
            // set transform reference for graph node to make reactionManager more efficient.

            geomanager.SetTransformRef(geomanager.findGraphNode(newmyNAME), geoComp.transform);
        }

        ///// <summary>
        ///// Adds a dependence in the lifemanager and updatemanager and reactionmanager system
        ///// </summary>
		internal void AddDependence(AbstractGeoObj fromMGO, AbstractGeoObj toMGO)
		{
			//Debug.Log("adding directed edge from " + fromMGO.GetComponent<AbstractGeoObj>().figName + " to " + toMGO.GetComponent<AbstractGeoObj>().figName);
			if(string.IsNullOrEmpty(fromMGO.figName))
			{
				fromMGO.InitializeFigure();
			}
			if (string.IsNullOrEmpty(toMGO.figName))
			{
				toMGO.InitializeFigure();
			}
			geomanager.AddDirectedEdge(fromMGO.figName, toMGO.figName, 1);
		}

		/// <summary>
		/// Removes a dependence in the lifemanager and updatemanager and reactionmanager system. Does not delete the object
		/// </summary>
		/// <param name="geoComp1">GeoObj that will be removed from geoComp2 neighbor list</param>
		/// <param name="geoComp2">GeoObj that will be removed from geoComp1 neighbor list</param>
		public void removeDependence(Transform geoComp1, Transform geoComp2)
		{
			if (geoComp1.GetComponent<AbstractGeoObj>() != null)
			{
				GraphNode<string> fromNode = geomanager.findGraphNode(geoComp1.GetComponent<AbstractGeoObj>().figName);
				GraphNode<string> toNode = geomanager.findGraphNode(geoComp2.GetComponent<AbstractGeoObj>().figName);

				fromNode.Neighbors.Remove(toNode);
				fromNode.BidirectionalNeighbors.Remove(toNode);
				toNode.Neighbors.Remove(fromNode);
				toNode.BidirectionalNeighbors.Remove(fromNode);
			}
		}

		internal void removeComponent(AbstractGeoObj geoComp)
        {
			Debug.Log(geoComp.figName + " is being removed from the graph. Refer to trace for reason.");

			if (blockDelete == false)
			{

				if (geoComp.GetComponent<AnchorableBehaviour>() != null)
				{
					geoComp.GetComponent<AnchorableBehaviour>().Detach();
				}


				if (geoComp.GetComponent<straightEdgeBehave>() != null || geoComp.GetComponent<flatfaceBehave>() != null)
				{
					Node<string> meNode = geoComp.FindGraphNode();
					if (rManList.Contains(meNode))
					{
						rManList.Remove(meNode);
					}
					geomanager.Remove(meNode.Value);
					Destroy(geoComp.gameObject);
				}
				else
				{

					string meNAME = geoComp.figName;
					Node<string> meNode = geomanager.findGraphNode(meNAME);
					if (rManList.Contains(meNode))
					{
						rManList.Remove(meNode);
					}
					geomanager.Remove(meNAME);
					Destroy(geoComp.gameObject);
				}

			}
        }

        public void removeComponentS(string geoCompS)
        {
            if (blockDelete == false)
            {
                geomanager.Remove(geoCompS);
                Destroy(GameObject.Find(geoCompS));
            }
        }

        internal void checkLifeRequirements(AbstractGeoObj geoComp)
        {
            //add case for clones.

            bool killorder = false;

			NodeList<string> neighborList = geoComp.FindGraphNode().Neighbors;

            GeoObjType type = geoComp.figType;

            int pointCount = 0;

            switch (type)
            {
                case GeoObjType.point:
                    //degree == 0
                    killorder = false;
                    break;
                case GeoObjType.line:
                    pointCount = neighborTypeCount(neighborList, GeoObjType.point);
                    //Debug.Log(geoComp.gameObject.name+" has "+pointCount+" points.");
                    if (pointCount < 2)
                    {
                        killorder = true;
                    }
                    else
                    {
                        killorder = false;
                    }
                    break;
                case GeoObjType.polygon:
                    pointCount = neighborTypeCount(neighborList, GeoObjType.point);
                    //TOFIX:  add cycle check
                    if (pointCount < 3)
                    {
                        killorder = true;
                    }
                    else
                    {
                        killorder = false;
                    }
                    break;
                case GeoObjType.circle:
                    pointCount = neighborTypeCount(neighborList, GeoObjType.point);
                    if (pointCount < 1) //|| geoComp.GetComponent<DependentCircle>().Radius <= 0.00000000001f)
                    {
                        killorder = true;
                    }
                    else
                    {
                        killorder = false;
                    }
                    break;
                case GeoObjType.revolvedsurface:
                    //pointCount = neighborTypeCount (neighborList, "Point");
                    //arcCount = neighborTypeCount (neighborList, "Arc");
                    pointCount = neighborTypeCount(neighborList, GeoObjType.point);
                    if (pointCount < 2)
                    {
                        killorder = true;
                    }
                    else
                    {
                        killorder = false;
                    }
                    break;
                case GeoObjType.sphere:
                    pointCount = neighborTypeCount(neighborList, GeoObjType.point);
                    if (pointCount < 2 || geoComp.GetComponent<DependentSphere>().radius <= 0.00000001f)
                    {
                        killorder = true;
                    }
                    else
                    {
                        killorder = false;
                    }
                    break;
				default:
					break;
            }

            if (killorder)
            {
                removeComponent(geoComp);
            }
        }

        internal int neighborTypeCount(NodeList<string> neighborList, GeoObjType value)
        {
			return neighborList.Where(element => element.mytransform.GetComponent<AbstractGeoObj>().figType == value).Count();
        }

        public void replaceDepentVar(Transform parent, Transform oldObj, Transform newObj)
        {
            GeoObjType type = parent.transform.GetComponent<AbstractGeoObj>().figType;

            switch (type)
            {
                case GeoObjType.point:
                    break;
                case GeoObjType.line:
                    if (parent.transform.GetComponent<InteractableLineSegment>().point1 == oldObj.transform.GetComponent<InteractablePoint>())
                    {
                        parent.transform.GetComponent<InteractableLineSegment>().point1 = newObj.transform.GetComponent<InteractablePoint>();
                    }
                    else if (parent.transform.GetComponent<InteractableLineSegment>().point2 == oldObj.transform.GetComponent<InteractablePoint>())
                    {
                        parent.transform.GetComponent<InteractableLineSegment>().point2 = newObj.transform.GetComponent<InteractablePoint>();
                    }
                    break;
                case GeoObjType.polygon:
                    GeoObjType oldType = oldObj.transform.GetComponent<AbstractGeoObj>().figType;
                    switch (oldType)
                    {
                        case GeoObjType.point:
                            if (parent.transform.GetComponent<AbstractPolygon>().pointList.Contains(oldObj.GetComponent<AbstractPoint>()))
                            {
                                int idx = parent.transform.GetComponent<AbstractPolygon>().pointList.IndexOf(oldObj.GetComponent<AbstractPoint>());
                                parent.transform.GetComponent<AbstractPolygon>().pointList[idx] = newObj.GetComponent<AbstractPoint>();
                            }
                            break;
                        case GeoObjType.line:
                            if (parent.transform.GetComponent<AbstractPolygon>().lineList.Contains(oldObj.GetComponent<AbstractLineSegment>()))
                            {
                                int idx = parent.transform.GetComponent<AbstractPolygon>().lineList.IndexOf(oldObj.GetComponent<AbstractLineSegment>());
                                parent.transform.GetComponent<AbstractPolygon>().pointList[idx] = newObj.GetComponent<AbstractPoint>();
                            }
                            break;
                    }
                    break;
                case GeoObjType.circle:
                    Debug.Log("Rebuild This: Arc Case for ObjManHelper reassign");
                    //if (parent.transform.GetComponent<AbstractCircle>().attachedPoint.transform == oldObj.transform)
                    //{
                    //    parent.transform.GetComponent<ArcBehave>().attachedPoint = newObj.transform;
                    //}
                    break;
                case GeoObjType.revolvedsurface:
                    // only dependent on a line.
                    Debug.Log("Rebuild This: RevolvedSurface Case for ObjManHelper reassign");

                    //if (parent.transform.GetComponent<AbstractRevolvedSurface>().transform == oldObj.transform)
                    //{
                    //    parent.transform.GetComponent<AbstractRevolvedSurface>().attachedLine = newObj.transform;
                    //}
                    break;
            }
        }

        public bool checkIfPath(Transform startObj, Transform endObj)
        {
            bool done = false;
            bool isPath = false;

            NodeList<string> neighborList = geomanager.neighborsOfNode(startObj.GetComponent<AbstractGeoObj>().figName);
            NodeList<string> rmNeighborList = new NodeList<string>();

            Node<string> target = geomanager.findGraphNode(endObj.GetComponent<AbstractGeoObj>().figName);

            while (!done)
            {
                if (neighborList.Contains(target))
                {
                    isPath = true;
                    done = true;
                }
                else
                {
                    int idx = 0;
                    foreach (Node<string> node in neighborList)
                    {
                        foreach (Node<string> node2 in geomanager.neighborsOfNode(node.Value))
                        {
                            if (!neighborList.Contains(node2) && !rmNeighborList.Contains(node))
                            {
                                neighborList.Add(node2);
                                neighborList.Remove(node);
                                rmNeighborList.Add(node);
                                idx++;
                            }
                        }
                    }
                    if (idx == 0)
                    {
                        done = true;
                        isPath = neighborList.Contains(target);
                    }
                }
            }
            return isPath;
        }

    }
}
