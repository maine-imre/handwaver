using System.Collections;
using System.Collections.Generic;
using IMRE.EmbodiedUserInput;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class GreatArc : EmbodiedAction
    {
        public override void checkClassifier(EmbodiedClassifier classifier)
        {
            //I imagine this needing to call to two other scripts, each one dealing with each hand
            //and updating their side of the arc. However is there a way to use different types of 
            //classifiers in the same file? (can this be simplified to a single file or less than 3 to 4 scripts?
            throw new System.NotImplementedException();
        }
        
        public override void endAction(EmbodiedClassifier classifier)
        {
            throw new System.NotImplementedException();
        }
    }
}