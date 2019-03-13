using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.Space;
using UnityEngine;
using IMRE.HandWaver;
using System.Linq;

namespace IMRE.Gestures
{
    public class SelectGesture : PointAtGesture
    {
        public float tol = .02f;
        public float angleTol = .02f;
        private bool isComplete = false;
        private MasterGeoObj closestObj;

        protected override bool DeactivationConditionsActionComplete()
        {
            return isComplete;
        }

        protected override void WhileGestureActive(BodyInput bodyInput, Chirality chirality)
        {
            Hand hand = getHand(bodyInput, chirality);
            //sethandtomodeSelect
            float shortestDist = Mathf.Infinity;
            closestObj = null;

            foreach (MasterGeoObj mgo in FindObjectsOfType<MasterGeoObj>().Where(g =>
                (g.GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>() == null ||
                 (g.GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>() != null && !g.GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>().isAttached))))
            {
                float distance = mgo.LocalDistanceToClosestPoint(hand.Fingers[1].Joints[3].Position);
                float angle = mgo.PointingAngleDiff(hand.Fingers[1].Joints[3].Position - mgo.ClosestSystemPosition(hand.Fingers[1].Joints[3].Position),
                    hand.Fingers[1].Direction); //considering implementing some sort of a pointing direction function.

                if (Mathf.Abs(distance) < shortestDist)
                {
                    if (distance < shortestDist && angle < angleTol)
                    {
                        closestObj = mgo;
                        shortestDist = distance;
                    }
                }
                else
                {
                    //check to see if any higher priority objectes lie within epsilon
                    bool v = (Mathf.Abs(distance) - shortestDist <= tol) && (
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


            if (closestObj != null && shortestDist <= tol)
            {
            if (closestObj.IsSelected)
                    closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.none;
                else
                    closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.selected;
                //This determines if you have to cancel the gesture to select another object
                isComplete = true;
            }
        }
    }
}