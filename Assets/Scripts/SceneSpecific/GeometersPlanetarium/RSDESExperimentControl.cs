using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
    /// <summary>
    ///     A control for data collection in the Geometer's Planetarium Scene.
    /// </summary>
    public class RSDESExperimentControl : MonoBehaviour
    {
        private RSDESManager man => RSDESManager.ins;

        private List<pinData> pins
        {
            get
            {
                //Creates list and adds pins that are placed to that list
                return Enumerable.ToList(Enumerable.Where(man.PinnedPoints,
                    p => p.pin.myPintype == RSDESPin.pintype.Star));
            }
        }
    }
}