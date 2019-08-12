using System;
using System.Text.RegularExpressions;
using IMRE.HandWaver.Kernel.Geos;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace IMRE.HandWaver.Kernel.GeoGebraInterface
{
    [Serializable]
    public struct ggbOutput : IComponentData
    {
        internal NativeString512 cmd;
    }


    public class GeoGebraCommandEvalSystem : JobComponentSystem
    {


        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new GeoElementJob();

            return job.Schedule(this, inputDependencies);
        }

        //This is a template for the jobs we will use.  See GitHub issue for a list of all possible types.
        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [BurstCompile]
        private struct GeoElementJob : IJobForEach<GeoElement, ggbOutput>
        {
            // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
            public void Execute(ref GeoElement target, [ReadOnly] ref ggbOutput DataGGB)
            {

              
                //update data in GeoElement to reflect GGB Input
                //TODO
                
                //update Mesh/Line/etc. Data to reflect new Object Definition.
                //TODO
            }
        }
    }
}