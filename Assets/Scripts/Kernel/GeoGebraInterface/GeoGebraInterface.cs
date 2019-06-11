using System;
using IMRE.HandWaver.Kernel.Geos;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.GeoGebraInterface
{

    [Serializable]
    public struct ggbOutput : IComponentData
    {
        private string cmd;
    }


    public class GeoGebraInterface : JobComponentSystem
    {

        //This is a template for the jobs we will use.  See GitHub issue for a list of all possible types.
        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [BurstCompile]
        struct PointDataJob : IJobForEach<GeoElement, ggbOutput>
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

        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new PointDataJob
            {
                //define data within the job based on stream from GGB.
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}