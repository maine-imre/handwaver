public class OrionControlPanel : UnityEngine.MonoBehaviour
{
    public Leap.Unity.Interaction.InteractionSlider bodyScaleSlider;

    public Leap.Unity.Interaction.InteractionButton[] buttons = new Leap.Unity.Interaction.InteractionButton[13];
    private VerletV3 ControlScript;
    public Leap.Unity.Interaction.InteractionSlider simulationScaleSlider;
    public Leap.Unity.Interaction.InteractionSlider timeSlider;

    // Use this for initialization
    private void Start()
    {
        ControlScript = (VerletV3) GetComponent("VerletV3");
        buttons[0].OnPress += teleportToSol;
        buttons[1].OnPress += teleportToMercury;
        buttons[2].OnPress += teleportToVenus;
        buttons[3].OnPress += teleportToEarth;
        buttons[4].OnPress += teleportToMars;
        buttons[5].OnPress += teleportToJupiter;
        buttons[6].OnPress += teleportToSaturn;
        buttons[7].OnPress += teleportToUranus;
        buttons[8].OnPress += teleportToNeptune;
        buttons[9].OnPress += teleportToPluto;
        buttons[10].OnPress += teleportToMoon;
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
        ControlScript.timeStep = timeSlider.HorizontalSliderValue;
        ControlScript.bodyScale = bodyScaleSlider.HorizontalSliderValue;
        ControlScript.scale = simulationScaleSlider.HorizontalSliderValue;
    }
}