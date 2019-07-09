using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.GGBFunctions
{
    
    
    /// <summary>
    /// Geometry functions to be used within Geogebra session
    /// </summary>
    public class Geometry : MonoBehaviour
    {
        /// <summary>
        ///  Returns the affine ratio λ of three collinear points A, B and C, where C = A + λ * AB.
        /// </summary>
        /// <param name="idPointA"></param>
        /// <param name="idPointB"></param>
        /// <param name="idPointC"></param>
        public void AffineRatio(int idPointA, int idPointB, int idPointC) =>
            StartCoroutine(HandWaverServerTransport.execCommand("AffineRatio(" +idPointA+","+idPointB+","+idPointC+")"));

        /// <param name="idA">Object</param>
        public void Angle(int idA) => StartCoroutine(HandWaverServerTransport.execCommand("Angle(" +ObjectFromID(idA)+")"));

        /// <param name="idA">Vector, Line, Plane</param>
        /// <param name="idB">Vector, Line, Plane</param>
        public void Angle(int idA, int idB) => StartCoroutine(HandWaverServerTransport.execCommand("Angle(" +ObjectFromID(idA)+","+ObjectFromID(idB)+")"));
        
        /// <param name="idA">Point</param>
        /// <param name="idB">Apex</param>
        /// <param name="idC">Point, Angle</param>
        public void Angle(int idA, int idB, int idC) => StartCoroutine(HandWaverServerTransport.execCommand("Angle(" +ObjectFromID(idA)+","+ObjectFromID(idB)+","+ObjectFromID(idC)+")"));
        
        /// <param name="idA">Point</param>
        /// <param name="idB">Point</param>
        /// <param name="idC">Point</param>
        /// <param name="idD">Direction</param>
        public void Angle(int idA, int idB, int idC, int idD) => StartCoroutine(HandWaverServerTransport.execCommand("Angle(" +ObjectFromID(idA)+","+ObjectFromID(idB)+","+ObjectFromID(idC)+","+ObjectFromID(idD)+")"));

        /// <param name="idA">Line</param>
        /// <param name="idB">Line</param>
        public void AngleBisector(int idA, int idB) => StartCoroutine(HandWaverServerTransport.execCommand("AngleBisector(" +ObjectFromID(idA)+","+ObjectFromID(idB)+")"));
        
        /// <param name="idA">Point</param>
        /// <param name="idB">Point</param>
        /// <param name="idC">Point</param>
        public void AngleBisector(int idA, int idB, int idC) => StartCoroutine(HandWaverServerTransport.execCommand("AngleBisector(" +ObjectFromID(idA)+","+ObjectFromID(idB)+","+ObjectFromID(idC)+")"));

        //to be continued at Arc         https://wiki.geogebra.org/en/Geometry_Commands
        
        
        
        
        //TODO: Pull from specific class into generic helper functions
        public string ObjectFromID(int id) => ""; //TODO: get object associated with that id.
        
    }
}