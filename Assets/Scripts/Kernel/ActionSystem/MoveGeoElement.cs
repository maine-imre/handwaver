using System;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.ActionSystem;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Kernel.GGBFunctions;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.ActionSystem
{
    public class MoveGeoElement : GeoElementFunction
    {
        public override void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier)
        {
                switch (geo.Type)
                {
                    case ElementType.point:
                        Geometry.Point(Geometry.Float3Value(classifier.origin));
                        break;
                    case ElementType.line:
                        //design discussion needed here
                        //Geometry.Line(Geometry.Float3Value(classifier.origin), );
                        break;
                    case ElementType.plane:
                        break;
                    case ElementType.sphere:
                        break;
                    case ElementType.circle:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
    }
}