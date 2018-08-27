/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class clearDrawings : MonoBehaviour
    {

        public void clearDraw()
        {
            foreach (GameObject drawing in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (drawing.gameObject.name.Contains("Line Object"))
                {
                    DestroyImmediate(drawing);
                }
            }
        }
        public void clearOneDraw(Vector3 position)
        {
            float bestDist = 100;
            Transform toDest = null;
            foreach (GameObject drawing in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (drawing.gameObject.name.Contains("Line Object"))
                {
                    Vector3 drawPos = drawing.GetComponent<SphereCollider>().center;

                    if (Vector3.Magnitude(drawPos - position) < bestDist) { }

                    bestDist = Vector3.Magnitude(drawPos - position);
                    toDest = drawing.gameObject.transform;
                }
            }

            if (bestDist < .5f)
            {
                toDest.transform.gameObject.SetActive(false);
            }
        }

		public void addColiders()
		{

			foreach (GameObject drawing in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if (drawing.gameObject.name.Contains("Line Object"))
				{
					drawing.AddComponent<SphereCollider>().isTrigger = true;
				}
			}
		}
    }
}
