using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A script to manage the Scale and Dimension study.  (See Dimmel's Proposal, Appendix B)
    /// </summary>
    public class crossSectionsScaleStudy : MonoBehaviour
    {
        public enum figureType {circle,annulus,sphere,torus,hypersphere,hypertorus};
        public figureType myFigureType;
    }

}