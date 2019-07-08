using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using HybridWebSocket;

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

            WebSocket ws = WebSocketFactory.CreateInstance("ws://"+HandWaverServerTransport.HOSTURL);
            string subscribeString = "subscribe : "+HandWaverServerTransport.sessionId;
            
            // Add OnOpen event listener
            ws.OnOpen += () =>
            {
                Debug.Log("WS connected!");
                Debug.Log("WS state: " + ws.GetState());
            };

            // Add OnMessage event listener
            ws.OnMessage += msg =>
            {
                Debug.Log("WS received message: " + Encoding.UTF8.GetString(msg));
            };

            // Add OnError event listener
            ws.OnError += errMsg =>
            {
                Debug.Log("WS error: " + errMsg);
            };

            // Add OnClose event listener
            ws.OnClose += code =>
            {
                Debug.Log("WS closed with code: " + code.ToString());
            };

            // Connect to the server
            ws.Connect();
            
            ws.Send(subscribeString);

            
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