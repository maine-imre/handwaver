RSDESManager.cs is the file which holds functionality for the Geometer's Planetarium scene.
- toggleLights  
globalLight.gameObject.SetActive(showLights.ToggleState);

- toggleEarth  
GetComponent<MeshRenderer>().enabled = showEarth.ToggleState;

- resetEarthTilt  
earthRot = Quaternion.Euler(23.4f, 0, 0);  
earthTilt();

- toggleSun  
            Sun.GetComponent<Renderer>().enabled = !Sun.GetComponent<Renderer>().enabled;
            Sun.GetComponent<Light>().enabled = !Sun.GetComponent<Light>().enabled;
- toggleMoon  
Moon.GetComponent<Renderer>().enabled = !Moon.GetComponent<Renderer>().enabled;

- switchLat
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
       
- toggleCircles  
            if (SelectedPoints.Count >= 2)
                for (var i = 0; i < selectedPoints.Count - 1; i++)
                for (var j = i + 1; j < selectedPoints.Count; j++)
                    instantiateGreatCircle(selectedPoints[i], selectedPoints[j]);

- instantiateGreatCircle(pinData pinA, pinData pinB)
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

- toggleGreatArcs  
                if (SelectedPoints.Count >= 2)
                    for (var i = 0; i < selectedPoints.Count - 1; i++)
                    for (var j = i + 1; j < selectedPoints.Count; j++)
                        instantiateGreatArc(selectedPoints[i], selectedPoints[j]);
- instantiateGreatArc(pinData pinA, pinData pinB)

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

- toggleDistance  
            if (verboseLogging)
                Debug.Log("DISTANCE toggled");

- setTimeScale(float value)  
            timeScale = value;
            timeScaleDisplay.text = value.ToString();
