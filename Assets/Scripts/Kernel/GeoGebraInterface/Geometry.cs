using IMRE.HandWaver.Kernel.Geos;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.GGBFunctions
{
    /// <summary>
    ///     Geometry functions to be used within Geogebra session
    /// </summary>
    /// TODO:  Comment
    public class Geometry : MonoBehaviour
    {
        /// <summary>
        /// Creates static reference to instance within scene.
        /// </summary>
        private static Geometry ins;
        
        //TODO: Pull from specific class into generic helper functions
        public static string ObjectFromID(int id) => GeoElementDataBase.GetElement(id).ElementName.ToString();

        public static string Float3Value(float3 f) => $"{f.x}, {f.y}, {f.z}";
        
        private void Start()
        {
            ins = this;
        }

        #region AffineRatio

        public static void AffineRatio(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AffineRatio(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void AffineRatio(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AffineRatio(" + input + ")"));
        }

        #endregion

        #region Angle

        public static void Angle(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Angle(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Angle(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Angle(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Angle(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Angle(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Angle(" + input + ")"));
        }

        #endregion

        #region AngleBisector

        public static void AngleBisector(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AngleBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void AngleBisector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AngleBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void AngleBisector(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AngleBisector(" + input + ")"));
        }

        #endregion

        #region Arc

        public static void Arc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Arc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Arc(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Arc(" + input + ")"));
        }

        #endregion

        #region AreCollinear

        public static void AreCollinear(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreCollinear(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void AreCollinear(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreCollinear(" + input + ")"));
        }

        #endregion

        #region AreConcurrent

        public static void AreConcurrent(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreConcurrent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void AreConcurrent(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreConcurrent(" + input + ")"));
        }

        #endregion

        #region AreConcyclic

        public static void AreConcyclic(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreConcyclic(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void AreConcyclic(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreConcyclic(" + input + ")"));
        }

        #endregion

        #region AreCongruent

        public static void AreCongruent(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "AreCongruent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void AreCongruent(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreCongruent(" + input + ")"));
        }

        #endregion

        #region AreEqual

        public static void AreEqual(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AreEqual(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void AreEqual(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreEqual(" + input + ")"));
        }

        #endregion

        #region AreParallel

        public static void AreParallel(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AreParallel(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void AreParallel(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreParallel(" + input + ")"));
        }

        #endregion

        #region ArePerpendicular

        public static void ArePerpendicular(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "ArePerpendicular(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void ArePerpendicular(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ArePerpendicular(" + input + ")"));
        }

        #endregion

        #region Area

        public static void Area(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Area(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Area(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Area(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Area(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Area(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public static void Area(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Area(" + input + ")"));
        }

        #endregion

        #region Barycenter

        public static void Barycenter(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Barycenter(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void Barycenter(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Barycenter(" + input + ")"));
        }

        #endregion

        #region Centroid

        public static void Centroid(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Centroid(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Centroid(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Centroid(" + input + ")"));
        }

        #endregion

        #region CircularArc

        public static void CircularArc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircularArc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void CircularArc(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircularArc(" + input + ")"));
        }

        #endregion

        #region CircularSector

        public static void CircularSector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircularSector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void CircularSector(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircularSector(" + input + ")"));
        }

        #endregion

        #region CircumcircularArc

        public static void CircumcircularArc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircumcircularArc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void CircumcircularArc(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircumcircularArc(" + input + ")"));
        }

        #endregion

        #region CircumcircularSector

        public static void CircumcircularSector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircumcircularSector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void CircumcircularSector(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircumcircularSector(" + input + ")"));
        }

        #endregion

        #region Circumference

        public static void Circumference(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Circumference(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Circumference(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Circumference(" + input + ")"));
        }

        #endregion

        #region ClosestPoint

        public static void ClosestPoint(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "ClosestPoint(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void ClosestPoint(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ClosestPoint(" + input + ")"));
        }

        #endregion

        #region ClosestPointRegion

        public static void ClosestPointRegion(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "ClosestPointRegion(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public static void ClosestPointRegion(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ClosestPointRegion(" + input + ")"));
        }

        #endregion

        #region CrossRatio

        public static void CrossRatio(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CrossRatio(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void CrossRatio(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CrossRatio(" + input + ")"));
        }

        #endregion

        #region Cubic

        public static void Cubic(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Cubic(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Cubic(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Cubic(" + input + ")"));
        }

        #endregion

        #region Direction

        public static void Direction(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Direction(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Direction(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Direction(" + input + ")"));
        }

        #endregion

        #region Distance

        public static void Distance(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Distance(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Distance(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Distance(" + input + ")"));
        }

        #endregion

        #region Envelope

        public static void Envelope(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Envelope(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Envelope(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Envelope(" + input + ")"));
        }

        #endregion

        #region Intersect

        public static void Intersect(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Intersect(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Intersect(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Intersect(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Intersect(" + input + ")"));
        }

        #endregion

        #region IntersectPath

        public static void IntersectPath(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "IntersectPath(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void IntersectPath(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " IntersectPath(" + input + ")"));
        }

        #endregion

        #region Intersection

        public static void Intersection(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "Intersection(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Intersection(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Intersection(" + input + ")"));
        }

        #endregion

        #region Length

        public static void Length(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Length(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Length(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Length(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Length(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Length(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Length(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Length(" + input + ")"));
        }

        #endregion

        #region Line

        public static void Line(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Line(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Line(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Line(" + input + ")"));
        }

        #endregion

        #region Locus

        public static void Locus(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Locus(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Locus(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Locus(" + input + ")"));
        }

        #endregion

        #region LocusEquation

        public static void LocusEquation(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "LocusEquation(" + ObjectFromID(id: idA) + ")"));
        }

        public static void LocusEquation(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "LocusEquation(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public static void LocusEquation(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " LocusEquation(" + input + ")"));
        }

        #endregion

        #region Midpoint

        public static void Midpoint(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Midpoint(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Midpoint(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Midpoint(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Midpoint(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Midpoint(" + input + ")"));
        }

        #endregion

        #region Perimeter

        public static void Perimeter(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Perimeter(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Perimeter(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Perimeter(" + input + ")"));
        }

        #endregion

        #region PerpendicularBisector

        public static void PerpendicularBisector(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + ")"));
        }

        public static void PerpendicularBisector(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public static void PerpendicularBisector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) +
                ")"));
        }

        public static void PerpendicularBisector(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PerpendicularBisector(" + input + ")"));
        }

        #endregion

        #region PerpendicularLine

        public static void PerpendicularLine(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularLine(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public static void PerpendicularLine(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "PerpendicularLine(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void PerpendicularLine(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PerpendicularLine(" + input + ")"));
        }

        #endregion

        #region Point

        public static void Point(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Point(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Point(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Point(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Point(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Point(" + input + ")"));
        }

        #endregion

        #region PointIn

        public static void PointIn(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "PointIn(" + ObjectFromID(id: idA) + ")"));
        }

        public static void PointIn(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PointIn(" + input + ")"));
        }

        #endregion

        #region Polygon

        public static void Polygon(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Polygon(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Polygon(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public static void Polygon(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Polygon(" + input + ")"));
        }

        #endregion

        #region Polyline

        public static void Polyline(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Polyline(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Polyline(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public static void Polyline(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Polyline(" + input + ")"));
        }

        #endregion

        #region Prove

        public static void Prove(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Prove(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Prove(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Prove(" + input + ")"));
        }

        #endregion

        #region ProveDetails

        public static void ProveDetails(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "ProveDetails(" + ObjectFromID(id: idA) + ")"));
        }

        public static void ProveDetails(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ProveDetails(" + input + ")"));
        }

        #endregion

        #region Radius

        public static void Radius(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Radius(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Radius(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Radius(" + input + ")"));
        }

        #endregion

        #region Ray

        public static void Ray(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Ray(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Ray(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Ray(" + input + ")"));
        }

        #endregion

        #region RigidPolygon

        public static void RigidPolygon(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + ")"));
        }

        public static void RigidPolygon(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public static void RigidPolygon(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " RigidPolygon(" + input + ")"));
        }

        #endregion

        #region Sector

        public static void Sector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Sector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public static void Sector(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Sector(" + input + ")"));
        }

        #endregion

        #region Segment

        public static void Segment(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Segment(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Segment(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Segment(" + input + ")"));
        }

        #endregion

        #region Slope

        public static void Slope(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Slope(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Slope(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Slope(" + input + ")"));
        }

        #endregion

        #region Tangent

        public static void Tangent(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Tangent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Tangent(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Tangent(" + input + ")"));
        }

        #endregion

        #region TriangleCenter

        public static void TriangleCenter(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "TriangleCenter(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void TriangleCenter(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " TriangleCenter(" + input + ")"));
        }

        #endregion

        #region TriangleCurve

        public static void TriangleCurve(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "TriangleCurve(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public static void TriangleCurve(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " TriangleCurve(" + input + ")"));
        }

        #endregion

        #region Trilinear

        public static void Trilinear(int idA, int idB, int idC, int idD, int idE, int idF)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Trilinear(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + "," + ObjectFromID(id: idF) + ")"));
        }

        public static void Trilinear(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Trilinear(" + input + ")"));
        }

        #endregion

        #region Vertex

        public static void Vertex(int idA)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Vertex(" + ObjectFromID(id: idA) + ")"));
        }

        public static void Vertex(int idA, int idB)
        {
            ins.StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Vertex(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public static void Vertex(string input)
        {
            ins.StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Vertex(" + input + ")"));
        }

        #endregion
    }
}