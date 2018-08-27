/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class PointUIGen : MonoBehaviour
    {
        public int numberUI = 5;

        public Transform nextUI;

        public Transform rHand;
        public Transform lHand;

        private IEnumerator genNow;
        public bool generating;

        public List<Transform> listUI = new List<Transform>();
        public int nextUInum = 0;

        void Awake()
        {
            for (int idxUI = 0; idxUI < numberUI; idxUI++)
            {
                GameObject pointUI = Instantiate(Resources.Load("GeoUI/PointUI", typeof(GameObject))) as GameObject;
                pointUI.transform.parent = this.transform;
                listUI.Add(pointUI.transform);
            }
            nextUI = listUI[nextUInum];
        }


        void Update()
        {
            if ((nextUInum + 5 > listUI.Count) && !generating)
            {
                genNow = generator(.06f);
                StartCoroutine(genNow);
                generating = true;
            }
        }

        public void selectUI()
        {
            nextUInum = nextUInum + 1;
            nextUI = listUI[nextUInum];
        }

        private IEnumerator generator(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                GameObject pointUI = Instantiate(Resources.Load("GeoUI/PointUI", typeof(GameObject))) as GameObject;
                pointUI.transform.parent = this.transform;
                listUI.Add(pointUI.transform);
                nextUI = listUI[nextUInum];

                if (nextUInum + 15 < listUI.Count)
                {
                    generating = false;
                    StopCoroutine(genNow);
                }
            }
        }
    }
}