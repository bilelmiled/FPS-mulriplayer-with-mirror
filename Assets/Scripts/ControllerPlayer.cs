using UnityEngine;

[RequireComponent(typeof(MotorPlayer))]
public class ControllerPlayer : MonoBehaviour
{
    public CharacterController control;

    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpForce = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;

    [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private Transform ground;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask groundMask;

    bool isGrounded;
    Vector3 velocity;

    private MotorPlayer motor;

    void Start()
    {
        motor = GetComponent<MotorPlayer>();
    }
    void Update()
    {
        if(PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


        isGrounded = Physics.CheckSphere(ground.position, groundRadius, groundMask);

        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }

        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xMov + transform.forward * zMov;

        control.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        control.Move(velocity * Time.deltaTime);

        float yRot = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivityX;

        motor.Rotate(rotation);

        float xRot = Input.GetAxis("Mouse Y");

        float cameraRotationX = xRot * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);
    }
}
