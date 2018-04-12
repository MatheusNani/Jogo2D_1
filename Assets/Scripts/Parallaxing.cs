using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform [] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;

    private Transform cam;
    private Vector3 previosCamPos;

    void Awake()
    {
        cam = Camera.main.transform; // MAIN Camera reference

    }

    // Use this for initialization
    void Start () {
        // previosFrame
        previosCamPos = cam.position;

        // asigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		// for each background
        for(int i = 0; i < backgrounds.Length; i++)
        {
            // the parallax is the oposite of the camera movement because the previous frame multiplied by the scale.
            float parallax = (previosCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current position plus the parallax.
            float backgroundTargePosX = backgrounds[i].position.x + parallax;

            // creat a target position which is the background current position with  it's target position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargePosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //set previousCamPos to the camera's position at the end of the frame
        previosCamPos = cam.position;

	}
}
