using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;
using System.Linq;
using Leap;
using Leap.Unity.Gestures;

namespace IMRE.HandWaver.Interface
{

	public enum referenceFigureSet
	{
		none,
		natural,
		artificial
	}

	[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
	/// <summary>
	/// This script allows for double pinch scaling to occur in a scene. Objects need to be subscribed for this to function.
	/// The main contributor(s) to this script is CB, JH
	/// Status: WORKING
	/// </summary>
	public class worldScaleModifier : TwoHandedGesture
	{
		//refers to rsdesmanager for grabbing the earth
		public static worldScaleModifier ins;

		/// <summary>
		/// What figure set is currently in use.
		/// </summary>
		public referenceFigureSet figureSet;

		/// <summary>
		/// Allows the scaling tool to be bounded.
		/// </summary>
		public bool boundedScale = false;

        /// <summary>
        /// Sets the boundary for the scale tool.
        /// </summary>
        [Tooltip("These scale boundaries are ratios")]
        public Vector2 scaleBounds = new Vector2(.5f, 1.5f);

        /// <summary>
        /// Check to see if the scale is out of bounds.  Returns true when the scale is outside the defined <para=scaleBounds>scale bounds.</para>
        /// </summary>
        private bool scaleOutOfBounds
        {
            get
            {
                return boundedScale && !(absoluteScale <= scaleBounds.y && absoluteScale >= scaleBounds.x);
            }
        }

        public bool scaleDisplayShow
		{
			set
			{
				scaleDisplay.gameObject.SetActive(value);

			}
		}

		public bool scaleEnabled = false;

		public enum scalingMode { linear, exponential, log };

		public scalingMode mode = scalingMode.linear;

		public List<Transform> objectsToScale = new List<Transform>();

		private LineRenderer lr;
		private float initialDist;
		private Dictionary<Transform, Vector3> objectScales = new Dictionary<Transform, Vector3>();

		private Dictionary<Transform, Vector2> iconBoundsDictionary = new Dictionary<Transform, Vector2>();
		private Dictionary<Transform, referenceFigureSet> iconFigureSet = new Dictionary<Transform, referenceFigureSet>();
		public List<Transform> icons;
		public List<float> iconMeasures;
		public List<float> modelMeasures;
		public List<referenceFigureSet> figureSetType;

		public float minVisibleSize;
		public float maxVisibleSize;

		private List<Vector2> bounds;

		/// <summary>
		/// ratio of 1 unity meter.
		/// </summary>
		private float absoluteScale = 1f;
		private float initAbsoluteScale;
		public TMPro.TextMeshPro scaleDisplay;
		public Transform scaleIconParent;
		public Material lineMat;
		private AudioSource aSource;
		public AudioClip boundNoise;

		public float AbsoluteScale
		{
			get
			{
				return absoluteScale;
			}

			set
			{
				absoluteScale = value;


				if (Space.RSDESManager.ins != null)
				{
					Space.RSDESManager.SimulationScale = absoluteScale;
				}
				else
				{
					Debug.LogWarning("NO SPACE SCENE FOUND");
				}
				//apply the change in distance of the hands as new sizes for all objects to scale in the environment
				scaleIconParent.localScale = Vector3.one * absoluteScale;
				objectsToScale.ForEach(p => p.transform.localScale = objectScales[p] * absoluteScale);
			}
		}

		protected override void Start()
		{
			base.Start();
			aSource = GetComponent<AudioSource>();
			if(icons.Count != iconMeasures.Count || iconMeasures.Count != modelMeasures.Count)
			{
				Debug.LogWarning("CheckWorldScale Modifier Setup. Model Count Mismatch");
			}

			for (int i = 0; i < icons.Count; i++)
			{
				icons[i].localScale = Vector3.one * iconMeasures[i] / modelMeasures[i];
			}

			ins = this;
			objectsToScale.ForEach(p => objectScales[p] = p.transform.localScale);  //the inital local scale of all objects applied

			//Debug.Log("result of if check for icons and icon measures amounts being equivalent: "+(icons.Count == iconMeasures.Count));
			if (icons.Count == iconMeasures.Count && icons.Count == figureSetType.Count)
			{
				bounds = new List<Vector2>();

				for (int i = 0; i < icons.Count; i++)
				{
					bounds.Add(new Vector2(minVisibleSize / iconMeasures[i], maxVisibleSize / iconMeasures[i]));
					iconBoundsDictionary.Add(icons[i], bounds[i]);
					iconFigureSet.Add(icons[i], figureSetType[i]);
				}

				//setup line renderer
				lr = GetComponent<LineRenderer>();
				lr.material = lineMat;
				lr.useWorldSpace = true;
				lr.positionCount = 2;
				lr.enabled = false;

				if (scaleIconParent != null)
				{
					iconBoundsDictionary.Keys.ToList().ForEach(p => p.transform.parent = scaleIconParent);
				}
			}
			else
			{
				Debug.LogWarning("ICON MEASURES NOT KNOWN FOR ALL ICONS");
			}
		}

		protected override void WhenGestureActivated(Hand leftHand, Hand rightHand)
		{
			scaleIconParent.gameObject.SetActive(true);

			lr.enabled = true;  //turn on or off the line renderer based on double gasp status
			initialDist = Mathf.Max((leftHand.GetPinchPosition() - rightHand.GetPinchPosition()).magnitude, .01f);  //the initial distan cec between the hands
			initAbsoluteScale = AbsoluteScale;
		}

		protected override void WhenGestureDeactivated(Hand maybeNullLeftHand, Hand maybeNullRightHand, DeactivationReason reason)
		{
			lr.enabled = false;  //turn on or off the line renderer based on double gasp status
			scaleIconParent.gameObject.SetActive(false);
            playFeedback(reason);
		}


        protected override void WhileGestureActive(Hand leftHand, Hand rightHand)
		{
			if (scaleEnabled)
			{
				//line for feedback purposes
				lr.SetPosition(0, leftHand.GetPinchPosition());
				lr.SetPosition(1, rightHand.GetPinchPosition());

				//get the distance between the hands when they are both gripped
				float currDist = (leftHand.GetPinchPosition() - rightHand.GetPinchPosition()).magnitude;
				float r = Ratio(currDist);
				AbsoluteScale = initAbsoluteScale * r;

				//turn on and off icons based on their scale bounds.
				iconBoundsDictionary.Keys.ToList().ToList().ForEach(p => p.gameObject.SetActive(iconFigureSet[p] == figureSet && iconBoundsDictionary[p].x < AbsoluteScale && iconBoundsDictionary[p].y > AbsoluteScale));
				if (scaleDisplay != null)
				{
					scaleDisplay.SetText("1 : " + AbsoluteScale );
				}


				//move the scale icons to middle of line.
				scaleIconParent.transform.position = (rightHand.GetPinchPosition() + leftHand.GetPinchPosition()) / 2f;
			}
			else
			{
				iconBoundsDictionary.Keys.ToList().ForEach(p => p.gameObject.SetActive(false));

			}
		}

		/// <summary>
		/// Calculates the ratio of the curent distance between hands to the initial distance between hands
		/// </summary>
		/// <param name="currDist">current distance between hands that are pinching</param>
		/// <returns>ratio as a float, used as a multiplier</returns>
		private float Ratio(float currDist)
		{
			switch (mode)
			{
				case scalingMode.linear:
					return (currDist / initialDist);
				case scalingMode.exponential:
					return Mathf.Exp(currDist/initialDist);
				case scalingMode.log:
					return Mathf.Log(currDist / initialDist);
				default:
					return 1f;
			}
		}

		protected override bool ShouldGestureActivate(Hand leftHand, Hand rightHand)
		{
			return !scaleOutOfBounds && leftHand.IsPinching() && rightHand.IsPinching()&& !(leapHandDataLogger.ins.currHands.lIhand.isGraspingObject || leapHandDataLogger.ins.currHands.rIhand.isGraspingObject);
		}

		protected override bool ShouldGestureDeactivate(Hand leftHand, Hand rightHand, out DeactivationReason? deactivationReason)
		{
            bool outOfBounds = scaleOutOfBounds;
            if (outOfBounds)
            {
                if(absoluteScale > scaleBounds.y)
                {
                    absoluteScale = scaleBounds.y;
                }else if (absoluteScale < scaleBounds.x)
                {
                    absoluteScale = scaleBounds.x;
                }
            }

            if (outOfBounds || leapHandDataLogger.ins.currHands.lIhand.isGraspingObject || leapHandDataLogger.ins.currHands.rIhand.isGraspingObject)
			{
				deactivationReason = DeactivationReason.CancelledGesture;
			}
			else
			{
				deactivationReason = DeactivationReason.FinishedGesture;
			}
			return outOfBounds || (!(leftHand.IsPinching() && rightHand.IsPinching()) && !(leapHandDataLogger.ins.currHands.lIhand.isGraspingObject || leapHandDataLogger.ins.currHands.rIhand.isGraspingObject));
		}


		public static void advanceFigureType()
		{
			switch (ins.figureSet)
			{
				case referenceFigureSet.artificial:
					ins.figureSet = referenceFigureSet.none;
					break;
				default:
					ins.figureSet++;
					break;
			}
		}


        private void playFeedback(DeactivationReason reason)
        {
            switch (reason)
            {
                case DeactivationReason.FinishedGesture:
                    break;
                case DeactivationReason.CancelledGesture:
					playBoundNoise();
                    break;
                default:
                    break;
            }
        }

		private void playBoundNoise()
		{
			aSource.clip = boundNoise;
			aSource.Play();
		}
	}
}
