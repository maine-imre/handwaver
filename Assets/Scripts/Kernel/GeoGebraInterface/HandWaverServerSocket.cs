using Enumerable = System.Linq.Enumerable;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : UnityEngine.MonoBehaviour
    {
        /// <summary>
        ///     If this is set, the session will use this session ID rather than generate a new one.
        /// </summary>
        public static string overrideSID;

        private static readonly string pattern = @"(?'elementName'\w+)\s*\=\s*(?'type'[\w]*)\((?'args'[\w{}.,\s]+)\)";

        private static readonly System.Text.RegularExpressions.RegexOptions options =
            System.Text.RegularExpressions.RegexOptions.Multiline;

        /// <summary>
        ///     For use in assigning id numbers to geoelements.
        /// </summary>
        private static int id;

        private bool connect;

        public socket.io.Socket sock;

        private int xint;

        private void Start()
        {
            initSession();
            sock = socket.io.Socket.Connect("http://localhost:8080");

            sock.On(socket.io.SystemEvents.connect, SubscribeCallback);

            sock.On("disconnect", () => { UnityEngine.Debug.Log("disconnected"); });
            sock.On("connect_error", (string str) => { UnityEngine.Debug.LogError(str); });
            sock.On("add", addFunc);
            sock.On("remove", removeFunc);
            sock.On("update", updateFunc);
            sock.On("rename", renameFunc);

            StartCoroutine(delayedStart());
        }

        private System.Collections.IEnumerator delayedStart()
        {
            while (!connect) yield return new UnityEngine.WaitForEndOfFrame();
            testCMD();
            yield return new UnityEngine.WaitForEndOfFrame();
            StartCoroutine(HandWaverServerTransport.execCommand("B = (5,5,6)")); // Point B
            yield return new UnityEngine.WaitForEndOfFrame();
            StartCoroutine(HandWaverServerTransport.execCommand("Line(A, B)")); // Line f
        }

        private void SubscribeCallback()
        {
            sock.Emit("subscribe", HandWaverServerTransport.sessionId, UnityEngine.Debug.Log);
            UnityEngine.Debug.Log("connected");
            connect = true;
        }

        public void addFunc(string objCmd)
        {
            UnityEngine.Debug.Log("server added!\n " + objCmd);
            // Should add element to GeoElementDataBase

            foreach (System.Text.RegularExpressions.Match cmd in System.Text.RegularExpressions.Regex.Matches(objCmd,
                pattern, options))
            {
                string eName = cmd.Groups["elementName"].Value;
                string eType = cmd.Groups["type"].Value;
                IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.AddElement(
                    new IMRE.HandWaver.Kernel.Geos.GeoElement(id++, new Unity.Entities.NativeString64(eName)));

                IMRE.HandWaver.Kernel.Geos.GeoElement e =
                    IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElement(eName);
                string[] args =
                    Enumerable.ToArray(Enumerable.Select(cmd.Groups["args"].Value.Split(','), s => s.Trim()));

                IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GeoElements[e.ElementId] = UpdateElement(e, eType, args);
            }

            outputElements();
        }

        public void removeFunc(string objName)
        {
            UnityEngine.Debug.Log("removed " + objName);
            IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.RemoveElement(objName);
        }

        public void updateFunc(string objCmd)
        {
            UnityEngine.Debug.Log("server updated!\n " + objCmd);
            foreach (System.Text.RegularExpressions.Match cmd in System.Text.RegularExpressions.Regex.Matches(objCmd,
                pattern, options))
            {
                string eName = cmd.Groups["elementName"].Value.Trim();
                string eType = cmd.Groups["type"].Value;
                IMRE.HandWaver.Kernel.Geos.GeoElement e =
                    IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElement(eName);
                string[] args =
                    Enumerable.ToArray(Enumerable.Select(cmd.Groups["args"].Value.Split(','), s => s.Trim()));
                ;

                //Debug.LogFormat("*{0}* of type ({1}) with args **{2}** was updated", eName, eType, args.ToString());

                IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GeoElements[e.ElementId] = UpdateElement(e, eType, args);
            }
        }

        /// <summary>
        ///     Updates the element with passed in data
        /// </summary>
        /// <param name="e">GeoElement Object</param>
        /// <param name="eType">enum type of the element</param>
        /// <param name="args">arguments used to create element</param>
        /// <exception cref="System.ArgumentException">Unsupported type</exception>
        private static IMRE.HandWaver.Kernel.Geos.GeoElement UpdateElement(IMRE.HandWaver.Kernel.Geos.GeoElement e,
            string eType, string[] args)
        {
            if (e.Type == IMRE.HandWaver.Kernel.Geos.ElementType.err)
                switch (eType)
                {
                    case "":
                        e.Type = IMRE.HandWaver.Kernel.Geos.ElementType.point;
                        break;
                    case "Line":
                        e.Type = IMRE.HandWaver.Kernel.Geos.ElementType.line;
                        break;
                    case "Plane":
                        e.Type = IMRE.HandWaver.Kernel.Geos.ElementType.plane;
                        break;
                    case "Sphere":
                        e.Type = IMRE.HandWaver.Kernel.Geos.ElementType.sphere;
                        break;
                    case "Circle":
                        e.Type = IMRE.HandWaver.Kernel.Geos.ElementType.circle;
                        break;
                    default:
                        UnityEngine.Debug.LogError("Misunderstood type \"" + eType + "\"");
                        break;
                }

            Unity.Mathematics.int4 eDeps = e.Deps;
            switch (e.Type)
            {
                case IMRE.HandWaver.Kernel.Geos.ElementType.point:
                    //Assumed Point
                    // arg0 is x value
                    // arg1 is y value.
                    // arg2 is z value.
                    Unity.Mathematics.float3 newPos = new Unity.Mathematics.float3(float.Parse(args[0]),
                        float.Parse(args[1]), float.Parse(args[2]));
                    e.F0 = newPos;
                    break;
                case IMRE.HandWaver.Kernel.Geos.ElementType.line:
                    // arguments should be as follows
                    // arg0 is name of point A
                    // arg1 is name of point B
                    eDeps[0] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[1]);
                    break;
                case IMRE.HandWaver.Kernel.Geos.ElementType.plane:

                    if (args.Length == 3) //Assume construction method as follows Plane(PointA, PointB, PointC)
                    {
                        eDeps[0] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[0]);
                        eDeps[1] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[1]);
                        eDeps[2] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[2]);
                        break;
                    }

                    // arguments should be as follows
                    // arg0 is name of point A
                    // arg1 is float 3 of normal dir
                    eDeps[0] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[0]);
                    e.F0 = args[1].ParseFloat3();

                    break;
                case IMRE.HandWaver.Kernel.Geos.ElementType.sphere:
                    // arguments should be as follows
                    // arg0 is name of origin point
                    // arg1 is name of edge point
                    eDeps[0] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[1]);
                    break;
                case IMRE.HandWaver.Kernel.Geos.ElementType.circle:
                    // arguments should be as follows
                    // arg0 is name of origin point
                    // arg1 is name of edge point
                    // arg2 is the normal direction
                    eDeps[0] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = IMRE.HandWaver.Kernel.Geos.GeoElementDataBase.GetElementId(args[1]);
                    e.F0 = args[2].ParseFloat3();
                    break;
                default:
                    throw new System.ArgumentException("Misunderstood type \"" + eType + "\"");
            }

            e.Deps = eDeps;
            e.Updated = System.DateTime.Now;
            return e;
        }

        public void renameFunc(string objName)
        {
            UnityEngine.Debug.Log("rename " + objName);

            //TODO: Implement
        }

        [UnityEngine.ContextMenu("test cmd")]
        public void testCMD()
        {
            //Test the command changes
            StartCoroutine(HandWaverServerTransport.execCommand("A = (" + (xint++) + ", 2, 3)")); // Point A
        }

        [UnityEngine.ContextMenu("Output Element Dictionary")]
        public void outputElements()
        {
            foreach (IMRE.HandWaver.Kernel.Geos.GeoElement geo in IMRE.HandWaver.Kernel.Geos.GeoElementDataBase
                    .GeoElements.Values) //for each named element
                //debug out name, deps values, and f0 value
                UnityEngine.Debug.Log(geo);
        }

        /// <summary>
        ///     Initializes the Session ID within the HWServer Transport.
        ///     Attempts a handshake with the server with the specified sessionID.
        /// </summary>
        private void initSession()
        {
            HandWaverServerTransport.sessionId =
                string.IsNullOrEmpty(overrideSID) ? System.Guid.NewGuid().ToString() : overrideSID;
            StartCoroutine(HandWaverServerTransport.serverHandhake());
        }
    }

    public static class Float3Ext
    {
        /// <summary>
        ///     Parses a float3 from a string.
        ///     Accepts format X,Y,Z (X,Y,Z) {X,Y,Z}
        /// </summary>
        /// <param name="value">float3 as string</param>
        /// <returns>float3 representation of value</returns>
        public static Unity.Mathematics.float3 ParseFloat3(this string value)
        {
            // Remove the parentheses
            if (value.StartsWith("{") && value.EndsWith("}") || // i.e. {X,Y,Z}
                value.StartsWith("(") && value.EndsWith(")")) // i.e. (X,Y,Z)
                value = value.Substring(1, value.Length - 2);

            // split the items
            string[] sArray = value.Split(',');

            // store as a Vector3
            Unity.Mathematics.float3 result = new Unity.Mathematics.float3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
    }
}