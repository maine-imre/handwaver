using System;
using IMRE.HandWaver.Kernel;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace IMRE.EmbodiedAction
{
    public class MakePoint : MonoBehaviour
    {
        public SteamVR_Action_Boolean makePointAction;
        
        public Hand hand;


        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (makePointAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned", this);
                return;
            }

            makePointAction.AddOnChangeListener(OnMakePointActionChange, hand.handType);
        }
        
        private void OnDisable()
        {
            if (makePointAction != null)
                makePointAction.RemoveOnChangeListener(OnMakePointActionChange, hand.handType);
        }
        
        private void OnMakePointActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                SpawnPoint();
            }
        }

        private void SpawnPoint()
        {
                float3 origin = hand.transform.position;
                StartCoroutine(HandWaverServerTransport.execCommand(
                    "A = (" + origin.x + "," + origin.y + "," + origin.z + ")"));
                //TODO can the new obj be called something other than A?
        }
    }
}
    
