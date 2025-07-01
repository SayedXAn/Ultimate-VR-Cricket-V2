using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;                                // For device velocity
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallGenerator : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty gripAction;          // Action bound to �right-grip (pressed)�

    [Header("References")]
    public Transform rightController;               // Drag the right-hand controller transform here
    public GameObject ballPrefab;                   // Prefab that already has XR Grab Interactable + Rigidbody

    [Header("Tuning")]
    public bool applyAngularVelocity = true;        // Toggle if you want spin

    GameObject currentBall;
    Rigidbody currentRB;

    Vector3 lastPos;
    Vector3 lastRot;
    public XRControllerRecorder xrController;
    Vector3 previousPosition;
    Quaternion previousRotation;
    Vector3 currentVelocity;
    Vector3 currentAngularVelocity;
    public float ballForce = 5.0f;

    void Start()
    {
        // Make sure the action is enabled (important in Action-based XR rigs)
        gripAction.action.Enable();
        previousPosition = rightController.position;
        previousRotation = rightController.rotation;

        lastPos = rightController.position;
        lastRot = rightController.eulerAngles;
    }

    void Update()
    {
        Vector3 currentPosition = rightController.position;
        Quaternion currentRotation = rightController.rotation;

        currentVelocity = (currentPosition - previousPosition) / Time.deltaTime;
        currentAngularVelocity = (currentRotation.eulerAngles - previousRotation.eulerAngles) / Time.deltaTime;

        previousPosition = currentPosition;
        previousRotation = currentRotation;

        bool isPressed = gripAction.action.IsPressed();

        // 1. Pressed ? generate/attach
        if (isPressed && currentBall == null)
            GrabNewBall();

        // 2. Released ? let go / throw
        if (!isPressed && currentBall != null)
            ReleaseBall();

        // Track deltas each frame so we can compute velocity manually as a fallback
        lastPos = rightController.position;
        lastRot = rightController.eulerAngles;
    }

    void GrabNewBall()
    {
        currentBall = Instantiate(ballPrefab,
                                   rightController.position,
                                   rightController.rotation,
                                   rightController);           // parent to controller

        currentRB = currentBall.GetComponent<Rigidbody>();
        if (currentRB) currentRB.isKinematic = true;
    }

    void ReleaseBall()
    {
        currentBall.transform.parent = null;
        if (currentRB == null) currentRB = currentBall.GetComponent<Rigidbody>();
        currentRB.isKinematic = false;

        currentRB.linearVelocity = currentVelocity * ballForce;
        currentRB.angularVelocity = currentAngularVelocity;
        currentRB.WakeUp();

        currentBall = null;
        currentRB = null;
    }

}
