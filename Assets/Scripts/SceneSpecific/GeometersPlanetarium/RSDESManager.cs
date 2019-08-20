using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Leap.Unity.LeapPaint_v3;
using TMPro;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
    #region Structs

    [Serializable]
    public struct pinData
    {
        public RSDESPin pin;
        public Vector3 contactPoint;

        public Vector2 latLong
        {
            get => pin.Latlong;
            set => pin.Latlong = value;
        }

        public pinData(RSDESPin pin, Vector3 contactPoint)
        {
            RSDESManager.onEarthTilt += pin.onEarthTilt;
            this.pin = pin;
            this.contactPoint = contactPoint;
            latLong = GeoPlanetMaths.latlong(contactPoint, RSDESManager.earthPos);
            //pin.Latlong = this.latLong;
        }
    }

    #endregion Structs

    /// <summary>
    ///     Handles interactions between pins and the Room-scale dynamic earth.
    ///     Handles instation of  arcs and circles
    ///     needs to be integrated into the kernel.
    ///     The main contributor(s) to this script is NG
    ///     Status: WORKING
    /// </summary>
    public class RSDESManager : MonoBehaviour
    {
        private readonly float realEarthtilt = 23.44f;
        private bool activeMenus;
        public bool sunBetweenTropics = true;

        #region Variables

        public static RSDESManager ins;
        internal static bool verboseLogging = false;
        public List<pinData> pinnedPoints = new List<pinData>();

        private List<pinData>
            selectedPoints = new List<pinData>();

        internal Dictionary<RSDESPin, pinData> pinDB =
            new Dictionary<RSDESPin, pinData>();

        public List<pinData> PinnedPoints
        {
            get => pinnedPoints;

            set => pinnedPoints = value;
        }

        public List<pinData> SelectedPoints
        {
            get
            {
                selectedPoints.RemoveAll(p => p.pin == null);
                return selectedPoints;
            }

            set
            {
                if (verboseLogging)
                    Debug.Log(value + " is being selected");
                selectedPoints = value;

                ////this should also change visibility of buttons.
                //if (selectedPoints.Count == 2)
                //{
                //	drawCircle.ignoreContact = false;
                //	showGreatArc.ignoreContact = false;
                //	findDistance.ignoreContact = false;
                //}
                //else if (selectedPoints.Count >= 3)
                //{
                //	drawCircle.ignoreContact = false;
                //	showGreatArc.ignoreContact = false;
                //	findDistance.ignoreContact = true;
                //}
                //else
                //{
                //	drawCircle.ignoreContact = true;
                //	showGreatArc.ignoreContact = true;
                //	findDistance.ignoreContact = true;
                //}
                //drawCircle.GetComponent<Renderer>().enabled = drawCircle.ignoreContact;
                //showGreatArc.GetComponent<Renderer>().enabled = showGreatArc.ignoreContact;
                //findDistance.GetComponent<Renderer>().enabled = findDistance.ignoreContact;
            }
        }

        public Action updateStarFieldsGlobal;

        public void callUpdateStarFieldsGlobal()
        {
            if (updateStarFieldsGlobal != null && updateStarFieldsGlobal.Method != null)
                updateStarFieldsGlobal.Invoke();
        }

        internal static int LR_Resolution = 300;
        internal static float LR_width = .005f;

        private static float earthRadius;

        public static float EarthRadius
        {
            get => earthRadius;

            set
            {
                earthRadius = value;
                if (onEarthTilt != null && onEarthTilt.Method != null)
                    onEarthTilt.Invoke();
            }
        }

        private static float simulationScale;

        public static float SimulationScale
        {
            get => simulationScale;

            set
            {
                if (simulationScale != float.NaN && simulationScale != 0)
                {
                    EarthRadius *= value / simulationScale;
                    ins.transform.localScale *= value / simulationScale;
                }

                simulationScale = value;
            }
        }

        internal void toggleNightSky()
        {
            starParent.gameObject.SetActive(!starParent.gameObject.activeSelf);
        }

        public static Vector3 earthPos;
        public static Quaternion earthRot;
        internal static Action onEarthTilt;
        private DateTime simulationTime;

        public static RSDESPin sunPin;
        public static RSDESPin moonPin;
        public static RSDESPin northPolePin;
        public static RSDESPin southPolePin;

        internal static float sunDist = 146000000000; //meters 150,000,000,000
        internal static float moonDist = 384472282; //meters 384,000,000
        internal static float earthTrueRadius = 6371393; //meters 6,371,393

        public Light globalLight;

        public Transform Sun;
        public Material SunMaterial;
        public Transform Moon;
        public Material MoonMaterial;

        public Transform RSDESStar;
        
        private bool gLat;

        public bool GlobalLatitude
        {
            set
            {
                bool lat = myLatMode == latitudeMode.both || myLatMode == latitudeMode.incremental;
                bool specialLat = myLatMode == latitudeMode.both || myLatMode == latitudeMode.special;
                bool dec = !(myLatMode == latitudeMode.special);
                bool specialDec = specialLat;
                for (int i = 0; i < latRenderer.Length; i++)
                {
                    if (i < nLatDivisions)
                    {
                        latRenderer[i].enabled = lat && value;
                        decRenderer[i].enabled = dec && value;
                    }
                    else
                    {
                        latRenderer[i].enabled = specialLat && value;
                        decRenderer[i].enabled = specialDec && value;
                    }

                    gLat = value;
                }
            }
            get => gLat;
            
        }

        private bool gLong;
        public bool GlobalLongitude{
        		set{
				bool lng = myLatMode == latitudeMode.both || myLatMode == latitudeMode.incremental;
				bool gha = myLatMode == latitudeMode.both || myLatMode == latitudeMode.incremental;
        		    bool sLong = myLatMode == latitudeMode.both || myLatMode == latitudeMode.special;
            		bool sGHA = myLatMode == latitudeMode.both || myLatMode == latitudeMode.special;
        		for (int i = 0; i < longRenderer.Length; i++){
				if (i < nLongDivisions){
					longRenderer[i].enabled = lng && value;
					ghaRenderer[i].enabled = gha && value;
				}else{
					longRenderer[i].enabled = sLong && value;
					ghaRenderer[i].enabled = sGHA && value;
				}
        		}
        		gLong = value;
        		}
        		get => gLong;
        }

        //public leapButtonToggleExtension showSun;
        //public leapButtonToggleExtension showMoon;

        //public InteractionSlider timeSlider;
        //public TMPro.TextMeshPro timeText;
        //public InteractionSlider dateSlider;
        //public TMPro.TextMeshPro dateText;
        [Range(-2f, 5f)] public float timeScale = 1f;

        public TextMeshPro timeDateGlobalMenuText;

        internal DateTime SimulationTime
        {
            get => simulationTime;

            set
            {
                simulationTime = value;
                Horizons.ins.generateData(value);
                if (timeDateGlobalMenuText != null)
                    timeDateGlobalMenuText.SetText(value.ToString("yyyy/MM/dd HH:mm"));

                var ScaleFactor = earthTrueRadius / EarthRadius;
            }
        }

        private void SetSunMoonPositions()
        {
            var EarthOffset = Vector3.zero;
            if (Horizons.planetsHaveValues) EarthOffset = Horizons.Planets.Find(p => p.id == 399).position;
            if (Sun != null && sunPin != null && Horizons.planetsHaveValues)
            {
                if (sunBetweenTropics)
                {
                    var tmp = (Horizons.Planets.Find(p => p.id == 10).position - EarthOffset).latlong();
                    tmp.x = tmp.x / realEarthtilt * earthTiltDeg;
                    sunPin.Latlong = tmp;
                }
                else
                {
                    sunPin.Latlong = (Horizons.Planets.Find(p => p.id == 10).position - EarthOffset).latlong();
                }
            }
            else
            {
                Debug.LogWarning("Sun, " + Sun + " ,or Sunpin, " + sunPin + " ,are not set.");
                RSDESPin.Constructor().GetComponent<RSDESPin>().setupSun();
            }

            if (Moon != null && moonPin != null && Horizons.planetsHaveValues)
            {
                moonPin.Latlong = (Horizons.Planets.Find(p => p.id == 301).position - EarthOffset).latlong();
            }
            else
            {
                Debug.LogWarning("moon, " + Moon + " ,or moonpin, " + moonPin + " ,are not set.");
                RSDESPin.Constructor().GetComponent<RSDESPin>().setupMoon();
            }
        }

        //public leapButtonToggleExtension allowTiltButton;
        public CapsuleCollider axisCollider;
        public MeshRenderer axisRenderer;

        //public leapButtonToggleExtension showLatitude;
        //public leapButtonToggleExtension showLongitude;
        //public leapButtonToggleExtension showGHA;
        //public leapButtonToggleExtension showDeclination;
        //public leapButtonToggleExtension showGreatArc;
        //public leapButtonToggleExtension showEarth;
        //public leapButtonToggleExtension showLights;
        public PressableSlider timeScaleSlider;
        public TextMeshPro timeScaleDisplay;

        private LineRenderer[] latRenderer;
        private LineRenderer[] longRenderer;
        private LineRenderer[] ghaRenderer;
        private LineRenderer[] decRenderer;
        private LineRenderer poleRenderer;

        private readonly float[] _specialLat = {0f, 0f, 0f, 0f, 0f};

        private float earthTiltDeg;
        private int _spLatCount;
        internal int specialLatitudeCount = 5;

        private readonly int nLongDivisions = 18; //18 divisions means minor lines 10 deg, major lines 20 deg.
        private readonly int nLatDivisions = 18; //10 deg lines.
        private static readonly float celestialSphereMinRadius = 50f;

        internal static float radiusOfLargerSphere =>
            Mathf.Max(celestialSphereMinRadius * EarthRadius, celestialSphereMinRadius);

        //public leapButtonToggleExtension drawCircle;

        private readonly Dictionary<List<pinData>,
                LineRenderer>
            greatArcsLRs =
                new Dictionary<List<pinData>,
                    LineRenderer>();

        //public leapButtonToggleExtension findDistance;

        private List<RSDESLineData> arcLineData =
            new List<RSDESLineData>();

        #endregion Variables

        #region Monobehaviour Functions

        private IEnumerator clock;

        private IEnumerator clockUpdate()
        {
            while (true)
            {
                SimulationTime = DateTime.UtcNow;
                yield return new WaitForSecondsRealtime(60f);
            }
        }

        private void Start()
        {
            spawnNightSky();
            //set the viewable distance to be big.
            FindObjectOfType<LeapXRServiceProvider>().GetComponent<Camera>().farClipPlane =
                Mathf.Pow(10, 17);
            //timeScaleSlider.OnSliderValueChanged.AddListener(setTimeScale);
            clock = clockUpdate();
            StartCoroutine(clock);
            Horizons.planetsDataUpdated += SetSunMoonPositions;
        }

        private void Awake()
        {
            ins = this;
            EarthRadius = 1f;
            //initialize system scales to agree.
            SimulationTime = DateTime.UtcNow;
            simulationScale = earthRadius / earthTrueRadius;
//			worldScaleModifier.ins.AbsoluteScale = simulationScale;

            earthPos = transform.position;
            //Debug.Log(earthRot.eulerAngles);

            if (sunPin == null) RSDESPin.Constructor().setupSun();
            if (moonPin == null) RSDESPin.Constructor().setupMoon();

            if (northPolePin == null) RSDESPin.Constructor().setupNorthPole();
            if (southPolePin == null) RSDESPin.Constructor().setupSouthPole();

            onEarthTilt += updateLatLongLines;
            onEarthTilt += SetSunMoonPositions;
            onEarthTilt += updateNightSky;
            onEarthTilt += updatePoleLine;

            //resetEarthTilt();
        }

        #endregion Monobehaviour Functions+

        #region Behaviour Functions

        /// <summary>
        ///     Calls any function subscribed to onEarthTilt
        /// </summary>
        internal void earthTilt()
        {
            transform.rotation = earthRot;

            earthTiltDeg = Vector3.Angle(Vector3.up, transform.up);
            _specialLat[0] = 90 - earthTiltDeg; //arctic
            _specialLat[1] = -90 + earthTiltDeg; //ant arctic
            _specialLat[2] = 0 - earthTiltDeg; //tropic of cancer
            _specialLat[3] = 0 + earthTiltDeg; //tropic of capricorn

            if (onEarthTilt != null && onEarthTilt.Method != null) onEarthTilt.Invoke();
        }

        internal void setTime()
        {
            //float time = timeSlider.HorizontalSliderPercent;
            //float date = dateSlider.HorizontalSliderPercent;
            //SimulationTime = new DateTime(2018, Mathf.FloorToInt(date * 12f) % 12, Mathf.FloorToInt(date * 30) % 30,
            //	Mathf.FloorToInt(time * 24) % 24, Mathf.FloorToInt(time * 60) % 60, Mathf.FloorToInt(time * 3600) % 3600);
            //dateText.SetText(SimulationTime.ToShortDateString());
            //timeText.SetText(SimulationTime.ToShortTimeString());
        }

        private Transform starParent;

        private readonly Dictionary<Transform, StellarData.Star> starDataMap =
            new Dictionary<Transform, StellarData.Star>();

        internal void spawnNightSky()
        {
            starParent = Instantiate(new GameObject()).transform;
            starParent.name = "Star Container";
            StellarData.getData(earthPos);
            foreach (var cStar in StellarData.Stars)
                if (cStar.VisualMagnitude <= 6)
                {
                    var Pos =
                        new Vector3(cStar.position.x * 4841427, cStar.position.y * 4841427,
                                cStar.position.z * 4841427).normalized.ScaleMultiplier(radiusOfLargerSphere)
                            .Translate(earthPos);

                    RSDESStar = Instantiate(
                        Resources.Load<GameObject>("Prefabs/RSDES/RSDESStar_CS"), Pos,
                        Quaternion.identity, starParent).transform;

                    starDataMap.Add(RSDESStar, cStar);
                }

            //turn off night sky.
            toggleNightSky();
        }

        internal void updateNightSky()
        {
            foreach (var star in starDataMap.Keys)
                star.position =
                    new Vector3(starDataMap[star].position.x * 4841427,
                            starDataMap[star].position.y * 4841427, starDataMap[star].position.z * 4841427).normalized
                        .ScaleMultiplier(radiusOfLargerSphere).Translate(earthPos);
        }

        #endregion Behaviour Functions

        #region Toggles and Interface Functions

        public void toggleLights()
        {
//			globalLight.gameObject.SetActive(showLights.ToggleState);
        }

        public void toggleEarth()
        {
            //GetComponent<MeshRenderer>().enabled = showEarth.ToggleState;
        }

        public void resetEarthTilt()
        {
            //northPolePin.overrideNorthPoleLock = true;
            //northPolePin.Latlong = (Quaternion.Euler(23.4f, 0, 0)*Vector3.up).latlong();
            //northPolePin.overrideNorthPoleLock = false;
            earthRot = Quaternion.Euler(23.4f, 0, 0);
            earthTilt();
        }

        public void toggleSun()
        {
            Sun.GetComponent<Renderer>().enabled = !Sun.GetComponent<Renderer>().enabled;
            Sun.GetComponent<Light>().enabled = !Sun.GetComponent<Light>().enabled;
        }

        public void toggleMoon()
        {
            Moon.GetComponent<Renderer>().enabled = !Moon.GetComponent<Renderer>().enabled;
        }

        public void switchLat()
        {
            switch (myLatMode)
            {
                case latitudeMode.off:
                    myLatMode = latitudeMode.incremental;
                    break;
                default:
                    myLatMode++;
                    break;
            }

            updateLatLongLines();
        }

        public enum latitudeMode
        {
            incremental,
            special,
            both,
            off
        }

        //this needs to be incremented by a button.
        public latitudeMode myLatMode = latitudeMode.off;

        public Color articRenderer;
        public Color tropicsRenderer;
        public Color equatorRenderer;

        public void updateLatLongLines()
        {
            if (latRenderer == null)
            {
                latRenderer = new LineRenderer[nLatDivisions + specialLatitudeCount];
                for (var i = 0; i < nLatDivisions + specialLatitudeCount; i++)
                {
                    latRenderer[i] = RSDESGeneratedLine();
                    latRenderer[i].positionCount = LR_Resolution;
                    if (i % 2 == 0 && i < nLatDivisions)
                    {
                        latRenderer[i].startWidth = LR_width * 3f;
                        latRenderer[i].endWidth = LR_width * 3f;
                    }
                    else
                    {
                        latRenderer[i].startWidth = LR_width;
                        latRenderer[i].endWidth = LR_width;
                    }

                    latRenderer[i].useWorldSpace = true;
                }
            }

            if (decRenderer == null)
            {
                decRenderer = new LineRenderer[nLatDivisions + specialLatitudeCount];
                for (var i = 0; i < nLatDivisions + specialLatitudeCount; i++)
                {
                    decRenderer[i] = RSDESGeneratedLine();
                    decRenderer[i].positionCount = LR_Resolution;
                    if (i % 2 == 0 && i < nLatDivisions)
                    {
                        decRenderer[i].startWidth = LR_width * 3f;
                        decRenderer[i].endWidth = LR_width * 3f;
                    }
                    else
                    {
                        decRenderer[i].startWidth = LR_width;
                        decRenderer[i].endWidth = LR_width;
                    }

                    decRenderer[i].useWorldSpace = true;
                }
            }

            //need to add subdivided grid.
            if (longRenderer == null)
            {
                longRenderer = new LineRenderer[nLongDivisions];
                for (var i = 0; i < nLongDivisions; i++)
                {
                    {
                        longRenderer[i] = RSDESGeneratedLine();
                    }
                    longRenderer[i].positionCount = LR_Resolution;
                    if (i % 2 == 0)
                    {
                        longRenderer[i].startWidth = LR_width * 3f;
                        longRenderer[i].endWidth = LR_width * 3f;
                    }
                    else
                    {
                        longRenderer[i].startWidth = LR_width;
                        longRenderer[i].endWidth = LR_width;
                    }

                    longRenderer[i].useWorldSpace = true;
                }
            }

            if (ghaRenderer == null)
            {
                ghaRenderer = new LineRenderer[nLongDivisions];
                for (var i = 0; i < nLongDivisions; i++)
                {
                    ghaRenderer[i] = RSDESGeneratedLine();
                    ghaRenderer[i].positionCount = LR_Resolution;
                    if (i % 2 == 0)
                    {
                        ghaRenderer[i].startWidth = LR_width * 3f;
                        ghaRenderer[i].endWidth = LR_width * 3f;
                    }
                    else
                    {
                        ghaRenderer[i].startWidth = LR_width;
                        ghaRenderer[i].endWidth = LR_width;
                    }

                    ghaRenderer[i].useWorldSpace = true;
                }
            }

            var lat = myLatMode == latitudeMode.both || myLatMode == latitudeMode.incremental;
            var specialLat = myLatMode == latitudeMode.both || myLatMode == latitudeMode.special;
            var dec = !(myLatMode == latitudeMode.special);
            var specialDec = specialLat;
            for (var i = 0; i < nLatDivisions + specialLatitudeCount; i++)
            {
                if (lat || dec || specialLat)
                {
                    var theta = i * 180f / (nLatDivisions - 1) - 90f;
                    if (i >= nLatDivisions) theta = _specialLat[i - nLatDivisions];
                    var positions = GeoPlanetMaths
                        .latAtPoint(GeoPlanetMaths.directionFromLatLong(theta, 0) + earthPos, LR_Resolution)
                        .Translate(-earthPos).ScaleMultiplier(EarthRadius).Translate(earthPos);
                    latRenderer[i].SetPositions(positions);
                    decRenderer[i].SetPositions(positions.Translate(-earthPos)
                        .ScaleMultiplier(radiusOfLargerSphere / EarthRadius).Translate(earthPos));
                }

                if (i < nLatDivisions)
                {
                    latRenderer[i].enabled = lat;
                    decRenderer[i].enabled = dec;
                }
                else
                {
                    latRenderer[i].enabled = specialLat;
                    decRenderer[i].enabled = specialDec;
                }

                //arctics
                latRenderer[nLatDivisions].startColor = articRenderer;
                latRenderer[nLatDivisions].endColor = articRenderer;
                decRenderer[nLatDivisions].startColor = articRenderer;
                decRenderer[nLatDivisions].endColor = articRenderer;
                latRenderer[nLatDivisions + 1].startColor = articRenderer;
                latRenderer[nLatDivisions + 1].endColor = articRenderer;
                decRenderer[nLatDivisions + 1].startColor = articRenderer;
                decRenderer[nLatDivisions + 1].endColor = articRenderer;
                //tropics
                latRenderer[nLatDivisions + 2].startColor = tropicsRenderer;
                latRenderer[nLatDivisions + 2].endColor = tropicsRenderer;
                decRenderer[nLatDivisions + 2].startColor = tropicsRenderer;
                decRenderer[nLatDivisions + 2].endColor = tropicsRenderer;
                latRenderer[nLatDivisions + 3].startColor = tropicsRenderer;
                latRenderer[nLatDivisions + 3].endColor = tropicsRenderer;
                decRenderer[nLatDivisions + 3].startColor = tropicsRenderer;
                decRenderer[nLatDivisions + 3].endColor = tropicsRenderer;
            }

            var sLong = true;
            var sGHA = true;
            for (var i = 0; i < nLongDivisions; i++)
            {
                if (sLong || sGHA)
                {
                    //we only need to consider positive longitude since they are great circles.				
                    var positions = GeoPlanetMaths.longAtPoint(
                        GeoPlanetMaths.directionFromLatLong(new Vector2(0, 180f / nLongDivisions * i)) +
                        earthPos, LR_Resolution);
                    longRenderer[i].SetPositions(positions);
                    ghaRenderer[i].SetPositions(positions.Translate(-earthPos)
                        .ScaleMultiplier(radiusOfLargerSphere / EarthRadius).Translate(earthPos));
                }

                longRenderer[i].enabled = sLong;
                ghaRenderer[i].enabled = sGHA;
            }
        }

        public void updatePoleLine()
        {
            if (poleRenderer == null)
            {
                poleRenderer = RSDESGeneratedLine();
            }
            else
            {
                var positions = new Vector3[2];
                positions[0] = northPolePin.pinTip.transform.position;
                positions[1] = southPolePin.pinTip.transform.position;
                //polesExist.Add(northPolePin.gameObject.name + southPolePin.gameObject.name);
                //LineRenderer poleRenderer = RSDESGeneratedLine();
                //poleRenderer.positionCount = positions.Length;
                poleRenderer.SetPositions(positions);
                //poleRenderer.SetPositions(test[northPolePin.pinTip.transform.position, southPolePin.pinTip.transform.position]);
            }
        }

        private readonly List<string> circlesExist =
            new List<string>();

        public void toggleCircles()
        {
            if (SelectedPoints.Count >= 2)
                for (var i = 0; i < selectedPoints.Count - 1; i++)
                for (var j = i + 1; j < selectedPoints.Count; j++)
                    instantiateGreatCircle(selectedPoints[i], selectedPoints[j]);
        }

        public void instantiateGreatCircle(pinData pinA, pinData pinB)
        {
            if (!circlesExist.Contains(pinA.pin.name + pinB.pin.name))
            {
                circlesExist.Add(pinA.pin.name + pinB.pin.name);
                var newLR = RSDESGeneratedLine();
                newLR.GetComponent<RSDESLineData>().associatedPins =
                    new List<pinData> {pinA, pinB};
                newLR.GetComponent<RSDESLineData>().LineType = lineType.circle;
                newLR.startWidth = LR_width;
                newLR.endWidth = LR_width;
                newLR.positionCount = LR_Resolution;
                newLR.SetPositions(GeoPlanetMaths.greatCircleCoordinates(pinA.pin.transform.position,
                    pinB.pin.transform.position, LR_Resolution));
                if (verboseLogging) Debug.Log(newLR.name + " was created for the two points.");
                newLR.loop = true;
            }
        }

        private readonly List<string> greatArcsExist =
            new List<string>();

        public void toggleGreatArcs()
        {
            {
                if (SelectedPoints.Count >= 2)
                    for (var i = 0; i < selectedPoints.Count - 1; i++)
                    for (var j = i + 1; j < selectedPoints.Count; j++)
                        instantiateGreatArc(selectedPoints[i], selectedPoints[j]);
            }
        }

        private LineRenderer RSDESGeneratedLine()
        {
            return Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/RSDESGeneratedLine"))
                .GetComponent<LineRenderer>();
        }

        public void instantiateGreatArc(pinData pinA, pinData pinB)
        {
            if (!greatArcsExist.Contains(pinA.pin.name + pinB.pin.name))
            {
                greatArcsExist.Add(pinA.pin.name + pinB.pin.name);
                var newLR = RSDESGeneratedLine();
                newLR.GetComponent<RSDESLineData>().associatedPins =
                    new List<pinData> {pinA, pinB};
                newLR.GetComponent<RSDESLineData>().LineType = lineType.arc;
                greatArcsLRs.Add(new List<pinData> {pinA, pinB}, newLR);
                newLR.startWidth = LR_width;
                newLR.endWidth = LR_width;
                newLR.positionCount = LR_Resolution;
                newLR.SetPositions(GeoPlanetMaths.greatArcCoordinates(pinA.pin.transform.position,
                    pinB.pin.transform.position, LR_Resolution));
                newLR.loop = false;
            }
        }

        public void toggleDistance()
        {
            if (verboseLogging)
                Debug.Log("DISTANCE toggled");
            //if (arcLineData.Count == 0)
            //	Debug.Log("no arc line data found.");
            //arcLineData.ForEach(a => a.isDistTextEnabled = findDistance.ToggleState);

//			GameObject.FindObjectsOfType<RSDESLineData>().ToList().ForEach(a => a.isDistTextEnabled = findDistance.ToggleState);
//			GameObject.FindObjectsOfType<RSDESPin>().ToList().ForEach(a => a.latlongLabel.gameObject.SetActive(findDistance.ToggleState));
        }

        public void setTimeScale(float value)
        {
            timeScale = value;
            timeScaleDisplay.text = value.ToString();
        }

        #endregion Toggles and Interface Functions
    }
}
