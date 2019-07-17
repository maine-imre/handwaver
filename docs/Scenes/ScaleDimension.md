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

### Intersection of a plane and an annulus

Consider an annulus <img src="/docs/Scenes/tex/53d147e7f3fe6e47ee05b88b166bd3f6.svg?invert_in_darkmode&sanitize=true" align=middle width=12.32879834999999pt height=22.465723500000017pt/> with inner radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> and outer radius <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/>. Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the distance between the center of the annulus and the line segment <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/> formed by the intersection of a plane with the annulus. This intersection will yield either a point, a line segment, or two line segments. 
Let <img src="/docs/Scenes/tex/5a5214935f8b6ee914efeece84e7535c.svg?invert_in_darkmode&sanitize=true" align=middle width=17.614197149999992pt height=21.18721440000001pt/> be the distance between the two points in the case where the intersection yields one line segment, which will occur when the intersection is in between <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/>. By the same logic as the cross-section of a circle, we have the equation 
<p align="center"><img src="/docs/Scenes/tex/d90b91fd06dac0b69ccee3297e55ee3a.svg?invert_in_darkmode&sanitize=true" align=middle width=106.59806355pt height=18.312383099999998pt/></p>

Following the logic used for the circle, the points lie at <img src="/docs/Scenes/tex/88b6ae92dc45e65fe3ca08e37c808f5e.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=30.173662199999985pt/>.  If <img src="/docs/Scenes/tex/023bfdc737f856ed7c8314f05df2282f.svg?invert_in_darkmode&sanitize=true" align=middle width=43.02498749999999pt height=22.465723500000017pt/>, the two values are equal and there is only one point formed by the intersection of <img src="/docs/Scenes/tex/9b325b9e31e85137d1de765f43c0f8bc.svg?invert_in_darkmode&sanitize=true" align=middle width=12.92464304999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/ddcb483302ed36a59286424aa5e0be17.svg?invert_in_darkmode&sanitize=true" align=middle width=11.18724254999999pt height=22.465723500000017pt/>. 

The other case results from the intersection occurcing at a height with a magnitude less than that of the inner radius, giving two line segments. The math for this case is essentially the same as one line segment, but with an additional calculation for the two inner points. 
Let <img src="/docs/Scenes/tex/5a5214935f8b6ee914efeece84e7535c.svg?invert_in_darkmode&sanitize=true" align=middle width=17.614197149999992pt height=21.18721440000001pt/> be the distance between the two outer points, one for each line segment. As before, their location can be calculated by <p align="center"><img src="/docs/Scenes/tex/d90b91fd06dac0b69ccee3297e55ee3a.svg?invert_in_darkmode&sanitize=true" align=middle width=106.59806355pt height=18.312383099999998pt/></p>

Thus, the points lie at <img src="/docs/Scenes/tex/88b6ae92dc45e65fe3ca08e37c808f5e.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=30.173662199999985pt/>. 
Now let <img src="/docs/Scenes/tex/b5f9a72535216bfe7d51d9273a867952.svg?invert_in_darkmode&sanitize=true" align=middle width=20.43005579999999pt height=21.18721440000001pt/> be the distance between the two inner points. The location for both of those points can be found using the same calculation as the outer points, but using <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> and <img src="/docs/Scenes/tex/31fae8b8b78ebe01cbfbe2fe53832624.svg?invert_in_darkmode&sanitize=true" align=middle width=12.210846449999991pt height=14.15524440000002pt/> instead of <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> and <img src="/docs/Scenes/tex/332cc365a4987aacce0ead01b8bdcc0b.svg?invert_in_darkmode&sanitize=true" align=middle width=9.39498779999999pt height=14.15524440000002pt/>. Thus, the equation is <p align="center"><img src="/docs/Scenes/tex/753dce046caf86b05b1b1917db78f476.svg?invert_in_darkmode&sanitize=true" align=middle width=104.67840735pt height=18.312383099999998pt/></p>

Thus, the inner points lie at <img src="/docs/Scenes/tex/8f6882a9b1fb02eae1a4bf03b3dc4bfb.svg?invert_in_darkmode&sanitize=true" align=middle width=111.76558964999998pt height=30.173662199999985pt/>. In the event that <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> > <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/>, there is no intersection between the annulus and plane.

### Intersection of a plane and a sphere

Consider a sphere <img src="/docs/Scenes/tex/e257acd1ccbe7fcb654708f1a866bfe9.svg?invert_in_darkmode&sanitize=true" align=middle width=11.027402099999989pt height=22.465723500000017pt/> with radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> centered at the origin. Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the distance between the center of the sphere and the plane <img src="/docs/Scenes/tex/df5a289587a2f0247a5b97c1e8ac58ca.svg?invert_in_darkmode&sanitize=true" align=middle width=12.83677559999999pt height=22.465723500000017pt/> formed by intersecting the sphere with a plane. In the event that the intersection only hits the top or bottom edge of the sphere, the resulting cross-section will simply be a point at the top or bottom of the sphere. 

### Intersection of a plane and a torus

### Intersection of a hyperplane and a hypersphere

### Intersection of a hyperplane and a three-torus


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

### 5-cell

### 8-cell

## Orthogrpahic Projection from 4D to 3D


##References
