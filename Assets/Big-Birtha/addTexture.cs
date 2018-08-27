using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addTexture : MonoBehaviour {

	private string name = "";
	private Material thisMat;
	public Material[] materials;
	// Use this for initialization
	void Start () {
		name = gameObject.name;
		switch(name){											//Getting the month correct
				case "Earth":
					Debug.Log("Earth");
					foreach (Material item in materials)
					{
						if(item.name == "earth"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Moon":
					Debug.Log("Moon");
					foreach (Material item in materials)
					{
						if(item.name == "moon"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Mars":
					Debug.Log("Mars");
					foreach (Material item in materials)
					{
						if(item.name == "mars"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Jupiter":
					Debug.Log("Jupiter");
					foreach (Material item in materials)
					{
						if(item.name == "jupiter"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Saturn":
					Debug.Log("Saturn");
					foreach (Material item in materials)
					{
						if(item.name == "saturn"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Venus":
					Debug.Log("Venus");
					foreach (Material item in materials)
					{
						if(item.name == "venus"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Mercury":
					Debug.Log("Mercury");
					foreach (Material item in materials)
					{
						if(item.name == "mercury"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Uranus":
					Debug.Log("Uranus");
					foreach (Material item in materials)
					{
						if(item.name == "uranus"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Neptune":
					Debug.Log("Neptune");
					foreach (Material item in materials)
					{
						if(item.name == "neptune"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				case "Pluto":
					Debug.Log("Pluto");
					foreach (Material item in materials)
					{
						if(item.name == "pluto"){
							thisMat = item;
						}
					}
					((MeshRenderer)gameObject.GetComponent("MeshRenderer")).material = thisMat;
					break;
				default:
					Debug.Log("Other");
					break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
