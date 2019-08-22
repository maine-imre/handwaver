using IMRE.EmbodiedUserInput;

namespace IMRE.HandWaver.Space
{
public class PinTerminatorToggle : PinFunctions
    {
    		public float desiredAngle = 90f;
    		
    		public override void pinFunction(RSDESPin pin, EmbodiedClassifier classifier){
			if(pin != null)
			{
				pin.toggleTerminator();
			}
		}
		
    		public override Unity.Mathematics.float3 pinDirection(RSDESPin pin){
    			return (Unity.Mathematics.float3) pin.directionFromLatLong();
    		}
    }
}
