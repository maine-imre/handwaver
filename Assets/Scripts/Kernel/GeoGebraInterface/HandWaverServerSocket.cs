using System;
using IBM.SocketIO.Impl;
using UnityEngine;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : MonoBehaviour
    {
        
        /// <summary>
        /// If this is set, the session will use this session ID rather than generate a new one.
        /// </summary>
        public static string overrideSID;
        
        private void Start()
        {
            initSession();
                                                                                             
            SocketMediator sock = new SocketMediator("http://localhost:8080");                                                                                                                                  
            sock.On("connect", () => Debug.Log("connected"));                                                
            sock.On("disconnect", () => { Debug.Log("disconnected"); });                                     
            sock.On("connect_error", e => { Debug.Log("failed to connect: " + e); });                        
                                                                                                 
            sock.On("add", args => Debug.Log("add:"+args));                                                  
            sock.On("remove", args => Debug.Log("remove:"+ args));                                           
            sock.On("update", args => Debug.Log("update:"+ args));                                           
            sock.On("rename", args => Debug.Log("rename:"+ args));                                           
                                                                                                 
            sock.Emit("subscribe", HandWaverServerTransport.sessionId);                                                          
        
        //Test the command changes
            StartCoroutine(HandWaverServerTransport.execCommand("B = Point({1, 2, 3})"));    // Point A
//            StartCoroutine(HandWaverServerTransport.execCommand("Point({3, 5, 1})"));    // Point B
//            StartCoroutine(HandWaverServerTransport.execCommand("Line(A, B)"));          // Line f
//            StartCoroutine(HandWaverServerTransport.execCommand("A=Point({2,3,4})"));    // Move Point Attempts
            
            
        }

        /// <summary>
        /// Initializes the Session ID within the HWServer Transport.
        /// Attempts a handshake with the server with the specified sessionID.
        /// </summary>
        private void initSession()
        {
            HandWaverServerTransport.sessionId = string.IsNullOrEmpty(overrideSID) ? Guid.NewGuid().ToString() : overrideSID;
            StartCoroutine(HandWaverServerTransport.serverHandhake());

        }
    }
}