using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Rendering;
using IMRE.Math;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class RenderSingleThreaded : MonoBehaviour
{

    Dictionary<int, GameObject> DictElements = new Dictionary<int, GameObject>();
    
    void Update()
    {
        foreach (GeoElement geoElement in GeoElementDataBase.GeoElements.GetValueArray(Allocator.Persistent))
        {
            updateMesh(geoElement);
        }
    }
    
    public void updateMesh(GeoElement geo)
    {
        if (geo.Updated == DateTime.MinValue)
        {
            return;
        }
                
        // Store a local copy of the render mesh.
        var newMesh = new Mesh();

        // Switch through the types of elements. Update the mesh based on type.
        switch (geo.Type)
        {
            case ElementType.point:
                newMesh = GeoElementRenderLib.Point(geo.F0);
                break;
            case ElementType.line:
                newMesh = GeoElementRenderLib.Segment(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                    GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                    GeoElementDataBase.GeoElements[geo.Deps[0]].F0);
                break;
            case ElementType.plane:
                newMesh =
                    GeoElementRenderLib.Plane(GeoElementDataBase.GeoElements[geo.Deps[0]].F0, geo.F0);
                break;
            case ElementType.sphere:
                newMesh = GeoElementRenderLib.Sphere(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                    (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                     GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude());
                break;
            case ElementType.circle:
                newMesh = GeoElementRenderLib.Circle(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                    (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                     GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude(), geo.F0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //Assign updated rendermesh to the geoelement.
        //GeoElementDataBase.AssignMesh(geo.ElementId, newRenderMesh.mesh);
        if (DictElements.ContainsKey(geo.ElementId))
        {
            DictElements[geo.ElementId].GetComponent<MeshFilter>().mesh = newMesh;
        }
        else
        {
            GameObject newGameObject = new GameObject();
            newGameObject.AddComponent<MeshFilter>();
            newGameObject.AddComponent<MeshRenderer>();
            newGameObject.GetComponent<MeshFilter>().mesh = newMesh;
            DictElements[geo.ElementId] = newGameObject;
        }
        
    }
    
}