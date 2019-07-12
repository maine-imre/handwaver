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

### Intersection of a plane and a torus

### Intersection of a hyperplane and a hypersphere

### Intersection of a hyperplane and a three-torus


## Nets

### Triangle

### Square

### Cube

### Tetrahedron

### 5-cell

### 8-cell

## Net Folding

### Triangle

### Square

### Cube

### Tetrahedron

### 5-cell

### 8-cell

## Orthogrpahic Projection from 4D to 3D


##References
