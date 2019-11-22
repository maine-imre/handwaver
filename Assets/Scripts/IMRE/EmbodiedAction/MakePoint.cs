using System;
using IMRE.HandWaver.Kernel;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;

namespace IMRE.EmbodiedAction
{
    public class MakePoint : MonoBehaviour
    {
        private SteamVR_Behaviour_Pose trigger;

        private void Start()
        {
            trigger = GetComponent<SteamVR_Behaviour_Pose>();
            if (trigger.poseAction.changed && trigger.isValid)
            {
                float3 origin = trigger.origin.position;
                StartCoroutine(HandWaverServerTransport.execCommand(
                    "A = (" + origin.x + "," + origin.y + "," + origin.z + ")"));
                //TODO can the new obj be called something other than A?
            }
        }
    }
}
    
