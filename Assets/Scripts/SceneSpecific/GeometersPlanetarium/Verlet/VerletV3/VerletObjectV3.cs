public class VerletObjectV3 : UnityEngine.MonoBehaviour
{
    private float bodyScale = 1;
    private UnityEngine.Vector3d CenterBodyOffset;
    private VerletV3 controlScript;

    public UnityEngine.Vector3d
        initialVelocity; //The initial velocity of the body (note: doesn't change once the calculation starts)

    public UnityEngine.Vector3 inputPosition;
    public UnityEngine.Vector3 inputVelocity;
    public float mass; //The mass of the body
    private double oldSecondCounter;
    public UnityEngine.Vector3d position; //The position vector of the body
    public UnityEngine.Vector3d previousPosition; //The previous position of the body
    public float radius; //The radius of the body
    public float rotation;
    private float scale = 1;
    private UnityEngine.GameObject sceneController;
    public float sunscale = 1;
    private double timePassed = 0;

    #region Constructors

    public static VerletObjectV3 Constructor()
    {
        UnityEngine.GameObject go =
            Instantiate(UnityEngine.Resources.Load<UnityEngine.GameObject>("Prefabs/Space/VerletObjectV3"));
        return go.GetComponent<VerletObjectV3>();
    }

    #endregion

    // Use this for initialization
    private void Start()
    {
        sceneController = UnityEngine.GameObject.Find("SceneController"); //To keep various variables up to date
        controlScript = (VerletV3) sceneController.GetComponent("VerletV3");
        position = V3toV3D(inputPosition); //Convert input floats to doubles
        initialVelocity = V3toV3D(inputVelocity); //Ditto above
        initialVelocity =
            initialVelocity * 0.00001157407407 *
            controlScript.timeStep; //Unit conversion of Velocity from AU/Day to AU/Sec then multiplys by timestep
        previousPosition = position - initialVelocity; //Calculates previous position based on initial velocity
    }

    // Update is called once per frame
    private void Update()
    {
        double time = controlScript.masterTimeCounter;
        scale = controlScript.scale;
        if (rotation != 0)
        {
            transform.rotation *=
                UnityEngine.Quaternion.AngleAxis((float) (time - oldSecondCounter) * (360 / (rotation * 86400)),
                    UnityEngine.Vector3.up);
            oldSecondCounter = time;
        }

        bodyScale = controlScript.bodyScale * controlScript.scale;
        CenterBodyOffset = controlScript.CenterBodyOffset;

        UnityEngine.Vector3
            outputPosition = V3DtoV3(position - CenterBodyOffset); //For displaying position in Unity editor
        transform.position = outputPosition * scale; //Updates the position of the unity object

        float diameter = 1;
        if (gameObject.name != "Sol")
            diameter = 0.0000000067f * 2 * bodyScale * radius;
        else
            diameter = 0.0000000067f * 2 * bodyScale / sunscale * radius;
        transform.localScale = new UnityEngine.Vector3(diameter, diameter, diameter);
    }

    public void newPos(UnityEngine.Vector3d np)
    {
        previousPosition = position; //Saves the position for next calculation
        position = np;
    }

    private UnityEngine.Vector3 V3DtoV3(UnityEngine.Vector3d value)
    {
        //Convert from Vector3d to Vector3
        float xVal = (float) value.x;
        float yVal = (float) value.y;
        float zVal = (float) value.z;
        return new UnityEngine.Vector3(xVal, yVal, zVal);
    }

    private UnityEngine.Vector3d V3toV3D(UnityEngine.Vector3 value)
    {
        //Convert from Vector3 to Vector3d
        double xVal = value.x;
        double yVal = value.y;
        double zVal = value.z;
        return new UnityEngine.Vector3d(xVal, yVal, zVal);
    }
}