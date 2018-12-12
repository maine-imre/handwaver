using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IMRE.HandWaver.Shearing
{
	/// <summary>
	/// This script is the main managing script for the ShearingLab scene in HandWaver.
 /// Build for Bock's MST Thesis.   
	/// The main contributor(s) to this script is CB
	/// Status: WORKING
	/// </summary>
	public class shearingLabManager : MonoBehaviour
    {

        public List<Transform> transforms = new List<Transform>();
        internal List<Transform> measurementDisplays = new List<Transform>();

        public static Transform TriangleParent;
        public static Transform PyramidParent0;
        public static Transform PyramidParent1;
        public static Transform PrismParent0;
        public static Transform PrismParent1;

        public static float height1 = 1.5f;
        public static float height2 = 1.2f;
        public static bool calibrated = false;
        private bool measurementOn = false;
		public float heightdiff = -.3f;

        private List<AbstractPoint> apexList = new List<AbstractPoint>();
        private Dictionary<AbstractPoint, Vector3> apexPos = new Dictionary<AbstractPoint, Vector3>();


		private void Awake()
		{
			TriangleParent = transforms[0];
			PyramidParent0 = transforms[1];
			PyramidParent1 = transforms[2];
			PrismParent0 = transforms[3];
			PrismParent1 = transforms[4];
		}

		/// <summary>
		/// before starting the shearing lab, calibrate height using a function key.
		/// </summary>
		private void calibrateHeight()
        {
            //set height 1 to 80 percent of HMD height
            //set height 2 to .3m less than HMD height

			//@Nathan, this is wrong somehow...
            float baseHeight = Camera.main.transform.position.y;
			if(baseHeight <= 0)
			{
				baseHeight = 1.5f;
			}
			Debug.Log(baseHeight);
            height1 = .3f * baseHeight;
            height2 = .8f*baseHeight;
            calibrated = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!calibrated)
                {
                    calibrateHeight();
                }
                setActive(TriangleParent);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!calibrated)
                {
                    calibrateHeight();
                }
                setActive(PyramidParent0);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                if (!calibrated)
                {
                    calibrateHeight();
                }
                setActive(PyramidParent1);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                if (!calibrated)
                {
                    calibrateHeight();
                }
                setActive(PrismParent0);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                if (!calibrated)
                {
                    calibrateHeight();
                }
                setActive(PrismParent1);
            }
            else if (Input.GetKeyDown(KeyCode.F10))
            {
				measurementOn = !measurementOn;
				for (int i = 0; i < measurementDisplays.Count; i++)
                {
                    setActive(measurementDisplays[i],measurementOn);
                }
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
				//if point is moving, stop it.
                if(TriangleParent.GetComponentInChildren<constructTriangleOnParallelLines>() != null)
					TriangleParent.GetComponentInChildren<constructTriangleOnParallelLines>().stopAnimatingPoint();
                if (PyramidParent0.GetComponentInChildren<constructPyramidOnParallelPlanes>() != null)
                    PyramidParent0.GetComponentInChildren<constructPyramidOnParallelPlanes>().stopAnimatingPoint();
                if (PyramidParent1.GetComponentInChildren<constructPyramidOnParallelPlanes>() != null)
                   PyramidParent1.GetComponentInChildren<constructPyramidOnParallelPlanes>().stopAnimatingPoint();

                foreach (AbstractPoint point in apexList)
                {
                    //resets apex points to initial positions
                    point.Position3 = apexPos[point];
                }
            }
        }

		internal void disableDisplays()
		{
			foreach(Transform display in measurementDisplays)
			{
				display.gameObject.SetActive(false);
			}
		}

		private void setActive(Transform t)
		{
			bool value = true;
			t.gameObject.SetActive(value);
		}

		private void setActive(Transform t,bool value)
        {
            t.gameObject.SetActive(value);
        }

        internal void addApexToList(AbstractPoint apex)
        {
            apexList.Add(apex);
            apexPos.Add(apex, apex.Position3);
        }
    }
}