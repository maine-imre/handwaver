namespace IMRE.HandWaver.Kernel
{
    /// <summary>
    ///     This static class will be used to communicate with the HandWaver servers.
    /// </summary>
    public static class HandWaverServerTransport
    {
        /// <summary>
        ///     Version number for use of validation of current app and or compat issues.
        /// </summary>
        private const string VERSION = "1234321";

        /// <summary>
        ///     This is the host url that all requests are sent to.
        ///     For now we are selfhosting the server on the computer we are working on.
        ///     This MUST be changed for production.
        /// </summary>
        internal const string HOSTURL = "localhost:8080";

        /// <summary>
        ///     Internal cache of session id.
        ///     Access this through the public sessionID var.
        /// </summary>
        private static string _sessionId = "c55d57b8-8624-11e9-bc42-526af7764f64";

        /// <summary>
        ///     Internal Guid representation of the
        ///     <para>_sessionId</para>
        /// </summary>
        private static System.Guid _guid;

        /// <summary>
        ///     Returns true when a valid session is established with a server.
        /// </summary>
        private static bool sessionEstablished;

        /// <summary>
        ///     Public access to the session id string.
        ///     This should be used for getting/setting
        /// </summary>
        public static string sessionId
        {
            // This should remain an expression body as we do not need to do anything during a get of sessionId.
            get => _sessionId;

            // Change from expression body later if needed to add functionality.
            set
            {
                // Is it a valid Guid
                if (System.Guid.TryParse(value, out _guid))
                    // assign
                    _sessionId = value;
                else
                    //Throw a format exception
                    throw new System.FormatException(value + " is not in valid Guid format!");
            }
        }

        /// <summary>
        ///     Gets a PNG of the current session.
        ///     This will be a static perspective, and then cached as a texture.
        ///     TODO: return or use texture.
        /// </summary>
        /// <param name="path">If saving the texture, pass in the location to be used here.</param>
        /// <returns></returns>
        public static System.Collections.IEnumerator getPNG([JetBrains.Annotations.CanBeNullAttribute]
            string path)
        {
            using (UnityEngine.Networking.UnityWebRequest req =
                UnityEngine.Networking.UnityWebRequest.Get(HOSTURL + "/getPNG?sessionId=" + sessionId))
            {
                //request and wait for response
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    UnityEngine.Debug.Log(": Error: " + req.error);
                    req.Dispose();
                }
                else
                {
                    try
                    {
                        // Get a cached copy of the texture.
                        UnityEngine.Texture tex = ((UnityEngine.Networking.DownloadHandlerTexture) req.downloadHandler)
                            .texture;

                        // If the path is null or empty skip saving.
                        if (string.IsNullOrEmpty(path)) yield break;
                        byte[] bytes = UnityEngine.ImageConversion.EncodeToPNG((UnityEngine.Texture2D) tex);
                        System.IO.File.WriteAllBytes(path, bytes);
                    }
                    finally
                    {
                        // Dispose of the request as we are finished with it}
                        req.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Sends a request to the server to save the current session.
        ///     Optionally provide a path to save the session to file.
        /// </summary>
        /// <param name="path">Where you want the xml document to be saved.</param>
        /// <returns></returns>
        public static System.Collections.IEnumerator saveCurrSession([JetBrains.Annotations.CanBeNullAttribute]
            string path)
        {
            UnityEngine.WWWForm form = new UnityEngine.WWWForm();
            form.AddField("sessionId", sessionId);

            using (UnityEngine.Networking.UnityWebRequest req =
                UnityEngine.Networking.UnityWebRequest.Post(HOSTURL + "/saveCurrSession", form))
            {
                //request and wait for response
                yield return req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError)
                {
                    UnityEngine.Debug.Log(req.error);
                    req.Dispose();
                }
                else
                {
                    try
                    {
                        UnityEngine.Debug.Log("Session saved!");

                        // If the path doesnt exist. Skip saving.
                        if (string.IsNullOrEmpty(path)) yield break;

                        // Create XML Document
                        System.Xml.XmlDocument currSession = new System.Xml.XmlDocument();

                        // Load in data from response body
                        currSession.LoadXml(((UnityEngine.Networking.DownloadHandlerFile) req.downloadHandler).text);


                        // Save the xmldocument to the path provided.
                        currSession.Save(path);
                    }
                    finally
                    {
                        // Dispose of the request that we are done using.
                        req.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the current session from the server.
        /// </summary>
        /// <param name="path">Where we locally save the returned session data</param>
        /// <returns></returns>
        public static System.Collections.IEnumerator getCurrSession(string path)
        {
            using (UnityEngine.Networking.UnityWebRequest req =
                UnityEngine.Networking.UnityWebRequest.Get(HOSTURL + "/getCurrSession?sessionId=" + sessionId))
            {
                //request and wait for response
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    UnityEngine.Debug.Log(": Error: " + req.error);
                    req.Dispose();
                }
                else
                {
                    try
                    {
                        UnityEngine.Debug.Log("Session saved!");

                        // Create XML Document
                        System.Xml.XmlDocument currSession = new System.Xml.XmlDocument();

                        // Load in data from response body
                        currSession.LoadXml(((UnityEngine.Networking.DownloadHandlerFile) req.downloadHandler).text);

                        // If the path doesnt exist. Skip saving.
                        if (string.IsNullOrEmpty(path))
                        {
                            UnityEngine.Debug.LogError("Path provided is null or empty: " + path);
                            yield break;
                        }

                        // Save the xmldocument to the path provided.
                        currSession.Save(path);
                    }
                    finally
                    {
                        // Dispose of the request that we are done using.
                        req.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Sends a geogebra command to the server at the
        ///     <para>sessionID</para>
        ///     currently in use.
        /// </summary>
        /// <param name="cmdString">A string containing a geogebra command. This is not checked before being sent.</param>
        /// <returns></returns>
        public static System.Collections.IEnumerator execCommand(string cmdString)
        {
            while (!sessionEstablished) yield return new UnityEngine.WaitForEndOfFrame();
            // Request body
            UnityEngine.WWWForm form = new UnityEngine.WWWForm();
            // Populate request body
            form.AddField("sessionId", sessionId);
            form.AddField("command", cmdString);

            using (UnityEngine.Networking.UnityWebRequest req =
                UnityEngine.Networking.UnityWebRequest.Post(HOSTURL + "/command", form))
            {
                //request and wait for response
                yield return req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError)
                    UnityEngine.Debug.Log(req.error);
                else
                    UnityEngine.Debug.Log("command sent!\n" + cmdString);
                // Dispose of the request that we are done using.
                req.Dispose();
            }
        }

        /// <summary>
        ///     Sends a request to establish a session with the server.
        ///     If the sessionId is already within the system, it returns the xml data for scene recreation.
        /// </summary>
        /// <returns></returns>
        public static System.Collections.IEnumerator serverHandhake()
        {
            using (UnityEngine.Networking.UnityWebRequest req =
                UnityEngine.Networking.UnityWebRequest.Get(HOSTURL + "/handshake?sessionId=" + sessionId + "&version=" +
                                                           VERSION))
            {
                //request and wait for response
                yield return req.SendWebRequest();
                if (req.isNetworkError)
                {
                    UnityEngine.Debug.Log(": Error: " + req.error);
                    req.Dispose();
                }
                else
                {
                    try
                    {
                        UnityEngine.Debug.Log("Established connection to server with session id " + sessionId);

                        sessionEstablished = true;
                        //TODO: check if there is a response of a session xml data.
                        //TODO: reconstruct scene using the xml data returned.
                    }
                    finally
                    {
                        // Dispose of the request that we are done using.
                        req.Dispose();
                    }
                }
            }
        }
    }
}