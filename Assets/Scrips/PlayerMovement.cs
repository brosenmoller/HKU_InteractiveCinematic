using UnityEngine;

/* Base of the PlayerController is from https://sharpcoderblog.com/blog/unity-3d-fps-controller.
 * Edited for use in this project. */

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool canMove = true;
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSpeed = 0.2f;
    [SerializeField] private float lookXLimit = 45.0f;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public void SetCanMove(bool value) => canMove = value;

    public bool IsMoving { private set; get; } = false;
    public bool IsSprinting { private set; get; } = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {


        HorizontalMovement();
        UpdateCamera();
    }

    private void HorizontalMovement()
    {
        if (!canMove) { return; }

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 inputDirection = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (inputDirection != Vector2.zero) { IsMoving = true; }
        else { IsMoving = false; }

        IsSprinting = Input.GetKeyDown(KeyCode.LeftShift);
        float curSpeedX = (IsSprinting ? runningSpeed : walkingSpeed) * inputDirection.y;
        float curSpeedY = (IsSprinting ? runningSpeed : walkingSpeed) * inputDirection.x;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            //jumpSound.Play();
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void UpdateCamera()
    {
        if (!canMove) { return; }

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    public void WarpPlayer(Vector3 warpLocation)
    {
        transform.position = warpLocation;
        Physics.SyncTransforms();
    }
}