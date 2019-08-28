using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.Geos
{
    public static class GeoElementDataBase
    {
        /// <summary>
        ///     Dictionary to keep track of the Geoelement's id based on the string name.
        ///     Key is element name as a string (max 30 characters).
        /// </summary>
        public static NativeHashMap<NativeString64, int> GeoNameDb = new NativeHashMap<NativeString64, int>(200, Allocator.Persistent);

        /// <summary>
        ///     Dictionary of all GeoElements within the scene.
        ///     Key is element id as int.
        /// </summary>
        public static NativeHashMap<int, GeoElement> GeoElements = new NativeHashMap<int, GeoElement>(200, Allocator.Persistent);
        
        public static NativeHashMap<int, RenderMesh> GeoRenderMeshs = new NativeHashMap<int, RenderMesh>(200, Allocator.Persistent);


        /// <summary>
        ///     Interface method to return element id from element name.
        /// </summary>
        /// <param name="k">Element Name</param>
        /// <returns>Element Id</returns>
        public static int GetElementId(string k)
        {
            return GeoNameDb[new NativeString64(k)];
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
            GeoElements[e.ElementId] = e;
            GeoRenderMeshs[e.ElementId] = new RenderMesh();
            GeoNameDb[e.ElementName] = e.ElementId;
        }

        /// <summary>
        ///     Removes GeoElement from all dictionaries
        /// </summary>
        /// <param name="eName">Element Name</param>
        public static void RemoveElement(string eName)
        {
            GeoElements.Remove(GetElementId(eName));
            GeoRenderMeshs.Remove(GetElementId(eName));
            //Must be done last if removed by Name
            GeoNameDb.Remove(new NativeString64(eName));
        }
        
        public static void RemoveElement(int eId)
        {
            GeoNameDb.Remove(GetElement(eId).ElementName);
            GeoRenderMeshs.Remove(eId);
            //Must be done last if removed by Id
            GeoElements.Remove(eId);
        }

        public static bool HasElement(GeoElement e)
        {
            return GeoElements.ContainsKey(e.ElementId);
        }

        public static bool HasElement(string eName)
        {
            return GeoNameDb.ContainsKey(new NativeString64(eName));
        }

        public static bool HasElement(int eId)
        {
            return GeoElements.ContainsKey(eId);
        }
    }
}