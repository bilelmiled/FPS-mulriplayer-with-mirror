using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    void Update()
    {
        Camera cam = Camera.main;
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
