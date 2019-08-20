using UnityEngine;

public class OrionControlPanel : MonoBehaviour
{
    private VerletV3 ControlScript;

    // Use this for initialization
    private void Start()
    {
        ControlScript = (VerletV3) GetComponent("VerletV3");
    }

    private void teleportToSol()
    {
        teleportTo("Sol");
    }

    private void teleportToMercury()
    {
        teleportTo("Mercury");
    }

    private void teleportToVenus()
    {
        teleportTo("Venus");
    }

    private void teleportToEarth()
    {
        teleportTo("Earth");
    }

    private void teleportToMars()
    {
        teleportTo("Mars");
    }

    private void teleportToJupiter()
    {
        teleportTo("Jupiter");
    }

    private void teleportToSaturn()
    {
        teleportTo("Saturn");
    }

    private void teleportToUranus()
    {
        teleportTo("Uranus");
    }

    private void teleportToNeptune()
    {
        teleportTo("Neptune");
    }

    private void teleportToPluto()
    {
        teleportTo("Pluto");
    }

    private void teleportToMoon()
    {
        teleportTo("Moon");
    }

    private void teleportTo(string body)
    {
        ControlScript.CenterBody = body;
    }

    // Update is called once per frame
    private void Update()
    {
        //ControlScript.timeStep = timeSlider.HorizontalSliderValue;
        //ControlScript.bodyScale = bodyScaleSlider.HorizontalSliderValue;
        //ControlScript.scale = simulationScaleSlider.HorizontalSliderValue;
    }
}