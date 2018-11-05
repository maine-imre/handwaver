/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Leap.Unity.Interaction;
using System.Linq;

namespace IMRE.HandWaver.Solver
{
	/// <summary>
	/// Manages calls for intersections between objects in the scene.
	/// The main contributor(s) to this script is __
	/// Status: Holy shit does this need optimization!
	/// </summary>
	class intersectionManager : MonoBehaviour
	{
		 public Dictionary<MasterGeoObj, MasterGeoObj[]> pastIntersections;
		private HashSet<string> hashIntersectedFigs;
		public HashSet<string> hashIntersectionProducts;
        internal static intersectionManager ins;

        /// <summary>
        /// Use this boolean to turn on and off the intersection check.  Turning off the component may break the HW_GeoSolver.
        /// </summary>
        public bool intersectionCheckEnabled = true;

        internal void UpdateIntersections(NodeList<string> input)
        {
            //foreach (Node<string> node in input)
            //{
            //    if (hashIntersectionProducts.Contains(node.Value))
            //    {
            //        //run this on result figures not products.
            //        updateIntersectionProduct(node.mytransform.GetComponent<MasterGeoObj>());
            //    }
            //}
        }

        public void updateIntersectionProduct(MasterGeoObj mgo)
        {
            MasterGeoObj[] parents = pastIntersections[mgo];

            switch (parents[0].figType)
            {
                case GeoObjType.circle:
                    switch (parents[1].figType)
                    {
                        case GeoObjType.circle:
                            updateCircleCircle(mgo, parents[0],parents[1]);
                            break;
                        case GeoObjType.sphere:
                            updateSphereCircle(mgo, parents[1], parents[0]);
                            break;
                        case GeoObjType.flatface:
                            updateCirclePlane(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.straightedge:
                            updateCircleLine(mgo, parents[0], parents[1]);
                            break;
                        default:
                            Debug.LogWarning("ParentTypeMisMatch");
                            break;
                    }
                    break;
                case GeoObjType.sphere:
                    switch (parents[1].figType)
                    {
                        case GeoObjType.circle:
                            updateSphereCircle(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.sphere:
                            updateSphereSphere(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.flatface:
                            updateSpherePlane(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.straightedge:
                            updateSphereLine(mgo, parents[0], parents[1]);
                            break;
                        default:
                            Debug.LogWarning("ParentTypeMisMatch");
                            break;
                    }
                    break;
                case GeoObjType.flatface:
                    switch (parents[1].figType)
                    {
                        case GeoObjType.circle:
                            updateCirclePlane(mgo, parents[1], parents[0]);
                            break;
                        case GeoObjType.sphere:
                            updateSpherePlane(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.flatface:
                            updatePlanePlane(mgo, parents[0], parents[1]);
                            break;
                        case GeoObjType.straightedge:
                            updatePlaneLine(mgo, parents[0], parents[1]);
                            break;
                        default:
                            Debug.LogWarning("ParentTypeMisMatch");
                            break;
                    }
                    break;
                case GeoObjType.straightedge:
                    switch (parents[1].figType)
                    {
                        case GeoObjType.circle:
                            updateCircleLine(mgo, parents[1], parents[0]);
                            break;
                        case GeoObjType.sphere:
                            updateSphereLine(mgo, parents[1], parents[0]);
                            break;
                        case GeoObjType.flatface:
                            updatePlaneLine(mgo, parents[1], parents[0]);
                            break;
                        case GeoObjType.straightedge:
                            updateLineLine(mgo, parents[0], parents[1]);
                            break;
                        default:
                            Debug.LogWarning("ParentTypeMisMatch");
                            break;
                    }
                    break;
                default:
                    Debug.LogWarning("ParentTypeMisMatch");
                    break;
            }
        }

        private void updateLineLine(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.LineLineIntersection(masterGeoObj1.GetComponent<straightEdgeBehave>(), masterGeoObj2.GetComponent<straightEdgeBehave>());
            if (data.figtype == mgo.figType)
            {
                //point only option.
                mgo.Position3 = data.vectordata[0];
                mgo.AddToRManager();
            }
            else if(data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updatePlaneLine(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.LinePlaneIntersection(masterGeoObj1.GetComponent<flatfaceBehave>(), masterGeoObj2.GetComponent<straightEdgeBehave>());
            if (data.figtype == mgo.figType)
            {
                //point only option;
                mgo.Position3 = data.vectordata[0];
                mgo.AddToRManager();
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updatePlanePlane(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.PlanePlaneIntersection(masterGeoObj1.GetComponent<flatfaceBehave>(), masterGeoObj2.GetComponent<flatfaceBehave>());
            if (data.figtype == mgo.figType)
            {
                //line only option
                mgo.Position3 = data.vectordata[0];
                mgo.AddToRManager();
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateSphereLine(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereLineIntersection(masterGeoObj1.GetComponent<AbstractSphere>(), masterGeoObj2.GetComponent<straightEdgeBehave>());
            if (data.figtype == mgo.figType)
            {
                //point or points;
                if (data.vectordata.Length > 1)
                {
                    mgo.Position3 = data.vectordata[mgo.intersectionMultipleIDX];
                }
                else
                {
                    mgo.Position3 = data.vectordata[0];
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateSpherePlane(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SpherePlaneIntersection(masterGeoObj1.GetComponent<AbstractSphere>(), masterGeoObj2.GetComponent<flatfaceBehave>());
            if (data.figtype == mgo.figType)
            {
                switch (data.figtype)
                {
                    case GeoObjType.point:
                        mgo.Position3 = data.vectordata[0];
                        mgo.AddToRManager();
                        break;
                    case GeoObjType.circle:
                        DependentCircle circle = mgo.GetComponent<DependentCircle>();
                        circle.centerPos = data.vectordata[0];
                        circle.normalDir = data.vectordata[1];
                        circle.center.Position3 = data.vectordata[0];
                        circle.edgePos = data.vectordata[2];
                        circle.edge.Position3 = data.vectordata[2];
                        circle.AddToRManager();
                        circle.edge.AddToRManager();
                        circle.center.AddToRManager();
                        break;
                    default:
                        break;
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateSphereSphere(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereSphereIntersection(masterGeoObj1.GetComponent<AbstractSphere>(), masterGeoObj2.GetComponent<AbstractSphere>());
            if (data.figtype == mgo.figType)
            {
                switch (data.figtype)
                {
                    case GeoObjType.point:
                        mgo.Position3 = data.vectordata[0];
                        mgo.AddToRManager();
                        break;
                    case GeoObjType.circle:
                        DependentCircle circle = mgo.GetComponent<DependentCircle>();
                        circle.centerPos = data.vectordata[0];
                        circle.normalDir = data.vectordata[1];
                        circle.center.Position3 = data.vectordata[0];
                        circle.edgePos = data.vectordata[2];
                        circle.edge.Position3 = data.vectordata[2];
                        circle.AddToRManager();
                        circle.edge.AddToRManager();
                        circle.center.AddToRManager();
                        break;
                    default:
                        break;
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateCirclePlane(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CirclePlaneIntersection(masterGeoObj1.GetComponent<AbstractCircle>(), masterGeoObj2.GetComponent<flatfaceBehave>());
            if (data.figtype == mgo.figType)
            {
                if (data.vectordata.Length > 1)
                {
                    mgo.Position3 = data.vectordata[mgo.intersectionMultipleIDX];
                }
                else
                {
                    mgo.Position3 = data.vectordata[0];
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateSphereCircle(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereCircleIntersection(masterGeoObj1.GetComponent<AbstractSphere>(), masterGeoObj2.GetComponent<AbstractCircle>());
            if (data.figtype == mgo.figType)
            {
                switch (data.figtype)
                {
                    case GeoObjType.point:
                        mgo.Position3 = data.vectordata[0];
                        mgo.AddToRManager();
                        break;
                    case GeoObjType.circle:
                        DependentCircle circle = mgo.GetComponent<DependentCircle>();
                        circle.centerPos = data.vectordata[0];
                        circle.normalDir = data.vectordata[1];
                        circle.center.Position3 = data.vectordata[0];
                        circle.edgePos = data.vectordata[2];
                        circle.edge.Position3 = data.vectordata[2];
                        circle.AddToRManager();
                        circle.edge.AddToRManager();
                        circle.center.AddToRManager();
                        break;
                    default:
                        break;
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateCircleCircle(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CircleCircleIntersection(masterGeoObj1.GetComponent<AbstractCircle>(), masterGeoObj2.GetComponent<AbstractCircle>());

            if (data.figtype == mgo.figType)
            {
                switch (data.figtype)
                {
                    case GeoObjType.point:
                        mgo.Position3 = data.vectordata[0];
                        mgo.AddToRManager();
                        break;
                    case GeoObjType.circle:
                        DependentCircle circle = mgo.GetComponent<DependentCircle>();
                        circle.centerPos = data.vectordata[0];
                        circle.normalDir = data.vectordata[1];
                        circle.center.Position3 = data.vectordata[0];
                        circle.edgePos = data.vectordata[2];
                        circle.edge.Position3 = data.vectordata[2];
                        circle.AddToRManager();
                        circle.edge.AddToRManager();
                        circle.center.AddToRManager();
                        break;
                    default:
                        break;
                }

            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void updateCircleLine(MasterGeoObj mgo, MasterGeoObj masterGeoObj1, MasterGeoObj masterGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CircleLineIntersection(masterGeoObj1.GetComponent<AbstractCircle>(), masterGeoObj2.GetComponent<straightEdgeBehave>());
            if (data.figtype == mgo.figType)
            {
                switch (data.figtype)
                {
                    case GeoObjType.point:
                        mgo.Position3 = data.vectordata[0];
                        mgo.AddToRManager();
                        break;
                    case GeoObjType.circle:
                        DependentCircle circle = mgo.GetComponent<DependentCircle>();
                        circle.centerPos = data.vectordata[0];
                        circle.normalDir = data.vectordata[1];
                        circle.center.Position3 = data.vectordata[0];
                        circle.edgePos = data.vectordata[2];
                        circle.edge.Position3 = data.vectordata[2];
                        circle.AddToRManager();
                        circle.edge.AddToRManager();
                        circle.center.AddToRManager();
                        break;
                    default:
                        break;
                }
            }
            else if (data.figtype == GeoObjType.none)
            {
                mgo.DeleteGeoObj();
            }
            else
            {
                Debug.LogWarning("TYPE MISMATCH");
            }
        }

        private void Start()
		{
			pastIntersections = new Dictionary<MasterGeoObj, MasterGeoObj[]>();
			hashIntersectedFigs = new HashSet<string>();
			hashIntersectionProducts = new HashSet<string>();

            //this way the intersectionmanager can be disabled but present in a scene.
            if (intersectionCheckEnabled)
            {
                StartCoroutine(checkAllIntersections(5f));
            }

            ins = this;
		}

		private void AddToDictionary(MasterGeoObj obj1, MasterGeoObj obj2, MasterGeoObj[] objList)
		{
			if (objList != null && objList.Length > 0)
			{
				MasterGeoObj[] inputList = new MasterGeoObj[2];
				inputList[0] = obj1;
				inputList[1] = obj2;

				hashIntersectedFigs.Add(obj1.name+obj2.name);

				foreach (MasterGeoObj obj in objList)
				{
					hashIntersectionProducts.Add(obj.name);
					pastIntersections.Add(obj, inputList);
					this.GetComponent<HW_GeoSolver>().addDependence(obj.transform, obj1.transform);
					this.GetComponent<HW_GeoSolver>().addDependence(obj.transform, obj2.transform);
				}

			}
		}

		private bool checkInDictionary(MasterGeoObj obj1, MasterGeoObj obj2)
		{
			return (hashIntersectedFigs.Contains(obj1.name+obj2.name) || hashIntersectedFigs.Contains(obj1.name+ obj2.name));

			//return (pastIntersections.ContainsKey(inputList) || pastIntersections.ContainsKey(inputList2));
		}

		internal IEnumerator checkAllIntersections(float waitTime)
		{
			while (true)
			{
				checkIntersectAny();
				yield return new WaitForSecondsRealtime(waitTime);
			}
		}

		/// <summary>
		/// Checks if there is an intersection between any figures in the scene that isnt currently attached to any anchor.
		/// </summary>
		public void checkIntersectAny()
		{
			MasterGeoObj[] geoObjList = GameObject.FindObjectsOfType<MasterGeoObj>().Where(g => g.GetComponent<AbstractPoint>() == null).ToArray();

			for (int i = 0; i < geoObjList.Length; i++)
			{
				MasterGeoObj obj1 = geoObjList[i];
				if (obj1.GetComponent<AnchorableBehaviour>() != null && (obj1.GetComponent<AnchorableBehaviour>().isAttached))
					continue;
				if(true)
				{
					for (int j = i + 1; j < geoObjList.Length; j++)
					{

						MasterGeoObj obj2 = geoObjList[j];
						if (obj2.GetComponent<AnchorableBehaviour>() != null && (obj2.GetComponent<AnchorableBehaviour>().isAttached))
							continue;
						if (obj1 != obj2)
						{
							checkIntersection(obj1, obj2);
						}
					}
				}
			}
		}

		/// <summary>
		/// Given a pair of MasterGeoObjs, check to see if they intersect.
		/// </summary>
		internal void checkIntersection(MasterGeoObj obj1, MasterGeoObj obj2)
		{

			if (!checkInDictionary(obj1, obj2))
			{
				//Debug.Log(obj1.figName + " " + obj2.figName);

				switch (obj1.figType)
				{
					case GeoObjType.sphere:
						switch (obj2.figType)
						{
                            case GeoObjType.sphere:
                                AddToDictionary(obj1, obj2, checkSphereSphere(obj1.GetComponent<AbstractSphere>(), obj2.GetComponent<AbstractSphere>()));
                                break;
                            case GeoObjType.straightedge:
                                AddToDictionary(obj1, obj2, checkSphereStraightEdge(obj1.GetComponent<AbstractSphere>(), obj2.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.flatface:
                                AddToDictionary(obj1, obj2, checkSphereFlatface(obj1.GetComponent<AbstractSphere>(), obj2.GetComponent<flatfaceBehave>()));
                                break;
                        }
                        break;
                    case GeoObjType.circle:
                        switch (obj2.figType)
                        {
                            case GeoObjType.sphere:
                                AddToDictionary(obj1, obj2, checkSpherecircle(obj2.GetComponent<AbstractSphere>(), obj1.GetComponent<AbstractCircle>()));
                                break;
                            case GeoObjType.circle:
                                AddToDictionary(obj1, obj2, CheckCircleCircle(obj1.GetComponent<AbstractCircle>(), obj2.GetComponent<AbstractCircle>()));
                                break;
                            case GeoObjType.straightedge:
                                AddToDictionary(obj1, obj2, checkCircleStraightEdge(obj1.GetComponent<AbstractCircle>(), obj2.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.flatface:
                                AddToDictionary(obj1, obj2, checkCircleFlatface(obj1.GetComponent<AbstractCircle>(), obj2.GetComponent<flatfaceBehave>()));
                                break;
                        }
						break;
                    case GeoObjType.flatface:
                        switch (obj2.figType)
                        {
                            case GeoObjType.sphere:
                                AddToDictionary(obj1, obj2, checkSphereFlatface(obj2.GetComponent<AbstractSphere>(), obj1.GetComponent<flatfaceBehave>()));
                                break;
                            case GeoObjType.circle:
                                AddToDictionary(obj1, obj2, checkCircleFlatface(obj2.GetComponent<AbstractCircle>(), obj1.GetComponent<flatfaceBehave>()));
                                break;
                            case GeoObjType.straightedge:
                                AddToDictionary(obj1, obj2, checkFlatfaceStraightedge(obj1.GetComponent<flatfaceBehave>(), obj2.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.flatface:
                                AddToDictionary(obj1, obj2, checkFlatfaceFlatface(obj1.GetComponent<flatfaceBehave>(), obj2.GetComponent<flatfaceBehave>()));
                                break;
                        }
                        break;
                    case GeoObjType.straightedge:
                        switch (obj2.figType)
                        {
                            case GeoObjType.sphere:
                                AddToDictionary(obj1, obj2, checkSphereStraightEdge(obj2.GetComponent<AbstractSphere>(), obj1.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.circle:
                                AddToDictionary(obj1, obj2, checkCircleStraightEdge(obj2.GetComponent<AbstractCircle>(), obj1.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.straightedge:
                                AddToDictionary(obj1, obj2, checkStraightedgeStraightedge(obj1.GetComponent<straightEdgeBehave>(), obj2.GetComponent<straightEdgeBehave>()));
                                break;
                            case GeoObjType.flatface:
                                AddToDictionary(obj1, obj2, checkFlatfaceStraightedge(obj2.GetComponent<flatfaceBehave>(), obj1.GetComponent<straightEdgeBehave>()));
                                break;
                        }
                        break;

                }

			}
			
		}

        private MasterGeoObj[] checkStraightedgeStraightedge(straightEdgeBehave straightEdgeBehave1, straightEdgeBehave straightEdgeBehave2)
        {
            intersectionFigData data = IntersectionMath.LineLineIntersection(straightEdgeBehave1, straightEdgeBehave2);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkFlatfaceFlatface(flatfaceBehave flatfaceBehave1, flatfaceBehave flatfaceBehave2)
        {
            intersectionFigData data = IntersectionMath.PlanePlaneIntersection(flatfaceBehave1, flatfaceBehave2);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.line)
            {
                Debug.LogWarning("Need to Construct Line");
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkFlatfaceStraightedge(flatfaceBehave flatfaceBehave, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.LinePlaneIntersection(flatfaceBehave, straightEdgeBehave);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkCircleFlatface(AbstractCircle abstractCircle, flatfaceBehave flatfaceBehave)
        {
            intersectionFigData data = IntersectionMath.CirclePlaneIntersection(abstractCircle, flatfaceBehave);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }else if (data.vectordata.Length == 2)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkCircleStraightEdge(AbstractCircle abstractCircle, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.CircleLineIntersection(abstractCircle, straightEdgeBehave);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private MasterGeoObj[] CheckCircleCircle(AbstractCircle abstractCircle1, AbstractCircle abstractCircle2)
        {
            intersectionFigData data = IntersectionMath.CircleCircleIntersection(abstractCircle1, abstractCircle2);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkSpherecircle(AbstractSphere abstractSphere, AbstractCircle abstractCircle)
        {
            intersectionFigData data = IntersectionMath.SphereCircleIntersection(abstractSphere, abstractCircle);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkSphereFlatface(AbstractSphere abstractSphere, flatfaceBehave flatfaceBehave)
        {
            intersectionFigData data = IntersectionMath.SpherePlaneIntersection(abstractSphere, flatfaceBehave);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.circle)
            {
                DependentPoint centerPoint = GeoObjConstruction.dPoint(data.vectordata[0]);
                Vector3 radiusDirection = Vector3.up;
                if(Vector3.Cross(radiusDirection,data.vectordata[1]).magnitude == 0)
                {
                    radiusDirection = Vector3.right;
                }
                radiusDirection = Vector3.Cross(data.vectordata[1], radiusDirection).normalized;

                DependentPoint edgePoint = GeoObjConstruction.dPoint(data.vectordata[0] + data.floatdata[0] * radiusDirection);
                DependentCircle newCircle = GeoObjConstruction.dCircle(centerPoint, edgePoint, data.vectordata[1]);
                mgoResult = new MasterGeoObj[] { centerPoint.setIntersectionFigure(0), edgePoint.setIntersectionFigure(1), newCircle.setIntersectionFigure(2) };
            }
            else if(data.figtype == GeoObjType.point)
            {
                mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]) };
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkSphereStraightEdge(AbstractSphere abstractSphere, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.SphereLineIntersection(abstractSphere, straightEdgeBehave);

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private MasterGeoObj[] checkSphereSphere(AbstractSphere abstractSphere1, AbstractSphere abstractSphere2)
        {
            intersectionFigData data = IntersectionMath.SphereSphereIntersection(abstractSphere1, abstractSphere2);

			Debug.Log("Data produces " + data.figtype.ToString());
			Debug.Log("Point Value " + data.vectordata[0].ToString());

            MasterGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.circle)
            {
                DependentPoint centerPoint = GeoObjConstruction.dPoint(data.vectordata[0]);
                Vector3 radiusDirection = Vector3.up;
                if (Vector3.Cross(radiusDirection, data.vectordata[1]).magnitude == 0)
                {
                    radiusDirection = Vector3.right;
                }
                radiusDirection = Vector3.Cross(data.vectordata[1], radiusDirection).normalized;

                DependentPoint edgePoint = GeoObjConstruction.dPoint(data.vectordata[2]);
                DependentCircle newCircle = GeoObjConstruction.dCircle(centerPoint, edgePoint, data.vectordata[1]);
                mgoResult = new MasterGeoObj[] { centerPoint.setIntersectionFigure(0), edgePoint.setIntersectionFigure(0), newCircle.setIntersectionFigure(0) };
            }
            else if (data.figtype == GeoObjType.point)
            {
                mgoResult = new MasterGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]) };
            }
            return mgoResult;
        }
    }
}