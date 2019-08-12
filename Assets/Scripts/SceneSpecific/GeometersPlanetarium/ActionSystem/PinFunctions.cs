using IMRE.EmbodiedUserInput;

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
        		pinData pin = null;
        		//find closest point
        		for(i = 0; i < RSDESManager.ins.pinnedPoints.Length; i++){
				if(Mathematics.Operations.magnitude(classifier.origin - pinnedPoints[i].pin.contactPoint) < dist && Mathematics.Operations.Angle(classifier.direction, pinDirection(pinnedPoints[i].pin) < angleTolerance)
				{
					pin = RSDESManager.ins.pinnedPoints[i];
					dist = Mathematics.Operations.magnitude(classifier.origin - RSDESManager.ins.pinnedPoints[i].contactPoint);
				}
			}
			if(pin != null){
				pinFunction(pin);
			}
        }

        public override void endAction(EmbodiedClassifier classifier)
        {
            throw new System.NotImplementedException();
        }
        
        public abstract void pinFunction(RSDESPin pin);
        public abstract Unity.Mathematics.float3 pinDirection(RSDESPin pin);
    }
}
