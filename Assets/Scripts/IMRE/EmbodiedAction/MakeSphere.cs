using IMRE.HandWaver.Kernel;
using IMRE.HandWaver.Kernel.Geos;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;

namespace IMRE.EmbodiedAction
{
    public class MakeSphere : MonoBehaviour
    {
        private SteamVR_Behaviour_Pose trigger;

        private void Start()
        {
            trigger = GetComponent<SteamVR_Behaviour_Pose>();
            if (trigger.poseAction.changed && trigger.isValid)
            {
                //TODO find geo elements for each hand
//                GeoElement geo1;
//                GeoElement geo2;
//                StartCoroutine(HandWaverServerTransport.execCommand(
//                    "A = Sphere(" + geo1.ElementName + "," + geo2.ElementName + ")");
//TODO can the new obj be called something other than A?
            }
        }
    }

}