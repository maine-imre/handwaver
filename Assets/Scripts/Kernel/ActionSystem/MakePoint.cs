using System.Reflection;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.ActionSystem;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Kernel.GGBFunctions;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.ActionSystem
{
    public class MakePoint : GeoElementFunction
    {

        /// <summary>
        /// Cooldown is used here to limit the amount of times the classifier's function is executed
        /// </summary>
        public float cooldown = 2f;
        
        /// <summary>
        /// Start time is used to note when the classifier's function was last activated
        /// </summary>
        private float _startTime = 0f;
        public override void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier)
        {
            //Check to see that the cooldown is done on the gesture
            if (Time.time > (_startTime + cooldown))
            {
                //set the time for when the function is called
                _startTime = Time.time;
                
                Geometry.Point(Geometry.Float3Value(classifier.origin));
            }
        }
    }
}