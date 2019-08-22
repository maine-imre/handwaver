namespace IMRE.HandWaver.Space
{
public class PinStarlightRaysToggle : PinFunctions
    {
    
    		public override Unity.Mathematics.float3 pinDirection(RSDESPin pin)
            {
	            return (Unity.Mathematics.float3) pin.directionFromLatLong();
            }
    		
        public override void pinFunction(RSDESPin pin, EmbodiedClassifier classifier){
			if(pin != null)
			{
				//TODO fix this its failing
				UnityEngine.Debug.Log("I made it so far");
				pin.StarMode = RSDESPin.starFieldSelect.single;
				UnityEngine.Debug.Log("I am this: "+pin.StarMode);
			}
		}
    }
}
