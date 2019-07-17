# Mathematical Audit of Algorithms for Scale and Dimension Scene

In this document, we describe the methods we use (and their sources) for computing the dynamic figures described in the scale and dimension scene.  We acknolwedge that the cases presented here may be limited in nature.  While this scene was being developed a team was working to integrate with a server-side implementation of GeoGebra.  Any generalized version would use the GGB implementation to do the heavy lifting.

## Cross-Sections



### Intersection of a plane and a circle

Consider a circle <img src="/docs/Scenes/tex/9b325b9e31e85137d1de765f43c0f8bc.svg?invert_in_darkmode&sanitize=true" align=middle width=12.92464304999999pt height=22.465723500000017pt/> in a plane orthagonal to <img src="/docs/Scenes/tex/707b9156aaaa9c0848cf4e0456912ea5.svg?invert_in_darkmode&sanitize=true" align=middle width=52.05482864999998pt height=24.65753399999998pt/> and and a plane orthagonal to <img src="/docs/Scenes/tex/b02fcb10a71a10e66d862275f5a94c06.svg?invert_in_darkmode&sanitize=true" align=middle width=52.05482864999998pt height=24.65753399999998pt/>.
Let <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> be the radius of the circle.
Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the signed distance between the center of the circle and the line <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/> formed by the intersection of the plane and the circle's plane.
Let <img src="/docs/Scenes/tex/5a5214935f8b6ee914efeece84e7535c.svg?invert_in_darkmode&sanitize=true" align=middle width=17.614197149999992pt height=21.18721440000001pt/> be the distance between the points formed by the intersection of the circle and the line <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/>.
Since there is reflective symmetry in any direction on the circle, the points lie <img src="/docs/Scenes/tex/234b0c11974419b7414095e977d1aed7.svg?invert_in_darkmode&sanitize=true" align=middle width=9.39498779999999pt height=14.15524440000002pt/> in the direction of <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/> from the center of the circle.
By the pythagorean theorem we have <p align="center"><img src="/docs/Scenes/tex/d90b91fd06dac0b69ccee3297e55ee3a.svg?invert_in_darkmode&sanitize=true" align=middle width=106.59806355pt height=18.312383099999998pt/></p>

  So the points lie at <img src="/docs/Scenes/tex/88b6ae92dc45e65fe3ca08e37c808f5e.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=30.173662199999985pt/>.  If <img src="/docs/Scenes/tex/023bfdc737f856ed7c8314f05df2282f.svg?invert_in_darkmode&sanitize=true" align=middle width=43.02498749999999pt height=22.465723500000017pt/>, the two values are equal and there is only one point formed by the intersection of <img src="/docs/Scenes/tex/9b325b9e31e85137d1de765f43c0f8bc.svg?invert_in_darkmode&sanitize=true" align=middle width=12.92464304999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/>.  If <img src="/docs/Scenes/tex/185424e1f15ba5541cbf3b49e0d6601a.svg?invert_in_darkmode&sanitize=true" align=middle width=43.02498749999999pt height=22.465723500000017pt/>, then there is no intersection.

[//]: # add a diagram to illustrate

```c#
//if cross section only hits the edge of the circle
if (math.abs(height) == radius)
{
    //if top of circle, create point at intersection
    if (height == radius)
    {
        segmentEndPoint0 = Vector3.up * radius;
    }

    //if bottom of circle, create point at intersection
    else if (height == -radius)
    {
        segmentEndPoint0 = Vector3.down * radius;
    }
}
//cross section is a line that hits two points on the circle (height smaller than radius of circle)
else if (math.abs(height) < radius)
{
    //horizontal distance from center of circle to point on line segment
    float segmentLength = Mathf.Sqrt(1f - Mathf.Pow(height, 2));

    //calculations for endpoint coordinates of line segment
    segmentEndPoint0 = (Vector3.up * height) + (Vector3.left * segmentLength);
    segmentEndPoint1 = (Vector3.up * height) + (Vector3.right * segmentLength);
}
//height for cross section is outside of circle 
else if (math.abs(height) > radius)
{
    Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and an annulus

Consider an annulus <img src="/docs/Scenes/tex/53d147e7f3fe6e47ee05b88b166bd3f6.svg?invert_in_darkmode&sanitize=true" align=middle width=12.32879834999999pt height=22.465723500000017pt/> with inner radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> and outer radius <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/>. Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the distance between the center of the annulus and the line segment <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/> formed by the intersection of a plane with the annulus. This intersection will yield either a point, a line segment, or two line segments. 
Let <img src="/docs/Scenes/tex/5a5214935f8b6ee914efeece84e7535c.svg?invert_in_darkmode&sanitize=true" align=middle width=17.614197149999992pt height=21.18721440000001pt/> be the distance between the two points in the case where the intersection yields one line segment, which will occur when the intersection is in between <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/>. By the same logic as the cross-section of a circle, we have the equation 
<p align="center"><img src="/docs/Scenes/tex/d90b91fd06dac0b69ccee3297e55ee3a.svg?invert_in_darkmode&sanitize=true" align=middle width=106.59806355pt height=18.312383099999998pt/></p>

Following the logic used for the circle, the points lie at <img src="/docs/Scenes/tex/88b6ae92dc45e65fe3ca08e37c808f5e.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=30.173662199999985pt/>.  If <img src="/docs/Scenes/tex/023bfdc737f856ed7c8314f05df2282f.svg?invert_in_darkmode&sanitize=true" align=middle width=43.02498749999999pt height=22.465723500000017pt/>, the two values are equal and there is only one point formed by the intersection of <img src="/docs/Scenes/tex/9b325b9e31e85137d1de765f43c0f8bc.svg?invert_in_darkmode&sanitize=true" align=middle width=12.92464304999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/>. 

The other case results from the intersection occurcing at a height with a magnitude less than that of the inner radius, giving two line segments. The math for this case is essentially the same as one line segment, but with an additional calculation for the two inner points. 
Let <img src="/docs/Scenes/tex/5a5214935f8b6ee914efeece84e7535c.svg?invert_in_darkmode&sanitize=true" align=middle width=17.614197149999992pt height=21.18721440000001pt/> be the distance between the two outer points, one for each line segment. As before, their location can be calculated by <p align="center"><img src="/docs/Scenes/tex/d90b91fd06dac0b69ccee3297e55ee3a.svg?invert_in_darkmode&sanitize=true" align=middle width=106.59806355pt height=18.312383099999998pt/></p>

Thus, the points lie at <img src="/docs/Scenes/tex/88b6ae92dc45e65fe3ca08e37c808f5e.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=30.173662199999985pt/>. 
Now let <img src="/docs/Scenes/tex/b5f9a72535216bfe7d51d9273a867952.svg?invert_in_darkmode&sanitize=true" align=middle width=20.43005579999999pt height=21.18721440000001pt/> be the distance between the two inner points. The location for both of those points can be found using the same calculation as the outer points, but using <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> and <img src="/docs/Scenes/tex/31fae8b8b78ebe01cbfbe2fe53832624.svg?invert_in_darkmode&sanitize=true" align=middle width=12.210846449999991pt height=14.15524440000002pt/> instead of <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/332cc365a4987aacce0ead01b8bdcc0b.svg?invert_in_darkmode&sanitize=true" align=middle width=9.39498779999999pt height=14.15524440000002pt/>. Thus, the equation is <p align="center"><img src="/docs/Scenes/tex/753dce046caf86b05b1b1917db78f476.svg?invert_in_darkmode&sanitize=true" align=middle width=104.67840735pt height=18.312383099999998pt/></p>

Thus, the inner points lie at <img src="/docs/Scenes/tex/8f6882a9b1fb02eae1a4bf03b3dc4bfb.svg?invert_in_darkmode&sanitize=true" align=middle width=111.76558964999998pt height=30.173662199999985pt/>. In the case that <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> > <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/>, there is no intersection between the annulus and plane.

```c#
//cross-section only hits edge of annulus
if (math.abs(height) == outerRadius)
{
//if top edge, create point at intersection
if (height == outerRadius)
{
    segmentAEndPoint0 = Vector3.up * outerRadius;
}
//if bottom edge, create point at intersection
else
{
    segmentAEndPoint0 = Vector3.down * outerRadius;
}
}
//cross section is a line segment in between the inner circle and outer circle
else if (math.abs(height) < outerRadius && math.abs(height) >= innerRadius)
{
//horizontal distance from center to point on outer edge of annulus
x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));

//calculations for coordinates of line segment endpoints
segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
}
//cross section height is less than the inner radius, resulting in two line segments
else if (math.abs(height) < innerRadius)
{
//horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));
x2 = (Mathf.Sqrt(Mathf.Pow(innerRadius, 2) - Mathf.Pow(height, 2)));

//calculations for inner and outer endpoints for each line segment
segmentAEndPoint0 = (Vector3.up * height) + (Vector3.left * (x1));
segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));.tex.md

segmentBEndPoint0 = (Vector3.up * height) + (Vector3.right * (x2));
segmentBEndPoint1 = (Vector3.up * height) + (Vector3.right * (x1));
}
//cross section height is out of range of annulus
else if (math.abs(height) > outerRadius)
{
Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and a sphere

Consider a sphere <img src="/docs/Scenes/tex/e257acd1ccbe7fcb654708f1a866bfe9.svg?invert_in_darkmode&sanitize=true" align=middle width=11.027402099999989pt height=22.465723500000017pt/> with radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> centered at the origin. Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the distance between the center of the sphere and the plane <img src="/docs/Scenes/tex/df5a289587a2f0247a5b97c1e8ac58ca.svg?invert_in_darkmode&sanitize=true" align=middle width=12.83677559999999pt height=22.465723500000017pt/> formed by intersecting the sphere with a plane. In the event that the intersection only hits the top or bottom edge of the sphere, the resulting cross-section will simply be a point at the top or bottom of the sphere. 

```c#
//if cross section only hits the edge of the circle
if (math.abs(height) == radius)
{
    //if top of sphere, create point at intersection
    if (height == radius)
    {
        Vector3 segmentEndPoint0 = Vector3.up * radius;
    }
    //if bottom of circle, create point at intersection
    else if (height == -radius)
    {
        Vector3 segmentEndPoint0 = Vector3.down * radius;
    }
}
//cross section is a circle
else if (math.abs(height) < radius)
{
    //horizontal distance from center of circle to point on line segment
    renderCircle(Mathf.Sqrt(Mathf.Pow(radius,2) - Mathf.Pow(height, 2)), height*Vector3.up);
}
//height for cross section is outside of circle 
else if (math.abs(height) > radius)
{
    Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and a torus

``` c#

```

### Intersection of a hyperplane and a hypersphere

``` c#

```

### Intersection of a hyperplane and a three-torus

``` c#

```


## Nets

### Equaliaterial Triangle

The net of a triangle is three congruant line segments.  In it's unfolded state, the line segments are colinear. To fold the triangle net, hold one segment fixed and rotate the other two segments (clockwise and counterclockwise, respectively) by <img src="/docs/Scenes/tex/90ba29b77077491b320c9da207fbeceb.svg?invert_in_darkmode&sanitize=true" align=middle width=18.485245349999996pt height=27.77565449999998pt/> radians.

```C#
//angle of rotation in degrees (Unity.Mathematics works in degrees)
float t = percentFolded * 120f;
//matrix of vertices 
Vector3[] result = new Vector3[4];
//initial vertices
result[2] = Vector3.zero;
result[1] = Vector3.right;
//rotate vertex by t or -t around (0, 1, 0) with appropriate vector manipulation to connect triangle
result[0] = result[1] + Quaternion.AngleAxis(t, Vector3.up) * Vector3.right;
result[3] = result[2] + Quaternion.AngleAxis(-t, Vector3.up) * Vector3.left;
```

### Square

The net of a square is four congruant line segments.  In it's unfolded state, the line segments are colinear.  To fold the square net, hold one of the middle segments fixed.  Rotate the two adjacent segments around their respective endpoints by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> radians. The remaining segment is adjacent to one of the rotated segments (segment A).  Rotate that segment by ninety degrees around it's joining endpoint, with respect to the direction of segment A.  In effect, this vertex is rotated by <img src="/docs/Scenes/tex/06798cd2c8dafc8ea4b2e78028094f67.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to it's origional direction.

```c#
//angle of rotation in degrees (Unity.Mathematics works in degrees)
float angle = percentFolded * 90f;
//matrix of vertices
Vector3[] result = new Vector3[5];
//initial vertices that don't need to move/are pivot points
result[2] = Vector3.zero;
result[1] = Vector3.right;
//rotate vertice by t or -t around (0, 1, 0) 
result[0] = result[1] + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
result[3] = result[2] + Quaternion.AngleAxis(-angle, Vector3.up) * Vector3.left;
//rotate vertice by -2t around (0, 1, 0)
result[4] = result[3] + Quaternion.AngleAxis(-2 * angle, Vector3.up) * Vector3.left;
```

### Cube

The net of a cube is a collection of six congruant squares,
One square remains fixed in the center.  
Four squares share an edge with the center square, and rotate around that edge by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> to fold up the net.
On one of those four squares, a final square is constructed sharing the opposite edge.
The final square is rotated by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to the adjacent square, or <img src="/docs/Scenes/tex/06798cd2c8dafc8ea4b2e78028094f67.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to it's origional orientation, around it's shared edge.
```c        
float degreeFolded = percentFolded * 90f + 180f;
//14 points on cube net
Vector3[] result = new Vector3[14];

//4 vertices for base of cube
result[0] = .5f * (Vector3.forward + Vector3.right);
result[1] = .5f * (Vector3.forward + Vector3.left);
result[2] = .5f * (Vector3.back + Vector3.left);
result[3] = .5f * (Vector3.back + Vector3.right);

//use squareVert() to fold outer squares up relative to base square 
result[4] = squareVert(result[3], result[0], result[1], degreeFolded);
result[5] = squareVert(result[3], result[0], result[2], degreeFolded);

result[6] = squareVert(result[0], result[1], result[3], degreeFolded);
result[7] = squareVert(result[0], result[1], result[2], degreeFolded);


result[8] = squareVert(result[2], result[3], result[1], degreeFolded);
result[9] = squareVert(result[2], result[3], result[0], degreeFolded);

result[10] = squareVert(result[1], result[2], result[0], degreeFolded);
result[11] = squareVert(result[1], result[2], result[3], degreeFolded);

result[12] = squareVert(result[10], result[11], result[1], degreeFolded);
result[13] = squareVert(result[10], result[11], result[2], degreeFolded);
```

```c#
private static Vector3 squareVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint,
float degreeFolded)
{
    return Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
       (oppositePoint - (nSegmentA + nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
}
```

### Regular Tetrahedron

The net of a tetrahedron is a collection of four congruant equilaterial triangles.  One traingle remains fixed in the center, and each of the remaining triangles shares an edge with the center triangle.  All triangles except the center rotate around their shared edge by <img src="/docs/Scenes/tex/d198a46d6c0cc6400dd3ea7ebfe0c709.svg?invert_in_darkmode&sanitize=true" align=middle width=55.237455899999986pt height=27.77565449999998pt/>.

```c#
//scale the degree folded by the diehdral angle of the folded tetrahedron of ~70.52
float degreefolded = percentfolded * COMPLETEDFOLD + 180f;
//6 vertices on tetrahedron
Vector3[] result = new Vector3[6];

//inner 3 vertices
result[0] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.forward * .5f;
result[1] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.back * .5f;
result[2] = Vector3.zero;

//vertex between 0 and 1
//use trivert() to fold outer vertices up relative to inner vertices
result[3] = triVert(result[0], result[1], result[2], degreefolded);

//vertex between 1 and 2
result[4] = triVert(result[1], result[2], result[0], degreefolded);
//vertex between 0 and 2
result[5] = triVert(result[2], result[0], result[1], degreefolded);
```

```c#
private static Vector3 triVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint, float degreeFolded)
{
    return Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
           (oppositePoint - (nSegmentA + nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
}
```

### Regular 5-cell

The net of a five cell is composed of four congruant regular tetrahedrons.  One tetrahedron is fixed at the center.  Each of the remaining tetrahedrons is constructed to share a face of the center tetrahedron.  To fold the 5-cell net, each of the tetrahedrons (except the center) is rotated on a plane perpendicular to the shared face such that the direction from the center of the shared face to the opposite vertex and the direction from the center of the shared face to the end-state of the folded apex form a basis for the plane of rotation.

```c#
//8 points on unfolded fivecell
float4[] result = new float4[8];

//core tetrahedron (does not fold)
//coordiantes from wikipedia  https://en.wikipedia.org/wiki/5-cell, centered at origin, 
result[0] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), 1f / math.sqrt(3f), 1f)) / 2f;
result[1] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), 1f / math.sqrt(3f), -1f)) / 2f;
result[2] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), -2f / math.sqrt(3f), 0f)) / 2f;
result[3] = new float4(1f / math.sqrt(10f), -math.sqrt(3f / 2f), 0f, 0f);

//find position of convergent point for other tetrahedrons in the net.
float4 apex = new float4(-2 * math.sqrt(2f / 5f), 0f, 0f, 0f);

//apex of tetrahedron for each additional tetrahedron(from fases of first) foldling by degree t
float4 center1 = (result[0] + result[1] + result[2]) / 3f;
float4 dir1 = center1 - result[3];
result[4] = center1 + Math.Operations.rotate(dir1, dir1, apex - center1, degreeFolded);

float4 center2 = (result[0] + result[2] + result[3]) / 3f;
float4 dir2 = center2 - result[1];
result[5] = center2 + Math.Operations.rotate(dir2, dir2, apex - center2, degreeFolded);

float4 center3 = (result[0] + result[1] + result[3]) / 3f;
float4 dir3 = center3 - result[2];
result[6] = center3 + Math.Operations.rotate(dir3, dir3, apex - center3, degreeFolded);

float4 center4 = (result[1] + result[2] + result[3]) / 3f;
float4 dir4 = center4 - result[0];
result[7] = center4 + Math.Operations.rotate(dir4, dir4, apex - center4, degreeFolded);
```

### Regular 8-cell

The net of an 8-cell is a collection of eight congruant cubes.  One cube remains fixed in the center, and six additional cubes are constructed sharing the faces of the center cube. The remaining cube is constructed to share the face opposite of the shared face for one of these six cubes.

To fold the net of an 8-cell, the center cube is fixed.  The six adjacent cubes are rotated around a plane that is orthagonal to the face of the cube and colinear with the w-axis (if the net is constructed such that w is fixed for all verticies of the cubes), by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/>.  The remaining cube is rotated with respect to its adjacent cube by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> or by <img src="/docs/Scenes/tex/06798cd2c8dafc8ea4b2e78028094f67.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to it's origional position, with the rotation centerd on its shared face (note that this face is also moving during the rotaiton).


```c#
 //core cube (does not fold)
result[0] = (up + right + forward)/2f;
result[1] = (up + left + forward)/2f;
result[2] = (up + left + back)/2f;
result[3] = (up + right + back)/2f;

result[4] = (down + right + forward) / 2f;
result[5] = (down + left + forward) / 2f;
result[6] = (down + left + back) / 2f;
result[7] = (down + right + back) / 2f;

//above up face.
result[8] = result[0] + Math.Operations.rotate(up, up, wForward, degreeFolded);
result[9] = result[1] + Math.Operations.rotate(up,up, wForward, degreeFolded);
result[10] = result[2] + Math.Operations.rotate(up, up, wForward, degreeFolded);
result[11] = result[3] + Math.Operations.rotate(up, up, wForward, degreeFolded);

//below down face
result[12] = result[4] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[13] = result[5] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[14] = result[6] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[15] = result[7] + Math.Operations.rotate(down,down, wForward, degreeFolded);

//right of right face;
result[16] = result[0] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[17] = result[3] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[18] = result[7] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[19] = result[4] + Math.Operations.rotate(right, right, wForward, degreeFolded);

//left of left face
result[20] = result[1] + Math.Operations.rotate(left,left, wForward, degreeFolded);
result[21] = result[2] + Math.Operations.rotate(left, left,wForward, degreeFolded);
result[22] = result[6] + Math.Operations.rotate(left, left,wForward, degreeFolded);
result[23] = result[5] + Math.Operations.rotate(left, left,wForward, degreeFolded);

//forward of forward face.
result[24] = result[0] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[25] = result[1] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[26] = result[5] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[27] = result[4] + Math.Operations.rotate(forward, forward,wForward, degreeFolded);

//back of back face.
result[28] = result[2] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[29] = result[3] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[30] = result[7] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[31] = result[6] + Math.Operations.rotate(back,back, wForward, degreeFolded);

float4 tmp = Math.Operations.rotate(down,down, wForward, degreeFolded);
//down of double down.
result[32] = result[12] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[33] = result[13] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[34] = result[14] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[35] = result[15] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
```

## float3 and float4 Mathematical Library Extensions

### Angle between vectors
```c#
public static float Angle(float3 from, float3 to)
{
    return math.acos(math.dot(math.normalize(from), math.normalize(to)));
}
```

### Magnitude
Take the 2-norm of the vector v.

```c#
public static float magnitude(float3 v)
{
    return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
}

public static float magnitude(float4 v)
{
    return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
}
```

### Tripple Cross Product

```c#
public static float4 cross(float4 v, float4 w, float4 u)
{
    //from https://github.com/hollasch/ray4/blob/master/wire4/v4cross.c

    float A, B, C, D, E, F; /* Intermediate Values */

    A = (v.x * w.y) - (v.y * w.x);
    B = (v.x * w.z) - (v.z * w.x);
    C = (v.x * w.w) - (v.w * w.x);
    D = (v.y * w.z) - (v.z * w.y);
    E = (v.y * w.w) - (v.w * w.y);
    F = (v.z * w.w) - (v.w * w.z);

    return new float4(
        u[1] * F - u[2] * E + u[3] * D,
        -(u[0] * F) + u[2] * C - u[3] * B,
        u[0] * E - u[1] * C + u[3] * A,
        -(u[0] * D) + u[1] * B - u[2] * A
    );
}
```

### Rotaitons for Float4s

```c#
public static float4 rotate(float4 v, float4 basis0, float4 basis1, float theta)
{
    math.normalize(basis0);
    math.normalize(basis1);

float4 remainder = v - (project(v, basis0) + project(v, basis1));
    theta *= Mathf.Deg2Rad;

    float4 v2 = v;
    math.normalize(v2);

    if (math.dot(basis0, basis1) != 0f)
    {
        Debug.LogWarning("Basis is not orthagonal");
    }
    else if (math.dot(v2, basis0) != 1f || Vector4.Dot(v, basis1) != 0f)
    {
        Debug.LogWarning("Original Vector does not lie in the same plane as the first basis vector.");
    }

    return Vector4.Dot(v, basis0) * (math.cos(theta) * basis0 + basis1 * math.sin(theta)) +
           math.dot(v, basis1) * (math.cos(theta) * basis1 + math.sin(theta) * basis0) + remainder;
}
```

### Projection for Float4
```c#
public static float4 project(float4 v, float4 dir)
{
    return math.dot(v, dir) * math.normalize(dir);
}
```

## Determinants and Determinant Coeficients
```c#
private static float[] Determinant2X2(float4 v0, float4 v1)
{
    //find largest determinant of 2x2
    float[] determinants = new float[6];
    determinants[0] = v0.x * v1.y - v0.y * v1.x;
    determinants[1] = v0.x * v1.z - v0.z * v1.x;
    determinants[2] = v0.x * v1.w - v0.w * v1.x;
    determinants[3] = v0.y * v1.z - v0.z * v1.y;
    determinants[4] = v0.y * v1.w - v0.w * v1.y;
    determinants[5] = v0.z * v1.w - v0.w * v1.z;
    return determinants;
}
```      

```c#
/// <summary>
/// Assume the following structure, return the determinant coeficients for v0, v1, v2, v3
/// v0 v1 v2 v3
/// x00 x01 x02 x03
/// x10 x11 x12 x13
/// x20 x21 x22 x23
/// </summary>
/// <param name="matrix"></param>
/// <returns></returns>
private static float4 determinantCoef(float4x3 matrix)
{
    float4 bottomRow = matrix.c2;
    float[] determinants = Determinant2X2(matrix.c0, matrix.c1);
    return new float4(
        bottomRow.y*determinants[5]-bottomRow.z*determinants[4]+bottomRow.w*determinants[3],
        -(bottomRow.x*determinants[5]-bottomRow.z*determinants[2]+bottomRow.w*determinants[3]),
        bottomRow.x*determinants[4]-bottomRow.y*determinants[2]+bottomRow.w*determinants[0],
        -(bottomRow.x*determinants[3]-bottomRow.y*determinants[1]+bottomRow.z*determinants[0])
     );
}
```

### Basis for Hyperplane Orthangoal to a Vector

```c#
public static float4x3 basisSystem(this float4 v)
{
    math.normalize(v);
    //use method described here:  https://www.geometrictools.com/Documentation/OrthonormalSets.pdf
    if (v.x == 0 && v.y == 0 && v.z == 0 && v.w ==0)
    {
        Debug.LogError("Can't form basis from zero vector");
    }
    //the vector is the first basis vector for the 4-space, orthag to the hyperplane
    math.normalize(v);
    //establish a second basis vector
    float4 basis0;
    if (v.x != 0 || v.y != 0)
    {
        basis0 = new Vector4(v.y, v.x, 0, 0);
    }
    else
    {
        basis0 = new Vector4(0 ,0,v.w,v.z);
    }

    math.normalize(basis0);

    float[] determinants = Determinant2X2(v, basis0);

    //index of largest determinant
    int idx = 0;
    for (int i = 0; i < 6; i++)
    {
        if (determinants[i] > determinants[idx])
        {
            idx = i;
        }
    }

    if (determinants[idx] == 0)
    {
        Debug.LogError("No non-zero determinant");
    }
    //choose bottom row of det matrix to generate next basis vector
    float4 bottomRow;
    if (idx == 0 || idx == 1 || idx == 3)
    {
        bottomRow = new float4(0,0,0,1);
    }else if (idx == 2 || idx == 4)
    {
        bottomRow = new float4(0,0,1,0);
    }
    else
    {
        //idx = 5
        bottomRow = new float4(0,1,0,0);
    }

    float4 basis1 = determinantCoef(new float4x3(v, basis0, bottomRow));
    math.normalize(basis1);

    float4 basis2 = determinantCoef(new float4x3(v, basis0, basis1));
    math.normalize(basis2);

    //returns the basis that spans the hyperplane orthogonal to v
    float4x3 basis = new float4x3(basis0,basis1,basis2);
    //check that v is orthogonal.
    v.projectDownDimension(basis, ProjectionMethod.parallel,null, null,null );
    if (v.x != 0 || v.y != 0 || v.z != 0)
    {
        Debug.LogError("Basis is not orthogonal to v");
    }
    return basis;
}
```

## Projections from 4D to 3D


```c#
public static float3 projectDownDimension(this float4 v, float4x3 inputBasis, ProjectionMethod method,
float? Vangle, float4? eyePosition, float? viewingRadius)
{
float T, S;
float4x4 basis;
float4 tmp;
//set defaults
Vangle = Vangle ?? 0f;
eyePosition = eyePosition ?? float4.zero;
viewingRadius = viewingRadius ?? 1f;

switch (method)
{
    case ProjectionMethod.orthographic:
        math.normalize(inputBasis.c0);
        math.normalize(inputBasis.c1);
        math.normalize(inputBasis.c2);

        return new float3(math.dot(v, inputBasis.c0), math.dot(v, inputBasis.c1), math.dot(v, inputBasis.c2));
    case ProjectionMethod.projective:
        //using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
        T = 1f / (math.tan(Vangle.Value / 2f));
        tmp = v - eyePosition.Value;
        basis = calc4Matrix(eyePosition.Value, inputBasis);
        S = T / math.dot(v, basis.c3);

        return new float3(S * math.dot(tmp, basis.c0), S * math.dot(tmp, basis.c1),
            S * math.dot(tmp, basis.c2));

    case ProjectionMethod.parallel:
        //using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
        S = 1f / viewingRadius.Value;
        tmp = v - eyePosition.Value;
        basis = calc4Matrix(eyePosition.Value, inputBasis);

        return new float3(S * math.dot(tmp, basis.c0), S * math.dot(tmp, basis.c1),
            S * math.dot(tmp, basis.c2));

    default: return new float3(0f, 0f, 0f);
}
}

public static float4x4 calc4Matrix(float4 from, float4x3 basis){
//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3

float4 Up = basis.c1;
float4 Over = basis.c2;
//Get the normalized Wd column vector.
float4 Wd = basis.c0;
float norm = Math.Operations.magnitude(Wd);
if(norm ==0f)
  Debug.LogError("To point and from point are the same");
math.normalize(Wd);

//calculated the normalized Wa column vector.
float4 Wa =  Math.Operations.cross(Up, Over, Wd);
norm = Math.Operations.magnitude(Wa);
if (norm == 0f)
  Debug.LogError("Invalid Up Vector");
math.normalize(Wa);

//Calculate the normalized Wb column vector
float4 Wb = Math.Operations.cross(Over, Wd, Wa);
norm = Math.Operations.magnitude(Wb);
if (norm == 0f)
  Debug.LogError("Invalid Over Vector");
math.normalize(Wb);

float4 Wc = Math.Operations.cross(Wd, Wa, Wb);
math.normalize(Wc); //theoretically redundant.

return new float4x4(Wa, Wb, Wc, Wd);		
}
```

## Projections from 3D to 2D
To project from 2D to 3D, we have used a virtual camera and virtual canvas within Unity.  This allows for both orthographic and projective perspectives, with a fixed origion and perspective without manipulation of mesh properites.


## References
