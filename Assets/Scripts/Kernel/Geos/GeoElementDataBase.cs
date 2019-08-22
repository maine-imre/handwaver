namespace IMRE.HandWaver.Kernel.Geos
{
    public static class GeoElementDataBase
    {
        /// <summary>
        ///     Dictionary to keep track of the Geoelement's id based on the string name.
        ///     Key is element name as a string (max 30 characters).
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, int> GeoNameDb =
            new System.Collections.Generic.Dictionary<string, int>();

        /// <summary>
        ///     Dictionary of all GeoElements within the scene.
        ///     Key is element id as int.
        /// </summary>
        public static System.Collections.Generic.Dictionary<int, GeoElement> GeoElements =
            new System.Collections.Generic.Dictionary<int, GeoElement>();

        /// <summary>
        ///     Dictionary of all GeoElement meshes.
        ///     Key is element id as int.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<int, UnityEngine.Mesh> GeoElementMesh =
            new System.Collections.Generic.Dictionary<int, UnityEngine.Mesh>();

        /// <summary>
        ///     Interface method to return element id from element name.
        /// </summary>
        /// <param name="k">Element Name</param>
        /// <returns>Element Id</returns>
        public static int GetElementId(string k)
        {
            return GeoNameDb[k];
        }

        /// <summary>
        ///     Interface method to return the geoelement data from element id
        /// </summary>
        /// <param name="i">element id</param>
        /// <returns>the ith element</returns>
        public static GeoElement GetElement(int i)
        {
            return GeoElements[i];
        }

        /// <summary>
        ///     Interface method to return the geoelement data from element name
        /// </summary>
        /// <param name="k">Element Name</param>
        /// <returns>element labeled k</returns>
        public static GeoElement GetElement(string k)
        {
            return GetElement(GetElementId(k));
        }

        /// <summary>
        ///     Adds the GeoElement to the dictionaries
        /// </summary>
        /// <param name="e">Element to be added.</param>
        public static void AddElement(GeoElement e)
        {
            if (HasElement(e))
                UnityEngine.Debug.LogWarningFormat("Element {0} already exists within the current context.",
                    e.ElementId);
            GeoElements[e.ElementId] = e;
            GeoNameDb[e.ElementName.ToString()] = e.ElementId;
            GeoElementMesh[e.ElementId] = new UnityEngine.Mesh();
        }

        /// <summary>
        ///     Removes GeoElement from all dictionaries
        /// </summary>
        /// <param name="eName">Element Name</param>
        public static void RemoveElement(string eName)
        {
            GeoElements.Remove(GetElementId(eName));
            GeoElementMesh.Remove(GetElementId(eName));

            //Must be done last
            GeoNameDb.Remove(eName);
        }

        public static bool HasElement(GeoElement e)
        {
            return GeoElements.ContainsKey(e.ElementId);
        }

        public static bool HasElement(string eName)
        {
            return GeoNameDb.ContainsKey(eName);
        }

        public static bool HasElement(int eId)
        {
            return GeoElements.ContainsKey(eId);
        }
    }
}