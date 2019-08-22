using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
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
        internal int ElementId { 
            get => _elementId; 
            set => _elementId = value; 
        }

        private int _elementId;



        /// <summary>
        /// The last time this element was updated.
        /// </summary>
        internal DateTime Updated
        {
            get => _updated;
            set => _updated = value;
        }

        private DateTime _updated;


        /// <summary>
        /// Element name. (Max 30 characters)
        /// </summary>
        internal NativeString64 ElementName
        {
            get => _elementName;
            set => _elementName = value;
        }

        private NativeString64 _elementName;



        /// <summary>
        /// The type of the element.
        /// If uninitialized, will be error value.
        /// On first element update this will be set and assumed correct afterwards.
        /// </summary>
        internal ElementType Type
        {
            get => _type;
            set => _type = value;
        }

        private ElementType _type;


        /// <summary>
        /// Up to 4 dependencies can be stored by ID here.
        /// </summary>
        internal int4 Deps
        {
            get => _deps;
            set => _deps = value;
        }

        private int4 _deps;

        /// <summary>
        /// Any relevant float3 data can be stored here.
        /// </summary>
        internal float3 F0
        {
            get => _f0;
            set => _f0 = value;
        }

        private float3 _f0;
        
        
        
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
        }

        public override string ToString()
        {
            return $"{_elementName} element :: {_elementId} Id, {_type} Type, {_deps} Deps, {_f0} F0\n";
        }
    }
}