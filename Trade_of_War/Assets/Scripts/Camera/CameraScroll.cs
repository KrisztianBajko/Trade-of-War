using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Camera cam;
    private float camFOV;
    public float zoomSpeed;

    private float moseScrollInput;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camFOV = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        moseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        camFOV -= moseScrollInput * zoomSpeed;
        camFOV = Mathf.Clamp(camFOV, 20, 60);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFOV, zoomSpeed);
    }
}
