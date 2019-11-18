using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace IMRE.EmbodiedAction
{
    public abstract class AbstractOneHandedAction : MonoBehaviour
    {
        public int frequency;
        public SteamVR_Action_Boolean trigger;

        public SteamVR_Input_Sources handTypeL;
        public SteamVR_Input_Sources handTypeR;

        private IEnumerator _actionRoutineL;
        private IEnumerator _actionRoutineR;

        // Start is called before the first frame update
        private void Start()
        {
            trigger.AddOnStateUpListener(StartAction, handTypeL);
            trigger.AddOnStateUpListener(StartAction, handTypeR);

            trigger.AddOnStateDownListener(EndAction, handTypeL);
            trigger.AddOnStateDownListener(EndAction, handTypeR);

            _actionRoutineL = ActionRoutine(handTypeL);
            _actionRoutineR = ActionRoutine(handTypeR);
        }

        private void StartAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (fromSource == handTypeL)
                StartCoroutine(_actionRoutineL);
            if (fromSource == handTypeR)
                StartCoroutine(_actionRoutineR);
        }

        private void EndAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (fromSource == handTypeL)
                StopCoroutine(_actionRoutineL);

            if (fromSource == handTypeR)
                StopCoroutine(_actionRoutineR);
        }

        IEnumerator ActionRoutine(SteamVR_Input_Sources fromSource)
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f / (float) frequency);
                ActionImplementation(fromSource);
            }
        }

        public abstract void ActionImplementation(SteamVR_Input_Sources fromSource);
    }
}