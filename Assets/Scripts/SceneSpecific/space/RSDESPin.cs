using Leap.Unity;
using Leap.Unity.Interaction;
using Leap.Unity.LeapPaint_v3;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	[RequireComponent(typeof(LineRenderer), typeof(InteractionBehaviour), typeof(AnchorableBehaviour))]

	/// <summary>
	/// Handels user interaction with the RSDES Pins and renders features local to the pin.
	/// The main contributor(s) to this script is NG
	/// Status: WORKING
	/// </summary>
	public class RSDESPin : MonoBehaviour
	{
#region Constructors
	public static RSDESPin Constructor() {
		return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/RSDESoushPinPrefab")).GetComponent<RSDESPin>();
	}
#endregion

		#region Variables

		private static int count = 0;

		//These are the different types of pins
		public enum pintype { Star, Sun, Moon, northPole, southPole };

		public pintype myPintype = pintype.Star;
		public GameObject localPanel;
		public MeshRenderer pinHead;
		public Transform pinTip;
		public Color hoverColor = Color.grey;
		public Color selectColor = Color.yellow;
		public Transform star;
		public TMPro.TextMeshPro latlongLabel;
		public PressableUI selectButton;

		internal Action onPinMove;
		private InteractionBehaviour iBehave;
		private AnchorableBehaviour aBehave;
		private Vector3 recentContact;
		private LineRenderer myLR;
		private bool _onSurface = false;
		private bool selectState = false;
		private float onSurfaceTolerance = 0.001f;

		public Transform polarBear;
		public Transform tuxPenguin;

		private float moonRad = 1737000f;
		private float sunRad = 695508000f;

		private bool onSurface
		{
			get
			{
				return ((Vector3.Distance(pinTip.position, RSDESManager.earthPos) - RSDESManager.EarthRadius < onSurfaceTolerance));
			}

			set
			{
				if (RSDESManager.verboseLogging)
					Debug.Log(value + name);

				if (latRenderer != null)
				{
					latRenderer.enabled = value;
				}

				if (altitudeRenderer != null)
				{
					altitudeRenderer.enabled = value;
				}

				if (longRenderer != null)
				{
					longRenderer.enabled = value;
				}

				if (azimuthRenderer != null)
				{
					azimuthRenderer.enabled = value;
				}

				if (terminatorRenderer != null)
				{
					terminatorRenderer.enabled = value;
				}

				if (horizonStatus)
				{
					horizonPlaneObj.SetActive(value);
				}
				if (value || (myPintype == pintype.northPole || myPintype == pintype.southPole))
				{
					RSDESManager.onEarthTilt += onEarthTilt;

					enableLocalPanel();
				}
				_onSurface = value;
			}
		}

		private void enableLocalPanel()
		{
			if (myPintype == pintype.Star)
				localPanel.SetActive(true);
		}

		internal Color defaultColor = Color.white;

		public Vector3 Offset;

		private Vector2 latlong;

		public Vector2 Latlong
		{
			get
			{
				return latlong;
			}

			set
			{
				if (onPinMove != null && onPinMove.Method != null)
				{
					onPinMove.Invoke();
				}
				switch (myPintype)
				{
					case pintype.Star:
						latlong = value;
						if (star == null)
						{
							star = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/RSDESStar_CS")).transform;
							star.GetComponent<MeshRenderer>().material.color = defaultColor;
						}
						star.transform.position = GeoPlanetMaths.directionFromLatLong(latlong).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos);
						break;

					case pintype.Sun:
						//udpateTimeFromSun();
						latlong = value;
						star.GetComponent<MeshRenderer>().material = RSDESManager.ins.SunMaterial;
						star.localScale = Vector3.one * sunRad * RSDESManager.SimulationScale;
						star.transform.position = GeoPlanetMaths.directionFromLatLong(latlong).ScaleMultiplier(RSDESManager.sunDist * RSDESManager.SimulationScale).Translate(RSDESManager.earthPos);

						if (pinHead.GetComponent<MeshRenderer>() != null)
						{
							pinHead.GetComponent<MeshRenderer>().material = RSDESManager.ins.SunMaterial;
						}
						break;

					case pintype.Moon:
						//updateTimeFromMoon();
						latlong = value;
						star.GetComponent<MeshRenderer>().material = RSDESManager.ins.MoonMaterial;
						star.localScale = Vector3.one * moonRad * RSDESManager.SimulationScale;
						star.transform.position = GeoPlanetMaths.directionFromLatLong(latlong).ScaleMultiplier(RSDESManager.moonDist * RSDESManager.SimulationScale).Translate(RSDESManager.earthPos);

						if (pinHead.GetComponent<MeshRenderer>() != null)
						{
							pinHead.GetComponent<MeshRenderer>().material = RSDESManager.ins.MoonMaterial;
						}
						break;

					case pintype.northPole:
						//star.transform.position = GeoPlanetMaths.directionFromLatLong(new Vector2(90,0)).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos);
						latlong = new Vector2(90, 0);
						if (GetComponent<InteractionBehaviour>().isGrasped)
						{
							RSDESManager.earthRot = Quaternion.FromToRotation(Vector3.up, GeoPlanetMaths.directionFromLatLong(value));
							RSDESManager.ins.earthTilt();
						}
						break;

					case pintype.southPole:
						latlong = new Vector2(-90, 0);
						if (GetComponent<InteractionBehaviour>().isGrasped)
						{
							RSDESManager.earthRot = Quaternion.FromToRotation(Vector3.down, GeoPlanetMaths.directionFromLatLong(value));
							RSDESManager.ins.earthTilt();
						}
						break;
				}

				this.transform.position = GeoPlanetMaths.directionFromLatLong(latlong) * RSDESManager.EarthRadius + RSDESManager.earthPos;
				this.transform.rotation = Quaternion.FromToRotation(Vector3.down, GeoPlanetMaths.directionFromLatLong(latlong).normalized)/**RSDESManager.earthRot*/;

				if (latlongLabel != null)
					latlongLabel.SetText(GeoPlanetMaths.dmsFromFloat(latlong.x, true) + "  " + GeoPlanetMaths.dmsFromFloat(latlong.y, false)); // in deg min sec
			}
		}

		internal void playParticleEffect()
		{
			GetComponentInChildren<ParticleSystem>().Play();
		}

		internal Action onDelete;

		public PressableUI showLatitude;
		public PressableUI showLongitude;
		//public PressableUI showAzimuth;  //do this with latitude
		//public PressableUI showAltitude;  //do this with longitude

		private LineRenderer altitudeRenderer;

		private LineRenderer azimuthRenderer;

		public PressableUI showTerminator;

		[Range(0, 1)]
		private static float colorPercent = 0.15f;

		public PressableUI starFieldMode;
		private LineRenderer[] starRays;

        public enum starFieldSelect {single,allPins,allPinsEqualAltitude,withinEarth,LargeArray,off}
		private starFieldSelect starMode = starFieldSelect.off;

		private void updateStarMode()
		{
			StarMode = StarMode;
		}

		private readonly int starRayGridSize = 15;

		/// <summary>
		/// Distance between star ray grid in earth meters.
		/// </summary>
		private readonly float starRayGridSpacing = 2000000f;
        private int equalAltitudeCount = 50;

		private LineRenderer spawnStarRay(){
			LineRenderer tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/StarRay")).GetComponent<LineRenderer>();
			tmp.transform.parent = this.transform;
			return tmp;
		}


        public starFieldSelect StarMode
		{
			get
			{
				return starMode;
			}

			set
			{
				if (value != starMode)
				{
					if (value == starFieldSelect.allPins || value == starFieldSelect.allPinsEqualAltitude)
					{
						RSDESManager.ins.updateStarFieldsGlobal += updateStarMode;
					}
					else
					{
						RSDESManager.ins.updateStarFieldsGlobal -= updateStarMode;
					}
				}
				bool initialize = (value != starMode);
				if (!initialize)
				{
					switch (starMode)
					{
						case starFieldSelect.allPins:
							initialize = (starRays.Length != RSDESManager.ins.pinnedPoints.Where(p => p.pin.myPintype == pintype.Star).Count());
							break;
						case starFieldSelect.allPinsEqualAltitude:
							initialize = (starRays.Length/50 != RSDESManager.ins.pinnedPoints.Where(p => p.pin.myPintype == pintype.Star && p.pin.name != this.name).Count());
							break;
						default:
							break;
					}
				}

				//why aren't always deleting.
				if (initialize && starRays != null && starRays.Length > 0)
					starRays.ToList().Where(sr => sr != null && sr.GetComponent<LineRenderer>() != null).ToList().ForEach(sr => Destroy(sr));

				switch (starMode)
				{
					case starFieldSelect.single:
						//single
						if (initialize)
						{
							starRays = new LineRenderer[1];
							starRays[0] = spawnStarRay();
						}
						starRays[0].SetPositions(GeoPlanetMaths.starRayRendererCoordiantes(dbPinData));

						starRays.ToList().ForEach(p => p.startWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.endWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.positionCount = 2);

						break;

					case starFieldSelect.allPins:
						//single + all points

						List<pinData> pins = RSDESManager.ins.pinnedPoints.Where(p => p.pin.myPintype == pintype.Star || p.pin == this).ToList();
						if (pins.Count > 0)
						{
							if (initialize)
							{
								LineRenderer currStarRay = starRays[0];


								starRays = new LineRenderer[pins.Count];
								starRays[0] = currStarRay;
							}

							starRays[0].SetPositions(GeoPlanetMaths.starRayRendererCoordiantes(dbPinData));     //create new one
							starRays[0].startColor = defaultColor;
							starRays[0].endColor = defaultColor;
						}

						pinData cachedPinData = dbPinData;

						for (int i = 0; i < pins.Count; i++)
						{
							if (initialize)
							{
								starRays[i] = spawnStarRay();
								List<LineRenderer> l = starRays.ToList();
								l.RemoveAll(p => p == null);
								l.ForEach(p => p.GetComponent<LineRenderer>().startColor = defaultColor);
								l.ForEach(p => p.GetComponent<LineRenderer>().endColor = defaultColor);
							}
							starRays[i].SetPositions(GeoPlanetMaths.starRayRendererCoordiantes(cachedPinData, pins[i]));
						}

						//starRays[0].SetPositions(GeoPlanetMaths.starRayRendererCoordiantes(dbPinData));     //create new one

						starRays.ToList().ForEach(p => p.startWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.endWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.positionCount = 2);

						break;

					case starFieldSelect.allPinsEqualAltitude:
						// all pins equal altitude
						List<pinData> points = RSDESManager.ins.PinnedPoints.Where(p => p.pin.myPintype == pintype.Star && p.pin.name != this.name).ToList();

						if (initialize)
						{
							starRays = new LineRenderer[equalAltitudeCount * (points.Count)];
						}

						if (points.Count > 0)
						{
							Vector3 thisPinDirection = this.directionFromLatLong();
							for (int i = 0; i < points.Count; i++)
							{
								Vector3 diff = points[i].pin.directionFromLatLong() - this.directionFromLatLong();
								Color pinBcolor = points[i].pin.defaultColor;
								for (int j = 0; j < equalAltitudeCount; j++)
								{
									if (initialize)
									{
										starRays[equalAltitudeCount * i + j] = spawnStarRay();
										starRays[equalAltitudeCount * i + j].startColor = pinBcolor;
										starRays[equalAltitudeCount * i + j].endColor = pinBcolor;
									}
									starRays[equalAltitudeCount * i + j].SetPositions(GeoPlanetMaths.starRayRendererCoordiantes(dbPinData, GeoPlanetMaths.latlong(thisPinDirection + Quaternion.AngleAxis(j * 360 / equalAltitudeCount, thisPinDirection) * diff)));
								}
							}
						}

						starRays.ToList().ForEach(p => p.startWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.endWidth = RSDESManager.LR_width);
						starRays.ToList().ForEach(p => p.positionCount = 2);

						break;

					case starFieldSelect.withinEarth:
						//corase grid limited to the rays that pass through earth.
						Vector3[,] starRayData2 = GeoPlanetMaths.starRayRendererCoordiantesWithinEarth(this.transform.position, starRayGridSize);

						if (initialize || starRays == null)
							starRays = new LineRenderer[starRayData2.GetLength(1)];

						//can this be optimized with Linq?
						for (int i = 0; i < starRayData2.GetLength(1); i++)
						{
							if (initialize)
							{
								starRays[i] = spawnStarRay();
							}
							starRays[i].startWidth = RSDESManager.LR_width;
							starRays[i].endWidth = RSDESManager.LR_width;
							starRays[i].positionCount = 2;
							starRays[i].SetPosition(0, starRayData2[0, i]);
							starRays[i].SetPosition(1, starRayData2[1, i]);
						}
						break;

					case starFieldSelect.LargeArray:
						//grid
						Vector3[,] starRayData = GeoPlanetMaths.starRayRendererCoordiantes(this.transform.position, RSDESManager.SimulationScale * starRayGridSpacing, starRayGridSize, GeoPlanetMaths.coordinateSystem.cartesian);
						if (initialize || starRays == null)
							starRays = new LineRenderer[starRayData.GetLength(1)];

						//can this be optimized with Linq?
						for (int i = 0; i < starRayData.GetLength(1); i++)
						{
							if (initialize)
							{
								starRays[i] = spawnStarRay();
							}
							starRays[i].startWidth = RSDESManager.LR_width;
							starRays[i].endWidth = RSDESManager.LR_width;
							starRays[i].positionCount = 2;
							starRays[i].SetPosition(0, starRayData[0, i]);
							starRays[i].SetPosition(1, starRayData[1, i]);
						}
						break;

					case starFieldSelect.off:
						//off
						if (initialize || starRays == null)
						{
							starRays = new LineRenderer[0];
						}
						break;
				}
				if (initialize && starMode != starFieldSelect.allPinsEqualAltitude)
				{
					starRays.ToList().ForEach(p => p.GetComponent<LineRenderer>().startColor = defaultColor);
					starRays.ToList().ForEach(p => p.GetComponent<LineRenderer>().endColor = defaultColor);
				}
			}
		}

		public PressableUI horizonPlanes;
		private LineRenderer latRenderer;
		private LineRenderer longRenderer;
		private LineRenderer terminatorRenderer;
		public GameObject horizonPlaneObj;
		private bool horizonStatus = false;

		#endregion Variables

		#region Monobehaviour Functions

		private void Start()
		{
			if (myPintype == pintype.Star)
			{
				gameObject.name = "RSDESPin " + count++;
			}
			else
			{
				gameObject.name = myPintype + " RSDESPin " + count++;
			}

			iBehave = GetComponent<InteractionBehaviour>();
			aBehave = GetComponent<AnchorableBehaviour>();
			myLR = GetComponent<LineRenderer>();
			var haloComponent = pinHead.gameObject.GetComponent("Halo");
			var haloEnabledProperty = haloComponent.GetType().GetProperty("enabled");
			haloEnabledProperty.SetValue(haloComponent, false, null);
			//This handles the pin head color changes
			if (pinHead == null)
			{
				Debug.Log("pinHead not set in " + name);
				gameObject.SetActive(false);
			}
			pinHead.material.color = UnityEngine.Random.ColorHSV(0, 1, 0.9f, 1, .9f, 1);
			defaultColor = pinHead.material.color;

			aBehave.OnAttachedToAnchor += onAttached;
			aBehave.OnDetachedFromAnchor += onDetached;

			//This handles the grasp events for the pin
			iBehave.OnGraspBegin += beginGrasp;
			iBehave.OnHoverStay += hoverUpdate;
			if (myPintype != pintype.Star)
			{
				RSDESManager.onEarthTilt += onEarthTilt;
			}
			localPanel.SetActive(false);
			horizonPlaneObj.GetComponent<MeshRenderer>().material.SetColor("_TintColor", defaultColor);

			tuxPenguin.gameObject.SetActive(myPintype == pintype.southPole);
			polarBear.gameObject.SetActive(myPintype == pintype.northPole);

			onPinMove += updateStarMode;
			onPinMove += RSDESManager.ins.callUpdateStarFieldsGlobal;
			if(myPintype == pintype.Moon || myPintype == pintype.Sun)
			{
				iBehave.ignoreGrasping = true;
			}
		}

		public pinData dbPinData
		{
			get
			{
				return RSDESManager.ins.pinDB[this];
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (myPintype == pintype.Star && (iBehave != null && iBehave.isGrasped) && collider.GetComponentInParent<RSDESManager>() != null)
			{
				onSurface = true;
				setupPin();
				iBehave.ReleaseFromGrasp();

                //invoke onPinMove when the pin is placed on the surface.  This is needed for star rays, possibly for other features.
                if (onPinMove != null && onPinMove.Method != null)
                {
                    onPinMove.Invoke();
                }
			}
		}

		private void setupPin()
		{
			pinData myPinData = new pinData(this, pinTip.transform.position);
			RSDESManager.ins.PinnedPoints.Add(myPinData);           //Might need a way to specify rather than all contact points
			if (!RSDESManager.ins.pinDB.ContainsKey(this))
			{
				RSDESManager.ins.pinDB.Add(this, myPinData);
			}
		}

		private void OnTriggerStay(Collider collider)
		{
			//if (myPintype == pintype.Star)
			{
				if ((iBehave != null && iBehave.isGrasped) && collider.GetComponentInParent<RSDESManager>() != null && this.GetComponent<InteractionBehaviour>().isGrasped == true)
				{
					snapToSurface();
				}
			}
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider.GetComponentInParent<RSDESManager>() != null)
			{
				iBehave.ReleaseFromGrasp();
				snapToSurface();
			}

			if (onPinMove != null && onPinMove.Method != null)
				onPinMove.Invoke();
		}

		private void OnDisable()
		{
			if (RSDESManager.ins != null)
			{
				//RSDESManager.ins.PinnedPoints.RemoveAll(p => p.pin == this);
				RSDESManager.onEarthTilt -= onEarthTilt;
			}
		}

		#endregion Monobehaviour Functions

		#region Behaviour Functions

		private bool allowDespawn = false;

		/// <summary>
		/// Use this function to delete a pin.
		/// </summary>
		internal void despawn()
		{
			if (allowDespawn)
			{
				if (latRenderer != null)
				{
					Destroy(latRenderer.gameObject);
				}
				if (longRenderer != null)
				{
					Destroy(longRenderer.gameObject);
				}
				if (terminatorRenderer != null)
				{
					Destroy(terminatorRenderer.gameObject);
				}
				if (azimuthRenderer != null)
				{
					Destroy(azimuthRenderer.gameObject);
				}
				if (altitudeRenderer != null)
				{
					Destroy(altitudeRenderer.gameObject);
				}

				if (onDelete != null && onDelete.Method != null)
				{
					onDelete.Invoke();
				}

				RSDESManager.ins.PinnedPoints.RemoveAll(p => p.pin == this);
				RSDESManager.ins.SelectedPoints.RemoveAll(p => p.pin == this);
				Destroy(gameObject);
			}
		}

		private void updateTimeFromMoon()
		{
			throw new NotImplementedException();
		}

		private void udpateTimeFromSun()
		{
			throw new NotImplementedException();
		}

		private LineRenderer RSDESGeneratedLine(){
			return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/RSDESGeneratedLine")).GetComponent<LineRenderer>();
		}

		public void toggleAltitude()
		{
			if (!onSurface && !iBehave.isGrasped)
				return;

			if (altitudeRenderer == null)
				altitudeRenderer = RSDESGeneratedLine();
			altitudeRenderer.positionCount = 3;
			altitudeRenderer.startWidth = RSDESManager.LR_width;
			altitudeRenderer.endWidth = RSDESManager.LR_width;
			altitudeRenderer.useWorldSpace = true;
			altitudeRenderer.startColor = Color.Lerp(defaultColor, Color.white, colorPercent);
			altitudeRenderer.endColor = Color.Lerp(defaultColor, Color.white, colorPercent);

			setAltitudePos();
			altitudeRenderer.loop = false;
			altitudeRenderer.enabled = !altitudeRenderer.enabled;
			onPinMove += setAltitudePos;
		}

		public void toggleAzimuth()
		{
			if (!onSurface && !iBehave.isGrasped)
				return;

			if (azimuthRenderer == null)
				azimuthRenderer = RSDESGeneratedLine();
			azimuthRenderer.positionCount = 3;
			azimuthRenderer.startWidth = RSDESManager.LR_width;
			azimuthRenderer.endWidth = RSDESManager.LR_width;
			azimuthRenderer.useWorldSpace = true;
			azimuthRenderer.startColor = Color.Lerp(defaultColor, Color.white, colorPercent);
			azimuthRenderer.endColor = Color.Lerp(defaultColor, Color.white, colorPercent);

			setAzimuthPos();
			azimuthRenderer.loop = false;
			azimuthRenderer.enabled = !azimuthRenderer.enabled;
			onPinMove += setAzimuthPos;
		}

		public void toggleLat()
		{
			if (!onSurface && !iBehave.isGrasped)
				return;

			if (latRenderer == null)
				latRenderer = RSDESGeneratedLine();
			latRenderer.positionCount = RSDESManager.LR_Resolution;
			latRenderer.startWidth = RSDESManager.LR_width;
			latRenderer.endWidth = RSDESManager.LR_width;
			latRenderer.useWorldSpace = true;
			latRenderer.startColor = Color.Lerp(defaultColor, Color.white, colorPercent);
			latRenderer.endColor = Color.Lerp(defaultColor, Color.white, colorPercent);

			setLatPos();
			latRenderer.loop = true;
			latRenderer.enabled = !latRenderer.enabled;
			onPinMove += setLatPos;
		}

		public void toggleLong()
		{
			if (!onSurface && !iBehave.isGrasped)
				return;

			if (longRenderer == null)
				longRenderer = RSDESGeneratedLine();

			longRenderer.positionCount = RSDESManager.LR_Resolution;
			longRenderer.startWidth = RSDESManager.LR_width;
			longRenderer.endWidth = RSDESManager.LR_width;
			longRenderer.useWorldSpace = true;
			longRenderer.startColor = Color.Lerp(defaultColor, Color.white, colorPercent);
			longRenderer.endColor = Color.Lerp(defaultColor, Color.white, colorPercent);

			setLongPos();
			longRenderer.loop = true;
			longRenderer.enabled = !longRenderer.enabled;
			onPinMove += setLongPos;
		}

		internal void setLongPos()
		{
			longRenderer.SetPositions(this.longAtPoint());
		}

		public void toggleTerminator()
		{
			if (!onSurface && !iBehave.isGrasped)
				return;
			if (terminatorRenderer == null)
				terminatorRenderer = RSDESGeneratedLine();
			else
				terminatorRenderer.enabled = !terminatorRenderer.enabled;

			terminatorRenderer.positionCount = RSDESManager.LR_Resolution;
			terminatorRenderer.startWidth = RSDESManager.LR_width;
			terminatorRenderer.endWidth = RSDESManager.LR_width;
			terminatorRenderer.useWorldSpace = true;
			terminatorRenderer.startColor = Color.Lerp(defaultColor, Color.white, colorPercent);
			terminatorRenderer.endColor = Color.Lerp(defaultColor, Color.white, colorPercent);

			setTerminatorPos();
			onPinMove += setTerminatorPos;
		}

		private void setTerminatorPos()
		{
			terminatorRenderer.SetPositions(this.terminatorOfStar());
		}

		public void toggleStarField()
		{
			switch (starMode)
			{
				case starFieldSelect.off:
					starMode = starFieldSelect.single;
					break;
				default:
					starMode++;
					break;
			}
			StarMode++;
		}

		public void toggleHorizonPlane()
		{
			horizonPlaneObj.SetActive(!horizonPlaneObj.activeSelf);
			horizonStatus = horizonPlaneObj.activeSelf;
		}

		/// <summary>
		/// This is called each frame the earth is tilted
		/// </summary>
		internal void onEarthTilt()
		{
			if (iBehave != null && !iBehave.isGrasped)
			{
				Latlong = latlong;
				if (latRenderer != null)
				{
					latRenderer.SetPositions(this.latAtPoint());
				}
				if (longRenderer != null)
				{
					longRenderer.SetPositions(this.longAtPoint());
				}
				if (terminatorRenderer != null)
				{
					terminatorRenderer.SetPositions(this.terminatorOfStar());
				}
			}
		}

		private void onAttached()
		{
			transform.localScale *= 0.6f;

			localPanel.SetActive(false);
		}

		private void onDetached()
		{
			transform.localScale /= 0.6f;
		}

		private void beginGrasp()
		{
			//RSDESManager.ins.PinnedPoints.RemoveAll(p => p.pin == this);
		}

		private void snapToSurface()
		{
			if (onSurface)
				Latlong = (pinTip.transform.position - RSDESManager.earthPos).latlong();
			else
			{
				Latlong = (pinTip.transform.position - RSDESManager.earthPos).latlong();
				enableLocalPanel();
			}
		}

		private void hoverUpdate()
		{
			float glow = iBehave.closestHoveringControllerDistance.Map(0F, 0.2F, 1F, 0.0F);
			pinHead.material.color = Color.Lerp(defaultColor, hoverColor, glow);
		}

		public void toggleSelection()
		{
			if (onSurface)
			{
				if (RSDESManager.ins.SelectedPoints.Any(p => p.pin == this))            //deselect
				{
					if (RSDESManager.verboseLogging)
					{
						Debug.Log(name + " has been deselected!");
					}
					selectState = !selectState;
					var haloComponent = pinHead.gameObject.GetComponent("Halo");
					var haloEnabledProperty = haloComponent.GetType().GetProperty("enabled");
					haloEnabledProperty.SetValue(haloComponent, selectState, null);
					RSDESManager.ins.SelectedPoints.RemoveAll(p => p.pin == this);
				}
				else                                                                                //select
				{
					if (RSDESManager.verboseLogging)
					{
						Debug.Log(name + " has been selected!");
					}
					selectState = !selectState;

					RSDESManager.ins.SelectedPoints.Add(RSDESManager.ins.PinnedPoints.Find(p => p.pin == this));
					var haloComponent = pinHead.gameObject.GetComponent("Halo");
					var haloEnabledProperty = haloComponent.GetType().GetProperty("enabled");
					haloEnabledProperty.SetValue(haloComponent, selectState, null);
				}
			}
			else
			{
				Debug.Log("IM NOT ON THE SURFACE");
			}
		}

		#endregion Behaviour Functions

		#region Setup Functions

		private void setLatPos()
		{
			latRenderer.SetPositions(this.latAtPoint());
		}

		private void setAzimuthPos()
		{
			azimuthRenderer.SetPosition(0, GeoPlanetMaths.directionFromLatLong(0f, this.latlong.y).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos));
			azimuthRenderer.SetPosition(1, RSDESManager.earthPos);
			azimuthRenderer.SetPosition(2, GeoPlanetMaths.directionFromLatLong(0f, 0f).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos));
		}

		private void setAltitudePos()
		{
			altitudeRenderer.SetPosition(0, GeoPlanetMaths.directionFromLatLong(0f, this.latlong.y).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos));
			altitudeRenderer.SetPosition(1, RSDESManager.earthPos);
			altitudeRenderer.SetPosition(2, GeoPlanetMaths.directionFromLatLong(this.latlong).ScaleMultiplier(RSDESManager.radiusOfLargerSphere).Translate(RSDESManager.earthPos));
		}

		internal void setupSun()
		{
			myPintype = pintype.Sun;
			star = RSDESManager.ins.Sun;
			RSDESManager.sunPin = this;
			setupPin();

			star.GetComponent<MeshRenderer>().material = RSDESManager.ins.SunMaterial;
		}

		internal void setupMoon()
		{
			myPintype = pintype.Moon;
			star = RSDESManager.ins.Moon;
			RSDESManager.moonPin = this;
			setupPin();

			star.GetComponent<MeshRenderer>().material = RSDESManager.ins.MoonMaterial;
		}

		internal void setupNorthPole()
		{
			myPintype = pintype.northPole;
			star = null;
			RSDESManager.northPolePin = this;
			Latlong = new Vector2(90, 0);
			setupPin();

			//this.transform.position = GeoPlanetMaths.directionFromLatLong(latlong) * RSDESManager.EarthRadius + RSDESManager.earthPos;
			//this.transform.rotation = Quaternion.FromToRotation(Vector3.down, GeoPlanetMaths.directionFromLatLong(latlong).normalized);
		}

		internal void setupSouthPole()
		{
			myPintype = pintype.southPole;
			star = null;
			RSDESManager.southPolePin = this;
			Latlong = new Vector2(-90, 0);
			setupPin();

			//this.transform.position = GeoPlanetMaths.directionFromLatLong(latlong) * RSDESManager.EarthRadius + RSDESManager.earthPos;
			//this.transform.rotation = Quaternion.FromToRotation(Vector3.down, GeoPlanetMaths.directionFromLatLong(latlong).normalized);
		}

		#endregion Setup Functions

		#region Editor Functions

		[ContextMenu("Show If in pinnedpoints")]
		public void showPinned()
		{
			Debug.Log("****");
			Debug.Log(name);
			Debug.Log("Is found in pins list: " + RSDESManager.ins.PinnedPoints.Any(p => p.pin == this));
			Debug.Log("Is pin data found: " + (RSDESManager.ins.pinDB[this]));
			Debug.Log("Is pin valid: " + RSDESManager.ins.pinDB[this].pin.name);
			Debug.Log("****");
		}

		[ContextMenu("Show If Selected")]
		public void showSelectionStatus()
		{
			Debug.Log("****");
			Debug.Log(name);
			Debug.Log("Is intended to be selected: " + (selectState));
			Debug.Log("Is found in selected pins list: " + RSDESManager.ins.SelectedPoints.Any(p => p.pin == this));
			Debug.Log("Success: " + ((selectState) == RSDESManager.ins.SelectedPoints.Any(p => p.pin == this)));
			Debug.Log("****");
		}

		[ContextMenu("Select This Pin")]
		public void selectPin()
		{
			toggleSelection();
		}

		[ContextMenu("StarRay")]
		public void starRayTest()
		{
			toggleStarField();
		}

		#endregion Editor Functions
	}
}