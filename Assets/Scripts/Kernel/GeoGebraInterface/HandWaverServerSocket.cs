using System;
using socket.io;
using UnityEngine;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : MonoBehaviour
    {
        /// <summary>
        ///     If this is set, the session will use this session ID rather than generate a new one.
        /// </summary>
        public static string overrideSID;

        private void Start()
        {
            initSession();
            var sock = Socket.Connect("http://localhost:8080");                                                                                                                                  
            sock.On("connect", () => Debug.Log("connected"));                                                
            sock.On("disconnect", () => { Debug.Log("disconnected"); });                                     
            sock.On("connect_error", debugLog);                        
            sock.On("add", debugLog);                                                  
            sock.On("remove", debugLog);
            sock.On("update", debugLog);
            sock.On("rename", debugLog);

            sock.Emit("subscribe", HandWaverServerTransport.sessionId);

            //Test the command changes
            StartCoroutine(HandWaverServerTransport.execCommand("A = Point({1, 2, 3})")); // Point A
//            StartCoroutine(HandWaverServerTransport.execCommand("Point({3, 5, 1})"));    // Point B
//            StartCoroutine(HandWaverServerTransport.execCommand("Line(A, B)"));          // Line f
//            StartCoroutine(HandWaverServerTransport.execCommand("A=Point({2,3,4})"));    // Move Point Attempts
        }

        void debugLog(string str)
        {
            Debug.Log(str);
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