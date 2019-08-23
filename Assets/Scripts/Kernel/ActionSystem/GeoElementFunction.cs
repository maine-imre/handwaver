using System.Collections;
using System.Collections.Generic;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.Space;
using IMRE.Math;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.ActionSystem
{
    public class GeoElementFunction : EmbodiedAction
    {
        public float tolerance = .05f;
        public float angleTolerance = 15f;
        private float desiredAngle = 0f;

        public override void checkClassifier(EmbodiedClassifier classifier)
        {
            float bestDist = tolerance;
            GeoElement geo = new GeoElement();
            //find closest point
            for (int i = 0; i < GeoElementDatabase.GeoElements.Values.Count; i++)
            {
                float3 closestPoint = GeoElementProximityLib.closestPointOnSurface(GeoElementDatabase.GeoElements.Values[i]);
                if ((Operations.magnitude(classifier.origin - closestPoint) < bestDist) &&
                    (Operations.Angle(classifier.direction,classifier.origin - closestPoint) < angleTolerance))
                {
                    geo = GeoElementDatabase.GeoElements.Values[i];
                    bestDist = Operations.magnitude(classifier.origin - closestPoint);
                }
            }

            if (!geo.Equals(default(GeoElement)))
            {
                geoElementFunction(geo, classifier);
            }
        }

        public override void endAction(EmbodiedClassifier classifier)
        {
            return;
        }

        public abstract void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier);
    }
}