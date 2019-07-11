using UnityEngine;

namespace IMRE.HandWaver.Kernel.GGBFunctions
{
    /// <summary>
    ///     Geometry functions to be used within Geogebra session
    /// </summary>
    /// TODO:  Comment
    public class Geometry : MonoBehaviour
    {
        public void AffineRatio(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AffineRatio( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

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
                "Angle( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Angle(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Angle(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void AngleBisector(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AngleBisector(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void AngleBisector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AngleBisector( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }


        public void Arc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Arc( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AreCollinear(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreCollinear( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AreConcurrent(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcurrent( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void AreConcyclic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "AreConcyclic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void AreCongruent(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "AreCongruent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void AreEqual(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AreEqual(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void AreParallel(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("AreParallel(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void ArePerpendicular(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("ArePerpendicular(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

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
                "Area( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
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

        public void Barycenter(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Barycenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void Centroid(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Centroid(" + ObjectFromID(idA) + ")"));
        }

        public void CircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularArc( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircularSector( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircumcircularArc(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularArc( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void CircumcircularSector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CircumcircularSector( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) +
                ")"));
        }

        public void Circumference(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Circumference(" + ObjectFromID(idA) + ")"));
        }

        public void ClosestPoint(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "ClosestPoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void ClosestPointRegion(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("ClosestPointRegion(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public void CrossRatio(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "CrossRatio(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Cubic(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Cubic(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Direction(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Direction(" + ObjectFromID(idA) + ")"));
        }

        public void Distance(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Distance(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Envelope(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Envelope(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Intersect(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Intersect(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Intersect(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Intersect(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void IntersectPath(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("IntersectPath(" + ObjectFromID(idA) + "," + ObjectFromID(idB) +
                                                     ")"));
        }

        public void Intersection(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand(
                    "Intersection(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Length(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Length(" + ObjectFromID(idA) + ")"));
        }

        public void Length(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Length( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Length(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Length(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Line(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Line(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Locus(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Locus(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

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

        public void Midpoint(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + ")"));
        }

        public void Midpoint(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Midpoint(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Perimeter(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Perimeter(" + ObjectFromID(idA) + ")"));
        }

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
                "PerpendicularBisector( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) +
                ")"));
        }

        public void PerpendicularLine(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("PerpendicularLine(" + ObjectFromID(idA) + "," +
                                                     ObjectFromID(idB) + ")"));
        }

        public void PerpendicularLine(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "PerpendicularLine( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Point(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + ")"));
        }

        public void Point(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Point(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void PointIn(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("PointIn(" + ObjectFromID(idA) + ")"));
        }

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
                "Polygon( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
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
                ObjectFromID(idD) + "+ObjectFromID(idE)+"));
        }

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
                "Polyline( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
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
                ObjectFromID(idD) + "+ObjectFromID(idE)+"));
        }

        public void Prove(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Prove(" + ObjectFromID(idA) + ")"));
        }

        public void ProveDetails(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("ProveDetails(" + ObjectFromID(idA) + ")"));
        }

        public void Radius(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Radius(" + ObjectFromID(idA) + ")"));
        }

        public void Ray(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Ray(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

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
                "RigidPolygon( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
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
                ObjectFromID(idD) + "+ObjectFromID(idE)+"));
        }

        public void Sector(int idA, int idB, int idC)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Sector( " + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + ")"));
        }

        public void Segment(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Segment(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void Slope(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Slope(" + ObjectFromID(idA) + ")"));
        }

        public void Tangent(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Tangent(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }

        public void TriangleCenter(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCenter(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void TriangleCurve(int idA, int idB, int idC, int idD)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "TriangleCurve(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + ")"));
        }

        public void Trilinear(int idA, int idB, int idC, int idD, int idE, int idF)
        {
            StartCoroutine(HandWaverServerTransport.execCommand(
                "Trilinear(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + "," + ObjectFromID(idC) + "," +
                ObjectFromID(idD) + "+ObjectFromID(idE)+" + ObjectFromID(idF) + ")"));
        }

        public void Vertex(int idA)
        {
            StartCoroutine(HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + ")"));
        }

        public void Vertex(int idA, int idB)
        {
            StartCoroutine(
                HandWaverServerTransport.execCommand("Vertex(" + ObjectFromID(idA) + "," + ObjectFromID(idB) + ")"));
        }


        //TODO: Pull from specific class into generic helper functions
        public string ObjectFromID(int id)
        {
            return "";
        }
    }
}