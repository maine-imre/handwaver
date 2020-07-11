/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;
using System.Linq;

namespace IMRE.HandWaver
{
/// <summary>
/// Points that snap to other geometeric objects
/// will be depreciated with new geometery kernel.
/// </summary>
	class SnappablePoint : DependentPoint
	{

		#region Constructors
            public static SnappablePoint Constructor()
						{
							return GameObject.Instantiate(PrefabManager.GetPrefab("SnappablePoint")).GetComponent<SnappablePoint>();
						}
        #endregion
		private List<AbstractGeoObj> relativeObjects;
        private List<float> relativeWeights;


		internal AbstractGeoObj attachedObject{
			get
			{
				return _attachedObject;
			}
			set
			{
				if (value!= null)
				{
					SnapToFigure(value);
                    setupRelativePosition(value);
				}
				_attachedObject = value;
			}
		}

        private void setupRelativePosition(AbstractGeoObj value)
        {
            switch (value.figType)
            {
                //case GeoObjType.point:
                //    break;
                case GeoObjType.line:
                    AbstractLineSegment line = value.GetComponent<AbstractLineSegment>();
                    relativeWeights.Add(Vector3.Magnitude(Position3 - line.vertex0) / Vector3.Magnitude(line.vertex0 - line.vertex1));
                    relativeWeights.Add(Vector3.Magnitude(Position3 - line.vertex1) / Vector3.Magnitude(line.vertex0 - line.vertex1));
                    break;
                case GeoObjType.polygon:
                    Debug.LogWarning("Polygonsn not yet supported for Relative Movement");
                    break;
                //case GeoObjType.prism:
                //    break;
                //case GeoObjType.pyramid:
                //    break;
                case GeoObjType.circle:
                    AbstractCircle circle = value.GetComponent<AbstractCircle>();
                    relativeWeights.Add(Vector3.SignedAngle(circle.edgePos - circle.centerPos, this.Position3 - circle.centerPos, circle.normalDir));
                    break;
                case GeoObjType.sphere:
                    Vector3 direction = Vector3.Normalize(this.Position3 - value.GetComponent<AbstractSphere>().centerPosition);
                    relativeWeights.Add(direction.x);
                    relativeWeights.Add(direction.y);
                    relativeWeights.Add(direction.z);
                    break;
                //case GeoObjType.revolvedsurface:
                //    break;
                //case GeoObjType.torus:
                //    break;
                case GeoObjType.flatface:
                    Debug.LogWarning("Flatface not yet supported for Relative Movement");
                    break;
                case GeoObjType.straightedge:
                    Debug.LogWarning("Straightedge not yet supported for Relative Movement");
                    break;
                default:
                    Debug.LogWarning(value.figType.ToString() + " not supported for relative movement");
                    break;
            }
        }

        private AbstractGeoObj _attachedObject;

		internal override void SnapToFigure(AbstractGeoObj toObj)
		{
			Debug.Log(name + " is attempting to snap to " + toObj.name + ".");
			//if object and child objects are not being grasped
			if (!(this.GetComponent<InteractionBehaviour>().isGrasped))
			{///this object is being grasped
				switch (toObj.figType)
				{
					case GeoObjType.point:
						Position3 = toObj.Position3;
						break;
					case GeoObjType.line:
						Position3 = Vector3.Project(Position3 - toObj.Position3, toObj.GetComponent<AbstractLineSegment>().vertex0 - toObj.GetComponent<AbstractLineSegment>().vertex1) + toObj.Position3;
						break;
					case GeoObjType.polygon:
						Vector3 positionOnPlane = Vector3.ProjectOnPlane(Position3 - toObj.Position3, toObj.GetComponent<AbstractPolygon>().normDir) + toObj.Position3;
						if (toObj.GetComponent<AbstractPolygon>().checkInPolygon(positionOnPlane))
						{
							Position3 = positionOnPlane;
						}
						break;
					//case GeoObjType.prism:
					//	break;
					//case GeoObjType.pyramid:
					//	break;
					case GeoObjType.circle:
						Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(Position3 - toObj.Position3, toObj.GetComponent<AbstractCircle>().normalDir) + toObj.Position3;
						if (Vector3.Magnitude(positionOnPlane2 - toObj.Position3) == toObj.GetComponent<AbstractCircle>().Radius)
						{
							Position3 = positionOnPlane2;
						}
						break;
					case GeoObjType.sphere:
						Vector3 lineDir = Vector3.Normalize(Position3 - toObj.Position3);
						Position3 = toObj.GetComponent<AbstractSphere>().radius * lineDir + toObj.Position3;
						break;
					//case GeoObjType.revolvedsurface:
					//	break;
					//case GeoObjType.torus:
					//	break;
					case GeoObjType.flatface:
						Position3 = Vector3.ProjectOnPlane(Position3 - toObj.Position3, toObj.GetComponent<flatfaceBehave>().normalDir) + toObj.Position3;
						break;
					case GeoObjType.straightedge:
						Position3 = Vector3.Project(Position3 - toObj.Position3, toObj.GetComponent<straightEdgeBehave>().normalDir) + toObj.Position3;
						break;
					default:
						break;
				}
                setupRelativePosition(toObj);
			}
			else
			{
				switch (toObj.figType)
				{
					case GeoObjType.point:
						this.Position3 = toObj.Position3;
						break;
					case GeoObjType.line:
                        AbstractLineSegment line = toObj.GetComponent<AbstractLineSegment>();
                        this.Position3 = line.vertex0 * relativeWeights[0] + line.vertex1 * relativeWeights[1];
						break;
					case GeoObjType.polygon:
                        Debug.LogWarning("Polygon not yet supported for Relative Movement");
                        break;

					//case GeoObjType.prism:
					//	break;
					//case GeoObjType.pyramid:
					//	break;
					case GeoObjType.circle:
                        AbstractCircle circle = toObj.GetComponent<AbstractCircle>();
                        Vector3 basis1 = circle.edgePos - circle.centerPos;
                        Vector3 basis2 = Vector3.Cross(basis1, circle.normalDir);
                        Vector3.OrthoNormalize(ref basis1, ref basis2);
                        this.Position3 = circle.Radius * (basis1 * Mathf.Sin(relativeWeights[0]) + basis2 * Mathf.Cos(relativeWeights[0]))+circle.centerPos;
						break;
					case GeoObjType.sphere:
                        Vector3 direction = relativeWeights[0] * Vector3.right + relativeWeights[1] * Vector3.up + relativeWeights[2] * Vector3.forward;
                        this.Position3 = toObj.GetComponent<AbstractSphere>().radius * direction;
						break;
					//case GeoObjType.revolvedsurface:
					//	break;
					//case GeoObjType.torus:
					//	break;
					case GeoObjType.flatface:
                        Debug.LogWarning("Straightedge not yet supported for Relative Movement");
                        break;
					case GeoObjType.straightedge:
                        Debug.LogWarning("Straightedge not yet supported for Relative Movement");
                        break;
					default:
                        Debug.LogWarning(toObj.figType.ToString() + " not supported for relative movement");
						break;
				}
			}
		}
	}
}
