using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player’s transform
    public Vector3 offset; // Offset from the player’s position
    public float smoothSpeed = 0.125f; // Speed of camera following

    public float mouseSensitivity = 100f;
    public float verticalRotationLimit = 80f;

    private float xRotation = 0f;

    public float distanceFromPlayer = 2f; // Adjust this value as needed

    bool isCamRotation = false;

    private void Start()
    {
        isCamRotation = false;
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            isCamRotation = !isCamRotation;
        }
    }

    void Update()
    {
        if(isCamRotation)
        {
            // Get mouse input for vertical rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            // Adjust the vertical rotation
            xRotation -= mouseX;
            xRotation = Mathf.Clamp(-verticalRotationLimit, xRotation, verticalRotationLimit);

            // Apply rotation to the camera
            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);

            // Maintain the camera's position relative to the player
            Vector3 offset = new Vector3(0f, 0f, -distanceFromPlayer);
            transform.position = player.position + transform.rotation * offset;

            // Make sure the camera looks at the player
            transform.LookAt(player);
        }
        
    }
}
