using Enumerable = System.Linq.Enumerable;

namespace IMRE.EmbodiedUserInput
{
  public abstract class EmbodiedAction : UnityEngine.MonoBehaviour{
      public classifierType type;
  
      private void Update()
      {
          Enumerable.ToList(Enumerable.Where(Enumerable.ToList(EmbodiedUserInputClassifierAuthoring.classifiers),
              c => c.type == type && c.isEligible)).ForEach(classifier => checkClassifier(classifier));
          Enumerable.ToList(Enumerable.Where(Enumerable.ToList(EmbodiedUserInputClassifierAuthoring.classifiers),
            c => c.type == type && c.wasCancelled)).ForEach(classifier => endAction(classifier));
      }

      //Note that this doesn't imply that there is an actionable action, only an eligible classifier
      public abstract void checkClassifier(EmbodiedClassifier classifier);

      //Note that this could be called without an action to end
      public abstract void endAction(EmbodiedClassifier classifier);
    }
}
