namespace IMRE.HandWaver.Space
{
    /// <summary>
    ///     Includes all nececssary calculations for the Geometer's Planetarium scene.
    ///     Needs to be integrated into the kernel.
    ///     Functions include moving to and from local coordinate system.
    ///     Functions include moving between spherical and cartesian cordinate systems
    /// </summary>
    public static class GeoPlanetMaths
    {
        internal static UnityEngine.Vector3[] ScaleMultiplier(this UnityEngine.Vector3[] vec, float multiplier)
        {
            for (int i = 0; i < vec.Length; i++) vec[i] = multiplier * vec[i];
            return vec;
        }

        internal static UnityEngine.Vector3[] Translate(this UnityEngine.Vector3[] vec, UnityEngine.Vector3 translation)
        {
            for (int i = 0; i < vec.Length; i++) vec[i] += translation;
            return vec;
        }

        internal static UnityEngine.Vector3 ScaleMultiplier(this UnityEngine.Vector3 vec, float multiplier)
        {
            vec *= multiplier;
            return vec;
        }

        internal static UnityEngine.Vector3 Translate(this UnityEngine.Vector3 vec, UnityEngine.Vector3 translation)
        {
            vec += translation;
            return vec;
        }

        /// <summary>
        ///     Returns the degree min second representation of the latitude or longitude
        /// </summary>
        /// <param name="f">the signed value of the lat or long</param>
        /// <param name="latitude">boolina.  lat = true, long = false</param>
        /// <returns></returns>
        internal static string dmsFromFloat(float f, bool latitude)
        {
            string append = " ";
            if (f == 0)
                append = " ";
            else if (latitude && f > 0)
                append = "N";
            else if (latitude)
                append = "S";
            else if (f > 0)
                append = "E";
            else
                append = "W";
            return dmsFromFloat(f) + append;
        }

        internal static string dmsFromFloat(float f)
        {
            int sec = (int) UnityEngine.Mathf.Round(UnityEngine.Mathf.Abs(f * 3600));
            int deg = sec / 3600;
            sec %= 3600;
            int min = sec / 60;
            //sec %= 60;
            decimal SEC = (decimal) (UnityEngine.Mathf.Abs(f * 3600) - deg * 3600 - min * 60);
            return deg + "° " + min + "' " + System.Math.Round(SEC, 3) + "'' ";
        }

        #region Find Latitude and Longitude

        /// <summary>
        ///     given coordinates in world space, and the earth's center, find the lattidue and longitude of the point.
        /// </summary>
        /// <param name="pointOnSurface"></param>
        /// <param name="earthCenter"></param>
        /// <returns></returns>
        public static UnityEngine.Vector2 latlong(UnityEngine.Vector3 pointOnSurface, UnityEngine.Vector3 earthCenter)
        {
            if (RSDESManager.verboseLogging)
                UnityEngine.Debug.Log("point on surface selected is " + pointOnSurface + "\nEarth center is at " +
                                      earthCenter);

            return latlong(pointOnSurface - earthCenter);
        }

        /// <summary>
        ///     Given coordiates local to earth, find the latitude and longitude of the point.
        /// </summary>
        /// <param name="pointLocalToEarthCenter"></param>
        /// <returns></returns>
        public static UnityEngine.Vector2 latlong(this UnityEngine.Vector3 pointLocalToEarthCenter)
        {
            pointLocalToEarthCenter = UnityEngine.Quaternion.Inverse(RSDESManager.earthRot) *
                                      pointLocalToEarthCenter.normalized;

            //define the prime meridian to be in the Vector3.right direction.
            //define the north pole to be in the Vector3.up direction.
            //define Northern latitude to be postive, and Southern latitude to be negative.

            //in Unity 2018.1, Vector3.angle's rangle is 180 not 90.

            float latitude = 90 - UnityEngine.Vector3.Angle(UnityEngine.Vector3.up, pointLocalToEarthCenter);

            float longitude = 0f;
            if (UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, pointLocalToEarthCenter).magnitude == 0)
                longitude = 0f;
            else
                longitude = -UnityEngine.Vector3.SignedAngle(UnityEngine.Vector3.right,
                    UnityEngine.Vector3.ProjectOnPlane(pointLocalToEarthCenter, UnityEngine.Vector3.up).normalized,
                    UnityEngine.Vector3.up);

            return new UnityEngine.Vector2(latitude, longitude);
        }

        /// <summary>
        ///     A function for the differences of latitude and longitude that takes into account the cuts made in the argument
        ///     branch
        /// </summary>
        /// <param name="latlong1"></param>
        /// <param name="latlong2"></param>
        /// <returns></returns>
        public static UnityEngine.Vector2 angleDifference(UnityEngine.Vector2 latlong1, UnityEngine.Vector2 latlong2)
        {
            return new UnityEngine.Vector2(angleDifference(latlong1.x, latlong2.x),
                angleDifference(latlong1.y, latlong2.y));
        }

        /// <summary>
        ///     A function for the differences of angles that takes into account the cuts made in the argument branch
        ///     this returns a signed angle.
        /// </summary>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        /// <returns></returns>
        public static float angleDifference(float angle1, float angle2)
        {
            //this signed angle difference accoutns for a cut at 180/-180.
            if (angle1 - angle2 < 180)
                return angle1 - angle2;
            if (angle1 > angle2)
                return (angle1 - angle2) % 180;
            return -((angle1 - angle2) % 180);
        }

        public static UnityEngine.Vector3 directionFromLatLong(this RSDESPin pin)
        {
            if (RSDESManager.verboseLogging && pin == null)
                UnityEngine.Debug.Log("dead pin", pin);
            return directionFromLatLong(pin.Latlong);
        }

        public static UnityEngine.Vector3 directionFromLatLong(UnityEngine.Vector2 latlong)
        {
            float latitude = latlong.x;
            float longitude = latlong.y;
            UnityEngine.Vector3 tmp = UnityEngine.Quaternion.AngleAxis(-longitude, UnityEngine.Vector3.up) *
                                      UnityEngine.Vector3.right;
            UnityEngine.Vector3 axis = UnityEngine.Vector3.Cross(tmp, UnityEngine.Vector3.up);
            UnityEngine.Vector3 absPos = UnityEngine.Quaternion.AngleAxis(latitude, axis) * tmp;
            return (RSDESManager.earthRot * absPos).normalized;
        }

        public static UnityEngine.Vector3 directionFromLatLong(float latitude, float longitude)
        {
            return directionFromLatLong(new UnityEngine.Vector2(latitude, longitude));
        }

        #endregion

        #region Define verticies of circles and arcs

        public static UnityEngine.Vector3[] longAtPoint(this RSDESPin pin)
        {
            return longAtPoint(directionFromLatLong(pin.Latlong) + RSDESManager.earthPos, RSDESManager.LR_Resolution);
        }

        public static UnityEngine.Vector3[] latAtPoint(this RSDESPin pin)
        {
            return latAtPoint(directionFromLatLong(pin.Latlong) * RSDESManager.EarthRadius + RSDESManager.earthPos,
                RSDESManager.LR_Resolution);
        }

        public static UnityEngine.Vector3[] longAtPoint(UnityEngine.Vector3 pointOnSurface, int count)
        {
            return greatCircleCoordinates(pointOnSurface,
                RSDESManager.earthPos + RSDESManager.earthRot * (RSDESManager.EarthRadius * UnityEngine.Vector3.up),
                count);
        }

        /// <summary>
        ///     Outputs the latitude of a point, assuming that the radius of the larger earth is given by the magnitude of the
        ///     pointOnSurface.
        /// </summary>
        /// <param name="pointOnSurface"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static UnityEngine.Vector3[] latAtPoint(UnityEngine.Vector3 pointOnSurface, int count)
        {
            UnityEngine.Vector3 center = UnityEngine.Vector3.Project(pointOnSurface - RSDESManager.earthPos,
                                             RSDESManager.earthRot * UnityEngine.Vector3.up) + RSDESManager.earthPos;
            UnityEngine.Vector3 normal = RSDESManager.earthRot * UnityEngine.Vector3.up;
            return circleCoordinates(center, (pointOnSurface - center).magnitude, normal, count);
        }

        internal static UnityEngine.Vector3[] starRayRendererCoordiantes(pinData myPinData)
        {
            return starRayRendererCoordiantes(myPinData, myPinData);
        }

        private static UnityEngine.Vector3[] greatCircleCoordinates(UnityEngine.Vector3 normal, int count)
        {
            //we have a plane through the earth's center with normal direction of 
            //make a basis
            UnityEngine.Vector3 basis0 = UnityEngine.Vector3.Cross(normal, UnityEngine.Vector3.right).normalized;
            if (basis0.magnitude == 0)
                basis0 = UnityEngine.Vector3.Cross(normal, UnityEngine.Vector3.forward).normalized;
            UnityEngine.Vector3 basis1 = UnityEngine.Vector3.Cross(normal, UnityEngine.Vector3.up).normalized;
            if (basis1.magnitude == 0)
                basis1 = UnityEngine.Vector3.Cross(normal, UnityEngine.Vector3.forward).normalized;
            return greatCircleCoordinates(basis0 * RSDESManager.EarthRadius + RSDESManager.earthPos,
                basis1 * RSDESManager.EarthRadius + RSDESManager.earthPos, count);
        }

        public static UnityEngine.Vector3[] greatCircleCoordinates(pinData pin1, pinData pin2)
        {
            return circleCoordinates(RSDESManager.earthPos, RSDESManager.EarthRadius,
                UnityEngine.Vector3.Cross(directionFromLatLong(pin1.latLong), directionFromLatLong(pin2.latLong)),
                RSDESManager.LR_Resolution);
        }

        public static UnityEngine.Vector3[] greatCircleCoordinates(UnityEngine.Vector3 pointOnSurface1,
            UnityEngine.Vector3 pointOnSurface2, int count)
        {
            return circleCoordinates(RSDESManager.earthPos, RSDESManager.EarthRadius,
                UnityEngine.Vector3.Cross(pointOnSurface1 - RSDESManager.earthPos,
                    pointOnSurface2 - RSDESManager.earthPos), count);
            //return circleCoordinates(RSDESManager.earthPos, RSDESManager.earthRadius, Vector3.Cross(pointOnSurface1 - RSDESManager.earthPos, pointOnSurface2 - RSDESManager.earthPos),count);
        }

        public static UnityEngine.Vector3[] greatArcCoordinates(pinData pin1, pinData pin2)
        {
            return arcCoordinates(RSDESManager.earthPos, RSDESManager.EarthRadius,
                directionFromLatLong(pin1.latLong).ScaleMultiplier(RSDESManager.EarthRadius)
                    .Translate(RSDESManager.earthPos),
                directionFromLatLong(pin2.latLong).ScaleMultiplier(RSDESManager.EarthRadius)
                    .Translate(RSDESManager.earthPos), RSDESManager.LR_Resolution);
        }

        public static UnityEngine.Vector3[] greatArcCoordinates(UnityEngine.Vector3 pointOnSurface1,
            UnityEngine.Vector3 pointOnSurface2, int count)
        {
            return arcCoordinates(RSDESManager.earthPos, RSDESManager.EarthRadius, pointOnSurface1, pointOnSurface2,
                count);
        }

        public static UnityEngine.Vector3[] circleCoordinates(UnityEngine.Vector3 center, float radius,
            UnityEngine.Vector3 normal, int count)
        {
            return arcCoordinates(center, radius, normal, count, 0, 360);
        }

        public static UnityEngine.Vector3[] arcCoordinates(UnityEngine.Vector3 center, float radius,
            UnityEngine.Vector3 normal, int count, float startAlpha, float endAlpha)
        {
            //use rotations to find a basis.
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.up, normal);
            UnityEngine.Vector3 point1 = rot * UnityEngine.Vector3.right;
            UnityEngine.Vector3 point2 = rot * UnityEngine.Vector3.forward;
            return arcCoordinates(center, radius, radius * point1.normalized + center,
                radius * point2.normalized + center, count, startAlpha, endAlpha);
        }

        public static UnityEngine.Vector3[] arcCoordinates(UnityEngine.Vector3 center, float radius,
            UnityEngine.Vector3 startPointOnCircle, UnityEngine.Vector3 endPointOnCircle, int count)
        {
            if (UnityEngine.Vector3.ProjectOnPlane(startPointOnCircle - endPointOnCircle, startPointOnCircle - center)
                    .magnitude == 0)
                UnityEngine.Debug.LogWarning("Colinear Points Won't determine a basis.");

            UnityEngine.Vector3 basis0 = (startPointOnCircle - center).normalized;
            UnityEngine.Vector3 basis1 = (endPointOnCircle - center).normalized;
            if (UnityEngine.Vector3.Dot(basis1, basis0) != 0)
            {
                if (RSDESManager.verboseLogging)
                    UnityEngine.Debug.LogWarning("Basis isn't orthagonal");
                UnityEngine.Vector3.OrthoNormalize(ref basis0, ref basis1);
            }

            float alpha0 = UnityEngine.Vector3.SignedAngle(basis0, startPointOnCircle - center,
                UnityEngine.Vector3.Cross(basis0, basis1));

            float alpha1 = alpha0 - UnityEngine.Vector3.SignedAngle(endPointOnCircle - center,
                               startPointOnCircle - center, UnityEngine.Vector3.Cross(basis0, basis1));

            return arcCoordinates(center, radius, startPointOnCircle, endPointOnCircle, count, alpha0, alpha1);
        }

        public static UnityEngine.Vector3[] arcCoordinates(UnityEngine.Vector3 center, float radius,
            UnityEngine.Vector3 pointOnCircle1, UnityEngine.Vector3 pointOnCircle2, int count, float startAlpha,
            float endAlpha)
        {
            if (RSDESManager.verboseLogging)
                UnityEngine.Debug.Log("CENTER : " + center);
            if (UnityEngine.Vector3.ProjectOnPlane(pointOnCircle1 - pointOnCircle2, pointOnCircle1 - center)
                    .magnitude == 0)
                UnityEngine.Debug.LogWarning("Colinear Points Won't determine a basis.");

            //note we are working in degrees generally, but Mathf handles radians.
            UnityEngine.Vector3 basis0 = (pointOnCircle1 - center).normalized;
            UnityEngine.Vector3 basis1 = (pointOnCircle2 - center).normalized;
            UnityEngine.Vector3.OrthoNormalize(ref basis0, ref basis1);

            //handle the cut in the argument's branch.
            //(endAlpha - startAlpha)
            float angleDiff = angleDifference(endAlpha, startAlpha);
            if (angleDiff == 0) angleDiff = 360f;

            UnityEngine.Vector3[] result = new UnityEngine.Vector3[count];
            for (int i = 0; i < count; i++)
            {
                float alpha = (startAlpha + i * angleDiff) / count;
                result[i] = radius * (basis0 * UnityEngine.Mathf.Cos(alpha * UnityEngine.Mathf.PI / 180) +
                                      basis1 * UnityEngine.Mathf.Sin(alpha * UnityEngine.Mathf.PI / 180)) + center;
            }

            return result;
        }

        #endregion

        #region Find information at a point

        /// <summary>
        ///     Find the normal direction of a plane tangent to the earth's surface at a point.
        /// </summary>
        /// <param name="pointOnEarthSurface"></param>
        /// <param name="earthPos"></param>
        /// <returns></returns>
        public static UnityEngine.Vector3 normalToTangentPlane(UnityEngine.Vector3 pointOnEarthSurface,
            UnityEngine.Vector3 earthPos)
        {
            return normalToTangentPlane(pointOnEarthSurface - earthPos);
        }

        public static UnityEngine.Vector3 normalToTangentPlane(UnityEngine.Vector3 pointLocalToEarth)
        {
            return pointLocalToEarth.normalized;
        }

        public static UnityEngine.Vector3[] terminatorOfStar(this RSDESPin pin)
        {
            return terminatorOfStar(directionFromLatLong(pin.Latlong), RSDESManager.earthPos,
                RSDESManager.LR_Resolution);
        }

        public static UnityEngine.Vector3[] terminatorOfStar(UnityEngine.Vector3 subPointOfStar,
            UnityEngine.Vector3 earthPos, int count)
        {
            return greatCircleCoordinates(subPointOfStar.normalized, count);
        }

        /// <summary>
        ///     Linear model.  Obslete b/c of @Tim's Horizons scripts
        /// </summary>
        /// <param name="simulationTime"></param>
        /// <returns></returns>
        internal static UnityEngine.Vector3 SunDirection(System.DateTime simulationTime)
        {
            //eventually this will query data base and interpolate.
            //really the question is where is the subpoint.
            float latitude = 23.5f / 180f * (simulationTime.DayOfYear % 180 - 90f);
            if (simulationTime.DayOfYear < 365f / 4f || simulationTime.DayOfYear > 365f * 3f / 4f) latitude *= -1;
            float longitude = 360f / 24f * 60f * 60f * (float) simulationTime.TimeOfDay.TotalSeconds;
            return directionFromLatLong(latitude, longitude);
        }

        /// <summary>
        ///     Linear model.  Obslete b/c of @Tim's Horizons scripts
        /// </summary>
        /// <param name="simulationTime"></param>
        /// <returns></returns>
        internal static UnityEngine.Vector3 MoonDirection(System.DateTime simulationTime)
        {
            //eventually this will query data base and interpolate.
            float longitude = 360f / 24f * 60f * 60f * 27f * (float) simulationTime.TimeOfDay.TotalSeconds +
                              27f * (simulationTime.DayOfYear % 27);

            float latitude = 0f;
            //deal with latitude later;
            return directionFromLatLong(latitude, longitude);
        }

        public static System.DateTime timeOfSimulation(UnityEngine.Vector3 subPointofSun)
        {
            //how to manage?
            UnityEngine.Vector2 coordinates = latlong(subPointofSun);
            int year = 2018;
            int month = 3 + UnityEngine.Mathf.RoundToInt(coordinates.x * (3f / 23.5f));
            month = month % 12;
            int day = 21 + UnityEngine.Mathf.RoundToInt(coordinates.x * (90f / 23.5f) - (month - 3) * 30f);
            while (day > 30)
            {
                month++;
                day -= 30;
            }

            while (day < 0)
            {
                month--;
                day += 30;
            }

            int hour = (int) (coordinates.y * (24 / 360)) % 24;
            int min = (int) (coordinates.y * (24 * 60 / 360)) % (24 * 60);
            int sec = (int) (coordinates.y * (24 * 60 * 60 / 360)) % (24 * 60 * 60);

            return new System.DateTime(year, month, day, hour, min, sec);
        }

        #endregion

        #region Find information between points

        public static float greatCircleDistance(UnityEngine.Vector3 pointLocalEarth1,
            UnityEngine.Vector3 pointLocalEarth2)
        {
            UnityEngine.Vector2 p1 = latlong(pointLocalEarth1);
            UnityEngine.Vector2 p2 = latlong(pointLocalEarth2);

            UnityEngine.Vector2 diff = angleDifference(p1, p2);

            float cosc = UnityEngine.Mathf.Cos(90 - p1.y) * UnityEngine.Mathf.Cos(90 - p2.y) +
                         UnityEngine.Mathf.Sin(90 - p1.y) * UnityEngine.Mathf.Sin(90 - p2.y) *
                         UnityEngine.Mathf.Cos(diff.x);
            float rads = UnityEngine.Mathf.Acos(cosc);

            return rads * RSDESManager.earthTrueRadius;
        }

        /// <summary>
        ///     A function that finds the great arc angle measure between two pins.
        /// </summary>
        /// <param name="pin1"></param>
        /// <param name="pin2"></param>
        /// <returns>Angle in radians</returns>
        public static float greatArcLength(pinData pin1, pinData pin2)
        {
            UnityEngine.Vector2 p1 = pin1.latLong;
            UnityEngine.Vector2 p2 = pin2.latLong;

            UnityEngine.Vector2 diff = angleDifference(p1, p2) * UnityEngine.Mathf.Deg2Rad;

            float cosc =
                UnityEngine.Mathf.Cos((90 - p1.x) * UnityEngine.Mathf.Deg2Rad) *
                UnityEngine.Mathf.Cos((90 - p2.x) * UnityEngine.Mathf.Deg2Rad) +
                UnityEngine.Mathf.Sin((90 - p1.x) * UnityEngine.Mathf.Deg2Rad) *
                UnityEngine.Mathf.Sin((90 - p2.x) * UnityEngine.Mathf.Deg2Rad) * UnityEngine.Mathf.Cos(diff.y);
            float rads = UnityEngine.Mathf.Acos(cosc);
            return rads;
        }

        #endregion

        #region Star Field Stuff

        /// <summary>
        ///     Make a polar grid of star rays.
        /// </summary>
        /// <param name="subpoint">The subpoint of the star whose light is modeled by the rays</param>
        /// <param name="count"> Number of rays between the subpoint and the terminator.</param>
        /// <returns></returns>
        public static UnityEngine.Vector3[,] starRayRendererCoordiantesWithinEarth(UnityEngine.Vector3 subpoint,
            int count)
        {
            float spacing = RSDESManager.EarthRadius / count;
            return starRayRendererCoordiantes(subpoint, spacing, count, coordinateSystem.polar);
        }

        public static UnityEngine.Vector3[,] starRayRendererCoordiantesWithinEarth(UnityEngine.Vector3 subpoint,
            float spacing, int count)
        {
            return starRayRendererCoordiantes(subpoint, spacing, count, coordinateSystem.polar);
        }

        public enum coordinateSystem
        {
            cartesian,
            polar
        }

        /// <summary>
        ///     Generate two points to define every starRay.
        /// </summary>
        /// <param name="subpoint">The subpoint of the star whose light is modeled by the rays </param>
        /// <param name="spacing">The distance between star rays, to scale.</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static UnityEngine.Vector3[,] starRayRendererCoordiantes(UnityEngine.Vector3 subpoint, float spacing,
            int count, coordinateSystem cs)
        {
            int size = 0;
            switch (cs)
            {
                case coordinateSystem.cartesian:
                    size = (int) UnityEngine.Mathf.Pow(2 * count - 1, 2);
                    break;
                case coordinateSystem.polar:
                    size = count * count + 1;
                    break;
            }

            UnityEngine.Vector3[,] result = new UnityEngine.Vector3[2, size];
            UnityEngine.Vector3 direction = subpoint - RSDESManager.earthPos;

            UnityEngine.Vector3 basis0 = UnityEngine.Vector3.Cross(direction, UnityEngine.Vector3.right).normalized;
            if (basis0.magnitude == 0)
                basis0 = UnityEngine.Vector3.Cross(direction, UnityEngine.Vector3.forward).normalized;
            UnityEngine.Vector3 basis1 = UnityEngine.Vector3.Cross(direction, basis0).normalized;

            if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(basis0, basis1)) +
                UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(basis0, direction)) +
                UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(basis1, direction)) != 0)
            {
                if (UnityEngine.Vector3.Dot(basis0, basis1) != 0) UnityEngine.Debug.LogWarning("BASIS NOT ORTHAGNOAL");
                if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(basis0, direction)) +
                    UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(basis1, direction)) !=
                    0) UnityEngine.Debug.Log("BASIS NOT NORMAL");
                UnityEngine.Vector3.OrthoNormalize(ref direction, ref basis0, ref basis1);
            }

            switch (cs)
            {
                case coordinateSystem.cartesian:
                    for (int i = -count + 1; i < count; i++)
                    {
                        for (int j = -count + 1; j < count; j++)
                        {
                            UnityEngine.Vector3 center1 = i * spacing * basis0 + j * spacing * basis1 + subpoint;

                            result[0, (count + i - 1) * (2 * count - 1) + (count + j - 1)] = center1 + 500 * direction;
                            result[1, (count + i - 1) * (2 * count - 1) + (count + j - 1)] = center1 - 500 * direction;
                        }
                    }

                    break;
                case coordinateSystem.polar:
                    for (int i = 0; i < count; i++)
                    {
                        //iterate around circles
                        float theta = i * 360f * UnityEngine.Mathf.Deg2Rad / count;
                        for (int j = 0; j < count; j++)
                        {
                            //iterate through radius.
                            float rad = (j + 1) * spacing;
                            UnityEngine.Vector3 center2 =
                                basis0 * rad * UnityEngine.Mathf.Cos(theta) +
                                basis1 * rad * UnityEngine.Mathf.Sin(theta) + subpoint;
                            result[0, i * count + j] = center2 + 500 * direction;
                            result[1, i * count + j] = center2 - 500 * direction;
                        }
                    }

                    result[0, count * count] = subpoint + 500 * direction;
                    result[1, count * count] = subpoint - 500 * direction;
                    break;
            }

            return result;
        }

        public static UnityEngine.Vector3[] starRayRendererCoordiantes(pinData SUBPOINT, pinData CENTER)
        {
            UnityEngine.Vector3 subpoint = directionFromLatLong(SUBPOINT.latLong).Translate(RSDESManager.earthPos);
            UnityEngine.Vector3 center = directionFromLatLong(CENTER.latLong).ScaleMultiplier(RSDESManager.EarthRadius)
                .Translate(RSDESManager.earthPos);
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[2];

            UnityEngine.Vector3 direction = subpoint - RSDESManager.earthPos;

            //the moon is a special case where the lines are not parallel.
            if (SUBPOINT.pin.myPintype == RSDESPin.pintype.Moon)
                direction = directionFromLatLong(SUBPOINT.latLong)
                                .ScaleMultiplier(RSDESManager.SimulationScale * RSDESManager.moonDist)
                                .Translate(RSDESManager.earthPos) - center;

            UnityEngine.Vector3 earthPos = RSDESManager.earthPos;
            float earthRad = RSDESManager.EarthRadius;

            result[0] = center + 500 * direction;
            result[1] = center - 500 * direction;
            return result;
        }

        public static UnityEngine.Vector3[] starRayRendererCoordiantes(pinData SUBPOINT, UnityEngine.Vector2 CENTER)
        {
            UnityEngine.Vector3 subpoint = directionFromLatLong(SUBPOINT.latLong).Translate(RSDESManager.earthPos);
            UnityEngine.Vector3 center = directionFromLatLong(CENTER).ScaleMultiplier(RSDESManager.EarthRadius)
                .Translate(RSDESManager.earthPos);
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[2];

            UnityEngine.Vector3 direction = subpoint - RSDESManager.earthPos;
            //the moon is a special case where the lines are not parallel.
            if (SUBPOINT.pin.myPintype == RSDESPin.pintype.Moon)
                direction = directionFromLatLong(SUBPOINT.latLong)
                                .ScaleMultiplier(RSDESManager.SimulationScale * RSDESManager.moonDist)
                                .Translate(RSDESManager.earthPos) - center;

            UnityEngine.Vector3 earthPos = RSDESManager.earthPos;
            float earthRad = RSDESManager.EarthRadius;

            result[0] = center + 500 * direction;
            result[1] = center - 500 * direction;
            return result;
        }

        #endregion
    }
}