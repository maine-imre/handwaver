/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using Leap.Unity;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class SaveCSV : MonoBehaviour
    {

        public Transform LeftHand;
        public Transform lPinchDetect;
        public Transform RightHand;
        public Transform rPinchDetect;
        public Transform HMD_camera;
        public Transform blueCyl;
        public Transform redCyl;
        public Transform greenCyl;
        public Transform purpleCyl;
        public Transform purpleSphere;
        public Transform drawTool;

        private IEnumerator coroutine;

        void Start()
        {
            coroutine = Savecsv();
            StartCoroutine(coroutine);
        }

        private IEnumerator Savecsv()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);

                string filepath = @"C:/HW_Logs/save_dat.csv";
                string delimiter = ",";

                string[][] output = new string[][] {
                new string[]{"Time", Time.time.ToString () },
                new string[]{"LHand Pos", LeftHand.transform.position.ToString()},
                new string[]{"LHand Pinch", lPinchDetect.GetComponent<PinchDetector>().IsPinching.ToString()},
                new string[]{"RHand Pos", RightHand.transform.position.ToString()},
                new string[]{"RHand Pinch", rPinchDetect.GetComponent<PinchDetector>().IsPinching.ToString()},
                new string[]{"HMD Pos", HMD_camera.transform.position.ToString()},
                new string[]{"HMD Quaterion", HMD_camera.transform.rotation.ToString()},
                new string[]{"GCyl Pos", greenCyl.transform.position.ToString()},
                new string[]{"GCyl Quaterion", greenCyl.transform.rotation.ToString()},
                new string[]{"BCyl Pos", blueCyl.transform.position.ToString()},
                new string[]{"BCyl Quaterion", blueCyl.transform.rotation.ToString()},
                new string[]{"RCyl Pos", redCyl.transform.position.ToString()},
                new string[]{"RCyl Quaterion", redCyl.transform.rotation.ToString()},
                new string[]{"PCyl Pos", purpleCyl.transform.position.ToString()},
                new string[]{"PCyl Quaterion", purpleCyl.transform.rotation.ToString()},
                new string[]{"PSphere Pos", purpleSphere.transform.position.ToString()},
                new string[]{"PSphere Quaterion", purpleSphere.transform.rotation.ToString()},
				//new string[]{"Draw Enabled?", drawTool.transform.GetComponent<PinchDraw>().drawEnabled.ToString()}
			};

                int length = output.GetLength(0);

                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < length; index++)
                {
                    sb.AppendLine(string.Join(delimiter, output[index]));
                }

                File.AppendAllText(filepath, sb.ToString());
            }
        }
    }
}