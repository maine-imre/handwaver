using IMRE.EmbodiedUserInput;
using Unity.Mathematics;

namespace IMRE.HandWaver.Space
{
public class PinLatitudeToggle : PinFunctions
{
		public float desiredAngle = 90f;
		public override void pinFunction(RSDESPin pin, EmbodiedClassifier classifier)
		{
			if(pin != null)
			{
				pin.toggleLat();
			}
		}

		public override float3 pinDirection(RSDESPin pin){
			return new float3(0f,1f,0f);
		}
	}
}
