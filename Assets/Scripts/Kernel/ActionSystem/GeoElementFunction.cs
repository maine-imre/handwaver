using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.Kernel;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.Math;
using UnityEngine;

namespace IMRE.HandWaver.ActionSystem
{
    public abstract class GeoElementFunction : EmbodiedAction
    {
        public float angleTolerance = 15f;
        private float desiredAngle = 0f;
        public float tolerance = .05f;

        public override void checkClassifier(EmbodiedClassifier classifier)
        {
            var bestDist = tolerance;
            var geo = new GeoElement();
            //find closest point
            for (var i = 0; i < GeoElementDataBase.GeoElements.Length; i++)
            {
                if (!GeoElementDataBase.HasElement(i)) continue;

                var closestPoint =
                    GeoElementProximityLib.ClosestPosition(GeoElementDataBase.GeoElements[i], classifier.origin);
                if (!((classifier.origin - closestPoint).Magnitude() < bestDist) ||
                    !(Operations.Angle(classifier.direction, classifier.origin - closestPoint) <
                      angleTolerance)) continue;
                geo = GeoElementDataBase.GeoElements[i];
                bestDist = (classifier.origin - closestPoint).Magnitude();
            }

            if (!geo.Equals(default(GeoElement))) geoElementFunction(geo, classifier);
        }

        public override void endAction(EmbodiedClassifier classifier)
        {
        }

        public abstract void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier);
    }
}