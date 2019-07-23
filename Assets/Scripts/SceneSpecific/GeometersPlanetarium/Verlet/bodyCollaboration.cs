using UnityEngine;

namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class bodyCollaboration : MonoBehaviour
    {
        public float scale = 1; //Scale for every body in the scene
        public float sizeScale = 1; //The sale of the body size. 1 Is actual size
        public float timestep = 1; //Timestep for every body in the scene

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (timestep == 0) timestep = 0.0000001f;
            var
                massObject =
                    GameObject
                        .FindGameObjectsWithTag("massObject"); //Gathers all of the massObjects for updating
            for (var i = 0; i < massObject.Length; i++)
            {
                //Updates relivant datapoints
                var massiveBody = massObject[i].GetComponent<VerletObjectV1>();
                massiveBody.timeStep = timestep;
                massiveBody.scale = scale;
                var scaleValue = 6.68459e-09f * sizeScale * massiveBody.radius * scale;
                massObject[i].transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
        }
    }
}