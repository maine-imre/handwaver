using System;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Rendering;
using IMRE.Math;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;

public class RenderSystem : JobComponentSystem
{
    [BurstCompile]
    private struct RenderSystemJob : IJobForEach<GeoElement>
    {
        public void Execute([ChangedFilter] ref GeoElement geo)
        {
            // Store a local copy of the render mesh.
            RenderMesh newRenderMesh = geo.RenderMesh;
            
            // Switch through the types of elements. Update the mesh based on type.
            switch (geo.Type)
            {
                case ElementType.point:
                    newRenderMesh.mesh = GeoElementRenderLib.Point(geo.F0);
                    break;
                case ElementType.line:
                    newRenderMesh.mesh = GeoElementRenderLib.Line(GeoElementDataBase.GeoElements[geo.Deps[0]].F0, 
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 - GeoElementDataBase.GeoElements[geo.Deps[0]].F0));
                    break;
                case ElementType.plane:
                    newRenderMesh.mesh =
                        GeoElementRenderLib.Plane(GeoElementDataBase.GeoElements[geo.Deps[0]].F0, geo.F0);
                    break;
                case ElementType.sphere:
                    newRenderMesh.mesh = GeoElementRenderLib.Sphere(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                         GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude());
                    break;
                case ElementType.circle:
                    newRenderMesh.mesh = GeoElementRenderLib.Circle(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                         GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude(), geo.F0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Assign updated rendermesh to the geoelement.
            geo.RenderMesh = newRenderMesh;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new RenderSystemJob();

        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}