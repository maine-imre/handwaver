using System.Collections.Generic;
using System.Linq;
using IMRE.HandWaver;
using UnityEngine;

namespace IMRE.Gestures
{
    public class GraspMasterGeoObj : GraspGesture
    {
        public float outerRadius = .05f;
        public float innerRadius = .02f;
        public float aboutEqual = 1.1f;
        public float muchCloser = .3f;

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
            MasterGeoObj closestObj = bestObject(hand);
            isComplete = closestObj != null;
            if (isComplete)
            {
                closestObj.Position3 = hand.Fingers[1].Joints[3].Position;
            }
        }

        private MasterGeoObj bestObject(Hand hand)
        {
            float shortestDist = Mathf.Infinity;
            float shortestDist2 = Mathf.Infinity;
            closestObj = null;
            MasterGeoObj closestObj2 = null;

            List<MasterGeoObj> eligibleMGO = FindObjectsOfType<MasterGeoObj>()
                .Where(mgo => checkEligible(mgo, hand, outerRadius)).ToList();

            foreach (MasterGeoObj mgo in eligibleMGO)
            {
                float distance = mgo.LocalDistanceToClosestPoint(hand.Fingers[1].Joints[3].Position);

                if (Mathf.Abs(distance) < shortestDist)
                {
                    if (distance < shortestDist)
                    {
                        if (closestObj != null)
                        {
                            closestObj2 = closestObj;
                            shortestDist2 = shortestDist;
                        }

                        closestObj = mgo;
                        shortestDist = distance;
                    }
                    else if (distance < shortestDist2)
                    {
                        shortestDist2 = distance;
                        closestObj2 = mgo;
                    }
                }
            }

            //we prioritize taking the lowest dimension object over taking the closest object.

            if (closestObj != null && closestObj2 != null)
            {
                float closestRatio = shortestDist / shortestDist2;
                //compare the two closest objects
                if (closestRatio < aboutEqual && closestRatio > muchCloser)
                {
                    //of all eligible objects, filter by closer than about equal.
                    eligibleMGO = eligibleMGO.Where(mgo => checkEligible(mgo, hand, shortestDist * aboutEqual))
                        .ToList();
                    //filter by dimension
                    for (int i = 0; i < 3 && !isComplete; i++)
                    {
                        if (eligibleMGO.Any(mgo => mgo.Dimension == i))
                        {
                            //take the closest within the lowest dimension
                            return eligibleMGO.FirstOrDefault(mgo => mgo.Dimension == i);
                        }
                    }
                }
                else if (closestRatio < muchCloser || shortestDist < innerRadius)
                {
                    //keep closest Object.
                    return closestObj;
                }
                else
                {
                    return null;
                }
            }

            return null; //error case
        }
 

    private bool checkEligible(MasterGeoObj mgo,Hand hand, float maxDist)
        {
            return (mgo.LocalDistanceToClosestPoint(hand.Fingers[1].Joints[3].Position) < maxDist) &&
                   (mgo.PointingAngleDiff(hand.Fingers[1].Joints[3].Position - 
                                          mgo.ClosestSystemPosition(hand.Fingers[1].Joints[3].Position),
                       hand.Fingers[1].Direction) < angleTol);
        } 
    }
}