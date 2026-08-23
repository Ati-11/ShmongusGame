using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float horizontalInput = 1f;
    public float verticalInput = 1f;
    private float xRotation = 0f;
    public float mouseSensitivity = 1.0f;
    public float walkSpeed = 5.0f;
    public Transform playerCamera;

    // Crouch variables
    public float crouchheight = 0.2f;
    public float standheight = 0.35f;
    public float crouchspeed = 5f;
    public float crouchwalkspeed = 0.5f;

    private float currentSpeed;
    private bool isCrouching = false;


    private Rigidbody body;
    private CapsuleCollider capsule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        // Camera Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        // Crouch Adjustments
        if (isCrouching)
        {
            currentSpeed = crouchwalkspeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        float targetHeight = isCrouching ? crouchheight : standheight;
        Vector3 localpos = playerCamera.localPosition;
        localpos.y = Mathf.Lerp(localpos.y, targetHeight, Time.deltaTime * crouchspeed);
        playerCamera.localPosition = localpos;

        // Collider Crouching
        float targetColliderHeight = isCrouching ? 0.5f : 1f;
        float targetColliderCenterY = isCrouching ? 0.35f : 0.1f;

        capsule.height = Mathf.Lerp(capsule.height, targetColliderHeight, Time.deltaTime * crouchspeed);
        capsule.center = new Vector3(0, Mathf.Lerp(capsule.center.y, targetColliderCenterY, Time.deltaTime * crouchspeed), 0);
    }
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal") * 1;
        verticalInput = Input.GetAxis("Vertical") * 1;

        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput);
        move = transform.TransformDirection(move);
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }
}
