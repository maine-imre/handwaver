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
        public static string ObjectFromID(int id)
        {
            return GeoElementDataBase.GetElement(id).ElementName.ToString();
        }

        public static string Float3Value(float3 f)
        {
            return $"{f.x}, {f.y}, {f.z}";
        }

        private void Start()
        {
            ins = this;
        }

        #region AffineRatio

        public static void AffineRatio(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "AffineRatio(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void AffineRatio(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AffineRatio(" + input + ")"));
        }

        #endregion

        #region Angle

        public static void Angle(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Angle(" + ObjectFromID(idA) + ")"));
        }

        public static void Angle(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Angle(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Angle(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Angle(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Angle(" + input + ")"));
        }

        #endregion

        #region AngleBisector

        public static void AngleBisector(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("AngleBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void AngleBisector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "AngleBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void AngleBisector(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AngleBisector(" + input + ")"));
        }

        #endregion

        #region Arc

        public static void Arc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Arc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Arc(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Arc(" + input + ")"));
        }

        #endregion

        #region AreCollinear

        public static void AreCollinear(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "AreCollinear(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void AreCollinear(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreCollinear(" + input + ")"));
        }

        #endregion

        #region AreConcurrent

        public static void AreConcurrent(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcurrent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void AreConcurrent(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreConcurrent(" + input + ")"));
        }

        #endregion

        #region AreConcyclic

        public static void AreConcyclic(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcyclic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void AreConcyclic(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreConcyclic(" + input + ")"));
        }

        #endregion

        #region AreCongruent

        public static void AreCongruent(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "AreCongruent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void AreCongruent(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreCongruent(" + input + ")"));
        }

        #endregion

        #region AreEqual

        public static void AreEqual(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("AreEqual(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void AreEqual(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreEqual(" + input + ")"));
        }

        #endregion

        #region AreParallel

        public static void AreParallel(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("AreParallel(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void AreParallel(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" AreParallel(" + input + ")"));
        }

        #endregion

        #region ArePerpendicular

        public static void ArePerpendicular(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("ArePerpendicular(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void ArePerpendicular(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" ArePerpendicular(" + input + ")"));
        }

        #endregion

        #region Area

        public static void Area(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Area(" + ObjectFromID(idA) + ")"));
        }

        public static void Area(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Area(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Area(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Area(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public static void Area(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Area(" + input + ")"));
        }

        #endregion

        #region Barycenter

        public static void Barycenter(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Barycenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void Barycenter(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Barycenter(" + input + ")"));
        }

        #endregion

        #region Centroid

        public static void Centroid(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Centroid(" + ObjectFromID(idA) + ")"));
        }

        public static void Centroid(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Centroid(" + input + ")"));
        }

        #endregion

        #region CircularArc

        public static void CircularArc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularArc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void CircularArc(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" CircularArc(" + input + ")"));
        }

        #endregion

        #region CircularSector

        public static void CircularSector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularSector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void CircularSector(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" CircularSector(" + input + ")"));
        }

        #endregion

        #region CircumcircularArc

        public static void CircumcircularArc(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularArc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void CircumcircularArc(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" CircumcircularArc(" + input + ")"));
        }

        #endregion

        #region CircumcircularSector

        public static void CircumcircularSector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularSector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void CircumcircularSector(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" CircumcircularSector(" + input + ")"));
        }

        #endregion

        #region Circumference

        public static void Circumference(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Circumference(" + ObjectFromID(idA) + ")"));
        }

        public static void Circumference(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Circumference(" + input + ")"));
        }

        #endregion

        #region ClosestPoint

        public static void ClosestPoint(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "ClosestPoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void ClosestPoint(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" ClosestPoint(" + input + ")"));
        }

        #endregion

        #region ClosestPointRegion

        public static void ClosestPointRegion(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("ClosestPointRegion(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public static void ClosestPointRegion(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" ClosestPointRegion(" + input + ")"));
        }

        #endregion

        #region CrossRatio

        public static void CrossRatio(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "CrossRatio(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void CrossRatio(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" CrossRatio(" + input + ")"));
        }

        #endregion

        #region Cubic

        public static void Cubic(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Cubic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Cubic(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Cubic(" + input + ")"));
        }

        #endregion

        #region Direction

        public static void Direction(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Direction(" + ObjectFromID(idA) + ")"));
        }

        public static void Direction(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Direction(" + input + ")"));
        }

        #endregion

        #region Distance

        public static void Distance(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Distance(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Distance(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Distance(" + input + ")"));
        }

        #endregion

        #region Envelope

        public static void Envelope(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Envelope(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Envelope(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Envelope(" + input + ")"));
        }

        #endregion

        #region Intersect

        public static void Intersect(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Intersect(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Intersect(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Intersect(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Intersect(" + input + ")"));
        }

        #endregion

        #region IntersectPath

        public static void IntersectPath(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("IntersectPath(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void IntersectPath(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" IntersectPath(" + input + ")"));
        }

        #endregion

        #region Intersection

        public static void Intersection(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "Intersection(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Intersection(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Intersection(" + input + ")"));
        }

        #endregion

        #region Length

        public static void Length(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Length(" + ObjectFromID(idA) + ")"));
        }

        public static void Length(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Length(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Length(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Length(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Length(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Length(" + input + ")"));
        }

        #endregion

        #region Line

        public static void Line(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Line(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Line(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Line(" + input + ")"));
        }

        #endregion

        #region Locus

        public static void Locus(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Locus(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Locus(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Locus(" + input + ")"));
        }

        #endregion

        #region LocusEquation

        public static void LocusEquation(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("LocusEquation(" + ObjectFromID(idA) + ")"));
        }

        public static void LocusEquation(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("LocusEquation(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public static void LocusEquation(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" LocusEquation(" + input + ")"));
        }

        #endregion

        #region Midpoint

        public static void Midpoint(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + ")"));
        }

        public static void Midpoint(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Midpoint(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Midpoint(" + input + ")"));
        }

        #endregion

        #region Perimeter

        public static void Perimeter(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Perimeter(" + ObjectFromID(idA) + ")"));
        }

        public static void Perimeter(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Perimeter(" + input + ")"));
        }

        #endregion

        #region PerpendicularBisector

        public static void PerpendicularBisector(int idA)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularBisector(" + ObjectFromID(idA) + ")"));
        }

        public static void PerpendicularBisector(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularBisector(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public static void PerpendicularBisector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "PerpendicularBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) +
                ")"));
        }

        public static void PerpendicularBisector(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" PerpendicularBisector(" + input + ")"));
        }

        #endregion

        #region PerpendicularLine

        public static void PerpendicularLine(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularLine(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public static void PerpendicularLine(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "PerpendicularLine(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void PerpendicularLine(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" PerpendicularLine(" + input + ")"));
        }

        #endregion

        #region Point

        public static void Point(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + ")"));
        }

        public static void Point(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Point(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Point(" + input + ")"));
        }

        #endregion

        #region PointIn

        public static void PointIn(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("PointIn(" + ObjectFromID(idA) + ")"));
        }

        public static void PointIn(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" PointIn(" + input + ")"));
        }

        #endregion

        #region Polygon

        public static void Polygon(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Polygon(" + ObjectFromID(idA) + ")"));
        }

        public static void Polygon(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Polygon(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public static void Polygon(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Polygon(" + input + ")"));
        }

        #endregion

        #region Polyline

        public static void Polyline(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Polyline(" + ObjectFromID(idA) + ")"));
        }

        public static void Polyline(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void Polyline(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public static void Polyline(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Polyline(" + input + ")"));
        }

        #endregion

        #region Prove

        public static void Prove(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Prove(" + ObjectFromID(idA) + ")"));
        }

        public static void Prove(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Prove(" + input + ")"));
        }

        #endregion

        #region ProveDetails

        public static void ProveDetails(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("ProveDetails(" + ObjectFromID(idA) + ")"));
        }

        public static void ProveDetails(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" ProveDetails(" + input + ")"));
        }

        #endregion

        #region Radius

        public static void Radius(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Radius(" + ObjectFromID(idA) + ")"));
        }

        public static void Radius(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Radius(" + input + ")"));
        }

        #endregion

        #region Ray

        public static void Ray(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Ray(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Ray(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Ray(" + input + ")"));
        }

        #endregion

        #region RigidPolygon

        public static void RigidPolygon(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("RigidPolygon(" + ObjectFromID(idA) + ")"));
        }

        public static void RigidPolygon(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void RigidPolygon(int idA, int idB, int idC, int idD, int idE)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public static void RigidPolygon(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" RigidPolygon(" + input + ")"));
        }

        #endregion

        #region Sector

        public static void Sector(int idA, int idB, int idC)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Sector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public static void Sector(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Sector(" + input + ")"));
        }

        #endregion

        #region Segment

        public static void Segment(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Segment(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Segment(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Segment(" + input + ")"));
        }

        #endregion

        #region Slope

        public static void Slope(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Slope(" + ObjectFromID(idA) + ")"));
        }

        public static void Slope(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Slope(" + input + ")"));
        }

        #endregion

        #region Tangent

        public static void Tangent(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Tangent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Tangent(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Tangent(" + input + ")"));
        }

        #endregion

        #region TriangleCenter

        public static void TriangleCenter(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void TriangleCenter(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" TriangleCenter(" + input + ")"));
        }

        #endregion

        #region TriangleCurve

        public static void TriangleCurve(int idA, int idB, int idC, int idD)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCurve(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public static void TriangleCurve(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" TriangleCurve(" + input + ")"));
        }

        #endregion

        #region Trilinear

        public static void Trilinear(int idA, int idB, int idC, int idD, int idE, int idF)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(
                "Trilinear(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + "," + ObjectFromID(idF) + ")"));
        }

        public static void Trilinear(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Trilinear(" + input + ")"));
        }

        #endregion

        #region Vertex

        public static void Vertex(int idA)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + ")"));
        }

        public static void Vertex(int idA, int idB)
        {
            ins.StartCoroutine(
                HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public static void Vertex(string input)
        {
            ins.StartCoroutine(HandWaverServerTransport.execCommand(" Vertex(" + input + ")"));
        }

        #endregion
    }
}