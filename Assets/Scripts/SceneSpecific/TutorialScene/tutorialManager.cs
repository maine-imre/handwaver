/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using IMRE.HandWaver.HWIO;
using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IMRE.HandWaver.Solver;



namespace IMRE.HandWaver
{
	[System.Serializable]

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class tutorialManager : MonoBehaviour
    {
#pragma warning disable 0649, 0169

		[System.Serializable]
		public enum tutorialStep { movePoint,
									strechPoint,
									strechLine,
									strechPoly,
									selectTest,
									getStraightEdge,
									addWheel,
									revolvePoint,
									revolveLine,
									wireFramePin,
									polyPin,
									polyCutPin,
									crossSection,
									drawSomething,
									eraseDrawing,	
		}
		public tutorialStepCheck objectChecker;
		public Transform revPointSpawnLoc;
		public static tutorialManager ins;
		public Light spotLight;
		public AudioClip successClip;
		public ParticleSystem successParticles;
		[System.Serializable]
		public struct anchorPoints
		{
			public Anchor pointSpawner;
			internal Vector3 initPosPoint;
			public Anchor revolveWheelAnchor;
			internal Vector3 initRevWheelPos;
			public Anchor selectMenu;
			internal Vector3 initSelectMenuPos;
			public Anchor StraightedgeSpawner;
			internal Vector3 initSESpawnerPos;
			public Anchor eraserAnchor;
			internal Vector3 initEraserPos;
			public Anchor pinchDrawAnchor;
			internal Vector3 initDrawPos;
			public Anchor wireFramePinAnchor;
			internal Vector3 initWireFramePos;
			public Anchor polyPinAnchor;
			internal Vector3 initPolyPinPos;
			public Anchor polyCutPinAnchor;
			internal Vector3 initPolyCutPinPos;
			public Anchor crossSectionAnchor;
			internal Vector3 initCSPos;
			

		}
		[System.Serializable]
		public struct tutorialBooleanChecks
		{
			public bool hasPoint;
			public bool hasLine;
			public bool hasPoly;
			public bool hasPrism;
			public bool hasSelected;
			public bool hasSE;
			public bool hasRevUse;
			public bool hasRevSurf;
			public bool hasDrawing;
			public bool hasCircle;
			public bool hasPinCut;
			public bool hasCrossCut;
		}
		public tutorialBooleanChecks thisTutCheck;
		public anchorPoints thisAnchorPoints;

		public tutorialStep currStep = tutorialStep.movePoint;
		private bool _completeStep;
		private toolboxfollower follower;
		private AudioSource thisAS;


		public Transform ped1;
		public Transform ped2;
		public Transform ped3;
		public Transform ped4;
		
		public Transform spotLightAbove;

#pragma warning disable 0649, 0169

		private void Start()
		{
			follower = FindObjectOfType<toolboxfollower>();
			thisAS = GetComponent<AudioSource>();
			initAnchors();
			ins = this;
			resetMGO();
			if(follower != null)
				followerLock();
			StartCoroutine(focusSpotLightAbove(spotLightAbove.transform.position.y * Vector3.up + Vector3.right * ped1.transform.position.x + Vector3.forward * ped1.transform.position.z));
			StartCoroutine(raisePedestal(ped1, ped1.position + (1.5f * Vector3.up)));
			//GeoObjConstruction.iPoint(objectChecker.transform.position + Vector3.up*.2f);
		}

		private void initAnchors()
		{
			//problem here, these are taken when the columns are underneath the table. 
			//changing to make relative to ped.
			thisAnchorPoints.initPosPoint = thisAnchorPoints.pointSpawner.transform.position - ped1.transform.position;

			thisAnchorPoints.initRevWheelPos = thisAnchorPoints.revolveWheelAnchor.transform.position - ped2.transform.position;
			thisAnchorPoints.initSelectMenuPos = thisAnchorPoints.selectMenu.transform.position - ped2.transform.position;
			thisAnchorPoints.initSESpawnerPos = thisAnchorPoints.StraightedgeSpawner.transform.position - ped2.transform.position;

			thisAnchorPoints.initEraserPos = thisAnchorPoints.eraserAnchor.transform.position - ped3.transform.position;
			thisAnchorPoints.initDrawPos = thisAnchorPoints.pinchDrawAnchor.transform.position - ped3.transform.position;
			thisAnchorPoints.initWireFramePos = thisAnchorPoints.wireFramePinAnchor.transform.position - ped3.transform.position;
			thisAnchorPoints.initPolyPinPos = thisAnchorPoints.polyPinAnchor.transform.position - ped3.transform.position;
			thisAnchorPoints.initPolyCutPinPos = thisAnchorPoints.polyCutPinAnchor.transform.position - ped3.transform.position;
			thisAnchorPoints.initCSPos = thisAnchorPoints.crossSectionAnchor.transform.position - ped4.transform.position;

			thisAnchorPoints.eraserAnchor.transform.position = Vector3.down * 20f;
			thisAnchorPoints.revolveWheelAnchor.transform.position = Vector3.down * 20f + Vector3.right * 4f;
			thisAnchorPoints.selectMenu.transform.position = Vector3.down * 20f + Vector3.right * 2f;
			thisAnchorPoints.StraightedgeSpawner.transform.position = Vector3.down + Vector3.right * 20f;
			thisAnchorPoints.pinchDrawAnchor.transform.position = Vector3.down * 7.9f;
		}

		private void followerLock()
		{
			follower.parkSpot = new Vector3(-9, -9, -9);
			if(!follower.isParked)
				follower.parkFollower();
		}

		private void followerUnlock()
		{
			follower.parkSpot = new Vector3(-1.85f, 1.1f, -1.2f);
			if (follower.isParked)
				follower.parkFollower();
		}

		private MasterGeoObj activeObjDemo = null;

		public bool completeStep
		{
			set
			{
				if (value)
				{
					switch (CurrStep)
					{
						case tutorialStep.movePoint:
							//want to show a line segment.
							activeObjDemo = GeoObjConstruction.iLineSegment(GeoObjConstruction.iPoint(ped1.transform.position + Vector3.up * 1f + Vector3.right * .2f), GeoObjConstruction.iPoint(ped1.transform.position + Vector3.up * 1f + Vector3.right * .1f));
							activeObjDemo.leapInteraction = false;
							activeObjDemo.GetComponent<CapsuleCollider>().enabled = false;
							activeObjDemo.GetComponent<InteractableLineSegment>().point1.leapInteraction = false;
							activeObjDemo.GetComponent<InteractableLineSegment>().point2.leapInteraction = false;
							activeObjDemo.GetComponent<InteractableLineSegment>().point1.GetComponent<SphereCollider>().enabled = false;
							activeObjDemo.GetComponent<InteractableLineSegment>().point2.GetComponent<SphereCollider>().enabled = false;
							CurrStep = tutorialStep.strechPoint;
							break;
						case tutorialStep.strechPoint:
							MasterGeoObj activeObjDemo2 = GeoObjConstruction.iLineSegment(GeoObjConstruction.iPoint(ped1.transform.position + Vector3.up * 1.3f + Vector3.right * .2f), GeoObjConstruction.iPoint(ped1.transform.position + Vector3.up * 1.3f + Vector3.right * .1f));
							MasterGeoObj activeObjDemo3 = GeoObjConstruction.iLineSegment(activeObjDemo2.GetComponent<InteractableLineSegment>().point1, activeObjDemo.GetComponent<InteractableLineSegment>().point1);
							MasterGeoObj activeObjDemo4 = GeoObjConstruction.iLineSegment(activeObjDemo2.GetComponent<InteractableLineSegment>().point2, activeObjDemo.GetComponent<InteractableLineSegment>().point2);
							//make a rectangle.
							List<AbstractLineSegment> lineList = new List<AbstractLineSegment>(new AbstractLineSegment[] { activeObjDemo.GetComponent<InteractableLineSegment>(), activeObjDemo2.GetComponent<InteractableLineSegment>(), activeObjDemo3.GetComponent<InteractableLineSegment>(), activeObjDemo4.GetComponent<InteractableLineSegment>() });
							List<AbstractPoint> pointList = new List<AbstractPoint>(new AbstractPoint[] { activeObjDemo.GetComponent<InteractableLineSegment>().point1, activeObjDemo2.GetComponent<InteractableLineSegment>().point1, activeObjDemo2.GetComponent<InteractableLineSegment>().point2, activeObjDemo.GetComponent<InteractableLineSegment>().point2 });
							activeObjDemo = GeoObjConstruction.iPolygon(lineList,pointList);

							activeObjDemo.GetComponent<SphereCollider>().enabled = false;
							activeObjDemo2.GetComponent<CapsuleCollider>().enabled = false;
							activeObjDemo2.GetComponent<InteractableLineSegment>().point1.GetComponent<SphereCollider>().enabled = false;
							activeObjDemo2.GetComponent<InteractableLineSegment>().point2.GetComponent<SphereCollider>().enabled = false;
							activeObjDemo3.GetComponent<CapsuleCollider>().enabled = false;
							activeObjDemo4.GetComponent<CapsuleCollider>().enabled = false;

							CurrStep = tutorialStep.strechLine;
							break;
						case tutorialStep.strechLine:
							activeObjDemo = GeoObjConstruction.iPrism(activeObjDemo.GetComponent<InteractablePolygon>(), activeObjDemo.transform.position + Vector3.forward * .1f);
							activeObjDemo.GetComponent<SphereCollider>().enabled = false;
							activeObjDemo.GetComponent<InteractablePrism>().vertexPoints.ForEach(p => p.GetComponent<SphereCollider>().enabled = false);
							activeObjDemo.GetComponent<InteractablePrism>().lineSegments.ForEach(p => p.GetComponent<CapsuleCollider>().enabled = false);
							activeObjDemo.GetComponent<InteractablePrism>().sides.ForEach(p => p.GetComponent<SphereCollider>().enabled = false);
							activeObjDemo.GetComponent<InteractablePrism>().bases.ForEach(p => p.GetComponent<SphereCollider>().enabled = false);

							CurrStep = tutorialStep.strechPoly;
							break;
						case tutorialStep.strechPoly:

							//need to show that this is needing to be selected???
							activeObjDemo.GetComponent<InteractablePrism>().sides.ForEach(p => p.deleteGeoObj());
							activeObjDemo.GetComponent<InteractablePrism>().bases.ForEach(p => p.deleteGeoObj());
							activeObjDemo.GetComponent<InteractablePrism>().lineSegments.ForEach(p => p.deleteGeoObj());
							activeObjDemo.GetComponent<InteractablePrism>().vertexPoints.ForEach(p => p.deleteGeoObj());
							activeObjDemo.deleteGeoObj();

                            //skip directly to pins.
                            resetMGO();
                            objectChecker.transform.parent = ped3;
                            objectChecker.transform.localPosition = Vector3.zero;
                            CurrStep = tutorialStep.wireFramePin;
                            thisAnchorPoints.pinchDrawAnchor.transform.position = thisAnchorPoints.initDrawPos + ped3.transform.position;
                            StartCoroutine(focusSpotLightAbove(spotLightAbove.transform.position.y * Vector3.up + Vector3.right * ped3.transform.position.x + Vector3.forward * ped3.transform.position.z));
                            StartCoroutine(raisePedestal(ped3, ped3.position + (1.5f * Vector3.up)));

                            //**********skip rotation tutorial.  uncomment thiss to re-enable.*************

                            //CurrStep = tutorialStep.getStraightEdge;
                            //thisAnchorPoints.selectMenu.transform.position = thisAnchorPoints.initSelectMenuPos + ped2.transform.position;//enable select
                            //StartCoroutine(focusSpotLightAbove(spotLightAbove.transform.position.y * Vector3.up + Vector3.right * ped2.transform.position.x + Vector3.forward * ped2.transform.position.z));
                            //StartCoroutine(raisePedestal(ped2, ped2.position + (1.5f * Vector3.up)));

                            resetMGO();
                            //objectChecker.transform.parent = ped2;
                            //objectChecker.transform.localPosition = Vector3.zero;

                            //thisAnchorPoints.StraightedgeSpawner.transform.position = thisAnchorPoints.initSESpawnerPos + ped2.transform.position;
                            break;
						case tutorialStep.getStraightEdge:
							resetMGO();
							//thisAnchorPoints.revolveWheelAnchor.transform.position = ped2.transform.position + Vector3.up + Vector3.right;
							thisAnchorPoints.revolveWheelAnchor.transform.position = thisAnchorPoints.initRevWheelPos + ped2.transform.position;
							CurrStep = tutorialStep.addWheel;
							break;
						case tutorialStep.addWheel:
							CurrStep = tutorialStep.selectTest;
							break;
						case tutorialStep.selectTest:
							CurrStep = tutorialStep.revolvePoint;
							break;
						case tutorialStep.revolvePoint:
						//	resetMGO();
						//	CurrStep = tutorialStep.revolveLine;
						//	break;
						//case tutorialStep.revolveLine:
							resetMGO();
							objectChecker.transform.parent = ped3;
							objectChecker.transform.localPosition = Vector3.zero;
							CurrStep = tutorialStep.wireFramePin;
							thisAnchorPoints.pinchDrawAnchor.transform.position = thisAnchorPoints.initDrawPos + ped3.transform.position;
							StartCoroutine(focusSpotLightAbove(spotLightAbove.transform.position.y * Vector3.up + Vector3.right * ped3.transform.position.x + Vector3.forward * ped3.transform.position.z));
							StartCoroutine(raisePedestal(ped3, ped3.position + (1.5f * Vector3.up)));
							break;
						case tutorialStep.wireFramePin:
							resetMGO();
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * .2f);
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * .1f);

							CurrStep = tutorialStep.polyCutPin;
							break;
						case tutorialStep.polyPin:
							resetMGO();
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * .2f);
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * .1f);
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * -.2f);
							GeoObjConstruction.dPoint(ped3.transform.position + Vector3.up * 1f + Vector3.right * -.1f);

							CurrStep = tutorialStep.polyCutPin;
							break;
						case tutorialStep.polyCutPin:
                            //instantiate a solid here that can be cut with the cross section.
							CurrStep = tutorialStep.crossSection;

                            Vector3 basePos = ped4.transform.position + 0.5f * Vector3.up;

                            GeoObjConstruction.dPyramid(GeoObjConstruction.rPoly(4, .5f, basePos), GeoObjConstruction.dPoint(basePos + Vector3.up * .5f));

							StartCoroutine(focusSpotLightAbove(spotLightAbove.transform.position.y * Vector3.up + Vector3.right * ped4.transform.position.x + Vector3.forward * ped4.transform.position.z));
							StartCoroutine(raisePedestal(ped4, ped4.position + (1.5f * Vector3.up)));
							break;
						case tutorialStep.crossSection:

                            //skip drawing tutorial.
							//CurrStep = tutorialStep.drawSomething;
                            
                            //end tutorial.
							break;
						case tutorialStep.drawSomething:
							CurrStep = tutorialStep.eraseDrawing;
							thisAnchorPoints.eraserAnchor.transform.position = thisAnchorPoints.initEraserPos + ped3.transform.position;
							break;
						case tutorialStep.eraseDrawing:
							break;
						default:
							CurrStep = tutorialStep.movePoint;//if error restart
							break;
					}
				}

			}
		}

		private void resetMGO()
		{
			StartCoroutine(wait(3.5f));
		}

		private IEnumerator wait(float v)
		{
			yield return new WaitForSeconds(v);
			//foreach (MasterGeoOBj mgo in FindObjectsOfType<MasterGeoOBj>().Where(mgo => mgo.isActiveAndEnabled &&
			//														mgo.GetComponent<straightEdgeBehave>() == null &&
			//															(!(mgo.GetComponent<AnchorableBehaviour>() != null) ||
			//															mgo.GetComponent<AnchorableBehaviour>() != null && !mgo.GetComponent<AnchorableBehaviour>().isAttached)))
			//{
			//	ObjManHelper.ins.removeComponent(mgo.transform);
			//}
			if (CurrStep == tutorialStep.addWheel)
				GeoObjConstruction.iPoint(revPointSpawnLoc.position);
			else if (CurrStep == tutorialStep.revolvePoint)
				GeoObjConstruction.iLineSegment(GeoObjConstruction.iPoint(revPointSpawnLoc.position), GeoObjConstruction.iPoint(revPointSpawnLoc.position + Vector3.up * 0.4f));
			else if (CurrStep == tutorialStep.revolveLine)
				resetSE();//doesnt work?
		}

		private void resetSE()
		{
			foreach (straightEdgeBehave se in FindObjectsOfType<straightEdgeBehave>().Where(se => (!(se.GetComponent<AnchorableBehaviour>() != null) ||
																							se.GetComponent<AnchorableBehaviour>() != null && 
																							!se.GetComponent<AnchorableBehaviour>().isAttached)))
			{
				HW_GeoSolver.ins.removeComponent(se);
			}
		}

		public tutorialBooleanChecks ThisTutCheck
		{
			get
			{
				return thisTutCheck;
			}

			set
			{
				thisTutCheck = value;
				checkProg();
			}
		}

		internal tutorialStep CurrStep
		{
			get
			{
				return currStep;
			}

			set
			{
				currStep = value;
				playSuccess();
			}
		}

		private void playSuccess()
		{
			thisAS.clip = successClip;
			thisAS.Play();
			if(currStep != tutorialStep.movePoint)//first step
				successParticles.Play();
		}

		private void checkProg()
		{
			switch (currStep)
			{
				case tutorialStep.movePoint:
					completeStep = thisTutCheck.hasPoint;
					break;
				case tutorialStep.strechPoint:
					completeStep = thisTutCheck.hasLine;
					break;
				case tutorialStep.strechLine:
					completeStep = thisTutCheck.hasPoly;
					break;
				case tutorialStep.strechPoly:
					completeStep = thisTutCheck.hasPrism;
					break;
				case tutorialStep.selectTest:
					completeStep = thisTutCheck.hasSelected;
					break;
				case tutorialStep.getStraightEdge:
					completeStep = thisTutCheck.hasSE;
					break;
				case tutorialStep.addWheel:
					completeStep = thisTutCheck.hasRevUse;
					break;
				case tutorialStep.revolvePoint:
					completeStep = thisTutCheck.hasCircle;
					break;
				case tutorialStep.revolveLine:
					completeStep = thisTutCheck.hasRevSurf;
					break;
				case tutorialStep.wireFramePin:
					completeStep = thisTutCheck.hasLine;
					break;
				case tutorialStep.polyPin:
					completeStep = thisTutCheck.hasPoly;
					break;
				case tutorialStep.polyCutPin:
					completeStep = thisTutCheck.hasPinCut;
					break;
				case tutorialStep.crossSection:
					completeStep = thisTutCheck.hasCrossCut;
					break;
				case tutorialStep.drawSomething:
					completeStep = thisTutCheck.hasDrawing;
					break;
				case tutorialStep.eraseDrawing:
					completeStep = !thisTutCheck.hasDrawing;//maybe change this to something else, but for now you have to erase all drawings
					break;
				default:
					break;
			}
		}

		public void focusLight(Transform target)
		{
			spotLight.transform.LookAt(target, Vector3.up);
		}

		public IEnumerator focusSpotLightAbove(Vector3 endPos)
		{
			DateTime startTime = DateTime.Now;
			Vector3 startPos = spotLightAbove.transform.position;
			while ((float)DateTime.Now.Subtract(startTime).TotalSeconds < 5.0f)
			{
				spotLightAbove.transform.position = Vector3.Lerp(startPos, endPos, (float)DateTime.Now.Subtract(startTime).TotalSeconds / 5.0f);
				yield return new WaitForEndOfFrame();
			}
		}

		public IEnumerator raisePedestal(Transform ped, Vector3 endPos)
		{
			DateTime startTime = DateTime.Now;
			Vector3 startPos = ped.position;
			while ((float)DateTime.Now.Subtract(startTime).TotalSeconds < 5.0f)
			{
				ped.position = Vector3.Lerp(startPos, endPos, (float)DateTime.Now.Subtract(startTime).TotalSeconds / 5.0f);
				yield return new WaitForEndOfFrame();
			}
		}
	}
}