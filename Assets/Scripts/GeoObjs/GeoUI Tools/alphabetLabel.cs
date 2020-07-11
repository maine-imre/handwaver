/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Leap.Unity.Interaction;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace IMRE.HandWaver
{
	[RequireComponent(typeof(InteractionBehaviour), typeof(AnchorableBehaviour), typeof(TextMeshPro))]

/// <summary>
/// Labeling system for AbstractGeoObjs.
/// Needs UX overhaul.
/// Currently not used.
/// </summary>
	internal class alphabetLabel : MonoBehaviour {
#region Constructors
public static alphabetLabel Constructor(){
	return GameObject.Instantiate(PrefabManager.GetPrefab("AlphabetLabel")).GetComponent<alphabetLabel>();
}

#endregion

		internal static int totalCount = 0;
		public static Dictionary<string, string> labelDict = new Dictionary<string, string>();
		private int _thisCount;
		private string _label;
		private TextMeshPro labelText;
		private InteractionBehaviour thisIBehave;
		private AnchorableBehaviour thisABehave;
		public AbstractGeoObj recentHit;
		private bool setBool = false;
		public static bool firstLabelMade = false;

		public string Label
		{
			get
			{
				if (!setBool)
				{
					string result = "";
					if (_thisCount >= 25)
						result = ((char)(65 + (_thisCount / 25) - 1)).ToString();

					return result + _label;
				}
				else
				{
					return _label.ToString();
				}
			}
			set
			{
				_label = value;
				if(value != null)
					setBool = true;
				updateText();
			}
		}

		public void spawnOnMGO(AbstractGeoObj MGO, string label)
		{
			_thisCount = (int)(label.ToCharArray()[label.ToCharArray().Length - 1] - 65);
			if (_thisCount > totalCount)
				totalCount = _thisCount + 1;
			recentHit = MGO;
			this.Label = label;
			attachToMGO();
		}

		private void Start()
		{
			if(!firstLabelMade)
				firstLabelMade = true;
			thisIBehave = GetComponent<InteractionBehaviour>();
			thisABehave = GetComponent<AnchorableBehaviour>();

			thisIBehave.OnGraspEnd += graspEnd;

			labelText = GetComponent<TextMeshPro>();
			_thisCount = totalCount++;
			_label = ((char)(65 + (_thisCount % 25))).ToString();
			updateText();

		}

		private void graspEnd()
		{
			if (recentHit != null)
				attachToMGO();
		}

		private void attachToMGO()
		{
			labelDict.Add(recentHit.figName, Label);
			thisIBehave.ignoreContact = true;
			thisIBehave.ignoreHoverMode = IgnoreHoverMode.Both;
			thisIBehave.ignoreGrasping= true;
			thisABehave.enabled = false;
			transform.SetParent(recentHit.transform);
			recentHit.Label = Label;
			Destroy(transform.GetChild(0).gameObject);
			StartCoroutine(transformFollow(recentHit));
		}

		private IEnumerator transformFollow(AbstractGeoObj followMGO)
		{
			while (followMGO.isActiveAndEnabled)
			{
				transform.position = followMGO.transform.position;
				transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
				yield return new WaitForEndOfFrame();
			}
			Destroy(gameObject);//if attached object doesnt exist anymore
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.GetComponent<AbstractGeoObj>() != null)
				recentHit = other.gameObject.GetComponent<AbstractGeoObj>();
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.GetComponent<AbstractGeoObj>() != null && other.gameObject.GetComponent<AbstractGeoObj>() == recentHit)
				recentHit = null;
		}

		private void updateText()
		{
			labelText.text = Label;
		}

	}
}