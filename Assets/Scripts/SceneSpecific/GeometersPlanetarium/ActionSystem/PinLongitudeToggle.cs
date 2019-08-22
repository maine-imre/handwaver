using IMRE.EmbodiedUserInput;
using Unity.Mathematics;

namespace IMRE.HandWaver.Space
{
public class PinLongitudeToggle : PinFunctions
    {
		public float desiredAngle = 0f;
		public override void pinFunction(RSDESPin pin, EmbodiedClassifier classifier)
		{
			if(pin != null)
			{
				pin.toggleLong();
			}
		}

		public override float3 pinDirection(RSDESPin pin){
			return new float3(0f,1f,0f);
		}
    }
}
