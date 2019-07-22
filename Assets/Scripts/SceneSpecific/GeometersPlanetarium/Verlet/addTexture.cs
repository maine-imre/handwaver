public class addTexture : UnityEngine.MonoBehaviour
{
    public UnityEngine.Material[] materials;

    private string name = "";

    private UnityEngine.Material thisMat;

    // Use this for initialization
    private void Start()
    {
        name = gameObject.name;
        switch (name)
        {
            //Getting the month correct
            case "Earth":
                UnityEngine.Debug.Log("Earth");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "earth") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Moon":
                UnityEngine.Debug.Log("Moon");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "moon") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Mars":
                UnityEngine.Debug.Log("Mars");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "mars") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Jupiter":
                UnityEngine.Debug.Log("Jupiter");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "jupiter") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Saturn":
                UnityEngine.Debug.Log("Saturn");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "saturn") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Venus":
                UnityEngine.Debug.Log("Venus");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "venus") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Mercury":
                UnityEngine.Debug.Log("Mercury");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "mercury") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Uranus":
                UnityEngine.Debug.Log("Uranus");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "uranus") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Neptune":
                UnityEngine.Debug.Log("Neptune");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "neptune") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Pluto":
                UnityEngine.Debug.Log("Pluto");
                foreach (UnityEngine.Material item in materials)
                {
                    if (item.name == "pluto") thisMat = item;
                }

                ((UnityEngine.MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            default:
                UnityEngine.Debug.Log("Other");
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}