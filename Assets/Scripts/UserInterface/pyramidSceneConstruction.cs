using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;

using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class pyramidSceneConstruction : MonoBehaviour
    {
        public int nSides = 4;
        public float unit = 0.5f;
        public bool prism = false;
#pragma warning disable 0649
		public Transform spawnPoint;
		public AudioClip errorClip;
		public AudioClip successClip;

		private AbstractPolygon basePoly;
        private AbstractPolygon basePoly2;
        private AbstractPoint apex;
        private DependentPyramid dPyramid;
        private InteractablePrism iPrism;
		private Anchor thisAnchor;
		private AudioSource thisAS;

#pragma warning restore 0649
		void Start()
		{
			thisAnchor = GetComponent<Anchor>();
			thisAS = GetComponent<AudioSource>();
			thisAnchor.OnNoAnchorablesAttached += onAnchorRemove;
		}

		/// <summary>
		/// Called by the object as it is anchored so that it will save the state 
		/// </summary>
		/// <param name="_nSides">new value for nSides variable</param>
		/// <param name="_unit">new value for unit variable</param>
		/// <param name="_prism">new value for prism boolean</param>
		public void OnDocked(int _nSides, float _unit, bool _prism)
		{
			if ((_nSides != nSides || _unit != unit || _prism != prism) || (_nSides == 4 && _unit == 0.5f && _prism == false))
				//if already made with same param; OR statement to handle if you choose to use deault param
			{
				this.nSides = _nSides;
				this.unit = _unit;
				this.prism = _prism;
				if (dPyramid != null)	//deletes old pyramid when you attempt to spawn a new one
					HW_GeoSolver.ins.removeComponent(dPyramid);//not working. Camden fix this plsthx.
				Generate();
			}
			else
			{
				thisAS.clip = errorClip;
				thisAS.Play();
			}
		}

		[ContextMenu("Generate Pyramid Figure")]
        public void Generate()
        {
            float apothem = 0.5f *unit/ Mathf.Tan(Mathf.PI / nSides);
            apex = GeoObjConstruction.iPoint(spawnPoint.position + unit * Vector3.up);
            basePoly = GeoObjConstruction.rPoly(nSides, apothem, spawnPoint.position);
            dPyramid = GeoObjConstruction.dPyramid(basePoly, apex);

            if (prism)
            {

				dPyramid.sides.ForEach(p => p.LeapInteraction = false);
				dPyramid.sides.ForEach(p => p.transform.GetChild(0).gameObject.SetActive(false));

				iPrism = GeoObjConstruction.iPrism(basePoly, spawnPoint.position + unit * Vector3.up);
				iPrism.LeapInteraction = false;

				iPrism.sides.ForEach(p => GeoObjConstruction.dPyramid(p, apex).sides.ForEach(q => q.LeapInteraction = false));
				iPrism.sides.ForEach(p => p.LeapInteraction = false);

				iPrism.bases.ForEach(p => p.pointList.ForEach(q => q.LeapInteraction = false));
				iPrism.bases.ForEach(p => p.pointList.ForEach(q => q.GetComponent<MeshRenderer>().enabled = false));

                HW_GeoSolver.ins.addDependence(basePoly2.transform, apex.transform);

                apex.GetComponent<InteractionBehaviour>().OnGraspedMovement += updateFigure;
            }
            else
            {
				HW_GeoSolver.ins.removeComponent(basePoly2);
                //iPrism.geoManager.removeComponent(iPrism.transform);
            }
			thisAS.clip = successClip;
			thisAS.Play();
		}

		internal void onAnchorRemove()
		{
			this.nSides = 4;
			this.unit = 0.5f;
			this.prism = false;
		}

		private void updateFigure(Vector3 p1, Quaternion p2, Vector3 p3, Quaternion p4, List<InteractionController> p5)
        {
            basePoly2.pointList.ForEach(p => p.transform.position += basePoly2.transform.position - apex.transform.position);
            basePoly.transform.position += basePoly2.transform.position - apex.transform.position;
        }
    }

}