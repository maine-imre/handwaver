/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System;
using Leap.Unity.Interaction;

using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver.HWIO
{
/// <summary>
/// Creates and saves XML files for saving and loading geo objs.
/// Will be refactored with new geometery solver.
/// </summary>
	class XMLManager : MonoBehaviour
	{
		public static XMLManager ins;
		public static string sessionID;
		private void Awake()
		{
			ins = this;

#if !UNITY_EDITOR
			if (!Directory.Exists(Application.dataPath + @"/../autosaves/"))
			{
				Directory.CreateDirectory(Application.dataPath + @" /../autosaves/");
			}
			sessionID = System.DateTime.Now.ToString("yyyyMMddTHHmmss");
			StartCoroutine(autoSaving());
#endif
			checkLog();
			#if StandaloneWindows64
			commandLineArgumentParse.logStateChange.AddListener(checkLog);
			#endif
		}

		private void checkLog()
		{
#if !UNITY_EDITOR

			
			if (!Directory.Exists(Application.dataPath + @"/../dataCollection/"))
			{
				Directory.CreateDirectory(Application.dataPath + @" /../dataCollection/");
			}


			if (commandLineArgumentParse.logCheck())
			{
				StopCoroutine(autoSaving());
				StartCoroutine(logGeoData());
			}
#endif
		}

		private IEnumerator logGeoData()
		{
			if (!Directory.Exists(Application.dataPath + @"/../dataCollection/"))
			{
				Directory.CreateDirectory(Application.dataPath + @" /../dataCollection/");
			}
			sessionID = System.DateTime.Now.ToString("yyyyMMddTHHmmss");
			while (true)
			{
				yield return new WaitForSecondsRealtime(1f);
				LogGeoObjs(Application.dataPath + @"/../dataCollection/ConstructionHistory" + sessionID + @".hwlog");
			}
		}


		private IEnumerator autoSaving()
		{
			while (true)
			{

				yield return new WaitForSecondsRealtime(15f);
				if (File.Exists(Application.dataPath + @"/../autosaves/ConstructionAutoSave" + sessionID + @".hw"))
				{
					if (File.Exists(Application.dataPath + @"/../autosaves/ConstructionAutoSave" + sessionID + @".hwbak"))
					{
						File.Delete(Application.dataPath + @"/../autosaves/ConstructionAutoSave" + sessionID + @".hwbak");
					}
					File.Move(Application.dataPath + @"/../autosaves/ConstructionAutoSave" + sessionID + @".hw", Application.dataPath + @"/../autosaves/ConstructionAutoSave" + sessionID + @".hwbak");
				}
				SaveGeoObjs(Application.dataPath + @"/../autosaves/ConstructionAutoSave"+ sessionID + @".hw");
			}
		}

		public GeoObjDatabase GeoObjDB;

		public void LoadGeoObjs(string path)
		{
			string newPath = Path.ChangeExtension(path, ".xml");
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(GeoObjDatabase));
				File.Move(path, newPath);
				FileStream stream = new FileStream(newPath, FileMode.Open);
				GeoObjDB = serializer.Deserialize(stream) as GeoObjDatabase;
				stream.Close();
				xmlToMGO();
			}
			finally
			{
				File.Move(newPath, path);
			}
		}

		//[ContextMenu("Load from Save")] for debugging make public and uncomment
		private void xmlToMGO(){
			Dictionary<string, MasterGeoObj> spawnedObjects = new Dictionary<string, MasterGeoObj>();
			foreach (GeoObj geo in GeoObjDB.list)
			{
				switch (geo.type)
				{
					case GeoObjType.point:
						MasterGeoObj spawnedPoint = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								spawnedPoint = GeoObjConstruction.dPoint(geo.position);
								break;
							case GeoObjDef.Interactable:
								spawnedPoint = GeoObjConstruction.iPoint(geo.position);
								break;
							case GeoObjDef.Static:
								spawnedPoint = GeoObjConstruction.sPoint(geo.position);
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedPoint != null)
						{
							if(!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedPoint, geo.label);
							spawnedObjects.Add(geo.figName, spawnedPoint);
						}
						break;
					case GeoObjType.line:
						MasterGeoObj spawnedLine = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								spawnedLine = GeoObjConstruction.dLineSegment(spawnedObjects[geo.dependencies[0]] as AbstractPoint, spawnedObjects[geo.dependencies[1]] as AbstractPoint);
								break;
							case GeoObjDef.Interactable:
								spawnedLine = GeoObjConstruction.iLineSegment(spawnedObjects[geo.dependencies[0]] as AbstractPoint, spawnedObjects[geo.dependencies[1]] as AbstractPoint);
								break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedLine != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedLine, geo.label);
							spawnedObjects.Add(geo.figName, spawnedLine);
						}
						break;
					case GeoObjType.polygon:
						MasterGeoObj spawnedPoly = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						List<AbstractLineSegment> lineList;
						List<AbstractPoint> pointList;
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								lineList = new List<AbstractLineSegment>();
								geo.dependencies.Where(d => spawnedObjects[d].figType == GeoObjType.line).ToList().ForEach(l => lineList.Add(spawnedObjects[l] as AbstractLineSegment));
								pointList = new List<AbstractPoint>();
								geo.dependencies.Where(d => spawnedObjects[d].figType == GeoObjType.point).ToList().ForEach(p => pointList.Add(spawnedObjects[p] as AbstractPoint));
								spawnedPoly = GeoObjConstruction.dPolygon(lineList, pointList);
								break;
							case GeoObjDef.Interactable:
								lineList = new List<AbstractLineSegment>();
								geo.dependencies.Where(d => spawnedObjects[d].figType == GeoObjType.line).ToList().ForEach(l => lineList.Add(spawnedObjects[l] as AbstractLineSegment));
								pointList = new List<AbstractPoint>();
								geo.dependencies.Where(d => spawnedObjects[d].figType == GeoObjType.point).ToList().ForEach(p => pointList.Add(spawnedObjects[p] as AbstractPoint));
								spawnedPoly = GeoObjConstruction.iPolygon(lineList, pointList);
								break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedPoly != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedPoly, geo.label);
							spawnedObjects.Add(geo.figName, spawnedPoly);
						}
						break;
					case GeoObjType.prism:
						MasterGeoObj spawnedPrism = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Interactable:
								if (!(geo.prismData.bases.Count < 2 || geo.prismData.sides.Count < spawnedObjects[geo.prismData.bases[0]].GetComponent<AbstractPolygon>().pointList.Count))
								{
									List<AbstractPolygon> bases = new List<AbstractPolygon>();
									List<AbstractPolygon> sides = new List<AbstractPolygon>();
									geo.prismData.bases.ForEach(b => bases.Add(spawnedObjects[b] as AbstractPolygon));
									geo.prismData.sides.ForEach(s => sides.Add(spawnedObjects[s] as AbstractPolygon));
									spawnedPrism = GeoObjConstruction.iPrism(bases, sides);
								}
								else
								{
							List<AbstractLineSegment> edges = new List<AbstractLineSegment>();
							geo.prismData.edges.ForEach(e => edges.Add(spawnedObjects[e] as AbstractLineSegment));
							spawnedPrism = GeoObjConstruction.iPrism(edges);
						}
						break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedPrism != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedPrism, geo.label);
							spawnedObjects.Add(geo.figName, spawnedPrism);
						}
						break;
					case GeoObjType.pyramid:
						MasterGeoObj spawnedPyramid = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.

						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								spawnedPyramid = GeoObjConstruction.dPyramid(spawnedObjects[geo.dependencies[0]] as AbstractPolygon, spawnedObjects[geo.pyramidData.apexName] as AbstractPoint);
								break;
							case GeoObjDef.Interactable:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedPyramid != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedPyramid, geo.label);
							spawnedObjects.Add(geo.figName, spawnedPyramid);
						}
						break;
					case GeoObjType.circle:
							MasterGeoObj spawnedCircle = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
							switch (geo.definition)
							{
								case GeoObjDef.Abstract:
									Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
									break;
								case GeoObjDef.Dependent:
									spawnedCircle = GeoObjConstruction.dCircle(spawnedObjects[geo.dependencies[0]] as AbstractPoint, spawnedObjects[geo.dependencies[1]] as AbstractPoint, geo.circleData.normDir);
									break;
								case GeoObjDef.Interactable:
									Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
									break;
								case GeoObjDef.Static:
									Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
									break;
								case GeoObjDef.none:
									Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
									break;
								default:
									break;
							}
							if (spawnedCircle != null)
							{
								if (!String.IsNullOrEmpty(geo.label))
									GeoObjConstruction.label(spawnedCircle, geo.label);
							spawnedObjects.Add(geo.figName, spawnedCircle);
							}
							break;
					case GeoObjType.sphere:
						MasterGeoObj spawnedSphere = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								spawnedSphere = GeoObjConstruction.dSphere(spawnedObjects[geo.dependencies[0]] as AbstractPoint, spawnedObjects[geo.dependencies[1]] as AbstractPoint);
								break;
							case GeoObjDef.Interactable:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedSphere != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedSphere, geo.label);
							spawnedObjects.Add(geo.figName, spawnedSphere);
						}
						break;
					case GeoObjType.revolvedsurface:
						MasterGeoObj spawnedrevSurf = null;//initialzed as null so that cases that do not spawn still have it initialized but still fails a null check.
						switch (geo.definition)
						{
							case GeoObjDef.Abstract:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Dependent:
								spawnedrevSurf = GeoObjConstruction.dRevSurface(spawnedObjects[geo.dependencies[0]] as AbstractPoint, spawnedObjects[geo.dependencies[1]] as AbstractLineSegment, geo.revSurfData.normDir);
								spawnedrevSurf.transform.position = geo.position;
								spawnedrevSurf.transform.rotation = geo.rotation;
								break;
							case GeoObjDef.Interactable:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.Static:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							case GeoObjDef.none:
								Debug.Log(geo.figName + " was attempted to spawn but is not supported yet within XMLManager script! Add construction function in place of this log.");
								break;
							default:
								break;
						}
						if (spawnedrevSurf != null)
						{
							if (!String.IsNullOrEmpty(geo.label))
								GeoObjConstruction.label(spawnedrevSurf, geo.label);
							spawnedObjects.Add(geo.figName, spawnedrevSurf);
						}
						break;
					case GeoObjType.torus:
						break;
					case GeoObjType.flatface:
						Transform flatface = flatfaceBehave.Constructor().transform;
						flatface.transform.position = geo.position;
						flatface.transform.rotation = geo.rotation;
						if (!String.IsNullOrEmpty(geo.label))
							GeoObjConstruction.label(flatface.GetComponent<flatfaceBehave>(), geo.label);
						spawnedObjects.Add(geo.figName, flatface.GetComponent<MasterGeoObj>());
						break;
					case GeoObjType.straightedge:
						Transform straightEdge = straightEdgeBehave.Constructor().transform;
						straightEdge.transform.position = geo.position;
						straightEdge.transform.rotation = geo.rotation;
						if (!String.IsNullOrEmpty(geo.label))
							GeoObjConstruction.label(straightEdge.GetComponent<straightEdgeBehave>(), geo.label);
						spawnedObjects.Add(geo.figName, straightEdge.GetComponent<MasterGeoObj>());
						break;
					default:
						break;
				}
			}
		}

		public void LogGeoObjs(string path)
		{
			masterGeoObjtoGeoObj(true);
			if (GeoObjDB.list.Count == 0)
				return;
			using (FileStream stream = File.Open(path, FileMode.Append))
			{
				GeoObjDB.list = GeoObjDB.list.OrderBy(g => g.constructionIndex).ToList();//orders them before writing to xml file
				XmlSerializer serializer = new XmlSerializer(typeof(GeoObjDatabase));
				serializer.Serialize(stream, GeoObjDB);
			}
		}

		public void SaveGeoObjs(string path)
		{
			masterGeoObjtoGeoObj(true);
			if (GeoObjDB.list.Count == 0)
				return;
			using (FileStream stream = File.Create(path))
			{
				GeoObjDB.list = GeoObjDB.list.OrderBy(g => g.constructionIndex).ToList();//orders them before writing to xml file
				XmlSerializer serializer = new XmlSerializer(typeof(GeoObjDatabase));
				serializer.Serialize(stream, GeoObjDB);
			}
		}

		//[ContextMenu("Saves to File")] for debugging uncomment and make public
		/// <param name="clear">If true completely clears out the list of objects previously saved during session</param>
		private void masterGeoObjtoGeoObj( bool clear)
		{
			MasterGeoObj[] allObj = FindObjectsOfType<MasterGeoObj>().Where(g => (g.tag != "NoSave" && ((g.GetComponent<AnchorableBehaviour>() != null && !g.GetComponent<AnchorableBehaviour>().isAttached) || g.GetComponent<AnchorableBehaviour>() == null))).ToArray();
			if(clear)
				GeoObjDB.list = new List<GeoObj>();

			foreach (MasterGeoObj obj in allObj)
			{
				List<string> dependencyList = new List<string>();
				GeoObjDef thisObjDef;
				switch (obj.figType)
				{
					case GeoObjType.point:
						thisObjDef = determineDef(obj, GeoObjType.point);
						GeoObjDB.list.Add(new GeoObj(obj.figName, obj.transform.position, thisObjDef, GeoObjType.point));
						break;
					case GeoObjType.line:
						thisObjDef = determineDef(obj, GeoObjType.line);
						AbstractLineSegment thisLineRef = obj.GetComponent<AbstractLineSegment>();
						HW_GeoSolver.ins.geomanager.neighborsOfNode(thisLineRef.figName)
							.Where(d => HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPoint>() != null).ToList()
							.ForEach(d => dependencyList.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<MasterGeoObj>().figName));
						GeoObjDB.list.Add(new GeoObj(obj.figName, obj.transform.position, obj.transform.rotation, new figData.line(thisLineRef.vertex0, thisLineRef.vertex1), thisObjDef, dependencyList, GeoObjType.line));
						break;
					case GeoObjType.polygon:
						thisObjDef = determineDef(obj, GeoObjType.polygon);
						AbstractPolygon thisPolyRef = obj.GetComponent<AbstractPolygon>();
						dependencyList = new List<string>();
						thisPolyRef.pointList.ForEach(p => dependencyList.Add(p.figName));
						thisPolyRef.lineList.ForEach(l => dependencyList.Add(l.figName));
						GeoObjDB.list.Add(new GeoObj(
							obj.figName, obj.transform.position,
							obj.transform.rotation, 
							new figData.polygon(thisPolyRef.normDir, thisPolyRef.lineList.Count), thisObjDef, 
							dependencyList, GeoObjType.polygon));
						break;
					case GeoObjType.prism:
						thisObjDef = determineDef(obj, GeoObjType.prism);
						InteractablePrism thisPrismRef = obj.GetComponent<InteractablePrism>();// no abstract version
						List<string> bases = new List<string>();
						List<string> sides = new List<string>();
						List<string> edges = new List<string>();
						thisPrismRef.bases.ForEach(b => bases.Add(b.figName));
						thisPrismRef.sides.ForEach(s => sides.Add(s.figName));
						thisPrismRef.lineSegments.ForEach(e => edges.Add(e.figName));
						GeoObjDB.list.Add(new GeoObj(obj.figName, obj.transform.position, obj.transform.rotation, new figData.prism(bases, sides, edges), thisObjDef, GeoObjType.prism));
						break;
					case GeoObjType.pyramid:
						thisObjDef = determineDef(obj, GeoObjType.pyramid);
						DependentPyramid thisPyramidRef = GetComponent<DependentPyramid>();// no abstract version
						HW_GeoSolver.ins.geomanager.neighborsOfNode(thisPyramidRef.figName).ToList()
							.ForEach(d => dependencyList.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<MasterGeoObj>().figName));
						GeoObjDB.list.Add(new GeoObj(
							obj.figName, obj.transform.position,
							obj.transform.rotation, 
							new figData.pyramid( thisPyramidRef.apex.figName, thisPyramidRef.apex.transform.position),
							thisObjDef, dependencyList, GeoObjType.pyramid));
						break;
					case GeoObjType.circle:
						thisObjDef = determineDef(obj, GeoObjType.circle);
						AbstractCircle thisCircleRef = obj.GetComponent<AbstractCircle>();
						HW_GeoSolver.ins.geomanager.neighborsOfNode(thisCircleRef.figName).ToList()
							.ForEach(d => dependencyList.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<MasterGeoObj>().figName));
						GeoObjDB.list.Add(new GeoObj(
							obj.figName, obj.transform.position, 
							obj.transform.rotation, 
							new figData.circle(thisCircleRef.centerPos,thisCircleRef.edgePos, thisCircleRef.normalDir),
							thisObjDef, dependencyList, GeoObjType.circle));
						break;
					case GeoObjType.sphere:
						thisObjDef = determineDef(obj, GeoObjType.sphere);
						AbstractSphere thisSphereRef = obj.GetComponent<AbstractSphere>();
						HW_GeoSolver.ins.geomanager.neighborsOfNode(thisSphereRef.figName).ToList()
							.ForEach(d => dependencyList.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<MasterGeoObj>().figName));
						GeoObjDB.list.Add(new GeoObj(obj.figName, obj.transform.position, 
							obj.transform.rotation, 
							new figData.sphere(thisSphereRef.edgePosition), 
							thisObjDef ,dependencyList, GeoObjType.sphere));
						break;
					case GeoObjType.revolvedsurface:
						thisObjDef = determineDef(obj, GeoObjType.revolvedsurface);
						AbstractRevolvedSurface thisRevSurfRef = obj.GetComponent<AbstractRevolvedSurface>();
						HW_GeoSolver.ins.geomanager.neighborsOfNode(thisRevSurfRef.figName).ToList()
							.ForEach(d => dependencyList.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<MasterGeoObj>().figName));
						GeoObjDB.list.Add(new GeoObj(obj.figName, obj.transform.position, obj.transform.rotation, new figData.revSurf(thisRevSurfRef.normalDirection) ,thisObjDef , dependencyList, GeoObjType.revolvedsurface));
						break;
					//case GeoObjType.torus:
					//	thisObjDef = determineDef(obj, GeoObjType.torus);
					//																			Case doesn't exist yet.
					//	break;
					case GeoObjType.flatface:
						thisObjDef = GeoObjDef.none;
						GeoObjDB.list.Add(new GeoObj(obj.name, obj.transform.position, obj.transform.rotation, thisObjDef, GeoObjType.flatface));
						break;
					case GeoObjType.straightedge:
						thisObjDef = GeoObjDef.none;
						GeoObjDB.list.Add(new GeoObj(obj.name, obj.transform.position, obj.transform.rotation, thisObjDef, GeoObjType.straightedge));
						break;
					default:
						Debug.Log("Object type not supported within XMLManager! "+obj.figType+" was attempted and failed to serialize.");
						break;
				}
			}
			if (!clear)
			{
				GeoObjDB.list.Distinct();//if you dont clear the list then remove exact duplicates
			}
		}

		private GeoObjDef determineDef(MasterGeoObj obj, GeoObjType figType)
		{
			switch (figType)
			{
				case GeoObjType.point:
					if(obj.GetComponent<InteractablePoint>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if(obj.GetComponent<DependentPoint>() != null)
					{
						return GeoObjDef.Dependent;
					}else if (obj.GetComponent<StaticPoint>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.line:
					if (obj.GetComponent<InteractableLineSegment>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if (obj.GetComponent<DependentLineSegment>() != null)
					{
						return GeoObjDef.Dependent;
					}
					else if (obj.GetComponent<StaticLineSegment>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.polygon:
					if (obj.GetComponent<InteractablePolygon>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if (obj.GetComponent<DependentPolygon>() != null)
					{
						return GeoObjDef.Dependent;
					}
					else if (obj.GetComponent<StaticPolygon>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.prism:
					if (obj.GetComponent<InteractablePrism>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if (obj.GetComponent<DependentPrism>() != null)
					{
						return GeoObjDef.Dependent;
					}
					else if (obj.GetComponent<StaticSolid>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.pyramid:
					if (obj.GetComponent<DependentPyramid>() != null)				//Case doesn't exist yet
					//{
					//	return GeoObjDef.Interactable;
					//}
					//else if (obj.GetComponent<DependentPyramid>() != null)
					{
						return GeoObjDef.Dependent;
					}
					//else if (obj.GetComponent<staticPyramid>() != null)				//Case doesn't exist yet
					//{
					//	return GeoObjDef.Static;
					//}
					else
					{
						return GeoObjDef.none;
					}
				case GeoObjType.circle:
					if (obj.GetComponent<InteractableCircle>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if (obj.GetComponent<DependentCircle>() != null)
					{
						return GeoObjDef.Dependent;
					}
					else if (obj.GetComponent<StaticCircle>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.sphere:
					//if (obj.GetComponent<AbstractSphere>() != null)
					//{
					//	return GeoObjDef.Interactable;									//Case doesn't exist yet
					//}
					if (obj.GetComponent<DependentSphere>() != null)
					{
						return GeoObjDef.Dependent;
					}
					//else if (obj.GetComponent<StaticSphere>() != null)
					//{
					//	return GeoObjDef.Static;										//Case doesn't exist yet
					//}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.revolvedsurface:
					if (obj.GetComponent<InteractableRevolvedSurface>() != null)
					{
						return GeoObjDef.Interactable;
					}
					else if (obj.GetComponent<DependentRevolvedSurface>() != null)
					{
						return GeoObjDef.Dependent;
					}
					else if (obj.GetComponent<StaticRevolvedSurface>() != null)
					{
						return GeoObjDef.Static;
					}
					else
					{
						return GeoObjDef.Abstract;
					}
				case GeoObjType.torus:
					//if (obj.GetComponent<InteractableTorus>() != null)
					//{
					//	return GeoObjDef.Interactable;
					//}
					//else if (obj.GetComponent<DependentTorus>() != null)
					//{
					//	return GeoObjDef.Dependent;											//Cases do not exist yet
					//}
					//else if (obj.GetComponent<StaticTorus>() != null)
					//{
					//	return GeoObjDef.Static;
					//}
					//else
					//{
					//	return GeoObjDef.Abstract;
					//}
					return GeoObjDef.none;
				case GeoObjType.flatface:
					return GeoObjDef.none;

				case GeoObjType.straightedge:
					return GeoObjDef.none;

				default:
					Debug.Log("Object type not supported within XMLManager! " + obj.figType + " was attempted and failed to serialize.");

					return GeoObjDef.none;
			}
		}

	}



	[System.Serializable]
	public class GeoObjDatabase
	{
		public List<GeoObj> list = new List<GeoObj>();
	}

	[System.Serializable]
	public class GeoObj
	{
		/// <summary>
		/// Identifier for GeoObjs
		/// </summary>
		public string figName;
		/// <summary>
		/// Identifier for GeoObjs
		/// </summary>
		public int constructionIndex;
		/// <summary>
		/// Vector3 Location of Obj
		/// </summary>
		public Vector3 position;
		/// <summary>
		/// Rotation value for the geoobj
		/// </summary>
		public Quaternion rotation;
		/// <summary>
		/// GeoObj type
		/// </summary>
		public GeoObjType type;
		/// <summary>
		/// GeoObj definition
		/// </summary>
		public GeoObjDef definition;
		/// <summary>
		/// figName for dependent objects
		/// </summary>
		public List<string> dependencies = new List<string>();

		public DateTime currTime;

		public figData.line lineData;
		public figData.circle circleData;
		public figData.sphere sphereData;
		public figData.polygon polygonData;
		public figData.pyramid pyramidData;
		public figData.revSurf revSurfData;
		public figData.prism prismData;
		public string label;




#region Constructors
		public GeoObj() { }// default constructor needed to keep XMLSerialization working. DO NOT REMOVE

		public GeoObj(string figName, Vector3 position, GeoObjDef def, GeoObjType type)
		{
			this.figName = figName;
			this.position = position;
			this.definition = def;
			this.type = type;
			this.constructionIndex = int.Parse(new string (figName.ToCharArray().Where(c => Char.IsDigit(c)).ToArray()));
			this.currTime = System.DateTime.Now;
			if (alphabetLabel.firstLabelMade && alphabetLabel.labelDict.ContainsKey(figName))
				this.label = alphabetLabel.labelDict[figName];

		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, GeoObjDef def, GeoObjType type) : this(name, position, def, type)
		{
			this.rotation = rotation;
		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.line line, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, def, type)
		{
			this.rotation = rotation;
			this.dependencies = dependencyList;
			this.lineData = line;
		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.dependencies = dependencyList;
		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.polygon polygon, GeoObjDef def,List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.polygonData = polygon;
			this.dependencies = dependencyList;
		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.pyramid pyramid, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.pyramidData = pyramid;
			this.dependencies = dependencyList;
			this.type = type;

		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.circle circle, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.circleData = circle;
			this.dependencies = dependencyList;
			this.type = type;

		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.sphere sphere, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.sphereData = sphere;
			this.dependencies = dependencyList;
			this.type = type;
		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.prism prismData, GeoObjDef def, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.prismData = prismData;

		}

		public GeoObj(string name, Vector3 position, Quaternion rotation, figData.revSurf revSurf, GeoObjDef def, List<string> dependencyList, GeoObjType type) : this(name, position, rotation, def, type)
		{
			this.revSurfData = revSurf;
			this.dependencies = dependencyList;
		}

#endregion
	}

	[System.Serializable]
	public class figData{

		[System.Serializable]
		public struct line
		{
			public Vector3 vertex0;
			public Vector3 vertex1;

			public line(Vector3 vertex0, Vector3 vertex1)
			{
				this.vertex0 = vertex0;
				this.vertex1 = vertex1;
			}
		}

		[System.Serializable]
		public struct circle
		{
			public Vector3 centerPos;
			public Vector3 edgePos;
			public Vector3 normDir;

			public circle(Vector3 centerPos, Vector3 edgePos, Vector3 normDir)
			{
				this.centerPos = centerPos;
				this.edgePos = edgePos;
				this.normDir = normDir;
			}
		}

		[System.Serializable]
		public struct revSurf
		{
			public Vector3 normDir;
			
			public revSurf(Vector3 normDir)
			{
				
				this.normDir = normDir;
			}
		}
		[System.Serializable]
		public struct sphere
		{
			//centerPos defined by geoObj.position
			public Vector3 edgePos;

			public sphere(Vector3 edgePos)
			{
				this.edgePos = edgePos;
			}
		}

		[System.Serializable]
		public struct polygon
		{
			public Vector3 normDir;
			public int nSides;

			public polygon(Vector3 normDir, int nSides) : this()
			{
				this.normDir = normDir;
				this.nSides = nSides;
			}
			//pointList and lineList defined in geoObj.dependencies
		}

		[System.Serializable]
		public struct prism
		{
			//public Vector3 base2pos; defined on base itself
			//first polygon defined in geoObj.dependencies	
			public List<string> bases;
			public List<string> sides;
			public List<string> edges;

			public prism(List<string> bases, List<string> sides, List<string> edges)
			{
				this.bases = bases;
				this.sides = sides;
				this.edges = edges;
			}
		}

		[System.Serializable]
		public struct pyramid
		{
			//basePoly defined in geoObj.dependencies
			public string apexName;
			public Vector3 apexPos;//could define aswell in geoObj.dependencies, but might be best to just use this to spawn a point

			public pyramid(string name, Vector3 position) : this()
			{
				this.apexName = name;
				this.apexPos = position;
			}
		}
		
	}
}