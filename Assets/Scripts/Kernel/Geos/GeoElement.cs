using System;
using UnityEngine;
using Unity.Rendering;

namespace IMRE.HandWaver.Kernel.Geos{
  public abstract class GeoElement
  {
    public int ElementID;
    public abstract string ElementName;
    public abstract Vector3 closestPoint(Vector3 position);
    public abstract void RenderFigure();
  }
}
