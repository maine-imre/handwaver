using System.Linq;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public abstract class EmbodiedAction : MonoBehaviour
    {
        public classifierType type;
        
        private IEnumerator coroutine;
        
        //TODO consider making frequency adaptive.
        public int frequency = 30;
        private float deltaTime => 1f/((float) frequency);
        
        private void Start(){
            coroutine = RunClassifiers(deltaTime);
            StartCoroutine(coroutine);
        }


        private IEnumerator RunClassifiers(float deltaTime)
        {
            while(true)
            {
            yield return new WaitForSeconds(waitTime);
            EmbodiedUserInputClassifierAuthoring.classifiers.ToList().Where(c => c.type == type && c.isEligible)
                .ToList().ForEach(checkClassifier);
            EmbodiedUserInputClassifierAuthoring.classifiers.ToList().Where(c => c.type == type && c.wasCancelled)
                .ToList().ForEach(endAction);
           }
        }

        //Note that this doesn't imply that there is an actionable action, only an eligible classifier
        public abstract void checkClassifier(EmbodiedClassifier classifier);

        //Note that this could be called without an action to end
        public abstract void endAction(EmbodiedClassifier classifier);
    }
}
