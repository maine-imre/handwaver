using System;
using System.Text.RegularExpressions;
using IMRE.HandWaver.Kernel.Geos;
using socket.io;
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

        private void Start()
        {

            initSession();
            sock = Socket.Connect("http://localhost:8080");

            sock.On("connect", subscribeCallback);
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

        void subscribeCallback()
        {
            sock.Emit("subscribe", HandWaverServerTransport.sessionId);
        }
        public void addFunc(string objName)
        {
            Debug.Log("Add "+objName);
            //TODO: Implement
        }

        public void removeFunc(string objName)
        {
            Debug.Log("remove "+objName);

            //TODO: Implement
        }

        public void updateFunc(string objCmd)
        {
            foreach (Match cmd in Regex.Matches(objCmd, pattern, options))
            {
                string eName = cmd.Groups["elementName"].Value;
                string eType = cmd.Groups["type"].Value;
                GeoElement e = GeoElementDataBase.GetElement(eName);
                string[] args = cmd.Groups["args"].Value.Split(',');
                
                
                switch (eType)
                {
                    case "":
                        //Assumed Circle
                        float3 newPos = new float3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2]));
                        //TODO: assign new pos. Send to render.
                        break;
                    case "Line":
                        
                        break;
                    case "Plane":
                        break;
                    case "Sphere":
                        break;
                    case "Circle":
                        break;
                    default:
                        throw new ArgumentException("Misunderstood type \""+eType+"\"");
                }
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
}