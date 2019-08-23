using UnityEngine;
using UnityEngine.UI;

namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class VerletV2StressTestControl : MonoBehaviour
    {
        private Vector3 generatePosition;
        private int internalNumber;
        public Transform massiveBody;
        public int numberOfBodies;
        public Text text;
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
            for (var i = 0; i < numberOfBodies - internalNumber; i++)
            {
                ((VerletObjectV2) massiveBody.GetComponent("VerletObjectV2")).mass =
                    Random.Range(100000, 1000000000000);
                massiveBody.transform.position = new Vector3(Random.Range(-10, 10),
                    Random.Range(-10, 10), Random.Range(-10, 10));
                Instantiate(massiveBody);
                internalNumber += 10;
            }
        }
    }
}