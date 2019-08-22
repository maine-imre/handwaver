using IMRE.EmbodiedUserInput;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

namespace IMRE.HandWaver.Space
{
    public class MoveObject : PinFunctions
    {
        public override void pinFunction(RSDESPin pin, EmbodiedClassifier classifier)
        {
            pin.Latlong = GeoPlanetMaths.latlong(classifier.origin, RSDESManager.earthPos);
        }

        public override float3 pinDirection(RSDESPin pin)
        {
            return (Unity.Mathematics.float3) pin.directionFromLatLong();
        }
    }
}