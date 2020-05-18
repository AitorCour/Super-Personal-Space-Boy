using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Enemy : EnemyBehaviour
{
    public LayerMask mask;
    public float acceleration;
    public float speedRot;
    private float distance;
    private Vector3 destination;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent.acceleration = acceleration;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * radius;
        Gizmos.DrawRay(transform.position, direction); //forward
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 direction = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, direction, out hit, radius, mask) && !detected)
        {
            //Debug.Log(hit.transform.name);
            //Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 10.0f);
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                Debug.Log("Player detected");
                destination = player.transform.position;
                detected = true;
            }
        }

        distance = Vector3.Distance(destination, transform.position);

        if (detected)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
        }
        else
        {
            agent.isStopped = true;
        }
        if(distance <= attackDistance)
        {
            agent.isStopped = true;
            Debug.Log("destination achieved");

            float speedRot = Time.deltaTime * speed;
            transform.Rotate(Vector3.up * speedRot, Space.World);
            detected = false;
        }
    }
}
