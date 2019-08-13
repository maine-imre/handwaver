using System;
using System.ComponentModel;
using Unity.Entities;
using Unity.Mathematics;

namespace IMRE.HandWaver.Kernel.Geos
{
    [Serializable]
    public struct GeoElement : IComponentData
    {
        internal int ElementId;
        internal DateTime Updated;
        internal NativeString64 ElementName;
        
        
        internal int4 deps;
        internal float3 f0;
        
        /// <summary>
        /// Cosntructor for an element
        /// </summary>
        /// <param name="eId"> element id from ggb server</param>
        /// <param name="eName">element name from ggb server. Max 30 characters.</param>
        internal GeoElement(int eId, NativeString64 eName) : this()
        {
            ElementId = eId;
            ElementName = eName;
            Updated = DateTime.Now;
            GeoElementDataBase.AddElement(this);
        }
    }
}