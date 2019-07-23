using System;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.Geos{
  
  [Serializable]
  public struct GeoElement : IComponentData
  {
    private int elementID;
    private string elementName;
    private DateTime updated;
  }
}
