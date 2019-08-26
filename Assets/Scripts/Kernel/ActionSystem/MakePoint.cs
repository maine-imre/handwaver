using System.Reflection;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.ActionSystem;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Kernel.GGBFunctions;

namespace IMRE.HandWaver.Kernel.ActionSystem
{
    public class MakePoint : GeoElementFunction
    {
        public override void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier)
        {
            Geometry.Point("" + classifier.origin.ToString());
            //HandWaverServerTransport.execCommand("Point(" + classifier.origin.ToString() + ")");
        }
    }
}