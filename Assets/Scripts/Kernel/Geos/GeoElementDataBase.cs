using System;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.Geos
{
    public static class GeoElementDataBase
    {
        /// <summary>
        /// Dictionary to keep track of the Geoelement's id based on the string name.
        /// Key is element name as a string (max 30 characters).
        /// </summary>
        public static Dictionary<String, int> GeoNameDb = new Dictionary<string, int>();

        /// <summary>
        /// Dictionary of all GeoElements within the scene.
        /// Key is element id as int.
        /// </summary>
        private static Dictionary<int,GeoElement> GeoElements = new Dictionary<int,GeoElement>();
        
        /// <summary>
        /// Dictionary of all GeoElement meshes.
        /// Key is element id as int.
        /// </summary>
        private static Dictionary<int,Mesh> GeoElementMesh = new Dictionary<int,Mesh>();

        /// <summary>
        /// Interface method to return element id from element name.
        /// </summary>
        /// <param name="k">Element Name</param>
        /// <returns>Element Id</returns>
        public static int GetElementId(string k) => GeoNameDb[k];
        
        /// <summary>
        /// Interface method to return the geoelement data from element id
        /// </summary>
        /// <param name="i">element id</param>
        /// <returns>the ith element</returns>
        public static GeoElement GetElement(int i) => GeoElements[i];

        /// <summary>
        /// Interface method to return the geoelement data from element name
        /// </summary>
        /// <param name="k">Element Name</param>
        /// <returns>element labeled k</returns>
        public static GeoElement GetElement(string k) => GetElement(GetElementId(k));
        
        /// <summary>
        /// Adds the GeoElement to the dictionaries
        /// </summary>
        /// <param name="e">Element to be added.</param>
        /// <exception cref="Exception">Element already Found exception</exception>
        public static void AddElement(GeoElement e)
        {
            if(GeoElements.ContainsKey(e.ElementId))
            {
                throw new Exception("Element id "+e.ElementId+" already exists with in the system as : "+GeoElements[e.ElementId].ToString());
            }
            GeoElements.Add(e.ElementId, e);
            GeoNameDb.Add(e.ElementName.ToString(), e.ElementId);
            GeoElementMesh.Add(e.ElementId, new Mesh());
        }

        /// <summary>
        /// Removes GeoElement from all dictionaries
        /// </summary>
        /// <param name="eName">Element Name</param>
        public static void RemoveElement(string eName)
        {
            GeoElements.Remove(GetElementId(eName));
            GeoElementMesh.Remove(GetElementId(eName));
            
            //Must be done last
            GeoNameDb.Remove(eName);
        }
        
        public static bool HasElement(GeoElement e) => GeoElements.ContainsKey(e.ElementId);
        public static bool HasElement(string eName) => GeoNameDb.ContainsKey(eName);
        public static bool HasElement(int eId) => GeoElements.ContainsKey(eId);
    }
}