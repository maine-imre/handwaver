using System;
using System.ComponentModel;
using Unity.Entities;
using Unity.Mathematics;

namespace IMRE.HandWaver.Kernel.Geos
{
    enum ElementType
    {
        err,
        point,
        line,
        plane,
        sphere,
        circle
    }
    
    
    [Serializable]
    public struct GeoElement : IComponentData
    {
        /// <summary>
        /// Integer ID for the object. This should be readonly after creation.
        /// </summary>
        internal int ElementId;
        
        /// <summary>
        /// The last time this element was updated.
        /// </summary>
        internal DateTime Updated;
        
        /// <summary>
        /// Element name. (Max 30 characters)
        /// </summary>
        internal NativeString64 ElementName;

        /// <summary>
        /// The type of the element.
        /// If uninitialized, will be error value.
        /// On first element update this will be set and assumed correct afterwards.
        /// </summary>
        internal ElementType type;
        
        /// <summary>
        /// Up to 4 dependencies can be stored by ID here.
        /// </summary>
        internal int4 deps;
        
        /// <summary>
        /// Any relevant float3 data can be stored here.
        /// </summary>
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