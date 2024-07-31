using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves
    private Vector3 targetPosition; // The point to which the player will move (from mouse click)
    private bool isMovingToTarget; // Flag to check if the player is moving to a clicked position
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private Animator playerAnimator;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag=="ground")
                {
                    // Set the target position to the hit point
                    targetPosition = hit.point;
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
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the player to face the target position
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Stop moving to the target position when close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
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
}
