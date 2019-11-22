using IMRE.HandWaver.Kernel;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;

namespace IMRE.EmbodiedAction
{
    public class MakePlane : MonoBehaviour
    {
        private SteamVR_Behaviour_Pose trigger;

        private void Start()
        {
            trigger = GetComponent<SteamVR_Behaviour_Pose>();
            if (trigger.poseAction.changed && trigger.isValid)
            {
                float3 origin = trigger.origin.position;
                float3 normal = trigger.origin.up; //TODO check this
                StartCoroutine(HandWaverServerTransport.execCommand(
                    "A = Plane(" + origin.x + "," + origin.y + "," + origin.z + "," +
                    ", " + normal.x + "," + normal.y + "," + normal.z + ")"));
                //TODO can the new obj be called something other than A?
            }
        }
    }
}
