using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float speed = 0.125f;
    public Vector3 offset;
    public float minFOV = 20f;
    public float maxFOV = 60f;
    public float sensitivity = 10f;
    public float FOV;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;

        cameraZoom();
    }
    private void cameraZoom()
    {
        FOV = Camera.main.fieldOfView;
        FOV += (Input.GetAxis("Mouse ScrollWheel") * sensitivity) * -1;
        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
        Camera.main.fieldOfView = FOV;
    }
}
