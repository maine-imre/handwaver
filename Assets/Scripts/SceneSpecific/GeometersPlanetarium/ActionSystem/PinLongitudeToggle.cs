using IMRE.EmbodiedUserInput;

namespace IMRE.HandWaver.Space
{
public class PinLongitudeToggle : EmbodiedAction
    {
    		public override float desiredAngle = 0f;
    		public override Unity.Mathematics.float3 pinDirection(RSDESPin pin){
    			return new float3(0f,1f,0f);
    		}
    		
    		public override void pinFunction(RSDESPin pin){
			if(pin != null)
			{
				pin.toggleLong();
			}
		}
    }
}
