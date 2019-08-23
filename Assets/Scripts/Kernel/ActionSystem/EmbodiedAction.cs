using System.Linq;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public abstract class EmbodiedAction : MonoBehaviour
    {
        public classifierType type;

        private void Update()
        {
            EmbodiedUserInputClassifierAuthoring.classifiers.ToList().Where(c => c.type == type && c.isEligible)
                .ToList().ForEach(checkClassifier);
            EmbodiedUserInputClassifierAuthoring.classifiers.ToList().Where(c => c.type == type && c.wasCancelled)
                .ToList().ForEach(endAction);
        }

        //Note that this doesn't imply that there is an actionable action, only an eligible classifier
        public abstract void checkClassifier(EmbodiedClassifier classifier);

        //Note that this could be called without an action to end
        public abstract void endAction(EmbodiedClassifier classifier);
    }
}