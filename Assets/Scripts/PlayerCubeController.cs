using UnityEngine;

public class PlayerCubeController : MonoBehaviour {
    private OVRInput.Controller activeController;
    private OVRInput.Button activeButton;

    private CubeFactory cFactory;
    private ResultsPrinter rPrinter;

    public Material resetUnselected;
    public Material resetSelected;
    public GameObject resetCube; 

    private float gain; 

    private bool waitingToStart;
    private float timeToSubtract;
    private float timeForCube;
    private bool colliding;
    private bool resetting;
    private bool waitingForReset;
    private float timeColliding;
    private int numOfCollisions; 
    
    // Use this for initialization
    void Start() {
        cFactory = GameObject.Find("CubeFactory").GetComponent<CubeFactory>();
        rPrinter = GameObject.Find("Results").GetComponent<ResultsPrinter>();
        waitingToStart = true;
        timeToSubtract = 0.0f;
        timeForCube = 0.0f;
        colliding = false;
        resetting = false;
        waitingForReset = false;
        timeColliding = 0.0f;
        numOfCollisions = 0;

    }

    // Update is called once per frame
    void Update() {
        UpdateControllerPosition();
        
        if (waitingToStart) {
            if (resetting && OVRInput.Get(activeButton)) {
                resetCube.SetActive(false);
                resetting = false;
                cFactory.NextCube();
                timeToSubtract = Time.timeSinceLevelLoad;
                resetCube.GetComponent<Renderer>().material = resetUnselected;
                waitingToStart = false;
            }
        }
        else {
            if (waitingForReset)
            {
                if (resetting && OVRInput.Get(activeButton))
                {
                    resetCube.SetActive(false);
                    resetCube.GetComponent<Renderer>().material = resetUnselected;
                    waitingForReset = false;
                    timeForCube = 0.0f;
                    numOfCollisions = 0;
                    timeColliding = 0.0f;
                    colliding = false;
                    resetting = false;
                    cFactory.NextCube();
                }
            }
            else
            {
                timeForCube += Time.deltaTime;

                if (colliding)
                {
                    timeColliding += Time.deltaTime;
                }

                Vector3 cubePos = cFactory.GetCubePosition();
                Vector3 cubeScale = cFactory.GetCubeScale();
                float dist = Vector3.Distance(transform.position, cubePos);
                rPrinter.RecieveData(Time.timeSinceLevelLoad - timeToSubtract, cFactory.GetCubeNumber(), timeForCube,
                    numOfCollisions, timeColliding, dist, transform.position, cubePos, cubeScale);

                if (colliding && OVRInput.Get(activeButton))
                {
                    resetCube.SetActive(true);
                    waitingForReset = true;
                    colliding = false;
                    cFactory.destroyCurrentCube();
                }
            }

        }
    }


    private void UpdateControllerPosition() {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(activeController);
        Vector3 newPostion = new Vector3();

        if (controllerPosition.z > 0.1f)
            newPostion.z = Mathf.Min(200.0f, 100.0f + ((controllerPosition.z - 0.1f) * (250.0f * gain)));
        else
            newPostion.z = 100.0f;

        if (controllerPosition.y > 0.0f)
            newPostion.y = Mathf.Min(150.0f, 100.0f + (controllerPosition.y * (250.0f * gain)));
        else
            newPostion.y = Mathf.Max(50.0f, 100.0f + (controllerPosition.y * (250.0f * gain)));

        if (controllerPosition.x > 0.0f)
            newPostion.x = Mathf.Min(50.0f, controllerPosition.x * (250.0f * gain));
        else
            newPostion.x = Mathf.Max(-50.0f, controllerPosition.x * (250.0f * gain));

        transform.position = newPostion;
    }

    public void SetActiveController(string hand) {
        if (hand == "left") {
            activeController = OVRInput.Controller.LTouch;
        }
        else {
            activeController = OVRInput.Controller.RTouch;
            activeButton = OVRInput.Button.One;
        }
    }

    public void SetGain(float g) {
        gain = g;
    }

    public void SetPlayerSize(float x, float y, float z) {
        transform.localScale = new Vector3(x, y, z);
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == "ResetCube")
        {
            resetting = true;
            other.gameObject.GetComponent<Renderer>().material = resetSelected;
        }
        else
        {
            cFactory.SetSelectedMaterial(other.gameObject);
            colliding = true;
            numOfCollisions++;
        }

    }

    void OnTriggerExit(Collider other) {
        if (other.name == "ResetCube")
        {
            resetting = false;
            other.gameObject.GetComponent<Renderer>().material = resetUnselected;
        }
        else
        {
            cFactory.SetUnselectedMaterial(other.gameObject);
            colliding = false;
        }
    }
}
