namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class VerletV2StressTestControl : UnityEngine.MonoBehaviour
    {
        private UnityEngine.Vector3 generatePosition;
        private int internalNumber;
        public UnityEngine.Transform massiveBody;
        public int numberOfBodies;
        public UnityEngine.UI.Text text;
        private VerletObjectV1 vO;

        // Use this for initialization
        private void Start()
        {
            vO = massiveBody.GetComponent<VerletObjectV1>();
        }

        // Update is called once per frame
        private void Update()
        {
            text.text = (numberOfBodies / 5).ToString();
            numberOfBodies += 1;
            for (int i = 0; i < numberOfBodies - internalNumber; i++)
            {
                ((VerletObjectV2) massiveBody.GetComponent("VerletObjectV2")).mass =
                    UnityEngine.Random.Range(100000, 1000000000000);
                massiveBody.transform.position = new UnityEngine.Vector3(UnityEngine.Random.Range(-10, 10),
                    UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
                Instantiate(massiveBody);
                internalNumber += 10;
            }
        }
    }
}