using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraHeight;

    void Update()
    {
        Vector3 pos = player.transform.position;
        pos += cameraHeight;
        transform.position = pos;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit))
        {
            if (hit.transform == player)
            {
                // In Range and i can see you!
            }
        }

        Vector3 targetDirection = player.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = 1 * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
