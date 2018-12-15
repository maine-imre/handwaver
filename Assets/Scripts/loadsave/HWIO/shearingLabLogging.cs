using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace IMRE.HandWaver.HWIO
{
/// <summary>
/// Logging features for sheairng lab in Bock's MST Thesis.
/// Depreciated
/// </summary>
	public class shearingLabLogging : MonoBehaviour
	{
		public static shearingLabLogging ins;
		public shearingDataBase currShearingData;

		public string sessionID {
			get
			{
				return XMLManager.sessionID;
			}
		}


		private void Awake()
		{
			ins = this;
			commandLineArgumentParse.logStateChange.AddListener(checkLog);

			checkLog();
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
				StartCoroutine(LogShearingData());
			}
#endif
		}

		private IEnumerator LogShearingData()
		{
			if (!Directory.Exists(Application.dataPath + @"/../dataCollection/"))
			{
				Directory.CreateDirectory(Application.dataPath + @" /../dataCollection/");
			}
			while (true)
			{
				yield return new WaitForSecondsRealtime(1f);
				LogShearing(Application.dataPath + @"/../dataCollection/ShearingLabHistory" + sessionID + @".hwlog");
			}
		}

		/// <summary>
		///  Logs data
		/// </summary>
		/// <param name="path">path to log out a .xml</param>
		private void LogShearing(string path)
		{
			updateCurrentData();
			if (currShearingData.list.Count == 0)
				return;

			using (FileStream stream = File.Open(path, FileMode.Append))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(shearingDataBase));
				serializer.Serialize(stream, currShearingData);
			}
		}

		private void updateCurrentData()
		{
			foreach (IMRE.HandWaver.Shearing.constructTriangleOnParallelLines baseObj in FindObjectsOfType<IMRE.HandWaver.Shearing.constructTriangleOnParallelLines>())
			{
				currShearingData.list.Add(new shearData(baseObj));
			}

			foreach (IMRE.HandWaver.Shearing.constructPyramidOnParallelPlanes baseObj in FindObjectsOfType<IMRE.HandWaver.Shearing.constructPyramidOnParallelPlanes>())
			{
				currShearingData.list.Add(new shearData(baseObj));
			}

		}
	}


	[System.Serializable]
	public class shearingDataBase
	{
		public List<shearData> list = new List<shearData>();
	}


	[System.Serializable]
	public class shearData
	{

		public DateTime currTime;
		public float crossSectionHeight;
		public Vector3[] crossSectionVerticies;

		public shearData(IMRE.HandWaver.Shearing.constructTriangleOnParallelLines source) : this()
		{
			this.crossSectionHeight = source.crossSectionHeight;
			this.crossSectionVerticies = source.findCrossSectionSegment();
		}

		public shearData(IMRE.HandWaver.Shearing.constructPyramidOnParallelPlanes source) : this()
		{
			this.crossSectionHeight = source.crossSectionHeight;
			this.crossSectionVerticies = source.myMesh.vertices;
		}

		public shearData()
		{
			this.currTime = DateTime.Now;

		}
	}
	}