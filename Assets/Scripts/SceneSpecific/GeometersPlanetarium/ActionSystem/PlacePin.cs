using IMRE.EmbodiedUserInput;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
    public class PlacePin : EmbodiedAction
    {
        /// <summary>
        /// Tolerance,in this case, controls the distance from the surface the fingertip needs to be to
        /// place a pin on the surface.
        /// Example: 0.01f is 1 cm
        /// </summary>
        public float tolerance = .01f;
        
        /// <summary>
        /// Cooldown is used to limit how many pins are placed within how much time.
        /// Without this a pin would be placed on every frame when a finger is close enough to
        /// the surface of Earth
        /// </summary>
        public float cooldown = 2f;
        
        /// <summary>
        /// Start time is initialized to 0 to get through the first check of placing a pin on the earth.
        /// Otherwise it is set at the time when a finger places a pin on the Earth's surface.
        /// It is then used with cooldown to compare with the current time and limit pin placement
        /// </summary>
        private float _startTime = 0f;
        public override void checkClassifier(EmbodiedClassifier classifier)
        {
            //look at whether the gesture being performed is close to the surface of the Earth
            float dist = IMRE.Math.Operations.magnitude((float3) RSDESManager.earthPos - classifier.origin);

            if (Unity.Mathematics.math.abs(RSDESManager.EarthRadius - dist) < tolerance && Time.time > _startTime + cooldown)
            {
                    //figure out when the gesture began
                    _startTime = Time.time;
                    
                    //make a pin at a location on the Earth closest to where the gesture is
                    RSDESPin.Constructor(GeoPlanetMaths.latlong(classifier.origin, RSDESManager.earthPos));
                    
                    //the gesture has completed the functionality.
                    classifier.shouldFinish = true;
            }
        }

        public override void endAction(EmbodiedClassifier classifier)
        {
            //do nothing
        }
    }
}