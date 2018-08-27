/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

#region Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;
using System;
using System.Linq;
using IMRE.HandWaver.Solver;
#endregion

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class shipsWheelControl : MonoBehaviour
	{
		#region Public Variables

		public float rotationVal = 0;
		public float deltaRotate = 0;

		public float tolerance = .1f;

		public float angVelTarget = 10f;
		public bool coolDownBool = true;
		private bool firstCoolDown = true;

		//public bool snap = false;
		//public int snapPlace = 1;
#pragma warning disable 0649

		public List<Transform> attachedGeoObjs;
#pragma warning restore 0649

		private straightEdgeBehave parentSE;
		private int turnCount;

		private InteractionBehaviour thisIBehave;

		private DateTime actionTime;
		#endregion

		public void Start()
		{
			parentSE = this.transform.parent.transform.parent.GetComponent<straightEdgeBehave>();
			rotationVal = 0;
			deltaRotate = 0;
			thisIBehave = GetComponent<InteractionBehaviour>();
			thisIBehave.OnGraspBegin += capsuleFalse;
			thisIBehave.OnGraspEnd += capsuleTrue;
			GetComponent<Rigidbody>().maxAngularVelocity = 20f;
			actionTime = DateTime.Now;

		}

		#region OnGrasp and Drop Events
		private void capsuleTrue()
		{
			parentSE.GetComponent<CapsuleCollider>().enabled = true;
		}

		private void capsuleFalse()
		{
			parentSE.GetComponent<CapsuleCollider>().enabled = false;
		}
		#endregion

		public void Update()
		{
			//consider moving this to an enumerator triggered by an event on Ibehave.

			deltaRotate = Mathf.Sign(Vector3.Cross(this.transform.up, this.transform.forward).x) * Mathf.Sign(this.GetComponent<Rigidbody>().angularVelocity.x) * Vector3.Magnitude(this.GetComponent<Rigidbody>().angularVelocity) * (180f / Mathf.PI) * Time.deltaTime;
			rotationVal += deltaRotate;
			//if(actionTime == null)
			//{
			//	actionTime = DateTime.Now;
			//}
			coolDownBool = DateTime.Now.Subtract(actionTime).TotalSeconds > 3.0f;
			#region Rotate Selected & Attached Geo Objects

			if ((deltaRotate != 0))
			{
				switch (parentSE.WheelType)
				{
					case shipWheelOffStraightedge.wheelType.revolve:
						if ((Mathf.Abs(deltaRotate) > 1.5f) && (coolDownBool || firstCoolDown))
						{
							firstCoolDown = false;
							revolve();
							rotate();
							actionTime = DateTime.Now;
						}
						else
						{
							rotate();
						}
						break;
					case shipWheelOffStraightedge.wheelType.hoist:
						if ((Mathf.Abs(deltaRotate) > 1.5f) && (coolDownBool || firstCoolDown))
						{
							firstCoolDown = false;
							hoist();
							rotate();
							actionTime = DateTime.Now;
						}
						else
						{
							rotate();
						}
						break;
					case shipWheelOffStraightedge.wheelType.rotate:
						rotate();
						break;
				}
			}


			#endregion
		}

		float rotationAroundX()
		{
			float angle = this.transform.localRotation.eulerAngles.x;
			if (Vector3.Dot(Quaternion.Euler(angle, 0, 0) * Vector3.up, Vector3.Normalize(this.transform.up)) != 1)
			{
				angle = 180 - angle;
			}
			return angle;
		}

		#region Check number of turns, and if this turn is new.

		public int nTurns()
		{
			int n = 0;
			n = (int)(System.Math.Truncate(rotationVal));
			//Debug.Log("The value of rotationVal is " + rotationVal);
			//Debug.Log("The value of nTurns is " + n);
			return n;
		}

		public bool newTurn()
		{
			bool isNew = (turnCount - nTurns() >= 3);
			//Debug.Log("The value of turnCount is " + turnCount);

			if (isNew)
			{
				turnCount = nTurns();
			}
			//Debug.Log(isNew + " is value for isNew boolean");
			return isNew;
		}
		#endregion


		#region Spawn GeoObjs From Script
		public void makeCircle(Vector3 revPoint, Transform attachedPoint, Vector3 normDir)
		{
			#region intialize an arc or circle
			/*DependentCircle dc = */GeoObjConstruction.dCircle(GeoObjConstruction.dPoint(revPoint), attachedPoint.GetComponent<AbstractPoint>(), normDir);
			#endregion
		}

		public void makeRevolvedSurface(Transform attachedLine, Vector3 revPoint, Vector3 normDir)
		{
			#region intialize a cylinder, cone or conic /etc.
			Transform newPoint = PoolManager.Pools["GeoObj"].Spawn("PointPreFab");
			newPoint.transform.position = revPoint;//nullreference on this line from shipswheel

			DependentRevolvedSurface drs = GeoObjConstruction.dRevSurface(newPoint.GetComponent<AbstractPoint>(), attachedLine.GetComponent<AbstractLineSegment>(), normDir);
			HW_GeoSolver.ins.addDependence(drs.transform, newPoint.transform);
            HW_GeoSolver.ins.addDependence(drs.transform, attachedLine.transform);
			#endregion
		}

		public void makeSphere(Transform attachedArc, float radius)
		{
			#region initalize a sphere

			DependentCircle circle = attachedArc.GetComponent<DependentCircle>();
			//Transform sphereMesh = new GameObject("Sphere").transform;
			Transform sphereMesh = PoolManager.Pools["GeoObj"].Spawn("SpherePreFab").transform;

			DependentSphere sphere = sphereMesh.GetComponent<DependentSphere>();
			sphere.edge = circle.edge;
			sphere.center = circle.center;
			sphere.edgePosition = circle.edgePos;
			sphere.centerPosition = circle.centerPos;

			sphere.initializefigure();
			sphere.addToRManager();


			sphereMesh.gameObject.tag = "Sphere";
			sphereMesh.GetComponent<MasterGeoObj>().figType = GeoObjType.sphere;
			HW_GeoSolver.ins.addComponent(sphereMesh.GetComponent<MasterGeoObj>());
			HW_GeoSolver.ins.addDependence(sphereMesh.transform, attachedArc.transform);
			#endregion
		}

		public void makeTorus(Transform attachedArc, Vector3 normalDir)
		{
			#region initalize a sphere
			//Transform TorusMesh = new GameObject("Torus").transform;
			//TorusMesh.transform.parent = GameObject.Find("GeoObj").transform;
			//TorusMesh.transform.position = this.transform.position;
			//TorusMesh.gameObject.AddComponent<MeshRenderer>();
			//TorusMesh.gameObject.AddComponent<torusBehave>().attachedArc = attachedArc.transform.GetComponent<ArcBehave>();
			//TorusMesh.gameObject.GetComponent<torusBehave>().normalDirection = parentSE.normalDir();
			//TorusMesh.GetComponent<Renderer>().material = Resources.Load("StaticShapesCrossSection/RedStaticTransparent", typeof(Material)) as Material;
			//TorusMesh.GetComponent<Renderer>().material.color = Random.ColorHSV();
			//TorusMesh.GetComponent<torusBehave>().Init();
			//TorusMesh.gameObject.tag = "Torus";
			//TorusMesh.gameObject.AddComponent<LifeManager>().type = "Torus";
			//TorusMesh.GetComponent<LifeManager>().ObjectManager = ObjectManager.transform;
			//TorusMesh.GetComponent<LifeManager>().OnSpawned();
			//TorusMesh.GetComponent<LifeManager>().ObjectManager.GetComponent<ObjManHelper>().addDependence(TorusMesh.transform, attachedArc.transform.transform);
			#endregion
		}
		#endregion

		#region Operations
		public void revolve()
		{
			#region Revolve
			if (newTurn())
			{
				Vector3 spindleCenter = parentSE.GetComponent<straightEdgeBehave>().center;
				Vector3 normal = parentSE.GetComponent<straightEdgeBehave>().normalDir;

				foreach (MasterGeoObj geoObj in FindObjectsOfType<MasterGeoObj>().Where(geoObj => (geoObj.IsSelected && geoObj.figType != GeoObjType.straightedge)))
				{
					switch (geoObj.figType)
					{
						case GeoObjType.point:
							Vector3 center = Vector3.Project(geoObj.gameObject.transform.position - spindleCenter, parentSE.normalDir) + spindleCenter;
							makeCircle(center, geoObj.transform, normal);
							//geoObj.IsSelected = false;
							break;
						case GeoObjType.line:
							Vector3 center2 = Vector3.Project(geoObj.gameObject.transform.position - spindleCenter, parentSE.normalDir) + spindleCenter;

							makeRevolvedSurface(geoObj.gameObject.transform, center2, normal);

							AbstractLineSegment lineALS = geoObj.GetComponent<AbstractLineSegment>();

							Vector3 diff1 = Vector3.Project(lineALS.vertex0 - lineALS.transform.position, normal);
							Vector3 diff2 = Vector3.Project(lineALS.vertex1 - lineALS.transform.position, normal);

							if (geoObj.GetComponent<InteractableLineSegment>() != null)
							{
								makeCircle(center2 + diff1, geoObj.GetComponent<InteractableLineSegment>().point1.transform, normal);

								makeCircle(center2 + diff2, geoObj.GetComponent<InteractableLineSegment>().point2.transform, normal);
							}
							else if (geoObj.GetComponent<DependentLineSegment>() != null)
							{
								makeCircle(center2 + diff1, geoObj.GetComponent<DependentLineSegment>().point1.transform, normal);

								makeCircle(center2 + diff2, geoObj.GetComponent<DependentLineSegment>().point2.transform, normal);
							}
							else
							{
								Debug.LogWarning("Can't work with abstractlinesegments yet");
							}
							//geoObj.IsSelected = false;
							break;
						case GeoObjType.polygon:
							foreach (AbstractLineSegment lineObj in geoObj.transform.GetComponent<AbstractPolygon>().lineList)
							{
								Vector3 center3 = Vector3.Project(lineObj.gameObject.transform.position - spindleCenter, parentSE.normalDir) + spindleCenter;

								makeRevolvedSurface(lineObj.gameObject.transform, center3, normal);

								if (lineObj.GetComponent<InteractableLineSegment>() != null)
								{

									Vector3 diff12 = Vector3.Project(lineObj.GetComponent<InteractableLineSegment>().point1.transform.position - lineObj.transform.position, normal);
									Vector3 diff22 = Vector3.Project(lineObj.GetComponent<InteractableLineSegment>().point2.transform.position - lineObj.transform.position, normal);

									makeCircle(center3 + diff12, lineObj.GetComponent<InteractableLineSegment>().point1.transform, normal);
									makeCircle(center3 + diff22, lineObj.GetComponent<InteractableLineSegment>().point2.transform, normal);
								} else if (lineObj.GetComponent<DependentLineSegment>() != null)
								{
									Vector3 diff12 = Vector3.Project(lineObj.GetComponent<DependentLineSegment>().point1.transform.position - lineObj.transform.position, normal);
									Vector3 diff22 = Vector3.Project(lineObj.GetComponent<DependentLineSegment>().point2.transform.position - lineObj.transform.position, normal);

									makeCircle(center3 + diff12, lineObj.GetComponent<DependentLineSegment>().point1.transform, normal);
									makeCircle(center3 + diff22, lineObj.GetComponent<DependentLineSegment>().point2.transform, normal);
								}
								else
								{
									Debug.LogWarning("Can't work with abstractlinesegments yet");
								}
							}
							//geoObj.IsSelected = false;
							break;
						case GeoObjType.revolvedsurface:
							break;
						case GeoObjType.circle:
							//    //check if the circle and the axis are orthagonal.  This is a trivial case.
							//    //if (Vector3.Magnitude(Vector3.Cross(arcObj.GetComponent<ArcBehave>().normalDirection, parentSE.normalDir())) > 0)
							//    //{
							//    //check if the circle's center is on the axis of rotation.  This  produces a sphere.  Else produce a torus.
							//    if (Vector3.Magnitude(Vector3.ProjectOnPlane(arcObj.GetComponent<ArcBehave>().centerPoint.transform.position - parentSE.center(), parentSE.normalDir())) < .05f)
							//    {
							//        //make sphere
							//        if (Vector3.Magnitude(Vector3.Cross(Vector3.Normalize(arcObj.GetComponent<ArcBehave>().normalDirection), Vector3.Normalize(parentSE.normalDir()))) - 1 < .01f)
							//        {
							//            makeSphere(arcObj.transform, Vector3.Magnitude(Vector3.ProjectOnPlane(arcObj.GetComponent<ArcBehave>().attachedPoint.transform.position - parentSE.center(), parentSE.normalDir())));
							//        }else
							//        {
							//            Debug.Log("Arc not Orthagonal, Doesn't make a sphere.");
							//        }
							//    }
							//    else
							//    {
							//        makeTorus(arcObj.transform, parentSE.normalDir());
							//    }
							break;
						case GeoObjType.prism:

							foreach(AbstractLineSegment line in geoObj.GetComponent<InteractablePrism>().lineSegments)
							{
								Vector3 center3 = Vector3.Project(geoObj.gameObject.transform.position - spindleCenter, parentSE.normalDir) + spindleCenter;

								makeRevolvedSurface(geoObj.gameObject.transform, center3, normal);

								AbstractLineSegment lineALS2 = geoObj.GetComponent<AbstractLineSegment>();

								Vector3 diff1b = Vector3.Project(lineALS2.vertex0 - lineALS2.transform.position, normal);
								Vector3 diff2b = Vector3.Project(lineALS2.vertex1 - lineALS2.transform.position, normal);

								if (geoObj.GetComponent<InteractableLineSegment>() != null)
								{
									makeCircle(center3 + diff1b, geoObj.GetComponent<InteractableLineSegment>().point1.transform, normal);

									makeCircle(center3 + diff2b, geoObj.GetComponent<InteractableLineSegment>().point2.transform, normal);
								}
								else if (geoObj.GetComponent<DependentLineSegment>() != null)
								{
									makeCircle(center3 + diff1b, geoObj.GetComponent<DependentLineSegment>().point1.transform, normal);

									makeCircle(center3 + diff2b, geoObj.GetComponent<DependentLineSegment>().point2.transform, normal);
								}
								else
								{
									Debug.LogWarning("Can't work with abstractlinesegments yet");
								}
							}
							break;
					}
					#endregion

				}
			}
		}
		public void hoist()
		{
			Debug.Log("attempting hoist");
			#region Hoist
			MasterGeoObj[] selObj = FindObjectsOfType<MasterGeoObj>().Where(o => ((o.figType != GeoObjType.point) && o.IsSelected)).ToArray();
			foreach (MasterGeoObj currObj in selObj)
			{
				switch (currObj.figType)
				{
					case GeoObjType.point:
						break;
					case GeoObjType.line:
						AbstractPoint newPoint = GeoObjConstruction.iPoint(currObj.GetComponent<InteractableLineSegment>().point1.transform.position);
						AbstractPoint newPoint2 = GeoObjConstruction.iPoint(currObj.GetComponent<InteractableLineSegment>().point2.transform.position);

						GeoObjConstruction.iLineSegment(newPoint, newPoint2);

						currObj.GetComponent<InteractableLineSegment>().point2.transform.RotateAround(parentSE.center, parentSE.normalDir, deltaRotate);
						currObj.GetComponent<InteractableLineSegment>().point1.transform.RotateAround(parentSE.center, parentSE.normalDir, deltaRotate);
						break;
					case GeoObjType.polygon:
						break;
					case GeoObjType.prism:
						break;
					case GeoObjType.pyramid:
						break;
					case GeoObjType.circle:
						break;
					case GeoObjType.sphere:
						break;
					case GeoObjType.revolvedsurface:
						break;
					case GeoObjType.torus:
						break;
					case GeoObjType.flatface:
						break;
					case GeoObjType.straightedge:
						break;
					default:
						break;
				}


			}

				//These need to be developed.

				//foreach (GameObject planeObj in GameObject.FindGameObjectsWithTag("SelPlane"))
				//{
				//    foreach (Transform point in planeObj.GetComponent<PlaneBehave>().pointList)
				//    {
				//        point.transform.RotateAround(parentSE.center(), parentSE.normalDir(), deltaRotate);
				//    }
				//}

				//foreach (GameObject arc in GameObject.FindGameObjectsWithTag("SelArc"))
				//{
				//    attachedGeoObjs.Add(arc.transform);
				//    attachedGeoObjs.Add(arc.GetComponent<ArcBehave>().attachedPoint.transform);
				//}

				//foreach (GameObject circle in GameObject.FindGameObjectsWithTag("SelCircle"))
				//{
				//    attachedGeoObjs.Add(circle.transform);
				//    attachedGeoObjs.Add(circle.GetComponent<CircleBehave>().attachedLine.GetComponent<LineBehave>().point1);
				//    attachedGeoObjs.Add(circle.GetComponent<CircleBehave>().attachedLine.GetComponent<LineBehave>().point2);
				//}
			#endregion
		}
		public void rotate()
		{
			List<MasterGeoObj> objToRotate = new List<MasterGeoObj>();

			foreach (MasterGeoObj geoObj in FindObjectsOfType<MasterGeoObj>().Where(geoObj => (geoObj.IsSelected && geoObj.figType != GeoObjType.straightedge)))
			{

				switch (geoObj.figType)
				{
					case GeoObjType.point:
						if (!objToRotate.Contains(geoObj))
						{
							objToRotate.Add(geoObj);
						}
						break;
					case GeoObjType.line:
						if (geoObj.GetComponent<InteractableLineSegment>() != null)
						{
							geoObj.GetComponent<MasterGeoObj>().addToRManager();
							geoObj.GetComponent<InteractableLineSegment>().point1.addToRManager();
							geoObj.GetComponent<InteractableLineSegment>().point2.addToRManager();
							if (!objToRotate.Contains(geoObj.GetComponent<InteractableLineSegment>().point1.GetComponent<MasterGeoObj>()))
							{
								objToRotate.Add(geoObj.GetComponent<InteractableLineSegment>().point1.GetComponent<MasterGeoObj>());
							}
							if (!objToRotate.Contains(geoObj.GetComponent<InteractableLineSegment>().point2.GetComponent<MasterGeoObj>()))
							{
								objToRotate.Add(geoObj.GetComponent<InteractableLineSegment>().point2.GetComponent<MasterGeoObj>());
							}
						}
						break;
					case GeoObjType.polygon:
						foreach (AbstractPoint point in geoObj.GetComponent<InteractablePolygon>().pointList)
						{
							if (!objToRotate.Contains(point))
							{
								objToRotate.Add(point);
								point.GetComponent<MasterGeoObj>().addToRManager();
							}
						}
						break;
					case GeoObjType.circle:
						//attachedGeoObjs.Add(arc.transform);
						geoObj.GetComponent<AbstractCircle>().normalDir = Quaternion.AngleAxis(deltaRotate, parentSE.normalDir) * geoObj.GetComponent<AbstractCircle>().normalDir;
						geoObj.transform.RotateAround(parentSE.center, parentSE.normalDir, deltaRotate);

						geoObj.GetComponent<MasterGeoObj>().addToRManager();

						if (!geoObj.GetComponent<DependentCircle>().edge.GetComponent<MasterGeoObj>().IsSelected)
						{
							attachedGeoObjs.Add(geoObj.GetComponent<DependentCircle>().edge.transform);
						}
						break;
					case GeoObjType.revolvedsurface:
						geoObj.GetComponent<MasterGeoObj>().addToRManager();

						attachedGeoObjs.Add(geoObj.transform);
						if (!objToRotate.Contains(geoObj.GetComponent<DependentRevolvedSurface>().attachedLine.GetComponent<InteractableLineSegment>().point1.GetComponent<MasterGeoObj>()))
						{
							objToRotate.Add(geoObj.GetComponent<DependentRevolvedSurface>().attachedLine.GetComponent<InteractableLineSegment>().point1.GetComponent<MasterGeoObj>());
						}
						if (!objToRotate.Contains(geoObj.GetComponent<DependentRevolvedSurface>().attachedLine.GetComponent<InteractableLineSegment>().point2.GetComponent<MasterGeoObj>()))
						{
							objToRotate.Add(geoObj.GetComponent<DependentRevolvedSurface>().attachedLine.GetComponent<InteractableLineSegment>().point2.GetComponent<MasterGeoObj>());
						}
						break;
					default:
						geoObj.GetComponent<MasterGeoObj>().addToRManager();

						geoObj.transform.Rotate(parentSE.normalDir, deltaRotate);
						//geoObj.transform.rotation = Quaternion.Euler(rotationVal, 0f, 0f);
						//geoObj.position = Quaternion.AngleAxis(deltaRotate, parentSE.normalDir()) * (geoObj.position - parentSE.center()) + parentSE.center();
						geoObj.transform.RotateAround(parentSE.center, parentSE.normalDir, deltaRotate);
						break;
				}

			}
			foreach (MasterGeoObj geoObj in objToRotate)
			{
				//this should only contain point for now, but we leave a switch for future uses...
				switch (geoObj.figType)
				{
					case GeoObjType.point:
						geoObj.transform.RotateAround(parentSE.center, parentSE.normalDir, deltaRotate);
						geoObj.transform.Rotate(parentSE.normalDir, deltaRotate);
						geoObj.GetComponent<MasterGeoObj>().addToRManager();
						break;
				}
			}
			#endregion
		}
	}
}