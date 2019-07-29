using UnityEngine;

public class addTexture : MonoBehaviour
{
    public Material[] materials;

    private string name = "";

    private Material thisMat;

    // Use this for initialization
    private void Start()
    {
        name = gameObject.name;
        switch (name)
        {
            //Getting the month correct
            case "Earth":
                Debug.Log("Earth");
                foreach (var item in materials)
                    if (item.name == "earth")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Moon":
                Debug.Log("Moon");
                foreach (var item in materials)
                    if (item.name == "moon")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Mars":
                Debug.Log("Mars");
                foreach (var item in materials)
                    if (item.name == "mars")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Jupiter":
                Debug.Log("Jupiter");
                foreach (var item in materials)
                    if (item.name == "jupiter")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Saturn":
                Debug.Log("Saturn");
                foreach (var item in materials)
                    if (item.name == "saturn")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Venus":
                Debug.Log("Venus");
                foreach (var item in materials)
                    if (item.name == "venus")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Mercury":
                Debug.Log("Mercury");
                foreach (var item in materials)
                    if (item.name == "mercury")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Uranus":
                Debug.Log("Uranus");
                foreach (var item in materials)
                    if (item.name == "uranus")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Neptune":
                Debug.Log("Neptune");
                foreach (var item in materials)
                    if (item.name == "neptune")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            case "Pluto":
                Debug.Log("Pluto");
                foreach (var item in materials)
                    if (item.name == "pluto")
                        thisMat = item;

                ((MeshRenderer) gameObject.GetComponent("MeshRenderer")).material = thisMat;
                break;
            default:
                Debug.Log("Other");
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}