using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Vars")]
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    //[SerializeField] private float lookSpeed = 2.0f;
    //[SerializeField] private float lookXLimit = 45.0f;

    MainManager mainManager;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    //float rotationX = 0;

    [Header("Camera Vars")]
    [SerializeField] private float mouseSensitivity = 1.5f;
    [Tooltip("Minimum Angle X that cam can move")]
    [SerializeField] private float minimum_X = -30f;
    [Tooltip("Maximum Angle X that cam can move")]
    [SerializeField] private float maximum_X = 30f;
    private float rotation_X, rotation_Y;


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySounds("VinylCrackle");
        AudioManager.instance.PlaySounds("Atmosphere1");
        characterController = GetComponent<CharacterController>();
        mainManager = MainManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        HandleRotation();
    }

    void PlayerMovement()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = MainManager.instance.canPlayerMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = MainManager.instance.canPlayerMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && MainManager.instance.canPlayerMove && characterController.isGrounded)
        {
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

        // Move the controller
        if (MainManager.instance.canPlayerMove) characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        /*if (MainManager.instance.canPlayerMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }*/
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotation_X += mouseY * mouseSensitivity;
        rotation_Y += mouseX * mouseSensitivity;

        rotation_X = ClampAngle(rotation_X, minimum_X, maximum_X);

        Quaternion rotation = Quaternion.Euler(rotation_X, rotation_Y, 0);
        if (MainManager.instance.canPlayerMove) transform.rotation = rotation;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;   //if it goes below add 360  
        }

        if (angle > 360f)
        {
            angle -= 360f;  //if it goes above 360 subtract 
        }

        return Mathf.Clamp(angle, min, max);
    }
}
