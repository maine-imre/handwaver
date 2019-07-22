namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class objectController : UnityEngine.MonoBehaviour
    {
        public double counter;
        public float dt = 0.001f;
        public GravationalObject[] objects;

        public double stepsPerFrame = 28800;

        // Use this for initialization
        private void Start()
        {
            counter = 0;
        }

        private void test()
        {
            for (int i = 0; i < objects.Length; i++) objects[i].step1(dt);
            for (int i = 0; i < objects.Length; i++)
            {
                for (int r = 0; r < objects.Length; r++)
                {
                    if (i != r)
                        objects[i].step2(objects[r], dt);
                }
            }

            for (int i = 0; i < objects.Length; i++) objects[i].step3(dt);
        }

        // Update is called once per frame
        private void Update()
        {
            for (int i = 0; i < stepsPerFrame; i++) test();
            counter += stepsPerFrame * dt;
        }
    }
}