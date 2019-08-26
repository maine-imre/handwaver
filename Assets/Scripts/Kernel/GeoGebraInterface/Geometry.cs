using IMRE.HandWaver.Kernel.Geos;
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
        public string ObjectFromID(int id) => GeoElementDataBase.GetElement(id).ElementName.ToString();

        #region AffineRatio

        public void AffineRatio(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AffineRatio(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void AffineRatio(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AffineRatio(" + input + ")"));
        }

        #endregion

        #region Angle

        public void Angle(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Angle(" + ObjectFromID(id: idA) + ")"));
        }

        public void Angle(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Angle(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Angle(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Angle(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Angle(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Angle(" + input + ")"));
        }

        #endregion

        #region AngleBisector

        public void AngleBisector(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AngleBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void AngleBisector(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AngleBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void AngleBisector(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AngleBisector(" + input + ")"));
        }

        #endregion

        #region Arc

        public void Arc(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Arc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Arc(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Arc(" + input + ")"));
        }

        #endregion

        #region AreCollinear

        public void AreCollinear(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreCollinear(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void AreCollinear(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreCollinear(" + input + ")"));
        }

        #endregion

        #region AreConcurrent

        public void AreConcurrent(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreConcurrent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void AreConcurrent(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreConcurrent(" + input + ")"));
        }

        #endregion

        #region AreConcyclic

        public void AreConcyclic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "AreConcyclic(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void AreConcyclic(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreConcyclic(" + input + ")"));
        }

        #endregion

        #region AreCongruent

        public void AreCongruent(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "AreCongruent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void AreCongruent(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreCongruent(" + input + ")"));
        }

        #endregion

        #region AreEqual

        public void AreEqual(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AreEqual(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void AreEqual(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreEqual(" + input + ")"));
        }

        #endregion

        #region AreParallel

        public void AreParallel(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "AreParallel(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void AreParallel(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " AreParallel(" + input + ")"));
        }

        #endregion

        #region ArePerpendicular

        public void ArePerpendicular(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "ArePerpendicular(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void ArePerpendicular(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ArePerpendicular(" + input + ")"));
        }

        #endregion

        #region Area

        public void Area(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Area(" + ObjectFromID(id: idA) + ")"));
        }

        public void Area(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Area(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Area(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Area(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Area(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public void Area(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Area(" + input + ")"));
        }

        #endregion

        #region Barycenter

        public void Barycenter(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Barycenter(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void Barycenter(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Barycenter(" + input + ")"));
        }

        #endregion

        #region Centroid

        public void Centroid(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Centroid(" + ObjectFromID(id: idA) + ")"));
        }

        public void Centroid(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Centroid(" + input + ")"));
        }

        #endregion

        #region CircularArc

        public void CircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircularArc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void CircularArc(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircularArc(" + input + ")"));
        }

        #endregion

        #region CircularSector

        public void CircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircularSector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void CircularSector(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircularSector(" + input + ")"));
        }

        #endregion

        #region CircumcircularArc

        public void CircumcircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircumcircularArc(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void CircumcircularArc(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircumcircularArc(" + input + ")"));
        }

        #endregion

        #region CircumcircularSector

        public void CircumcircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CircumcircularSector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void CircumcircularSector(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CircumcircularSector(" + input + ")"));
        }

        #endregion

        #region Circumference

        public void Circumference(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Circumference(" + ObjectFromID(id: idA) + ")"));
        }

        public void Circumference(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Circumference(" + input + ")"));
        }

        #endregion

        #region ClosestPoint

        public void ClosestPoint(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "ClosestPoint(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void ClosestPoint(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ClosestPoint(" + input + ")"));
        }

        #endregion

        #region ClosestPointRegion

        public void ClosestPointRegion(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "ClosestPointRegion(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public void ClosestPointRegion(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ClosestPointRegion(" + input + ")"));
        }

        #endregion

        #region CrossRatio

        public void CrossRatio(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "CrossRatio(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void CrossRatio(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " CrossRatio(" + input + ")"));
        }

        #endregion

        #region Cubic

        public void Cubic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Cubic(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Cubic(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Cubic(" + input + ")"));
        }

        #endregion

        #region Direction

        public void Direction(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Direction(" + ObjectFromID(id: idA) + ")"));
        }

        public void Direction(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Direction(" + input + ")"));
        }

        #endregion

        #region Distance

        public void Distance(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Distance(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Distance(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Distance(" + input + ")"));
        }

        #endregion

        #region Envelope

        public void Envelope(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Envelope(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Envelope(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Envelope(" + input + ")"));
        }

        #endregion

        #region Intersect

        public void Intersect(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Intersect(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Intersect(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Intersect(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Intersect(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Intersect(" + input + ")"));
        }

        #endregion

        #region IntersectPath

        public void IntersectPath(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "IntersectPath(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void IntersectPath(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " IntersectPath(" + input + ")"));
        }

        #endregion

        #region Intersection

        public void Intersection(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "Intersection(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Intersection(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Intersection(" + input + ")"));
        }

        #endregion

        #region Length

        public void Length(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Length(" + ObjectFromID(id: idA) + ")"));
        }

        public void Length(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Length(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Length(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Length(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Length(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Length(" + input + ")"));
        }

        #endregion

        #region Line

        public void Line(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Line(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Line(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Line(" + input + ")"));
        }

        #endregion

        #region Locus

        public void Locus(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Locus(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Locus(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Locus(" + input + ")"));
        }

        #endregion

        #region LocusEquation

        public void LocusEquation(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "LocusEquation(" + ObjectFromID(id: idA) + ")"));
        }

        public void LocusEquation(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "LocusEquation(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) +
                                                     ")"));
        }

        public void LocusEquation(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " LocusEquation(" + input + ")"));
        }

        #endregion

        #region Midpoint

        public void Midpoint(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Midpoint(" + ObjectFromID(id: idA) + ")"));
        }

        public void Midpoint(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Midpoint(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Midpoint(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Midpoint(" + input + ")"));
        }

        #endregion

        #region Perimeter

        public void Perimeter(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Perimeter(" + ObjectFromID(id: idA) + ")"));
        }

        public void Perimeter(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Perimeter(" + input + ")"));
        }

        #endregion

        #region PerpendicularBisector

        public void PerpendicularBisector(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + ")"));
        }

        public void PerpendicularBisector(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public void PerpendicularBisector(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "PerpendicularBisector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) +
                ")"));
        }

        public void PerpendicularBisector(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PerpendicularBisector(" + input + ")"));
        }

        #endregion

        #region PerpendicularLine

        public void PerpendicularLine(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "PerpendicularLine(" + ObjectFromID(id: idA) + "," +
                                                     ObjectFromID(id: idB) + ")"));
        }

        public void PerpendicularLine(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "PerpendicularLine(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void PerpendicularLine(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PerpendicularLine(" + input + ")"));
        }

        #endregion

        #region Point

        public void Point(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Point(" + ObjectFromID(id: idA) + ")"));
        }

        public void Point(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Point(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Point(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Point(" + input + ")"));
        }

        #endregion

        #region PointIn

        public void PointIn(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "PointIn(" + ObjectFromID(id: idA) + ")"));
        }

        public void PointIn(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " PointIn(" + input + ")"));
        }

        #endregion

        #region Polygon

        public void Polygon(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Polygon(" + ObjectFromID(id: idA) + ")"));
        }

        public void Polygon(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Polygon(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Polygon(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Polygon(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public void Polygon(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Polygon(" + input + ")"));
        }

        #endregion

        #region Polyline

        public void Polyline(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Polyline(" + ObjectFromID(id: idA) + ")"));
        }

        public void Polyline(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Polyline(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Polyline(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void Polyline(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Polyline(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public void Polyline(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Polyline(" + input + ")"));
        }

        #endregion

        #region Prove

        public void Prove(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Prove(" + ObjectFromID(id: idA) + ")"));
        }

        public void Prove(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Prove(" + input + ")"));
        }

        #endregion

        #region ProveDetails

        public void ProveDetails(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "ProveDetails(" + ObjectFromID(id: idA) + ")"));
        }

        public void ProveDetails(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " ProveDetails(" + input + ")"));
        }

        #endregion

        #region Radius

        public void Radius(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Radius(" + ObjectFromID(id: idA) + ")"));
        }

        public void Radius(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Radius(" + input + ")"));
        }

        #endregion

        #region Ray

        public void Ray(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Ray(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Ray(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Ray(" + input + ")"));
        }

        #endregion

        #region RigidPolygon

        public void RigidPolygon(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + ")"));
        }

        public void RigidPolygon(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(
                    cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void RigidPolygon(int idA, int idB, int idC, int idD, int idE)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "RigidPolygon(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + ")"));
        }

        public void RigidPolygon(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " RigidPolygon(" + input + ")"));
        }

        #endregion

        #region Sector

        public void Sector(int idA, int idB, int idC)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Sector(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + ")"));
        }

        public void Sector(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Sector(" + input + ")"));
        }

        #endregion

        #region Segment

        public void Segment(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Segment(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Segment(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Segment(" + input + ")"));
        }

        #endregion

        #region Slope

        public void Slope(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Slope(" + ObjectFromID(id: idA) + ")"));
        }

        public void Slope(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Slope(" + input + ")"));
        }

        #endregion

        #region Tangent

        public void Tangent(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Tangent(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Tangent(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Tangent(" + input + ")"));
        }

        #endregion

        #region TriangleCenter

        public void TriangleCenter(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "TriangleCenter(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void TriangleCenter(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " TriangleCenter(" + input + ")"));
        }

        #endregion

        #region TriangleCurve

        public void TriangleCurve(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "TriangleCurve(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + ")"));
        }

        public void TriangleCurve(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " TriangleCurve(" + input + ")"));
        }

        #endregion

        #region Trilinear

        public void Trilinear(int idA, int idB, int idC, int idD, int idE, int idF)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(
                cmdString: "Trilinear(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + "," + ObjectFromID(id: idC) + "," +
                ObjectFromID(id: idD) + "," + ObjectFromID(id: idE) + "," + ObjectFromID(id: idF) + ")"));
        }

        public void Trilinear(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Trilinear(" + input + ")"));
        }

        #endregion

        #region Vertex

        public void Vertex(int idA)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: "Vertex(" + ObjectFromID(id: idA) + ")"));
        }

        public void Vertex(int idA, int idB)
        {
            StartCoroutine(
                routine: HandWaverServerTransport.execCommand(cmdString: "Vertex(" + ObjectFromID(id: idA) + "," + ObjectFromID(id: idB) + ")"));
        }

        public void Vertex(string input)
        {
            StartCoroutine(routine: HandWaverServerTransport.execCommand(cmdString: " Vertex(" + input + ")"));
        }

        #endregion
    }
}