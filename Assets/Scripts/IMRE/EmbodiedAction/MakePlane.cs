using IMRE.HandWaver.Kernel;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace IMRE.EmbodiedAction
{
    public class MakePlane : MonoBehaviour
    {
        public SteamVR_Action_Boolean Action;
        
        public Hand hand;
        
        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (Action == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned", this);
                return;
            }

            Action.AddOnChangeListener(OnMakePointActionChange, hand.handType);
        }
        
        private void OnDisable()
        {
            if (Action != null)
                Action.RemoveOnChangeListener(OnMakePointActionChange, hand.handType);
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
            float3 normal = hand.transform.up; //TODO check this
            
            Debug.Log("Plane at : " + origin + " : " + normal);
            
            StartCoroutine(HandWaverServerTransport.execCommand(
                elementNameManager.GenerateName()+" = Plane(" + origin.x + "," + origin.y + "," + origin.z + "," +
                ", " + normal.x + "," + normal.y + "," + normal.z + ")"));
            //TODO can the new obj be called something other than A?
        }
    }
}
