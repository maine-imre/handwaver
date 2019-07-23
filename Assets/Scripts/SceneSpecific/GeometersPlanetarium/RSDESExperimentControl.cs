namespace IMRE.HandWaver.Space
{
    /// <summary>
    ///     A control for data collection in the Geometer's Planetarium Scene.
    /// </summary>
    public class RSDESExperimentControl : UnityEngine.MonoBehaviour
    {
        private RSDESManager man => RSDESManager.ins;

        private System.Collections.Generic.List<pinData> pins
        {
            get
            {
                //Creates list and adds pins that are placed to that list
                return System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(man.PinnedPoints,
                    p => p.pin.myPintype == RSDESPin.pintype.Star));
            }
        }
    }
}