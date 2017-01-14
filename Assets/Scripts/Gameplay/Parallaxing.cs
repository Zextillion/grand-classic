using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;     // Array (list) of all the back- and foregrounds to be parallaxed
    private float[] parallaxScales;     // The proportion of the camera's movement to move the backgrounds by
    [SerializeField]
    float smoothing = 1f;        // How smooth the parallax is going to be. Make sure to set this above 0.
    [SerializeField]
    float verticalScrollSpeed = 1f;

    private Transform cam;              // reference to the main camera's transform
    private Vector3 previousCamPos;     // the position of the camera in the previous frame

    private GameObject player;

    // Is called before Start (). Great for references.
    void Awake()
    {   // set up camera reference
        cam = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

	// Use this for initialization
	void Start ()
    {   // The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        // assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {   // for each background
        if (player != null)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {   
                // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
                float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];

                // set a target x position which is the current position plus the parallax
                float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;

                // VerticalScrolling
                float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i] * verticalScrollSpeed;

                float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

                // create a target position which is the background's current position with its target x position
                Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

                // fade between current position and the target position using lerp
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            }

            // set the previousCamPos to the camera's position at the end of the frame.
            previousCamPos = cam.position;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}
}
