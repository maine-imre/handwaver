using IMRE.EmbodiedUserInput;
using UnityEngine;

public class TestInput : EmbodiedAction
{
    public override void checkClassifier(EmbodiedClassifier classifier)
    {
        Debug.Log("checking thanks");
    }

    public override void endAction(EmbodiedClassifier classifier)
    {
        throw new System.NotImplementedException();
    }
}
