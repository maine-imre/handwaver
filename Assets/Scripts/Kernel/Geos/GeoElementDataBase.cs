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
        private static Dictionary<String, int> GeoNameDb;

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
        
        
        public static void AddElement(GeoElement e)
        {
            if(GeoElements.ContainsKey(e.ElementId))
            {
                throw new Exception("Element id already exists with in the system");
            }
            GeoElements.Add(e.ElementId, e);
            GeoNameDb.Add(e.ElementName.ToString(), e.ElementId);
        }
    }
}