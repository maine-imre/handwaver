using System;
using UnityEngine;
using Unity.Rendering;

namespace IMRE.HandWaver.Kernel.Geos{
  public abstract interface GeoElement
  {
    public int ElementID;
    public abstract string ElementName;
  }
}
