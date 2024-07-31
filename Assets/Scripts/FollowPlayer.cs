using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player’s transform
    public Vector3 offset; // Offset from the player’s position
    public float smoothSpeed = 0.125f; // Speed of camera following

    private void LateUpdate()
    {
        // Calculate the desired position with the offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update camera position
        transform.position = smoothedPosition;

        // Optionally, you can make the camera look at the player
        // transform.LookAt(player);
    }
}
