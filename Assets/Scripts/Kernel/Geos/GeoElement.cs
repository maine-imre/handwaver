using System;
using Unity.Entities;

namespace IMRE.HandWaver.Kernel.Geos
{
    [Serializable]
    public struct GeoElement : IComponentData
    {
        private int elementID;
        private string elementName;
        private DateTime updated;
    }
}