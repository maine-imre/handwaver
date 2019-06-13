using System;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using WebSocketSharp;

namespace IMRE.HandWaver.Kernel
{
    public class HandWaverServerSocket : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(HandWaverServerTransport.serverHandhake());

            using (WebSocket ws = new WebSocket("ws://"+HandWaverServerTransport.HOSTURL))
            {
                ws.OnMessage += (sender, e) => { Debug.Log(e.Data); };
                ws.Connect();
            }
            
            StartCoroutine(HandWaverServerTransport.execCommand("f:y=x"));
            StartCoroutine(HandWaverServerTransport.execCommand("f:y=-x"));
            
        }
    }
}