/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;
using System;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class arctusBehaveV3 : HandWaverTools
	{
#pragma warning disable 0649
		public DependentSphere thisSphere;
		public bool centerSet = false;
		public bool edgeSet = false;
		private AbstractPoint center;
		private AbstractPoint edge;

		public arctusHandle centerHandle;
		public arctusHandle edgeHandle;
		public GameObject handlePrefab;

		public Transform shipsWheel;
		public SphereCollider wheelSpinCollider;

		public LineRenderer thisCircle;
		private AnchorableBehaviour thisABehave;
		public float angleDeltaThreshold;
#pragma warning restore 0649

		private void Start()
		{
			thisCircle.positionCount = 300;
			thisCircle.useWorldSpace = true;
			thisABehave = centerHandle.GetComponent<AnchorableBehaviour>();
			thisABehave.OnAttachedToAnchor += attach;
			thisABehave.OnDetachedFromAnchor += detach;
		}

		internal arctusHandle spawnEdge(Vector3 position)
		{
			edgeHandle = Instantiate(handlePrefab).GetComponent<arctusHandle>();
			edgeHandle.thisArctus = this;
			edgeHandle.transform.position = .2f * Vector3.up + this.transform.position;

			return edgeHandle;
		}

		private void detach()
		{
		}

		private void attach()
		{
			thisCircle.enabled = false;
		}

		private void Update()
		{
			//adjust the axis of the ships wheel.
			if (shipsWheel.transform.position != centerPos || shipsWheel.transform.rotation != Quaternion.FromToRotation(Vector3.right, normalDir)) {
				shipsWheel.transform.position = centerPos;
				shipsWheel.transform.rotation = Quaternion.FromToRotation(Vector3.right, normalDir);
			}
			if (thisCircle.enabled)
			{
				Vector3 basis1 = normalDir;
				Vector3 basis2 = Vector3.up;
				if (basis1 == basis2)
					basis2 = Vector3.right;
				Vector3.OrthoNormalize(ref basis1, ref basis2);

				Vector3[] positions = new Vector3[300];
				for (int i = 0; i < 300; i++)
				{
					//wheelSpinCollider.transform.rotation * * Quaternion.FromToRotation(shipsWheel.transform.forward, wheelSpinCollider.transform.forward)
					positions[i] = Quaternion.FromToRotation(shipsWheel.forward, wheelSpinCollider.transform.forward) * (radius * (basis1 * Mathf.Sin(Mathf.PI * 2 * i / 300) + basis2 * Mathf.Cos(Mathf.PI * 2 * i / 300))) + centerPos;
				}
				thisCircle.SetPositions(positions);
			}
			if (wheelSpinCollider.GetComponent<Rigidbody>().angularVelocity.magnitude > angleDeltaThreshold)
			{
				StartCoroutine(waitForSphere());
			}
		}

		private IEnumerator waitForSphere()
		{
			yield return new WaitForSecondsRealtime(2.0f);
			checkCenterEdge();
		}

        private Vector3 centerPos
        {
            get
            {
                if (Center != null)
                {
                    return Center.transform.position;
                }
                else
                {
                    return centerHandle.transform.position;
                }
            }
        }

        private Vector3 edgePos
        {
            get
            {
                if (Edge != null)
                {
                    return Edge.transform.position;
                }
                else
                {
                    return centerHandle.transform.position;
                }

            }
        }

        private Vector3 normalDir
        {
            get
            {
				if (edgeHandle != null)
					return centerPos - edgePos;
				else
					return Vector3.right;
			}
        }

        private float radius
        {
            get
            {
                return Vector3.Magnitude(centerPos - edgePos);
            }
        }

        public AbstractPoint Center
        {
            get
            {
                return center;
            }

            set
            {
				wheelSpinCollider.enabled = (value != null);
                center = value;
                centerSet = (value != null);
				thisCircle.enabled = (value != null);
				//checkCenterEdge();
            }
        }

		public AbstractPoint Edge
        {
            get
            {
                return edge;
            }

            set
            {
                edge = value;
                edgeSet = (value!=null);
                //checkCenterEdge();
            }
        }

        private void checkCenterEdge()
        {
            if (centerSet && edgeSet)
            {
				Transform thisSphereT = PoolManager.Pools["GeoObj"].Spawn("SpherePreFab").transform;
				thisSphere = thisSphereT.GetComponent<DependentSphere>();
				thisSphere.center = Center;
				thisSphere.centerPosition = Center.transform.position;
				thisSphere.edge = Edge;
				thisSphere.edgePosition = Edge.transform.position;
				thisSphere.transform.position = Center.transform.position;
                
                HW_GeoSolver.ins.addDependence(thisSphere.transform, Center.transform);
                HW_GeoSolver.ins.addDependence(thisSphere.transform, Edge.transform);

                thisSphere.initializefigure();

				Destroy(edgeHandle.gameObject);
                Destroy(gameObject);
            }
        }

	}
}