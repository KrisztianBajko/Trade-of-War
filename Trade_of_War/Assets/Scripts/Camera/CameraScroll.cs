using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float minFOV;
    public float maxFOV;
    public float zoomSpeed;

    private Camera cam;
    private float camFOV;
    private float moseScrollInput;
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
        camFOV = Mathf.Clamp(camFOV, minFOV, maxFOV);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFOV, zoomSpeed);
    }
}
