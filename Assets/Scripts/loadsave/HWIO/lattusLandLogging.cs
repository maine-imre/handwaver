using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using IMRE.HandWaver.Lattice;
using UnityEngine;

namespace IMRE.HandWaver.HWIO
{
/// <summary>
/// Logging features for lattice land.  Need to eliminate extraneous data.
/// </summary>
	public class lattusLandLogging : MonoBehaviour
	{
		public static lattusLandLogging ins;
		public latticeLandDB currLatticeData;

		public string sessionID
		{
			get
			{
				return XMLManager.sessionID;
			}
		}


		private void Awake()
		{
			ins = this;

			checkLog();
			#if StandaloneWindows64
			commandLineArgumentParse.logStateChange.AddListener(checkLog);
			#endif
		}


		private void checkLog()
		{
//#if !UNITY_EDITOR
//						
//			if (!Directory.Exists(Application.dataPath + @"/../dataCollection/"))
//			{
//				Directory.CreateDirectory(Application.dataPath + @" /../dataCollection/");
//			}
//
//			if(commandLineArgumentParse.logCheck())
//			{
//				StartCoroutine(LogShearingData());
//			}
//#endif
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
				LogLattice(Application.dataPath + @"/../dataCollection/LatticeLandData" + sessionID + @".hwlog");
			}
		}

		/// <summary>
		///  Logs data
		/// </summary>
		/// <param name="path">path to log out a .xml</param>
		private void LogLattice(string path)
		{
			updateCurrentData();

			using (FileStream stream = File.Open(path, FileMode.Append))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(latticeLandDB));
				serializer.Serialize(stream, currLatticeData);
			}
		}

		private void updateCurrentData()
		{
			currLatticeData.leftHand = new latticeLandData(IMRE.HandWaver.Lattice.FingerPointLineMaker.left);
			currLatticeData.rightHand = new latticeLandData(IMRE.HandWaver.Lattice.FingerPointLineMaker.right);

		}
	}


	[System.Serializable]
	public class latticeLandDB
	{
		public DateTime currTime = DateTime.Now;

		public latticeLandData leftHand;
		public latticeLandData rightHand;

	}


	[System.Serializable]
	public class latticeLandData
	{

		public FingerPointLineMaker.pinType currentPinType = FingerPointLineMaker.pinType.none;

		public latticeLandData()
		{

		}

		public latticeLandData(FingerPointLineMaker source) : this()
		{
			this.currentPinType = source.thisPinType;
		}
	}
}