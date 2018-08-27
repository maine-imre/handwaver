/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using UnityEngine;
using Leap.Unity.Interaction;
using System.Linq;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class tutorialStepCheck : MonoBehaviour
    {

        private void Update()
        {
            tutorialManager.tutorialBooleanChecks currVar = new tutorialManager.tutorialBooleanChecks();
            RaycastHit[] hits = Physics.BoxCastAll(transform.position + (Vector3.up * 0.2f), Vector3.one, Vector3.up, Quaternion.identity, 1f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<MasterGeoObj>() != null && ((hit.collider.GetComponent<AnchorableBehaviour>() != null && !hit.collider.GetComponent<AnchorableBehaviour>().isAttached) ^ hit.collider.GetComponent<AnchorableBehaviour>() == null))
                {
                    MasterGeoObj obj = hit.collider.GetComponent<MasterGeoObj>();
                    switch (obj.figType)
                    {
                        case GeoObjType.point:
                            currVar.hasPoint = !(obj.GetComponent<AnchorableBehaviour>() != null) || !obj.GetComponent<AnchorableBehaviour>().isAttached;
                            if (HW_GeoSolver.ins.geomanager.bidirectionalNeighborsOfNode(obj.figName).Any(neighbor => neighbor.mytransform.GetComponent<DependentRevolvedSurface>() != null))
                                currVar.hasRevSurf = true;
                            break;
                        case GeoObjType.line:
                            currVar.hasLine = Vector3.Distance(((AbstractLineSegment)obj).vertex0, ((AbstractLineSegment)obj).vertex1) > 0.1f;
                            break;
                        case GeoObjType.polygon:
                            currVar.hasPoly = true;
                            break;
                        case GeoObjType.prism:
                            currVar.hasPrism = true;
                            break;
                        case GeoObjType.straightedge:
                            currVar.hasSE = true;
                            if (obj.GetComponent<straightEdgeBehave>().wheelType == shipWheelOffStraightedge.wheelType.revolve)
                                currVar.hasRevUse = true;
                            break;
                        case GeoObjType.circle:
                            currVar.hasCircle = true;

                            break;
                        default:
                            break;
                    }
                    if (obj.IsSelected)
                        currVar.hasSelected = true;
                }
                else
                {
                    //if (hit.collider.GetComponent<pinchDrawing>() != null)
                    //{
                    //	currVar.hasDrawing = true;
                    //}else 
                    if (hit.collider.GetComponent<LineSegmentMaker>() && hit.collider.GetComponent<LineSegmentMaker>().thisPinType == LineSegmentMaker.pinType.polycut)
                    {
                        currVar.hasPinCut = false;
                    }
                    //	}else if(hit.collider.GetComponent<CrossSectionBehave>() != null)//this is to check if the crosssection tool is within the area we check and it has been moved from initial rotation
                    //	{
                    //		currVar.hasCrossCut = (hit.collider.transform.rotation != Quaternion.identity);
                    //	}
                    //}
                }



                tutorialManager.ins.ThisTutCheck = currVar;
            }
        }
    }

}