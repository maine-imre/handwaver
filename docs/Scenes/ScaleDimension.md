# Mathematical Audit of Algorithms for Scale and Dimension Scene

In this document, we describe the methods we use (and their sources) for computing the dynamic figures described in the scale and dimension scene. We acknolwedge that the cases presented here may be limited in nature. While this scene was being developed, a team was working to integrate with a server-side implementation of GeoGebra. Any generalized version would use the GGB implementation to do the heavy lifting.

There are four main sections. The first section describes algorithms for cross sections. The second section describes algorithms for rendering and folding nets of figures. The third section describes algorithms for projection of 4D figures into 3D. The fourth section describes extensions of the Unity.Mathematics library that are necessary to work with the other algorithms.

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
segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));

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

Consider a sphere <img src="/docs/Scenes/tex/e257acd1ccbe7fcb654708f1a866bfe9.svg?invert_in_darkmode&sanitize=true" align=middle width=11.027402099999989pt height=22.465723500000017pt/> with radius <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> centered at the origin. Let <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> be the distance between the center of the sphere and the plane <img src="/docs/Scenes/tex/df5a289587a2f0247a5b97c1e8ac58ca.svg?invert_in_darkmode&sanitize=true" align=middle width=12.83677559999999pt height=22.465723500000017pt/> formed by intersecting the sphere with a plane. In the event that the intersection only hits the top or bottom edge of the sphere, the resulting cross-section will simply be a point at that edge of the sphere. 
If the intersection occurs at a height less than the radius of the sphere, the cross-section will be a circlular plane. Let this circle's radius be <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/>. Using the Pythagorean Theorem, the value of <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> can be calculated with 
<p align="center"><img src="/docs/Scenes/tex/bab904fcd080433ae760f7fa8768d11a.svg?invert_in_darkmode&sanitize=true" align=middle width=116.50112429999999pt height=19.998924pt/></p> 

So the circle has a known radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> at a distance of magnitude <img src="/docs/Scenes/tex/6dec54c48a0438a5fcde6053bdb9d712.svg?invert_in_darkmode&sanitize=true" align=middle width=8.49888434999999pt height=14.15524440000002pt/> from the center of the sphere. If the plane intersecting the sphere is outside the radius of the circle, there is no resulting cross-section. 

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

### Intersection of a plane and a cone - Conic Sections
Similar to the torus, we only consider cross-sections orthagonal to the axis of the cone.

```c#
```

### Intersection of a plane and a torus - Toric Sections (Moroni, 2017; Weisstein, n.d.)

``` c#
public void crossSectTorus(float height)
{
    //this function is broken into cases where each function is well behaved.
    if (math.abs(height) < revolveRadius - circleRadius)
    {
        float oneNth = 1f / (n);

        for (int i = 0; i < n; i++)
        {
            crossSectionRenderer[0].SetPosition(i, spiricMath((i * oneNth), height, 0f, 0f,0));
            crossSectionRenderer[1].SetPosition(i, spiricMath((i * oneNth), height, 0f, 0f,1));
            crossSectionRenderer[2].SetPosition(i, spiricMath((i * oneNth), height, 0f, 0f,2));
            crossSectionRenderer[3].SetPosition(i, spiricMath((i * oneNth), height, 05f, 0f,3));
            //may need to use reflection.
        }

        crossSectionRenderer.ToList().ForEach(r => r.enabled = true);
    }
    else if (math.abs(height) < revolveRadius + circleRadius){
                    //there is only one spiric
        for (int i = 0; i < n; i++)
        {
            float theta = i * (1f/ n) * Mathf.PI * 2;
            crossSectionRenderer[0].SetPosition(i, spiricOutsideMath(theta, height));
        }
        crossSectionRenderer.ToList().ForEach(r => r.enabled = false);
        crossSectionRenderer[0].enabled = true;
    }
    else
    {
        crossSectionRenderer.ToList().ForEach(r => r.enabled = false);

    }
}
```

```c#
private float3 spiricMath(float v, float height, float alpha, float phi, int idx)
{

    //uses method described here: arXiv:1708.00803v2 [math.GM] 6 Aug 2017
    float p = math.abs(height);
    float x_q = p * math.sin(alpha) * math.cos(phi);
    float y_q = p * math.sin(alpha) * math.cos(phi);
    float z_q = p * math.sin(phi);
    float R = revolveRadius;
    float r = circleRadius;
    float w =.25f * v;


    //v ranges from -1 to 1
    //make two valuies of 2 to accomidate different solution constraints
    //TODO fix these bounds.  the bounds assume phi = 0 and alpha = 0

    //need to project onto different ranges for each solution path.
    float t_0, t_1, t_2, t_3;

        float dist0 = math.sqrt(math.pow(r, 2) - math.pow(w * math.cos(phi) + p * math.sin(phi), 2));
        float dist1 = math.sqrt(math.pow(r, 2) - math.pow(w * math.cos(phi) + p * math.sin(phi), 2));
        t_0 = math.sqrt(-math.pow(p * math.cos(phi) - w * math.sin(phi), 2) + math.pow(R + dist0, 2));
        t_1 = -t_0;
        t_2 = math.sqrt(-math.pow(p * math.cos(phi) - w * math.sin(phi), 2) + math.pow(R - dist1, 2));
        t_3 = -t_2;



        float3 c0 = new float3(x_q + t_0 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
            y_q - t_0 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi));
        float3 c1 = new float3(x_q + t_1 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
            y_q - t_1 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi));
        float3 c2 = new float3(x_q + t_2 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
            y_q - t_2 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi));
        float3 c3 = new float3(x_q + t_3 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
            y_q - t_3 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi));

        switch (idx)
        {
            case 0: return c0;
            case 1: return c1;
            case 2: return c2;
            case 3: return c3;
            default: return new float3();
        }
    }
```

```c#
private float3 spiricOutsideMath(float theta, float height)
{
    //convert values to variables for equation
    float d = 2f * (Mathf.Pow(circleRadius, 2) + Mathf.Pow(revolveRadius, 2) -
                            Mathf.Pow(height, 2));
    float e = 2f * (Mathf.Pow(circleRadius, 2) - Mathf.Pow(revolveRadius, 2) -
                            Mathf.Pow(height, 2));
    float f = -(circleRadius + revolveRadius + height) *
              (circleRadius + revolveRadius - height) *
              (circleRadius - revolveRadius + height) *
              (circleRadius - revolveRadius - height);

    //distance results 
    float r0;
    float r1;

    r0 = Mathf.Sqrt(
             Mathf.Sqrt(
                 Mathf.Pow(
                     -d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                     2) +
                 4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) +
             e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
         Mathf.Sqrt(2);

    r1 = Mathf.Sqrt(
             -Mathf.Sqrt(
                 Mathf.Pow(
                     -d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                     2) +
                 4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) +
             e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
         Mathf.Sqrt(2);


    float3x2 result = new float3x2();

    //distance results converted to theta

    return r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward) + Vector3.up*height;
    //result.c1 = -r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward)+ Vector3.up*height;
    //Debug.Log(height + " : " + d + " : " + e + " : " + f +" : " + r0+" : " +r1);
    //return result;
}
```

```c#
private Vector3 torusPosition(float alpha, float beta)
{
    //3D vectors for describing positions on the circle
    //the center of a cricle (which could be revolved to create the torus
    Vector3 firstPosition = new Vector3(revolveRadius * Mathf.Cos(alpha), revolveRadius * Mathf.Sin(alpha), 0f);
    //the position of a vertex with a circle centered at Vector3.right*rotateRadius
    Vector3 secondPosition = new Vector3(circleRadius * Mathf.Cos(beta), 0f, circleRadius * Mathf.Sin(beta)) +
                             Vector3.right * revolveRadius;

    //mapping of rotation
    Vector3 result = firstPosition + Quaternion.FromToRotation(Vector3.right, firstPosition) * secondPosition;
    return result;
}
```

### Intersection of a hyperplane and a hypersphere (Makholm, 2015)

If an (n-1) dimensional hyperplane intersects an n-dimensional hypersphere, the result is an (n-1) dimensional hypersphere. In particular, the intersection of a 3-plane and a 3-sphere is a 2-sphere.

If we take the 3-plane to be the 3-dimensional rendering's space, the center of the 2-sphere appears fixed for all cross-sections (suppose this is at the origin). Additionally, we can relate their radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/> of the 2-sphere to the radius <img src="/docs/Scenes/tex/1e438235ef9ec72fc51ac5025516017c.svg?invert_in_darkmode&sanitize=true" align=middle width=12.60847334999999pt height=22.465723500000017pt/> of the 3-sphere and the "height" <img src="/docs/Scenes/tex/2ad9d098b937e46f9f58968551adac57.svg?invert_in_darkmode&sanitize=true" align=middle width=9.47111549999999pt height=22.831056599999986pt/> of the hyperplane by <img src="/docs/Scenes/tex/8261e5c490c1e693d8f0ee4865e18b64.svg?invert_in_darkmode&sanitize=true" align=middle width=93.26282789999998pt height=26.76175259999998pt/> (Makholm, 2015).

``` c#
renderSphere(Mathf.Sqrt(radius*radius-sliderval*sliderval));
```


### Intersection of a hyperplane and a hypercone

```c#
```

### Intersection of a hyperplane and a three-torus (Hartley, 2007)

We limit the case of the hyperplane and three-torus intersection to a three-torus to be cross-sected along axes perpendicular to the revolutions used to construct the three torus.
We also choose our three-dimensional perpsective to be embedded within the hyperplane.

Hartley (2007) describes a set of parametric equations for a three-torus.

<p align="center"><img src="/docs/Scenes/tex/9af53b7a396bd302151b390126388f0d.svg?invert_in_darkmode&sanitize=true" align=middle width=263.64520875pt height=16.438356pt/></p>
<p align="center"><img src="/docs/Scenes/tex/3d76cea205f5c56c064c2e23772e641b.svg?invert_in_darkmode&sanitize=true" align=middle width=249.9123297pt height=16.438356pt/></p>
<p align="center"><img src="/docs/Scenes/tex/d4364be5e7a858a2d67849ea539f1a9d.svg?invert_in_darkmode&sanitize=true" align=middle width=162.45616035pt height=16.438356pt/></p>
<p align="center"><img src="/docs/Scenes/tex/afd5bcf2a9c83e55ee3830ff3e4165fb.svg?invert_in_darkmode&sanitize=true" align=middle width=75.97217429999999pt height=16.438356pt/></p>

We are only interested in cross-sections where either <img src="/docs/Scenes/tex/7f4304bf2e52f41a41fc13b12bd19e48.svg?invert_in_darkmode&sanitize=true" align=middle width=58.69858169999999pt height=22.465723500000017pt/> or <img src="/docs/Scenes/tex/5b51bd2e6f329245d425b8002d7cf942.svg?invert_in_darkmode&sanitize=true" align=middle width=12.397274999999992pt height=22.465723500000017pt/> are fixed. We choose our coordinate system to map <img src="/docs/Scenes/tex/244be3c7db382d3e1400c7c4caa1023a.svg?invert_in_darkmode&sanitize=true" align=middle width=41.02358204999999pt height=14.15524440000002pt/> to the remaining three axis.
We need a function that maps <img src="/docs/Scenes/tex/d7093223b4d827e8c29d4ed84b7ae088.svg?invert_in_darkmode&sanitize=true" align=middle width=28.047932549999988pt height=22.831056599999986pt/> ranging from <img src="/docs/Scenes/tex/29632a9bf827ce0200454dd32fc3be82.svg?invert_in_darkmode&sanitize=true" align=middle width=8.219209349999991pt height=21.18721440000001pt/> to <img src="/docs/Scenes/tex/034d0a6be0424bffe9a6e7ac9236c0f5.svg?invert_in_darkmode&sanitize=true" align=middle width=8.219209349999991pt height=21.18721440000001pt/> to <img src="/docs/Scenes/tex/244be3c7db382d3e1400c7c4caa1023a.svg?invert_in_darkmode&sanitize=true" align=middle width=41.02358204999999pt height=14.15524440000002pt/>.
Fix a value of <img src="/docs/Scenes/tex/2ad9d098b937e46f9f58968551adac57.svg?invert_in_darkmode&sanitize=true" align=middle width=9.47111549999999pt height=22.831056599999986pt/>, the cross-section height. 
For any choice of <img src="/docs/Scenes/tex/e427f148d4d1d76000bc79cdbffe89dc.svg?invert_in_darkmode&sanitize=true" align=middle width=73.83551504999998pt height=22.465723500000017pt/> to be fixed to the value of <img src="/docs/Scenes/tex/2ad9d098b937e46f9f58968551adac57.svg?invert_in_darkmode&sanitize=true" align=middle width=9.47111549999999pt height=22.831056599999986pt/>, we can map <img src="/docs/Scenes/tex/d7093223b4d827e8c29d4ed84b7ae088.svg?invert_in_darkmode&sanitize=true" align=middle width=28.047932549999988pt height=22.831056599999986pt/> to a subset of <img src="/docs/Scenes/tex/0b1666db7be254fa8998cf3a27c985bb.svg?invert_in_darkmode&sanitize=true" align=middle width=37.46952164999999pt height=22.831056599999986pt/> and solve for the remaining variable (e.g. choose <img src="/docs/Scenes/tex/12976e89aecd7544d72cc0e1be762587.svg?invert_in_darkmode&sanitize=true" align=middle width=43.78601369999999pt height=22.831056599999986pt/>, sovle for <img src="/docs/Scenes/tex/44bc9d542a92714cac84e01cbbb7fd61.svg?invert_in_darkmode&sanitize=true" align=middle width=8.68915409999999pt height=14.15524440000002pt/>, map <img src="/docs/Scenes/tex/42fb6671e7cd0870e192c254b9ab6e03.svg?invert_in_darkmode&sanitize=true" align=middle width=49.87425464999999pt height=22.465723500000017pt/>, <img src="/docs/Scenes/tex/2fc51e4b14b71b4013218ff011414820.svg?invert_in_darkmode&sanitize=true" align=middle width=47.416205099999985pt height=22.465723500000017pt/>, <img src="/docs/Scenes/tex/6cdedf141f3dcfe676d88f25575c9280.svg?invert_in_darkmode&sanitize=true" align=middle width=51.74647994999999pt height=22.465723500000017pt/>) for those values of <img src="/docs/Scenes/tex/d7093223b4d827e8c29d4ed84b7ae088.svg?invert_in_darkmode&sanitize=true" align=middle width=28.047932549999988pt height=22.831056599999986pt/>.

``` c#
private float3 HyperToricSection(float alpha, float beta, float h)
{
float a;
float b;
float c;

//since we are fixing one of the x,y,z,w values, we can find a value for a, b, or c and use alpha and beta to parameterize the other two.
switch (plane)
    {
case crossSectionPlane.x:
a = alpha;
b = beta;
c = math.asin(h/ (R + (P + math.cos(a)) * math.cos(b)));
    if (solutionA)
    {
        c = Mathf.PI - c;
    }
break;
        case crossSectionPlane.y:
a = alpha;
b = beta;
c = math.asin(h/ (P + math.cos(a)));
    if (solutionA)
    {
        c = Mathf.PI - c;
    }
break;
        case crossSectionPlane.z:
b = alpha;
c = beta;
a = math.asin(h);
    if (solutionA)
    {
        a = Mathf.PI - a;
    }
break;
        case crossSectionPlane.w:
a = alpha;
b = beta;
c = math.asin(h/ (R + (P + math.cos(a)) * math.cos(b)));
    if (solutionA)
    {
        c = Mathf.PI - c;
    }
break;
        default:
b = alpha;
c = beta;
a = math.asin(h);
    if (solutionA)
    {
        a = Mathf.PI - a;
    }
break;
    }

    float w = (R + (P + math.cos(a)) * math.cos(b)) * math.cos(c);
    float x = (R + (P + math.cos(a)) * math.cos(b)) * math.sin(c);
    float y = (P + math.cos(a)) * math.sin(b);
    float z = math.sin(a);

    switch (plane)
    {
        case crossSectionPlane.x:
            return new float3(w,y,z);
        case crossSectionPlane.y:
            return new float3(w,x,z);
        case crossSectionPlane.z:
            return new float3(x,y,w);
        case crossSectionPlane.w:
            return new float3(x,y,z);
        default:
            return new float3(0,0,0);
    }
}
```

```c#
        private void renderToricSection(float height)
        {
            Vector3[] verts = new Vector3[(n + 1) * (n - 1) + 1];

            //Array of 2D vectors for UV map of vertices
            Vector2[] uvs = new Vector2[verts.Length];
            float oneNth = 1f / ((float) n);

            //loop through n-1 times, since edges wrap around
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //index value computation
                    int idx = i + n * j;

                    //find radian value of length of curve
                    float alpha = i * oneNth * 2 * Mathf.PI;
                    float beta = j * oneNth * 2 * Mathf.PI;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = HyperToricSection(alpha, beta, height);
                }
            }
        }
```


## Nets

### Equaliaterial Triangle

The net of a triangle is three congruent line segments. In its unfolded state, the line segments are colinear. To fold the triangle net, hold the middle segment fixed and rotate the other two segments (clockwise and counterclockwise, respectively) by <img src="/docs/Scenes/tex/90ba29b77077491b320c9da207fbeceb.svg?invert_in_darkmode&sanitize=true" align=middle width=18.485245349999996pt height=27.77565449999998pt/> radians.

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

The net of a square is four congruent line segments. In its unfolded state, the line segments are colinear. To fold the square net, hold one of the middle segments fixed. Rotate the two adjacent segments around their respective endpoints by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> radians. The remaining segment is adjacent to one of the rotated segments (segment A).  Rotate that segment by ninety degrees around its joining endpoint, with respect to the direction of segment A.  In effect, this vertex is rotated by <img src="/docs/Scenes/tex/06798cd2c8dafc8ea4b2e78028094f67.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to its origional direction.

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

The net of a cube is a collection of six congruent squares.
One square remains fixed in the center.  
Four squares share an edge with the center square, and rotate around that edge by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> to fold up the net.
On one of those four squares, a final square is constructed sharing the opposite edge.
The final square is rotated by <img src="/docs/Scenes/tex/4eb105c60f67ef131323b9c0969450b8.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to the adjacent square, or <img src="/docs/Scenes/tex/06798cd2c8dafc8ea4b2e78028094f67.svg?invert_in_darkmode&sanitize=true" align=middle width=8.099960549999997pt height=22.853275500000024pt/> with respect to its origional orientation, around it's shared edge, and becomes the "top" square face.
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

The net of a tetrahedron is a collection of four congruent equilaterial triangles. One traingle remains fixed in the center, and each of the remaining triangles shares their bottom edge with the center triangle. All triangles except the center rotate up around their shared edge by <img src="/docs/Scenes/tex/d198a46d6c0cc6400dd3ea7ebfe0c709.svg?invert_in_darkmode&sanitize=true" align=middle width=55.237455899999986pt height=27.77565449999998pt/>.

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

### Regular 5-cell (Hypertetrahedron)

The regular 5-cell is a collection of four congruent regular tetrahedrons. One regular tetrahedron is fixed at the center. Four tetrahedrons are constructed to share a face with the center tetrahedron.

In this algorithm we calculate the folded state of the five-cell net to determine the angle needed to fully fold the net into a five-cell. Below the apex is the value of the folded state of the vertex opposite to the face shared with the center tetrahedron for each of the outer tetrahedrons.

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
//TODO consider making the initial projection onto n

//apex of tetrahedron for each additional tetrahedron(from fases of first) foldling by degree t
float4 center1 = (result[0] + result[1] + result[2]) / 3f;
float4 dir1 = center1 - result[3];
result[4] = center1 + Math.Operations.rotate(dir1, apex - center1, degreeFolded);

float4 center2 = (result[0] + result[2] + result[3]) / 3f;
float4 dir2 = center2 - result[1];
result[5] = center2 + Math.Operations.rotate(dir2,apex - center2, degreeFolded);

float4 center3 = (result[0] + result[1] + result[3]) / 3f;
float4 dir3 = center3 - result[2];
result[6] = center3 + Math.Operations.rotate(dir3, apex - center3, degreeFolded);

float4 center4 = (result[1] + result[2] + result[3]) / 3f;
float4 dir4 = center4 - result[0];
result[7] = center4 + Math.Operations.rotate(dir4, apex - center4, degreeFolded);
```

### Regular 8-cell

The net of a regular 8-cell (or hypercube) is a collection of eight congruent cubes. One cube is fixed at the center. Six cubes are constructed from the faces of the center cube. The remaining cube is constructed on the opposite face of one of the six outer cubes.

To fold the net of the hypercube, the verticies of the opposite faces of an outer cube are rotated in a 2-plane (whose basis is constructed from a vector orthagonal to all of the faces of the adjacent cube and the cross-product of the sides of the face shared with an adjacent cube). If two faces are shared with an adjacent cube, choose the adjacent cube to be the core (center) cube. A vertex is rotated around the vertex of the opposite face which shares a segment of the cube.

Note that the 8th cube (below: down of down) is rotated with respect to its adjacent cube and not the core cube. Effectively, it is rotated by twice the angle, with respect to the lower cube.

The cubes of the regular 8-cell net are folded by 90-degrees to construct the regular 8-cell.

```c#
float4[] result = new float4[4 * 9];

//core or center cube (does not fold)
result[0] = (up + right + forward)/2f;
result[1] = (up + left + forward)/2f;
result[2] = (up + left + back)/2f;
result[3] = (up + right + back)/2f;

result[4] = (down + right + forward) / 2f;
result[5] = (down + left + forward) / 2f;
result[6] = (down + left + back) / 2f;
result[7] = (down + right + back) / 2f;

//above up face.            
result[8] = result[0] + Math.Operations.rotate(up, wForward, degreeFolded);
result[9] = result[1] + Math.Operations.rotate(up, wForward, degreeFolded);
result[10] = result[2] + Math.Operations.rotate(up, wForward, degreeFolded);
result[11] = result[3] + Math.Operations.rotate(up, wForward, degreeFolded);

//below down face
result[12] = result[4] + Math.Operations.rotate(down, wForward, degreeFolded);
result[13] = result[5] + Math.Operations.rotate(down, wForward, degreeFolded);
result[14] = result[6] + Math.Operations.rotate(down, wForward, degreeFolded);
result[15] = result[7] + Math.Operations.rotate(down, wForward, degreeFolded);

//right of right face;
result[16] = result[0] + Math.Operations.rotate(right, wForward, degreeFolded);
result[17] = result[3] + Math.Operations.rotate(right, wForward, degreeFolded);
result[18] = result[7] + Math.Operations.rotate(right, wForward, degreeFolded);
result[19] = result[4] + Math.Operations.rotate(right, wForward, degreeFolded);

//left of left face
result[20] = result[1] + Math.Operations.rotate(left, wForward, degreeFolded);
result[21] = result[2] + Math.Operations.rotate(left,wForward, degreeFolded);
result[22] = result[6] + Math.Operations.rotate(left,wForward, degreeFolded);
result[23] = result[5] + Math.Operations.rotate(left,wForward, degreeFolded);

//forward of forward face.
result[24] = result[0] + Math.Operations.rotate(forward, wForward, degreeFolded);
result[25] = result[1] + Math.Operations.rotate(forward, wForward, degreeFolded);
result[26] = result[5] + Math.Operations.rotate(forward, wForward, degreeFolded);
result[27] = result[4] + Math.Operations.rotate(forward,wForward, degreeFolded);

//back of back face.
result[28] = result[2] + Math.Operations.rotate(back, wForward, degreeFolded);
result[29] = result[3] + Math.Operations.rotate(back, wForward, degreeFolded);
result[30] = result[7] + Math.Operations.rotate(back, wForward, degreeFolded);
result[31] = result[6] + Math.Operations.rotate(back, wForward, degreeFolded);

//down of double down.
result[32] = result[12] + Math.Operations.rotate(down, wForward, 2f*degreeFolded);
result[33] = result[13] + Math.Operations.rotate(down, wForward, 2f*degreeFolded);
result[34] = result[14] + Math.Operations.rotate(down, wForward, 2f*degreeFolded);
result[35] = result[15] + Math.Operations.rotate(down, wForward, 2f*degreeFolded);
```

## Projections

## Orthographic Projection from 4D to 3D

```c#
Unity.Mathematics.math.normalize(inputBasis.c0);
Unity.Mathematics.math.normalize(inputBasis.c1);
Unity.Mathematics.math.normalize(inputBasis.c2);

return new Unity.Mathematics.float3(Unity.Mathematics.math.dot(v, inputBasis.c0),
    Unity.Mathematics.math.dot(v, inputBasis.c1),
    Unity.Mathematics.math.dot(v, inputBasis.c2));
```

## Parallel Projection from 4D to 3D (Hollasch, 1991)

```c#
//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
T = 1f / Unity.Mathematics.math.tan(Vangle.Value / 2f);
tmp = v - eyePosition.Value;
basis = calc4Matrix(eyePosition.Value, inputBasis);
S = T / Unity.Mathematics.math.dot(v, basis.c3);

return new Unity.Mathematics.float3(S * Unity.Mathematics.math.dot(tmp, basis.c0),
    S * Unity.Mathematics.math.dot(tmp, basis.c1),
    S * Unity.Mathematics.math.dot(tmp, basis.c2));
```

## Projective Projection from 4D to 3D (Hollasch, 1991)

```c#
//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
S = 1f / viewingRadius.Value;
tmp = v - eyePosition.Value;
basis = calc4Matrix(eyePosition.Value, inputBasis);

return new Unity.Mathematics.float3(S * Unity.Mathematics.math.dot(tmp, basis.c0),
    S * Unity.Mathematics.math.dot(tmp, basis.c1),
    S * Unity.Mathematics.math.dot(tmp, basis.c2));
```

## Stereographic Projection from 4D to 3D 
We consider a hypercube or a 5-cell where the verticies are arranged on a sphere with radius <img src="/docs/Scenes/tex/89f2e0d2d24bcf44db73aab8fc03252c.svg?invert_in_darkmode&sanitize=true" align=middle width=7.87295519999999pt height=14.15524440000002pt/>.
Then, the stereographic projection of a vertex is the intersection of the line through the north pole <img src="/docs/Scenes/tex/41b6c06aff103f46a64d6965e91ed06f.svg?invert_in_darkmode&sanitize=true" align=middle width=67.23366705pt height=24.65753399999998pt/> and the vertex and the <img src="/docs/Scenes/tex/cf4b72df1e2b52767fb792da8da43986.svg?invert_in_darkmode&sanitize=true" align=middle width=38.50445939999999pt height=21.18721440000001pt/> hyperplane.

That is, <img src="/docs/Scenes/tex/a7dbabc722e053fe8366a461a1b19ee0.svg?invert_in_darkmode&sanitize=true" align=middle width=450.15178890000004pt height=33.20539859999999pt/>

```c#
 float r = Math.Operations.magnitude(v);
 //assume north pole is at (0,0,0,1);
 float4 north = new float4(0,0,0,1)*r;
 float4 vPrime = (north-v)*(Math.Operations.magnitude(north)/(Unity.Mathemathics.math.dot((north - v),Unity.Mathematics.normalize(north))+north;
  return new float3 (vPrime.x, vPrime.y, vPrime.z);
```

### Projections of Segments and Quads
Each segment and quad (mesh) is projected according to the given method. In the case of stereogrphaic projection, segments and quads are inflated to lie on the surface of the hypersphere.

A triangle is treated as a quad where two verticies lie at the same point. This may be refactored in the future. As a result, the density of subtriangles is greater near one of the verticies.

```c#
public static Unity.Mathematics.float3[] projectSegment(Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
            float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
        {
            Unity.Mathematics.float3[] result = new Unity.Mathematics.float3[n];
            for(int i = 0; i < n; i ++)
            {
                Unity.Mathematics.float4 v = ((float)i/((float)n-1f))*(b-a)+a;
                if(method == ProjectionMethod.stereographic)
                {
                    //assume center == Vector4.zero;
                    //assume a and b are on surface of sp
                    v = Unity.Mathematics.math.normalize(v)*Math.Operations.magnitude(a);
                }    
                result[i] = projectDownDimension(v,inputBasis,method,Vangle, eyePosition, viewingRadius);
            }    
            return result;
        }
```

```c#
         public static Unity.Mathematics.float3[] projectTriangle  (Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, 
                                                               Unity.Mathematics.float4 c,
                                                               int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
                                                                float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
         {
             return  projectQuad  (a, b,c,c, n, inputBasis, method, Vangle, eyePosition, viewingRadius);
         }
```


```c#
        public static Unity.Mathematics.float3[] projectQuad  (Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, 
                                                               Unity.Mathematics.float4 c, Unity.Mathematics.float4 d,
                                                               int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
                                                                float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
         {
             Unity.Mathematics.float4[] result = new Unity.Mathematics.float4[n];

             for(int i = 0; i < n; i++){
                 Unity.Mathematics.float4 a1 = ((float)i/((float)n-1f))*(b-a)+a;
                 Unity.Mathematics.float4 b1 = ((float)i/((float)n-1f))*(c-d)+d;
                if(method == ProjectionMethod.stereographic)
                {
                    //assume center == Vector4.zero;
                    //assume a and b are on surface of sp
                    a1 = Unity.Mathematics.math.normalize(a1)*Math.Operations.magnitude(a);
                    b1 = Unity.Mathematics.math.normalize(b1)*Math.Operations.magnitude(b);
                }   
                 Unity.Mathematics.float4[] seg = projectSegment(a1, b1, n, inputBasis, method,
                                                        Vangle, eyePosition, viewingRadius);
                 Copy(seg, 0, result, i*n,n);
             }
             return result;
         }
```


## Projection from 3D to 2D
We have used a virtual camera and rendered that camera's perspective on a plane surface, using the UnityEngine to project 3D figures into 2D. This avoids manipulation of meshes and line renderers and allows for Parallel and Projective perspecitves.

## Mathematics Library Extensions

We have extended Unity.Mathematics to include serveral operations for float3 and float4. While System.Mathematics duplicates some of these operations, they require the varaible to be cast as a Vector3.

### Angle between two float3
```c#
public static float Angle(float3 from, float3 to)
{
    return math.acos(math.dot(math.normalize(from), math.normalize(to)));
}

```

### Magnitude of a float3 (2-Norm)
```c#
public static float magnitude(float3 v)
{
    return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
}
```

### Magnitude of a float4 (2-Norm)

```c#
public static float magnitude(float4 v)
{
    return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
}
```

### Tripple Cross Product for Float4 (Hollasch, 1991)
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

### From --> To Rotation for float4
Rotation of a from vecotor in the plane of (from X to) towards to for a measure of theta degrees
```c#
public static float4 rotate(float4 from, float4 to, float theta)
{
    float4 basis0 = math.normalize(from);
    float4 basis1 = math.normalize(to - project(to,from));
    return rotate(from, basis0, basis1, theta);
}
```

### Rotation for float4
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

### Projection for float4
```c#
public static float4 project(float4 v, float4 dir)
{
    return math.dot(v, dir) * math.normalize(dir);
}
```


## References
Brisson, D. W. (1978). Hypergraphics: visualizing complex relationships in art, science, and technology. Boulder, Colo.: Published by Westview Press for the American Association for the Advancement of Science.

Hartley, M. (2007). 4D torus. Retrieved July 22, 2019, from http://www.dr-mikes-maths.com/4d-torus.html

Hollasch, Steven (1991).  "Four-Space Visualization of 4D Objects."  Masters Thesis, Arizona State University.

Makholm, Henning (2015).  "Would the cross section of a hypersphere be a sphere?" Mathematics Stack Exchange.  Retrieved from https://math.stackexchange.com/questions/1159613/would-the-cross-section-of-a-hypersphere-be-a-sphere

Moroni, L. (2017). The toric sections: a simple introduction. ArXiv:1708.00803 [Math]. Retrieved from http://arxiv.org/abs/1708.00803

 Weisstein, Eric W. (n.d.) "Spiric Section." From MathWorld--A Wolfram Web Resource. http://mathworld.wolfram.com/SpiricSection.html 
