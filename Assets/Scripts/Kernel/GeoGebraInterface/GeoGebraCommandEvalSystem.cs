using JobForEachExtensions = Unity.Entities.JobForEachExtensions;

namespace IMRE.HandWaver.Kernel.GeoGebraInterface
{
    [System.SerializableAttribute]
    public struct ggbOutput : Unity.Entities.IComponentData
    {
        internal Unity.Entities.NativeString512 cmd;
    }

    public class GeoGebraCommandEvalSystem : Unity.Entities.JobComponentSystem
    {
        // OnUpdate runs on the main thread.
        protected override Unity.Jobs.JobHandle OnUpdate(Unity.Jobs.JobHandle inputDependencies)
        {
            GeoElementJob job = new GeoElementJob();

            return JobForEachExtensions.Schedule(job, this, inputDependencies);
        }

        //This is a template for the jobs we will use.  See GitHub issue for a list of all possible types.
        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [Unity.Burst.BurstCompileAttribute]
        private struct GeoElementJob : Unity.Entities.IJobForEach<IMRE.HandWaver.Kernel.Geos.GeoElement, ggbOutput>
        {
            // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
            public void Execute(ref IMRE.HandWaver.Kernel.Geos.GeoElement target,
                [Unity.Collections.ReadOnlyAttribute] ref ggbOutput DataGGB)
            {
                //update data in GeoElement to reflect GGB Input
                //TODO

                //update Mesh/Line/etc. Data to reflect new Object Definition.
                //TODO
            }
        }
    }
}