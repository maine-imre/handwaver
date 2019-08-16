using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IMRE.HandWaver.Kernel.Geos;
using JetBrains.Annotations;
using socket.io;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : MonoBehaviour
    {
        /// <summary>
        ///     If this is set, the session will use this session ID rather than generate a new one.
        /// </summary>
        public static string overrideSID;

        public Socket sock;
        
        
        private static string pattern = @"(?'elementName'\w+)\s*\=\s*(?'type'[\w]*)\((?'args'[\w{}.,\s]+)\)";
        private static RegexOptions options = RegexOptions.Multiline;

        /// <summary>
        /// For use in assigning id numbers to geoelements.
        /// </summary>
        private static int id;
        
        private void Start()
        {

            initSession();
            sock = Socket.Connect("http://localhost:8080");
            
            sock.On("connect", SubscribeCallback);
            sock.On("disconnect", () => { Debug.Log("disconnected"); });
            sock.On("connect_error", (string str) => { Debug.LogError(str); });
            sock.On("add", addFunc);
            sock.On("remove", removeFunc);
            sock.On("update", updateFunc);
            sock.On("rename", renameFunc);

            testCMD();
            StartCoroutine(HandWaverServerTransport.execCommand("B = (5,5,6)"));    // Point B
            StartCoroutine(HandWaverServerTransport.execCommand("Line(A, B)"));          // Line f
        }

        void SubscribeCallback()
        {
            sock.Emit("subscribe", HandWaverServerTransport.sessionId, Debug.Log);
        }
        public void addFunc(string objName)
        {
            Debug.Log("Added "+objName);
            // Should add element to GeoElementDataBase
            GeoElementDataBase.AddElement(new GeoElement(id++, new NativeString64(objName)));
        }

        public void removeFunc(string objName)
        {
            Debug.Log("removed "+objName);
            GeoElementDataBase.RemoveElement(objName);
        }

        public void updateFunc(string objCmd)
        {
            Debug.Log("Updated with msg "+objCmd);
            foreach (Match cmd in Regex.Matches(objCmd, pattern, options))
            {
                string eName = cmd.Groups["elementName"].Value;
                string eType = cmd.Groups["type"].Value;
                GeoElement e = GeoElementDataBase.GetElement(eName);
                string[] args = cmd.Groups["args"].Value.Split(',');

                if (e.type == ElementType.err)
                {
                    switch (eType)
                    {
                        case "":
                            e.type = ElementType.point;
                            break;
                        case "Line":
                            e.type = ElementType.line;
                            break;
                        case "Plane":
                            e.type = ElementType.plane;
                            break;
                        case "Sphere":
                            e.type = ElementType.sphere;
                            break;
                        case "Circle":
                            e.type = ElementType.circle;
                            break;
                        default:
                            Debug.LogError("Misunderstood type \""+eType+"\"");
                            break;
                    }    
                }
                
                switch (eType)
                {
                    case "":
                        //Assumed Point
                        // arg0 is x value
                        // arg1 is y value.
                        // arg2 is z value.
                        float3 newPos = new float3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2]));
                        e.f0 = newPos;
                        break;
                    case "Line":
                        // arguments should be as follows
                        // arg0 is name of point A
                        // arg1 is name of point B
                        e.deps[0] = GeoElementDataBase.GetElementId(args[0]);
                        e.deps[1] = GeoElementDataBase.GetElementId(args[1]);
                        break;
                    case "Plane":
                        
                        if (args.Length == 3)    //Assume construction method as follows Plane(PointA, PointB, PointC)
                        {
                            e.deps[0] = GeoElementDataBase.GetElementId(args[0]);
                            e.deps[1] = GeoElementDataBase.GetElementId(args[1]);
                            e.deps[2] = GeoElementDataBase.GetElementId(args[2]);
                            break;
                        }
                        
                        // arguments should be as follows
                        // arg0 is name of point A
                        // arg1 is float 3 of normal dir
                        e.deps[0] = GeoElementDataBase.GetElementId(args[0]);
                        e.f0 = args[1].ParseFloat3();

                        break;
                    case "Sphere":
                        // arguments should be as follows
                        // arg0 is name of origin point
                        // arg1 is name of edge point
                        e.deps[0] = GeoElementDataBase.GetElementId(args[0]);
                        e.deps[1] = GeoElementDataBase.GetElementId(args[1]);
                        break;
                    case "Circle":
                        // arguments should be as follows
                        // arg0 is name of origin point
                        // arg1 is name of edge point
                        // arg2 is the normal direction
                        e.deps[0] = GeoElementDataBase.GetElementId(args[0]);
                        e.deps[1] = GeoElementDataBase.GetElementId(args[1]);
                        e.f0 = args[2].ParseFloat3();
                        break;
                    default:
                        throw new ArgumentException("Misunderstood type \""+eType+"\"");
                }
                e.Updated = DateTime.Now;
            } 
        }
        public void renameFunc(string objName)
        {
            Debug.Log("rename "+objName);

            //TODO: Implement
        }
       
        
        
        
        private int xint;
        [ContextMenu("test cmd")]
        public void testCMD()
        {
            
            //Test the command changes
            StartCoroutine(HandWaverServerTransport.execCommand("A = ("+(xint++)+", 2, 3)")); // Point A

        }

        [ContextMenu("Output Element Dictionary")]
        public void outputElements()
        {
            foreach (KeyValuePair<string,int> i in GeoElementDataBase.GeoNameDb)    //for each named element
            {
                //debug out name, deps values, and f0 value
                Debug.Log("Element named "+i.Key+" has the values of "+GeoElementDataBase.GetElement(i.Value).deps+" and "+GeoElementDataBase.GetElement(i.Value).f0+"\n");
            }
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
        /// Parses a float3 from a string.
        /// Accepts format X,Y,Z (X,Y,Z) {X,Y,Z}
        /// </summary>
        /// <param name="value">float3 as string</param>
        /// <returns>float3 representation of value</returns>
        public static float3 ParseFloat3(this string value)
        {
            // Remove the parentheses
            if (value.StartsWith ("{") && value.EndsWith ("}") ||    // i.e. {X,Y,Z}
                value.StartsWith ("(") && value.EndsWith (")") )     // i.e. (X,Y,Z)
            {
                value = value.Substring(1, value.Length-2);
            }
 
            // split the items
            string[] sArray = value.Split(',');
 
            // store as a Vector3
            float3 result = new float3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
 
            return result;
        }
    }
}