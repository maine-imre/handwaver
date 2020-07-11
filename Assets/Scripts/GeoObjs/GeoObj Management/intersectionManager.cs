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
/// Finds intersections between geometric figures
/// Runs automatically.
/// Will be depreciated with new geometery kernel.
/// </summary>
	class intersectionManager : MonoBehaviour
	{
		 public Dictionary<AbstractGeoObj, AbstractGeoObj[]> pastIntersections;
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
            //        updateIntersectionProduct(node.mytransform.GetComponent<AbstractGeoObj>());
            //    }
            //}
        }

        public void updateIntersectionProduct(AbstractGeoObj mgo)
        {
            AbstractGeoObj[] parents = pastIntersections[mgo];

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

        private void updateLineLine(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.LineLineIntersection(AbstractGeoObj1.GetComponent<straightEdgeBehave>(), AbstractGeoObj2.GetComponent<straightEdgeBehave>());
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

        private void updatePlaneLine(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.LinePlaneIntersection(AbstractGeoObj1.GetComponent<flatfaceBehave>(), AbstractGeoObj2.GetComponent<straightEdgeBehave>());
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

        private void updatePlanePlane(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.PlanePlaneIntersection(AbstractGeoObj1.GetComponent<flatfaceBehave>(), AbstractGeoObj2.GetComponent<flatfaceBehave>());
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

        private void updateSphereLine(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereLineIntersection(AbstractGeoObj1.GetComponent<AbstractSphere>(), AbstractGeoObj2.GetComponent<straightEdgeBehave>());
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

        private void updateSpherePlane(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SpherePlaneIntersection(AbstractGeoObj1.GetComponent<AbstractSphere>(), AbstractGeoObj2.GetComponent<flatfaceBehave>());
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

        private void updateSphereSphere(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereSphereIntersection(AbstractGeoObj1.GetComponent<AbstractSphere>(), AbstractGeoObj2.GetComponent<AbstractSphere>());
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

        private void updateCirclePlane(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CirclePlaneIntersection(AbstractGeoObj1.GetComponent<AbstractCircle>(), AbstractGeoObj2.GetComponent<flatfaceBehave>());
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

        private void updateSphereCircle(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.SphereCircleIntersection(AbstractGeoObj1.GetComponent<AbstractSphere>(), AbstractGeoObj2.GetComponent<AbstractCircle>());
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

        private void updateCircleCircle(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CircleCircleIntersection(AbstractGeoObj1.GetComponent<AbstractCircle>(), AbstractGeoObj2.GetComponent<AbstractCircle>());

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

        private void updateCircleLine(AbstractGeoObj mgo, AbstractGeoObj AbstractGeoObj1, AbstractGeoObj AbstractGeoObj2)
        {
            intersectionFigData data = IntersectionMath.CircleLineIntersection(AbstractGeoObj1.GetComponent<AbstractCircle>(), AbstractGeoObj2.GetComponent<straightEdgeBehave>());
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
			pastIntersections = new Dictionary<AbstractGeoObj, AbstractGeoObj[]>();
			hashIntersectedFigs = new HashSet<string>();
			hashIntersectionProducts = new HashSet<string>();

            //this way the intersectionmanager can be disabled but present in a scene.
            if (intersectionCheckEnabled)
            {
                StartCoroutine(checkAllIntersections(5f));
            }

            ins = this;
		}

		private void AddToDictionary(AbstractGeoObj obj1, AbstractGeoObj obj2, AbstractGeoObj[] objList)
		{
			if (objList != null && objList.Length > 0)
			{
				AbstractGeoObj[] inputList = new AbstractGeoObj[2];
				inputList[0] = obj1;
				inputList[1] = obj2;

				hashIntersectedFigs.Add(obj1.name+obj2.name);

				foreach (AbstractGeoObj obj in objList)
				{
					hashIntersectionProducts.Add(obj.name);
					pastIntersections.Add(obj, inputList);
					this.GetComponent<HW_GeoSolver>().AddDependence(obj, obj1);
					this.GetComponent<HW_GeoSolver>().AddDependence(obj, obj2);
				}

			}
		}

		private bool checkInDictionary(AbstractGeoObj obj1, AbstractGeoObj obj2)
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
			AbstractGeoObj[] geoObjList = GameObject.FindObjectsOfType<AbstractGeoObj>().Where(g => g.GetComponent<AbstractPoint>() == null).ToArray();

			for (int i = 0; i < geoObjList.Length; i++)
			{
				AbstractGeoObj obj1 = geoObjList[i];
				if (obj1.GetComponent<AnchorableBehaviour>() != null && (obj1.GetComponent<AnchorableBehaviour>().isAttached))
					continue;
				if(true)
				{
					for (int j = i + 1; j < geoObjList.Length; j++)
					{

						AbstractGeoObj obj2 = geoObjList[j];
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
		/// Given a pair of AbstractGeoObjs, check to see if they intersect.
		/// </summary>
		internal void checkIntersection(AbstractGeoObj obj1, AbstractGeoObj obj2)
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

        private AbstractGeoObj[] checkStraightedgeStraightedge(straightEdgeBehave straightEdgeBehave1, straightEdgeBehave straightEdgeBehave2)
        {
            intersectionFigData data = IntersectionMath.LineLineIntersection(straightEdgeBehave1, straightEdgeBehave2);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkFlatfaceFlatface(flatfaceBehave flatfaceBehave1, flatfaceBehave flatfaceBehave2)
        {
            intersectionFigData data = IntersectionMath.PlanePlaneIntersection(flatfaceBehave1, flatfaceBehave2);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.line)
            {
                Debug.LogWarning("Need to Construct Line");
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkFlatfaceStraightedge(flatfaceBehave flatfaceBehave, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.LinePlaneIntersection(flatfaceBehave, straightEdgeBehave);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkCircleFlatface(AbstractCircle abstractCircle, flatfaceBehave flatfaceBehave)
        {
            intersectionFigData data = IntersectionMath.CirclePlaneIntersection(abstractCircle, flatfaceBehave);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }else if (data.vectordata.Length == 2)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkCircleStraightEdge(AbstractCircle abstractCircle, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.CircleLineIntersection(abstractCircle, straightEdgeBehave);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private AbstractGeoObj[] CheckCircleCircle(AbstractCircle abstractCircle1, AbstractCircle abstractCircle2)
        {
            intersectionFigData data = IntersectionMath.CircleCircleIntersection(abstractCircle1, abstractCircle2);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkSpherecircle(AbstractSphere abstractSphere, AbstractCircle abstractCircle)
        {
            intersectionFigData data = IntersectionMath.SphereCircleIntersection(abstractSphere, abstractCircle);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkSphereFlatface(AbstractSphere abstractSphere, flatfaceBehave flatfaceBehave)
        {
            intersectionFigData data = IntersectionMath.SpherePlaneIntersection(abstractSphere, flatfaceBehave);

            AbstractGeoObj[] mgoResult = null;
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
                mgoResult = new AbstractGeoObj[] { centerPoint.setIntersectionFigure(0), edgePoint.setIntersectionFigure(1), newCircle.setIntersectionFigure(2) };
            }
            else if(data.figtype == GeoObjType.point)
            {
                mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]) };
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkSphereStraightEdge(AbstractSphere abstractSphere, straightEdgeBehave straightEdgeBehave)
        {
            intersectionFigData data = IntersectionMath.SphereLineIntersection(abstractSphere, straightEdgeBehave);

            AbstractGeoObj[] mgoResult = null;
            if (data.figtype == GeoObjType.point)
            {
                if (data.vectordata.Length == 1)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0) };
                }
                else if (data.vectordata.Length == 2)
                {
                    mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]).setIntersectionFigure(0), GeoObjConstruction.dPoint(data.vectordata[1]).setIntersectionFigure(1) };

                }
            }
            return mgoResult;
        }

        private AbstractGeoObj[] checkSphereSphere(AbstractSphere abstractSphere1, AbstractSphere abstractSphere2)
        {
            intersectionFigData data = IntersectionMath.SphereSphereIntersection(abstractSphere1, abstractSphere2);

			Debug.Log("Data produces " + data.figtype.ToString());
			Debug.Log("Point Value " + data.vectordata[0].ToString());

            AbstractGeoObj[] mgoResult = null;
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
                mgoResult = new AbstractGeoObj[] { centerPoint.setIntersectionFigure(0), edgePoint.setIntersectionFigure(0), newCircle.setIntersectionFigure(0) };
            }
            else if (data.figtype == GeoObjType.point)
            {
                mgoResult = new AbstractGeoObj[] { GeoObjConstruction.dPoint(data.vectordata[0]) };
            }
            return mgoResult;
        }
    }
}