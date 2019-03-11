using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IMRE.HandWaver.Space;
using Leap;
using UnityEngine;
using UnityEngine.XR;
using IMRE.HandWaver.Space;
using Leap.Unity;
using IMRE.HandWaver;
using System.Linq;

namespace IMRE.Gestures
{
    public class SelectGesture : PointAtGesture
    {
        public float tol = .02f;
        private bool isComplete = false;

        protected override bool DeactivationConditionsActionComplete()
        {
            return isComplete;
        }

        protected override void WhileGestureActive(Leap.Hand hand, InputDevice osvrController)
        {
            float shortestDist = Mathf.Infinity;

        private MasterGeoObj closestObj = null;
            foreach (MasterGeoObj mgo in FindObjectsOfType<MasterGeoObj>().Where(g => (
                     g.GetComponent<AnchorableBehaviour>() == null ||
                    (g.GetComponent<AnchorableBehaviour>() != null &&
                    !g.GetComponent<AnchorableBehaviour>().isAttached))))
        {
            float distance = mgo.LocalDistanceToClosestPoint(hand.Fingers[1].TipPosition.ToVector3());
            float angle = mgo.PointingAngleDiff(hand.Fingers[1].TipPosition.ToVector3(),
                hand.Fingers[1].Direction.ToVector3());

            if (Mathf.Abs(distance) < shortestDist)
            {
                if (distance < shortestDist && angle < angleTolerance)
                {
                    closestObj = mgo;
                    shortestDist = distance;
                }
            }
            else
            {
                //check to see if any higher priority objectes lie within epsilon
                bool v = (Mathf.Abs(distance) - shortestDist <= maximumRangeToSelect) && (
                             ((closestObj.figType == GeoObjType.line || closestObj.figType == GeoObjType.polygon) &&
                              mgo.figType == GeoObjType.point)
                             || (closestObj.figType == GeoObjType.polygon && mgo.figType == GeoObjType.point)
                         );
                if (v)
                {
                    closestObj = mgo;
                    shortestDist = distance;
                }
            }
        }

        if (closestObj != null && shortestDist <= maximumRangeToSelect)
    {
    if (debugSelect)
    Debug.Log(closestObj + " is the object toggling selection state.");
    if (closestObj.IsSelected)
    closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.none;
    else
    closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.selected;
    completeBool = true;
}

}
}