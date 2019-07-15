using UnityEngine;

namespace IMRE.HandWaver.Kernel.GGBFunctions
{
    /// <summary>
    ///     Geometry functions to be used within Geogebra session
    /// </summary>
    /// TODO:  Comment
    public class Geometry : MonoBehaviour
    {
        //TODO: Pull from specific class into generic helper functions
        public string ObjectFromID(int id)
        {
            return "";
        }

        #region AffineRatio

        public void AffineRatio(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AffineRatio(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AffineRatio(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AffineRatio(" + input + ")"));
        }

        #endregion

        #region Angle

        public void Angle(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Angle(" + ObjectFromID(idA) + ")"));
        }

        public void Angle(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Angle(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Angle(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Angle(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Angle(" + input + ")"));
        }

        #endregion

        #region AngleBisector

        public void AngleBisector(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AngleBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void AngleBisector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AngleBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AngleBisector(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AngleBisector(" + input + ")"));
        }

        #endregion

        #region Arc

        public void Arc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Arc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Arc(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Arc(" + input + ")"));
        }

        #endregion

        #region AreCollinear

        public void AreCollinear(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreCollinear(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AreCollinear(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreCollinear(" + input + ")"));
        }

        #endregion

        #region AreConcurrent

        public void AreConcurrent(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcurrent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AreConcurrent(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreConcurrent(" + input + ")"));
        }

        #endregion

        #region AreConcyclic

        public void AreConcyclic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcyclic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void AreConcyclic(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreConcyclic(" + input + ")"));
        }

        #endregion

        #region AreCongruent

        public void AreCongruent(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "AreCongruent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void AreCongruent(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreCongruent(" + input + ")"));
        }

        #endregion

        #region AreEqual

        public void AreEqual(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AreEqual(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void AreEqual(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreEqual(" + input + ")"));
        }

        #endregion

        #region AreParallel

        public void AreParallel(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AreParallel(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void AreParallel(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" AreParallel(" + input + ")"));
        }

        #endregion

        #region ArePerpendicular

        public void ArePerpendicular(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("ArePerpendicular(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void ArePerpendicular(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" ArePerpendicular(" + input + ")"));
        }

        #endregion

        #region Area

        public void Area(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Area(" + ObjectFromID(idA) + ")"));
        }

        public void Area(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Area(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Area(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Area(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Area(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public void Area(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Area(" + input + ")"));
        }

        #endregion

        #region Barycenter

        public void Barycenter(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Barycenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void Barycenter(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Barycenter(" + input + ")"));
        }

        #endregion

        #region Centroid

        public void Centroid(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Centroid(" + ObjectFromID(idA) + ")"));
        }

        public void Centroid(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Centroid(" + input + ")"));
        }

        #endregion

        #region CircularArc

        public void CircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularArc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircularArc(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" CircularArc(" + input + ")"));
        }

        #endregion

        #region CircularSector

        public void CircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularSector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircularSector(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" CircularSector(" + input + ")"));
        }

        #endregion

        #region CircumcircularArc

        public void CircumcircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularArc(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircumcircularArc(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" CircumcircularArc(" + input + ")"));
        }

        #endregion

        #region CircumcircularSector

        public void CircumcircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularSector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircumcircularSector(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" CircumcircularSector(" + input + ")"));
        }

        #endregion

        #region Circumference

        public void Circumference(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Circumference(" + ObjectFromID(idA) + ")"));
        }

        public void Circumference(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Circumference(" + input + ")"));
        }

        #endregion

        #region ClosestPoint

        public void ClosestPoint(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "ClosestPoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void ClosestPoint(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" ClosestPoint(" + input + ")"));
        }

        #endregion

        #region ClosestPointRegion

        public void ClosestPointRegion(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("ClosestPointRegion(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public void ClosestPointRegion(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" ClosestPointRegion(" + input + ")"));
        }

        #endregion

        #region CrossRatio

        public void CrossRatio(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CrossRatio(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void CrossRatio(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" CrossRatio(" + input + ")"));
        }

        #endregion

        #region Cubic

        public void Cubic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Cubic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Cubic(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Cubic(" + input + ")"));
        }

        #endregion

        #region Direction

        public void Direction(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Direction(" + ObjectFromID(idA) + ")"));
        }

        public void Direction(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Direction(" + input + ")"));
        }

        #endregion

        #region Distance

        public void Distance(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Distance(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Distance(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Distance(" + input + ")"));
        }

        #endregion

        #region Envelope

        public void Envelope(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Envelope(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Envelope(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Envelope(" + input + ")"));
        }

        #endregion

        #region Intersect

        public void Intersect(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Intersect(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Intersect(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Intersect(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Intersect(" + input + ")"));
        }

        #endregion

        #region IntersectPath

        public void IntersectPath(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("IntersectPath(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void IntersectPath(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" IntersectPath(" + input + ")"));
        }

        #endregion

        #region Intersection

        public void Intersection(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "Intersection(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Intersection(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Intersection(" + input + ")"));
        }

        #endregion

        #region Length

        public void Length(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Length(" + ObjectFromID(idA) + ")"));
        }

        public void Length(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Length(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Length(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Length(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Length(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Length(" + input + ")"));
        }

        #endregion

        #region Line

        public void Line(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Line(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Line(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Line(" + input + ")"));
        }

        #endregion

        #region Locus

        public void Locus(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Locus(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Locus(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Locus(" + input + ")"));
        }

        #endregion

        #region LocusEquation

        public void LocusEquation(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("LocusEquation(" + ObjectFromID(idA) + ")"));
        }

        public void LocusEquation(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("LocusEquation(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void LocusEquation(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" LocusEquation(" + input + ")"));
        }

        #endregion

        #region Midpoint

        public void Midpoint(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + ")"));
        }

        public void Midpoint(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Midpoint(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Midpoint(" + input + ")"));
        }

        #endregion

        #region Perimeter

        public void Perimeter(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Perimeter(" + ObjectFromID(idA) + ")"));
        }

        public void Perimeter(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Perimeter(" + input + ")"));
        }

        #endregion

        #region PerpendicularBisector

        public void PerpendicularBisector(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("PerpendicularBisector(" + ObjectFromID(idA) + ")"));
        }

        public void PerpendicularBisector(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularBisector(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public void PerpendicularBisector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "PerpendicularBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) +
                ")"));
        }

        public void PerpendicularBisector(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" PerpendicularBisector(" + input + ")"));
        }

        #endregion

        #region PerpendicularLine

        public void PerpendicularLine(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularLine(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public void PerpendicularLine(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "PerpendicularLine(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void PerpendicularLine(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" PerpendicularLine(" + input + ")"));
        }

        #endregion

        #region Point

        public void Point(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + ")"));
        }

        public void Point(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Point(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Point(" + input + ")"));
        }

        #endregion

        #region PointIn

        public void PointIn(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("PointIn(" + ObjectFromID(idA) + ")"));
        }

        public void PointIn(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" PointIn(" + input + ")"));
        }

        #endregion

        #region Polygon

        public void Polygon(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Polygon(" + ObjectFromID(idA) + ")"));
        }

        public void Polygon(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Polygon(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Polygon(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Polygon(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public void Polygon(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Polygon(" + input + ")"));
        }

        #endregion

        #region Polyline

        public void Polyline(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Polyline(" + ObjectFromID(idA) + ")"));
        }

        public void Polyline(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Polyline(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Polyline(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Polyline(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Polyline(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public void Polyline(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Polyline(" + input + ")"));
        }

        #endregion

        #region Prove

        public void Prove(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Prove(" + ObjectFromID(idA) + ")"));
        }

        public void Prove(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Prove(" + input + ")"));
        }

        #endregion

        #region ProveDetails

        public void ProveDetails(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("ProveDetails(" + ObjectFromID(idA) + ")"));
        }

        public void ProveDetails(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" ProveDetails(" + input + ")"));
        }

        #endregion

        #region Radius

        public void Radius(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Radius(" + ObjectFromID(idA) + ")"));
        }

        public void Radius(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Radius(" + input + ")"));
        }

        #endregion

        #region Ray

        public void Ray(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Ray(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Ray(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Ray(" + input + ")"));
        }

        #endregion

        #region RigidPolygon

        public void RigidPolygon(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("RigidPolygon(" + ObjectFromID(idA) + ")"));
        }

        public void RigidPolygon(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "RigidPolygon(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + ")"));
        }

        public void RigidPolygon(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" RigidPolygon(" + input + ")"));
        }

        #endregion

        #region Sector

        public void Sector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Sector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Sector(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Sector(" + input + ")"));
        }

        #endregion

        #region Segment

        public void Segment(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Segment(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Segment(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Segment(" + input + ")"));
        }

        #endregion

        #region Slope

        public void Slope(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Slope(" + ObjectFromID(idA) + ")"));
        }

        public void Slope(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Slope(" + input + ")"));
        }

        #endregion

        #region Tangent

        public void Tangent(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Tangent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Tangent(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Tangent(" + input + ")"));
        }

        #endregion

        #region TriangleCenter

        public void TriangleCenter(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void TriangleCenter(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" TriangleCenter(" + input + ")"));
        }

        #endregion

        #region TriangleCurve

        public void TriangleCurve(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCurve(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void TriangleCurve(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" TriangleCurve(" + input + ")"));
        }

        #endregion

        #region Trilinear

        public void Trilinear(int idA, int idB, int idC, int idD, int idE, int idF)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Trilinear(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "," + ObjectFromID(idE) + "," + ObjectFromID(idF) + ")"));
        }

        public void Trilinear(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Trilinear(" + input + ")"));
        }

        #endregion

        #region Vertex

        public void Vertex(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + ")"));
        }

        public void Vertex(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Vertex(string input)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(" Vertex(" + input + ")"));
        }

        #endregion
    }
}