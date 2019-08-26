using System;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Rendering;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

public class RenderSystem : JobComponentSystem
{

    [BurstCompile]
    struct RenderSystemJob : IJobForEach<GeoElement>
    {
        
        public void Execute([ChangedFilter] ref GeoElement geo)
        {
            //TODO: After merge with render branch, link these to the proper functions within GeoElementRenderLib.
            /*switch (geo.Type)
            {
                case ElementType.point:
                    geo.RenderMesh.mesh = GeoElementRenderLib.
                    break;
                case ElementType.line:
                    break;
                case ElementType.plane:
                    break;
                case ElementType.sphere:
                    break;
                case ElementType.circle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/
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