using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves
    public float proximityDistance = 1f; // Distance to move near the target object
    private Vector3 targetPosition; // The point to which the player will move
    private bool isMovingToTarget; // Flag to check if the player is moving to a clicked position
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private Animator playerAnimator;
    public GameObject LookTarget;

    //for cam follow
    public Vector3 offset; // Offset from the player’s position
    public float smoothSpeed = 0.125f; // Speed of camera following
    public GameObject MainCam;
    

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Initialize targetPosition to the player's current position
        targetPosition = transform.position;
        isMovingToTarget = false;
    }

    private void Update()
    {
        // Handle mouse click movement
        if (Input.GetMouseButtonDown(0))
        {
            // Check for clicked objects with the "movetarget" tag
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.transform.CompareTag("movetarget"))
                {
                    // Set the target position to a point near the hit object's position
                    Vector3 hitPosition = hit.point;
                    Vector3 direction = (hit.transform.position - hitPosition).normalized;
                    targetPosition = new Vector3(hitPosition.x + direction.x * proximityDistance, transform.position.y, hitPosition.z + direction.z * proximityDistance);
                    isMovingToTarget = true;
                }
            }
        }

        // Handle keyboard movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 keyboardMovement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (keyboardMovement.magnitude > 0)
        {
            // Move the player using keyboard input
            transform.Translate(keyboardMovement * moveSpeed * Time.deltaTime, Space.World);
            isMovingToTarget = false; // Stop moving to the target position

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(keyboardMovement.normalized, Vector3.up);

            // Rotate the player towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            playerAnimator.SetBool(IsWalkingHash, true);
        }
        else if (isMovingToTarget)
        {
            // Move the player towards the target position
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Ensure movement is only on the XZ plane

            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the player to face the target position
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Stop moving to the target position when close enough
            if (Vector3.Distance(new Vector3(transform.position.x, targetPosition.y, transform.position.z), targetPosition) < 0.75f)
            {
                isMovingToTarget = false;
            }
            playerAnimator.SetBool(IsWalkingHash, true);
        }
        else
        {
            playerAnimator.SetBool(IsWalkingHash, false);
        }

    }


    //for camera movement
    private void LateUpdate()
    {
        // Calculate the desired position with the offset
        Vector3 desiredPosition = LookTarget.transform.position + offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(MainCam.transform.position, desiredPosition, smoothSpeed);

        // Update camera position
        MainCam.transform.position = smoothedPosition;

        // Optionally, you can make the camera look at the player
        MainCam.transform.LookAt(LookTarget.transform);

    }
}
