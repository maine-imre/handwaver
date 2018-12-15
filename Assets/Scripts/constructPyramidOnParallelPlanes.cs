using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using TMPro;
using System;
using Leap.Unity.Interaction;
using Leap.Unity;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver.Shearing
{
    /// <summary>
    /// Constructs a pyramid with a given number of sides bound to parallel planes for Bock's MST Thesis.
    /// </summary>
    public class constructPyramidOnParallelPlanes : MonoBehaviour
    {
        [Range(3, 10)]
        public int nSides = 4;

        public Color mgoColor;

		public InteractionBehaviour volumeModel;
		public InteractionBehaviour surfaceAreaModel;
		public InteractionBehaviour crossSectionModel;


        private DependentPyramid myPyramid;

        public float interactionRadius = .5f;

		private TextMeshPro _tmpro;
        private TextMeshPro tmpro
		{
			get
			{
				if (_tmpro != null)
				{
					return _tmpro;
				}
				else
				{
					_tmpro = GetComponentInChildren<TextMeshPro>();
					return _tmpro;
				}
			}
		}

        public MeshFilter mf;

        public enum pyramidCrossMode { PlaneIntersection, RiemanApprox };

        public pyramidCrossMode myMode = pyramidCrossMode.PlaneIntersection;

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
        public bool overrideHandInput = false;

        [Range(0,100)]
        public int divisions = 5;

        private PalmDirectionDetector[] palmDetectors;

		private IEnumerator updateMesh;

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

		public Mesh myMesh;
        private float perciseRange = 1.5f;
        private IEnumerator movePointOnLine;
		private Coroutine currentMovePointOnLine;
        private flatlandSurface flatface1;
        private flatlandSurface flatface2;

        internal void Start()
        {

			shearingLabManager labMan = FindObjectOfType<shearingLabManager>();
			labMan.measurementDisplays.Add(surfaceAreaModel.transform);
			labMan.measurementDisplays.Add(volumeModel.transform);
			//labMan.measurementDisplays.Add(crossSectionModel.transform);
			labMan.disableDisplays();

			surfaceAreaModel.GetComponent<MeshRenderer>().materials[0].color = mgoColor;
			volumeModel.GetComponent<MeshRenderer>().materials[0].color = mgoColor;
			crossSectionModel.GetComponent<MeshRenderer>().materials[0].color = mgoColor;
			//copy into base scene on Load.
			//this.transform.parent = SceneManager.GetSceneByName("HandWaverBase").GetRootGameObjects()[0].transform.parent;

			//construct parallel planes

			flatface1 = PoolManager.Pools["Tools"].Spawn("flatlandSurface", Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height1, Quaternion.identity, this.transform).GetComponent<flatlandSurface>();
			flatface2 = PoolManager.Pools["Tools"].Spawn("flatlandSurface", Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2, Quaternion.identity, this.transform).GetComponent<flatlandSurface>();

			flatface1.otherFlatlandSurface = flatface2;
			flatface2.otherFlatlandSurface = flatface1;

			flatface1.transform.localScale = new Vector3 ( 3f, .0001f, 3f );
			flatface2.transform.localScale = new Vector3 ( 3f, .0001f, 3f );

			AbstractPoint apexPoint = GeoObjConstruction.iPoint(Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2);
            AbstractPolygon basePoly = GeoObjConstruction.rPoly(nSides, .3f, Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height1);

            myPyramid = GeoObjConstruction.dPyramid(basePoly, apexPoint);

            foreach (AbstractLineSegment line in myPyramid.allEdges) { line.LeapInteraction = false; }
			foreach(AbstractPoint point in basePoly.pointList)
			{
				point.LeapInteraction = false;
				flatface1.attachedObjs.Add(point);
			}
            foreach(AbstractPolygon face in myPyramid.allfaces)
            {
                face.LeapInteraction = false;
            }
			flatface2.attachedObjs.Add(apexPoint);
            flatface2.attachedObjs.Add(basePoly);

            myPyramid.GetComponent<InteractionBehaviour>().enabled = false;

            mf.transform.position = Vector3.zero;
            mf.transform.localScale = Vector3.one;
            mf.transform.rotation = Quaternion.identity;

            palmDetectors = GetComponentsInChildren<PalmDirectionDetector>();

			//NATHAN HELP ME HERE
            for (int i = 0; i < palmDetectors.Length; i++)
            {
                PalmDirectionDetector palm = palmDetectors[i];

				palm.OnActivate.AddListener(startUpdateMesh);
                palm.OnDeactivate.AddListener(endUpdateMesh);

                ExtendedFingerDetector finger = palm.GetComponent<ExtendedFingerDetector>();

				finger.OnDeactivate.AddListener(endUpdateMesh);
				//the extended finger detectors are set to enable/disable the respective PalmDirectionDetectors.

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
			}

			updateMesh = updateMeshRoutine();
            if (overrideHandInput)
            {
                startUpdateMesh();
            }

            foreach(AbstractPolygon face in myPyramid.allfaces)
            {
                face.figColor = mgoColor;
            }

            myPyramid.apex.GetComponent<InteractionBehaviour>().OnGraspEnd += constantVelOutOfRange;
            labMan.addApexToList(myPyramid.apex);

        }

		internal void stopAnimatingPoint()
		{
			myPyramid.apex.Position3 = Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2;
			StopCoroutine(currentMovePointOnLine);
		}

		private void LateUpdate()
		{

				myMesh = mf.GetComponent<MeshFilter>().mesh;

				switch (myMode)
				{
					case pyramidCrossMode.PlaneIntersection:
					// log this
					myMesh.vertices = PyramidPlaneIntersect(crossSectionHeight);
					//log that
						myMesh.triangles = polygonTriangles(myMesh.vertices.Length);
						//myMesh.RecalculateNormals();
						//myMesh.RecalculateTangents();
						break;

					case pyramidCrossMode.RiemanApprox:
						myMesh.vertices = RiemanApproxVertices(divisions);
						myMesh.triangles = RiemanApproxTriangles(divisions);
						myMesh.RecalculateNormals();
						myMesh.RecalculateTangents();
						break;
				}
		}

		private void endUpdateMesh()
		{
			StopCoroutine(updateMesh);
		}

		private void startUpdateMesh()
		{
			StartCoroutine(updateMesh);
		}

		private void Update()
        {
			volumeModel.transform.localScale = Vector3.one * Mathf.Pow(volume, 1f/3f);
			surfaceAreaModel.transform.localScale = Vector3.one * Mathf.Pow(surfaceArea,1f/2f) * .1f;
			crossSectionModel.transform.localScale = Vector3.one * Mathf.Pow(crossSectionArea,1f/2f) * .1f;
		}

   //     private string infoString
   //     {
   //         get
			//{
			//	return "Volume :  " + volume + "    Surface Area :  " + surfaceArea + "    Perimeter :  " + perimeter + "   CrossSection :  " + crossSectionArea;
   //         }
   //     }

        private Vector3[] PyramidPlaneIntersect(float planeHeight)
        {
            //find an intersection in a parallel plane.

            Vector3[] vertices = new Vector3[myPyramid.basePolygon.pointList.Count];

			Vector3 planePos = Vector3.up*planeHeight;
			Vector3 planeNorm = Vector3.up;

			foreach (InteractableLineSegment line in myPyramid.allEdges)
            {
                if (line.point1 == myPyramid.apex)
                {
                    int idx = myPyramid.basePolygon.pointList.IndexOf(line.point2);
                    //LinePlane intersection uses LinesPos and LineDir.  
					//vertices[idx] = IntersectionMath.LinePlaneIntersection(line.vertex0, line.vertex1-line.vertex0, planePos,planeNorm).vectordata[0];

                    //SegmentPlaneIntersection uses LinePos1 and LinePos2
                    vertices[idx] = IntersectionMath.SegmentPlaneIntersection(line.vertex0, line.vertex1, planePos, planeNorm).vectordata[0];
                }
                else if (line.point2 == myPyramid.apex)
                {
                    int idx = myPyramid.basePolygon.pointList.IndexOf(line.point1);
					//vertices[idx] = IntersectionMath.LinePlaneIntersection(line.vertex0, line.vertex1-line.vertex0, planePos, planeNorm).vectordata[0];
                    vertices[idx] = IntersectionMath.SegmentPlaneIntersection(line.vertex0, line.vertex1, planePos, planeNorm).vectordata[0];
                }
            }
            return vertices;
        }

        private Vector3[] PyramidPlaneIntersectionProjectionDown(float delta,float lastDelta)
        {
			Vector3[] result = PyramidPlaneIntersect(delta);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Vector3.ProjectOnPlane(result[i] - myPyramid.basePolygon.center + myPyramid.basePolygon.normDir * lastDelta, myPyramid.basePolygon.normDir) + myPyramid.basePolygon.center + myPyramid.basePolygon.normDir * lastDelta;
            }
            return result;
        }

        private static int[] polygonTriangles(int sideNum)
        {
            int[] result = new int[(sideNum - 2) * 3];
            for (int i = 0; i < sideNum-2; i++)
            {
                result[3 * i] = 0;
                result[3 * i + 1] = i+1;
                result[3 * i + 2] = i + 2;
            }
            return result;
        }

        private Vector3[] RiemanApproxVertices(int divisions)
        {
            int numSides = myPyramid.basePolygon.pointList.Count;
            Vector3[] result = new Vector3[2*divisions * numSides];
            for (int i = 0; i < divisions; i++)
            {
                PyramidPlaneIntersect(i / divisions).CopyTo(result, 2 * i * numSides);
                PyramidPlaneIntersectionProjectionDown(i+1 / divisions, i / divisions).CopyTo(result, (2 * i + 1) * numSides);
            }
            return result;
        }

        private int[] RiemanApproxTriangles(int divisions)
        {
            int numSides = myPyramid.basePolygon.pointList.Count;
            int[] result = new int[numSides * 3 * divisions * 4];

            for (int i = 0; i < 2*divisions; i++)
            {
                annulusTriangess(numSides,i).CopyTo(result, numSides * 6 * i);
            }
            return result;
        }

        private int[] annulusTriangess(int numSides, int idx)
        {
            int[] result = new int[numSides * 6];

            //make a quad
            for (int i = 0; i < numSides; i++)
            {
                //[0,1,3]
                //[0.0,1.0,1.1]

                result[6 * i] = i + idx;
                result[6 * i + 1] = i + numSides + idx;
                result[6 * i + 2] = i + 1 + idx;
                if(i+1 == numSides)
                {
                    result[6 * i + 2] = idx;
                }

                //[1,2,3]
                //[1.0,1.1,0.1]

                result[6 * i + 3] = i+numSides + idx;
                result[6 * i + 4] = i + 1 + numSides + idx;
                result[6 * i + 5] = i + 1 + idx;
                if(i+1 == numSides)
                {
                    result[6 * i + 4] = numSides + idx;
                    result[6 * i + 5] = idx;
                }
            }
            return result;
        }

		private IEnumerator updateMeshRoutine()
		{
			while (true)
			{
				if (!overrideHandInput)
				{

					crossSectionHeight = findPalmHeight();
				}

				yield return new WaitForEndOfFrame();
			}
		}

        private float findPalmHeight()
        {
			float dist = Mathf.Infinity;
			float value = 0f;
			foreach (PalmDirectionDetector palm in palmDetectors.Where(p => p.IsActive == true))
            {
				Vector3 palmPos = palm.HandModel.GetComponent<RiggedHand>().palm.transform.position;
				if (Vector3.ProjectOnPlane(palmPos - this.transform.position, Vector3.up).magnitude < dist)
					value = palmPos.y;
            }
			return value;
        }

		private bool checkPalmInBox(Vector3 palmPos)
		{
			return checkFloatInRange(palmPos.x, this.transform.position.x+1.5f, this.transform.position.x-1.5f) && checkFloatInRange(palmPos.y, height2,height1) && checkFloatInRange(palmPos.z,this.transform.position.z+1.5f,this.transform.position.z-1.5f) ;
		}

		private bool checkFloatInRange(float x, float xMax, float xMin)
		{
			return (x > xMin && x < xMax) || (x<xMin && x > xMax);
		}

		private Vector3[] RiemanApproxVertices(object divisions)
        {
            throw new NotImplementedException();
        }

        internal float surfaceArea
        {
            get
            {
                float sum = 0f;
                foreach(AbstractPolygon face in myPyramid.allfaces)
                {
                    sum += face.area;
                }
				return sum;
            }
        }

        internal float volume
        {
            get
			{ 
                return myPyramid.basePolygon.area *Mathf.Abs(height1-height2)/3f;
			}
        }

        internal float perimeter
        {
            get
            {
                float sum = 0f;
                foreach (AbstractLineSegment line in myPyramid.allEdges)
                {
                    sum += (line.vertex0 - line.vertex1).magnitude;
                }
				return sum;
            }
        }

        internal float crossSectionArea
        {
            get
            {
				if (myPyramid.basePolygon.area != float.NaN)
				{
					return myPyramid.basePolygon.area * (crossSectionHeight / (height2 - height1));
				}
				else
				{
					return 0f;
				}
            }
        }

        /// <summary>
        /// when thrown outside of the range, it will run to infinity at 2.0 meters per second.
        /// </summary>
        private void constantVelOutOfRange()
        {
            if ((myPyramid.apex.Position3 - myPyramid.basePolygon.center).magnitude > perciseRange)
            {
                Debug.Log("GO TO INFINITY AND BEYOND!");
                if (flatface2.attachedObjs.Contains(myPyramid.apex))
                {
                    flatface2.attachedObjs.Remove(myPyramid.apex);
                }
				if(currentMovePointOnLine != null)
                StopCoroutine(currentMovePointOnLine);
                movePointOnLine = animatePoint(myPyramid.apex, Vector3.ProjectOnPlane((myPyramid.apex.Position3 - myPyramid.basePolygon.center), Vector3.up).normalized);
                currentMovePointOnLine = StartCoroutine(movePointOnLine);
            }
            else
            {
				if (currentMovePointOnLine != null) 
                StopCoroutine(currentMovePointOnLine);
                if (!flatface2.attachedObjs.Contains(myPyramid.apex))
                {
                    flatface2.attachedObjs.Add(myPyramid.apex);
                }
            }
        }

        private IEnumerator animatePoint(AbstractPoint point, Vector3 lineDir)
        {
            while (true)
            {
                point.Position3 += lineDir * Time.smoothDeltaTime * 2f;
                point.AddToRManager();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}