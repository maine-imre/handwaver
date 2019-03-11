/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class flatlandSurface : flatfaceBehave
    {
		#region Constructors
		public static flatlandSurface Constructor(){
			GameObject go = GameObject.Instantiate(PrefabManager.GetPrefab("flatlandSurface"));
			return go.GetComponent<flatlandSurface>();
		}
		#endregion

        public flatlandSurface otherFlatlandSurface;

        public List<MasterGeoObj> attachedObjs;

        private void Update()
        {
            if (this.GetComponent<InteractionBehaviour>().isGrasped)
            {
                otherFlatlandSurface.transform.rotation = this.transform.rotation;
            }

            foreach (MasterGeoObj obj in attachedObjs)
            {
                Vector3 newPos = Vector3.ProjectOnPlane(obj.transform.position - this.transform.position, this.transform.up) + this.transform.position;
                if (obj.transform.position != newPos)
                {
                    obj.transform.position = newPos;
                    obj.AddToRManager();
                }
            }
        }

        private new void Start()
        {
            Material mat = gameObject.GetComponent<Renderer>().material;
            mat.renderQueue = 3002;
            gameObject.GetComponent<Renderer>().material = mat;
        }

        //private void Update()
        //{

        //}

        //private void OnTriggerStay(Collider other)
        //{
        //    //if (collision.transform.GetComponent<InteractionBehaviour>() == null || collision.transform.GetComponent<InteractionBehaviour>().isGrasped == false)
        //    //{
        //    if (other.gameObject.GetComponent<AbstractPoint>() != null && (other.gameObject.GetComponent<AnchorableBehaviour>() != null || !other.gameObject.GetComponent<AnchorableBehaviour>().isAttached))
        //    {
        //        other.transform.position = Vector3.ProjectOnPlane(other.transform.position - this.transform.position, this.transform.up) + this.transform.position;
        //    }
        //    else if (other.gameObject.GetComponent<AbstractPolygon>() != null)
        //    {
        //        //project the polygon into the plane.  Turn skewing off.  Keep Polygon in the plane.
        //        AbstractPolygon poly = other.gameObject.GetComponent<AbstractPolygon>();
        //        other.transform.position = Vector3.ProjectOnPlane(other.transform.position - this.transform.position, this.transform.up) + this.transform.position;
        //        //poly.skewable = false;
        //        ObjManHelper.ins.addToReactionManager(poly.transform);
        //        foreach (AbstractPoint point in poly.pointList)
        //        {
        //            point.transform.position = Vector3.ProjectOnPlane(point.transform.position - this.transform.position, this.transform.up) + this.transform.position;
        //            ObjManHelper.ins.addToReactionManager(point.transform);
        //        }
        //    }
        //    else if (other.gameObject.GetComponent<InteractableLineSegment>() != null)
        //    {
        //        InteractableLineSegment line = other.gameObject.GetComponent<InteractableLineSegment>();
        //        other.transform.position = Vector3.ProjectOnPlane(other.transform.position - this.transform.position, this.transform.up) + this.transform.position;

        //        ObjManHelper.ins.addToReactionManager(line.transform);

        //        line.point1.transform.position = Vector3.ProjectOnPlane(line.point1.transform.position - this.transform.position, this.transform.up) + this.transform.position;
        //        ObjManHelper.ins.addToReactionManager(line.point1.transform);

        //        line.point2.transform.position = Vector3.ProjectOnPlane(line.point2.transform.position - this.transform.position, this.transform.up) + this.transform.position;
        //        ObjManHelper.ins.addToReactionManager(line.point2.transform);
        //    }
        //    // }
        //}
    }
}