using System;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.Geos{
  public interface GeoElement
  {
    int ElementID { get; set; }
    string ElementName { get; set; }
  }
}
