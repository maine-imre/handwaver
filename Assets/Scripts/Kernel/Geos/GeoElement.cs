using System;
using System.ComponentModel;
using Unity.Entities;

namespace IMRE.HandWaver.Kernel.Geos
{
    [Serializable]
    public struct GeoElement : IComponentData
    {
        internal int ElementId;
        internal DateTime Updated;
        internal NativeString64 ElementName;

        /// <summary>
        /// Cosntructor for an element
        /// </summary>
        /// <param name="eId"> element id from ggb server</param>
        /// <param name="eName">element name from ggb server. Max 30 characters.</param>
        internal GeoElement(int eId, NativeString64 eName)
        {
            ElementId = eId;
            ElementName = eName;
            Updated = DateTime.Now;
            GeoElementDataBase.AddElement(this);
        }
    }
}