using System;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.Geos
{
    public static class GeoElementDataBase
    {
        /// <summary>
        /// Dictionary to keep track of the Geoelement's id based on the string name
        /// </summary>
        private static Dictionary<String, int> GeoNameDb;

        /// <summary>
        /// List of all GeoElements within the scene.
        /// </summary>
        private static List<GeoElement> GeoElements = new List<GeoElement>();
        
        /// <summary>
        /// List of all GeoElement meshes.
        /// 
        /// </summary>
        private static List<Mesh> GeoElementMesh = new List<Mesh>();

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
            GeoElements[e.ElementId] = e;
            GeoNameDb.Add(e.ElementName.ToString(), e.ElementId);
        }
    }
}