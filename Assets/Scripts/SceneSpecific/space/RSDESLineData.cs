using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	#region Enumerators
	public enum lineType { circle, arc}
	#endregion

	[RequireComponent(typeof(LineRenderer))]

	/// <summary>
	/// This script allows for circles and arcs to be made between RSDESpushPinPreFabs in the RSDES scene.
	/// Also allows for the calculatino and display of the distance of arcs between RSDESpushPinPreFabs.
	/// The main contributor(s) to this script is NG
	/// Status: WORKING
	/// </summary>
	public class RSDESLineData : MonoBehaviour
	{

		#region Variables
		public List<pinData> associatedPins = new List<pinData>();
		public lineType LineType;

		private float _distance;

		/// <summary>
		/// Getting this value will calculate the distance between associated Pins
		/// </summary>
		public float distance
		{
			get
			{
				calculateDist();
				return _distance;
			}
			set
			{
				_distance = value;
			}
		}

		/// <summary>
		/// This is the position value
		/// </summary>
		private Vector3 _distanceTextPosition;

		private LineRenderer thisLR;

		private bool _isVisible;
		/// <summary>
		/// Use this property to change the enabled state of line renderer.
		/// </summary>
		public bool isVisible
		{
			set
			{
				_isVisible = value;
				if(thisLR != null)
					thisLR.enabled = value;
			}
			get
			{
				return _isVisible;
			}
		}

		public TMPro.TextMeshPro distanceText;
		private bool _isDistTextEnabled = false;
		/// <summary>
		/// Use this property to change the enabled state of distance text.
		/// </summary>
		public bool isDistTextEnabled
		{
			set
			{
				_isDistTextEnabled = value;

				if (distanceText != null)
				{
					distanceText.gameObject.SetActive(value);
					if(associatedPins != null && associatedPins.Count ==2)
					updateDistanceText();
				}
			}
			get
			{
				return _isDistTextEnabled;
			}
		}
		#endregion

		#region Monobehaviour Functions
		private void Start()
		{
			thisLR = GetComponent<LineRenderer>();
			if (associatedPins != null && associatedPins.Count > 0) {
				associatedPins.ForEach(p => p.pin.onPinMove += updateLineRenderers);
				associatedPins.ForEach(p => p.pin.onDelete += despawn);
				GetComponentInParent<LineRenderer>().startColor = associatedPins[0].pin.defaultColor;
				GetComponentInParent<LineRenderer>().endColor = associatedPins[1].pin.defaultColor;
			}
			if(RSDESManager.ins != null)
			{
				isDistTextEnabled = RSDESManager.ins.findDistance.ToggleState;
			}
		}
		#endregion

		#region Behaviour Functions
		private void despawn()
		{
			Destroy(gameObject);
		}

		private void updateLineRenderers()
		{
			switch (LineType)
			{
				case lineType.arc:
					thisLR.SetPositions(GeoPlanetMaths.greatArcCoordinates(associatedPins[0],associatedPins[1]));
					updateDistanceText();
					break;
				case lineType.circle:
					thisLR.SetPositions(GeoPlanetMaths.greatCircleCoordinates(associatedPins[0], associatedPins[1]));
					break;
				default:
					
					isVisible = false;
					break;
			}
		}

		private void calculateDist()
		{
			_distance =  GeoPlanetMaths.greatArcLength(associatedPins[0],associatedPins[1]);
			_distanceTextPosition = thisLR.GetPosition(Mathf.FloorToInt(thisLR.positionCount / 2));
		}

		private void updateDistanceText()
		{
			//distanceText.SetText(distance + " RAD");
			distanceText.SetText(distance*Mathf.Rad2Deg + " DEG");
			distanceText.transform.position = _distanceTextPosition*(1f+.01f*RSDESManager.EarthRadius);
		}
		#endregion

	}
}