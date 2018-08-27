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

    public class enableCameraSplitScreen : MonoBehaviour
    {

        public Camera firstPersonCamera;
        public Camera overheadCamera1;
        public Camera overheadCamera2;
        public Camera overheadCamera3;

        void Start()
        {
            if (!overheadCamera1.isActiveAndEnabled)
            {
                Debug.Log("show all camreas");
                ShowAllCameras();
            }
        }

        public void ShowAllCameras()
        {
            firstPersonCamera.enabled = true;
            overheadCamera1.enabled = true;
            overheadCamera2.enabled = true;
            overheadCamera3.enabled = true;
        }

        public void ShowFirstPersonView()
        {
            firstPersonCamera.enabled = true;
            overheadCamera1.enabled = false;
            overheadCamera2.enabled = false;
            overheadCamera3.enabled = false;
        }
    }
}
