using IMRE.EmbodiedUserInput;
using IMRE.Math;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.Space
{

    public abstract class PinFunctions : EmbodiedAction
    {
		public float tolerance = .05f;
    		public float angleTolerance = 15f;
    		private float desiredAngle = 0f;
    		
        public override void checkClassifier(EmbodiedClassifier classifier)
        {
        		float bestDist = tolerance;
        		pinData pin = default(pinData);
        		//find closest point
        		for(int i = 0; i < RSDESManager.ins.pinnedPoints.Count; i++){
				if ((Operations.magnitude(classifier.origin - (float3) (RSDESManager.ins.pinnedPoints[i].contactPoint)) < bestDist) &&
				                         (Operations.Angle(classifier.direction,
					                         pinDirection(RSDESManager.ins.pinnedPoints[i].pin)) < angleTolerance))
				{
					pin = RSDESManager.ins.pinnedPoints[i];
					bestDist = Operations.magnitude(classifier.origin - (float3) RSDESManager.ins.pinnedPoints[i].contactPoint);
				}
			}
			if(!pin.Equals(default(pinData))){
				pinFunction(pin.pin, classifier);
			}
        }

        public override void endAction(EmbodiedClassifier classifier)
        {
            return;
        }
        
        public abstract void pinFunction(RSDESPin pin, EmbodiedClassifier classifier);
        public abstract float3 pinDirection(RSDESPin pin);
    }
}
