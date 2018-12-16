using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;
using Leap.Unity;
using UnityEngine.SceneManagement;
using System.Linq;
using IMRE.HandWaver.Solver;
using Leap.Unity.Interaction;
using Leap.Unity.Interaction.Internal;

namespace IMRE.HandWaver.Shearing
{ 
	/// <summary>
	/// Constructs a triangle bound to two parallel lines for Bock's MST Thesis.
	/// </summary>
    public class constructTriangleOnParallelLines : MonoBehaviour
    {
        public Color mgoColor;

		public InteractionBehaviour areaModel;
		public InteractionBehaviour crossSectionModel;
		public InteractionBehaviour perimeterModel;

		private float _crossSectionHeight;

		// log this
		public float crossSectionHeight
		{
			get
			{
				if (height1 < _crossSectionHeight || height2 > _crossSectionHeight)
				{
					return _crossSectionHeight;
				}
				else
				{
					return height1;
				}
			}
			set
			{
				if (height2 > value && height1 < value)
				{
					_crossSectionHeight = value;
				}
			}
		}
		internal parallelLines line1;
        internal parallelLines line2;

        private AbstractLineSegment l1;
        private AbstractLineSegment l2;

        internal List<AbstractPoint> points = new List<AbstractPoint>();
        internal List<AbstractLineSegment> lines = new List<AbstractLineSegment>();
        internal AbstractPolygon triangle;

		//private TextMeshPro _tmpro;
		//private TextMeshPro tmpro
		//{
		//	get
		//	{
		//		if (_tmpro != null)
		//		{
		//			return _tmpro;
		//		}
		//		else
		//		{
		//			_tmpro = GetComponentInChildren<TextMeshPro>();
		//			return _tmpro;
		//		}
		//	}
		//}

		private LineRenderer myLR;

        private PalmDirectionDetector[] palmDetectors;

        private IEnumerator updateLR;

        public float height1
        {
            get
            {
                return shearingLabManager.height1;
            }
        }
        public float height2
        {
            get
            {
                return shearingLabManager.height2;
            }
        }
        public bool overridePalmDetector  =false;
        public float interactionRadius = 0.5f;
        [Range(0,2)]
        public float perciseRange = 1.5f;
		private bool releaseToInfinity = false;
		internal IEnumerator movePointOnLine;
		private Coroutine currentMPOL;
		private bool followLineDirection;

		internal void Start()
        {
			shearingLabManager labMan = FindObjectOfType<shearingLabManager>();
			labMan.measurementDisplays.Add(areaModel.transform);
			labMan.measurementDisplays.Add(perimeterModel.transform);
			//labMan.measurementDisplays.Add(crossSectionModel.transform);
			labMan.disableDisplays();

			areaModel.GetComponent<MeshRenderer>().materials[0].color = mgoColor;
			perimeterModel.GetComponent<LineRenderer>().materials[0].color = mgoColor;
			crossSectionModel.GetComponent<LineRenderer>().materials[0].color = mgoColor;


			line1 = PoolManager.Pools["Tools"].Spawn("ParallelLines").GetComponent<parallelLines>();
            parallelLines line2 = PoolManager.Pools["Tools"].Spawn("ParallelLines").GetComponent<parallelLines>();

            line1.Position3 = Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2;
            line2.Position3 = Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height1;

            line1.otherLine = line2;
            line2.otherLine = line1;

            AbstractPoint p1 = GeoObjConstruction.iPoint(line1.Position3);
            AbstractPoint p2 = GeoObjConstruction.iPoint(line2.Position3);
            AbstractPoint p3 = GeoObjConstruction.iPoint(line2.Position3 + line2.normalDir * 0.3f);

            p1.GetComponent<InteractionBehaviour>().OnGraspedMovement += checkOutOfRange;
            p1.GetComponent<InteractionBehaviour>().OnGraspEnd += constantVelOutOfRange;

            labMan.addApexToList(p1);
            //line not on Parallel
            l1 = GeoObjConstruction.dLineSegment(p1, p2);
            //line not on parallel
            l2 = GeoObjConstruction.dLineSegment(p1, p3);
            //line on parallel
            AbstractLineSegment l3 = GeoObjConstruction.dLineSegment(p3, p2);

            line1.attachedObjs.Add(p1);
            line2.attachedObjs.Add(p2);
            line2.attachedObjs.Add(p3);

            points.Add(p1);
            points.Add(p2);
            points.Add(p3);

            lines.Add(l1);
            lines.Add(l2);
            lines.Add(l3);

            triangle = GeoObjConstruction.dPolygon(lines, points);

            foreach(AbstractLineSegment line in lines)
            {
                line.GetComponent<InteractionBehaviour>().enabled = false;
            }

            triangle.GetComponent<InteractionBehaviour>().enabled = false;

            myLR = GetComponent<LineRenderer>();

            palmDetectors = GetComponentsInChildren<PalmDirectionDetector>();

            for (int i = 0; i < palmDetectors.Length; i++)
            {
                PalmDirectionDetector palm = palmDetectors[i];
                palm.OnActivate.AddListener(startUpdateMesh);
                palm.OnDeactivate.AddListener(endUpdateMesh);

                ExtendedFingerDetector finger = palm.GetComponent<ExtendedFingerDetector>();
                finger.OnDeactivate.AddListener(endUpdateMesh);

				switch (i)
				{
					case 0:
						palm.HandModel = leapHandDataLogger.ins.currHands.Lhand_rigged;
						finger.HandModel = leapHandDataLogger.ins.currHands.Lhand_rigged;
						break;
					case 1:
						palm.HandModel = leapHandDataLogger.ins.currHands.RHand_rigged;
						finger.HandModel = leapHandDataLogger.ins.currHands.RHand_rigged;
						break;
					default:
						break;
				}
                //the extended finger detectors are set to enable/disable the respective PalmDirectionDetectors.
            }

            updateLR = updateLR_Routine();

            if (overridePalmDetector)
            {
                startUpdateMesh();
            }

            triangle.figColor = mgoColor;

			movePointOnLine = animatePoint(points[0]);
        }

		internal void stopAnimatingPoint()
		{
			points[0].Position3 = Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2;
			StopCoroutine(currentMPOL);
		}

		private IEnumerator animatePoint(AbstractPoint point)
		{
			while (true)
			{
				Vector3 lineDir = line1.normalDir.normalized;
				if (!followLineDirection)
				{
					lineDir = -lineDir;
				}
				point.Position3 += lineDir * Time.smoothDeltaTime * 2f;
				point.AddToRManager();
				yield return new WaitForEndOfFrame();
			}
		}

        /// <summary>
        /// when thrown outside of the range, it will run to infinity at 2.0 meters per second.
        /// </summary>
        private void constantVelOutOfRange()
        {
			//if ((points[0].Position3 - line1.Position3).magnitude > perciseRange && points[0].GetComponent<Rigidbody>().velocity.magnitude > 1.0f)
			if((points[0].Position3 - line1.Position3).magnitude > perciseRange)
			{
				followLineDirection = Vector3.Dot(points[0].Position3 - line1.Position3,line1.normalDir) > 0;
				releaseToInfinity = true;
				Debug.Log("GO TO INFINITY AND BEYOND!");
				if (line1.attachedObjs.Contains(points[0]))
				{
					line1.attachedObjs.Remove(points[0]);
				}
				currentMPOL = StartCoroutine(movePointOnLine);
			}
			else
			{
				releaseToInfinity = false;
				if(currentMPOL != null)
					StopCoroutine(currentMPOL);
				if (!line1.attachedObjs.Contains(points[0]))
				{
					line1.attachedObjs.Add(points[0]);
				}
			}
        }


        /// <summary>
        /// Only use this on the first point
        /// this should allow you to throw the point towards infinity.  the drag is zero, so we're good to go.
        /// </summary>
        /// <param name="oldPosition"></param>
        /// <param name="oldRotation"></param>
        /// <param name="newPosition"></param>
        /// <param name="newRotation"></param>
        /// <param name="graspingControllers"></param>
        private void checkOutOfRange(Vector3 oldPosition, Quaternion oldRotation, Vector3 newPosition, Quaternion newRotation, List<InteractionController> graspingControllers)
        {
            if ((newPosition - line1.Position3).magnitude > perciseRange)
            {
                points[0].GetComponent<Rigidbody>().isKinematic = false;
                points[0].GetComponent<InteractionBehaviour>().graspedMovementType = InteractionBehaviour.GraspedMovementType.Nonkinematic;
            }
            else
            {
                points[0].GetComponent<Rigidbody>().isKinematic = true;
                points[0].GetComponent<InteractionBehaviour>().graspedMovementType = InteractionBehaviour.GraspedMovementType.Kinematic;
            }
        }

        private void endUpdateMesh()
        {
            StopCoroutine(updateLR);
        }

        private void startUpdateMesh()
        {
            StartCoroutine(updateLR);
        }

		private void Update()
		{

			myLR.SetPositions(findCrossSectionSegment());

			areaModel.transform.localScale = Vector3.one * Mathf.Pow(triangleArea,1f/2f) * .1f;
			perimeterModel.transform.localScale = Vector3.one * trianglePerimeter;
			crossSectionModel.transform.localScale = Vector3.one * triangleCrossSection;
		}

		private IEnumerator updateLR_Routine()
        {
            while (true)
            {
                if (!overridePalmDetector)
                {
                    crossSectionHeight = findPalmHeight();
                }

                yield return new WaitForEndOfFrame();
            }
        }

		private float findPalmHeight()
		{
			foreach (PalmDirectionDetector palm in palmDetectors.Where(p => p.IsActive == true))
			{
				Vector3 palmPos = palm.HandModel.GetComponent<RiggedHand>().palm.transform.position;
				//consider adding another condition for proximity to station.
				if (Vector3.Angle(palm.PointingDirection, Vector3.down) < palm.OffAngle && checkPalmInBox(palmPos))
				{
					return palmPos.y;
				}
			}
			return 0f;
		}

		private bool checkPalmInBox(Vector3 palmPos)
		{
			return checkFloatInRange(palmPos.x, this.transform.position.x + .5f, this.transform.position.x - .5f) && checkFloatInRange(palmPos.y, height2, height1) && checkFloatInRange(palmPos.z, this.transform.position.z + .5f, this.transform.position.z - .5f);
		}

		private bool checkFloatInRange(float x, float xMax, float xMin)
		{
			return (x > xMin && x < xMax) || (x < xMin && x > xMax);
		}

		// log this
		public Vector3[] findCrossSectionSegment()
        {
			if (IntersectionMath.SegmentPlaneIntersection(l1.vertex0, l1.vertex1, Vector3.up * crossSectionHeight, Vector3.up).figtype == GeoObjType.point && IntersectionMath.SegmentPlaneIntersection(l2.vertex0, l2.vertex1, Vector3.up * crossSectionHeight, Vector3.up).figtype == GeoObjType.point)
			{

				Vector3 vertex0 = IntersectionMath.SegmentPlaneIntersection(l1.vertex0, l1.vertex1, Vector3.up * crossSectionHeight, Vector3.up).vectordata[0];
				Vector3 vertex1 = IntersectionMath.SegmentPlaneIntersection(l2.vertex0, l2.vertex1, Vector3.up * crossSectionHeight, Vector3.up).vectordata[0];
				return new Vector3[] { vertex0, vertex1 };
			}
			else
			{
				//cross section height is erroring.
				Vector3 vertex0 = IntersectionMath.SegmentPlaneIntersection(l1.vertex0, l1.vertex1, Vector3.up * (height1+.00001f), Vector3.up).vectordata[0];
				Vector3 vertex1 = IntersectionMath.SegmentPlaneIntersection(l2.vertex0, l2.vertex1, Vector3.up * (height1 + .00001f), Vector3.up).vectordata[0];
				return new Vector3[] { vertex0, vertex1 };
			}
		}

		internal float triangleCrossSection
		{
			get
			{
				Vector3[] crossSectTriangle = findCrossSectionSegment();
				return (crossSectTriangle[0] - crossSectTriangle[1]).magnitude;
			}
		}

        internal float triangleArea
        {
            get
            {
                return triangle.area;
            }
        }

        internal float trianglePerimeter
        {
            get
            {
                float sum = 0f;
                foreach(AbstractLineSegment line in lines)
                {
                    sum += (line.vertex0 - line.vertex1).magnitude;
                }
				return sum;
            }
        }
    }
}