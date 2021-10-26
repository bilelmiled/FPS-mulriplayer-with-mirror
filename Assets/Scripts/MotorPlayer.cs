using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotorPlayer : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 rotation;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 velocity;

    [SerializeField]
    private float camLimit = 85f;

    private Rigidbody rb;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        PerformMovement();
        PerformRotation();
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -camLimit, camLimit);
        //cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        cam.transform.localRotation = Quaternion.Euler(currentCameraRotationX, 0, 0);
    }
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(velocity * Time.fixedDeltaTime);
        }
    }
}
