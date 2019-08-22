using System.Collections;
using System.Collections.Generic;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Space;
using IMRE.Math;
using Unity.Mathematics;
using UnityEngine;
using IMRE.Math;
using IMRE.EmbodiedUserInput;

namespace IMRE.HandWaver
{
    public abstract class GeoElementFunctions : EmbodiedAction
    {
        public float tolerance = .05f;
        public float angleTolerance = 15f;
        private float desiredAngle = 0f;

        public void CheckClassifier(EmbodiedClassifier classifier)
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
                GeoElementFunction(geo, classifier);
            }
        }

        public abstract void GeoElementFunction(GeoElement geo, EmbodiedClassifier classifier);
    }
}