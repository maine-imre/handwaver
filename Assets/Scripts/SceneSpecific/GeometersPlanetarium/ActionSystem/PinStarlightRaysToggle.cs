using IMRE.EmbodiedUserInput;

namespace IMRE.HandWaver.Space
{
public class PinStarlightRaysToggle : PinFunctions
    {
    
    		public override Unity.Mathematics.float3 pinDirection(RSDESPin pin){
    			return (Unity.Mathematics.float3) pin.contactPoint;
    		}
    		
        public override void pinFunction(RSDESPin pin){
			if(pin != null)
			{
				pin.pin.StarMode ++;
			}
		}
    }
}
