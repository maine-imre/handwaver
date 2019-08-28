using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using IMRE.HandWaver.Kernel.Geos;
using socket.io;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : MonoBehaviour
    {
        /// <summary>
        ///     If this is set, the session will use this session ID rather than generate a new one.
        /// </summary>
        public static string overrideSID;

        private static readonly string pattern = @"(?'elementName'\w+)\s*\=\s*(?'type'[\w]*)\((?'args'[\w{}.,\s]+)\)";

        private static readonly RegexOptions options =
            RegexOptions.Multiline;

        /// <summary>
        ///     For use in assigning id numbers to geoelements.
        /// </summary>
        private static int id;

        private bool connect;

        public Socket sock;

        private int xint;

        private void Start()
        {
            initSession();
            sock = Socket.Connect("http://localhost:8080");

            sock.On(SystemEvents.connect, SubscribeCallback);

            sock.On("disconnect", () => { Debug.Log("disconnected"); });
            sock.On("connect_error", (string str) => { Debug.LogError(str); });
            sock.On("add", addFunc);
            sock.On("remove", removeFunc);
            sock.On("update", updateFunc);
            sock.On("rename", renameFunc);

            StartCoroutine(delayedStart());
        }

        private IEnumerator delayedStart()
        {
            while (!connect) yield return new WaitForEndOfFrame();
            StartCoroutine(HandWaverServerTransport.execCommand("A = (1,1,1)")); // Point B
            yield return new WaitForEndOfFrame();
            StartCoroutine(HandWaverServerTransport.execCommand("B = (0,1,1)")); // Point B
            yield return new WaitForEndOfFrame();
            StartCoroutine(HandWaverServerTransport.execCommand("Line(A, B)")); // Line f
        }

        private void SubscribeCallback()
        {
            sock.Emit("subscribe", HandWaverServerTransport.sessionId, Debug.Log);
            Debug.Log("connected");
            connect = true;
        }

        public void addFunc(string objCmd)
        {
            Debug.Log("server added!\n " + objCmd);
            // Should add element to GeoElementDataBase

            foreach (Match cmd in Regex.Matches(objCmd,
                pattern, options))
            {
                var eName = cmd.Groups["elementName"].Value;
                var eType = cmd.Groups["type"].Value;
                GeoElementDataBase.AddElement(
                    new GeoElement(id++, new NativeString64(eName)));

                var e =
                    GeoElementDataBase.GetElement(eName);
                var args =
                    cmd.Groups["args"].Value.Split(',').Select(s => s.Trim()).ToArray();

                GeoElementDataBase.GeoElements[e.ElementId] = UpdateElement(e, eType, args);
            }

        }

        public void removeFunc(string objName)
        {
            Debug.Log("removed " + objName);
            GeoElementDataBase.RemoveElement(objName);
        }

        public void updateFunc(string objCmd)
        {
            Debug.Log("server updated!\n " + objCmd);
            foreach (Match cmd in Regex.Matches(objCmd,
                pattern, options))
            {
                var eName = cmd.Groups["elementName"].Value.Trim();
                var eType = cmd.Groups["type"].Value;
                var e =
                    GeoElementDataBase.GetElement(eName);
                var args =
                    cmd.Groups["args"].Value.Split(',').Select(s => s.Trim()).ToArray();
                ;

                //Debug.LogFormat("*{0}* of type ({1}) with args **{2}** was updated", eName, eType, args.ToString());

                GeoElementDataBase.GeoElements[e.ElementId] = UpdateElement(e, eType, args);
            }
        }

        /// <summary>
        ///     Updates the element with passed in data
        /// </summary>
        /// <param name="e">GeoElement Object</param>
        /// <param name="eType">enum type of the element</param>
        /// <param name="args">arguments used to create element</param>
        /// <exception cref="System.ArgumentException">Unsupported type</exception>
        private static GeoElement UpdateElement(GeoElement e,
            string eType, string[] args)
        {
            if (e.Type == ElementType.err)
                switch (eType)
                {
                    case "":
                        e.Type = ElementType.point;
                        break;
                    case "Line":
                        e.Type = ElementType.line;
                        break;
                    case "Plane":
                        e.Type = ElementType.plane;
                        break;
                    case "Sphere":
                        e.Type = ElementType.sphere;
                        break;
                    case "Circle":
                        e.Type = ElementType.circle;
                        break;
                    default:
                        Debug.LogError("Misunderstood type \"" + eType + "\"");
                        break;
                }

            var eDeps = e.Deps;
            switch (e.Type)
            {
                case ElementType.point:
                    //Assumed Point
                    // arg0 is x value
                    // arg1 is y value.
                    // arg2 is z value.
                    var newPos = new float3(float.Parse(args[0]),
                        float.Parse(args[1]), float.Parse(args[2]));
                    e.F0 = newPos;
                    break;
                case ElementType.line:
                    // arguments should be as follows
                    // arg0 is name of point A
                    // arg1 is name of point B
                    eDeps[0] = GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = GeoElementDataBase.GetElementId(args[1]);
                    break;
                case ElementType.plane:

                    if (args.Length == 3) //Assume construction method as follows Plane(PointA, PointB, PointC)
                    {
                        eDeps[0] = GeoElementDataBase.GetElementId(args[0]);
                        eDeps[1] = GeoElementDataBase.GetElementId(args[1]);
                        eDeps[2] = GeoElementDataBase.GetElementId(args[2]);
                        break;
                    }

                    // arguments should be as follows
                    // arg0 is name of point A
                    // arg1 is float 3 of normal dir
                    eDeps[0] = GeoElementDataBase.GetElementId(args[0]);
                    e.F0 = args[1].ParseFloat3();

                    break;
                case ElementType.sphere:
                    // arguments should be as follows
                    // arg0 is name of origin point
                    // arg1 is name of edge point
                    eDeps[0] = GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = GeoElementDataBase.GetElementId(args[1]);
                    break;
                case ElementType.circle:
                    // arguments should be as follows
                    // arg0 is name of origin point
                    // arg1 is name of edge point
                    // arg2 is the normal direction
                    eDeps[0] = GeoElementDataBase.GetElementId(args[0]);
                    eDeps[1] = GeoElementDataBase.GetElementId(args[1]);
                    e.F0 = args[2].ParseFloat3();
                    break;
                default:
                    throw new ArgumentException("Misunderstood type \"" + eType + "\"");
            }

            e.Deps = eDeps;
            e.Updated = DateTime.Now;
            return e;
        }

        public void renameFunc(string objName)
        {
            Debug.Log("rename " + objName);

            //TODO: Implement
        }

        [ContextMenu("test cmd")]
        public void testCMD()
        {
            //Test the command changes
            StartCoroutine(HandWaverServerTransport.execCommand("A = (1, 1, 0")); // Point A
        }

        [ContextMenu("Output Element Dictionary")]
        public void outputElements()
        {
            foreach (var geo in GeoElementDataBase
                    .GeoElements.GetValueArray(Allocator.Temp)) //for each named element
                //debug out name, deps values, and f0 value
                Debug.Log(geo);
        }

        /// <summary>
        ///     Initializes the Session ID within the HWServer Transport.
        ///     Attempts a handshake with the server with the specified sessionID.
        /// </summary>
        private void initSession()
        {
            HandWaverServerTransport.sessionId =
                string.IsNullOrEmpty(overrideSID) ? Guid.NewGuid().ToString() : overrideSID;
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
        public static float3 ParseFloat3(this string value)
        {
            // Remove the parentheses
            if (value.StartsWith("{") && value.EndsWith("}") || // i.e. {X,Y,Z}
                value.StartsWith("(") && value.EndsWith(")")) // i.e. (X,Y,Z)
                value = value.Substring(1, value.Length - 2);

            // split the items
            var sArray = value.Split(',');

            // store as a Vector3
            var result = new float3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
    }
}