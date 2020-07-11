/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
/// <summary>
/// An interactable point, the basis for the sandbox.
/// Will be depreciated with new Geometery kernel
/// </summary>
	class InteractablePoint : AbstractPoint, InteractiveFigure
    {
        #region Constructors
            public static InteractablePoint Constructor()
			{
				return PrefabManager.Spawn("InteractablePoint").GetComponent<InteractablePoint>();
			}
        #endregion

        public bool controllCollide = false;
        public bool glueBool = false;

        private List<Transform> neighbors;

        private Vector3 oldPos;
#pragma warning disable 0414
		private bool snapBool = false;
#pragma warning restore 0414

		public override void InitializeFigure()
        {
			base.InitializeFigure();
			this.figType = GeoObjType.point;
            
            initialScale = this.transform.localScale;

            this.neighbors = new List<Transform>();
            this.updateNeighbors();
            thisSelectStatus = thisSelectStatus;

            //moveRestrict();
            StartCoroutine(disallowSnap());
        }

        private IEnumerator disallowSnap()
        {
            yield return new WaitForSeconds(3.5f);
            snapBool = true;
        }
        internal override bool RMotion(NodeList<string> inputNodeList)
        {

            //if this has moved return true.
            //no other implementation needed.
            return hasMoved();
        }

        public override void updateFigure()
        {
            //no update needed.
        }

        public bool hasMoved()
        {
			bool result = (oldPos == null) || (this.Position3 == oldPos);
			this.Position3 = this.Position3;
            oldPos = this.Position3;
            return result;
        }

        /// <summary>
        /// Updates list containing the neighbors of this node.
        /// </summary>
        private void updateNeighbors()
        {
            int oldCount = 0;
            if (neighbors != null)
            {
                oldCount = neighbors.Count;
                neighbors.Clear(); //clear current list

            }


            //add current neighbors to list
            foreach (Node<string> neighbor in HW_GeoSolver.ins.geomanager.neighborsOfNode(this.figName))
            {
                neighbors.Add(GameObject.Find(neighbor.Value).transform);
            }

            changedNeighborNum = (neighbors.Count == oldCount);
        }

        private bool neigborsBeingGrasped()
        {
            if (changedNeighborNum)
            {
                this.changedNeighborNum = false;
                foreach (Transform neighbor in this.neighbors)
                {
                    if (neighbor.GetComponent<InteractionBehaviour>() != null &&
                                       neighbor.GetComponent<InteractionBehaviour>().isGrasped)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Stretch(InteractionController iControll)
        {
            if (stretchEnabled && thisIBehave.graspingControllers.Count > 1)
            {
                iControll.ReleaseGrasp();
                InteractablePoint newPoint = GeoObjConstruction.iPoint(this.Position3);
                /*InteractableLineSegment newLineSegment = */GeoObjConstruction.iLineSegment(this, newPoint);
				if(HW_GeoSolver.ins.thisInteractionMode == HW_GeoSolver.InteractionMode.rigid)
				{
					newPoint.LeapInteraction = false;
					this.LeapInteraction = false;
				}
                StartCoroutine(waitForStretch);
            }
        }

		internal override void SnapToFigure(AbstractGeoObj toObj)
		{
			//do nothing
		}

		internal override void GlueToFigure(AbstractGeoObj toObj)
        {
            if ((toObj.gameObject.tag.Contains("Point") && (toObj.figIndex > this.figIndex)) && ((this.GetComponent<InteractionBehaviour>().isGrasped) || (toObj.transform.GetComponent<InteractionBehaviour>().isGrasped)))
            {

                foreach (Node<string> node in HW_GeoSolver.ins.geomanager.Nodes)
                {
                    GraphNode<string> thisGraphNode = HW_GeoSolver.ins.geomanager.findGraphNode(this.figName);
                    if (HW_GeoSolver.ins.geomanager.neighborsOfNode(node.Value).Contains(thisGraphNode))
                    {
                        NodeList<string> newNeighborList = HW_GeoSolver.ins.geomanager.neighborsOfNode(node.Value);
                        GraphNode<string> otherGraphNode = HW_GeoSolver.ins.geomanager.findGraphNode(toObj.figName);
                        if (newNeighborList.Contains(otherGraphNode) == false)
                        {
                            HW_GeoSolver.ins.AddDependence(GameObject.Find(node.Value).GetComponent<AbstractGeoObj>(), toObj);
                            HW_GeoSolver.ins.replaceDepentVar(GameObject.Find(node.Value).transform, this.transform, toObj.transform);

                        }
                        else
                        {
                            //deletes redundancy instances between two points
                            HW_GeoSolver.ins.removeComponentS(node.Value);
                        }
                    }
                }
                HW_GeoSolver.ins.removeComponent(this);

                //check for cycle in geoObjMan graph
                if (HW_GeoSolver.ins.checkIfPath(this.transform, toObj.transform))
                {
                    //a cycle is being created.
                    //make a polygon

                    Debug.Log("NEED TO MAKE A POLYGON");
                }

            }
        }
	}
}
